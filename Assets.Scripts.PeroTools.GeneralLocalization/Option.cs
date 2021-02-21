namespace Assets.Scripts.PeroTools.GeneralLocalization
{
	public abstract class Option
	{
		public abstract void Apply(Localization localization);

		public abstract void Default(Localization localization);
	}
}
