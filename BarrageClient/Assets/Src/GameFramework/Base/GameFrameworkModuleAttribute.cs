using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GameFrameworkModuleAttribute:BaseAttribute
    {
        /*
        public int Priority { get; }
        public GameFrameworkModuleAttribute(int priority =0)
        {
            this.Priority = priority;
        }*/

    }
}
