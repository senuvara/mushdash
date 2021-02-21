using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Sirenix.OdinInspector;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	public class OnCustomEvent : Event
	{
		[PropertyOrder(-1)]
		[CustomValueDrawer("OnEventTypeGUI")]
		public string eventType;

		protected override void OnEnter()
		{
			Singleton<EventManager>.instance.RegEvent(eventType).trigger += Trigger;
		}

		protected override void OnExit()
		{
			Singleton<EventManager>.instance.RegEvent(eventType).trigger -= Trigger;
		}

		private void Trigger(object sender, object reciever, params object[] args)
		{
			if (!this)
			{
				OnExit();
			}
			else
			{
				Execute();
			}
		}
	}
}
