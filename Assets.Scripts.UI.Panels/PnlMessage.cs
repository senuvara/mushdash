using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
	public class PnlMessage : SerializedMonoBehaviour
	{
		public GameObject task;

		public GameObject achievement;

		public GameObject stageAchievement;

		public GameObject unlockLevel;

		public GameObject unlockRole;

		public GameObject unlockElfin;

		public GameObject unlockLoading;

		public GameObject unlockWelcome;

		public GameObject rank;

		public Transform layout;

		public Button btnDone;

		public float gap = 0.5f;

		public float closeGap = 1f;

		private void OnEnable()
		{
			btnDone.gameObject.SetActive(false);
			List<IData> messages = SingletonMonoBehaviour<MessageManager>.instance.messages.Where(delegate(IData m)
			{
				string result = m["type"].GetResult<string>();
				int result2;
				switch (result)
				{
				default:
					result2 = ((result == "rank") ? 1 : 0);
					break;
				case "task":
				case "achievement":
				case "stage_achievement":
				case "unlockLevel":
				case "unlockRole":
				case "unlockElfin":
				case "unlockLoading":
				case "unlockWelcome":
					result2 = 1;
					break;
				}
				return (byte)result2 != 0;
			});
			for (int i = 0; i < messages.Count; i++)
			{
				IData data = messages[i];
				string uid = data["uid"].GetResult<string>();
				string type = data["type"].GetResult<string>();
				int index = i;
				SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					if (type == "achievement")
					{
						GameObject gameObject = UnityEngine.Object.Instantiate(achievement, layout);
						gameObject.transform.SetSiblingIndex(index);
						Text componentInChildren = gameObject.GetComponentInChildren<Text>();
						if ((bool)componentInChildren)
						{
							componentInChildren.text = string.Format("<color=#FFCA5FFF>{0}</color>    {1}", Singleton<ConfigManager>.instance.GetConfigStringValue(type, "uid", "title", uid), Singleton<ConfigManager>.instance.GetConfigStringValue(type, "uid", "description", uid));
						}
					}
					else if (type == "task")
					{
						GameObject gameObject2 = UnityEngine.Object.Instantiate(task, layout);
						gameObject2.transform.SetSiblingIndex(index);
						Text componentInChildren2 = gameObject2.GetComponentInChildren<Text>();
						if ((bool)componentInChildren2)
						{
							componentInChildren2.text = Singleton<ConfigManager>.instance.GetConfigStringValue(type, "uid", "description", uid);
						}
					}
					else if (type == "stage_achievement")
					{
						try
						{
							string[] array = uid.Split('-');
							string text = array[array.Length - 2];
							string text2 = array[array.Length - 1];
							string cmpValue = uid.Replace($"-{text}-{text2}", string.Empty);
							int configIndex = Singleton<ConfigManager>.instance.GetConfigIndex(type, "uid", cmpValue);
							string text3 = (string)Singleton<ConfigManager>.instance[type][configIndex][text][int.Parse(text2)];
							GameObject gameObject3 = UnityEngine.Object.Instantiate(stageAchievement, layout);
							gameObject3.transform.SetSiblingIndex(index);
							Text componentInChildren3 = gameObject3.GetComponentInChildren<Text>();
							if ((bool)componentInChildren3)
							{
								componentInChildren3.text = text3;
							}
						}
						catch (Exception)
						{
						}
					}
					else if (type == "unlockLevel")
					{
						UnityEngine.Object.Instantiate(unlockLevel, layout).transform.SetSiblingIndex(index);
					}
					else if (type == "unlockRole")
					{
						UnityEngine.Object.Instantiate(unlockRole, layout).transform.SetSiblingIndex(index);
					}
					else if (type == "unlockElfin")
					{
						UnityEngine.Object.Instantiate(unlockElfin, layout).transform.SetSiblingIndex(index);
					}
					else if (type == "unlockLoading")
					{
						UnityEngine.Object.Instantiate(unlockLoading, layout).transform.SetSiblingIndex(index);
					}
					else if (type == "unlockWelcome")
					{
						UnityEngine.Object.Instantiate(unlockWelcome, layout).transform.SetSiblingIndex(index);
					}
					else if (type == "rank")
					{
						UnityEngine.Object.Instantiate(rank, layout).transform.SetSiblingIndex(index);
					}
					SingletonMonoBehaviour<MessageManager>.instance.Receive(type, uid);
					if (index == messages.Count - 1)
					{
						SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
						{
							btnDone.gameObject.SetActive(true);
						}, closeGap);
					}
				}, (float)i * gap);
			}
		}

		public void FinishMessage()
		{
			for (int i = 0; i < layout.childCount; i++)
			{
				UnityEngine.Object.Destroy(layout.GetChild(i).gameObject);
			}
			Singleton<DataManager>.instance.Save();
			base.gameObject.SetActive(false);
		}
	}
}
