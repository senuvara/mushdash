using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace PeroTools.Commons
{
	public static class GcControl
	{
		[DllImport("__Internal")]
		private static extern void GC_disable();

		[DllImport("__Internal")]
		private static extern void GC_enable();

		public static void Enable()
		{
		}

		public static void Disable()
		{
		}

		public static void Collect()
		{
			GC.Collect();
			Debug.LogFormat("[GC] Collect!, Now count: {0}.", GC.CollectionCount(0));
		}
	}
}
