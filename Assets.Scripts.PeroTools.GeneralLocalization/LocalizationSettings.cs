using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.GeneralLocalization.Modles;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization
{
	public class LocalizationSettings : SingletonScriptableObject<LocalizationSettings>
	{
		[NonSerialized]
		public List<Localization> glConfigs = new List<Localization>();

		[InfoBox("编辑全局的Scheme(计划),如Language，和每个计划下的Option(选项),如English、Chinese、Japanese等", InfoMessageType.Info, null)]
		[ListDrawerSettings(CustomAddFunction = "AddItem", OnTitleBarGUI = "DrawRefreshButton")]
		[HideReferenceObjectPicker]
		[Title("Scheme Settings", null, TitleAlignments.Left, true, true)]
		public List<Scheme> schemes = new List<Scheme>(0);

		public void Register(Localization localization)
		{
			if (localization == null)
			{
				return;
			}
			if (glConfigs.Contains(localization))
			{
				Debug.LogErrorFormat("You can't registered a Lolization twice.", localization.gameObject.name);
				return;
			}
			glConfigs.Add(localization);
			for (int i = 0; i < schemes.Count; i++)
			{
				Scheme scheme = schemes[i];
				ActiveOptionToLocalization(localization, scheme.schemeEntry, scheme.activeOption);
			}
		}

		public void UnRegister(Localization localization)
		{
			if (!(localization == null))
			{
				glConfigs.Remove(localization);
			}
		}

		public void ActiveOption(string schemeName, string optionEntryName)
		{
			Scheme scheme = schemes.Find((Scheme s) => s.schemeEntry.name == schemeName);
			if (scheme == null)
			{
				Debug.LogFormat("No Scheme {0} defined.", schemeName);
			}
			else
			{
				ActiveOption(scheme, optionEntryName);
			}
		}

		public void ActiveOption(Scheme scheme, string optionEntryName)
		{
			OptionEntry optionEntry = scheme.optionPairs.Find((GlobalOptionPair pair) => pair.optionEntry.name == optionEntryName).optionEntry;
			if (optionEntry == null)
			{
				Debug.LogFormat("There is no entry {0} under scheme {1}", optionEntryName, scheme.schemeEntry.name);
			}
			else
			{
				ActiveOption(scheme.schemeEntry, optionEntry);
			}
		}

		public void ActiveOption(SchemeEntry schemeEntry, OptionEntry optionEntry)
		{
			Scheme scheme = GetScheme(schemeEntry);
			if (scheme != null)
			{
				Debug.LogFormat("[Localization] Active Scheme [{0}] , Option [{1}].", schemeEntry.name, optionEntry.name);
				scheme.activeOption = optionEntry;
				for (int i = 0; i < glConfigs.Count; i++)
				{
					ActiveOptionToLocalization(glConfigs[i], schemeEntry, optionEntry);
				}
			}
		}

		public void ActiveOptionToLocalization(Localization localization, SchemeEntry schemeEntry, OptionEntry optionEntry)
		{
			if (localization == null || localization.selectedSchemeEntry == null || !(localization.selectedSchemeEntry == schemeEntry))
			{
				return;
			}
			bool flag = false;
			if (!(localization.lastActiveOptionEntry != optionEntry))
			{
				return;
			}
			localization.lastActiveOptionEntry = optionEntry;
			localization.ApplySchemeOption(schemeEntry, optionEntry);
			if (localization.sourceType != typeof(TextSource) && localization.sourceType != typeof(TextJsonSource))
			{
				return;
			}
			Scheme scheme = GetScheme(schemeEntry);
			if (scheme == null)
			{
				return;
			}
			GlobalOptionPair globalOptionPair = null;
			for (int i = 0; i < scheme.optionPairs.Count; i++)
			{
				if (scheme.optionPairs[i].optionEntry == optionEntry)
				{
					globalOptionPair = scheme.optionPairs[i];
				}
			}
			if (globalOptionPair != null && globalOptionPair.globalOption != null)
			{
				globalOptionPair.globalOption.Apply(localization);
			}
		}

		public Scheme GetScheme(SchemeEntry entry)
		{
			if (entry == null)
			{
				return null;
			}
			return schemes.Find((Scheme scheme) => scheme.schemeEntry == entry);
		}

		public Scheme GetScheme(string schemeName)
		{
			return schemes.Find((Scheme scheme) => scheme.schemeEntry.name == schemeName);
		}

		public OptionEntry GetActiveOption(SchemeEntry entry)
		{
			if (entry == null)
			{
				return null;
			}
			return GetScheme(entry)?.activeOption;
		}

		public string GetActiveOption(string schemeName)
		{
			Scheme scheme2 = schemes.Find((Scheme scheme) => scheme.schemeEntry.name == schemeName);
			if (scheme2 != null && scheme2.activeOption != null)
			{
				return scheme2.activeOption.name;
			}
			return null;
		}
	}
}
