using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;

namespace HallGrains
{
    public class ARpcCall : IRpcCall
    {
        public Type GetMessageType()
        {
            return GetType();
        }

        public virtual IMessage Handle(long userid, IMessage message)
        {
            return message;
        }
    }
}
