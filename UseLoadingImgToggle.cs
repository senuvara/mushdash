using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.UI.Controls;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UseLoadingImgToggle : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
{
	private Toggle m_Toggle;

	private List<int> m_UseIndexItem;

	private int m_RemoveIndex;

	private List<int> m_UseWelcomeIndexItem;

	private void Start()
	{
		m_Toggle = GetComponent<Toggle>();
		m_UseIndexItem = Singleton<DataManager>.instance["Account"]["UseLoadingIndex"].GetResult<List<int>>();
		m_UseWelcomeIndexItem = Singleton<DataManager>.instance["Account"]["UseWelcomeIndex"].GetResult<List<int>>();
	}

	public void OnPointerClick(PointerEventData data)
	{
		Singleton<AudioManager>.instance.PlayOneShot("sfx_common_button", Singleton<DataManager>.instance["GameConfig"]["SfxVolume"].GetResult<float>());
		if (m_Toggle.isOn)
		{
			if (Singleton<ItemManager>.instance.items[Singleton<DataManager>.instance["Account"]["SelectedItemIndex"].GetResult<int>()]["type"].GetResult<string>() == "loading")
			{
				if (!m_UseIndexItem.Contains(Singleton<ItemManager>.instance.items[Singleton<DataManager>.instance["Account"]["SelectedItemIndex"].GetResult<int>()]["index"].GetResult<int>()))
				{
					m_UseIndexItem.Add(Singleton<ItemManager>.instance.items[Singleton<DataManager>.instance["Account"]["SelectedItemIndex"].GetResult<int>()]["index"].GetResult<int>());
				}
			}
			else if (!m_UseWelcomeIndexItem.Contains(Singleton<ItemManager>.instance.items[Singleton<DataManager>.instance["Account"]["SelectedItemIndex"].GetResult<int>()]["index"].GetResult<int>()))
			{
				m_UseWelcomeIndexItem.Add(Singleton<ItemManager>.instance.items[Singleton<DataManager>.instance["Account"]["SelectedItemIndex"].GetResult<int>()]["index"].GetResult<int>());
			}
		}
		else if (Singleton<ItemManager>.instance.items[Singleton<DataManager>.instance["Account"]["SelectedItemIndex"].GetResult<int>()]["type"].GetResult<string>() == "loading")
		{
			if (m_UseIndexItem.Count > 1)
			{
				m_RemoveIndex = m_UseIndexItem.IndexOf(Singleton<ItemManager>.instance.items[Singleton<DataManager>.instance["Account"]["SelectedItemIndex"].GetResult<int>()]["index"].GetResult<int>());
				m_UseIndexItem.RemoveAt(m_RemoveIndex);
			}
			else
			{
				ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "cancelLimit"));
				m_Toggle.isOn = true;
			}
		}
		else if (m_UseWelcomeIndexItem.Count > 1)
		{
			m_RemoveIndex = m_UseWelcomeIndexItem.IndexOf(Singleton<ItemManager>.instance.items[Singleton<DataManager>.instance["Account"]["SelectedItemIndex"].GetResult<int>()]["index"].GetResult<int>());
			m_UseWelcomeIndexItem.RemoveAt(m_RemoveIndex);
		}
		else
		{
			ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "cancelLimit"));
			m_Toggle.isOn = true;
		}
		Singleton<DataManager>.instance.Save();
	}
}
