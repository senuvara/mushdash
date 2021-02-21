using System.Runtime.InteropServices;

namespace GameLogic
{
	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct BUFF_TRIGGER
	{
		public const uint SAMPLE1 = 1000u;

		public const uint SAMPLE2 = 1001u;

		public const uint RECOVER = 1002u;
	}
}
