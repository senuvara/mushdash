using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class MagicSkill : ISkill
	{
		public string uid => "Character_10";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.maxHp = 200;
			Singleton<BattleProperty>.instance.maxFever = 100f;
		}
	}
}
