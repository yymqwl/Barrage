﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using KcpLib;

namespace GameFramework
{
    public struct WaitSendBuffer
    {
        public byte[] Bytes;
        public int Length;

        public WaitSendBuffer(byte[] bytes, int length)
        {
            this.Bytes = bytes;
            this.Length = length;
        }
    }

    public class KChannel : AChannel
    {
        private Socket m_Socket;

        private IntPtr m_Kcp;

        private readonly Queue<WaitSendBuffer> m_SendBuffer = new Queue<WaitSendBuffer>();


        private readonly IPEndPoint m_RemoteEndPoint;

        //private uint m_LastRecvTime;

        //private readonly uint m_CreateTime;

        public uint RemoteConn { get; private set; }

        private readonly MemoryStream m_MemoryStream;



        // Accept
        public KChannel(uint localConn, uint remoteConn, Socket m_Socket, IPEndPoint m_RemoteEndPoint, KService kService) : base(kService, ChannelType.Accept)
        {
            this.m_MemoryStream = this.GetService().MemoryStreamManager.GetStream(NetWorkConstant.Str_Msg, ushort.MaxValue);

            this.LocalConn = localConn;
            this.RemoteConn = remoteConn;
            this.m_RemoteEndPoint = m_RemoteEndPoint;
            this.m_Socket = m_Socket;
            this.m_Kcp = Kcp.KcpCreate(this.RemoteConn, new IntPtr(this.LocalConn));

           
            ///Kcp 参数
            SetOutput();
            Kcp.KcpNodelay(this.m_Kcp, 1, 10, 1, 1);
            Kcp.KcpWndsize(this.m_Kcp, 256, 256);
            Kcp.KcpSetmtu(this.m_Kcp, 470);
            ///

            

            this.Accept();
        }

        // Connect
        public KChannel(uint localConn, Socket m_Socket, IPEndPoint m_RemoteEndPoint, KService kService) : base(kService, ChannelType.Connect)
        {
            this.m_MemoryStream = this.GetService().MemoryStreamManager.GetStream(NetWorkConstant.Str_Msg, ushort.MaxValue);
            this.LocalConn = localConn;
            this.m_Socket = m_Socket;
            this.m_RemoteEndPoint = m_RemoteEndPoint;


            this.Connect();
        }

        public uint LocalConn
        {
            get
            {
                return (uint)this.Id;
            }
            set
            {
                this.Id = value;
            }
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();


            if (this.m_Kcp != IntPtr.Zero)
            {
                Kcp.KcpRelease(this.m_Kcp);
                this.m_Kcp = IntPtr.Zero;
            }
            this.m_Socket = null;
            this.m_MemoryStream.Dispose();
        }

        public override MemoryStream Stream
        {
            get
            {
                return this.m_MemoryStream;
            }
        }



        private KService GetService()
        {
            return (KService)this.Service;
        }

        public void HandleConnnect(uint remoteConn)
        {
            if (this.IsConnected)
            {
                return;
            }

            this.RemoteConn = remoteConn;

            this.m_Kcp = Kcp.KcpCreate(this.RemoteConn, new IntPtr(this.LocalConn));
            SetOutput();
            Kcp.KcpNodelay(this.m_Kcp, 1, 10, 1, 1);
            Kcp.KcpWndsize(this.m_Kcp, 256, 256);
            Kcp.KcpSetmtu(this.m_Kcp, 470);

            m_ChannelState = ChannelState.EConnected;
            this.m_LastRecvTime = this.GetService().TimeNow;
            GetService().RemoveWaitConnectChannels(RemoteConn);

            HandleSend();
        }

        protected override void Accept()
        {
            if (this.m_Socket == null)
            {
                return;
            }

            uint timeNow = this.GetService().TimeNow;

            try
            {
                byte[] buffer = this.m_MemoryStream.GetBuffer();
                buffer.WriteTo(0, KcpProtocalType.ACK);
                buffer.WriteTo(1, LocalConn);
                buffer.WriteTo(5, RemoteConn);
                this.m_Socket.SendTo(buffer, 0, 9, SocketFlags.None, m_RemoteEndPoint);

                // 200毫秒后再次update发送connect请求
                this.GetService().AddToUpdateNextTime(timeNow + NetWorkConstant.Kcp_Delay_Time_Connect, this.Id);
            }
            catch (Exception e)
            {
                Log.Error(e);
                this.OnError(ErrorCode.ERR_SocketCantSend);
            }
        }

        /// <summary>
        /// 发送请求连接消息
        /// </summary>
        protected override void Connect()
        {
            if(IsDisposed)
            {
                return;
            }
            try
            {
                uint timeNow = this.GetService().TimeNow;

                this.m_LastRecvTime = timeNow;

                byte[] buffer = this.m_MemoryStream.GetBuffer();
                buffer.WriteTo(0, KcpProtocalType.SYN);
                buffer.WriteTo(1, this.LocalConn);
                this.m_Socket.SendTo(buffer, 0, 5, SocketFlags.None, m_RemoteEndPoint);

                // 200毫秒后再次update发送connect请求
                this.GetService().AddToUpdateNextTime(timeNow + NetWorkConstant.Kcp_Delay_Time_Connect, this.Id);
            }
            catch (Exception e)
            {
                Log.Error(e);
                this.OnError(ErrorCode.ERR_SocketCantSend);
            }
        }

        public override  void DisConnect()
        {
            if(m_ChannelState == ChannelState.EDisConnected)//已断开不作处理
            {
                return;
            }
            m_ChannelState = ChannelState.EDisConnected;
            if (this.m_Socket == null)
            {
                return;
            }
            try
            {
                byte[] buffer = this.m_MemoryStream.GetBuffer();
                buffer.WriteTo(0, KcpProtocalType.FIN);
                buffer.WriteTo(1, this.LocalConn);
                buffer.WriteTo(5, this.RemoteConn);
                buffer.WriteTo(9, (uint)this.Error);
                this.m_Socket.SendTo(buffer, 0, 13, SocketFlags.None, m_RemoteEndPoint);
            }
            catch (Exception e)
            {
                Log.Error(e);
                this.OnError(ErrorCode.ERR_SocketCantSend);
            }

            GetService().OnDisConnected(this);

        }

        public void Update()
        {
            if (this.IsDisposed)
            {
                return;
            }

            uint timeNow = this.GetService().TimeNow;

            if(m_ChannelState == ChannelState.EConnecting)
            {
                // 10秒没连接上直接从Service删除刷新
                if (timeNow - this.m_CreateTime > NetWorkConstant.Kcp_Connecting_Time)
                {
                    TimeOut();
                    this.OnError(ErrorCode.ERR_KcpCantConnect);
                    return;
                }

                if (timeNow - this.m_LastRecvTime < NetWorkConstant.Kcp_Delay_Time_Connect)
                {
                    return;
                }
                switch (ChannelType)
                {
                    case ChannelType.Accept:
                        this.Accept();
                        break;
                    case ChannelType.Connect:
                        this.Connect();
                        break;
                }
                return;
            }

            ///检查超时处理
            ///10S未连接直接删除刷新
            if (timeNow - this.m_LastRecvTime > NetWorkConstant.KcpIdleSessionTimeOut)
            {
                TimeOut();
                return;
            }


            try
            {
                Kcp.KcpUpdate(this.m_Kcp, timeNow);
            }
            catch (Exception e)
            {
                Log.Error(e);
                this.OnError(ErrorCode.ERR_SocketError);
                return;
            }


            if (this.m_Kcp != IntPtr.Zero)
            {
                uint nextUpdateTime = Kcp.KcpCheck(this.m_Kcp, timeNow);
                this.GetService().AddToUpdateNextTime(nextUpdateTime, this.Id);
            }


        }

        /// <summary>
        /// 超时直接释放
        /// </summary>
        private void TimeOut()
        {
            DisConnect();
        }

        private void HandleSend()
        {
            while (true)
            {
                if (this.m_SendBuffer.Count <= 0)
                {
                    break;
                }

                WaitSendBuffer buffer = this.m_SendBuffer.Dequeue();
                this.KcpSend(buffer.Bytes, buffer.Length);
            }
        }

        public void HandleRecv(byte[] date, int offset, int length)
        {
            if (this.IsDisposed)
            {
                return;
            }


            Kcp.KcpInput(this.m_Kcp, date, offset, length);
            this.GetService().AddToUpdateNextTime(0, this.Id);

            while (true)
            {
                if (this.IsDisposed)
                {
                    return;
                }
                int n = Kcp.KcpPeeksize(this.m_Kcp);
                if (n < 0)
                {
                    return;
                }
                if (n == 0)
                {
                    this.OnError((int)SocketError.NetworkReset);
                    return;
                }

                byte[] buffer = this.m_MemoryStream.GetBuffer();
                this.m_MemoryStream.SetLength(n);
                this.m_MemoryStream.Seek(0, SeekOrigin.Begin);
                int count = Kcp.KcpRecv(this.m_Kcp, buffer, ushort.MaxValue);
                if (n != count)
                {
                    return;
                }
                if (count <= 0)
                {
                    return;
                }

                this.m_LastRecvTime = this.GetService().TimeNow;
                m_ChannelState = ChannelState.EConnected;

                this.OnRead(this,this.m_MemoryStream);
            }
        }


        public void Output(IntPtr bytes, int count)
        {
            if (this.IsDisposed)
            {
                return;
            }
            try
            {
                if (count == 0)
                {
                    Log.Error($"output 0");
                    return;
                }

                byte[] buffer = this.m_MemoryStream.GetBuffer();
                buffer.WriteTo(0, KcpProtocalType.MSG);
                // 每个消息头部写下该channel的id;
                buffer.WriteTo(1, this.LocalConn);
                Marshal.Copy(bytes, buffer, 5, count);
                this.m_Socket.SendTo(buffer, 0, count + 5, SocketFlags.None, this.m_RemoteEndPoint);
            }
            catch (Exception e)
            {
                Log.Error(e);
                this.OnError(ErrorCode.ERR_SocketCantSend);
            }
        }

        private kcp_output m_kcpOutput;
        public void SetOutput()
        {
            m_kcpOutput = KcpOutput;
            Kcp.KcpSetoutput(this.m_Kcp,m_kcpOutput);
        }


#if ENABLE_IL2CPP
		[AOT.MonoPInvokeCallback(typeof(kcp_output))]
#endif
        public static int KcpOutput(IntPtr bytes, int len, IntPtr m_Kcp, IntPtr user)
        {
            KService.Output(bytes, len, user);
            return len;
        }

        private void KcpSend(byte[] buffers, int length)
        {
            if (this.IsDisposed)
            {
                return;
            }
            Kcp.KcpSend(this.m_Kcp, buffers, length);
            this.GetService().AddToUpdateNextTime(0, this.Id);
        }

        private void Send(byte[] buffer, int index, int length)
        {
            if (IsConnected)
            {
                this.KcpSend(buffer, length);
                return;
            }

            this.m_SendBuffer.Enqueue(new WaitSendBuffer(buffer, length));
        }

        public override void Send(MemoryStream stream)
        {
            if (this.m_Kcp != IntPtr.Zero)
            {
                // 检查等待发送的消息，如果超出两倍窗口大小，应该断开连接
                if (Kcp.KcpWaitsnd(this.m_Kcp) > NetWorkConstant.Kcp_WndSize * 2)
                {
                    this.OnError(ErrorCode.ERR_KcpWaitSendSizeTooLarge);
                    return;
                }
            }

            ushort size = (ushort)(stream.Length - stream.Position);
            byte[] bytes;
            if (this.IsConnected)
            {
                bytes = stream.GetBuffer();
            }
            else
            {
                bytes = new byte[size];
                Array.Copy(stream.GetBuffer(), stream.Position, bytes, 0, size);
            }

            Send(bytes, 0, size);
        }
    }
}
