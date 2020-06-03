using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public enum EChatRoom_State
    {
        InActive = 0,
        Active = 1,
    }
    public class ChatRoomInfo
    {
        public string Id;
        public List<ChatUser_Data> Ls_ChatUser = new List<ChatUser_Data>();
        public EChatRoom_State ChatRoom_State;
    }
    public interface IChatRoom : IGrainWithStringKey
    {
        Task<bool> Init();
        Task<bool> ShutDown();
        Task<int> Join(ChatUser_Data user);
        Task<int> Exit(string id);
        Task Update(float t);

        Task SendMessage(byte[] msg);//默认发给所有房间玩家
    }
}
