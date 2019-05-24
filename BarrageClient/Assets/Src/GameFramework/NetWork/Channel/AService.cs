using System;
using System.Net;

namespace GameFramework
{
    public enum NetworkProtocol
    {
        KCP,
        TCP,
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

        private Action<AChannel> m_RemoveCallback;
        public event Action<AChannel> RemoveCallback
        {
            add
            {
                this.m_RemoveCallback += value;
            }
            remove
            {
                this.m_RemoveCallback -= value;
            }
        }
        protected void OnDisConnect(AChannel channel)
        {
            m_RemoveCallback.InvokeGracefully(channel);
        }
        protected void OnAccept(AChannel channel)
        {
            this.m_AcceptCallback.Invoke(channel);
        }

        public abstract AChannel ConnectChannel(IPEndPoint ipEndPoint);

        public abstract AChannel ConnectChannel(string address);

        public abstract void Remove(long channelId);

        public abstract void Update();
    }
}