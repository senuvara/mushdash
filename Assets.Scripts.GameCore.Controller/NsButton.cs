namespace Assets.Scripts.GameCore.Controller
{
	public enum NsButton : long
	{
		None = 0L,
		A = 1L,
		B = 2L,
		X = 4L,
		Y = 8L,
		StickL = 0x10L,
		StickR = 0x20L,
		L = 0x40L,
		R = 0x80L,
		ZL = 0x100L,
		ZR = 0x200L,
		Plus = 0x400L,
		Minus = 0x800L,
		Left = 0x1000L,
		Up = 0x2000L,
		Right = 0x4000L,
		Down = 0x8000L,
		StickLLeft = 0x10000L,
		StickLUp = 0x20000L,
		StickLRight = 0x40000L,
		StickLDown = 0x80000L,
		StickRLeft = 0x100000L,
		StickRUp = 0x200000L,
		StickRRight = 0x400000L,
		StickRDown = 0x800000L,
		LeftSL = 0x1000000L,
		LeftSR = 0x2000000L,
		RightSL = 0x4000000L,
		RightSR = 0x8000000L
	}
}
