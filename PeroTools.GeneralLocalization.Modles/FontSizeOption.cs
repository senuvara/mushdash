using Assets.Scripts.PeroTools.GeneralLocalization;

namespace PeroTools.GeneralLocalization.Modles
{
	public class FontSizeOption : OptionGeneric<FontSizeSource, FontSize>
	{
		protected override void OnApply(FontSizeSource source, Localization localization)
		{
			source.target.fontSize = value.size;
			if (source.target.resizeTextForBestFit)
			{
				source.target.resizeTextMaxSize = value.maxSize;
			}
		}

		protected override FontSize DefaultValue(Localization localization)
		{
			return new FontSize();
		}
	}
}
