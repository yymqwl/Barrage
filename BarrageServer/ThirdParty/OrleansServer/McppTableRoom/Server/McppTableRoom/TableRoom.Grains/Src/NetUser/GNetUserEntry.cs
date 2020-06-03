using GameFramework;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public class GNetUserEntry : GEntry, INetUserEntry
    {
        protected Dictionary<string,INetUser> m_Dict_INetUser = new Dictionary<string, INetUser>();
        protected LinkedList<string> m_NeedDel_INetUser = new LinkedList<string>();
        protected UpdateTime m_Update_NetUser;
        public Task<INetUser> GetINetUser(string id)
        {
            return Task.FromResult(GrainFactory.GetGrain<INetUser>(id));
        }

        public async Task<bool> IsConnected(string id)
        {
            var inu = await GetINetUser(id);
            return await inu.GetIsConnected();
        }

        public async Task CreateNetUser(string id, INetUserObserver netUserObservers)
        {
            var inu = await GetINetUser(id);
            await inu.SetObserver(netUserObservers);
            m_Dict_INetUser.TryAdd(id, inu);
        }

        public async Task<int> SendMessage(string id,byte[] message)
        {
            var inu = await GetINetUser(id);
            return await inu.SendMessage(message);
        }
        


        public override Task<bool> Init()
        {
            m_Dict_INetUser.Clear();
            m_Update_NetUser = new UpdateTime(GameConstant.TCheckNetUser);
            m_Update_NetUser.Evt_Act += Update_NetUser;
            var bret = true;
            return Task.FromResult(bret);
        }

        public override Task<bool> ShutDown()
        {
            var bret = true;
            return Task.FromResult(bret);
        }
        public async void Update_NetUser()
        {
            //Log.Debug("Update_NetUser");
            m_NeedDel_INetUser.Clear();
            foreach (var vk in m_Dict_INetUser)
            {
                await vk.Value.CheckConnected();
                if(! await vk.Value.GetIsConnected() )
                {
                    m_NeedDel_INetUser.AddLast(vk.Key);
                }
            }
            
            foreach(var idstr in m_NeedDel_INetUser)
            {
                m_Dict_INetUser.Remove(idstr);
                Log.Debug($"OutOfTime{idstr}");
            }

        }
        
        
        public override Task Update(float t)
        {
            m_Update_NetUser.Update(t);
            return Task.CompletedTask;
        }

    }
}
