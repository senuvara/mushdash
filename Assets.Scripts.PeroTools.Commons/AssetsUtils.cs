using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Commons
{
	public class AssetsUtils
	{
		public static readonly string streamingPathServer = $"file:///{Application.streamingAssetsPath}";

		public static void CreateFile(string str, string pathName, bool isCover = true)
		{
			string path = Application.dataPath + pathName;
			if (!File.Exists(path))
			{
				StreamWriter streamWriter = File.CreateText(path);
				streamWriter.Close();
				if (!isCover)
				{
					File.WriteAllText(path, str);
				}
			}
			if (isCover)
			{
				File.WriteAllText(path, str);
			}
		}

		public static string ProjectPathToFullPath(string pathInProject)
		{
			int num = pathInProject.IndexOf("Assets/", StringComparison.Ordinal);
			return Path.Combine(Application.dataPath, pathInProject.Substring(num + 7));
		}

		public static Encoding GetBytesEncodeType(byte[] data)
		{
			byte b = data[0];
			byte b2 = data[1];
			if (b >= 239)
			{
				if (b == 239 && b2 == 187)
				{
					return Encoding.UTF8;
				}
				if (b == 254 && b2 == byte.MaxValue)
				{
					return Encoding.BigEndianUnicode;
				}
				if (b == byte.MaxValue && b2 == 254)
				{
					return Encoding.Unicode;
				}
				return Encoding.Default;
			}
			return Encoding.Default;
		}
	}
}
