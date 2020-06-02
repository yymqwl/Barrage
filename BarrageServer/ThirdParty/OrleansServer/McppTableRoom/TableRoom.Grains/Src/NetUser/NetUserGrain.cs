using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public class NetUserGrain : Grain , INetUser
    {
        public bool IsConnected { get; set; }
        public DateTime LastActiveTime { get; set; }


        protected INetUserObserver m_INetUserObserver;

        public Task<bool> GetIsConnected()
        {
            return Task.FromResult(this.IsConnected);
        }

        public Task SendMessage(byte[] message)
        {
            if (m_INetUserObserver != null)
            {
                m_INetUserObserver.Receive(message);
            }
            return Task.CompletedTask;
        }

        public Task SetObserver(INetUserObserver obser)
        {
            this.m_INetUserObserver = obser;
            return Task.CompletedTask;
        }

        public Task Ping()
        {
            this.LastActiveTime = DateTime.Now;
            return Task.CompletedTask;
        }
    }
}
