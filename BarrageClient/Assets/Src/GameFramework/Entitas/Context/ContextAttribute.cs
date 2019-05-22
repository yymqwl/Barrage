using System;
using System.Collections.Generic;
using System.Text;

namespace GameFramework
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ContextAttribute:BaseAttribute
    {
        public readonly string ContextName;
        public ContextAttribute(string contextName)
        {
            ContextName = contextName;
        }
    }
}
