using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization
{
	public class Scheme
	{
		public OptionEntry activeOption;

		[ListDrawerSettings(CustomAddFunction = "AddItem")]
		[HideReferenceObjectPicker]
		[LabelText("Options")]
		public List<GlobalOptionPair> optionPairs = new List<GlobalOptionPair>();

		public SchemeEntry schemeEntry;

		public string GetOptionName(int index)
		{
			if (index < 0 || index >= optionPairs.Count)
			{
				Debug.LogErrorFormat("Index [{0}] out of range [{1}].", index, optionPairs.Count);
				return null;
			}
			return optionPairs[index].optionEntry.name;
		}

		public List<string> GetAllOptionsName()
		{
			return optionPairs.Select((GlobalOptionPair pair) => pair.optionEntry.name).ToList();
		}

		public void GetAllOptionsName(List<string> list)
		{
			if (list != null)
			{
				list.Clear();
				for (int i = 0; i < optionPairs.Count; i++)
				{
					list.Add(optionPairs[i].optionEntry.name);
				}
			}
		}

		public bool Exist(string optionName)
		{
			return optionPairs.Exists((GlobalOptionPair pair) => pair.optionEntry.name == optionName);
		}

		public int OptionCount()
		{
			return (optionPairs != null) ? optionPairs.Count : 0;
		}
	}
}
