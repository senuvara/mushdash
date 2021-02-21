using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

namespace Assets.Scripts.GameCore.Skill
{
	public class BunnySkill : ISkill
	{
		public string uid => "Character_3";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.maxHp = 200;
			Singleton<BattleProperty>.instance.heartNoteRate = 3f;
			Singleton<BattleProperty>.instance.musicNoteRate = 3f;
			Singleton<BattleProperty>.instance.hideNoteRate = 3f;
			Singleton<BattleProperty>.instance.blockNoteRate = 3f;
		}
	}
}
