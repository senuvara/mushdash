using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class MagicGirlSkill : ISkill
	{
		public string uid => "Elfin_5";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.feverScoreRate = 1.2f;
		}
	}
}
