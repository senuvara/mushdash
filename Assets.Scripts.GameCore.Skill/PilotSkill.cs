using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class PilotSkill : ISkill
	{
		public string uid => "Character_4";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.maxHp = 250;
			Singleton<BattleProperty>.instance.isFeverGod = true;
		}
	}
}
