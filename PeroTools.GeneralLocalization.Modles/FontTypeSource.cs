using Assets.Scripts.PeroTools.GeneralLocalization;
using UnityEngine.UI;

namespace PeroTools.GeneralLocalization.Modles
{
	[SourcePath("UGUI/FontType", 0)]
	public class FontTypeSource : SourceGeneric<Text>
	{
		protected override Text DefaultSource(Localization localiation)
		{
			return localiation.GetComponent<Text>();
		}
	}
}
