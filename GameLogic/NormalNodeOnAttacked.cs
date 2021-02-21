using Assets.Scripts.PeroTools.Commons;
using FormulaBase;
using Spine;

namespace GameLogic
{
	public class NormalNodeOnAttacked : DoNothing
	{
		public override void Do(TrackEntry entry)
		{
			MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
			string key = "char_hurt";
			if (musicDataByIdx.noteData.type == 5)
			{
				key = "boss_hurt";
			}
			Boss.Instance.Play(key);
		}
	}
}
