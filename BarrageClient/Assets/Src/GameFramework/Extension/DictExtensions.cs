using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  GameFramework
{
    public static class DictExtensions
    {

        /// </typeparam>
        public static TValue Pop<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            var item = dict[key];
            dict.Remove(key);
            return item;
        }



        public static V GetValue<K, V>(this ConcurrentDictionary<K, V> dict, K key)
        {
            V v = default(V);
            dict.TryGetValue(key, out v);
            return v;
        }
        public static V GetValue<K, V>(this Dictionary<K, V> dict, K key)
        {
            V v = default(V);
            dict.TryGetValue(key, out v);
            return v;
        }
        public static V Remove<K, V>(this ConcurrentDictionary<K, V> dict, K key)
        {
            V v = default(V);
            dict.TryRemove(key, out v);
            return v;
        }
        public static void ConcurrentDictionary_Foreach<K, V>(this ConcurrentDictionary<K, V> dict, Action<K, V> act)
        {
            foreach (var kvp in dict)
            {
                act(kvp.Key, kvp.Value);
            }
        }
        //***********************dict

        //public delegate 
        public static void Dict_Foreach<TKey, TValue>(this Dictionary<TKey, TValue> dict, Action<TKey, TValue> act)
        {
            foreach (KeyValuePair<TKey, TValue> kvp in dict)
            {
                act(kvp.Key, kvp.Value);
            }
        }

        //******************************
    }
}
