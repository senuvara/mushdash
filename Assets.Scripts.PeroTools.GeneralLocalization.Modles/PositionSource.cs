using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	[SourcePath("Basic/Position", 0)]
	public class PositionSource : SourceGeneric<Transform>
	{
		protected override Transform DefaultSource(Localization localiation)
		{
			return localiation.GetComponent<Transform>();
		}
	}
}
