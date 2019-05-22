using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public static class GameFrameworkActionExtensions
    {


        public static bool InvokeGracefully(this GameFrameworkAction selfAction)
        {
            if (null != selfAction)
            {
                selfAction();
                return true;
            }
            return false;
        }
        public static bool InvokeGracefully<T>(this GameFrameworkAction<T> selfAction, T t)
        {
            if (null != selfAction)
            {
                selfAction(t);
                return true;
            }
            return false;
        }

        public static bool InvokeGracefully<T>(this Action<T> selfAction, T t)
        {
            if (null != selfAction)
            {
                selfAction(t);
                return true;
            }
            return false;
        }
        public static bool InvokeGracefully<T>(this Predicate<T> selfAction, T t)
        {
            if (null != selfAction)
            {
                return selfAction(t);
            }
            return false;
        }
        public static bool InvokeGracefully<T1, T2>(this Action<T1, T2> selfAction, T1 t1, T2 t2)
        {
            if (null != selfAction)
            {
                selfAction(t1, t2);
                return true;
            }
            return false;
        }

        public static bool InvokeGracefully(this Func<bool> selfAction)
        {
            if (null != selfAction)
            {
                return selfAction();
            }
            return false;
        }
    }
}
