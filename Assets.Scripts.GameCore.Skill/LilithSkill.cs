using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;

namespace Assets.Scripts.GameCore.Skill
{
	public class LilithSkill : ISkill
	{
		public string uid => "Elfin_7";

		public void Apply()
		{
			Singleton<BattleProperty>.instance.perfectScoreExtra = 1.05f;
			if (Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>() == 11)
			{
				Singleton<BattleProperty>.instance.perfectHpRevive = 2;
				Singleton<BattleProperty>.instance.musicNoteAddHp = 2;
			}
		}
	}
}
