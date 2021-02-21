using Assets.Scripts.GameCore.GameObjectLogics.GameObjectManager;
using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using E7.Native;
using FormulaBase;
using GameLogic;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AttacksController : MonoBehaviour
{
	public enum KeyAudioType
	{
		Hit,
		Touch,
		Others
	}

	public const int FAIL_PLAY_IDX1 = -1;

	public const int FAIL_PLAY_IDX2 = -2;

	private static AttacksController instance;

	[SerializeField]
	public AttackEffectManager attackAttackEffectManager;

	public GameObject sceneCamera;

	public AudioSource girlAudioSource;

	private float m_KeyAudioVolume;

	private float m_PreVolume;

	private AudioSource m_LongPressAc;

	private List<MusicData> m_musicData;

	private MusicData m_md;

	private BaseSpineObjectController m_ctrl;

	private BaseEnemyObjectController m_cubeController;

	[HideInInspector]
	public AudioSource curHitAs;

	[HideInInspector]
	public AudioSource curTouchAs;

	[HideInInspector]
	public AudioSource curLAs;

	private AudioSource m_PreAs;

	public NativeAudioController curHitAudioSource;

	public NativeAudioController curTouchAudioSource;

	public NativeAudioController curLAudioSource;

	public bool showAttackAnim = true;

	private Coroutine m_StopHitAudioCoroutine;

	private Coroutine m_StopTouchAudioCoroutine;

	private Coroutine m_StopLAudioCoroutine;

	public static AttacksController Instance => instance;

	private void Start()
	{
		instance = this;
		m_KeyAudioVolume = Singleton<DataManager>.instance["GameConfig"]["SfxVolume"].GetResult<float>();
		m_PreVolume = Singleton<AudioManager>.instance.pool.gameObjects.First().GetComponent<AudioSource>().volume;
	}

	private void OnDestroy()
	{
		if ((bool)m_LongPressAc)
		{
			m_LongPressAc.volume = m_PreVolume;
		}
	}

	public void PlayOneShot(string keyAudio, KeyAudioType audioType)
	{
		if (m_KeyAudioVolume == 0f)
		{
			return;
		}
		bool flag = false;
		AudioSource[] curAs = new AudioSource[1];
		NativeAudioController nativeAudioController = null;
		Coroutine coroutine = null;
		int index = 0;
		if (audioType != 0)
		{
			if (audioType != KeyAudioType.Touch)
			{
				if (audioType == KeyAudioType.Others)
				{
					index = 2;
					curAs[0] = curLAs;
					nativeAudioController = curLAudioSource;
					coroutine = m_StopLAudioCoroutine;
				}
			}
			else
			{
				index = 1;
				curAs[0] = curTouchAs;
				nativeAudioController = curTouchAudioSource;
				coroutine = m_StopTouchAudioCoroutine;
			}
		}
		else
		{
			index = 0;
			curAs[0] = curHitAs;
			nativeAudioController = curHitAudioSource;
			coroutine = m_StopHitAudioCoroutine;
		}
		if (flag)
		{
			nativeAudioController?.SetVolume(0f);
			nativeAudioController = Singleton<AudioManager>.instance.PlayAndroidOneShot(keyAudio, m_KeyAudioVolume, index, delegate
			{
				if (audioType != 0)
				{
					if (audioType != KeyAudioType.Touch)
					{
						if (audioType == KeyAudioType.Others)
						{
							curLAudioSource = null;
						}
					}
					else
					{
						curTouchAudioSource = null;
					}
				}
				else
				{
					curHitAudioSource = null;
				}
			});
		}
		else
		{
			AudioSource audioSource2 = curAs[0];
			if ((bool)audioSource2 && audioSource2.isPlaying && audioSource2.gameObject.activeSelf && m_PreAs == audioSource2 && audioSource2 != SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs)
			{
				Singleton<PoolManager>.instance.FastDestroy(audioSource2.gameObject);
				if (coroutine != null)
				{
					SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(coroutine);
				}
			}
			AudioSource audioSource = Singleton<AudioManager>.instance.PlayOneShot(keyAudio, m_KeyAudioVolume, delegate
			{
				if ((bool)curAs[0])
				{
					curAs[0].mute = false;
				}
				if (audioType != 0)
				{
					if (audioType != KeyAudioType.Touch)
					{
						if (audioType == KeyAudioType.Others)
						{
							curLAs = null;
						}
					}
					else
					{
						curTouchAs = null;
					}
				}
				else
				{
					curHitAs = null;
				}
			});
			float delay = 0.8f;
			coroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				AudioClip audioClip = Singleton<AssetBundleManager>.instance.LoadFromName<AudioClip>(keyAudio);
				if (audioClip.length > delay && (bool)audioSource)
				{
					audioSource.mute = true;
				}
			}, delay);
			if ((bool)audioSource)
			{
				curAs[0] = audioSource;
			}
		}
		if (audioType != 0)
		{
			if (audioType != KeyAudioType.Touch)
			{
				if (audioType == KeyAudioType.Others)
				{
					curLAs = curAs[0];
					curLAudioSource = nativeAudioController;
					m_StopLAudioCoroutine = coroutine;
				}
			}
			else
			{
				curTouchAs = curAs[0];
				curTouchAudioSource = nativeAudioController;
				m_StopTouchAudioCoroutine = coroutine;
			}
		}
		else
		{
			curHitAs = curAs[0];
			curHitAudioSource = nativeAudioController;
			m_StopHitAudioCoroutine = coroutine;
		}
		m_PreAs = curAs[0];
	}

	public void ShowAttack(int id, uint resultCode, uint actionType, bool isContinue = false)
	{
		switch (resultCode)
		{
		case 1u:
			BeAttacked();
			return;
		case 2u:
		case 3u:
		case 4u:
		case 6u:
			ShowAttackEffect(id, resultCode, isContinue);
			return;
		case 5u:
			PlayJumpAnim();
			return;
		}
		if (!Singleton<BattleEnemyManager>.instance.isMulHitting)
		{
			if (actionType == 1)
			{
				PlayRandomHitNothingAnim();
			}
			else
			{
				PlayAttackAnim(-1, isContinue, resultCode);
			}
		}
	}

	public void OnShowAttack(int idx, uint result)
	{
		m_musicData = Singleton<StageBattleComponent>.instance.GetMusicData();
		m_md = m_musicData[idx];
		if (idx >= GameMusicScene.instance.objCtrls.Length)
		{
			return;
		}
		m_ctrl = GameMusicScene.instance.objCtrls[idx];
		if ((bool)m_ctrl)
		{
			m_cubeController = (m_ctrl as BaseEnemyObjectController);
			decimal shotPause = 0m;
			m_musicData[idx] = m_md;
			if (m_cubeController != null)
			{
				m_cubeController.SetShotPause(shotPause);
				m_cubeController.AttackedSuccessful(result, Singleton<BattleEnemyManager>.instance.IsDead(idx));
			}
		}
	}

	public void BeAttacked()
	{
	}

	private void ShowAttackEffect(int id, uint resultCode, bool isContinue)
	{
		string hitEffectByIdx = Singleton<BattleEnemyManager>.instance.GetHitEffectByIdx(id);
		bool flag = false;
		if (id < GameGlobal.gGameMusicScene.showEffects.Length && GameGlobal.gGameMusicScene.showEffects[id] != null)
		{
			flag = GameGlobal.gGameMusicScene.showEffects[id][SceneChangeController.curScene];
		}
		if ((hitEffectByIdx == "1" || flag) && attackAttackEffectManager != null)
		{
			attackAttackEffectManager.ShowPlayResult(resultCode, id);
		}
		if (showAttackAnim)
		{
			PlayAttackAnim(id, isContinue, resultCode);
		}
		MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(id);
		if (musicDataByIdx.isLongPressType)
		{
			if (!SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs && (Singleton<BattleEnemyManager>.instance.isGroundPressing || Singleton<BattleEnemyManager>.instance.isAirPressing))
			{
				SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs = Singleton<AudioManager>.instance.PlayLoop("sfx_press");
				m_LongPressAc = SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs;
				SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs.volume = m_KeyAudioVolume;
			}
		}
		else if (musicDataByIdx.noteData.type != 8)
		{
			PlayOneShot(musicDataByIdx.noteData.key_audio, KeyAudioType.Hit);
		}
	}

	private void PlayAttackAnim(int id, bool isContinue, uint result)
	{
		MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(id);
		string actKey;
		if (musicDataByIdx.isLongPressType)
		{
			actKey = "char_press";
		}
		else if (musicDataByIdx.noteData.jumpNote)
		{
			actKey = "char_jump";
		}
		else
		{
			actKey = "char_atk_p";
			if (result == 3)
			{
				actKey = "char_atk_g";
			}
		}
		if (isContinue)
		{
			SingletonMonoBehaviour<GirlManager>.instance.AttacksWithoutExchange(result, actKey, id);
		}
		else
		{
			SingletonMonoBehaviour<GirlManager>.instance.AttackWithExchange(result, actKey, id);
		}
	}

	private void PlayRandomHitNothingAnim()
	{
		string text = string.Empty;
		if (Singleton<BattleEnemyManager>.instance.isAirPressing || Singleton<BattleEnemyManager>.instance.isGroundPressing || MultHitEnemyController.isBanEmptyAction)
		{
			return;
		}
		if (GameGlobal.gGameTouchPlay.isJumpBegan && !SingletonMonoBehaviour<GirlManager>.instance.IsJumpingAction())
		{
			if (GameGlobal.gGameTouchPlay.isPunchStay && Singleton<BattleEnemyManager>.instance.isGroundPressing)
			{
				return;
			}
			text = "char_jump";
			SingletonMonoBehaviour<GirlManager>.instance.SetJumpingAction(true);
			Singleton<EventManager>.instance.Invoke("Battle/OnEmptyJump");
			PlayOneShot("char_common_empty_jump", KeyAudioType.Others);
		}
		if (GameGlobal.gGameTouchPlay.isPunchBegan)
		{
			if (GameGlobal.gGameTouchPlay.isJumpStay && Singleton<BattleEnemyManager>.instance.isAirPressing)
			{
				return;
			}
			text = (SingletonMonoBehaviour<GirlManager>.instance.IsJumpingAction() ? "char_downhit" : "char_atk_miss");
			Singleton<EventManager>.instance.Invoke("Battle/OnEmptyAtk");
			PlayOneShot("char_common_empty_atk", KeyAudioType.Others);
		}
		float tick = 0f;
		if (text == "char_downhit")
		{
			Animator animator = SingletonMonoBehaviour<GirlManager>.instance.animator;
			float num = animator.GetCurrentAnimatorStateInfo(0).length - animator.GetTime();
			if (num < 71f / (339f * (float)Math.PI))
			{
				tick = 0.0166666675f;
			}
		}
		SpineActionController.Play(text, -1, tick);
	}

	public void PlayJumpAnim()
	{
		SingletonMonoBehaviour<GirlManager>.instance.AttacksWithoutExchange(5u, "char_jump");
		Singleton<EventManager>.instance.Invoke("Battle/OnEmptyJump");
		PlayOneShot("char_common_empty_jump", KeyAudioType.Others);
	}
}
