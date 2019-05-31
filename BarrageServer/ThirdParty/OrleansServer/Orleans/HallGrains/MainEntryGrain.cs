using IHall;
using Orleans;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HallGrains
{
 
    public class MainEntryGrain : Grain, IMainEntry
    {
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
    }
}
