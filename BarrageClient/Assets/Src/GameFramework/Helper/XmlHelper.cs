using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace GameFramework
{
    public static class XmlHelper
    {
        public static byte[] XmlSerialize(object sourceObj, Type type)
        {
            if (sourceObj != null)
            {
                type = type != null ? type : sourceObj.GetType();

                MemoryStream ms = new MemoryStream();
                System.Xml.Serialization.XmlSerializer xmlSerializer = new XmlSerializer(type);

                xmlSerializer.Serialize(ms, sourceObj);
                return ms.ToArray();
            }
            return null;
        }
        public static byte[] XmlSerialize<T>(T sourceObj)
        {
            Type tp = typeof(T);
            return XmlSerialize(sourceObj, tp);
        }

        public static string XmlSerialize_Str<T>(T sourceObj)
        {
            return Encoding.Default.GetString(XmlSerialize(sourceObj));
        }
        public static T XmlDeserialize<T>(byte[] bufs)
        {
            T result = default(T);
            using (MemoryStream ms = new MemoryStream(bufs))
            {
                Type tp = typeof(T);
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(tp);
                result = (T)xmlSerializer.Deserialize(ms);

            }


            return result;
        }

    }
}
