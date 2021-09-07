using GameFramework;
using IHall;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HallGrains
{
    public interface IRpcCall
    {
        Task<IMessage> Handle( long userid, IMessage message, IGrainFactory grainfactory);

        Type GetRequestType();
        Type GetResponeType();
    }
}
