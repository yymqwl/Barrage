using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public abstract class ABehaviour : IBehaviour
    {
        public IBehaviour Parent { get; set; }

        public virtual bool Init()
        {
            return true;
        }

        public virtual bool Shut()
        {
            return true;
        }

        public virtual void Update()
        {
        }

        public T GetParent<T>() where T : class, IBehaviour
        {
            return Parent as T;
        }
    }
}
