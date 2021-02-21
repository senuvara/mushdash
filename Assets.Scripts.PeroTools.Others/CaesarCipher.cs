using System;

namespace Assets.Scripts.PeroTools.Others
{
	public static class CaesarCipher
	{
		public static string Caesar(this string source, short shift)
		{
			int num = Convert.ToInt32('\uffff');
			int num2 = Convert.ToInt32('\0');
			char[] array = source.ToCharArray();
			for (int i = 0; i < array.Length; i++)
			{
				int num3 = Convert.ToInt32(array[i]) + shift;
				if (num3 > num)
				{
					num3 -= num;
				}
				else if (num3 < num2)
				{
					num3 += num;
				}
				array[i] = Convert.ToChar(num3);
			}
			return new string(array);
		}
	}
}
