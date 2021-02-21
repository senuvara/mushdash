using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class RockSkill : ISkill
	{
		public string uid => "Character_0";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.maxHp = 300;
		}
	}
}
