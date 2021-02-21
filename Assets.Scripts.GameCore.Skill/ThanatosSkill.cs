using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class ThanatosSkill : ISkill
	{
		public string uid => "Elfin_2";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.isBloodMissHardTime = true;
			Singleton<BattleProperty>.instance.missHardTime = 2m;
		}
	}
}
