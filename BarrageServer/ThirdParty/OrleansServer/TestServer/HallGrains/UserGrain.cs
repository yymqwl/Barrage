using IHall;
using Orleans;
using Orleans.Providers;
using System;
using System.Threading.Tasks;

namespace HallGrains
{

    public class UserGrain : Grain, IUser
    {
        public bool IsConnected { get; set; }
        public DateTime LastActiveTime { get; set; }
        public string Name { get; set; }
        public Task<bool> Connected()
        {

            return Task.FromResult(this.IsConnected);
        }

        public Task SetName(string name)
        {

            this.LastActiveTime = DateTime.Now;
            this.Name = name;
            this.IsConnected = true;

            Console.WriteLine($"Active:{name}");
            return Task.CompletedTask;
        }
        public Task Say(string msg)
        {
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }

        public Task Update(float t)
        {
            var ts = DateTime.Now - this.LastActiveTime;
            if (ts.TotalSeconds > 10)
            {
                this.IsConnected = false;
            }
            return Task.CompletedTask;
        }

    }
}
