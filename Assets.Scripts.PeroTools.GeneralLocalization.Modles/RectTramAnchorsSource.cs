using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	[SourcePath("UGUI/RectTramsformAnchor", 0)]
	public class RectTramAnchorsSource : SourceGeneric<RectTransform>
	{
		protected override RectTransform DefaultSource(Localization localiation)
		{
			return localiation.GetComponent<RectTransform>();
		}
	}
}
