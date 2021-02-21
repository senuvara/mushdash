using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	public class ChildrenOption : OptionGeneric<ChildrenSource, Transform>
	{
		protected override void OnApply(ChildrenSource source, Localization localization)
		{
			if (!(source.target != null) || !(value != null))
			{
				return;
			}
			for (int i = 0; i < source.target.childCount; i++)
			{
				Transform child = source.target.GetChild(i);
				bool flag = false;
				for (int j = 0; j < localization.optionPairs.Count; j++)
				{
					ChildrenOption childrenOption = (ChildrenOption)localization.optionPairs[j].option;
					if (childrenOption != null && childrenOption.value == child)
					{
						flag = true;
						break;
					}
				}
				if (flag && child != value)
				{
					child.gameObject.SetActive(false);
				}
				value.gameObject.SetActive(true);
			}
		}
	}
}
