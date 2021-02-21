namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	public class ComponentOption : OptionGeneric<ComponentSource, bool>
	{
		protected override void OnApply(ComponentSource source, Localization localization)
		{
			source.target.enabled = value;
		}
	}
}
