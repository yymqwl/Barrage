using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
using GameMain.ChatRoom;
using IHall;
using Orleans;

namespace HallGrains
{
    public class ChatRoomGrain : Grain, IChatRoom
    {
        protected Dictionary<long,IChatUser> Dict_ChatUser = new Dictionary<long, IChatUser>();
        public override async Task OnActivateAsync()
        {
             Dict_ChatUser.Clear();


             await base.OnActivateAsync();
        }
        public Task<IChatUser> EnterRoom(long id)
        {
            var icr = GrainFactory.GetGrain<IChatUser>(id);
            if(Dict_ChatUser.ContainsKey(id))
            {
                return Task.FromResult(Dict_ChatUser[id]);
            }
            Dict_ChatUser.Add(id,icr);
            return Task.FromResult(icr);
        }

        public async Task ExitRoom(long id)
        {
            if (Dict_ChatUser.Remove(id))
            {
                await BroadCast(new Say_Res { Msg = "Id:ExitRoom"  });
            }
            //return Task.CompletedTask;
        }
        
         

        public Task<IChatUser> GetChatUser(long id)
        {
            IChatUser icu = null;
            Dict_ChatUser.TryGetValue(id,out icu);
            return Task.FromResult(icu);
        }

        public async Task Update()
        {
            List<long> Ls_user = new List<long>();
            foreach(var vk in Dict_ChatUser)
            {
                 if( await vk.Value.CheckTimeOut())
                {
                    Ls_user.Add(vk.Key);
                }

            }
            if(Ls_user.Count>0)
            {
                
                Ls_user.ForEach(async (long id) =>
                {
                     await ExitRoom(id);
                });
                
            }
          
        }

        public async Task BroadCast(IMessage msg)
        {
            IMainEntry ime = GrainFactory.GetGrain<IMainEntry>(0);
            var igw= await ime.GetIGateWay();

            foreach(var vk in  Dict_ChatUser)
            {
                await igw.Reply(await vk.Value.GetSessionId(), msg);
            }
        }

        public async Task DirectPingUser(long id)
        {
            var iuser = await GetChatUser(id);
            if(iuser!=null)
            {
                await iuser.Ping();
            }

        }
    }
}
