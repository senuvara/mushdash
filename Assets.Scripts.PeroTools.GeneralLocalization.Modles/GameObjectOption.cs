namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	public class GameObjectOption : OptionGeneric<GameObjectSource, bool>
	{
		protected override void OnApply(GameObjectSource source, Localization localization)
		{
			source.target.SetActive(value);
		}
	}
}
