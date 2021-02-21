using Sirenix.OdinInspector;

namespace Assets.Scripts.PeroTools.GeneralLocalization
{
	public abstract class SourceGeneric<T> : Source where T : class
	{
		[HideReferenceObjectPicker]
		[Required]
		public T target;

		public override object GetSourceTarget()
		{
			return target;
		}

		public sealed override void Default(Localization localiation)
		{
			target = DefaultSource(localiation);
		}

		protected virtual T DefaultSource(Localization localiation)
		{
			return (T)null;
		}
	}
}
