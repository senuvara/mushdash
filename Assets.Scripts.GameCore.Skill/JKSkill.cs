using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class JKSkill : ISkill
	{
		public string uid => "Character_14";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.isCatchAvailable = true;
			Singleton<BattleProperty>.instance.maxHp = 250;
		}
	}
}
