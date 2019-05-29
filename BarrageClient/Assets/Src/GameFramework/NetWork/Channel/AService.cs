using System;
using System.Net;

namespace GameFramework
{
    public enum NetworkProtocol
    {
        TCP,
        KCP,
        WebSocket,
    }

    public abstract class AService : CObject
    {
        public abstract AChannel GetChannel(long id);

        ///接收Channel
        private Action<AChannel> m_AcceptCallback;

        public event Action<AChannel> AcceptCallback
        {
            add
            {
                this.m_AcceptCallback += value;
            }
            remove
            {
                this.m_AcceptCallback -= value;
            }
        }

        ///Service 开始时间
        public long m_StartTime;
        //当前时间
        public uint TimeNow { get;  set; }


        private Action<AChannel> m_DisConnectedCallback;
        public event Action<AChannel> DisConnectedCallback
        {
            add
            {
                this.m_DisConnectedCallback += value;
            }
            remove
            {
                this.m_DisConnectedCallback -= value;
            }
        }
        public AService()
        {
            this.m_StartTime = TimeHelper.ClientNow();
            this.TimeNow = (uint)TimeHelper.ClientNow();
        }
        public void OnDisConnected(AChannel channel)
        {
            m_DisConnectedCallback.InvokeGracefully(channel);
            Remove(channel.Id);
        }
        protected void OnAccept(AChannel channel)
        {
            this.m_AcceptCallback.Invoke(channel);
        }


        public abstract AChannel ConnectChannel(IPEndPoint ipEndPoint);

        public abstract AChannel ConnectChannel(string address);

        public abstract void Remove(long channelId);

        public virtual void Update()
        {
            this.TimeNow = (uint)(TimeHelper.ClientNow() - this.m_StartTime);
        }
    }
}