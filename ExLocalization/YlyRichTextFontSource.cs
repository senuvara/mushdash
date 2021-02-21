using Assets.Scripts.PeroTools.GeneralLocalization;

namespace ExLocalization
{
	[SourcePath("UGUI/Extension/YlyRichTextFont", 0)]
	public class YlyRichTextFontSource : SourceGeneric<YlyRichText>
	{
		protected override YlyRichText DefaultSource(Localization localiation)
		{
			return localiation.GetComponent<YlyRichText>();
		}
	}
}
