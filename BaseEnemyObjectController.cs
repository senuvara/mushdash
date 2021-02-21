using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.Graphics;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using FormulaBase;
using GameLogic;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemyObjectController : BaseSpineObjectController
{
	public const string POINT_CENTER = "pc";

	private static string[] listResultMap = new string[6]
	{
		string.Empty,
		string.Empty,
		"note_out_g",
		"note_out_g",
		"note_out_p",
		"char_jump"
	};

	[SerializeField]
	public bool attackedDoNothing;

	private SpineMountController m_SpineMountController;

	private SkeletonAnimation m_SkeletonAnimation;

	private GameObject m_CatchObj;

	protected MusicData m_MusicData;

	private int m_HpAdd;

	private SkeletonAnimation m_BloodAnimtion;

	private float m_ShowTick;

	private bool m_HasMiss;

	private bool m_HasNoteResult;

	public int nextNoteResult;

	public bool IsEmptyNode()
	{
		return idx >= GameMusicScene.instance.animations.Length || !GameMusicScene.instance.animations[idx];
	}

	public void AttackedSuccessful(uint result, bool isDead = true)
	{
		if (!attackedDoNothing)
		{
			OnControllerAttacked((int)result, isDead);
			GameGlobal.gGameMusicScene.OnObjBeAttacked(idx);
			OnAttackDestory();
		}
	}

	public virtual bool IsShotPause()
	{
		return false;
	}

	public virtual void SetShotPause(decimal tick)
	{
	}

	public virtual void OnAttackDestory()
	{
	}

	public override void SetIdx(int idx)
	{
		base.idx = idx;
	}

	public override void Init()
	{
		m_Renderer = base.gameObject.GetComponent<Renderer>();
		Transform transform = base.transform.Find("Catch");
		if ((bool)transform)
		{
			m_CatchAir = transform.GetChild(0).GetComponent<SpriteRenderer>();
			m_CatchGround = transform.GetChild(1).GetComponent<SpriteRenderer>();
		}
		m_SpineMountController = base.gameObject.GetComponent<SpineMountController>();
		m_SkeletonAnimation = base.gameObject.GetComponent<SkeletonAnimation>();
		m_MusicData = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
		if (Singleton<StageBattleComponent>.instance.isSceneChangeType && base.gameObject.name[1] != '0' && base.gameObject.name[1] != 'o' && base.gameObject.name[1] != 'm' && !base.gameObject.name.Contains("boss"))
		{
			m_MusicData.dt = (decimal)Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(base.gameObject.name.Substring(0, base.gameObject.name.IndexOf("("))).GetComponent<SpineActionController>().startDelay;
		}
		if (m_MusicData.configData.blood && m_MusicData.objId != 0)
		{
			m_HpAdd = 80;
			MakeBlood();
			Singleton<EffectManager>.instance.Preload("fx_hp_ground");
		}
		m_ShowTick = (float)(m_MusicData.tick - m_MusicData.dt);
		if (idx > 0)
		{
			if ((bool)m_Renderer)
			{
				m_Renderer.enabled = false;
			}
			if ((bool)m_CatchAir)
			{
				m_CatchAir.enabled = false;
			}
			if ((bool)m_CatchGround)
			{
				m_CatchGround.enabled = false;
			}
			if ((bool)m_BloodAnimtion)
			{
				m_BloodAnimtion.gameObject.SetActive(false);
			}
			base.isIn = false;
		}
		Transform transform2 = base.transform.Find("Catch");
		if ((bool)transform2)
		{
			m_CatchObj = transform2.gameObject;
		}
	}

	public override bool ControllerMissCheck(int i, decimal currentTick)
	{
		if (Singleton<StageBattleComponent>.instance.isDead)
		{
			return false;
		}
		bool flag = SingletonMonoBehaviour<GirlManager>.instance.IsAir();
		decimal missHardTime = GameGlobal.gGameMissPlay.GetMissHardTime();
		MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(i);
		if (musicDataByIdx.noteData.type == 0)
		{
			return false;
		}
		if (Singleton<StatisticsManager>.instance.isTutorial && (musicDataByIdx.noteData.type == 1 || musicDataByIdx.noteData.type == 4 || musicDataByIdx.noteData.type == 5 || musicDataByIdx.noteData.type == 9 || musicDataByIdx.noteData.type == 10 || musicDataByIdx.noteData.type == 11) && Singleton<BattleEnemyManager>.instance.GetPlayResult(musicDataByIdx.objId) == 0 && !m_HasNoteResult)
		{
			m_HasNoteResult = true;
			if (musicDataByIdx.doubleIdx > 0 && musicDataByIdx.doubleIdx != 9999)
			{
				if (musicDataByIdx.isAir)
				{
					if (!Singleton<BattleEnemyManager>.instance.IsDead(i))
					{
						Singleton<StatisticsManager>.instance.OnNoteResult(0);
					}
				}
				else if (!Singleton<BattleEnemyManager>.instance.IsDead(i))
				{
					if (!Singleton<BattleEnemyManager>.instance.IsDead(musicDataByIdx.doubleIdx))
					{
						Singleton<StatisticsManager>.instance.OnNoteResult(0);
						BaseSpineObjectController baseSpineObjectController = GameGlobal.gGameMusicScene.objCtrls[musicDataByIdx.doubleIdx];
						BaseEnemyObjectController baseEnemyObjectController = baseSpineObjectController as BaseEnemyObjectController;
						if ((bool)baseEnemyObjectController)
						{
							baseEnemyObjectController.nextNoteResult = -1;
						}
					}
					else
					{
						Singleton<StatisticsManager>.instance.OnNoteResult(0);
					}
				}
			}
			else
			{
				Singleton<StatisticsManager>.instance.OnNoteResult(0);
			}
		}
		if (missHardTime > 0m)
		{
			return false;
		}
		if (musicDataByIdx.doubleIdx > 0 && musicDataByIdx.doubleIdx != 9999)
		{
			if (Singleton<BattleEnemyManager>.instance.IsDead(i) && Singleton<BattleEnemyManager>.instance.IsDead(musicDataByIdx.doubleIdx))
			{
				return false;
			}
			byte playResult = Singleton<BattleEnemyManager>.instance.GetPlayResult(musicDataByIdx.doubleIdx);
			if (!musicDataByIdx.isAir)
			{
				if (flag)
				{
					return false;
				}
				Singleton<StageBattleComponent>.instance.SetCombo(0);
				Singleton<StageBattleComponent>.instance.SetCombo(0);
				return true;
			}
			if (flag)
			{
				Singleton<StageBattleComponent>.instance.SetCombo(0);
				Singleton<StageBattleComponent>.instance.SetCombo(0);
				return true;
			}
			if (playResult == 0)
			{
				return false;
			}
		}
		if (Singleton<BattleEnemyManager>.instance.IsDead(i))
		{
			return false;
		}
		if (musicDataByIdx.noteData.type != 4 && musicDataByIdx.noteData.type != 2 && musicDataByIdx.noteData.type != 6 && musicDataByIdx.noteData.type != 7 && musicDataByIdx.noteData.type != 8 && musicDataByIdx.noteData.type != 9 && musicDataByIdx.noteData.type != 10 && musicDataByIdx.noteData.type != 11)
		{
			Singleton<StageBattleComponent>.instance.SetCombo(0);
		}
		if (!string.IsNullOrEmpty(musicDataByIdx.noteData.boss_action) && musicDataByIdx.noteData.boss_action != "0" && (musicDataByIdx.noteData.type == 5 || musicDataByIdx.noteData.type == 8))
		{
			return true;
		}
		if (currentTick == -5m)
		{
			return false;
		}
		if (musicDataByIdx.noteData.type == 8)
		{
			return true;
		}
		if (musicDataByIdx.noteData.jumpNote && flag)
		{
			return false;
		}
		if (musicDataByIdx.noteData.pathway == 0 && flag)
		{
			return false;
		}
		if (musicDataByIdx.isAir)
		{
			return flag;
		}
		if (musicDataByIdx.isLongPressType)
		{
			return true;
		}
		return true;
	}

	public override void OnControllerStart()
	{
		base.isIn = false;
		SpineActionController.Play("in", idx);
		if ((bool)m_SkeletonAnimation && idx > 0)
		{
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				Action action = delegate
				{
					bool rendererEnable = GameGlobal.gGameMusicScene.preloads[idx] == base.gameObject;
					float currentTick = Singleton<StageBattleComponent>.instance.timeFromMusicStart;
					float dt = currentTick - m_ShowTick;
					TrackEntry currentState = m_SkeletonAnimation.state.GetCurrent(0);
					currentState.TrackTime = dt;
					if (dt != 0f)
					{
						if (dt < 0f)
						{
							Action callback = delegate
							{
								currentTick = Singleton<StageBattleComponent>.instance.timeFromMusicStart;
								dt = currentTick - m_ShowTick;
								currentState.TrackTime = dt;
								SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
								{
									if ((bool)m_Renderer && rendererEnable)
									{
										SetRendererEnabled();
										if ((bool)m_CatchAir)
										{
											m_CatchAir.enabled = true;
										}
										if ((bool)m_CatchGround)
										{
											m_CatchGround.enabled = true;
										}
										base.isIn = true;
									}
									if ((bool)m_BloodAnimtion)
									{
										m_BloodAnimtion.gameObject.SetActive(true);
									}
								}, 1, GraphicSettings.isOverOneHundred);
							};
							if (!GraphicSettings.isOverOneHundred)
							{
								SingletonMonoBehaviour<CoroutineManager>.instance.Delay(callback, 0f - dt);
							}
							else
							{
								SingletonMonoBehaviour<CoroutineManager>.instance.Delay(callback, Mathf.CeilToInt((0f - dt) / 0.01f), GraphicSettings.isOverOneHundred);
							}
						}
						if (dt > 0f)
						{
							SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
							{
								if ((bool)m_Renderer && rendererEnable)
								{
									SetRendererEnabled();
									if ((bool)m_CatchAir)
									{
										m_CatchAir.enabled = true;
									}
									if ((bool)m_CatchGround)
									{
										m_CatchGround.enabled = true;
									}
									base.isIn = true;
								}
								if ((bool)m_BloodAnimtion)
								{
									m_BloodAnimtion.gameObject.SetActive(true);
								}
							}, 1, GraphicSettings.isOverOneHundred);
						}
					}
					else
					{
						SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
						{
							if ((bool)m_Renderer && rendererEnable)
							{
								SetRendererEnabled();
								if ((bool)m_CatchAir)
								{
									m_CatchAir.enabled = true;
								}
								if ((bool)m_CatchGround)
								{
									m_CatchGround.enabled = true;
								}
								base.isIn = true;
							}
							if ((bool)m_BloodAnimtion)
							{
								m_BloodAnimtion.gameObject.SetActive(true);
							}
						}, 1, GraphicSettings.isOverOneHundred);
					}
				};
				if (Singleton<StageBattleComponent>.instance.isPause)
				{
					SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(action, () => !Singleton<StageBattleComponent>.instance.isPause);
				}
				else
				{
					action();
				}
			}, 1, GraphicSettings.isOverOneHundred);
		}
		string boss_action = m_MusicData.noteData.boss_action;
		if (boss_action != null && boss_action != "0")
		{
			Boss.Instance.Play(boss_action);
		}
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
		{
			if ((bool)m_SpineMountController)
			{
				m_SpineMountController.enabled = true;
				m_SpineMountController.OnControllerStart();
			}
		}, 0.1f);
	}

	private void SetRendererEnabled()
	{
		if (Singleton<StageBattleComponent>.instance.isSceneChangeType)
		{
			if (m_MusicData.noteData.prefab_name[1].ToString() == SceneChangeController.curScene.ToString() || m_MusicData.noteData.scene == "0")
			{
				m_Renderer.enabled = true;
			}
		}
		else
		{
			m_Renderer.enabled = true;
		}
	}

	public override void OnControllerAttacked(int result, bool isDeaded)
	{
		if (result > 4)
		{
			result = 4;
		}
		if (!isDeaded)
		{
			SpineActionController.Play("char_hurt", idx);
			return;
		}
		if (m_MusicData.configData.blood)
		{
			m_BloodAnimtion.state.SetAnimation(0, "out", false);
			int hp = BattleRoleAttributeComponent.instance.GetHp();
			decimal num = 0m;
			if (hp < Singleton<BattleProperty>.instance.hpRevive && Singleton<BattleProperty>.instance.hpSkillCount > 0)
			{
				Singleton<BattleProperty>.instance.hpSkillCount--;
				m_HpAdd = Mathf.RoundToInt((float)m_HpAdd * Singleton<BattleProperty>.instance.reviveRate);
				num = Singleton<BattleProperty>.instance.skillMissHardTime;
			}
			if (Singleton<BattleProperty>.instance.isBloodMissHardTime && Singleton<BattleProperty>.instance.missHardTime > num)
			{
				num = Singleton<BattleProperty>.instance.missHardTime;
			}
			if (num > 0m)
			{
				BattleRoleAttributeComponent.instance.MissHardEffect((float)num);
				GameGlobal.gGameMissPlay.SetMissHardTime(num);
			}
			if (hp >= BattleRoleAttributeComponent.instance.GetHpMax())
			{
				Singleton<TaskStageTarget>.instance.AddScore(Mathf.RoundToInt(300f * Singleton<BattleProperty>.instance.heartNoteRate), m_MusicData.objId, m_MusicData.noteData.ibms_id, m_MusicData.isAir);
			}
			BattleRoleAttributeComponent.instance.AddHp(m_HpAdd);
			Singleton<TaskStageTarget>.instance.AddBlood(1);
			if (Singleton<BattleProperty>.instance.godTimeCount == 0 && !Singleton<BattleProperty>.instance.isInGod && Singleton<BattleProperty>.instance.hasGodChance)
			{
				Singleton<BattleProperty>.instance.godTimeCount = 1;
			}
			if (Singleton<BattleProperty>.instance.isNekoSkillTrigger)
			{
				Singleton<TaskStageTarget>.instance.AddScore(Mathf.RoundToInt(300f * Singleton<BattleProperty>.instance.heartNoteRate), m_MusicData.objId, m_MusicData.noteData.ibms_id, m_MusicData.isAir);
				Singleton<EventManager>.instance.Invoke((!m_MusicData.isAir) ? "Battle/OnScoreGet" : "Battle/OnScoreGetAir", Singleton<TaskStageTarget>.instance.GetAddScore());
			}
			else
			{
				GameObject gameObject = Singleton<EffectManager>.instance.Play("fx_hp_ground");
				Transform transform = gameObject.transform;
				Vector3 position = gameObject.transform.position;
				float x = position.x;
				float y = (!m_MusicData.isAir) ? (-0.94f) : 0.94f;
				Vector3 position2 = gameObject.transform.position;
				transform.position = new Vector3(x, y, position2.z);
				Singleton<EventManager>.instance.Invoke((!m_MusicData.isAir) ? "Battle/OnHpGet" : "Battle/OnHpGetAir", m_HpAdd);
			}
			AttacksController.Instance.PlayOneShot("sfx_hp", AttacksController.KeyAudioType.Touch);
		}
		string actionKey = listResultMap[result];
		Vector3 position3 = base.gameObject.transform.position;
		Vector3 boneRealPosition = SpineActionController.GetBoneRealPosition("pc", base.gameObject);
		base.gameObject.transform.position = new Vector3(position3.x + boneRealPosition.x, position3.y + boneRealPosition.y, position3.z + boneRealPosition.z);
		SpineActionController.Play(actionKey, idx);
		if ((bool)m_CatchObj)
		{
			m_CatchObj.SetActive(false);
		}
		if (m_MusicData.noteData.type == 5)
		{
			Boss.Instance.Play("boss_hurt");
		}
		if (m_MusicData.doubleIdx > 0 && m_MusicData.doubleIdx != 9999 && Singleton<BattleEnemyManager>.instance.GetPlayResult(m_MusicData.doubleIdx) < 2)
		{
			SpineActionController spineActionController = GameGlobal.gGameMusicScene.spineActionCtrls[m_MusicData.doubleIdx];
			float trackTime = spineActionController.skAnimation.state.GetCurrent(0).TrackTime;
			SpineActionController.Play("note_charge", m_MusicData.doubleIdx);
			spineActionController.skAnimation.state.GetCurrent(0).TrackTime = trackTime;
		}
		if (m_MusicData.isLongPressType)
		{
			return;
		}
		if (m_MusicData.noteData.type == 1)
		{
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				Singleton<StatisticsManager>.instance.OnNoteResult(1);
			}, 0.25f);
		}
		else
		{
			Singleton<StatisticsManager>.instance.OnNoteResult(1);
		}
	}

	public override bool OnControllerMiss(int idx)
	{
		bool flag = __OnControllerMiss(idx);
		Singleton<BattleEnemyManager>.instance.SetPlayResult(idx, 1);
		if (flag)
		{
			AttacksController.Instance.BeAttacked();
		}
		OnAttackDestory();
		return flag;
	}

	private void MakeBlood()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>("hp_on_note"));
		m_BloodAnimtion = gameObject.GetComponent<SkeletonAnimation>();
		BoneFollower component = gameObject.GetComponent<BoneFollower>();
		component.skeletonRenderer = m_SkeletonAnimation;
		component.boneName = "hp";
		gameObject.transform.SetParent(base.transform, false);
		gameObject.transform.position = new Vector3(0f, -999f, 0f);
		gameObject.SetActive(false);
	}

	private bool __OnControllerMiss(int idx)
	{
		List<MusicData> musicData = Singleton<StageBattleComponent>.instance.GetMusicData();
		if (idx < 0 || idx > musicData.Count)
		{
			return false;
		}
		MusicData musicData2 = musicData[idx];
		if (musicData2.noteData.type == 0)
		{
			return false;
		}
		if (!musicData2.noteData.missCombo)
		{
			return false;
		}
		if (m_HasMiss)
		{
			return false;
		}
		m_HasMiss = true;
		int damageValueByIndex = Singleton<BattleEnemyManager>.instance.GetDamageValueByIndex(idx);
		damageValueByIndex -= Singleton<BattleProperty>.instance.hurtReduce;
		BattleRoleAttributeComponent.instance.Hurt(-damageValueByIndex, SingletonMonoBehaviour<GirlManager>.instance.IsAir());
		if (damageValueByIndex > 0 && Singleton<InputManager>.instance.isVibration)
		{
			Singleton<InputManager>.instance.SetNsVibration(InputManager.VibrationVaule.Default, InputManager.DeviceHandles.Both, 0.3f);
		}
		Singleton<StageBattleComponent>.instance.SetCombo(0, musicData2.noteData.type == 2 || musicData2.noteData.type == 8);
		GameGlobal.gGameMusicScene.OnObjBeMissed(base.idx);
		return true;
	}
}
