using System;
using System.IO;
using System.Net;
using GameFramework;

namespace GameFramework
{
    public enum ChannelType
    {
        Connect,
        Accept,
    }

    /// <summary>
    /// Accept 有未连接,连接中,已经连接3种状态。生命周期连接中->已连接->未连接
    /// Connect 生命周期。连接中->连接->未连接->连接中
    /// </summary>
    public enum ChannelState
    {
        EDisConnected,//未连接
        EConnecting,//连接中
        EConnected,//已连接
    }


    public abstract class AChannel : CObject
    {
        public long Id { get; set; }
        public ChannelType ChannelType { get; }

        public  ChannelState ChannelState => m_ChannelState;

        protected ChannelState m_ChannelState;

        public bool IsConnected { get { return ChannelState == ChannelState.EConnected; } }

        private AService m_Service;

        protected bool m_IsDisposed ;

        protected uint m_LastRecvTime;

        protected readonly uint m_CreateTime;

        public uint LastRecvTime { get { return m_LastRecvTime; } }
        public uint CreateTime { get { return m_CreateTime; } }

        public AService Service
        {
            get
            {
                return this.m_Service;
            }
        }

        public virtual bool IsDisposed
        {
            get;
        }
        public abstract MemoryStream Stream { get; }

        public int Error { get; set; }

        public IPEndPoint RemoteAddress { get; protected set; }

        private Action<AChannel, int> m_ErrorCallback;

        public event Action<AChannel, int> ErrorCallback
        {
            add
            {
                this.m_ErrorCallback += value;
            }
            remove
            {
                this.m_ErrorCallback -= value;
            }
        }

        private Action<AChannel,MemoryStream> m_ReadCallback;

        public event Action<AChannel, MemoryStream> ReadCallback
        {
            add
            {
                this.m_ReadCallback += value;
            }
            remove
            {
                this.m_ReadCallback -= value;
            }
        }

        protected void OnRead(AChannel ac ,MemoryStream memoryStream)
        {
            m_ReadCallback.InvokeGracefully(ac,memoryStream);
        }

        public virtual void OnError(int e)
        {
            this.Error = e;
            this.m_ErrorCallback?.Invoke(this, e);
        }

        protected AChannel(AService service, ChannelType channelType)
        {
            this.Id = IdGenerater.GenerateId();
            this.ChannelType = channelType;
            this.m_Service = service;
            m_IsDisposed = false;
            m_ChannelState = ChannelState.EConnecting;

            this.m_LastRecvTime = service.TimeNow;
            this.m_CreateTime = service.TimeNow;
        }

        protected virtual void Connect()
        {
            if(ChannelType != ChannelType.Connect)
            {
                throw new GameFrameworkException($"Error {ChannelType}");
            }
        }
        protected virtual void Accept()
        {
            if(ChannelType!= ChannelType.Accept)
            {
                throw new GameFrameworkException($"Error {ChannelType}");
            }
        }
        public abstract void DisConnect();


        public abstract void Send(MemoryStream stream);


        public override void Init()
        {
            base.Init();
        }
        public override void Dispose()
        {
            base.Dispose();
            m_IsDisposed = true;
            this.m_Service.Remove(this.Id);
        }
    }
}