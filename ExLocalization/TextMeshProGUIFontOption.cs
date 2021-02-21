using Assets.Scripts.PeroTools.GeneralLocalization;
using TMPro;

namespace ExLocalization
{
	public class TextMeshProGUIFontOption : OptionGeneric<TextMeshProGUIFontSource, TMP_FontAsset>
	{
		protected override void OnApply(TextMeshProGUIFontSource source, Localization localization)
		{
			source.target.font = value;
		}
	}
}
