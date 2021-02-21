using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	[SourcePath("UGUI/Text", 0)]
	public class TextSource : SourceGeneric<Text>
	{
		protected override Text DefaultSource(Localization localiation)
		{
			return localiation.GetComponent<Text>();
		}
	}
}
