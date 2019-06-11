using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
        public static void Serialize(object message, MemoryStream stream)
        {
            ((Google.Protobuf.IMessage)message).WriteTo(stream);
        }
        public static object Deserialize(Type type, byte[] bytes, int index, int count)
        {

            object message = Activator.CreateInstance(type);
            ((Google.Protobuf.IMessage)message).MergeFrom(bytes, index, count);
            ISupportInitialize iSupportInitialize = message as ISupportInitialize;
            if (iSupportInitialize == null)
            {
                return message;
            }
            iSupportInitialize.EndInit();
            return message;
        }
        public static object Deserialize(object instance, byte[] bytes, int index, int count)
        {
            object message = instance;
            ((Google.Protobuf.IMessage)message).MergeFrom(bytes, index, count);
            ISupportInitialize iSupportInitialize = message as ISupportInitialize;
            if (iSupportInitialize == null)
            {
                return message;
            }
            iSupportInitialize.EndInit();
            return message;
        }
        public static object Deserialize(Type type, MemoryStream stream)
        {
            object message = Activator.CreateInstance(type);
            ((Google.Protobuf.IMessage)message).MergeFrom(stream.GetBuffer(), (int)stream.Position, (int)(stream.Length- stream.Position) );
            ISupportInitialize iSupportInitialize = message as ISupportInitialize;
            if (iSupportInitialize == null)
            {
                return message;
            }
            iSupportInitialize.EndInit();
            return message;
        }
        public static object Deserialize(object message, MemoryStream stream)
        {
            // 这个message可以从池中获取，减少gc
            ((Google.Protobuf.IMessage)message).MergeFrom(stream.GetBuffer(), (int)stream.Position, (int)(stream.Length - stream.Position));
            ISupportInitialize iSupportInitialize = message as ISupportInitialize;
            if (iSupportInitialize == null)
            {
                return message;
            }
            iSupportInitialize.EndInit();
            return message;
        }


        public static T Deserialize<T>(byte[] data) where T : IMessage<T>
        {
            object message = Activator.CreateInstance<T>();
            ((Google.Protobuf.IMessage)message).MergeFrom(data);
            return (T)message;
        }


    }
}
