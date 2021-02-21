using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	public class ImageOption : OptionGeneric<ImageSource, Sprite>
	{
		protected override void OnApply(ImageSource source, Localization localization)
		{
			source.target.sprite = value;
		}
	}
}
