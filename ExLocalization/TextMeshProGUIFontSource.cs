using Assets.Scripts.PeroTools.GeneralLocalization;
using TMPro;

namespace ExLocalization
{
	[SourcePath("UGUI/FontType(TextMeshProGUI)", 0)]
	public class TextMeshProGUIFontSource : SourceGeneric<TextMeshProUGUI>
	{
		protected override TextMeshProUGUI DefaultSource(Localization localiation)
		{
			return localiation.GetComponent<TextMeshProUGUI>();
		}
	}
}
