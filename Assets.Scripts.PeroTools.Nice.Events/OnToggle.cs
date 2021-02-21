using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[DisallowMultipleComponent]
	public class OnToggle : Event
	{
		[SerializeField]
		[Required]
		[PropertyOrder(-1)]
		private Toggle m_Toggle;

		private bool m_IsOn;

		protected override void OnEnter()
		{
			m_IsOn = m_Toggle.isOn;
			m_Toggle.onValueChanged.AddListener(Listener);
		}

		protected override void OnExit()
		{
			m_Toggle.onValueChanged.RemoveListener(Listener);
		}

		private void Listener(bool isOn)
		{
			if (isOn != m_IsOn)
			{
				m_IsOn = isOn;
				Execute();
			}
		}
	}
}
