using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class AngelSkill : ISkill
	{
		public string uid => "Elfin_1";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.hurtReduce = 6;
		}
	}
}
