using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	[SourcePath("Basic/Scale", 0)]
	public class ScaleSource : SourceGeneric<Transform>
	{
		protected override Transform DefaultSource(Localization localiation)
		{
			return localiation.GetComponent<Transform>();
		}
	}
}
