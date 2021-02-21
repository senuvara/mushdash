using System;
using System.Collections.Generic;

namespace Assets.Scripts.PeroTools.Commons
{
	public static class ArrayUtils
	{
		public static void Remove<T>(this Stack<T> stack, Predicate<T> value)
		{
			List<T> list = stack.ToList();
			list.RemoveAll(value);
			list.Reverse();
			stack.Clear();
			foreach (T item in list)
			{
				stack.Push(item);
			}
		}

		public static bool Contains<T>(this T[] array, T value)
		{
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].Equals(value))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static int IndexOf<T>(this T[] array, T value)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Equals(value))
				{
					return i;
				}
			}
			return -1;
		}

		public static T[] Insert<T>(this T[] array, int index, T value)
		{
			if (index > array.Length - 1)
			{
				index = array.Length - 1;
			}
			T[] array2 = new T[array.Length + 1];
			for (int i = 0; i < array.Length + 1; i++)
			{
				if (i > index)
				{
					array2[i] = array[i - 1];
				}
				else if (i < index)
				{
					array2[i] = array[i];
				}
			}
			array2[index] = value;
			return array2;
		}

		public static T[] Add<T>(this T[] array, T value)
		{
			T[] array2 = new T[array.Length + 1];
			for (int i = 0; i < array.Length; i++)
			{
				array2[i] = array[i];
			}
			array2[array.Length] = value;
			return array2;
		}

		public static void Add<T>(ref T[] array, T value)
		{
			array = array.Add(value);
		}

		public static T[] Remove<T>(this T[] array, int index)
		{
			T[] array2 = new T[array.Length - 1];
			for (int i = 0; i < index; i++)
			{
				array2[i] = array[i];
			}
			for (int j = index; j < array2.Length; j++)
			{
				array2[j] = array[j + 1];
			}
			return array2;
		}

		public static void Remove<T>(ref T[] array, int index)
		{
			array = array.Remove(index);
		}

		public static T[] Remove<T>(this T[] array, T value)
		{
			int num = Array.IndexOf(array, value);
			return (num >= 0) ? array.Remove(num) : array;
		}

		public static void Remove<T>(ref T[] array, T value)
		{
			int num = Array.IndexOf(array, value);
			if (num < 0)
			{
				array = array.Remove(num);
			}
		}

		public static void MoveBack<T>(this T[] array, T value)
		{
			int num = Array.IndexOf(array, value);
			if (num > 0)
			{
				T val = array[num - 1];
				array[num - 1] = value;
				array[num] = val;
			}
		}

		public static void MoveForw<T>(this T[] array, T value)
		{
			int num = Array.IndexOf(array, value);
			if (num < array.Length - 1 && num >= 0)
			{
				T val = array[num + 1];
				array[num + 1] = value;
				array[num] = val;
			}
		}

		public static bool CanMoveBack<T>(this T[] array, T value)
		{
			return Array.IndexOf(array, value) > 0;
		}

		public static bool CanMoveForw<T>(this T[] array, T value)
		{
			int num = Array.IndexOf(array, value);
			return num < array.Length - 1 && num >= 0;
		}
	}
}
