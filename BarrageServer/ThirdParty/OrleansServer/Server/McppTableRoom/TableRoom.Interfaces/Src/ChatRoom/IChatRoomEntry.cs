using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TableRoom;

namespace TableRoom
{
    public  interface IChatRoomEntry : IEntry
    {
        Task<int> Join(ChatUser_Data user);

        Task<int> Exit(string id);

        Task<int> SendMessage(string Id,byte[] msg);
    }
}
