using Orleans;
using System.Threading.Tasks;

namespace IHall
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChatUser : IGrainWithIntegerKey
    {
        Task SetName(string name);
        Task Say(string msg);

        Task<bool> Connected();
    }
}
