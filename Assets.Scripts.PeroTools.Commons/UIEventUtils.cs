using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Assets.Scripts.PeroTools.Commons
{
	public class UIEventUtils
	{
		public static void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function, List<GameObject> gameObjects) where T : IEventSystemHandler
		{
			EventSystem current = EventSystem.current;
			if (!current)
			{
				return;
			}
			List<RaycastResult> list = new List<RaycastResult>();
			current.RaycastAll(data, list);
			GameObject gameObject = data.pointerCurrentRaycast.gameObject;
			foreach (RaycastResult item in list)
			{
				GameObject gameObject2 = item.gameObject;
				if (gameObject != gameObject2 && gameObjects.Contains(gameObject2) && gameObject2.activeSelf)
				{
					ExecuteEvents.Execute(gameObject2, data, function);
					break;
				}
			}
		}

		public static EventTrigger.TriggerEvent OnEvent(GameObject go, EventTriggerType eventType, UnityAction<BaseEventData> callback)
		{
			EventTrigger orAddComponent = go.GetOrAddComponent<EventTrigger>();
			orAddComponent.triggers = (orAddComponent.triggers ?? new List<EventTrigger.Entry>());
			EventTrigger.Entry entry = orAddComponent.triggers.Find((EventTrigger.Entry e) => e.eventID == eventType);
			if (entry == null)
			{
				entry = new EventTrigger.Entry();
				orAddComponent.triggers.Add(entry);
			}
			entry.eventID = eventType;
			EventTrigger.TriggerEvent triggerEvent = entry.callback;
			if (triggerEvent == null)
			{
				triggerEvent = (entry.callback = new EventTrigger.TriggerEvent());
			}
			triggerEvent.AddListener(callback);
			return triggerEvent;
		}
	}
}
