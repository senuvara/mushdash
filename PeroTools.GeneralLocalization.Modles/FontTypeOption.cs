using Assets.Scripts.PeroTools.GeneralLocalization;

namespace PeroTools.GeneralLocalization.Modles
{
	public class FontTypeOption : OptionGeneric<FontTypeSource, FontType>
	{
		protected override void OnApply(FontTypeSource source, Localization localization)
		{
			source.target.font = value.font;
		}

		protected override FontType DefaultValue(Localization localization)
		{
			return new FontType();
		}
	}
}
