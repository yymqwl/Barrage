using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    
    public class ChatUser_Data
    {
        public string Id =string.Empty;
        public string Name = string.Empty;
        public string ChatRoomId = string.Empty;//房间号
    }

    public interface IChatUser : INetUser
    {
        Task<ChatUser_Data> GetChatUser_Data();
        Task SetChatUser_Data(ChatUser_Data cu);

    }
}
