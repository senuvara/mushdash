using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	[SourcePath("Toggles/GameObject", 0)]
	public class GameObjectSource : SourceGeneric<GameObject>
	{
		protected override GameObject DefaultSource(Localization localiation)
		{
			return localiation.gameObject;
		}
	}
}
