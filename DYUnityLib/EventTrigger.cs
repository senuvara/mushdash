namespace DYUnityLib
{
	public class EventTrigger
	{
		public delegate void TriggerHandler(object sender, uint triggerId, params object[] args);

		public delegate void TriggerDecimalHandler(object sender, uint triggerId, decimal ts);

		public uint id;

		public event TriggerHandler Trigger;

		public event TriggerDecimalHandler TriggerDecimal;

		public void RaiseEvent(uint triggerId, params object[] args)
		{
			if (this.Trigger != null)
			{
				id = triggerId;
				this.Trigger(this, triggerId, args);
			}
		}

		public void RaiseEvent(uint triggerId, decimal tick)
		{
			if (this.TriggerDecimal != null)
			{
				id = triggerId;
				this.TriggerDecimal(this, triggerId, tick);
			}
		}
	}
}
