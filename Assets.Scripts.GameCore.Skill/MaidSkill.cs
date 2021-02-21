using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class MaidSkill : ISkill
	{
		public string uid => "Character_9";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.hpSkillCount = 1;
			Singleton<BattleProperty>.instance.maxHp = 250;
			Singleton<BattleProperty>.instance.reviveRate = 2.5f;
			Singleton<BattleProperty>.instance.hpRevive = 100;
			Singleton<BattleProperty>.instance.skillMissHardTime = 2.0m;
		}
	}
}
