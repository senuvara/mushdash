using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class BlackSkill : ISkill
	{
		public string uid => "Character_12";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.maxHp = 250;
			Singleton<BattleProperty>.instance.blockRP = 0.02m;
			Singleton<BattleProperty>.instance.blockRG = -0.04m;
			Singleton<BattleProperty>.instance.hitRangeAdded = 0.01m;
			Singleton<BattleProperty>.instance.blockNoHurtRange = 0;
		}
	}
}
