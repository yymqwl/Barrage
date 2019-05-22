using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public interface ISerialize
    {
        byte[] Serialize();

        void Deserialize(byte[] bytes);
    }
}
