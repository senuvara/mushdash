using System;
using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	public class ScaleOption : OptionGeneric<ScaleSource, ScaleOption.Property>
	{
		[Serializable]
		public class Property
		{
			public Vector3 scale;
		}

		protected override void OnApply(ScaleSource source, Localization localization)
		{
			source.target.localScale = value.scale;
		}

		protected override Property DefaultValue(Localization localization)
		{
			Property property = new Property();
			property.scale = localization.transform.localScale;
			return property;
		}
	}
}
