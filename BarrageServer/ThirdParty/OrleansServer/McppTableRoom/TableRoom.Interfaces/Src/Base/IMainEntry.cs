using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public interface IMainEntry : IEntry
    {
        Task<IChatRoomEntry> GetIChatRoomEntry();
        Task<IServerTimer> GetIServerTimer();
    }
}
