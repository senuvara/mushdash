namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	public class RectTramAnchorsOption : OptionGeneric<RectTramAnchorsSource, RectTramAnchors>
	{
		protected override void OnApply(RectTramAnchorsSource source, Localization localization)
		{
			source.target.anchorMin = value.min;
			source.target.anchorMax = value.max;
		}

		protected override RectTramAnchors DefaultValue(Localization localization)
		{
			return new RectTramAnchors();
		}
	}
}
