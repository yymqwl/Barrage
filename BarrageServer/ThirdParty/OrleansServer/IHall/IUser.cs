using Orleans;
using System.Threading.Tasks;

namespace IHall
{
    public interface IUser : IGrainWithIntegerKey
    {
        Task SetName(string name);
        Task Say(string msg);
        Task Update(float t);

        Task<bool> Connected();
    }
}
