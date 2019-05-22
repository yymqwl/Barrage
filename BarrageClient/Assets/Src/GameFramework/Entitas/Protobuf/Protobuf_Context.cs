using System.IO;
using ProtoBuf.Meta;
using ProtoBuf;

namespace GameFramework
{
    public class Protobuf_Context
    {
        protected  RuntimeTypeModel m_RuntimeTypeModel = TypeModel.Create();

        public RuntimeTypeModel RuntimeTypeModel => m_RuntimeTypeModel;

        public  byte[] Serialize<T>(T obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                m_RuntimeTypeModel.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public  T Deserialize<T>(byte[] bys)
        {
            using (MemoryStream ms = new MemoryStream(bys))
            {
                return (T)m_RuntimeTypeModel.Deserialize(ms, null, typeof(T));
            }

        }


    }
}
