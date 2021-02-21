using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	[SourcePath("Toggles/Children", 0)]
	public class ChildrenSource : SourceGeneric<Transform>
	{
		protected override Transform DefaultSource(Localization localiation)
		{
			return localiation.transform;
		}
	}
}
