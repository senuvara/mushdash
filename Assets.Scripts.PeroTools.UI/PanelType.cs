using System;

namespace Assets.Scripts.PeroTools.UI
{
	[Flags]
	public enum PanelType
	{
		None = 0x0,
		Home = 0x1,
		First = 0x2,
		Second = 0x4,
		Third = 0x8,
		Fourth = 0x10,
		Popup = 0x20,
		All = 0x1F
	}
}
