using UnityEngine;

namespace Assets.Scripts.PeroTools.Commons
{
	public static class MathUtils
	{
		public static float Round(float value, int num)
		{
			float num2 = Mathf.Pow(0.1f, num);
			return (float)Mathf.RoundToInt(value / num2) * num2;
		}

		public static float Floor(float value, int num)
		{
			float num2 = Mathf.Pow(0.1f, num);
			return (float)Mathf.FloorToInt(value / num2) * num2;
		}

		public static float Ceil(float value, int num)
		{
			float num2 = Mathf.Pow(0.1f, num);
			return (float)Mathf.CeilToInt(value / num2) * num2;
		}

		public static bool IsPointInTrapezoid(Vector2[] polygon, float x, float y)
		{
			Vector2 vector = polygon[0];
			Vector2 vector2 = polygon[1];
			Vector2 vector3 = polygon[2];
			Vector2 vector4 = polygon[3];
			if (x > vector4.x && x < vector4.x + (vector3.x - vector4.x) && y > vector.y && y < vector4.y)
			{
				return true;
			}
			if (x > vector.x && x < vector2.x && y > vector.y && y < vector4.y)
			{
				if (x < vector4.x && (y - vector.y) / (x - vector.x) < (vector4.y - vector.y) / (vector4.x - vector.x))
				{
					return true;
				}
				if (x > vector3.x && (y - vector2.y) / (x - vector2.x) > (vector3.y - vector2.y) / (vector3.x - vector2.x))
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}
}
