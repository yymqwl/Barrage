using IHall;
using Orleans;
using Orleans.Providers;
using System;
using System.Threading.Tasks;
using GameMain.ChatRoom;
using GameFramework;
using System.Threading;


namespace HallGrains
{

    public class ChatUserGrain : Grain, IChatUser
    {
        public bool IsConnected { get; set; }
        public DateTime LastActiveTime { get; set; }
        public string Name { get; set; }


        public long SessionId { get; set; }
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

       /*
        
        public Task Say(string msg)
        {
            Log.Debug($"SayGrain:ThreadId:{Thread.CurrentThread.ManagedThreadId}");

            IMainEntry ime = GrainFactory.GetGrain<IMainEntry>(0);
            var chatroom =   ime.GetIChatRoom();

            
            return Task.CompletedTask;
        }
        */
        
        public  async Task Say(string msg)
        {
            Console.WriteLine(msg);
            IMainEntry ime = GrainFactory.GetGrain<IMainEntry>(0);
            var chatroom = await ime.GetIChatRoom();
            await  chatroom.BroadCast(new Say_Res
            {
                Msg = $"{this.GetPrimaryKeyLong()}{Name}:{msg}"
            });

            //return Task.CompletedTask;
        }
        
        public   Task Update(float t)
        {
            var ts = DateTime.Now - this.LastActiveTime;
            if (ts.TotalSeconds > 10)
            {
                this.IsConnected = false;
            }
            return Task.CompletedTask;
        }

        public Task<long> GetId()
        {
            return Task.FromResult(this.GetPrimaryKeyLong());
        }

        public Task SetSessionId(long id)
        {
            SessionId = id;
            return Task.CompletedTask;
        }

        public Task<long> GetSessionId()
        {
            return Task.FromResult(SessionId);
        }

        public Task Ping()
        {
            this.LastActiveTime = DateTime.Now;
            return Task.CompletedTask;
        }

        public Task<bool> CheckTimeOut()
        {
            var ts = DateTime.Now - LastActiveTime;
            if (ts.TotalSeconds>= 10)
            {
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }
    }
}
