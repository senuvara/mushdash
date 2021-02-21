namespace Assets.Scripts.PeroTools.Commons
{
	public static class StringUtils
	{
		public static string LastAfter(this string str, char split)
		{
			string[] array = str.Split(split);
			return array[array.Length - 1];
		}

		public static string BeginBefore(this string str, char split)
		{
			return str.Split(split)[0];
		}

		public static string FirstToLower(this string str)
		{
			return str.Substring(0, 1).ToLower() + str.Substring(1);
		}

		public static string FirstToUpper(this string str)
		{
			return str.Substring(0, 1).ToUpper() + str.Substring(1);
		}

		public static bool IsUpper(this char c)
		{
			if (c >= 'A' && c <= 'Z')
			{
				return true;
			}
			return false;
		}

		public static string ClassToName(this string str)
		{
			string text = (string)str.Clone();
			int num = 0;
			for (int i = 0; i < str.Length; i++)
			{
				char c = str[i];
				if (c.IsUpper() && i != 0)
				{
					text = text.Insert(num++, " ");
				}
				num++;
			}
			return text;
		}
	}
}
