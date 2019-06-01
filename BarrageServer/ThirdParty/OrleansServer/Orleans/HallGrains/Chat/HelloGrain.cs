using IHall;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HallGrains
{
    public class HelloGrain : Grain, IHello
    {
        public override async Task OnActivateAsync()
        {
            Console.WriteLine($"{typeof(HelloGrain)}OnActivateAsync");
            await base.OnActivateAsync();
        }
        public async override Task OnDeactivateAsync()
        {
            Console.WriteLine($"{typeof(HelloGrain)}OnActivateAsync");
            await base.OnDeactivateAsync();
        }
        public Task<string> SayHello()
        {
            return Task.FromResult("HelloGrain");
        }

    }
}
