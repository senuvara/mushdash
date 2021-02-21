using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class StopMusic : Action
	{
		public override void Execute()
		{
			Singleton<AudioManager>.instance.StopBGM();
		}
	}
}
