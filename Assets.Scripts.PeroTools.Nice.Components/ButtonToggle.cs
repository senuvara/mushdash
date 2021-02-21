using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Components
{
	public class ButtonToggle : SerializedMonoBehaviour
	{
		[Required]
		public ToggleGroup group;

		public Button btnPre;

		public Button btnNext;

		public bool isLoop;

		private int m_Index;

		private List<Toggle> m_Toggles;

		private void Awake()
		{
			btnPre.onClick.AddListener(delegate
			{
				OnClick(true);
			});
			btnNext.onClick.AddListener(delegate
			{
				OnClick(false);
			});
		}

		private void OnEnable()
		{
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				m_Toggles = from tgl in GameUtils.FindObjectsOfType<Toggle>(@group.transform)
					where tgl.@group == @group && tgl.gameObject.activeSelf
					select tgl;
				m_Index = m_Toggles.FindIndex((Toggle t) => t.isOn);
			}, Time.deltaTime);
		}

		private void OnClick(bool pre)
		{
			m_Index = m_Toggles.FindIndex((Toggle t) => t.isOn);
			if (pre)
			{
				m_Index--;
			}
			else
			{
				m_Index++;
			}
			if (m_Index < 0)
			{
				if (isLoop)
				{
					m_Index = m_Toggles.Count - 1;
				}
				else
				{
					m_Index = 0;
				}
			}
			if (m_Index >= m_Toggles.Count)
			{
				if (isLoop)
				{
					m_Index = 0;
				}
				else
				{
					m_Index = m_Toggles.Count - 1;
				}
			}
			m_Toggles[m_Index].isOn = true;
		}
	}
}
