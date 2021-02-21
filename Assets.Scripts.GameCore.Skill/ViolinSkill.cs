using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class ViolinSkill : ISkill
	{
		public string uid => "Character_8";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.maxHp = 250;
			Singleton<BattleProperty>.instance.missComboMax = 100;
		}
	}
}
