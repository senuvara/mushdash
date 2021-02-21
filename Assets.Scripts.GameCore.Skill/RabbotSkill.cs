using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;

namespace Assets.Scripts.GameCore.Skill
{
	public class RabbotSkill : ISkill
	{
		public string uid => "Elfin_3";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.missToGreat = 5;
			Singleton<EventManager>.instance.Invoke("Battle/OnMiss2Great");
		}
	}
}
