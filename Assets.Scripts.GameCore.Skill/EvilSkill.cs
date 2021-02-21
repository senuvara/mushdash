using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class EvilSkill : ISkill
	{
		public string uid => "Character_11";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.maxHp = 200;
			Singleton<BattleProperty>.instance.hpChangedPerTime = -0.1m;
			Singleton<BattleProperty>.instance.scoreExtraRate = 1.25f;
		}
	}
}
