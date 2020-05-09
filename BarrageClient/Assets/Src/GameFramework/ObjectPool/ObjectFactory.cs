using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public abstract class ObjectFactory<T>
    {
        public abstract T CreateObj();
        public abstract void ReleaseObj(T t);
    }
}
