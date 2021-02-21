using Spine;

namespace GameLogic
{
	public class UnActiveObject : DoNothing
	{
		public override void Do(TrackEntry entry)
		{
			SpineActionController.Play("in", gameObject);
			gameObject.SetActive(false);
		}
	}
}
