using Assets.Scripts.PeroTools.Nice.Actions;
using Sirenix.Serialization;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	public class TextJsonOption : OptionGeneric<TextJsonSource, SetText>
	{
		protected override void OnApply(TextJsonSource source, Localization localization)
		{
			int indexInPlayables;
			SetText setText = source.target.GetSetText(out indexInPlayables);
			if (setText != null)
			{
				source.target.@event.playables[indexInPlayables] = value;
			}
		}

		protected override SetText DefaultValue(Localization localization)
		{
			TextJsonSource textJsonSource = (TextJsonSource)localization.source;
			int indexInPlayables;
			SetText setText = textJsonSource.target.GetSetText(out indexInPlayables);
			if (setText == null)
			{
				return null;
			}
			return (SetText)SerializationUtility.CreateCopy(setText);
		}
	}
}
