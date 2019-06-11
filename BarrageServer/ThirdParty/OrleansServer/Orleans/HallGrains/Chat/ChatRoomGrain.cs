﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GameFramework;
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
                return null;
            }
            Dict_ChatUser.Add(id,icr);
            return Task.FromResult(icr);
        }

        public Task ExitRoom(long id)
        {
            if (Dict_ChatUser.Remove(id))
            {
                
            }
            return Task.CompletedTask;
        }
        
         

        public Task<IChatUser> GetChatUser(long id)
        {
            IChatUser icu = null;
            Dict_ChatUser.TryGetValue(id,out icu);
            return Task.FromResult(icu);
        }

        public Task Update()
        {
            return Task.CompletedTask;
        }

        public async Task BroadCast(IMessage msg)
        {
            IMainEntry ime = GrainFactory.GetGrain<IMainEntry>(0);
            var igw= await ime.GetIGateWay();

            foreach(var vk in  Dict_ChatUser)
            {
                await igw.Reply(vk.Key, msg);
            }

            //return Task.CompletedTask;
            
        }
    }
}
