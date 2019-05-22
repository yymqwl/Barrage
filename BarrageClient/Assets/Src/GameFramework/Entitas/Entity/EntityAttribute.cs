using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public class EntityAttribute : BaseAttribute
    {
        public readonly Type ContextType;//所属的Context
        public readonly int PbId;//类型Id,也是Protobufid
        public EntityAttribute(Type type, int pbid)
        {
            ContextType = type;
            PbId = pbid;
        }

    }
}
