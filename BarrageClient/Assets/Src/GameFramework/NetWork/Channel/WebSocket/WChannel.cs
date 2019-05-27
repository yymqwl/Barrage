using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Threading;

namespace GameFramework
{
    public class WChannel : AChannel
    {
        public HttpListenerWebSocketContext WebSocketContext { get; }

        private readonly WebSocket m_WebSocket;

        private readonly Queue<byte[]> m_Queue = new Queue<byte[]>();

        private bool m_IsSending;

        //private bool m_IsConnected;

        private readonly MemoryStream m_MemoryStream;

        private readonly MemoryStream m_RecvStream;

        private CancellationTokenSource m_CancellationTokenSource = new CancellationTokenSource();

        public WChannel(HttpListenerWebSocketContext m_WebSocketContext, AService service) : base(service, ChannelType.Accept)
        {
            this.WebSocketContext = m_WebSocketContext;

            this.m_WebSocket = m_WebSocketContext.WebSocket;

            this.m_MemoryStream = this.GetService().MemoryStreamManager.GetStream("message", ushort.MaxValue);
            this.m_RecvStream = this.GetService().MemoryStreamManager.GetStream("message", ushort.MaxValue);

            m_ChannelState = ChannelState.EConnected;
            //m_IsConnected = true;
        }

        public WChannel(WebSocket m_WebSocket, AService service) : base(service, ChannelType.Connect)
        {
            this.m_WebSocket = m_WebSocket;

            this.m_MemoryStream = this.GetService().MemoryStreamManager.GetStream("message", ushort.MaxValue);
            this.m_RecvStream = this.GetService().MemoryStreamManager.GetStream("message", ushort.MaxValue);

            //m_IsConnected = false;
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            this.m_CancellationTokenSource.Cancel();
            this.m_CancellationTokenSource.Dispose();
            this.m_CancellationTokenSource = null;

            this.m_WebSocket.Dispose();

            this.m_MemoryStream.Dispose();
        }

        public override MemoryStream Stream
        {
            get
            {
                return this.m_MemoryStream;
            }
        }

        /*protected  void Start()
        {
            if (!this.m_IsConnected)
            {
                return;
            }

            this.StartRecv();
            this.StartSend();
        }*/

        private WService GetService()
        {
            return (WService)this.Service;
        }

        public async void ConnectAsync(string url)
        {
            try
            {
                await ((ClientWebSocket)this.m_WebSocket).ConnectAsync(new Uri(url), m_CancellationTokenSource.Token);
                m_ChannelState = ChannelState.EConnected;
                //m_IsConnected = true;
                //this.Start();
                this.StartRecv();
                this.StartSend();
            }
            catch (Exception e)
            {
                Log.Error(e);
                this.OnError(ErrorCode.ERR_WebsocketConnectError);
            }
        }

        public override void Send(MemoryStream stream)
        {
            byte[] bytes = new byte[stream.Length];
            Array.Copy(stream.GetBuffer(), bytes, bytes.Length);
            this.m_Queue.Enqueue(bytes);

            if (this.IsConnected)
            {
                this.StartSend();
            }
        }

        public async void StartSend()
        {
            if (this.IsDisposed)
            {
                return;
            }

            try
            {
                if (this.m_IsSending)
                {
                    return;
                }

                this.m_IsSending = true;

                while (true)
                {
                    if (this.m_Queue.Count == 0)
                    {
                        this.m_IsSending = false;
                        return;
                    }

                    byte[] bytes = this.m_Queue.Dequeue();
                    try
                    {
                        await this.m_WebSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Binary, true, m_CancellationTokenSource.Token);
                        if (this.IsDisposed)
                        {
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                        this.OnError(ErrorCode.ERR_WebsocketSendError);
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public async void StartRecv()
        {
            if (this.IsDisposed)
            {
                return;
            }

            try
            {
                while (true)
                {
#if SERVER
                    ValueWebSocketReceiveResult receiveResult;
#else
                    WebSocketReceiveResult receiveResult;
#endif
                    int receiveCount = 0;
                    do
                    {
#if SERVER
                        receiveResult = await this.m_WebSocket.ReceiveAsync(
                            new Memory<byte>(this.m_RecvStream.GetBuffer(), receiveCount, this.m_RecvStream.Capacity - receiveCount),
                            m_CancellationTokenSource.Token);
#else
                        receiveResult = await this.m_WebSocket.ReceiveAsync(
                            new ArraySegment<byte>(this.m_RecvStream.GetBuffer(), receiveCount, this.m_RecvStream.Capacity - receiveCount),
                            m_CancellationTokenSource.Token);
#endif
                        if (this.IsDisposed)
                        {
                            return;
                        }

                        receiveCount += receiveResult.Count;
                    }
                    while (!receiveResult.EndOfMessage);

                    if (receiveResult.MessageType == WebSocketMessageType.Close)
                    {
                        this.OnError(ErrorCode.ERR_WebsocketPeerReset);
                        return;
                    }

                    if (receiveResult.Count > ushort.MaxValue)
                    {
                        await this.m_WebSocket.CloseAsync(WebSocketCloseStatus.MessageTooBig, $"message too big: {receiveResult.Count}",
                            m_CancellationTokenSource.Token);
                        this.OnError(ErrorCode.ERR_WebsocketMessageTooBig);
                        return;
                    }

                    this.m_RecvStream.SetLength(receiveResult.Count);
                    this.OnRead(this,this.m_RecvStream);
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
                this.OnError(ErrorCode.ERR_WebsocketRecvError);
            }
        }

        public override void DisConnect()
        {
            m_ChannelState = ChannelState.EDisConnected;
            GetService().OnDisConnected(this);
        }
    }
}