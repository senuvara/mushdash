using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using FormulaBase;
using GameLogic;
using UnityEngine;

public class GroundEnergyBottleController : BaseEnemyObjectController
{
	private bool m_IsBeAttacked;

	public override void Init()
	{
		base.Init();
		Singleton<EffectManager>.instance.Preload("fx_hp_ground");
	}

	private void RecoveryEffects()
	{
		if (Singleton<BattleProperty>.instance.hasGodChance && Singleton<BattleProperty>.instance.godTimeCount == 0 && !Singleton<BattleProperty>.instance.isInGod)
		{
			Singleton<BattleProperty>.instance.godTimeCount = 1;
		}
		Singleton<TaskStageTarget>.instance.AddBlood(1);
		int num = m_MusicData.noteData.score;
		int hp = BattleRoleAttributeComponent.instance.GetHp();
		if (hp >= BattleRoleAttributeComponent.instance.GetHpMax())
		{
			Singleton<TaskStageTarget>.instance.AddScore(Mathf.RoundToInt(300f * Singleton<BattleProperty>.instance.heartNoteRate), m_MusicData.objId, m_MusicData.noteData.ibms_id, m_MusicData.isAir);
		}
		decimal num2 = 0m;
		if (hp < Singleton<BattleProperty>.instance.hpRevive && Singleton<BattleProperty>.instance.hpSkillCount > 0)
		{
			Singleton<BattleProperty>.instance.hpSkillCount--;
			num = Mathf.RoundToInt((float)num * Singleton<BattleProperty>.instance.reviveRate);
			num2 = Singleton<BattleProperty>.instance.skillMissHardTime;
		}
		if (Singleton<BattleProperty>.instance.isBloodMissHardTime && Singleton<BattleProperty>.instance.missHardTime > num2)
		{
			num2 = Singleton<BattleProperty>.instance.missHardTime;
		}
		if (num2 > 0m)
		{
			BattleRoleAttributeComponent.instance.MissHardEffect((float)num2);
			GameGlobal.gGameMissPlay.SetMissHardTime(num2);
		}
		BattleRoleAttributeComponent.instance.AddHp(num);
		if (Singleton<BattleProperty>.instance.isNekoSkillTrigger)
		{
			Singleton<TaskStageTarget>.instance.AddScore(Mathf.RoundToInt(300f * Singleton<BattleProperty>.instance.heartNoteRate), m_MusicData.objId, m_MusicData.noteData.ibms_id, m_MusicData.isAir);
			Singleton<EventManager>.instance.Invoke("Battle/OnScoreGet", Singleton<TaskStageTarget>.instance.GetAddScore());
		}
		else
		{
			GameObject gameObject = Singleton<EffectManager>.instance.Play("fx_hp_ground");
			Transform transform = gameObject.transform;
			Vector3 position = gameObject.transform.position;
			float x = position.x;
			Vector3 position2 = gameObject.transform.position;
			transform.position = new Vector3(x, -0.94f, position2.z);
			Singleton<EventManager>.instance.Invoke("Battle/OnHpGet", num);
		}
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
			RecoveryEffects();
			if (idx < GameMusicScene.instance.animators.Length)
			{
				Animator animator = GameMusicScene.instance.animators[idx];
				if ((bool)animator)
				{
					animator.speed = 0f;
				}
			}
			Singleton<TaskStageTarget>.instance.AddEnergyItemCount(1);
		}, dt);
	}

	public override bool OnControllerMiss(int i)
	{
		return true;
	}
}
