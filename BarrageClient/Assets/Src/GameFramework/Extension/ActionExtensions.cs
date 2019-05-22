using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public static class ActionExtensions
    {
        public static bool InvokeGracefully(this Action selfAction)
        {
            if (null != selfAction)
            {
                selfAction();
                return true;
            }
            return false;
        }
    }
}
