using GameFramework;
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

        public Task<int> SendMessage(byte[] message)
        {
            var iret = -1;
            if (m_INetUserObserver != null && IsConnected)
            {
                m_INetUserObserver.Receive(message);
                iret = 1;
            }
            return Task.FromResult(iret);
        }
         

        public Task SetObserver(INetUserObserver obser)
        {
            this.m_INetUserObserver = obser;
            IsConnected = true;
            this.LastActiveTime = DateTime.Now;
            return Task.CompletedTask;
        }
        
       

        public  Task Ping()
        {
            this.LastActiveTime = DateTime.Now;
            return Task.CompletedTask;
        }

        public Task CheckConnected()
        {
            if(IsConnected)
            {
                var ts = DateTime.Now - this.LastActiveTime;
                if (ts.TotalSeconds >= GameConstant.TClearNetUser)
                {
                    IsConnected = false;
                }
            }

            return Task.CompletedTask;

        }


        
        public override Task OnActivateAsync()
        {
            Log.Debug($"OnActivateAsync:NetUserGrain {this.GetPrimaryKeyString()} ");
            return base.OnActivateAsync();
        }
        public override Task OnDeactivateAsync()
        {
            Log.Debug($"OnDeactivateAsync:NetUserGrain {this.GetPrimaryKeyString()} ");
            return base.OnDeactivateAsync();
        }
    }
}
