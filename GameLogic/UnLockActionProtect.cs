using Spine;

namespace GameLogic
{
	public class UnLockActionProtect : DoNothing
	{
		public override void Do(TrackEntry entry)
		{
			SpineActionController component = gameObject.GetComponent<SpineActionController>();
			if (component != null)
			{
				component.SetProtectLevel(0);
				component.SetCurrentActionName(null);
			}
		}
	}
}
