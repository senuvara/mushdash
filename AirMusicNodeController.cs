using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using FormulaBase;
using GameLogic;
using UnityEngine;

public class AirMusicNodeController : BaseEnemyObjectController
{
	private bool m_IsBeAttacked;

	public override void Init()
	{
		base.Init();
		Singleton<EffectManager>.instance.Preload("fx_score_ground");
	}

	private void AddExtraScore(int result)
	{
		int score = m_MusicData.noteData.score;
		Singleton<TaskStageTarget>.instance.AddScore(Mathf.RoundToInt((float)score * Singleton<BattleProperty>.instance.musicNoteRate), m_MusicData.objId, m_MusicData.noteData.ibms_id, m_MusicData.isAir);
		BattleRoleAttributeComponent.instance.AddHp(Singleton<BattleProperty>.instance.musicNoteAddHp);
		Singleton<EventManager>.instance.Invoke("Battle/OnScoreGetAir", score);
		GameObject gameObject = Singleton<EffectManager>.instance.Play("fx_score_ground");
		Transform transform = gameObject.transform;
		Vector3 position = gameObject.transform.position;
		float x = position.x;
		Vector3 position2 = gameObject.transform.position;
		transform.position = new Vector3(x, 0.94f, position2.z);
		SpineActionController.Play("out", idx);
		AttacksController.Instance.PlayOneShot(m_MusicData.noteData.key_audio, AttacksController.KeyAudioType.Touch);
	}

	public override void OnControllerAttacked(int result, bool isDeaded)
	{
		if (m_IsBeAttacked || result == 5)
		{
			return;
		}
		m_IsBeAttacked = true;
		Singleton<BattleEnemyManager>.instance.SetPlayResult(idx, 4);
		float dt = (float)m_MusicData.tick - Singleton<StageBattleComponent>.instance.timeFromMusicStart;
		Singleton<TimerManager>.instance.Delay(delegate
		{
			AddExtraScore(result);
			if (idx < GameMusicScene.instance.animators.Length)
			{
				Animator animator = GameMusicScene.instance.animators[idx];
				if ((bool)animator)
				{
					animator.speed = 0f;
				}
			}
			Singleton<TaskStageTarget>.instance.AddNoteItemCount(1);
		}, dt);
	}
}
