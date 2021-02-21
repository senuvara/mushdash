using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.GeneralLocalization;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class ActiveLocalizationOptoin : Action
	{
		public OptionEntry optionEntry;

		public SchemeEntry schemeEntry;

		public override void Execute()
		{
			Debug.Log("Click");
			SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption(schemeEntry, optionEntry);
		}
	}
}
