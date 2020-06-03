using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public class GChatRoom : Grain, IChatRoom
    {

        ChatRoomInfo m_ChatRoomInfo = new ChatRoomInfo();
        IChatRoomEntry m_IChatRoomEntry;
        public Task<int> Exit(string id)
        {
            var iret = 1;
            return Task.FromResult(iret);
        }

        public Task<bool> Init()
        {
            var bret = true;
            m_ChatRoomInfo.Id = this.GetPrimaryKeyString();
            m_ChatRoomInfo.Ls_ChatUser.Clear();
            m_ChatRoomInfo.ChatRoom_State = EChatRoom_State.Active;

            this.m_IChatRoomEntry = GrainFactory.GetGrain<IChatRoomEntry>(0);
            return Task.FromResult(bret) ;
        }

        public Task<int> Join(ChatUser_Data user)
        {
            var iret = 1;
            return Task.FromResult(iret);
        }

        public async Task SendMessage(byte[] msg)
        {
            
            foreach(var cu_data in m_ChatRoomInfo.Ls_ChatUser)
            {
                await m_IChatRoomEntry.SendMessage(cu_data.Id,msg);
            }

        }

        public Task<bool> ShutDown()
        {
            var bret = true;

            m_ChatRoomInfo.Id = this.GetPrimaryKeyString();
            m_ChatRoomInfo.Ls_ChatUser.Clear();
            m_ChatRoomInfo.ChatRoom_State = EChatRoom_State.InActive;

            return Task.FromResult(bret);
        }

        public Task Update(float t)
        {
            return Task.CompletedTask;
        }





    }
}
