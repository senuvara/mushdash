using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class NekoSkill : ISkill
	{
		public string uid => "Character_16";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.isNekoCharacter = true;
			Singleton<BattleProperty>.instance.isNekoSkillTrigger = false;
			Singleton<BattleProperty>.instance.maxHp = 250;
		}
	}
}
