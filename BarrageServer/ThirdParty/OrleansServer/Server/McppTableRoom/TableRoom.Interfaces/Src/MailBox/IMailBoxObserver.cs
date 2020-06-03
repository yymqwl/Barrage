using Orleans;
using System;
using System.Collections.Generic;
using System.Text;

namespace TableRoom
{
    public interface IMailBoxObserver : IGrainObserver
    {
        void Receive(byte[] msg);
    }
}
