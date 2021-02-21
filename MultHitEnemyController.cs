using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using DG.Tweening;
using FormulaBase;
using GameLogic;
using UnityEngine;

public class MultHitEnemyController : NormalEnemyController
{
	private bool m_IsFlyAway;

	private int m_HitCount;

	private float m_AtkTime;

	private static float m_OriginSize;

	private static Camera m_Camera;

	private int m_MulHitLowThreshold;

	private int m_MulHitMidThreshold;

	private int m_MulHitHighThreshold;

	private float m_HitTime;

	public static bool isMulHitEnding;

	public static bool isBanEmptyAction;

	public static int hitCount;

	private static Sequence m_HittingCameraSequence;

	public static bool isHitting;

	public bool isDead => m_IsFlyAway;

	public bool isOver
	{
		get
		{
			if (m_HitCount > m_MulHitHighThreshold)
			{
				return true;
			}
			return false;
		}
	}

	public static void Pause()
	{
		DOTween.Pause(m_Camera);
		m_HittingCameraSequence.Pause();
	}

	public static void Resume()
	{
		DOTween.Play(m_Camera);
		m_HittingCameraSequence.Play();
	}

	public override void Init()
	{
		base.Init();
		if (!m_Camera)
		{
			m_Camera = Camera.main;
			m_OriginSize = m_Camera.orthographicSize;
		}
		m_MulHitLowThreshold = m_MusicData.GetMulHitLowThreshold();
		m_MulHitMidThreshold = m_MusicData.GetMulHitMidThreshold();
		m_MulHitHighThreshold = m_MusicData.GetMulHitHighThreshold();
		if (m_HittingCameraSequence == null)
		{
			m_HittingCameraSequence = DOTween.Sequence();
		}
	}

	private void OnDestroy()
	{
		if (m_HittingCameraSequence != null)
		{
			m_HittingCameraSequence = null;
		}
	}

	public override void OnControllerStart()
	{
		base.OnControllerStart();
		if (m_HitCount == 0)
		{
			Singleton<EventManager>.instance.Invoke("Battle/OnMulHitNoteEnter");
		}
		if (m_MusicData.noteData.boss_action != "0" && !string.IsNullOrEmpty(m_MusicData.noteData.boss_action))
		{
			SpineActionController.PlaySkeletonAnim(m_MusicData.noteData.boss_action, -2);
			if (m_MusicData.noteData.ibms_id == "16")
			{
				SpineActionController.PlaySkeletonAnim("standby", -2, true, false);
			}
		}
		SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
		{
			if (!Singleton<StageBattleComponent>.instance.isDead)
			{
				Singleton<EventManager>.instance.Invoke("UI/OnMultiHitTip");
			}
		}, m_MusicData.dt - 1.2m);
	}

	public override bool ControllerMissCheck(int i, decimal currentTick)
	{
		if (Singleton<BattleEnemyManager>.instance.GetPlayResult(i) == 0)
		{
			return base.ControllerMissCheck(i, currentTick);
		}
		return false;
	}

	public override bool OnControllerMiss(int index)
	{
		if (m_HitCount < m_MulHitMidThreshold && m_HitCount > 0)
		{
			Singleton<StatisticsManager>.instance.OnNoteResult(1);
		}
		if (m_HitCount == 0)
		{
			Singleton<StatisticsManager>.instance.OnNoteResult(0);
		}
		Singleton<EventManager>.instance.Invoke("Battle/OnMulHitNoteTipExit");
		Singleton<BattleEnemyManager>.instance.SetPlayResult(idx, 1);
		base.OnControllerMiss(idx);
		return true;
	}

	public override void OnControllerAttacked(int result, bool isDeaded)
	{
		AttacksController.Instance.PlayOneShot($"hitsound_{((hitCount <= 15) ? hitCount : 15):D3}", AttacksController.KeyAudioType.Hit);
		Singleton<BattleProperty>.instance.isHpChangable = false;
		m_AtkTime = Singleton<StageBattleComponent>.instance.timeFromMusicStart;
		if (Singleton<BattleEnemyManager>.instance.GetPlayResult(m_MusicData.objId) == 1 || m_IsFlyAway)
		{
			return;
		}
		if (m_MusicData.noteData.boss_action == "0" || string.IsNullOrEmpty(m_MusicData.noteData.boss_action))
		{
			base.gameObject.transform.position = new Vector3(-3.5f, 0f, 0f);
			SpineActionController.Play("note_multihit_hurt", idx);
		}
		else
		{
			SpineActionController.PlaySkeletonAnim("multi_atk_hurt", -2);
		}
		if (m_HitCount != 0)
		{
			if (!SingletonMonoBehaviour<GirlManager>.instance.animator.GetCurrentAnimatorStateInfo(0).IsName("char_multihit_start"))
			{
				SpineActionController.Play("char_multihit_start", -1);
			}
			ShakeMulCamera();
		}
		else
		{
			Singleton<EventManager>.instance.Invoke("Battle/OnMulHitNoteTipExit");
			DOTween.Pause(m_Camera);
			DOTween.Pause(m_Camera.transform);
			float duration = 0.083f;
			m_Camera.DOOrthoSize(m_OriginSize * 0.84f, duration).SetEase(Ease.OutQuad);
			Transform transform = m_Camera.transform;
			Vector3 position = m_Camera.transform.position;
			float y = position.y;
			Vector3 position2 = m_Camera.transform.position;
			transform.position = new Vector3(0f, y, position2.z);
			m_Camera.transform.DOMoveX(-1.08f, duration).SetEase(Ease.OutQuad);
			SpineActionController.Play("char_multihit_start", -1);
			Singleton<BattleEnemyManager>.instance.isMulHitting = true;
		}
		hitCount = ++m_HitCount;
		Singleton<EventManager>.instance.Invoke("Battle/OnMultiHitChanged");
		isHitting = true;
		if (m_HitCount >= m_MulHitHighThreshold)
		{
			AttacksController.Instance.ShowAttack(m_MusicData.objId, 4u, 1u);
			Succuess(true);
		}
	}

	public static void ShakeBossCamera()
	{
		if (m_HittingCameraSequence == null)
		{
			m_HittingCameraSequence = DOTween.Sequence();
		}
		if (!m_Camera)
		{
			m_Camera = Camera.main;
		}
		if ((bool)m_Camera)
		{
			if (!m_HittingCameraSequence.IsComplete())
			{
				m_HittingCameraSequence.Complete();
			}
			m_HittingCameraSequence = DOTween.Sequence();
			Vector3 originPos = new Vector3(0f, 0f, -10f);
			Vector3 vector = new Vector3(Random.Range(-0.3f, -0.3f), Random.Range(-0.15f, -0.15f), 0f);
			m_HittingCameraSequence.Append(m_Camera.transform.DOMove(originPos + vector, 0.04f));
			m_HittingCameraSequence.Append(m_Camera.transform.DOMove(originPos - vector * 0.3f, 0.04f));
			m_HittingCameraSequence.Append(m_Camera.transform.DOMove(originPos + vector * 0.1f, 0.04f));
			m_HittingCameraSequence.Append(m_Camera.transform.DOMove(originPos, 0.04f));
			m_HittingCameraSequence.OnComplete(delegate
			{
				m_Camera.transform.position = originPos;
			});
		}
	}

	public static void ShakeMulCamera()
	{
		if (m_HittingCameraSequence == null)
		{
			m_HittingCameraSequence = DOTween.Sequence();
		}
		if (!m_Camera)
		{
			m_Camera = Camera.main;
		}
		if ((bool)m_Camera)
		{
			if (!m_HittingCameraSequence.IsComplete())
			{
				m_HittingCameraSequence.Complete();
			}
			m_HittingCameraSequence = DOTween.Sequence();
			Vector3 originPos = new Vector3(-1.08f, 0f, -10f);
			Vector3 vector = new Vector3(Random.Range(-0.13f, -0.13f), Random.Range(-0.1f, 0.1f), 0f);
			m_HittingCameraSequence.Append(m_Camera.transform.DOMove(originPos + vector, 0.02f));
			m_HittingCameraSequence.Append(m_Camera.transform.DOMove(originPos - vector * 0.1f, 0.02f));
			m_HittingCameraSequence.Append(m_Camera.transform.DOMove(originPos + vector * 0.3f, 0.02f));
			m_HittingCameraSequence.Append(m_Camera.transform.DOMove(originPos, 0.02f));
			m_HittingCameraSequence.OnComplete(delegate
			{
				m_Camera.transform.position = originPos;
				m_Camera.orthographicSize = m_OriginSize * 0.813f;
			});
		}
	}

	public void OnControllerTick(TimeNodeOrder tno)
	{
		if (m_IsFlyAway || m_HitCount == 0)
		{
			return;
		}
		bool flag = Singleton<StageBattleComponent>.instance.timeFromMusicStart - m_AtkTime <= 0.5f && m_AtkTime != 0f;
		m_HitTime = GameGlobal.gTouch.tickTime;
		if (m_HitCount < m_MulHitLowThreshold)
		{
			if (m_HitCount == m_MulHitLowThreshold - 1 && tno.isLast)
			{
				hitCount = ++m_HitCount;
				Singleton<EventManager>.instance.Invoke("Battle/OnMultiHitChanged");
				AttacksController.Instance.ShowAttack(m_MusicData.objId, 3u, 1u);
				Succuess();
			}
			else if (!flag || tno.isLast)
			{
				Fail();
			}
		}
		else if (tno.isLast)
		{
			if (m_HitCount >= m_MulHitMidThreshold - 1)
			{
				hitCount = ++m_HitCount;
				Singleton<EventManager>.instance.Invoke("Battle/OnMultiHitChanged");
				AttacksController.Instance.ShowAttack(m_MusicData.objId, 4u, 1u);
				Succuess(true);
			}
			else if (m_HitCount < m_MulHitMidThreshold)
			{
				hitCount = ++m_HitCount;
				Singleton<EventManager>.instance.Invoke("Battle/OnMultiHitChanged");
				AttacksController.Instance.ShowAttack(m_MusicData.objId, 3u, 1u);
				Succuess();
			}
		}
		else if (!flag)
		{
			Succuess(false, false);
			Singleton<StageBattleComponent>.instance.SetCombo(0);
		}
	}

	private void Fail()
	{
		if (m_MusicData.noteData.boss_action == "0" || string.IsNullOrEmpty(m_MusicData.noteData.boss_action))
		{
			base.gameObject.transform.position = new Vector3(0f, -2.2f, 0f);
			SpineActionController.Play("note_multi_alive", idx);
		}
		else
		{
			string actionKey = "multi_atk_out";
			if (m_MusicData.noteData.ibms_id == "17")
			{
				actionKey = "multi_atk_end";
			}
			SpineActionController.PlaySkeletonAnim(actionKey, -2);
			if (m_MusicData.noteData.ibms_id == "16")
			{
				SpineActionController.PlaySkeletonAnim("standby", -2, true, false);
			}
		}
		OnControllerMiss(idx);
		InvokeHitEnd();
		m_IsFlyAway = true;
	}

	private void Succuess(bool isTotal = false, bool resulted = true)
	{
		Singleton<StatisticsManager>.instance.OnNoteResult(2);
		if (isTotal)
		{
			Singleton<StageBattleComponent>.instance.SetCombo(Singleton<StageBattleComponent>.instance.GetCombo() + 1);
			if (!FeverManager.Instance.IsOnFeverState())
			{
				FeverManager.Instance.AddFever(m_MusicData.noteData.fever);
			}
			Singleton<TaskStageTarget>.instance.AddScore(200, m_MusicData.objId, m_MusicData.noteData.ibms_id, m_MusicData.isAir, m_HitTime);
		}
		if (resulted)
		{
			Singleton<BattleEnemyManager>.instance.SetPlayResult(idx, (byte)((!isTotal) ? 3 : 4), false, true);
			Singleton<EventManager>.instance.Invoke(FeverManager.Instance.IsOnFeverState() ? ((!isTotal) ? "Battle/OnNoteGoldGreatHit" : "Battle/OnNoteGoldPerfectHit") : ((!isTotal) ? "Battle/OnNoteGreatHit" : "Battle/OnNotePerfectHit"));
		}
		if (m_MusicData.noteData.boss_action == "0" || string.IsNullOrEmpty(m_MusicData.noteData.boss_action))
		{
			SpineActionController.Play((!isTotal) ? "note_out_g" : "note_out_p", idx);
		}
		else
		{
			string actionKey = "hurt";
			if (m_MusicData.noteData.ibms_id == "17")
			{
				actionKey = "multi_atk_hurt_end";
			}
			SpineActionController.PlaySkeletonAnim(actionKey, -2);
			if (m_MusicData.noteData.ibms_id == "16")
			{
				SpineActionController.PlaySkeletonAnim("standby", -2, true, false);
			}
		}
		InvokeHitEnd();
		m_IsFlyAway = true;
	}

	private void InvokeHitEnd()
	{
		isHitting = false;
		Singleton<BattleProperty>.instance.isHpChangable = true;
		float duration = 0.083f;
		m_HittingCameraSequence.Kill(true);
		DOTween.Pause(m_Camera);
		DOTween.Pause(m_Camera.transform);
		m_Camera.orthographicSize = m_OriginSize * 0.84f;
		m_Camera.DOOrthoSize(m_OriginSize, duration).SetEase(Ease.OutQuad).OnComplete(delegate
		{
			m_Camera.orthographicSize = m_OriginSize;
		});
		Transform transform = m_Camera.transform;
		Vector3 position = m_Camera.transform.position;
		float y = position.y;
		Vector3 position2 = m_Camera.transform.position;
		transform.position = new Vector3(-1.08f, y, position2.z);
		m_Camera.transform.DOMoveX(0f, duration).SetEase(Ease.OutQuad);
		SpineActionController.Play("char_multihit_end", -1);
		Singleton<BattleEnemyManager>.instance.isMulHitting = false;
		hitCount = 0;
		Singleton<EventManager>.instance.Invoke("Battle/OnMultiHitChanged");
		isMulHitEnding = true;
		SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
		{
			SingletonMonoBehaviour<GirlManager>.instance.SetJumpingAction(false);
			SingletonMonoBehaviour<GirlManager>.instance.SetTone(false);
		}, 0.0166667f);
		SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
		{
			isMulHitEnding = false;
		}, 0.1333333f);
		isBanEmptyAction = true;
		SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
		{
			isBanEmptyAction = false;
		}, 0.3f);
	}
}
