using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class SantaRinSkill : ISkill
	{
		public string uid => "Character_13";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.maxHp = 250;
			Singleton<BattleProperty>.instance.accFunc = ((float oldAcc) => (oldAcc <= 0.9f) ? (oldAcc + 0.05f) : (oldAcc * 0.5f + 0.5f));
		}
	}
}
