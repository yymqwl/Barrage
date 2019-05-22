using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Microsoft.IO;

#if SERVER
using System.Runtime.InteropServices;
#endif

namespace GameFramework
{
    public static class KcpProtocalType
    {
        public const byte SYN = 1;
        public const byte ACK = 2;
        public const byte FIN = 3;
        public const byte MSG = 4;
    }

    public sealed class KService : AService
    {
        public static KService Instance { get; private set; }

        private uint m_IdGenerater = NetWorkConstant.KService_IdStart;//1000;

        // KService创建的时间
        public long m_StartTime;
        // 当前时间 - KService创建的时间
        public uint TimeNow { get; private set; }

        private Socket m_Socket;

        private readonly Dictionary<long, KChannel> m_LocalConnChannels = new Dictionary<long, KChannel>();

        private readonly byte[] m_Cache = new byte[NetWorkConstant.KService_CacheLen];

        private readonly Queue<long> m_RemovedChannels = new Queue<long>();

        public RecyclableMemoryStreamManager MemoryStreamManager = new RecyclableMemoryStreamManager();
        #region 连接相关
        // 记录等待连接的channel，10秒后或者第一个消息过来才会从这个dict中删除
        private readonly Dictionary<uint, KChannel> m_WaitConnectChannels = new Dictionary<uint, KChannel>();
        #endregion

        #region 定时器相关
        // 下帧要更新的channel
        private readonly HashSet<long> m_UpdateChannels = new HashSet<long>();
        // 下次时间更新的channel
        private readonly MultiMap<long, long> m_TimeId = new MultiMap<long, long>();
        private readonly List<long> m_TimeOutTime = new List<long>();
        // 记录最小时间，不用每次都去MultiMap取第一个值
        private long m_MinTime;
        #endregion


        private EndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, 0);

        public KService(IPEndPoint ipEndPoint, Action<AChannel> acceptCallback)
        {
            this.AcceptCallback += acceptCallback;

            this.m_StartTime = TimeHelper.ClientNow();
            this.TimeNow = (uint)(TimeHelper.ClientNow() - this.m_StartTime);
            this.m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //this.m_Socket.Blocking = false;
            this.m_Socket.Bind(ipEndPoint);
#if SERVER
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                const uint IOC_IN = 0x80000000;
                const uint IOC_VENDOR = 0x18000000;
                uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                this.m_Socket.IOControl((int)SIO_UDP_CONNRESET, new[] { Convert.ToByte(false) }, null);
            }
#endif
            Instance = this;
        }

        public KService()
        {
            this.m_StartTime = TimeHelper.ClientNow();
            this.TimeNow = (uint)(TimeHelper.ClientNow() - this.m_StartTime);
            this.m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //this.m_Socket.Blocking = false;
            this.m_Socket.Bind(new IPEndPoint(IPAddress.Any, 0));
#if SERVER
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                const uint IOC_IN = 0x80000000;
                const uint IOC_VENDOR = 0x18000000;
                uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
                this.m_Socket.IOControl((int)SIO_UDP_CONNRESET, new[] { Convert.ToByte(false) }, null);
            }
#endif
            Instance = this;
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (KeyValuePair<long, KChannel> keyValuePair in this.m_LocalConnChannels)
            {
                keyValuePair.Value.Dispose();
            }

            this.m_Socket.Close();
            this.m_Socket = null;
            Instance = null;
        }

        public void Recv()
        {
            if (this.m_Socket == null)
            {
                return;
            }

            while (m_Socket != null && this.m_Socket.Available > 0)
            {
                int messageLength = 0;
                try
                {
                    messageLength = this.m_Socket.ReceiveFrom(this.m_Cache, ref this.ipEndPoint);
                }
                catch (Exception e)
                {
                    Log.Error(e);
                    continue;
                }

                // 长度小于1，不是正常的消息
                if (messageLength < 1)
                {
                    continue;
                }
                // accept
                byte flag = this.m_Cache[0];

                // conn从1000开始，如果为1，2，3则是特殊包
                uint remoteConn = 0;
                uint localConn = 0;
                KChannel kChannel = null;
                switch (flag)
                {
                    case KcpProtocalType.SYN:  // accept
                                               // 长度!=5，不是accpet消息
                        if (messageLength != 5)
                        {
                            break;
                        }

                        IPEndPoint acceptIpEndPoint = (IPEndPoint)this.ipEndPoint;
                        this.ipEndPoint = new IPEndPoint(0, 0);

                        remoteConn = BitConverter.ToUInt32(this.m_Cache, 1);

                        // 如果已经收到连接，则忽略
                        if (this.m_WaitConnectChannels.TryGetValue(remoteConn, out kChannel))
                        {
                            break;
                        }

                        localConn = ++this.m_IdGenerater;
                        kChannel = new KChannel(localConn, remoteConn, this.m_Socket, acceptIpEndPoint, this);
                        this.m_LocalConnChannels[kChannel.LocalConn] = kChannel;
                        this.m_WaitConnectChannels[remoteConn] = kChannel;

                        this.OnAccept(kChannel);

                        break;
                    case KcpProtocalType.ACK:  // connect返回
                                               // 长度!=9，不是connect消息
                        if (messageLength != 9)
                        {
                            break;
                        }
                        remoteConn = BitConverter.ToUInt32(this.m_Cache, 1);
                        localConn = BitConverter.ToUInt32(this.m_Cache, 5);

                        kChannel = this.GetKChannel(localConn);
                        if (kChannel != null)
                        {
                            kChannel.HandleConnnect(remoteConn);
                        }
                        break;
                    case KcpProtocalType.FIN:  // 断开
                                               // 长度!=13，不是DisConnect消息
                        if (messageLength != 13)
                        {
                            break;
                        }

                        remoteConn = BitConverter.ToUInt32(this.m_Cache, 1);
                        localConn = BitConverter.ToUInt32(this.m_Cache, 5);

                        // 处理chanel
                        kChannel = this.GetKChannel(localConn);
                        if (kChannel != null)
                        {
                            // 校验remoteConn，防止第三方攻击
                            if (kChannel.RemoteConn == remoteConn)
                            {
                                kChannel.Disconnect(ErrorCode.ERR_PeerDisconnect);
                            }
                        }
                        break;
                    case KcpProtocalType.MSG:  // 断开
                                               // 长度<9，不是Msg消息
                        if (messageLength < 9)
                        {
                            break;
                        }
                        // 处理chanel
                        remoteConn = BitConverter.ToUInt32(this.m_Cache, 1);
                        localConn = BitConverter.ToUInt32(this.m_Cache, 5);

                        this.m_WaitConnectChannels.Remove(remoteConn);

                        kChannel = this.GetKChannel(localConn);
                        if (kChannel != null)
                        {
                            // 校验remoteConn，防止第三方攻击
                            if (kChannel.RemoteConn == remoteConn)
                            {
                                kChannel.HandleRecv(this.m_Cache, 5, messageLength - 5);
                            }
                        }
                        break;
                }
            }
        }

        public KChannel GetKChannel(long id)
        {
            AChannel aChannel = this.GetChannel(id);
            if (aChannel == null)
            {
                return null;
            }

            return (KChannel)aChannel;
        }

        public override AChannel GetChannel(long id)
        {
            if (this.m_RemovedChannels.Contains(id))
            {
                return null;
            }
            KChannel channel;
            this.m_LocalConnChannels.TryGetValue(id, out channel);
            return channel;
        }

        public static void Output(IntPtr bytes, int count, IntPtr user)
        {
            if (Instance == null)
            {
                return;
            }
            AChannel aChannel = Instance.GetChannel((uint)user);
            if (aChannel == null)
            {
                Log.Error($"not found kchannel, {(uint)user}");
                return;
            }

            KChannel kChannel = aChannel as KChannel;
            kChannel.Output(bytes, count);
        }

        public override AChannel ConnectChannel(IPEndPoint remoteEndPoint)
        {
            uint localConn = (uint)RandomHelper.RandomNumber(1000, int.MaxValue);
            KChannel oldChannel;
            if (this.m_LocalConnChannels.TryGetValue(localConn, out oldChannel))
            {
                this.m_LocalConnChannels.Remove(oldChannel.LocalConn);
                oldChannel.Dispose();
            }

            KChannel channel = new KChannel(localConn, this.m_Socket, remoteEndPoint, this);
            this.m_LocalConnChannels[channel.LocalConn] = channel;
            return channel;
        }

        public override AChannel ConnectChannel(string address)
        {
            IPEndPoint ipEndPoint2 = NetHelper.ToIPEndPoint(address);
            return this.ConnectChannel(ipEndPoint2);
        }

        public override void Remove(long id)
        {
            KChannel channel;
            if (!this.m_LocalConnChannels.TryGetValue(id, out channel))
            {
                return;
            }
            if (channel == null)
            {
                return;
            }
            this.m_RemovedChannels.Enqueue(id);

            // 删除channel时检查等待连接状态的字典是否要清除
            KChannel waitConnectChannel;
            if (this.m_WaitConnectChannels.TryGetValue(channel.RemoteConn, out waitConnectChannel))
            {
                if (waitConnectChannel.LocalConn == channel.LocalConn)
                {
                    this.m_WaitConnectChannels.Remove(channel.RemoteConn);
                }
            }
        }

#if !SERVER
		// 客户端channel很少,直接每帧update所有channel即可,这样可以消除TimerOut方法的gc
		public void AddToUpdateNextTime(long time, long id)
		{
		}
		
		public override void Update()
		{
			this.TimeNow = (uint) (TimeHelper.ClientNow() - this.m_StartTime);

			this.Recv();

			foreach (var kv in this.m_LocalConnChannels)
			{
				kv.Value.Update();
			}
			
			while (this.m_RemovedChannels.Count > 0)
			{
				long id = this.m_RemovedChannels.Dequeue();
				KChannel channel;
				if (!this.m_LocalConnChannels.TryGetValue(id, out channel))
				{
					continue;
				}
				this.m_LocalConnChannels.Remove(id);
				channel.Dispose();
			}
		}
#else
        // 服务端需要看channel的update时间是否已到
        public void AddToUpdateNextTime(long time, long id)
        {
            if (time == 0)
            {
                this.m_UpdateChannels.Add(id);
                return;
            }
            if (time < this.m_MinTime)
            {
                this.m_MinTime = time;
            }
            this.m_TimeId.Add(time, id);
        }

        public override void Update()
        {
            this.TimeNow = (uint)(TimeHelper.ClientNow() - this.m_StartTime);

            this.Recv();

            this.TimerOut();

            foreach (long id in m_UpdateChannels)
            {
                KChannel kChannel = this.GetKChannel(id);
                if (kChannel == null)
                {
                    continue;
                }
                if (kChannel.Id == 0)
                {
                    continue;
                }
                kChannel.Update();
            }
            this.m_UpdateChannels.Clear();

            while (this.m_RemovedChannels.Count > 0)
            {
                long id = this.m_RemovedChannels.Dequeue();
                KChannel channel;
                if (!this.m_LocalConnChannels.TryGetValue(id, out channel))
                {
                    continue;
                }
                this.m_LocalConnChannels.Remove(id);
                channel.Dispose();
            }
        }

        // 计算到期需要update的channel
        private void TimerOut()
        {
            if (this.m_TimeId.Count == 0)
            {
                return;
            }

            uint timeNow = this.TimeNow;

            if (timeNow < this.m_MinTime)
            {
                return;
            }
            this.m_TimeOutTime.Clear();

            foreach (KeyValuePair<long, List<long>> kv in this.m_TimeId.GetDictionary())
            {
                long k = kv.Key;
                if (k > timeNow)
                {
                    m_MinTime = k;
                    break;
                }
                this.m_TimeOutTime.Add(k);
            }
            foreach (long k in this.m_TimeOutTime)
            {
                foreach (long v in this.m_TimeId[k])
                {
                    this.m_UpdateChannels.Add(v);
                }

                this.m_TimeId.Remove(k);
            }
        }
#endif
    }
}