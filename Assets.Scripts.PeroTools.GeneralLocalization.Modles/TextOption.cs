namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	public class TextOption : OptionGeneric<TextSource, string>
	{
		protected override void OnApply(TextSource source, Localization localization)
		{
			if (!string.IsNullOrEmpty(value))
			{
				source.target.text = value.Replace("\\n", "\n");
			}
		}
	}
}
