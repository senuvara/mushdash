using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Commons
{
	public static class RandomUtils
	{
		public static int RandomEvent(IList<float> probabilities, params Action[] events)
		{
			int num = UnityEngine.Random.Range(0, int.MaxValue);
			float num2 = 0f;
			float num3 = probabilities.Sum();
			if (num3 != 1f)
			{
				float num4 = 1f - num3;
				for (int i = 0; i < probabilities.Count; i++)
				{
					float num5 = probabilities[i];
					probabilities[i] = num5 + num5 / num3 * num4;
				}
			}
			for (int j = 0; j < probabilities.Count; j++)
			{
				float num6 = probabilities[j];
				Action action = events[j];
				float num7 = num2 + num6 * 2.14748365E+09f;
				if ((float)num >= num2 && (float)num < num7)
				{
					if (j >= events.Length)
					{
						return -1;
					}
					action();
					return j;
				}
				num2 = num7;
			}
			return -1;
		}

		public static T Random<T>(this IList<T> array)
		{
			if (array.Count == 0)
			{
				return default(T);
			}
			int index = Random(0, array.Count);
			return array[index];
		}

		public static object LRandom(this IList array)
		{
			if (array.Count == 0)
			{
				return null;
			}
			int index = Random(0, array.Count);
			return array[index];
		}

		public static int Random(int min, int max)
		{
			return UnityEngine.Random.Range(min, max);
		}

		public static float Random(float min, float max)
		{
			return UnityEngine.Random.Range(min, max);
		}
	}
}
