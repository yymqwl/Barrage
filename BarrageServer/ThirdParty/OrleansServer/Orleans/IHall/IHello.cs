using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IHall
{
    public interface IHello : IGrainWithIntegerKey
    {
        Task<string> SayHello();
    }
}
