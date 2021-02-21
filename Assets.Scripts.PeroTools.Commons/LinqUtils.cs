using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.PeroTools.Commons
{
	public static class LinqUtils
	{
		public static T[] ToArray<T>(this IEnumerable<T> array)
		{
			return Enumerable.ToArray(array);
		}

		public static T[] ToArray<T>(this IList<T> array)
		{
			T[] array2 = new T[array.Count];
			for (int i = 0; i < array.Count; i++)
			{
				array2[i] = array[i];
			}
			return array2;
		}

		public static List<T> ToList<T>(this IEnumerable<T> array)
		{
			List<T> list = new List<T>();
			list.AddRange(array);
			return list;
		}

		public static List<T> Where<T>(this IList<T> array, Func<T, bool> predicate)
		{
			List<T> outArray = new List<T>();
			array.For(delegate(T element)
			{
				if (element != null && predicate(element))
				{
					outArray.Add(element);
				}
			});
			return outArray;
		}

		public static List<T> Where<T>(this IEnumerable<T> array, Func<T, bool> predicate)
		{
			List<T> list = new List<T>();
			foreach (T item in array)
			{
				if (item != null && predicate(item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		public static List<T> OfType<T>(this IList array)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < array.Count; i++)
			{
				object obj = array[i];
				T val = (T)obj;
				if (val != null)
				{
					list.Add(val);
				}
			}
			return list;
		}

		public static void For<T>(this IList<T> array, Action<T> predicate)
		{
			for (int i = 0; i < array.Count; i++)
			{
				predicate(array[i]);
			}
		}

		public static void For<T>(this IEnumerable<T> array, Action<T> predicate)
		{
			foreach (T item in array)
			{
				predicate(item);
			}
		}

		public static T Find<T>(this IList<T> array, Func<T, bool> predicate) where T : class
		{
			for (int i = 0; i < array.Count; i++)
			{
				T val = array[i];
				if (val != null && predicate(val))
				{
					return val;
				}
			}
			return (T)null;
		}

		public static int FindIndex<T>(this IList<T> array, Func<T, bool> predicate) where T : class
		{
			for (int i = 0; i < array.Count; i++)
			{
				T val = array[i];
				if (val != null && predicate(val))
				{
					return i;
				}
			}
			return -1;
		}

		public static int Count<T>(this IList<T> array, Func<T, bool> predicate)
		{
			int count = 0;
			array.For(delegate(T element)
			{
				if (element != null && predicate(element))
				{
					count++;
				}
			});
			return count;
		}

		public static List<T> Reverse<T>(this IList<T> array)
		{
			List<T> list = new List<T>();
			for (int num = array.Count - 1; num >= 0; num--)
			{
				T val = array[num];
				if (val != null)
				{
					list.Add(val);
				}
			}
			return list;
		}

		public static T Last<T>(this IList<T> array)
		{
			return array[array.Count - 1];
		}

		public static T First<T>(this IList<T> array)
		{
			return array[0];
		}

		public static bool Contains<T>(this IList<T> array, T predicate)
		{
			for (int i = 0; i < array.Count; i++)
			{
				if (array[i].Equals(predicate))
				{
					return true;
				}
			}
			return false;
		}

		public static bool Exists<T>(this IList<T> array, Func<T, bool> predicate)
		{
			for (int i = 0; i < array.Count; i++)
			{
				T val = array[i];
				if (val != null && predicate(val))
				{
					return true;
				}
			}
			return false;
		}

		public static List<T> Cast<T>(this IList array)
		{
			List<T> list = new List<T>();
			for (int i = 0; i < array.Count; i++)
			{
				object value = array[i];
				T item = (T)Convert.ChangeType(value, typeof(T));
				list.Add(item);
			}
			return list;
		}

		public static List<T> Cast<T>(this IList<T> array)
		{
			List<T> list = new List<T>();
			array.For(delegate(T element)
			{
				T item = (T)Convert.ChangeType(element, typeof(T));
				list.Add(item);
			});
			return list;
		}

		public static int Max<T>(this IList<T> array, Func<T, int> predicate)
		{
			int max = 0;
			array.For(delegate(T element)
			{
				int num = predicate(element);
				if (num > max)
				{
					max = num;
				}
			});
			return max;
		}

		public static float Max<T>(this IList<T> array, Func<T, float> predicate)
		{
			float max = 0f;
			array.For(delegate(T element)
			{
				float num = predicate(element);
				if (num > max)
				{
					max = num;
				}
			});
			return max;
		}

		public static decimal Max<T>(this IList<T> array, Func<T, decimal> predicate)
		{
			decimal max = 0m;
			array.For(delegate(T element)
			{
				decimal num = predicate(element);
				if (num > max)
				{
					max = num;
				}
			});
			return max;
		}

		public static int Min<T>(this IList<T> array, Func<T, int> predicate)
		{
			int min = 0;
			array.For(delegate(T element)
			{
				int num = predicate(element);
				if (num < min)
				{
					min = num;
				}
			});
			return min;
		}

		public static float Min<T>(this IList<T> array, Func<T, float> predicate)
		{
			float min = 0f;
			array.For(delegate(T element)
			{
				float num = predicate(element);
				if (num < min)
				{
					min = num;
				}
			});
			return min;
		}

		public static decimal Min<T>(this IList<T> array, Func<T, decimal> predicate)
		{
			decimal min = 0m;
			array.For(delegate(T element)
			{
				decimal num = predicate(element);
				if (num < min)
				{
					min = num;
				}
			});
			return min;
		}

		public static List<T1> Select<T1, T2>(this IList<T2> array, Func<T2, T1> predicate)
		{
			List<T1> list = new List<T1>();
			array.For(delegate(T2 element)
			{
				T1 item = predicate(element);
				list.Add(item);
			});
			return list;
		}

		public static List<T1> Select<T1, T2>(this IEnumerable<T2> array, Func<T2, T1> predicate)
		{
			List<T1> list = new List<T1>();
			array.For(delegate(T2 element)
			{
				T1 item = predicate(element);
				list.Add(item);
			});
			return list;
		}

		public static void SelectNoAlloc<T1, T2>(this IList<T2> array, List<T1> list, Func<T2, T1> predicate)
		{
			list.Clear();
			for (int i = 0; i < array.Count; i++)
			{
				T1 item = predicate(array[i]);
				list.Add(item);
			}
		}

		public static T Single<T>(this IList<T> array, Func<T, bool> predicate)
		{
			T value = default(T);
			array.For(delegate(T element)
			{
				if (predicate(element))
				{
					value = element;
				}
			});
			return value;
		}

		public static float Sum<T>(this IList<T> array, Func<T, float> predicate)
		{
			float sum = 0f;
			array.For(delegate(T element)
			{
				float num = predicate(element);
				sum += num;
			});
			return sum;
		}

		public static int Sum<T>(this IList<T> array, Func<T, int> predicate)
		{
			int sum = 0;
			array.For(delegate(T element)
			{
				int num = predicate(element);
				sum += num;
			});
			return sum;
		}
	}
}
