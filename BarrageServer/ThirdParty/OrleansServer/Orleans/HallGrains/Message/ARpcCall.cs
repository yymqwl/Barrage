using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using Orleans;

namespace HallGrains
{
    public class ARpcCall : IRpcCall
    {
        public Type GetMessageType()
        {
            return GetType();
        }

        public virtual IMessage Handle(long userid, IMessage message  , IGrainFactory grainfactory)
        {
            return message;
        }

    }
}
