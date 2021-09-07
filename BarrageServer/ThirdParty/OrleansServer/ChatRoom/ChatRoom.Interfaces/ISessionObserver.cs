using Orleans;
using System;
using System.Collections.Generic;
using System.Text;

namespace IHall
{
    public interface ISessionObserver  : IGrainObserver
    {
        void ReceiveMessage(string msg);
    }
}
