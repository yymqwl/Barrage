using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.IO;

namespace GameFramework
{
    public sealed class TService : AService
    {
        private readonly Dictionary<long, TChannel> m_IdChannels = new Dictionary<long, TChannel>();

        private readonly SocketAsyncEventArgs m_InnArgs = new SocketAsyncEventArgs();
        private Socket m_Acceptor;

        public RecyclableMemoryStreamManager MemoryStreamManager = new RecyclableMemoryStreamManager();

        public HashSet<long> m_NeedStartSendChannel = new HashSet<long>();

        public HashSet<TChannel> m_OutTimeChannel = new HashSet<TChannel>();
        public int PacketSizeLength { get; }


        UpdateTime m_UpdateTime;
        /// <summary>
        /// 即可做client也可做server
        /// </summary>
        public TService(int packetSizeLength, IPEndPoint ipEndPoint, Action<AChannel> acceptCallback) : base()
        {
            this.PacketSizeLength = packetSizeLength;
            m_UpdateTime = new UpdateTime(NetWorkConstant.Tcp_CheckIdleInternal);
            m_UpdateTime.Evt_Act += CheckTimeOut;
            this.AcceptCallback += acceptCallback;

            this.m_Acceptor = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.m_Acceptor.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.m_InnArgs.Completed += this.OnComplete;

            this.m_Acceptor.Bind(ipEndPoint);
            this.m_Acceptor.Listen(NetWorkConstant.Tcp_ListenLen);
            
            this.AcceptAsync();
        }

        public TService(int packetSizeLength)
        {
            this.PacketSizeLength = packetSizeLength;
            m_UpdateTime = new UpdateTime(NetWorkConstant.Tcp_CheckIdleInternal);
            m_UpdateTime.Evt_Act += CheckTimeOut;
        }

        public override void Dispose()
        {

            base.Dispose();

            foreach (long id in this.m_IdChannels.Keys.ToArray())
            {
                TChannel channel = this.m_IdChannels[id];
                channel.Dispose();
            }
            this.m_Acceptor?.Close();
            this.m_Acceptor = null;
            this.m_InnArgs.Dispose();
            m_UpdateTime.Dispose();
        }

        private void OnComplete(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Accept:
                    OneThreadSynchronizationContext.Instance.Post(this.OnAcceptComplete, e);
                    break;
                default:
                    throw new Exception($"socket accept error: {e.LastOperation}");
            }
        }

        public void AcceptAsync()
        {
            this.m_InnArgs.AcceptSocket = null;
            if (this.m_Acceptor.AcceptAsync(this.m_InnArgs))
            {
                return;
            }
            OnAcceptComplete(this.m_InnArgs);
        }

        private void OnAcceptComplete(object o)
        {
            if (this.m_Acceptor == null)
            {
                return;
            }
            SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;

            if (e.SocketError != SocketError.Success)
            {
                Log.Error($"accept error {e.SocketError}");
                this.AcceptAsync();
                return;
            }
            TChannel channel = new TChannel(e.AcceptSocket, this);
            this.m_IdChannels[channel.Id] = channel;

            try
            {
                this.OnAccept(channel);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            if (this.m_Acceptor == null)
            {
                return;
            }

            this.AcceptAsync();
        }

        public override AChannel GetChannel(long id)
        {
            TChannel channel = null;
            this.m_IdChannels.TryGetValue(id, out channel);
            return channel;
        }

        public override AChannel ConnectChannel(IPEndPoint ipEndPoint)
        {
            TChannel channel = new TChannel(ipEndPoint, this);
            this.m_IdChannels[channel.Id] = channel;

            return channel;
        }

        public override AChannel ConnectChannel(string address)
        {
            IPEndPoint ipEndPoint = NetHelper.ToIPEndPoint(address);
            return this.ConnectChannel(ipEndPoint);
        }

        public void MarkNeedStartSend(long id)
        {
            this.m_NeedStartSendChannel.Add(id);
        }

        public override void Remove(long id)
        {
            TChannel channel;
            if (!this.m_IdChannels.TryGetValue(id, out channel))
            {
                return;
            }
            if (channel == null)
            {
                return;
            }
            this.m_IdChannels.Remove(id);
            channel.Dispose();
        }

        public override void Update()
        {
            base.Update();

            foreach (long id in this.m_NeedStartSendChannel)
            {
                TChannel channel;
                if (!this.m_IdChannels.TryGetValue(id, out channel))
                {
                    continue;
                }

                if (channel.IsSending)
                {
                    continue;
                }

                try
                {
                    channel.StartSend();
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }

            this.m_NeedStartSendChannel.Clear();
            
            m_UpdateTime.Update(ClientTimer.Instance.DeltaTime);

        }
        public void CheckTimeOut()
        {
            m_OutTimeChannel.Clear();
            m_IdChannels.Dict_Foreach((long id,TChannel tc)=>
            {
                if(TimeNow- tc.LastRecvTime >= NetWorkConstant.Tcp_TimeOut)
                {
                    m_OutTimeChannel.Add(tc);
                }
            });
            foreach (TChannel tc in m_OutTimeChannel)
            {
                tc.DisConnect();
            }
            //Log.Debug("CheckTimeOut");
        }
    }
}