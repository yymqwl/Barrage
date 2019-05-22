using System;
using System.Collections.Generic;
using System.Text;
using Google.Protobuf;

namespace GameFramework
{
    /// <summary>
    /// Protobuf序列化与反序列化
    /// </summary>
    public static class Pb_3Helper
    {
        public static byte[] Serialize<T>(T obj) where T : IMessage<T>
        {
            return obj.ToByteArray();
        }
        public static T Deserialize<T>(byte[] data) where T : IMessage<T>
        {
            object message = Activator.CreateInstance<T>();
            ((IMessage)message).MergeFrom(data);
            return (T)message;
        }
    }
}
