using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	public class GridLayoutGroupOption : OptionGeneric<GridLayoutGroupSource, Property>
	{
		protected override void OnApply(GridLayoutGroupSource source, Localization localization)
		{
			source.target.cellSize = value.cellSize;
			source.target.spacing = value.spacing;
		}

		protected override Property DefaultValue(Localization localization)
		{
			Property property = new Property();
			GridLayoutGroup component = localization.GetComponent<GridLayoutGroup>();
			if (component != null)
			{
				property.cellSize = component.cellSize;
				property.spacing = component.spacing;
			}
			return property;
		}
	}
}
