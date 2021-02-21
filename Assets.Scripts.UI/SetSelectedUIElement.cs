using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	public class SetSelectedUIElement : MonoBehaviour
	{
		public GameObject firstSelected;

		public float delay;

		public Button btnClose;

		private EventSystem m_EventSystem;

		private void Awake()
		{
			if (btnClose != null)
			{
				btnClose.onClick.AddListener(OnUIClose);
			}
			m_EventSystem = EventSystem.current;
		}

		private void OnUIClose()
		{
			if (m_EventSystem != null)
			{
				firstSelected = m_EventSystem.currentSelectedGameObject;
			}
		}

		private void OnEnable()
		{
			if (!(firstSelected != null) || !(m_EventSystem != null))
			{
				return;
			}
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				m_EventSystem.SetSelectedGameObject(firstSelected);
				Animator component = firstSelected.GetComponent<Animator>();
				if (component != null)
				{
					component.SetTrigger("Highlighted");
				}
			}, delay);
		}

		private void OnDisable()
		{
			if (btnClose == null)
			{
				OnUIClose();
			}
		}
	}
}
