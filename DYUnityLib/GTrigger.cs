using UnityEngine;

namespace DYUnityLib
{
	internal class GTrigger
	{
		private static EventTrigger[] m_EventTriggers = new EventTrigger[9];

		private static EventTrigger MUSIC_STEP_EVENT = null;

		private static EventTrigger esObj;

		private static object args;

		public static EventTrigger RegEvent(uint eventIndex)
		{
			EventTrigger eventTrigger = m_EventTriggers[eventIndex];
			if (eventTrigger != null)
			{
				Debug.Log("Event " + eventIndex + " already Reg.");
				return eventTrigger;
			}
			EventTrigger eventTrigger2 = new EventTrigger();
			eventTrigger2.id = eventIndex;
			eventTrigger = eventTrigger2;
			m_EventTriggers[eventIndex] = eventTrigger;
			if (eventIndex == 4)
			{
				MUSIC_STEP_EVENT = eventTrigger;
			}
			return eventTrigger;
		}

		public static void UnRegEvent(uint eventIndex)
		{
			m_EventTriggers[eventIndex] = null;
		}

		public static void FireEvent(uint eventIndex, decimal tick)
		{
			if (eventIndex == 4)
			{
				MUSIC_STEP_EVENT.RaiseEvent(eventIndex, tick);
			}
			else if (m_EventTriggers[eventIndex] != null)
			{
				esObj = m_EventTriggers[eventIndex];
				if (esObj != null)
				{
					args = tick;
					esObj.RaiseEvent(eventIndex, args);
				}
			}
		}

		public static void FireEvent(uint eventIndex, params object[] args)
		{
			if (eventIndex == 4)
			{
				MUSIC_STEP_EVENT.RaiseEvent(eventIndex, args);
			}
			else if (m_EventTriggers[eventIndex] != null)
			{
				esObj = m_EventTriggers[eventIndex];
				if (esObj != null)
				{
					esObj.RaiseEvent(eventIndex, args);
				}
			}
		}

		public static void ClearEvent()
		{
			m_EventTriggers = new EventTrigger[m_EventTriggers.Length];
		}
	}
}
