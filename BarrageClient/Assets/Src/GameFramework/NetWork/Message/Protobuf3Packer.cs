using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class Protobuf3Packer : IMessagePacker
    {
        public object DeserializeFrom(Type type, MemoryStream stream)
        {
            return Pb_3Helper.Deserialize(type, stream);
        }

        public void SerializeTo(object obj, MemoryStream stream)
        {
            Pb_3Helper.Serialize(obj, stream);
        }
    }
}
