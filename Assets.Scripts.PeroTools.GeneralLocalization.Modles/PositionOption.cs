using System;
using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	public class PositionOption : OptionGeneric<PositionSource, PositionOption.Property>
	{
		[Serializable]
		public class Property
		{
			public Vector3 position;
		}

		protected override void OnApply(PositionSource source, Localization localization)
		{
			source.target.localPosition = value.position;
		}

		protected override Property DefaultValue(Localization localization)
		{
			Property property = new Property();
			property.position = localization.transform.localPosition;
			return property;
		}
	}
}
