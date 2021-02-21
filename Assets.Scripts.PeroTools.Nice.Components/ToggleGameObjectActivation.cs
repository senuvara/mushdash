using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Components
{
	public class ToggleGameObjectActivation : SerializedMonoBehaviour
	{
		[SerializeField]
		[ShowIf("HasToggle", true)]
		private GameObject m_Object;

		[SerializeField]
		[HideInInspector]
		private Toggle m_Toggle;

		private void Awake()
		{
			if ((bool)m_Toggle)
			{
				OnValueChanged(m_Toggle.isOn);
				m_Toggle.onValueChanged.AddListener(OnValueChanged);
			}
		}

		private void OnValueChanged(bool isOn)
		{
			m_Object.SetActive(isOn);
		}

		private void OnDisable()
		{
			m_Toggle.isOn = false;
		}
	}
}
