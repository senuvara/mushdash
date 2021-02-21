using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class MioSirSkill : ISkill
	{
		public string uid => "Elfin_0";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.feverTime = 7f;
		}
	}
}
