using Assets.Scripts.PeroTools.Commons;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[DisallowMultipleComponent]
	public class OnEventTrigger : Event
	{
		[SerializeField]
		[PropertyOrder(-1)]
		private EventTriggerType m_Type;

		private EventTrigger.TriggerEvent m_EventCallback;

		protected override void OnEnter()
		{
			EventTrigger orAddComponent = base.gameObject.GetOrAddComponent<EventTrigger>();
			m_EventCallback = new EventTrigger.TriggerEvent();
			m_EventCallback.AddListener(OnEventTriggerExecute);
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.callback = m_EventCallback;
			entry.eventID = m_Type;
			EventTrigger.Entry item = entry;
			orAddComponent.triggers.Add(item);
		}

		private void OnEventTriggerExecute(BaseEventData eventData)
		{
			Execute();
		}

		protected override void OnExit()
		{
			m_EventCallback.RemoveListener(OnEventTriggerExecute);
		}
	}
}
