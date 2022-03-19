#region Includes
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
#endregion

namespace TS.LocalizationSystem.Extensions
{
	public static class CollectionExtensions
	{
		#region Variables

		private const string NULL = "NULL";
		private const string EMPTY = "EMPTY";
		private const string LOG = "{0}: {1}";

		#endregion

		public static string Print<T>(this IList<T> list, bool pretty = false)
		{
			string result = "";

			if (list == null)
			{
				result = "NULL";
			}
			else if (list.Count == 0)
			{
				result = "EMPTY";
			}
			else
			{
				for (int i = 0; i < list.Count; i++)
				{
					result += string.Format(LOG, i, list[i].ToString());

					if (i < list.Count - 1)
					{
						result += (pretty ? "\n" : ", ");
					}
				}
			}

			return result;
		}
		public static string Print<TKey, TElement>(this IDictionary<TKey, TElement> dic, bool pretty = false)
		{
			string result = "";

			if (dic == null)
			{
				result = "NULL";
			}
			else if (dic.Count == 0)
			{
				result = "EMPTY";
			}
			else
			{
				int count = 0;

				foreach (TKey key in dic.Keys)
				{
					result += string.Format(LOG, key, dic[key].ToString());

					if (count < dic.Count - 1)
					{
						result += (pretty ? "\n" : ", ");
					}

					count++;
				}
			}

			return result;
		}
		public static T[] ToArray<T>(this IList<T> list)
		{
			T[] result = null;

			if (list != null && list.Count > 0)
			{
				result = new T[list.Count];

				for (int i = 0; i < list.Count; i++)
				{
					result[i] = list[i];
				}
			}

			return result;
		}
		public static List<T> ToList<T>(this T[] array)
		{
			List<T> result = new List<T>();

			if (array != null && array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					result.Add(array[i]);
				}
			}

			return result;
		}
		public static int RandomIndex<T>(this T[] array)
		{
			return Random.Range(0, array.Length);
		}
		public static T RandomElement<T>(this T[] array)
		{
			return array.IsNullOrEmpty() ? default(T) : array[RandomIndex(array)];
		}
		public static int RandomIndex(this IList list)
		{
			return Random.Range(0, list.Count);
		}
		public static T RandomElement<T>(this List<T> list)
		{
			return list.IsNullOrEmpty() ? default(T) : list[RandomIndex(list)];
		}
		public static bool IsNullOrEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count == 0;
		}
		public static bool IsNullOrEmpty<T>(this T[] array)
		{
			return array == null || array.Length == 0;
		}
		public static IOrderedEnumerable<T> Shuffle<T>(this IOrderedEnumerable<T> target, System.Random random)
		{
			return target.OrderBy(x => (random.Next()));
		}
	}
}