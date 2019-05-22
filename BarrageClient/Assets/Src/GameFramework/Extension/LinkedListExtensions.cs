using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public static class LinkedListExtensions
    {
        public static T Find_Lk_First<T>(this LinkedList<T> lk, Predicate<T> act)
        {

            foreach (T t in lk)
            {
                if (act(t))
                {
                    return t;
                }
            }
            return default(T);
        }
        public static void Foreach_Action<T>(this LinkedList<T> lk, Action<T> act)
        {
            for (LinkedListNode<T> current = lk.First; current != null; current = current.Next)
            {
                act(current.Value);
            }
        }
        public static void ForeachReverse_Action<T>(this LinkedList<T> lk, Action<T> act)
        {
            for (LinkedListNode<T> current = lk.Last; current != null; current = current.Previous)
            {
                act(current.Value);
            }
        }
    }
}
