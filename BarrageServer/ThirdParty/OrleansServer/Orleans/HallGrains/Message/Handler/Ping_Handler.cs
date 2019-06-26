using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using Orleans;

namespace HallGrains
{
    public class Ping_Handler : ARpcCall
    {
        public override IMessage Handle(long userid, IMessage message, IGrainFactory grainfactory)
        {
            
            return message;
        }
    }
}
