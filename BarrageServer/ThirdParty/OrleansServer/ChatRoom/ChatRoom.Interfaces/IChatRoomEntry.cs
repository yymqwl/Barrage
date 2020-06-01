using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IHall
{
    public  interface IChatRoomEntry : IEntry
    {
        Task SayHello();
        Task<bool> Init();

        Task<bool> ShutDown();
    }
}
