using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using ProtoBuf.Meta;
using System.IO;

namespace GameFramework
{
    public static class Pb_NetHelper
    {
        
        public static readonly RuntimeTypeModel m_RuntimeTypeModel = RuntimeTypeModel.Create();

        public static byte[] Serialize<T>(T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                m_RuntimeTypeModel.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public static T Deserialize<T>(byte[] bys)
        {
            using (MemoryStream ms = new MemoryStream(bys))
            {
                return (T)m_RuntimeTypeModel.Deserialize(ms,null,typeof(T));
            }

        }
    }
}
