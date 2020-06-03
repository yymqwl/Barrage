using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public interface INetUserEntry :IEntry
    {
        Task<INetUser> GetINetUser(string id);

        Task CreateNetUser(string id, INetUserObserver netUserObservers);

        Task<int> SendMessage(string id, byte[] message);


        Task<bool> IsConnected(string id);
    }
}
