using GameFramework;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IHall
{
    public interface IChatRoom : IGrainWithIntegerKey
    {

        Task<IChatUser> EnterRoom(long id);

        Task<IChatUser> GetChatUser(long id);


        Task BroadCast(IMessage msg);

        Task ExitRoom(long  id);
        Task Update();
    }
}
