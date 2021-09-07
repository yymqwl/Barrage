using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameFramework
{
    public abstract class RecycleObjectFactory<T>
    {
        public abstract T CreateObject();
       
    }
}
