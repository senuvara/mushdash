using Sirenix.OdinInspector;

namespace Assets.Scripts.PeroTools.GeneralLocalization
{
	public class GlobalOptionPair
	{
		[HideReferenceObjectPicker]
		[HideLabel]
		public GlobalOptoin globalOption = new TextGlobalOption();

		public OptionEntry optionEntry;
	}
}
