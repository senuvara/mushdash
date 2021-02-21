using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Specials
{
	[RequireComponent(typeof(Toggle))]
	public class PnlTroveItemToggleEnterAsSelected : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		private Toggle m_Toggle;

		private void Awake()
		{
			m_Toggle = GetComponent<Toggle>();
			m_Toggle.onValueChanged.AddListener(OnToggleChange);
		}

		private void OnToggleChange(bool state)
		{
			if (state && EventSystem.current.currentSelectedGameObject != base.gameObject)
			{
				EventSystem.current.SetSelectedGameObject(base.gameObject);
			}
		}

		public void OnSelect(BaseEventData eventData)
		{
			if (m_Toggle != null && !m_Toggle.isOn)
			{
				m_Toggle.isOn = true;
			}
		}
	}
}
