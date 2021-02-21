using Assets.Scripts.PeroTools.GeneralLocalization;
using UnityEngine.UI;

namespace PeroTools.GeneralLocalization.Modles
{
	[SourcePath("UGUI/FontSize", 0)]
	public class FontSizeSource : SourceGeneric<Text>
	{
		protected override Text DefaultSource(Localization localiation)
		{
			return localiation.GetComponent<Text>();
		}
	}
}
