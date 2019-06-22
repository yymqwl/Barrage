using GameFramework;
using IHall;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;

namespace HallGrains
{
    public interface IRpcCall
    {
        IMessage Handle( long userid, IMessage message, IGrainFactory grainfactory);

        Type GetMessageType();
    }
}
