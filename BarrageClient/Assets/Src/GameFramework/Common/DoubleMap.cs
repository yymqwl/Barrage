using System;
using System.Collections.Generic;

namespace GameFramework
{
    public class DoubleMap<K, V>
    {
        private readonly Dictionary<K, V> m_kv = new Dictionary<K, V>();
        private readonly Dictionary<V, K> m_vk = new Dictionary<V, K>();

        public DoubleMap()
        {
        }

        public DoubleMap(int capacity)
        {
            m_kv = new Dictionary<K, V>(capacity);
            m_vk = new Dictionary<V, K>(capacity);
        }

        public void ForEach(Action<K, V> action)
        {
            if (action == null)
            {
                return;
            }
            Dictionary<K, V>.KeyCollection keys = m_kv.Keys;
            foreach (K key in keys)
            {
                action(key, m_kv[key]);
            }
        }

        public List<K> Keys
        {
            get
            {
                return new List<K>(m_kv.Keys);
            }
        }

        public List<V> Values
        {
            get
            {
                return new List<V>(m_vk.Keys);
            }
        }

        public void Add(K key, V value)
        {
            if (key == null || value == null || m_kv.ContainsKey(key) || m_vk.ContainsKey(value))
            {
                return;
            }
            m_kv.Add(key, value);
            m_vk.Add(value, key);
        }

        public V GetValueByKey(K key)
        {
            if (key != null && m_kv.ContainsKey(key))
            {
                return m_kv[key];
            }
            return default(V);
        }

        public K GetKeyByValue(V value)
        {
            if (value != null && m_vk.ContainsKey(value))
            {
                return m_vk[value];
            }
            return default(K);
        }

        public void RemoveByKey(K key)
        {
            if (key == null)
            {
                return;
            }
            V value;
            if (!m_kv.TryGetValue(key, out value))
            {
                return;
            }

            m_kv.Remove(key);
            m_vk.Remove(value);
        }

        public void RemoveByValue(V value)
        {
            if (value == null)
            {
                return;
            }

            K key;
            if (!m_vk.TryGetValue(value, out key))
            {
                return;
            }

            m_kv.Remove(key);
            m_vk.Remove(value);
        }

        public void Clear()
        {
            m_kv.Clear();
            m_vk.Clear();
        }

        public bool ContainsKey(K key)
        {
            if (key == null)
            {
                return false;
            }
            return m_kv.ContainsKey(key);
        }

        public bool ContainsValue(V value)
        {
            if (value == null)
            {
                return false;
            }
            return m_vk.ContainsKey(value);
        }

        public bool Contains(K key, V value)
        {
            if (key == null || value == null)
            {
                return false;
            }
            return m_kv.ContainsKey(key) && m_vk.ContainsKey(value);
        }
    }
}