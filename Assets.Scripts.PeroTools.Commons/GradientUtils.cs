using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Commons
{
	public class GradientUtils
	{
		public static float GetUnusedColorTime(Gradient gradient)
		{
			float time;
			for (time = 0f; gradient.colorKeys.ToList().Exists((GradientColorKey c) => c.time == time); time += 0.01f)
			{
			}
			return time;
		}

		public static float GetUnusedAlphaTime(Gradient gradient)
		{
			float time;
			for (time = 0f; gradient.alphaKeys.ToList().Exists((GradientAlphaKey c) => c.time == time); time += 0.01f)
			{
			}
			return time;
		}

		public static Gradient BlendGradient(Gradient l, Gradient r, float percent)
		{
			Gradient gradient = new Gradient();
			List<GradientColorKey> list = new List<GradientColorKey>();
			int num = Mathf.Max(l.colorKeys.Length, r.colorKeys.Length);
			for (int i = 0; i < num; i++)
			{
				GradientColorKey lColorKey = new GradientColorKey(Color.black, -1f);
				GradientColorKey rColorKey = new GradientColorKey(Color.black, -1f);
				if (i < l.colorKeys.Length)
				{
					lColorKey = l.colorKeys[i];
				}
				if (i < r.colorKeys.Length)
				{
					rColorKey = r.colorKeys[i];
				}
				if (Math.Abs(lColorKey.time - rColorKey.time) <= 0f && lColorKey.time != -1f)
				{
					Color col = Color.Lerp(lColorKey.color, rColorKey.color, percent);
					list.RemoveAll((GradientColorKey c) => c.time == lColorKey.time);
					list.Add(new GradientColorKey(col, lColorKey.time));
					continue;
				}
				if (lColorKey.time != -1f)
				{
					lColorKey.color = Color.Lerp(lColorKey.color, r.Evaluate(lColorKey.time), percent);
					list.RemoveAll((GradientColorKey c) => c.time == lColorKey.time);
					list.Add(lColorKey);
				}
				if (rColorKey.time != -1f)
				{
					rColorKey.color = Color.Lerp(l.Evaluate(rColorKey.time), rColorKey.color, percent);
					list.RemoveAll((GradientColorKey c) => c.time == rColorKey.time);
					list.Add(rColorKey);
				}
			}
			gradient.colorKeys = list.ToArray();
			List<GradientAlphaKey> list2 = new List<GradientAlphaKey>();
			num = Mathf.Max(l.alphaKeys.Length, r.alphaKeys.Length);
			for (int j = 0; j < num; j++)
			{
				GradientAlphaKey lAlphaKey = new GradientAlphaKey(1f, -1f);
				GradientAlphaKey rAlphaKey = new GradientAlphaKey(1f, -1f);
				if (j < l.alphaKeys.Length)
				{
					lAlphaKey = l.alphaKeys[j];
				}
				if (j < r.alphaKeys.Length)
				{
					rAlphaKey = r.alphaKeys[j];
				}
				if (Math.Abs(lAlphaKey.time - rAlphaKey.time) <= 0f && lAlphaKey.time != -1f)
				{
					float alpha = Mathf.Lerp(lAlphaKey.alpha, rAlphaKey.alpha, percent);
					list2.RemoveAll((GradientAlphaKey c) => c.time == lAlphaKey.time);
					list2.Add(new GradientAlphaKey(alpha, lAlphaKey.time));
					continue;
				}
				if (lAlphaKey.time != -1f)
				{
					ref GradientAlphaKey reference = ref lAlphaKey;
					float alpha2 = lAlphaKey.alpha;
					Color color = r.Evaluate(lAlphaKey.time);
					reference.alpha = Mathf.Lerp(alpha2, color.a, percent);
					list2.RemoveAll((GradientAlphaKey c) => c.time == lAlphaKey.time);
					list2.Add(lAlphaKey);
				}
				if (rAlphaKey.time != -1f)
				{
					ref GradientAlphaKey reference2 = ref rAlphaKey;
					Color color2 = l.Evaluate(rAlphaKey.time);
					reference2.alpha = Mathf.Lerp(color2.a, rAlphaKey.alpha, percent);
					list2.RemoveAll((GradientAlphaKey c) => c.time == rAlphaKey.time);
					list2.Add(rAlphaKey);
				}
			}
			gradient.alphaKeys = list2.ToArray();
			return gradient;
		}
	}
}
