using IHall;
using Orleans;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace GHall
{
    public class GChatRoomEntry : Grain, IChatRoomEntry
    {

        public override Task OnActivateAsync()
        {
            /*
            var ts1 = new TimeSpan(0, 0, 1);
            var m_Timer = this.RegisterTimer(Time_Update,null, ts1, ts1);*/
            Console.WriteLine("OnActivateAsync GChatRoomEntry"+this.GetPrimaryKeyLong());
            return base.OnActivateAsync();
        }
        /*
        protected Task Time_Update(object obj)
        {
            Console.WriteLine("Time_Update");
            return Task.CompletedTask;
        }*/
        public override Task OnDeactivateAsync()
        {
            Console.WriteLine("OnDeactivateAsync GChatRoomEntry" + this.GetPrimaryKeyLong());
            return base.OnDeactivateAsync();
        }
        public Task<bool> Init()
        {
            var pret = true;
            return Task.FromResult(pret);
        }

        public Task SayHello()
        {
            Console.WriteLine("Say Hello");
            return Task.CompletedTask;
        }

        public Task<bool> ShutDown()
        {
            var pret = true;
            return Task.FromResult(pret);
        }
    }
}
