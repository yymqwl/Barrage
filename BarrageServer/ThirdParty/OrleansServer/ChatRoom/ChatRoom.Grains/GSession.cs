using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Orleans;
using IHall;
using System.Threading.Tasks;

namespace GHall
{
    public class GSession : Grain  , IHall.ISession
    {
        protected ISessionObserver m_SObserver;
        public override Task OnActivateAsync()
        {
            return base.OnActivateAsync();
        }
        public override Task OnDeactivateAsync()
        {
            return base.OnDeactivateAsync();
        }

        public Task SayHello()
        {
            try
            {
                m_SObserver.ReceiveMessage("Hello");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return Task.CompletedTask;
        }

        public Task SetISessionObserver(ISessionObserver isobs)
        {
            this.m_SObserver = isobs;
            return Task.CompletedTask;
        }
    }
}
