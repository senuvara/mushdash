using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class ZombiaSkill : ISkill
	{
		public string uid => "Character_6";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.maxHp = 250;
			Singleton<BattleProperty>.instance.hasGodChance = true;
			Singleton<BattleProperty>.instance.godTimeCount = 1;
		}
	}
}
