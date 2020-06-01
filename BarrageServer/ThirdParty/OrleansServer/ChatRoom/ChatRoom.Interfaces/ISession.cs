using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IHall
{
    public interface ISession : IGrainWithIntegerKey
    {
        Task SetISessionObserver(ISessionObserver isobs);

        Task SayHello();
    }
}
