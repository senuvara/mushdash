using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Controls
{
	public class Achvs : MonoBehaviour
	{
		public GameObject achvBox;

		private List<string> m_Achievements;

		private List<int> m_DoneIndex;

		private List<GameObject> m_GameObjects;

		private Dictionary<string, int> m_IndexCount;

		private int m_CurrentActivateAchv;

		public AchvSelect achvSelect;

		private int m_SeealbeAchvCount;

		private string m_CurrentLanguage;

		private void Awake()
		{
			m_Achievements = new List<string>();
			m_DoneIndex = new List<int>();
			m_GameObjects = new List<GameObject>();
			m_IndexCount = new Dictionary<string, int>();
			m_CurrentLanguage = Singleton<DataManager>.instance["Account"]["Language"].GetResult<string>();
		}

		private void OnEnable()
		{
			m_Achievements.Clear();
			m_DoneIndex.Clear();
			m_IndexCount.Clear();
			string a = string.Empty;
			int count = Singleton<ConfigManager>.instance["achievement"].Count;
			for (int i = 0; i < count; i++)
			{
				string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("achievement", i, "uid");
				string text = configStringValue.BeginBefore('-');
				string str = (i + 1 >= count) ? string.Empty : Singleton<ConfigManager>.instance.GetConfigStringValue("achievement", i + 1, "uid");
				string a2 = str.BeginBefore('-');
				if (!m_IndexCount.ContainsKey(text))
				{
					m_IndexCount.Add(text, 1);
				}
				else
				{
					m_IndexCount[text]++;
				}
				if (a != text && (!Singleton<AchievementManager>.instance.achievements.Contains(configStringValue) || a2 != text))
				{
					a = text;
					m_Achievements.Add(configStringValue);
					if (Singleton<AchievementManager>.instance.achievements.Contains(configStringValue) && a2 != text)
					{
						m_DoneIndex.Add(int.Parse(text));
					}
				}
			}
			UpdateRectTransformHeight();
			for (int j = 0; j < m_Achievements.Count; j++)
			{
				string text2 = m_Achievements[j];
				GameObject gameObject = (j >= m_GameObjects.Count) ? Object.Instantiate(achvBox, base.transform) : m_GameObjects[j];
				bool configBoolValue = Singleton<ConfigManager>.instance.GetConfigBoolValue("achievement", "uid", "hide", text2);
				if (configBoolValue && !Singleton<AchievementManager>.instance.IsDone(text2))
				{
					string arg = text2.BeginBefore('-');
					string s = text2.LastAfter('-');
					int num = int.Parse(s);
					if (num > 1)
					{
						text2 = $"{arg}-{num - 1}";
					}
					else
					{
						gameObject.SetActive(false);
					}
				}
				else
				{
					gameObject.SetActive(true);
				}
				string configStringValue2 = Singleton<ConfigManager>.instance.GetConfigStringValue("achievement", "uid", "title", text2);
				string configStringValue3 = Singleton<ConfigManager>.instance.GetConfigStringValue("achievement", "uid", "description", text2);
				gameObject.GetComponentInChildren<Text>().text = $"{configStringValue2}      <color=#FFFFFFB2>{configStringValue3}</color>";
				int num2 = int.Parse(text2.LastAfter('-'));
				int num3 = int.Parse(text2.BeginBefore('-'));
				num3 = ((!configBoolValue) ? num3 : (num3 + 1));
				int num4 = m_IndexCount[(j + 1).ToString()];
				num4 = ((!configBoolValue || m_DoneIndex.Contains(num3 - 1)) ? num4 : (num4 - 1));
				int num5 = 4 - num4;
				for (int num6 = 4; num6 > 0; num6--)
				{
					GameObject gameObject2 = gameObject.transform.GetChild(1).Find($"Level{num6}").gameObject;
					if (num6 > num5)
					{
						gameObject2.SetActive(true);
						bool flag = num6 > 5 - num2 || m_DoneIndex.Contains(num3) || configBoolValue;
						gameObject2.transform.GetChild(0).gameObject.SetActive(flag);
						gameObject2.transform.GetChild(1).gameObject.SetActive(!flag);
					}
					else
					{
						gameObject2.SetActive(false);
					}
				}
				if (j >= m_GameObjects.Count)
				{
					m_GameObjects.Add(gameObject);
				}
				gameObject.GetComponent<DOTweenAnimation>().DOPlay();
			}
		}

		private IEnumerator UpdateActivateAchv()
		{
			yield return new WaitForEndOfFrame();
			SetAchv(m_CurrentActivateAchv);
			if (++m_CurrentActivateAchv >= m_Achievements.Count)
			{
				achvSelect.UpdateSelectableObj();
			}
			else
			{
				StartCoroutine(UpdateActivateAchv());
			}
		}

		private void SetAchv(int AchvIndex)
		{
			string text = m_Achievements[AchvIndex];
			GameObject gameObject;
			if (AchvIndex >= m_GameObjects.Count)
			{
				gameObject = Object.Instantiate(achvBox, base.transform);
				m_GameObjects.Add(gameObject);
			}
			else
			{
				gameObject = m_GameObjects[AchvIndex];
			}
			gameObject.SetActive(!IsHideAchv(AchvIndex));
			UpdateRectTransformHeight();
			bool configBoolValue = Singleton<ConfigManager>.instance.GetConfigBoolValue("achievement", "uid", "hide", text);
			string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("achievement", "uid", "title", text);
			string configStringValue2 = Singleton<ConfigManager>.instance.GetConfigStringValue("achievement", "uid", "description", text);
			gameObject.GetComponentInChildren<Text>().text = $"{configStringValue}      <color=#FFFFFFB2>{configStringValue2}</color>";
			int num = int.Parse(text.LastAfter('-'));
			int num2 = int.Parse(text.BeginBefore('-'));
			num2 = ((!configBoolValue) ? num2 : (num2 + 1));
			int num3 = m_IndexCount[(AchvIndex + 1).ToString()];
			num3 = ((!configBoolValue || m_DoneIndex.Contains(num2 - 1)) ? num3 : (num3 - 1));
			int num4 = 4 - num3;
			for (int num5 = 4; num5 > 0; num5--)
			{
				GameObject gameObject2 = gameObject.transform.GetChild(1).Find($"Level{num5}").gameObject;
				if (num5 > num4)
				{
					gameObject2.SetActive(true);
					bool flag = num5 > 5 - num || m_DoneIndex.Contains(num2) || configBoolValue;
					gameObject2.transform.GetChild(0).gameObject.SetActive(flag);
					gameObject2.transform.GetChild(1).gameObject.SetActive(!flag);
				}
				else
				{
					gameObject2.SetActive(false);
				}
			}
			gameObject.GetComponent<DOTweenAnimation>().DOPlay();
		}

		private bool IsHideAchv(int AchvIndex)
		{
			string text = m_Achievements[AchvIndex];
			if (Singleton<ConfigManager>.instance.GetConfigBoolValue("achievement", "uid", "hide", text) && !Singleton<AchievementManager>.instance.IsDone(text))
			{
				return int.Parse(text.LastAfter('-')) <= 1;
			}
			return false;
		}

		private void UpdateRectTransformHeight()
		{
			int num = 0;
			for (int i = 0; i < m_Achievements.Count; i++)
			{
				if (!IsHideAchv(i))
				{
					num++;
				}
			}
			if (num != m_SeealbeAchvCount)
			{
				m_SeealbeAchvCount = num;
				Vector2 sizeDelta = achvBox.GetComponent<RectTransform>().sizeDelta;
				float y = sizeDelta.y;
				float spacing = GetComponent<VerticalLayoutGroup>().spacing;
				float y2 = (float)m_SeealbeAchvCount * y + (float)(m_SeealbeAchvCount - 1) * spacing;
				RectTransform component = GetComponent<RectTransform>();
				component.sizeDelta = new Vector2(component.rect.x, y2);
			}
		}
	}
}
