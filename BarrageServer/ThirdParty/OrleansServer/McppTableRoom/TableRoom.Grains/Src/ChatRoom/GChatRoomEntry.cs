using Orleans;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using GameFramework;

namespace TableRoom
{
    public class GChatRoomEntry : GEntry, IChatRoomEntry
    {
        Dictionary<string,IChatRoom> m_Dict_ChatRoom = new Dictionary<string, IChatRoom>();
        Dictionary<string,IChatUser> m_Dict_ChatUser = new Dictionary<string, IChatUser>();

        
        public override Task OnActivateAsync()
        {
            Log.Debug("OnActivateAsync GChatRoomEntry"+this.GetPrimaryKeyLong());
            this.Clear();
         
            return base.OnActivateAsync();
        }

        public override Task Update(float t)
        {

            return Task.CompletedTask;
        }
        

        public void Clear()
        {
            m_Dict_ChatRoom.Clear();
            m_Dict_ChatUser.Clear();

        }
        public override Task OnDeactivateAsync()
        {
            Log.Debug("OnDeactivateAsync GChatRoomEntry" + this.GetPrimaryKeyLong());
            this.Clear();
            return base.OnDeactivateAsync();
        }

        public IChatUser GetChatUser(string id)
        {
            IChatUser icu = default;
            m_Dict_ChatUser.TryGetValue(id, out icu);
            return icu;
        }

        public IChatRoom GetChatRoom(string id)
        {
            IChatRoom icr = default;
            m_Dict_ChatRoom.TryGetValue(id, out icr);
            return icr;
        }
        public string GenerateId()
        {
           return IdStringGenerater.GenerateIdString(8);
        }
        public async Task<IChatRoom> CreateIChatRoom()
        {
            var cr = this.GrainFactory.GetGrain<IChatRoom>(GenerateId());
            await cr.Init();
            return cr;
        }
        public async Task<IChatUser>  CreateIChatUser(ChatUser_Data user)
        {
            var cu = this.GrainFactory.GetGrain<IChatUser>(user.Id);
            m_Dict_ChatUser.TryAdd(user.Id, cu);
            await cu.SetChatUser_Data(user);
            return cu;
        }
        public async Task<int> Join(ChatUser_Data user)
        {
            var iret = 1;
            var cu = this.GetChatUser(user.Id);
            if(cu == null)
            {
                cu = await CreateIChatUser(user);
            }
            IChatRoom cr = null; 
            if (user.ChatRoomId == string.Empty)
            {
                cr = await CreateIChatRoom();
            }
            else
            {
                cr = GetChatRoom(user.ChatRoomId);
                if(cr!= null)
                {
                    iret = -2;//已经在房间中
                    return iret;
                }
                else
                {
                    cr = await CreateIChatRoom();
                }
            }

            iret = await cr.Join(user);

            return iret;
        }

        public async Task<int> Exit(string id)
        {
            var iret = 1;
            var cu = this.GetChatUser(id);
            if(cu == null)
            {
                return iret;
                //return Task.FromResult(iret);
            }
            var cu_data = await cu.GetChatUser_Data();
            var cr = this.GetChatRoom(cu_data.ChatRoomId);
            if(cr == null)
            {
                iret = -3;
                return iret;
            }
            iret = await cr.Exit(id);

            return iret;
        }

        public async Task<int> SendMessage(string Id, byte[] msg)
        {
            var iret = 1;
            var cu = GetChatUser(Id);
            if(cu == null)
            {
                iret = -2;
                return (iret);
            }
            await cu.SendMessage(msg);
            return (iret);
        }
    }
}
