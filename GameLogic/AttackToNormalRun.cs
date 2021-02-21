using Assets.Scripts.PeroTools.Commons;
using Spine;

namespace GameLogic
{
	public class AttackToNormalRun : DoNothing
	{
		public override void Do(TrackEntry entry)
		{
			if (!SingletonMonoBehaviour<GirlManager>.instance.IsJumpingAction())
			{
				SpineActionController.Play("char_run", gameObject);
			}
		}
	}
}
