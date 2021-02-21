using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;

namespace Assets.Scripts.GameCore.Skill
{
	public class RampageSkill : ISkill
	{
		public string uid => "Character_1";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.maxHp = 250;
			Singleton<BattleProperty>.instance.greatToPerfect = 5;
			Singleton<EventManager>.instance.Invoke("Battle/OnGreat2Perfect");
		}
	}
}
