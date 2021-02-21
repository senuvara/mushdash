using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class JokerSkill : ISkill
	{
		public string uid => "Character_7";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.maxHp = 200;
			Singleton<BattleProperty>.instance.comboRate = 0.7f;
		}
	}
}
