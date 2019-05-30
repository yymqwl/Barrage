using IHall;
using Orleans;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HallGrains
{
    [ImplicitStreamSubscription(GameConstant.HallStreamInput)]
    public class MainEntryGrain : Grain, IMainEntry , IAsyncObserver<string>
    {
        public List<IUser> Ls_UserGrain = new List<IUser>();

        public string Str_Test = "MainEntryGrain";

        private ObserverSubscriptionManager<IMainEntry_Obs> m_subsManager;

        private IAsyncStream<string> m_Stream;

        public override async Task OnActivateAsync()
        {
            Console.WriteLine($"{typeof(MainEntryGrain)}OnActivateAsync");
            m_subsManager = new ObserverSubscriptionManager<IMainEntry_Obs>();
            //RegisterTimer(Update_Timer, null, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));

            var streamProvider = this.GetStreamProvider(GameConstant.HallStreamProvider);
            m_Stream = streamProvider.GetStream<string>(Guid.NewGuid(), GameConstant.HallStreamInput);

            await m_Stream.SubscribeAsync(OnNextAsync);

            await base.OnActivateAsync();
        }
        public override Task OnDeactivateAsync()
        {
            Console.WriteLine($"{typeof(MainEntryGrain)}OnDeactivateAsync");
            m_Stream.GetAllSubscriptionHandles().Dispose();
            return base.OnDeactivateAsync();
        }

        public async Task Update_Timer(object obj)
        {
            Console.WriteLine("Update_Timer");
            await Update(0);
        }
        public   Task<Guid> Enter()
        {
            return  Task.FromResult(m_Stream.Guid);
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

        public async Task SendMsg(string msg)
        {
            string str_msg = "SendMsg";

            Console.WriteLine($"Msg:{str_msg}");

            Console.WriteLine($"Msg1:{Str_Test}");
            Str_Test = "MainEntryGrain:1";
            
            var hello = GetHello(0).Result;
            str_msg = await hello.GetName();
            Console.WriteLine($"Msg:{str_msg}");
            

            //return Task.CompletedTask;
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

        public async Task Update(float t)
        {
            List<IUser> tmp_del = new List<IUser>();

            for (int i = 0; i < this.Ls_UserGrain.Count; ++i)
            {
                IUser iuser = this.Ls_UserGrain[i];
                await iuser.Update(t);
                if (!(await iuser.Connected()))
                    tmp_del.Add(iuser);
            }

            if (tmp_del.Count > 0)
            {
                for (int i = 0; i < tmp_del.Count; ++i)
                {
                    IUser iuser = tmp_del[i];
                    this.Ls_UserGrain.Remove(iuser);
                    Console.WriteLine($"userRm{iuser.GetPrimaryKeyLong()}");
                }
            }
        }

        
        public Task<IMySql.IMysqlEntry> GetHello(long id)
        {
            var usergrain = GrainFactory.GetGrain<IMySql.IMysqlEntry>(id);
            return Task.FromResult(usergrain);
        }

        public Task OnNextAsync(string item, StreamSequenceToken token = null)
        {
            return Task.CompletedTask;
        }

        public Task OnCompletedAsync()
        {
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            return Task.CompletedTask;
        }
    }
}
