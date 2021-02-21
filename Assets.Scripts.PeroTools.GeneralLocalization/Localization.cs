using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.GeneralLocalization.Modles;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization
{
	[ExecuteInEditMode]
	public class Localization : SerializedMonoBehaviour
	{
		[NonSerialized]
		[HideInInspector]
		public OptionEntry lastActiveOptionEntry;

		private LocalizationSettings m_Manager;

		public List<OptionPair> optionPairs = new List<OptionPair>();

		public SchemeEntry selectedSchemeEntry;

		[HideReferenceObjectPicker]
		[HideLabel]
		public Source source = new ChildrenSource();

		public Type sourceType = typeof(ChildrenSource);

		public Option GetOption(OptionEntry entry)
		{
			if (entry == null)
			{
				return null;
			}
			for (int i = 0; i < optionPairs.Count; i++)
			{
				if (optionPairs[i].optionEntry == entry)
				{
					return optionPairs[i].option;
				}
			}
			return null;
		}

		public OptionEntry FindEntry(Option option)
		{
			for (int i = 0; i < optionPairs.Count; i++)
			{
				if (optionPairs[i].option == option)
				{
					return optionPairs[i].optionEntry;
				}
			}
			return null;
		}

		public void ApplySchemeOption(SchemeEntry schemeEntry, OptionEntry optionEntry)
		{
			if (!(schemeEntry == null) && !(optionEntry == null) && !(selectedSchemeEntry == null))
			{
				Option option = GetOption(optionEntry);
				if (option != null)
				{
					option.Apply(this);
					return;
				}
				Debug.LogFormat("No option {0} found under scheme {1}.", optionEntry.name, schemeEntry.name);
			}
		}

		private void OnEnable()
		{
			m_Manager = SingletonScriptableObject<LocalizationSettings>.instance;
			m_Manager.Register(this);
		}

		private void OnDisable()
		{
			if (!(m_Manager == null))
			{
				m_Manager.UnRegister(this);
			}
		}
	}
}
