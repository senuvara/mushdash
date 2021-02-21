using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	[SourcePath("UGUI/GridLayoutGroup", 0)]
	public class GridLayoutGroupSource : SourceGeneric<GridLayoutGroup>
	{
		protected override GridLayoutGroup DefaultSource(Localization localiation)
		{
			return localiation.GetComponent<GridLayoutGroup>();
		}
	}
}
