using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace GameFramework
{
    public static class CollectionExtensionMethods
    {

        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            T t = default(T);
            while(queue.TryDequeue(out t))
            {

            }

        }


        /// <summary>
        /// Remove and return a element in the list by index. Out of range indices are wrapped into range.
        /// </summary>
        /// <returns>
        /// A <see cref="T"/>
        /// </returns>
        public static T Pop<T> (this IList<T> list, int index)
        {
            while (index > list.Count)
                index -= list.Count;
            while (index < 0)
                index += list.Count;
            var o = list [index];
            list.RemoveAt (index);
            return o;
        }
    
        /// <summary>
        /// Return an element from a list by index. Out of range indices are wrapped into range.
        /// </summary>
        /// <returns>
        /// A <see cref="T"/>
        /// </returns>
        public static T Get<T> (this IList<T> list, int index)
        {
            while (index > list.Count)
                index -= list.Count;
            while (index < 0)
                index += list.Count;
            return list [index];
        }

        /// <summary>
        /// Remove and return a value from a dictionary.
        /// </summary>
        /// <param name='dict'>
        /// Dict.
        /// </param>
        /// <param name='key'>
        /// Key.
        /// </param>
        /// <typeparam name='TKey'>
        /// The 1st type parameter.
        /// </typeparam>
        /// <typeparam name='TValue'>
        /// The 2nd type parameter.
        /// </typeparam>
        public static TValue Pop<TKey,TValue> (this IDictionary<TKey,TValue> dict, TKey key)
        {
            var item = dict [key];
            dict.Remove (key);
            return item;
        }
    
        public static void Map<T> (this IList<T> list, System.Action<T> fn)
        {
            for (var i=0; i<list.Count; i++) {
                fn (list [i]);
            }
        }

        public static void Map<T> (this T[] list, System.Action<T> fn)
        {
            for (var i=0; i<list.Length; i++) {
                fn (list [i]);
            }
        }
    
        public static void Apply<T> (this IList<T> list, System.Func<T,T> fn)
        {
            for (var i=0; i<list.Count; i++) {
                list [i] = fn (list [i]);
            }
        }

        public static void Apply<T> (this T[] list, System.Func<T,T> fn)
        {
            for (var i=0; i<list.Length; i++) {
                list [i] = fn (list [i]);
            }
        }

        public static V GetValue<K,V>(this ConcurrentDictionary<K,V> dict,K key)
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
        public static void ConcurrentDictionary_Foreach<K, V>(this ConcurrentDictionary<K, V> dict,Action<K,V> act)
        {
            foreach (var kvp in dict)
            {
                act(kvp.Key,kvp.Value);
            }
        }
        //***********************dict

        //public delegate 
        public static void Dict_Foreach<TKey,TValue>(this Dictionary<TKey,TValue> dict, Action<TKey,TValue> act)
        {
            foreach(KeyValuePair<TKey,TValue>  kvp  in dict)
            {
                act(kvp.Key,kvp.Value);
            }
        }

        //******************************
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