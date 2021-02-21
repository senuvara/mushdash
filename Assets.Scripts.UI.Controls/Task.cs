using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Controls
{
	public class Task : MonoBehaviour
	{
		public Text text;

		private void OnEnable()
		{
			int siblingIndex = base.transform.GetSiblingIndex();
			SingletonDataObject singletonDataObject = Singleton<DataManager>.instance["Task"];
			string text = Singleton<TaskManager>.instance.tasks[siblingIndex];
			string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("task", "uid", "description", text);
			int num = -1;
			switch (text)
			{
			case "1-1-1":
				num = 3;
				break;
			case "1-2-2":
				num = 2;
				break;
			case "1-3-2":
				num = 2;
				break;
			case "1-4-2":
				num = 2;
				break;
			case "1-5-2":
				num = 2;
				break;
			case "1-6-2":
				num = 2;
				break;
			case "1-7-1":
				num = 2;
				break;
			case "1-8-1":
				num = 2;
				break;
			case "2-1-1":
				num = 4;
				break;
			case "2-2-1":
				num = 2;
				break;
			case "2-2-2":
				num = 3;
				break;
			case "2-3-1":
				num = 2;
				break;
			case "2-3-2":
				num = 3;
				break;
			case "2-4-1":
				num = 2;
				break;
			case "2-4-2":
				num = 3;
				break;
			case "2-5-1":
				num = 2;
				break;
			case "2-5-2":
				num = 3;
				break;
			case "2-6-1":
				num = 2;
				break;
			case "2-6-2":
				num = 3;
				break;
			case "2-7-1":
				num = 2;
				break;
			case "2-7-2":
				num = 3;
				break;
			case "2-8-1":
				num = 2;
				break;
			case "2-8-2":
				num = 3;
				break;
			case "3-1-1":
				num = 5;
				break;
			case "3-2-1":
				num = 3;
				break;
			case "3-3-1":
				num = 3;
				break;
			case "3-4-1":
				num = 3;
				break;
			case "3-5-1":
				num = 3;
				break;
			case "3-6-1":
				num = 3;
				break;
			case "3-7-1":
				num = 2;
				break;
			case "3-7-2":
				num = 3;
				break;
			case "3-8-1":
				num = 2;
				break;
			case "3-8-2":
				num = 3;
				break;
			}
			if (num == -1)
			{
				this.text.text = configStringValue;
				return;
			}
			int num2 = 0;
			if (singletonDataObject.Exists(text) && singletonDataObject[text].result != null)
			{
				num2 = singletonDataObject[text].GetResult<List<IData>>().Count;
			}
			this.text.text = $"{configStringValue}    ({num2}/{num})";
		}
	}
}
