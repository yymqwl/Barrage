using Orleans;
using System;
using System.Threading.Tasks;

namespace IHall
{
    public interface IMainEntry : IGrainWithIntegerKey
    {
        Task<Guid> Enter();

        Task SayHello();

        Task Update(float t);

        Task<IUser> Join(long id);

        Task SendMsg(string msg);

        Task SubscribeAsync(IMainEntry_Obs viewer);
        Task UnsubscribeAsync(IMainEntry_Obs viewer);
    }
}
