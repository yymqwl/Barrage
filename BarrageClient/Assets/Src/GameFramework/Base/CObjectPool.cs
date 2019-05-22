using System;
using System.Collections.Generic;

namespace GameFramework
{
    public class CObjectPool<T>
    {
        private readonly Dictionary<Type, Queue<T>> m_Dict = new Dictionary<Type, Queue<T>>();

        public CObjectPool()
        {
        }
        public T Fetch(Type type)
        {
            Queue<T> queue;
            if (!this.m_Dict.TryGetValue(type, out queue))
            {
                queue = new Queue<T>();
                this.m_Dict.Add(type, queue);
            }
            T obj;
            if (queue.Count > 0)
            {
                obj = queue.Dequeue();
            }
            else
            {
                obj = (T)Activator.CreateInstance(type);
            }

            return obj;
        }
        
        public T2 Fetch<T2>()where T2:T
        {
            T2 t = (T2)this.Fetch(typeof(T2));
            return t;
        }

        public void Recycle(T obj)
        {
            Type type = obj.GetType();
            Queue<T> queue;
            if (!this.m_Dict.TryGetValue(type, out queue))
            {
                queue = new Queue<T>();
                this.m_Dict.Add(type, queue);
            }
            queue.Enqueue(obj);
        }
    }
}