using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Microsoft.IO;


namespace GameFramework
{
    /// <summary>
    /// 封装Socket,将回调push到主线程处理
    /// </summary>
    public sealed class TChannel : AChannel
    {
        private Socket m_Socket;
        private SocketAsyncEventArgs m_InnArgs = new SocketAsyncEventArgs();
        private SocketAsyncEventArgs m_OutArgs = new SocketAsyncEventArgs();

        private readonly CircularBuffer m_RecvBuffer = new CircularBuffer();
        private readonly CircularBuffer m_SendBuffer = new CircularBuffer();

        private readonly MemoryStream m_MemoryStream;

        private bool m_IsSending;

        private bool m_IsRecving;

        private readonly PacketParser m_Parser;

        private readonly byte[] m_PacketSizeCache;

        public TChannel(IPEndPoint ipEndPoint, TService service) : base(service, ChannelType.Connect)
        {
            int packetSize = this.GetService().PacketSizeLength;
            this.m_PacketSizeCache = new byte[packetSize];
            this.m_MemoryStream = this.GetService().MemoryStreamManager.GetStream(NetWorkConstant.Str_Msg, ushort.MaxValue);

            this.m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.m_Socket.NoDelay = true;
            this.m_Parser = new PacketParser(packetSize, this.m_RecvBuffer, this.m_MemoryStream);
            this.m_InnArgs.Completed += this.OnComplete;
            this.m_OutArgs.Completed += this.OnComplete;

            this.RemoteAddress = ipEndPoint;

            this.m_IsSending = false;
            m_ChannelState = ChannelState.EDisConnected;
            m_LastRecvTime = GetService().TimeNow;
            Connect();
        }

        public TChannel(Socket m_Socket, TService service) : base(service, ChannelType.Accept)
        {
            int packetSize = this.GetService().PacketSizeLength;
            this.m_PacketSizeCache = new byte[packetSize];
            this.m_MemoryStream = this.GetService().MemoryStreamManager.GetStream(NetWorkConstant.Str_Msg, ushort.MaxValue);

            this.m_Socket = m_Socket;
            this.m_Socket.NoDelay = true;
            this.m_Parser = new PacketParser(packetSize, this.m_RecvBuffer, this.m_MemoryStream);
            this.m_InnArgs.Completed += this.OnComplete;
            this.m_OutArgs.Completed += this.OnComplete;

            this.RemoteAddress = (IPEndPoint)m_Socket.RemoteEndPoint;

            //this.m_IsConnected = true;
            this.m_IsSending = false;
            m_ChannelState = ChannelState.EConnected;
            m_LastRecvTime = GetService().TimeNow;
            Accept();
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.m_Socket.Close();
            this.m_InnArgs.Dispose();
            this.m_OutArgs.Dispose();
            this.m_InnArgs = null;
            this.m_OutArgs = null;
            this.m_Socket = null;
            this.m_MemoryStream.Dispose();
        }

        private TService GetService()
        {
            return (TService)this.Service;
        }

        public override MemoryStream Stream
        {
            get
            {
                return this.m_MemoryStream;
            }
        }


        protected override void Connect()
        {
            base.Connect();
            if (m_ChannelState == ChannelState.EDisConnected)
            {
                this.ConnectAsync(this.RemoteAddress);
                return;
            }
        }
        protected override void Accept()
        {
            base.Accept();
            if (!this.m_IsRecving)
            {
                this.m_IsRecving = true;
                this.StartRecv();
            }
            this.GetService().MarkNeedStartSend(this.Id);
        }
        /*
        public override void Start()
        {
            if (!this.m_IsConnected)
            {
                this.ConnectAsync(this.RemoteAddress);
                return;
            }

            if (!this.m_IsRecving)
            {
                this.m_IsRecving = true;
                this.StartRecv();
            }

            this.GetService().MarkNeedStartSend(this.Id);
        }*/

        public override void Send(MemoryStream stream)
        {
            if (this.IsDisposed)
            {
                throw new Exception("TChannel已经被Dispose, 不能发送消息");
            }

            this.m_LastRecvTime = GetService().TimeNow;
            switch (this.GetService().PacketSizeLength)
            {
                case Packet.PacketSizeLength4:
                    if (stream.Length > ushort.MaxValue * 16)
                    {
                        throw new Exception($"send packet too large: {stream.Length}");
                    }
                    this.m_PacketSizeCache.WriteTo(0, (int)stream.Length);
                    break;
                case Packet.PacketSizeLength2:
                    if (stream.Length > ushort.MaxValue)
                    {
                        throw new Exception($"send packet too large: {stream.Length}");
                    }
                    this.m_PacketSizeCache.WriteTo(0, (ushort)stream.Length);
                    break;
                default:
                    throw new Exception("packet size must be 2 or 4!");
            }

            this.m_SendBuffer.Write(this.m_PacketSizeCache, 0, this.m_PacketSizeCache.Length);
            this.m_SendBuffer.Write(stream);

            this.GetService().MarkNeedStartSend(this.Id);
        }

        private void OnComplete(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    OneThreadSynchronizationContext.Instance.Post(this.OnConnectComplete, e);
                    break;
                case SocketAsyncOperation.Receive:
                    OneThreadSynchronizationContext.Instance.Post(this.OnRecvComplete, e);
                    break;
                case SocketAsyncOperation.Send:
                    OneThreadSynchronizationContext.Instance.Post(this.OnSendComplete, e);
                    break;
                case SocketAsyncOperation.Disconnect:
                    OneThreadSynchronizationContext.Instance.Post(this.OnDisconnectComplete, e);
                    break;
                default:
                    throw new Exception($"m_Socket error: {e.LastOperation}");
            }
        }

        private void ConnectAsync(IPEndPoint ipEndPoint)
        {
            m_ChannelState = ChannelState.EConnecting;
            this.m_OutArgs.RemoteEndPoint = ipEndPoint;
            if (this.m_Socket.ConnectAsync(this.m_OutArgs))
            {
                return;
            }
            OnConnectComplete(this.m_OutArgs);
        }

        private void OnConnectComplete(object o)
        {
            if (this.m_Socket == null)
            {
                return;
            }
            SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;

            if (e.SocketError != SocketError.Success)
            {
                this.OnError((int)e.SocketError);
                return;
            }

            e.RemoteEndPoint = null;
            m_ChannelState = ChannelState.EConnected;

            //接收线程
            if (!this.m_IsRecving)
            {
                this.m_IsRecving = true;
                this.StartRecv();
            }
            this.GetService().MarkNeedStartSend(this.Id);

        }
        public override void DisConnect()
        {
            if(IsDisposed)
            {
                return;
            }
            if(m_ChannelState != ChannelState.EDisConnected)
            {
                m_Socket.Disconnect(false);
                m_ChannelState = ChannelState.EDisConnected;
                OnError(ErrorCode.ERR_SelfDisconnect);
                GetService().OnDisConnected(this);
            }
        }

        private void OnDisconnectComplete(object o)
        {
            m_ChannelState = ChannelState.EDisConnected;
            SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;
            this.OnError((int)e.SocketError);
        }

        private void StartRecv()
        {
            int size = this.m_RecvBuffer.ChunkSize - this.m_RecvBuffer.LastIndex;
            this.RecvAsync(this.m_RecvBuffer.Last, this.m_RecvBuffer.LastIndex, size);
        }

        public void RecvAsync(byte[] buffer, int offset, int count)
        {
            try
            {
                this.m_InnArgs.SetBuffer(buffer, offset, count);
            }
            catch (Exception e)
            {
                throw new Exception($"m_Socket set buffer error: {buffer.Length}, {offset}, {count}", e);
            }

            if (this.m_Socket.ReceiveAsync(this.m_InnArgs))
            {
                return;
            }
            OnRecvComplete(this.m_InnArgs);
        }

        private void OnRecvComplete(object o)
        {
            if (this.m_Socket == null)
            {
                return;
            }
            SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;

            if (e.SocketError != SocketError.Success)
            {
                this.OnError((int)e.SocketError);
                DisConnect();
                return;
            }

            if (e.BytesTransferred == 0)
            {
                this.OnError(ErrorCode.ERR_PeerDisconnect);
                DisConnect();
                return;
            }

            this.m_RecvBuffer.LastIndex += e.BytesTransferred;
            if (this.m_RecvBuffer.LastIndex == this.m_RecvBuffer.ChunkSize)
            {
                this.m_RecvBuffer.AddLast();
                this.m_RecvBuffer.LastIndex = 0;
            }
            m_LastRecvTime = GetService().TimeNow;//激活时间
            // 收到消息回调
            while (true)
            {
                try
                {
                    if (!this.m_Parser.Parse())
                    {
                        break;
                    }
                }
                catch (Exception ee)
                {
                    Log.Error(ee);
                    this.OnError(ErrorCode.ERR_SocketError);
                    DisConnect();
                    return;
                }

                try
                {
                    this.OnRead(this,this.m_Parser.GetPacket());
                }
                catch (Exception ee)
                {
                    Log.Error(ee);
                }
            }

            if (this.m_Socket == null)
            {
                return;
            }

            this.StartRecv();
        }

        public bool IsSending => this.m_IsSending;

        public void StartSend()
        {
            if (!this.IsConnected)
            {
                return;
            }

            // 没有数据需要发送
            if (this.m_SendBuffer.Length == 0)
            {
                this.m_IsSending = false;
                return;
            }

            this.m_IsSending = true;

            int sendSize = this.m_SendBuffer.ChunkSize - this.m_SendBuffer.FirstIndex;
            if (sendSize > this.m_SendBuffer.Length)
            {
                sendSize = (int)this.m_SendBuffer.Length;
            }

            this.SendAsync(this.m_SendBuffer.First, this.m_SendBuffer.FirstIndex, sendSize);
        }

        public void SendAsync(byte[] buffer, int offset, int count)
        {
            try
            {
                this.m_OutArgs.SetBuffer(buffer, offset, count);
            }
            catch (Exception e)
            {
                throw new Exception($"m_Socket set buffer error: {buffer.Length}, {offset}, {count}", e);
            }
            if (this.m_Socket.SendAsync(this.m_OutArgs))
            {
                return;
            }
            OnSendComplete(this.m_OutArgs);
        }

        private void OnSendComplete(object o)
        {
            if (this.m_Socket == null)
            {
                return;
            }
            SocketAsyncEventArgs e = (SocketAsyncEventArgs)o;

            if (e.SocketError != SocketError.Success)
            {
                this.OnError((int)e.SocketError);
                DisConnect();
                return;
            }

            if (e.BytesTransferred == 0)
            {
                this.OnError(ErrorCode.ERR_PeerDisconnect);
                DisConnect();
                return;
            }

            this.m_SendBuffer.FirstIndex += e.BytesTransferred;
            if (this.m_SendBuffer.FirstIndex == this.m_SendBuffer.ChunkSize)
            {
                this.m_SendBuffer.FirstIndex = 0;
                this.m_SendBuffer.RemoveFirst();
            }

            this.StartSend();
        }
        /*
        public void CheckTimeOut()
        {
            if(GetService().TimeNow - m_LastRecvTime >= NetWorkConstant.Tcp_TimeOut)
            {
                DisConnect();
            }
        }*/
    
    }
}