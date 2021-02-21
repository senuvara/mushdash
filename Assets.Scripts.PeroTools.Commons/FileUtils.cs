using System.IO;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Commons
{
	public class FileUtils
	{
		public static byte[] ReadAllBytes(string pathName)
		{
			return File.ReadAllBytes(pathName);
		}

		public static bool Exists(string pathName)
		{
			return File.Exists(pathName);
		}

		public static void CopyFileTo(string source, string dest)
		{
			Directory.CreateDirectory(Path.GetDirectoryName(dest));
			File.Copy(source, dest);
			Debug.LogFormat("Copy file from {0} to {1}", source, dest);
		}
	}
}
