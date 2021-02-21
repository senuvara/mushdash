using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using Assets.Scripts.PeroTools.PreWarm;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Specials
{
	public class TroveItemIndexs : MonoBehaviour, IPreWarm
	{
		private List<VariableBehaviour> m_Variables = new List<VariableBehaviour>();

		private void RefreshIndexs()
		{
			List<IData> result = Singleton<DataManager>.instance["Account"]["Items"].GetResult<List<IData>>();
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				m_Variables.Add(child.GetComponent<VariableBehaviour>());
			}
			int num = 0;
			for (int j = 0; j < m_Variables.Count; j++)
			{
				VariableBehaviour data = m_Variables[j];
				if (j >= result.Count)
				{
					break;
				}
				IData data2 = result[j];
				if (Singleton<ConfigManager>.instance[data2["type"].GetResult<string>()].Count <= data2["index"].GetResult<int>())
				{
					break;
				}
				if (Singleton<ConfigManager>.instance.GetConfigBoolValue(data2["type"].GetResult<string>(), data2["index"].GetResult<int>(), "hide"))
				{
					num++;
				}
				data.SetResult(num);
				num++;
			}
		}

		public void PreWarm(int slice)
		{
			if (slice == 0)
			{
				RefreshIndexs();
			}
		}
	}
}
