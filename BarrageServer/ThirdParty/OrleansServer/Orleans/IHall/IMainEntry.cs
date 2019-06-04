using Orleans;
using System;
using System.Threading.Tasks;

namespace IHall
{
    public interface IMainEntry : IGrainWithIntegerKey
    {
        Task<IChatRoom> GetIChatRoom();

    }
}
