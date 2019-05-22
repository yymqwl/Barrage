using System.Collections;
using System.Collections.Generic;

namespace GameFramework
{
	public static class ListExtensions
	{
        public static void Map<T>(this IList<T> list, System.Action<T> fn)
        {
            for (var i = 0; i < list.Count; i++)
            {
                fn(list[i]);
            }
        }

        public static void Map<T>(this T[] list, System.Action<T> fn)
        {
            for (var i = 0; i < list.Length; i++)
            {
                fn(list[i]);
            }
        }

        public static void Apply<T>(this IList<T> list, System.Func<T, T> fn)
        {
            for (var i = 0; i < list.Count; i++)
            {
                list[i] = fn(list[i]);
            }
        }

        /// <summary>
        /// Remove and return a element in the list by index. Out of range indices are wrapped into range.
        /// </summary>
        /// <returns>
        /// A <see cref="T"/>
        /// </returns>
        public static T Pop<T>(this IList<T> list, int index)
        {
            while (index > list.Count)
                index -= list.Count;
            while (index < 0)
                index += list.Count;
            var o = list[index];
            list.RemoveAt(index);
            return o;
        }


        /// <summary>
        /// Return an element from a list by index. Out of range indices are wrapped into range.
        /// </summary>
        /// <returns>
        /// A <see cref="T"/>
        /// </returns>
        public static T Get<T>(this IList<T> list, int index)
        {
            while (index > list.Count)
                index -= list.Count;
            while (index < 0)
                index += list.Count;
            return list[index];
        }
        public static T First<T>(this IList<T> source)
		{
			if (source.Count == 0)
				return default(T);

			return source[0];
		}

		public static T Last<T>(this IList<T> source)
		{
			if (source.Count == 0)
				return default(T);

			return source[source.Count - 1];
		}

		public static T PopFirst<T>(this IList<T> source)
		{
			if (source.Count == 0)
				return default(T);

			T value = source.First();
			source.RemoveAt(0);

			return value;
		}

		public static T PopLast<T>(this IList<T> source)
		{
			if (source.Count == 0)
				return default(T);

			T value = source.Last();
			source.RemoveAt(source.Count - 1);

			return value;
		}



		public static List<T> Clone<T>(this List<T> source)
		{
			List<T> newList = new List<T>();

			for (int i = 0; i < source.Count; i++)
			{
				newList.Add(source[i]);
			}

			return newList;
		}


		public static void SwitchPlaces<T>(this IList<T> list, int indexOne, int indexTwo)
		{
			if (indexOne < 0 || indexTwo < 0 || indexOne == indexTwo || indexOne >= list.Count || indexTwo >= list.Count)
				return;

			T temp = list[indexOne];
			list[indexOne] = list[indexTwo];
			list[indexTwo] = temp;
		}
	}
}