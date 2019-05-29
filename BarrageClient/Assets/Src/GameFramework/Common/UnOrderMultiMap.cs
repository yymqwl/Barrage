using System.Collections.Generic;
using System.Linq;

namespace GameFramework
{
    public class UnOrderMultiMap<T, K>
    {
        private readonly Dictionary<T, List<K>> m_Dict = new Dictionary<T, List<K>>();

        // 重用list
        private readonly Queue<List<K>> m_Queue = new Queue<List<K>>();

        public Dictionary<T, List<K>> GetDictionary()
        {
            return this.m_Dict;
        }

        public void Add(T t, K k)
        {
            List<K> list;
            this.m_Dict.TryGetValue(t, out list);
            if (list == null)
            {
                list = this.FetchList();
                this.m_Dict[t] = list;
            }
            list.Add(k);
        }

        public KeyValuePair<T, List<K>> First()
        {
            return this.m_Dict.First();
        }

        public int Count
        {
            get
            {
                return this.m_Dict.Count;
            }
        }

        private List<K> FetchList()
        {
            if (this.m_Queue.Count > 0)
            {
                List<K> list = this.m_Queue.Dequeue();
                list.Clear();
                return list;
            }
            return new List<K>();
        }

        private void RecycleList(List<K> list)
        {
            // 防止暴涨
            if (this.m_Queue.Count > 100)
            {
                return;
            }
            list.Clear();
            this.m_Queue.Enqueue(list);
        }

        public bool Remove(T t, K k)
        {
            List<K> list;
            this.m_Dict.TryGetValue(t, out list);
            if (list == null)
            {
                return false;
            }
            if (!list.Remove(k))
            {
                return false;
            }
            if (list.Count == 0)
            {
                this.RecycleList(list);
                this.m_Dict.Remove(t);
            }
            return true;
        }

        public bool Remove(T t)
        {
            List<K> list = null;
            this.m_Dict.TryGetValue(t, out list);
            if (list != null)
            {
                this.RecycleList(list);
            }
            return this.m_Dict.Remove(t);
        }

        /// <summary>
        /// 不返回内部的list,copy一份出来
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public K[] GetAll(T t)
        {
            List<K> list;
            this.m_Dict.TryGetValue(t, out list);
            if (list == null)
            {
                return new K[0];
            }
            return list.ToArray();
        }

        /// <summary>
        /// 返回内部的list
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public List<K> this[T t]
        {
            get
            {
                List<K> list;
                this.m_Dict.TryGetValue(t, out list);
                return list;
            }
        }

        public K GetOne(T t)
        {
            List<K> list;
            this.m_Dict.TryGetValue(t, out list);
            if (list != null && list.Count > 0)
            {
                return list[0];
            }
            return default(K);
        }

        public bool Contains(T t, K k)
        {
            List<K> list;
            this.m_Dict.TryGetValue(t, out list);
            if (list == null)
            {
                return false;
            }
            return list.Contains(k);
        }

        public bool ContainsKey(T t)
        {
            return this.m_Dict.ContainsKey(t);
        }

        public void Clear()
        {
            foreach (KeyValuePair<T, List<K>> keyValuePair in this.m_Dict)
            {
                this.RecycleList(keyValuePair.Value);
            }
            this.m_Dict.Clear();
        }
    }
}