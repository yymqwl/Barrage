using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public class GChatUser : NetUserGrain, IChatUser
    {
        protected ChatUser_Data m_ChatUser_Data;
        public Task<ChatUser_Data> GetChatUser_Data()
        {
            return Task.FromResult(m_ChatUser_Data);
        }

        public Task SetChatUser_Data(ChatUser_Data cu)
        {
            this.m_ChatUser_Data = cu;
            return Task.CompletedTask;
        }

        
    }
}
