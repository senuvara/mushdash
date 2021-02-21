using Assets.Scripts.PeroTools.GeneralLocalization;
using UnityEngine;

namespace ExLocalization
{
	public class YlyRichTextFontOption : OptionGeneric<YlyRichTextFontSource, Font>
	{
		protected override void OnApply(YlyRichTextFontSource source, Localization localization)
		{
			source.target.font = value;
		}
	}
}
