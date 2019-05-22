using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{

    
    public class IComponentAttribute:BaseAttribute
    {
        public readonly Type ContextType;//所属的Context
        public readonly int  PbId;//类型Id,也是Protobufid
        public IComponentAttribute(Type type,int pbid)
        {
            ContextType = type;
            PbId = pbid;
        }

    }

}
