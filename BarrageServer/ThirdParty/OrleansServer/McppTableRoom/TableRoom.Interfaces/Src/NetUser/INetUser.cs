using Orleans;
using System.Threading.Tasks;

namespace TableRoom
{
    public interface INetUser : IGrainWithStringKey
    {
        Task<bool> GetIsConnected();

        Task SetObserver(INetUserObserver obser);

        Task SendMessage(byte[] message);


        Task Ping();
    }
}
