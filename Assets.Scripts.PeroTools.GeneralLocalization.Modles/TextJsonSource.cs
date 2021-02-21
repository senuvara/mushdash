using Assets.Scripts.PeroTools.Nice.Actions;
using Assets.Scripts.PeroTools.Nice.Events;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	[SourcePath("UGUI/SetText(ActionFlow)", 0)]
	public class TextJsonSource : SourceGeneric<TextJsonSourceTarget>
	{
		protected override TextJsonSourceTarget DefaultSource(Localization localiation)
		{
			Event[] components = localiation.GetComponents<Event>();
			TextJsonSourceTarget textJsonSourceTarget = new TextJsonSourceTarget();
			for (int i = 0; i < components.Length; i++)
			{
				int indexInPlayables;
				SetText setText = TextJsonSourceTarget.GetSetText(components[i], 0, out indexInPlayables);
				if (setText != null)
				{
					textJsonSourceTarget.@event = components[i];
					textJsonSourceTarget.index = 0;
				}
			}
			return textJsonSourceTarget;
		}
	}
}
