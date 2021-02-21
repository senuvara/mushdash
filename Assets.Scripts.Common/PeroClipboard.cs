using UnityEngine;

namespace Assets.Scripts.Common
{
	public class PeroClipboard
	{
		public static void Copy(string text)
		{
			GUIUtility.systemCopyBuffer = text;
		}
	}
}
