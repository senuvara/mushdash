using Assets.Scripts.Common;
using Assets.Scripts.PeroTools.Commons;
using System.Collections.Generic;

namespace Assets.Scripts.PeroTools.Managers
{
	public class EventManager : Singleton<EventManager>
	{
		public delegate void EventCallFunc(params object[] args);

		internal class EventParam
		{
			public List<object> paramList = new List<object>();

			public EventParam(params object[] objs)
			{
				objs.ToList().Add(paramList);
			}
		}

		public class EventTrigger
		{
			public delegate void TriggerHandler(object sender, object reciever, params object[] args);

			public string uid;

			public object receiver;

			public event TriggerHandler trigger;

			internal void Play(params object[] args)
			{
				if (this.trigger != null)
				{
					this.trigger(this, receiver, args);
				}
			}

			internal void Play(object r, params object[] args)
			{
				if ((r == null || receiver == r) && this.trigger != null)
				{
					this.trigger(this, receiver, args);
				}
			}
		}

		public readonly Dictionary<string, object> events = CustomDefines.events;

		private readonly Dictionary<string, EventTrigger> m_EventTriggers = new Dictionary<string, EventTrigger>();

		public EventTrigger RegEvent(string uid, object receiver = null)
		{
			if (m_EventTriggers.ContainsKey(uid))
			{
				return m_EventTriggers[uid];
			}
			EventTrigger eventTrigger = new EventTrigger();
			eventTrigger.uid = uid;
			eventTrigger.receiver = receiver;
			EventTrigger eventTrigger2 = eventTrigger;
			m_EventTriggers.Add(uid, eventTrigger2);
			return eventTrigger2;
		}

		public EventTrigger UnregEvent(string uid)
		{
			if (!m_EventTriggers.ContainsKey(uid))
			{
				return null;
			}
			EventTrigger result = m_EventTriggers[uid];
			m_EventTriggers.Remove(uid);
			return result;
		}

		public void Invoke(string uid, params object[] args)
		{
			if (!m_EventTriggers.ContainsKey(uid))
			{
				if (events.ContainsKey(uid))
				{
					(events[uid] as EventCallFunc)?.Invoke(args);
				}
			}
			else
			{
				EventTrigger eventTrigger = m_EventTriggers[uid];
				eventTrigger.Play(args);
			}
		}

		public void InvokeReceiver(string uid, object reciever, params object[] args)
		{
			if (m_EventTriggers.ContainsKey(uid))
			{
				EventTrigger eventTrigger = m_EventTriggers[uid];
				eventTrigger.Play(reciever, args);
			}
		}
	}
}
