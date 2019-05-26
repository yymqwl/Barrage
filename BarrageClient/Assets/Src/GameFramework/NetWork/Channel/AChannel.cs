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
    /// Accept 有未连接,连接中,已经连接3种状态。生命周期连接中-》已连接-》未连接
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

        public virtual ChannelState ChannelState { get;  }

        private AService m_Service;

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

        private Action<MemoryStream> m_ReadCallback;

        public event Action<MemoryStream> ReadCallback
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

        protected void OnRead(MemoryStream memoryStream)
        {
            m_ReadCallback.InvokeGracefully(memoryStream);
        }

        public void OnError(int e)
        {
            this.Error = e;
            this.m_ErrorCallback?.Invoke(this, e);
        }

        protected AChannel(AService service, ChannelType channelType)
        {
            this.Id = IdGenerater.GenerateId();
            this.ChannelType = channelType;
            this.m_Service = service;
        }

        public abstract void Start();

        public abstract void Send(MemoryStream stream);


        public override void Init()
        {
            base.Init();
        }
        public override void Dispose()
        {
            base.Dispose();

            this.m_Service.Remove(this.Id);
        }
    }
}