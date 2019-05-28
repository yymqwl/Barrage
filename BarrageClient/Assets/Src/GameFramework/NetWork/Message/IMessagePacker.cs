using System;
using System.IO;

namespace GameFramework
{
    public interface IMessagePacker
    {
        void SerializeTo(object obj, MemoryStream stream);

        object DeserializeFrom(Type type, MemoryStream stream);
    }
}
