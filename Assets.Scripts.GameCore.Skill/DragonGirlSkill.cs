using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class DragonGirlSkill : ISkill
	{
		public string uid => "Elfin_6";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.bossAttackScoreRate = 1.3f;
		}
	}
}
