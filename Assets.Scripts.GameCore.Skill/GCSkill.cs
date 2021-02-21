using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class GCSkill : ISkill
	{
		public string uid => "Character_15";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.isGcCharacter = true;
			Singleton<BattleProperty>.instance.maxHp = 250;
		}
	}
}
