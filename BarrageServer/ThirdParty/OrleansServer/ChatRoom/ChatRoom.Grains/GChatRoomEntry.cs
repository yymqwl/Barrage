using IHall;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GHall
{
    public class GChatRoomEntry : Grain, IChatRoomEntry
    {

        public override Task OnActivateAsync()
        {
            Console.WriteLine("OnActivateAsync GChatRoomEntry"+this.iden);
            return base.OnActivateAsync();
        }
        public override Task OnDeactivateAsync()
        {

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
