using Assets.Scripts.PeroTools.GeneralLocalization.Modles;
using Sirenix.OdinInspector;

namespace Assets.Scripts.PeroTools.GeneralLocalization
{
	public class OptionPair
	{
		[HideReferenceObjectPicker]
		[HideLabel]
		public Option option = new TextOption();

		public OptionEntry optionEntry;
	}
}
