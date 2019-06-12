using IHall;
using Orleans;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using System.Threading.Tasks;
using System.Threading;

namespace HallGrains
{

    public class MainEntryGrain : Grain, IMainEntry
    {
        IChatRoom m_ChatRoom;
        IGateWay m_IGateWay;
        IHello m_IHello;

        public Task<IChatRoom> GetIChatRoom()
        {
            Log.Debug($"GetIChatRoom:ThreadId:{Thread.CurrentThread.ManagedThreadId}");
            return Task.FromResult(GrainFactory.GetGrain<IChatRoom>(0));
        }

        public Task<IGateWay> GetIGateWay()
        {
            return Task.FromResult(GrainFactory.GetGrain<IGateWay>(0));
        }

        public Task<IHello> GetIHello()
        {
            return Task.FromResult(m_IHello);
        }

        public override async Task OnActivateAsync()
        {
            Log.Debug ($"{typeof(MainEntryGrain)}OnActivateAsync");
            m_ChatRoom = await GetIChatRoom();
            m_IGateWay = await GetIGateWay();
            m_IHello = GrainFactory.GetGrain<IHello>(0);

            RegisterTimer(Update_Timer, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            await base.OnActivateAsync();
        }
        public async override Task OnDeactivateAsync()
        {
            Log.Debug($"{typeof(MainEntryGrain)}OnDeactivateAsync");
            await base.OnDeactivateAsync();
        }

        public Task Update()
        {
            return Task.CompletedTask;
        }

        public async Task Update_Timer(object obj)
        {
            await m_IHello.Update();
            await m_ChatRoom.Update();
            await m_IGateWay.Update();

            
            //Log.Debug($"MainEntry Update: threadId{Thread.CurrentThread.ManagedThreadId}");
            //return Task.CompletedTask;
        }

    }
        /*
        public List<IUser> Ls_UserGrain = new List<IUser>();

        public string Str_Test = "MainEntryGrain";

        private ObserverSubscriptionManager<IMainEntry_Obs> m_subsManager;



        public int m_Count = 0;

        private StreamSubscriptionHandle<string> m_SSHandle;

        public override async Task OnActivateAsync()
        {
            Console.WriteLine($"{typeof(MainEntryGrain)}OnActivateAsync");


            RegisterTimer(Update_Timer, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            m_subsManager = new ObserverSubscriptionManager<IMainEntry_Obs>();

            

            await base.OnActivateAsync();
        }
        public async override Task OnDeactivateAsync()
        {
            Console.WriteLine($"{typeof(MainEntryGrain)}OnDeactivateAsync");
            await m_SSHandle.UnsubscribeAsync();
          
            m_subsManager.Clear();
            await base.OnDeactivateAsync();
        }

        public  Task Update_Timer(object obj)
        {
            m_Count++;
            Console.WriteLine($"Update_Timer{m_Count}");
            return Task.CompletedTask;
            //await Update(0);
        }
        public   Task<Guid> Enter()
        {
            return  Task.FromResult(Guid.NewGuid());
        }

        public async Task<IUser> Join(long id)
        {
            var usergrain = GrainFactory.GetGrain<IUser>(id);
            await usergrain.SetName(id.ToString());
            Ls_UserGrain.Add(usergrain);
            return usergrain;
        }

        public Task SayHello()
        {
            string str_hello = "SayHello";
            Console.WriteLine($"{str_hello}Send");
            
            m_subsManager.Notify((IMainEntry_Obs imev) =>
            {
                imev.Handle(str_hello);

            });
           
            return Task.CompletedTask;
        }

        public  Task SendMsg(string msg)
        {
            string str_msg = "SendMsg";

            Console.WriteLine($"Msg:{str_msg}");

            Console.WriteLine($"Msg1:{Str_Test}");
            Str_Test = "MainEntryGrain:1";

            return Task.CompletedTask;
        }

        public Task SubscribeAsync(IMainEntry_Obs viewer)
        {
            if (!m_subsManager.IsSubscribed(viewer))
            {
                m_subsManager.Subscribe(viewer);
            }
            return Task.CompletedTask;
        }

        public Task UnsubscribeAsync(IMainEntry_Obs viewer)
        {
            if (m_subsManager.IsSubscribed(viewer))
            {
                m_subsManager.Unsubscribe(viewer);
            }
            return Task.CompletedTask;
        }

        public  Task Update(float t)
        {
            return Task.CompletedTask;
        }
    }*/
}
