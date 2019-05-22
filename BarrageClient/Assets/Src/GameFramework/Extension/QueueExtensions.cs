using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    public static class QueueExtensions
    {
        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            T t = default(T);
            while (queue.TryDequeue(out t))
            {

            }

        }
    }
}
