using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	[SourcePath("UGUI/Image", 0)]
	public class ImageSource : SourceGeneric<Image>
	{
		protected override Image DefaultSource(Localization localiation)
		{
			return localiation.GetComponent<Image>();
		}
	}
}
