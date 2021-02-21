using Assets.Scripts.PeroTools.GeneralLocalization.Modles;
using Assets.Scripts.PeroTools.Nice.Actions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization
{
	public class TextGlobalOption : GlobalOptoin
	{
		[HideLabel]
		public Font font;

		public override void Apply(Localization localization)
		{
			TextSource textSource = localization.source as TextSource;
			if (textSource != null && textSource.target != null)
			{
				if (textSource.target != null)
				{
					textSource.target.font = font;
				}
				return;
			}
			TextJsonSource textJsonSource = localization.source as TextJsonSource;
			if (textJsonSource != null && textJsonSource.target != null && textJsonSource.target.@event != null)
			{
				int indexInPlayables;
				SetText setText = textJsonSource.target.GetSetText(out indexInPlayables);
				if (setText != null && setText.Object != null)
				{
					setText.Object.font = font;
				}
			}
		}

		public override void Default(Localization localization)
		{
		}
	}
}
