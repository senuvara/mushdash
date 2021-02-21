using System;

namespace ToolGood.Words
{
	internal class NumberConventer
	{
		public static decimal ChnToArab(string ChnNum)
		{
			decimal num = 0m;
			string text = ChnNum;
			bool flag = false;
			if (ChnNum.IndexOf("负") != -1)
			{
				flag = true;
				text = text.Replace("负", string.Empty);
			}
			text = text.Replace("点", ".");
			text = text.Replace("分", string.Empty);
			text = text.Replace("角", string.Empty);
			text = text.Replace("元", ".");
			text = text.Replace("拾", "十");
			text = text.Replace("佰", "百");
			text = text.Replace("仟", "千");
			text = text.Replace("萬", "万");
			text = text.Replace("億", "亿");
			string[] array = text.Split('.');
			if (array.Length > 1)
			{
				num = GetArabDotPart(array[1]);
			}
			string[] array2 = array[0].Split(new char[1]
			{
				'亿'
			}, StringSplitOptions.RemoveEmptyEntries);
			if (array2.Length == 3)
			{
				num += HandlePart(array2[0]) * new decimal(10000000000000000L) + HandlePart(array2[1]) * 100000000m + HandlePart(array2[2]);
			}
			else if (array2.Length == 2)
			{
				num += HandlePart(array2[0]) * 100000000m + HandlePart(array2[1]);
			}
			else if (array2.Length == 1)
			{
				num += HandlePart(array2[0]);
			}
			if (flag)
			{
				return -num;
			}
			return num;
		}

		private static decimal HandlePart(string num)
		{
			decimal result = 0m;
			string[] array = num.Split(new char[1]
			{
				'万'
			}, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				result += Convert.ToDecimal(GetArabThousandPart(array[array.Length - i - 1])) * Convert.ToDecimal(Math.Pow(10000.0, Convert.ToDouble(i)));
			}
			return result;
		}

		private static decimal GetArabDotPart(string dotpart)
		{
			decimal num = 0.00m;
			string text = "0.";
			for (int i = 0; i < dotpart.Length; i++)
			{
				text += switchNum(dotpart[i].ToString());
			}
			return Convert.ToDecimal(text);
		}

		private static int GetArabThousandPart(string number)
		{
			string text = number;
			if (text == "零")
			{
				return 0;
			}
			if (text != string.Empty && text[0].ToString() == "十")
			{
				text = "一" + text;
			}
			text = text.Replace("零", string.Empty);
			int num = 0;
			int num2 = text.IndexOf("千");
			if (num2 != -1)
			{
				num += switchNum(text.Substring(0, num2)) * 1000;
				text = text.Remove(0, num2 + 1);
			}
			num2 = text.IndexOf("百");
			if (num2 != -1)
			{
				num += switchNum(text.Substring(0, num2)) * 100;
				text = text.Remove(0, num2 + 1);
			}
			num2 = text.IndexOf("十");
			if (num2 != -1)
			{
				num += switchNum(text.Substring(0, num2)) * 10;
				text = text.Remove(0, num2 + 1);
			}
			if (text != string.Empty)
			{
				num += switchNum(text);
			}
			return num;
		}

		private static int switchNum(string n)
		{
			switch (n)
			{
			case "壹":
				return 1;
			case "贰":
				return 2;
			case "叁":
				return 3;
			case "肆":
				return 4;
			case "伍":
				return 5;
			case "陆":
				return 6;
			case "柒":
				return 7;
			case "捌":
				return 8;
			case "玖":
				return 9;
			case "零":
				return 0;
			case "一":
				return 1;
			case "二":
				return 2;
			case "两":
				return 2;
			case "三":
				return 3;
			case "四":
				return 4;
			case "五":
				return 5;
			case "六":
				return 6;
			case "七":
				return 7;
			case "八":
				return 8;
			case "九":
				return 9;
			default:
				return -1;
			}
		}
	}
}
