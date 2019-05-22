using System.Collections;
using System.Collections.Generic;


namespace GameFramework
{
	public static class ArrayExtensions
	{

		public static T First<T>(this T[] source)
		{
			if (source.Length == 0)
				return default(T);

			return source[0];
		}

		public static T Last<T>(this T[] source)
		{
			if (source.Length == 0)
				return default(T);

			return source[source.Length - 1];
		}
        public static void Apply<T>(this T[] list, System.Func<T, T> fn)
        {
            for (var i = 0; i < list.Length; i++)
            {
                list[i] = fn(list[i]);
            }
        }



    }
}
