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

        Task<long> GetId();

        Task Ping();//活跃
        Task<bool> CheckTimeOut();//超时
        Task SetSessionId(long id);
        Task<long> GetSessionId();
    }
}
