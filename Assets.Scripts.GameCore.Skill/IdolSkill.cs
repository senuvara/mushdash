using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class IdolSkill : ISkill
	{
		public string uid => "Character_5";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.maxHp = 200;
			Singleton<BattleProperty>.instance.expRate = 1.5f;
		}
	}
}
