using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using DG.Tweening;
using FormulaBase;
using GameLogic;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GirlActionController : BaseSpineObjectController
{
	public bool isThisFrameJumpUp;

	public bool isThisFrameJumpDown;

	public static bool isPressHitNote;

	public static string pressHitNoteActKey = "char_uphit";

	private Tweener m_GhostDisapparTwn;

	private Sequence m_GhostDisapparSeq;

	public SpineActionController spineActionCtrl
	{
		get;
		private set;
	}

	public Material ghostMtrl
	{
		get;
		private set;
	}

	public Animation ghostAnimation
	{
		get;
		private set;
	}

	public ParticleSystem ghostParticles
	{
		get;
		private set;
	}

	public GameObject go
	{
		get;
		private set;
	}

	public Animator animator
	{
		get;
		private set;
	}

	public static GirlActionController instance
	{
		get;
		private set;
	}

	public static void ReleaseReferences()
	{
		instance.spineActionCtrl = null;
		instance.ghostMtrl = null;
		instance.ghostAnimation = null;
		instance.ghostParticles = null;
		instance.go = null;
		instance.animator = null;
	}

	public override void OnControllerStart()
	{
		SpineActionController.SetSynchroObjectsActive(base.gameObject, true);
		SpineActionController.Play("in", -1);
	}

	public override bool ControllerMissCheck(int idx, decimal currentTick)
	{
		return true;
	}

	public override void OnControllerAttacked(int result, bool isDeaded)
	{
	}

	public override bool OnControllerMiss(int idx)
	{
		return true;
	}

	public override void Init()
	{
		instance = this;
		go = base.gameObject;
		base.gameObject.SetActive(false);
		SpineActionController.SetSynchroObjectsActive(base.gameObject, false);
		spineActionCtrl = base.gameObject.GetComponent<SpineActionController>();
		animator = base.gameObject.GetComponent<Animator>();
		ghostMtrl = SingletonMonoBehaviour<GirlManager>.instance.girlGhost.GetComponent<Renderer>().sharedMaterial;
		ghostAnimation = SingletonMonoBehaviour<GirlManager>.instance.girlGhost.GetComponent<Animation>();
		ghostParticles = SingletonMonoBehaviour<GirlManager>.instance.girlGhost.transform.GetChild(0).GetComponent<ParticleSystem>();
	}

	public override void SetIdx(int idx)
	{
		base.idx = idx;
	}

	public void Attack(string actKey, uint result)
	{
		if (!Singleton<StageBattleComponent>.instance.isDead)
		{
			__Attack(actKey, result);
		}
	}

	private void GhostAttack(uint result, int id)
	{
		MusicData musicData = (id == -1) ? Singleton<StageBattleComponent>.instance.GetCurMusicData() : Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(id);
		if (musicData.isLongPressType)
		{
			return;
		}
		if (!SingletonMonoBehaviour<GirlManager>.instance.girlGhost.activeSelf)
		{
			SingletonMonoBehaviour<GirlManager>.instance.girlGhost.SetActive(true);
		}
		bool isGhostOnAir = Singleton<BattleEnemyManager>.instance.isGhostOnAir;
		bool isAir = musicData.isAir;
		bool isCharacterAirHitting = Singleton<BattleEnemyManager>.instance.isCharacterAirHitting;
		bool isCharacterGroundHitting = Singleton<BattleEnemyManager>.instance.isCharacterGroundHitting;
		bool isAirPressing = Singleton<BattleEnemyManager>.instance.isAirPressing;
		bool isGroundPressing = Singleton<BattleEnemyManager>.instance.isGroundPressing;
		string str = (result != 3) ? "Perfect" : "Great";
		Singleton<BattleEnemyManager>.instance.isGhostHitting = false;
		if (isAir)
		{
			if (isCharacterGroundHitting || isGroundPressing)
			{
				Singleton<BattleEnemyManager>.instance.isGhostHitting = true;
				GhostDisappear();
				if (isGhostOnAir)
				{
					Singleton<EventManager>.instance.Invoke("BattleShadow/OnUpHit" + str);
					return;
				}
				Singleton<BattleEnemyManager>.instance.isGhostOnAir = true;
				Singleton<EventManager>.instance.Invoke("BattleShadow/OnUpHitStart");
			}
		}
		else if (isCharacterAirHitting || isAirPressing)
		{
			Singleton<BattleEnemyManager>.instance.isGhostHitting = true;
			GhostDisappear();
			if (isGhostOnAir)
			{
				Singleton<BattleEnemyManager>.instance.isGhostOnAir = false;
				Singleton<EventManager>.instance.Invoke("BattleShadow/OnDownHitStart");
			}
			else
			{
				Singleton<EventManager>.instance.Invoke("BattleShadow/OnDownHit" + str);
			}
		}
	}

	public void GhostDisappear(float dt = 0.3f)
	{
		ghostMtrl.SetFloat("_Distortion", 0f);
		ghostAnimation.Stop();
		ghostParticles.Clear();
		ParticleSystem.EmissionModule emission = ghostParticles.emission;
		emission.rateOverTime = 0f;
		if (m_GhostDisapparTwn != null)
		{
			m_GhostDisapparTwn.Kill();
		}
		if (m_GhostDisapparSeq != null)
		{
			m_GhostDisapparSeq.Kill();
		}
		m_GhostDisapparTwn = ghostMtrl.DOFloat(1f, "_Distortion", 0.2f).SetDelay(dt).SetEase(Ease.Linear)
			.OnComplete(delegate
			{
				SingletonMonoBehaviour<GirlManager>.instance.girlGhost.SetActive(false);
			});
		m_GhostDisapparSeq = DOTweenUtils.Delay(delegate
		{
			ghostAnimation.Play();
			ghostParticles.Play();
		}, dt);
	}

	public void Pause()
	{
		if (m_GhostDisapparTwn != null)
		{
			m_GhostDisapparTwn.Pause();
		}
		if (m_GhostDisapparSeq != null)
		{
			m_GhostDisapparSeq.Pause();
		}
		if ((bool)animator)
		{
			animator.enabled = false;
		}
		if ((bool)spineActionCtrl)
		{
			spineActionCtrl.Pause();
		}
	}

	public void Resume()
	{
		if (m_GhostDisapparTwn != null)
		{
			m_GhostDisapparTwn.Play();
		}
		if (m_GhostDisapparSeq != null)
		{
			m_GhostDisapparSeq.Play();
		}
		if ((bool)animator)
		{
			animator.enabled = true;
		}
		if ((bool)spineActionCtrl)
		{
			spineActionCtrl.Resume();
		}
	}

	public void AttackQuick(string actKey, uint result, int id = -1)
	{
		if (Singleton<StageBattleComponent>.instance.isDead)
		{
			return;
		}
		MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(id);
		if (Singleton<BattleEnemyManager>.instance.isAirPressing)
		{
			if (musicDataByIdx.noteData.type != 3)
			{
				MusicData musicData = default(MusicData);
				for (int i = 1; i < 4; i++)
				{
					musicData = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(id - i);
					if (musicDataByIdx.configData.time - musicData.configData.time < 0.6m && musicData.isAir && musicData.noteData.type == 3 && musicData.isLongPressEnd)
					{
						isPressHitNote = true;
						break;
					}
				}
			}
			if (isPressHitNote)
			{
				pressHitNoteActKey = JumpAttack("char_uphit", 4u, id);
			}
		}
		if (Singleton<BattleEnemyManager>.instance.isAirPressing || Singleton<BattleEnemyManager>.instance.isGroundPressing)
		{
			GhostAttack(result, id);
			return;
		}
		List<TimeNodeOrder> timeNodeByTick = Singleton<StageBattleComponent>.instance.GetTimeNodeByTick(musicDataByIdx.tick);
		if (GameGlobal.gGameTouchPlay.isDoubleBegan && timeNodeByTick != null && timeNodeByTick.Count((TimeNodeOrder t) => t.md.doubleIdx > -1) >= 2)
		{
			int num = 0;
			List<int> list = new List<int>();
			foreach (TimeNodeOrder item in timeNodeByTick)
			{
				if (item.md.doubleIdx > -1 && !list.Contains(item.idx))
				{
					num++;
					list.Add(item.idx);
				}
			}
			if (num >= 2)
			{
				SpineActionController.Play("char_run", -1);
				SpineActionController.Play("char_bighit", -1);
				return;
			}
		}
		string text = actKey;
		switch (result)
		{
		case 2u:
			text = ((actKey == null || actKey.Length <= 2) ? string.Empty : actKey);
			break;
		case 3u:
			text = ((actKey == null || actKey.Length <= 2) ? "char_atk_g" : actKey);
			break;
		case 4u:
			text = ((actKey == null || actKey.Length <= 2) ? "char_atk_p" : actKey);
			break;
		}
		if (result > 1)
		{
			text = JumpAttack(text, result, id);
		}
		switch (text)
		{
		case "char_atk_p":
		case "char_atk_g":
		case "char_downhit":
			if (!Singleton<BattleEnemyManager>.instance.isCharacterGroundHitting)
			{
				Singleton<BattleEnemyManager>.instance.isGhostOnAir = false;
				Singleton<BattleEnemyManager>.instance.isCharacterGroundHitting = true;
				Singleton<BattleEnemyManager>.instance.isCharacterAirHitting = false;
				SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					Singleton<BattleEnemyManager>.instance.isCharacterGroundHitting = false;
				}, 0.1f);
			}
			break;
		}
		switch (text)
		{
		case "char_jumphit":
		case "char_jumphit_great":
		case "char_uphit":
			if (!Singleton<BattleEnemyManager>.instance.isCharacterAirHitting)
			{
				Singleton<BattleEnemyManager>.instance.isGhostOnAir = true;
				Singleton<BattleEnemyManager>.instance.isCharacterAirHitting = true;
				Singleton<BattleEnemyManager>.instance.isCharacterGroundHitting = false;
				SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					Singleton<BattleEnemyManager>.instance.isCharacterAirHitting = false;
				}, 0.1f);
			}
			break;
		}
		if (!string.IsNullOrEmpty(text) && !musicDataByIdx.isMul)
		{
			float tick = 0f;
			if (actKey == "char_downhit")
			{
				float num2 = animator.GetCurrentAnimatorStateInfo(0).length - animator.GetTime();
				if (num2 < 71f / (339f * (float)Math.PI))
				{
					tick = 0.0166666675f;
				}
			}
			SpineActionController.Play(text, -1, tick);
		}
		else
		{
			SpineActionController.PlaySkeleton(text, -1);
		}
		if (text == "char_atk_p" || text == "char_atk_g")
		{
			SingletonMonoBehaviour<GirlManager>.instance.SetJumpingAction(false);
		}
	}

	private string JumpAttack(string atkName, uint result, int id = -1)
	{
		if (result == 0 || atkName == "char_jump")
		{
			return atkName;
		}
		MusicData musicData = (id == -1) ? Singleton<StageBattleComponent>.instance.GetCurMusicData() : Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(id);
		if (musicData.isLongPressType)
		{
			return string.Empty;
		}
		bool flag = musicData.isAir;
		if ((musicData.noteData.type == 5 || musicData.noteData.type == 8) && musicData.noteData.boss_action != "0" && !string.IsNullOrEmpty(musicData.noteData.boss_action) && GameGlobal.gTouch.IsJumpTouch())
		{
			flag = true;
		}
		if (atkName == "char_hurt" || atkName == "char_jump_hurt")
		{
			return atkName;
		}
		atkName = (SingletonMonoBehaviour<GirlManager>.instance.IsJumpingAction() ? ((!flag) ? "char_downhit" : ((result != 3) ? "char_jumphit" : "char_jumphit_great")) : ((!flag) ? atkName : "char_uphit"));
		if (musicData.isMul)
		{
			atkName = ((result != 3) ? "char_jumphit" : "char_jumphit_great");
		}
		return atkName;
	}

	private void __Attack(string actKey, uint result)
	{
		SpineActionController.Play(actKey, -1);
	}
}
