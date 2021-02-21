using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization
{
	public abstract class OptionGeneric<TSource, TValue> : Option where TSource : Source
	{
		[HideReferenceObjectPicker]
		public TValue value;

		public override void Apply(Localization localization)
		{
			TSource val = (TSource)localization.source;
			if (val == null)
			{
				Debug.LogErrorFormat("Unable to convert to {0}.", typeof(TSource));
			}
			else if (val.GetSourceTarget() != null)
			{
				OnApply(val, localization);
			}
		}

		protected abstract void OnApply(TSource source, Localization localization);

		public sealed override void Default(Localization localization)
		{
			value = DefaultValue(localization);
		}

		protected virtual TValue DefaultValue(Localization localization)
		{
			return default(TValue);
		}
	}
}
