using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TableRoom
{
    public interface IMailBox : IGrainWithIntegerKey
    {
        Task SetObserver(IMailBoxObserver mbobser);

        Task SendMessage(byte[] message);

    }
}
