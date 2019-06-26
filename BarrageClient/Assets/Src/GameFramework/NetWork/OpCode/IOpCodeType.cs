using System;
using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{
    public interface IOpCodeType
    {
        ushort GetOpcode(Type type);
        Type GetType(ushort opcode);
        IMessage GetInstance(ushort opcode);
    }
}