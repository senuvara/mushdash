using Assets.Scripts.PeroTools.GeneralLocalization;
using UnityEngine;

namespace PeroTools.GeneralLocalization.Modles
{
	[SourcePath("UGUI/RectTramsformSize", 0)]
	public class RectTramSizeSource : SourceGeneric<RectTransform>
	{
		protected override RectTransform DefaultSource(Localization localiation)
		{
			return localiation.GetComponent<RectTransform>();
		}
	}
}
