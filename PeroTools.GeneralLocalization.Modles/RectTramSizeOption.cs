using Assets.Scripts.PeroTools.GeneralLocalization;
using UnityEngine;

namespace PeroTools.GeneralLocalization.Modles
{
	public class RectTramSizeOption : OptionGeneric<RectTramSizeSource, RectTramSize>
	{
		protected override void OnApply(RectTramSizeSource source, Localization localization)
		{
			source.target.sizeDelta = new Vector2(value.width, value.height);
		}

		protected override RectTramSize DefaultValue(Localization localization)
		{
			return new RectTramSize();
		}
	}
}
