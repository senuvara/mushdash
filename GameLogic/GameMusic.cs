using Assets.Scripts.GameCore.GameObjectLogics.GameObjectManager;
using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using DYUnityLib;
using FormulaBase;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
	public class GameMusic
	{
		public const byte NONE = 0;

		public const byte MISS = 1;

		public const byte COOL = 2;

		public const byte GREAT = 3;

		public const byte PERFECT = 4;

		public const byte JUMPOVER = 5;

		public const byte Fever = 6;

		public const uint TOUCH_ACTION_NONE = 0u;

		public const uint TOUCH_ACTION_SIGNLE_PRESS = 1u;

		public const uint TOUCH_ACTION_LONG_PRESS = 2u;

		public const uint TOUCH_ACTION_SLIDE_UP = 4u;

		public const uint TOUCH_ACTION_SLIDE_DOWN = 5u;

		public const int A_PERFECT_RANGE_INDEX = 0;

		public const int A_GREAT_RANGE_INDEX = 1;

		public const int B_PERFECT_RANGE_INDEX = 2;

		public const int B_GREAT_RANGE_INDEX = 3;

		public const int NODE_IS_NOT_A_BOSS = 0;

		public const int NODE_IS_BOSS_BODY_ATTACK = 1;

		public const int NODE_IS_BOSS_THROW_ATTACK = 2;

		public const string wheel_destroy = "destroy";

		private const string DEFAULT_ANIMATION = "AttackTargetObject/EnemyNormal";

		private decimal endHardTime = -1m;

		private FixUpdateTimer objTimer;

		private FixUpdateTimer stepTimer;

		private FixUpdateTimer sceneObjTimer;

		private float m_LastMusicTick;

		private int[][] m_MissMap;

		private int m_TnoIdx = -1;

		private Coroutine m_FullComboCoroutine1;

		private Coroutine m_FullComboCoroutine2;

		private Coroutine m_EndCoroutine;

		private int m_BlockNoDamageAirIndex = -1;

		private int m_BlockNoDamageGroundIndex = -1;

		private decimal m_CurTnoTime = -1m;

		private bool m_IsFullComboShow;

		private bool m_IsEnd;

		private float m_Gap;

		public bool invoke;

		private decimal m_DoubleBlockGap;

		public List<int> catchHeadNotes;

		public List<int> catchNotes;

		private float m_MulHitTime;

		private float m_MulHitHardTime;

		private List<float> m_DownHitTime = new List<float>();

		public void GameMusicFixTimerUpdate()
		{
			GameGlobal.gGameTouchPlay.TimeStep();
			if (!Singleton<AudioManager>.instance.bgm.isPlaying)
			{
				FixUpdateTimer.RollTimer();
				return;
			}
			int realTimeTick = Singleton<StageBattleComponent>.instance.realTimeTick;
			if (m_LastMusicTick == 0f)
			{
				m_LastMusicTick = realTimeTick;
			}
			m_LastMusicTick = realTimeTick;
			FixUpdateTimer.RollTimer();
		}

		public void TimerStepTrigger(object sender, uint triggerId, decimal ts)
		{
			if (Singleton<StageBattleComponent>.instance.isDead)
			{
				return;
			}
			invoke = true;
			decimal missHardTime = GameGlobal.gGameMissPlay.GetMissHardTime();
			if (missHardTime > 0m)
			{
				missHardTime -= 0.01m;
				GameGlobal.gGameMissPlay.SetMissHardTime(missHardTime);
			}
			int count = 0;
			TimeNodeOrder[] allTimeNodeByTick = Singleton<StageBattleComponent>.instance.GetAllTimeNodeByTick(ts, ref count);
			bool flag = Singleton<StageBattleComponent>.instance.IsAutoPlay();
			bool flag2 = false;
			bool flag3 = false;
			if (Singleton<BattleProperty>.instance.isCatchAvailable)
			{
				float time = (float)ts;
				int num = GameGlobal.gTouch.IsCatching(true, time);
				int num2 = GameGlobal.gTouch.IsCatching(false, time);
				Singleton<BattleProperty>.instance.isGroundCatching = (num >= 0);
				Singleton<BattleProperty>.instance.isAirCatching = (num2 >= 0);
				if (Singleton<BattleProperty>.instance.isAirCatching || Singleton<BattleProperty>.instance.isGroundCatching)
				{
					Catch(ts, num, num2);
					flag2 = Singleton<BattleEnemyManager>.instance.isGroundPressing;
					flag3 = Singleton<BattleEnemyManager>.instance.isAirPressing;
				}
			}
			if (flag)
			{
				AutoPlay(allTimeNodeByTick, count, ts);
			}
			int hp = BattleRoleAttributeComponent.instance.GetHp();
			if (Singleton<BattleProperty>.instance.reviveDeadline > 0 && !Singleton<StageBattleComponent>.instance.isDead && Singleton<BattleProperty>.instance.reviveDuration > 0m)
			{
				if (hp < Singleton<BattleProperty>.instance.reviveDeadline)
				{
					AttackEffectManager.instance.elfinRecoveryEffect.SetActive(true);
					AttackEffectManager.instance.roleRecoveryEffect.SetActive(true);
					if (!Singleton<BattleProperty>.instance.isReviveInvoked)
					{
						Singleton<EventManager>.instance.Invoke("Battle/OnFanRobotStartRevive");
						GameGlobal.gGameMissPlay.SetMissHardTime(Singleton<BattleProperty>.instance.skillMissHardTime);
						BattleRoleAttributeComponent.instance.MissHardEffect((float)Singleton<BattleProperty>.instance.skillMissHardTime);
					}
					Singleton<BattleProperty>.instance.isReviveInvoked = true;
				}
				if (Singleton<BattleProperty>.instance.isReviveInvoked && ts % Singleton<BattleProperty>.instance.reviveValue == 0m)
				{
					BattleRoleAttributeComponent.instance.AddHp(1);
					Singleton<BattleProperty>.instance.reviveDuration -= Singleton<BattleProperty>.instance.reviveValue;
				}
			}
			if (m_CurTnoTime >= Singleton<StageBattleComponent>.instance.lastTnoTime)
			{
				Singleton<BattleProperty>.instance.isHpChangable = false;
				if (!m_IsFullComboShow && !Singleton<StageBattleComponent>.instance.isTutorial)
				{
					m_IsFullComboShow = true;
					if (Singleton<TaskStageTarget>.instance.IsFullCombo() && Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>() != 2)
					{
						float gap = Singleton<AudioManager>.instance.bgm.clip.length - Singleton<AudioManager>.instance.bgm.time;
						if (gap < 3.5f)
						{
							m_Gap = 3.5f - gap;
						}
						else
						{
							m_Gap = 0f;
						}
						if (m_FullComboCoroutine1 != null)
						{
							SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_FullComboCoroutine1);
						}
						if (m_FullComboCoroutine2 != null)
						{
							SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_FullComboCoroutine2);
						}
						m_FullComboCoroutine2 = SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
						{
							gap -= 2f;
							m_FullComboCoroutine1 = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
							{
								if (m_IsFullComboShow && Singleton<TaskStageTarget>.instance.GetComboMax() > 0)
								{
									Singleton<StageBattleComponent>.instance.isInGame = false;
									if (Singleton<TaskStageTarget>.instance.IsFullCombo())
									{
										if (Singleton<BattleProperty>.instance.isGCScene)
										{
											Singleton<EventManager>.instance.Invoke("UI/OnFullComboGC");
										}
										else
										{
											Singleton<EventManager>.instance.Invoke("UI/OnFullCombo");
										}
									}
								}
							}, (!(gap < 1.5f)) ? gap : 1.5f);
						}, () => !Singleton<BattleEnemyManager>.instance.isMulHitting);
					}
				}
			}
			if (Singleton<BattleProperty>.instance.isHpChangable && Singleton<BattleProperty>.instance.hpChangedPerTime != 0m && hp > 0 && !Singleton<StageBattleComponent>.instance.isDead && ts % Singleton<BattleProperty>.instance.hpChangedPerTime == 0m)
			{
				int value = (int)(Singleton<BattleProperty>.instance.hpChangedPerTime / Math.Abs(Singleton<BattleProperty>.instance.hpChangedPerTime));
				BattleRoleAttributeComponent.instance.AddHp(value);
			}
			bool flag4 = false;
			bool flag5 = false;
			m_CurTnoTime = ts;
			if (allTimeNodeByTick != null)
			{
				for (int i = 0; i < count; i++)
				{
					TimeNodeOrder timeNodeOrder = allTimeNodeByTick[i];
					short idx = timeNodeOrder.idx;
					if (idx == 1 && !timeNodeOrder.isLongPressType && !timeNodeOrder.isMulType)
					{
						Singleton<BattleProperty>.instance.isHpChangable = true;
					}
					if (timeNodeOrder.isLongPressType)
					{
						if (!timeNodeOrder.isAir)
						{
							flag5 = true;
						}
						else
						{
							flag4 = true;
						}
						if (m_TnoIdx != idx)
						{
							m_TnoIdx = idx;
							if (timeNodeOrder.isLongPressStart)
							{
								if (!timeNodeOrder.isAir)
								{
									Singleton<StageBattleComponent>.instance.curPunchLpsIdx = m_TnoIdx;
								}
								else
								{
									Singleton<StageBattleComponent>.instance.curJumpLpsIdx = m_TnoIdx;
								}
							}
						}
						if (!flag && (timeNodeOrder.isAir || !flag2 || !catchNotes.Contains(timeNodeOrder.idx)) && (!timeNodeOrder.isAir || !flag3 || !catchNotes.Contains(timeNodeOrder.idx)))
						{
							LongPressController longPressController = (LongPressController)GameGlobal.gGameMusicScene.objCtrls[idx];
							longPressController.OnControllerHit(timeNodeOrder);
						}
					}
					if (timeNodeOrder.md.noteData.type == 6 || timeNodeOrder.md.noteData.type == 7)
					{
						if (!(timeNodeOrder.isAir ^ SingletonMonoBehaviour<GirlManager>.instance.IsAir()))
						{
							BaseSpineObjectController baseSpineObjectController = GameGlobal.gGameMusicScene.objCtrls[idx];
							GameGlobal.gGameTouchPlay.DisMissHardTime();
							if (Singleton<BattleEnemyManager>.instance.GetPlayResult(timeNodeOrder.idx) != 4)
							{
								Singleton<StatisticsManager>.instance.OnNoteResult(1);
							}
							baseSpineObjectController.OnControllerAttacked(4, false);
						}
						if (timeNodeOrder.isLast && Singleton<BattleEnemyManager>.instance.GetPlayResult(timeNodeOrder.idx) != 4)
						{
							Singleton<StatisticsManager>.instance.OnNoteResult(0);
						}
					}
					if (timeNodeOrder.isMuling)
					{
						MultHitEnemyController multHitEnemyController = (MultHitEnemyController)GameGlobal.gGameMusicScene.objCtrls[idx];
						multHitEnemyController.OnControllerTick(timeNodeOrder);
					}
					if (timeNodeOrder.md.noteData.type != 2)
					{
						continue;
					}
					byte playResult = Singleton<BattleEnemyManager>.instance.GetPlayResult(timeNodeOrder.idx);
					MusicData musicData = timeNodeOrder.md;
					for (int j = 1; j < 10; j++)
					{
						musicData = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(timeNodeOrder.idx + j);
						if (musicData.doubleIdx > 0)
						{
							break;
						}
					}
					bool flag6 = false;
					decimal d = musicData.tick - ts;
					if (musicData.doubleIdx > 0)
					{
						if (m_DoubleBlockGap == 0m)
						{
							m_DoubleBlockGap = timeNodeOrder.md.noteData.right_great_range + musicData.noteData.left_great_range + musicData.noteData.left_perfect_range;
						}
						if (d <= m_DoubleBlockGap)
						{
							byte playResult2 = Singleton<BattleEnemyManager>.instance.GetPlayResult(musicData.objId);
							bool flag7 = Singleton<BattleEnemyManager>.instance.IsPlayLeft(musicData.objId);
							byte playResult3 = Singleton<BattleEnemyManager>.instance.GetPlayResult(musicData.doubleIdx);
							bool flag8 = Singleton<BattleEnemyManager>.instance.IsPlayLeft(musicData.doubleIdx);
							flag6 = (((!flag7 || playResult2 != 3) && (!flag8 || playResult3 != 3)) ? true : false);
						}
					}
					bool flag9 = false;
					if (timeNodeOrder.result == 3 && playResult == 0)
					{
						if (!(timeNodeOrder.isAir ^ SingletonMonoBehaviour<GirlManager>.instance.IsAir()))
						{
							bool flag10 = true;
							if (Singleton<BattleEnemyManager>.instance.isGroundPressing || Singleton<BattleEnemyManager>.instance.isAirPressing || Singleton<BattleEnemyManager>.instance.isAirPressingHead || Singleton<BattleEnemyManager>.instance.isGroundPressingHead)
							{
								int idx2 = (!Singleton<BattleEnemyManager>.instance.isGroundPressing && !Singleton<BattleEnemyManager>.instance.isGroundPressingHead) ? Singleton<StageBattleComponent>.instance.curJumpLpsIdx : Singleton<StageBattleComponent>.instance.curPunchLpsIdx;
								flag10 = (Singleton<BattleEnemyManager>.instance.GetPlayResult(idx2) != 4);
							}
							if (MultHitEnemyController.isHitting)
							{
								flag10 = false;
							}
							if (GirlActionController.instance.isThisFrameJumpDown && !timeNodeOrder.md.isAir)
							{
								m_BlockNoDamageGroundIndex = timeNodeOrder.md.objId;
							}
							if (GirlActionController.instance.isThisFrameJumpUp && timeNodeOrder.md.isAir)
							{
								m_BlockNoDamageAirIndex = timeNodeOrder.md.objId;
							}
							if (m_BlockNoDamageGroundIndex == timeNodeOrder.md.objId || m_BlockNoDamageAirIndex == timeNodeOrder.md.objId)
							{
								int num3 = Mathf.RoundToInt((float)ts / 0.001f);
								int num4 = num3 - 5 + i;
								int num5 = num4 - (int)(timeNodeOrder.md.tick / 0.001m);
								if (num5 >= Singleton<BattleProperty>.instance.blockNoHurtRange)
								{
									flag6 = true;
								}
							}
							if (flag10 && !flag6)
							{
								Singleton<StatisticsManager>.instance.OnNoteResult(0);
								flag9 = GameGlobal.gGameMissPlay.MissCube(timeNodeOrder.idx, ts);
							}
						}
					}
					else if (timeNodeOrder.result == 4)
					{
						if (Singleton<BattleEnemyManager>.instance.isMulHitting)
						{
							Singleton<BattleEnemyManager>.instance.isMulHitting = false;
						}
						if (!timeNodeOrder.isAir)
						{
							MultHitEnemyController.isBanEmptyAction = false;
						}
					}
					SkeletonAnimation skeletonAnimation = GameGlobal.gGameMusicScene.animations[timeNodeOrder.idx];
					if (flag9 && playResult != 1 && skeletonAnimation.state.GetCurrent(0).Animation.Name != "destroy")
					{
						skeletonAnimation.state.SetAnimation(0, "destroy", false);
						GameObject gameObject = skeletonAnimation.gameObject;
						Vector3 position = gameObject.transform.position;
						Vector3 boneRealPosition = SpineActionController.GetBoneRealPosition("pc", gameObject);
						gameObject.transform.position = new Vector3(position.x + boneRealPosition.x, position.y + boneRealPosition.y, position.z + boneRealPosition.z);
						AttacksController.Instance.PlayOneShot("sfx_block", AttacksController.KeyAudioType.Others);
					}
					if (timeNodeOrder.isLast && playResult != 1 && !flag9 && Singleton<BattleEnemyManager>.instance.GetPlayResult(timeNodeOrder.idx) != 1)
					{
						int score = timeNodeOrder.md.noteData.score;
						score = Mathf.RoundToInt((float)score * Singleton<BattleProperty>.instance.blockNoteRate);
						Singleton<TaskStageTarget>.instance.AddScore(score, timeNodeOrder.md.objId, timeNodeOrder.md.noteData.ibms_id, timeNodeOrder.md.isAir);
						Singleton<BattleEnemyManager>.instance.SetPlayResult(timeNodeOrder.idx, 4);
						Singleton<TaskStageTarget>.instance.AddBlock(1);
						Singleton<StatisticsManager>.instance.OnNoteResult(1);
						Singleton<EventManager>.instance.Invoke((!SingletonMonoBehaviour<GirlManager>.instance.IsAir()) ? "Battle/OnNotePass" : "Battle/OnNotePassAir");
					}
				}
			}
			if (!flag5)
			{
				Singleton<StageBattleComponent>.instance.curPunchLpsIdx = -1;
			}
			if (!flag4)
			{
				Singleton<StageBattleComponent>.instance.curJumpLpsIdx = -1;
			}
			if (Singleton<StageBattleComponent>.instance.timeFromMusicStart >= Singleton<AudioManager>.instance.bgm.clip.length && !m_IsEnd)
			{
				m_IsEnd = true;
				Singleton<StageBattleComponent>.instance.isInGame = false;
				if (m_EndCoroutine != null)
				{
					SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_EndCoroutine);
				}
				m_EndCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					Singleton<StageBattleComponent>.instance.End();
				}, m_Gap);
			}
		}

		public void TimerTrigger(object sender, uint triggerId, params object[] args)
		{
			if (Singleton<BattleProperty>.instance.isAutoPlay)
			{
				return;
			}
			decimal d = (decimal)args[0];
			int num = (int)(d / 0.01m);
			decimal num2 = (decimal)Singleton<StageBattleComponent>.instance.timeFromMusicStart;
			if (!(num2 < (decimal)m_MissMap.Length))
			{
				return;
			}
			int[] array = m_MissMap[num];
			foreach (int idx in array)
			{
				MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
				if (musicDataByIdx.objId == 0)
				{
					break;
				}
				if (!musicDataByIdx.isLongPressStart && !musicDataByIdx.isLongPressEnd && !musicDataByIdx.isLongPressing && musicDataByIdx.noteData.type != 2 && musicDataByIdx.noteData.type != 9 && musicDataByIdx.noteData.type != 9 && musicDataByIdx.noteData.type != 9 && musicDataByIdx.noteData.type != 10 && musicDataByIdx.noteData.type != 11)
				{
					GameGlobal.gGameMissPlay.MissCube(idx, num2);
					if (musicDataByIdx.doubleIdx == -1)
					{
						break;
					}
				}
			}
		}

		private void AutoPlay(TimeNodeOrder[] tnos, int count, decimal ts)
		{
			if (m_DownHitTime.Contains(MathUtils.Round((float)ts, 2)))
			{
				SpineActionController.Play("char_downhit", -1);
			}
			if (tnos == null || count == 0)
			{
				return;
			}
			float timeFromMusicStart = Singleton<StageBattleComponent>.instance.timeFromMusicStart;
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				GameGlobal.gGameTouchPlay.isJumpBegan = false;
				GameGlobal.gGameTouchPlay.isPunchBegan = false;
				TimeNodeOrder timeNodeOrder = tnos[i];
				if (timeNodeOrder.md.noteData.type == 9 || timeNodeOrder.md.noteData.type == 10 || timeNodeOrder.md.noteData.type == 11)
				{
					continue;
				}
				if (timeNodeOrder.isLongPressType)
				{
					GameGlobal.gTouch.tickTime = (float)timeNodeOrder.md.tick;
					if (timeNodeOrder.isLongPressStart)
					{
						if (timeNodeOrder.isAir)
						{
							GameGlobal.gGameTouchPlay.isJumpBegan = true;
						}
						else
						{
							GameGlobal.gGameTouchPlay.isPunchBegan = true;
						}
						if (timeNodeOrder.isPerfectNode)
						{
							Singleton<BattleEnemyManager>.instance.SetLongPressEffect(true, timeNodeOrder.isAir);
							GameGlobal.gGameTouchPlay.TouchResult(timeNodeOrder.idx, 4, 1u);
						}
					}
					else
					{
						if (timeNodeOrder.isAir)
						{
							GameGlobal.gGameTouchPlay.isJumpStay = true;
						}
						else
						{
							GameGlobal.gGameTouchPlay.isPunchStay = true;
						}
						if (timeNodeOrder.isLongPressing && Singleton<BattleEnemyManager>.instance.GetPlayResult(timeNodeOrder.idx) == 0 && (Singleton<BattleEnemyManager>.instance.isGroundPressing || Singleton<BattleEnemyManager>.instance.isAirPressing))
						{
							Singleton<BattleEnemyManager>.instance.AddHp(timeNodeOrder.idx, -1);
							GameGlobal.gGameTouchPlay.TouchResult(timeNodeOrder.idx, 4, 1u);
						}
						if (timeNodeOrder.isLongPressEnd && timeNodeOrder.isPerfectNode)
						{
							GameGlobal.gGameTouchPlay.TouchResult(timeNodeOrder.idx, 4, 1u);
							Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, timeNodeOrder.isAir);
							int idx = LongPressController.GetIdx(timeNodeOrder.isAir);
							if (idx < GameGlobal.gGameMusicScene.spineActionCtrls.Length && idx >= 0)
							{
								SpineActionController spineActionController = GameGlobal.gGameMusicScene.spineActionCtrls[idx];
								spineActionController.DestroyLongPress();
							}
							if (timeNodeOrder.isAir)
							{
								Singleton<StageBattleComponent>.instance.curJumpLpsIdx = -1;
							}
							else
							{
								Singleton<StageBattleComponent>.instance.curPunchLpsIdx = -1;
							}
						}
					}
					if (timeNodeOrder.isAir)
					{
						GameGlobal.gGameTouchPlay.isJumpStay = true;
					}
					else
					{
						GameGlobal.gGameTouchPlay.isPunchStay = true;
					}
					GameGlobal.gGameTouchPlay.MoveTouchPhaser();
					timeNodeOrder.isFucked = true;
					continue;
				}
				if (!flag && (timeNodeOrder.isPerfectNode || timeNodeOrder.md.noteData.type == 6 || timeNodeOrder.md.noteData.type == 7))
				{
					GameGlobal.gTouch.tickTime = (float)timeNodeOrder.md.tick;
					if (timeNodeOrder.isAir || (timeNodeOrder.md.noteData.type == 2 && (timeNodeOrder.isRight || timeNodeOrder.isPerfectNode)))
					{
						bool flag2 = SingletonMonoBehaviour<GirlManager>.instance.IsJumpingAction();
						if (timeNodeOrder.md.noteData.type == 2)
						{
							if (flag2)
							{
								if (timeNodeOrder.isAir)
								{
									GameGlobal.gGameTouchPlay.isPunchBegan = true;
								}
							}
							else if (!timeNodeOrder.isAir)
							{
								GameGlobal.gGameTouchPlay.isJumpBegan = true;
							}
						}
						else if (!flag2 || timeNodeOrder.md.doubleIdx != -1)
						{
							GameGlobal.gGameTouchPlay.isJumpBegan = true;
						}
					}
					else
					{
						if ((timeNodeOrder.md.noteData.type == 6 || timeNodeOrder.md.noteData.type == 7) && !SingletonMonoBehaviour<GirlManager>.instance.IsJumpingAction())
						{
							continue;
						}
						GameGlobal.gGameTouchPlay.isPunchBegan = true;
					}
					if (timeNodeOrder.isMulStart)
					{
						m_MulHitTime = timeFromMusicStart;
					}
					if ((timeNodeOrder.md.noteData.type == 2 && (timeNodeOrder.isRight || timeNodeOrder.isPerfectNode)) || timeNodeOrder.md.noteData.type == 6 || timeNodeOrder.md.noteData.type == 7 || timeNodeOrder.md.doubleIdx > 0)
					{
						if ((timeNodeOrder.md.noteData.type == 6 || timeNodeOrder.md.noteData.type == 7) && Singleton<BattleEnemyManager>.instance.IsDead(timeNodeOrder.idx))
						{
							continue;
						}
						GameGlobal.gGameTouchPlay.BeginTouchPhaser();
					}
					else
					{
						if (timeNodeOrder.md.doubleIdx == -1)
						{
							flag = true;
						}
						else
						{
							GameGlobal.gGameTouchPlay.isDoubleBegan = true;
						}
						GameGlobal.gGameTouchPlay.TouchResult(timeNodeOrder.idx, 4, 1u);
					}
				}
				if (timeNodeOrder.isMuling && Singleton<BattleEnemyManager>.instance.GetPlayResult(timeNodeOrder.idx) > 1)
				{
					GameGlobal.gTouch.tickTime = (float)timeNodeOrder.md.tick;
					float num = timeFromMusicStart - m_MulHitTime;
					if (num > m_MulHitHardTime)
					{
						GameGlobal.gGameTouchPlay.TouchResult(timeNodeOrder.idx, 4, 1u);
						m_MulHitTime = timeFromMusicStart;
					}
				}
			}
		}

		private void Catch(decimal ts, int groundIndex, int airIndex)
		{
			int count = 0;
			TimeNodeOrder[] allTimeNodeByTick = Singleton<StageBattleComponent>.instance.GetAllTimeNodeByTick(ts, ref count);
			float timeFromMusicStart = Singleton<StageBattleComponent>.instance.timeFromMusicStart;
			bool flag = false;
			for (int i = 0; i < count; i++)
			{
				TimeNodeOrder timeNodeOrder = allTimeNodeByTick[i];
				if (!catchNotes.Contains(timeNodeOrder.md.objId) || (!timeNodeOrder.isAir && !Singleton<BattleProperty>.instance.isGroundCatching) || (timeNodeOrder.isAir && !Singleton<BattleProperty>.instance.isAirCatching))
				{
					continue;
				}
				int index = (!timeNodeOrder.isAir) ? groundIndex : airIndex;
				if (timeNodeOrder.isLongPressType)
				{
					GameGlobal.gTouch.tickTime = (float)timeNodeOrder.md.tick;
					if (timeNodeOrder.isLongPressStart)
					{
						if (timeNodeOrder.isAir)
						{
							GameGlobal.gGameTouchPlay.isJumpBegan = true;
						}
						else
						{
							GameGlobal.gGameTouchPlay.isPunchBegan = true;
						}
						if (timeNodeOrder.isPerfectNode)
						{
							Singleton<BattleEnemyManager>.instance.SetLongPressEffect(true, timeNodeOrder.isAir);
							GameGlobal.gGameTouchPlay.TouchResult(timeNodeOrder.idx, 4, 1u);
						}
					}
					else
					{
						if (timeNodeOrder.isLongPressing && Singleton<BattleEnemyManager>.instance.GetPlayResult(timeNodeOrder.idx) == 0 && (Singleton<BattleEnemyManager>.instance.isGroundPressing || Singleton<BattleEnemyManager>.instance.isAirPressing))
						{
							if (timeNodeOrder.isAir)
							{
								GameGlobal.gGameTouchPlay.isJumpStay = true;
							}
							else
							{
								GameGlobal.gGameTouchPlay.isPunchStay = true;
							}
							Singleton<BattleEnemyManager>.instance.AddHp(timeNodeOrder.idx, -1);
							GameGlobal.gGameTouchPlay.TouchResult(timeNodeOrder.idx, 4, 1u);
						}
						if (timeNodeOrder.isLongPressEnd && timeNodeOrder.isPerfectNode && Singleton<BattleEnemyManager>.instance.GetPlayResult(timeNodeOrder.idx) > 1)
						{
							GameGlobal.gGameTouchPlay.TouchResult(timeNodeOrder.idx, 4, 1u);
							Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, timeNodeOrder.isAir);
							int idx = LongPressController.GetIdx(timeNodeOrder.isAir);
							if (idx < GameGlobal.gGameMusicScene.spineActionCtrls.Length && idx >= 0)
							{
								SpineActionController spineActionController = GameGlobal.gGameMusicScene.spineActionCtrls[idx];
								spineActionController.DestroyLongPress();
							}
							if (timeNodeOrder.isAir)
							{
								Singleton<StageBattleComponent>.instance.curJumpLpsIdx = -1;
							}
							else
							{
								Singleton<StageBattleComponent>.instance.curPunchLpsIdx = -1;
							}
						}
					}
					GameGlobal.gGameTouchPlay.MoveTouchPhaser();
					GameGlobal.gTouch.RefreshCatchTime(index, (float)timeNodeOrder.md.tick);
				}
				else if (!flag && catchNotes.Contains(timeNodeOrder.idx) && (timeNodeOrder.isPerfectNode || timeNodeOrder.isRight) && Singleton<BattleEnemyManager>.instance.GetPlayResult(timeNodeOrder.idx) == 0)
				{
					GameGlobal.gTouch.tickTime = (float)timeNodeOrder.md.tick;
					if (timeNodeOrder.isMulStart)
					{
						m_MulHitTime = timeFromMusicStart;
					}
					if (timeNodeOrder.md.doubleIdx == -1)
					{
						flag = true;
					}
					else
					{
						GameGlobal.gGameTouchPlay.isDoubleBegan = true;
					}
					GameGlobal.gGameTouchPlay.isPunchBegan = !timeNodeOrder.isAir;
					GameGlobal.gGameTouchPlay.isJumpBegan = timeNodeOrder.isAir;
					GameGlobal.gGameTouchPlay.isPunch = !timeNodeOrder.isAir;
					GameGlobal.gGameTouchPlay.BeginTouchPhaser();
					GameGlobal.gTouch.RefreshCatchTime(index, (float)timeNodeOrder.md.tick);
				}
			}
		}

		public decimal GetMusicPassTick()
		{
			if (objTimer == null)
			{
				return 0m;
			}
			return objTimer.GetPassTick();
		}

		public void SetTimer(FixUpdateTimer timer)
		{
			objTimer = timer;
		}

		public void SetStepTimer(FixUpdateTimer timer)
		{
			stepTimer = timer;
		}

		public void SetSceneObjTimer(FixUpdateTimer timer)
		{
			sceneObjTimer = timer;
		}

		public bool IsRunning()
		{
			if (objTimer == null)
			{
				return false;
			}
			return objTimer.IsRunning();
		}

		public void Run()
		{
			if (objTimer == null)
			{
				Debug.Log("Run music with a null timer.");
				return;
			}
			objTimer.Run();
			if (stepTimer == null)
			{
				Debug.Log("Run music with a null step timer.");
				return;
			}
			stepTimer.Run();
			if (sceneObjTimer == null)
			{
				Debug.Log("Run music with a null scene obj timer.");
			}
			else
			{
				sceneObjTimer.Run();
			}
		}

		public void Stop()
		{
			if (objTimer == null)
			{
				Debug.Log("Stop music with a null timer.");
				return;
			}
			objTimer.Cancel();
			if (stepTimer == null)
			{
				Debug.Log("Stop music with a null step timer.");
				return;
			}
			stepTimer.Cancel();
			if (sceneObjTimer == null)
			{
				Debug.Log("Stop music with a null scene obj timer.");
			}
			else
			{
				sceneObjTimer.Cancel();
			}
		}

		public void Reset()
		{
			m_IsEnd = false;
			m_Gap = 0f;
			m_IsFullComboShow = false;
			catchNotes = new List<int>();
			m_CurTnoTime = -1m;
			catchNotes = new List<int>();
			catchHeadNotes = new List<int>();
			LongPressController.airFingerId = -1;
			LongPressController.groundFingerId = -1;
			LongPressController.index = -1;
			LongPressController.airIndex = -1;
			GameGlobal.gGameMissPlay.Init();
			GameGlobal.gGameTouchPlay.Init();
			InitData();
			InitTimer(240m);
			InitAutoPlay();
			if (m_FullComboCoroutine1 != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_FullComboCoroutine1);
				m_FullComboCoroutine1 = null;
			}
			if (m_FullComboCoroutine2 != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_FullComboCoroutine2);
				m_FullComboCoroutine2 = null;
			}
			if (m_EndCoroutine != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_EndCoroutine);
				m_EndCoroutine = null;
			}
		}

		public void LoadMusicDataByFileName()
		{
			InitEventTrigger();
			Reset();
		}

		public void SetEndHardTime(decimal t)
		{
			endHardTime = t;
		}

		public decimal GetEndHardTime()
		{
			return endHardTime;
		}

		public int GetMusicIndexByGenTick(decimal genTick)
		{
			List<MusicData> musicData = Singleton<StageBattleComponent>.instance.GetMusicData();
			if (musicData == null)
			{
				return -1;
			}
			if (musicData.Count <= 0)
			{
				return -1;
			}
			for (int i = 0; i < musicData.Count; i++)
			{
				MusicData musicData2 = musicData[i];
				if (musicData2.showTick == genTick && musicData2.objId > Singleton<BattleEnemyManager>.instance.GetCurrentGenIdx() && musicData2.objId != 0)
				{
					return musicData2.objId;
				}
			}
			return -1;
		}

		public int GetNodeIdByIdx(int idx)
		{
			List<MusicData> musicData = Singleton<StageBattleComponent>.instance.GetMusicData();
			if (musicData == null)
			{
				return 0;
			}
			if (idx < 0 || idx >= musicData.Count)
			{
				return 0;
			}
			MusicData musicData2 = musicData[idx];
			return musicData2.objId;
		}

		public string GetNodeAnimation(int idx)
		{
			List<MusicData> musicData = Singleton<StageBattleComponent>.instance.GetMusicData();
			if (musicData == null)
			{
				return "AttackTargetObject/EnemyNormal";
			}
			if (idx < 0 || idx >= musicData.Count)
			{
				return "AttackTargetObject/EnemyNormal";
			}
			MusicData musicData2 = musicData[idx];
			return musicData2.noteData.prefab_name;
		}

		private void InitData()
		{
			endHardTime = -1m;
			ResetMusicData();
		}

		private void InitAutoPlay()
		{
			m_DownHitTime = new List<float>();
			List<MusicData> list = new List<MusicData>(Singleton<StageBattleComponent>.instance.GetMusicData());
			float num = 0f;
			float num2 = 0.3f;
			float num3 = 0.5f;
			bool flag = true;
			list.Sort(delegate(MusicData l, MusicData r)
			{
				if (l.tick > r.tick)
				{
					return 1;
				}
				return (r.tick > l.tick) ? (-1) : 0;
			});
			for (int i = 0; i < list.Count; i++)
			{
				MusicData musicData = list[i];
				if (musicData.doubleIdx >= 0)
				{
					continue;
				}
				if (musicData.isAir && musicData.noteData.type != 2)
				{
					float num4 = (float)musicData.tick;
					if (musicData.noteData.type == 7 || musicData.noteData.type == 6)
					{
						float num5 = num4 - num;
						MusicData musicData2 = default(MusicData);
						if (i > 0)
						{
							musicData2 = list[i - 1];
						}
						if (num5 < num3 && num5 > num2 && (musicData2.isAir || musicData2.noteData.type == 2 || musicData2.objId == 0))
						{
							m_DownHitTime.Add(MathUtils.Round(num4 - 0.056f, 2));
						}
					}
					num = num4;
				}
				if (musicData.noteData.type == 2 && !musicData.isAir)
				{
					bool flag2 = true;
					float num6 = MathUtils.Round((float)musicData.tick, 2);
					int num7 = 20;
					if (i < list.Count - 1)
					{
						for (int j = i + 1; j < list.Count; j++)
						{
							MusicData musicData3 = list[j];
							if ((float)musicData3.tick > num6 + (float)num7 * 0.01f && musicData3.isAir && musicData3.noteData.type == 2)
							{
								break;
							}
							for (int k = 1; k <= num7; k++)
							{
								float num8 = num6 + (float)k * 0.01f;
								if (MathUtils.Round((float)musicData3.tick, 2) == num8)
								{
									flag2 = false;
									break;
								}
							}
						}
					}
					if (flag2)
					{
						float num9 = (float)musicData.tick;
						float num10 = num9 - num;
						if (num10 < num3 && num10 > num2 && !flag)
						{
							m_DownHitTime.Add(MathUtils.Round(num9 - 0.056f, 2));
						}
						num = num9;
					}
				}
				flag = ((musicData.noteData.type == 2) ? musicData.isAir : (!musicData.isAir));
			}
		}

		private void InitTimer(decimal total)
		{
			if (objTimer == null)
			{
				Debug.Log("Load music with null timer, before LoadMusicDataByFileName, call method SetTimer.");
				return;
			}
			objTimer.ClearTickEvent();
			objTimer.Init(total);
			MusicData musicData = Singleton<StageBattleComponent>.instance.GetMusicData().Last();
			int num = Mathf.RoundToInt((float)(musicData.tick + musicData.noteData.right_perfect_range) / 0.01f);
			m_MissMap = new int[num][];
			List<MusicData> musicData2 = Singleton<StageBattleComponent>.instance.GetMusicData();
			for (int i = 0; i < musicData2.Count; i++)
			{
				MusicData musicData3 = musicData2[i];
				if (musicData3.noteData.type != 0)
				{
					decimal d = musicData3.tick + musicData3.noteData.right_perfect_range + musicData3.noteData.right_great_range;
					if (musicData3.noteData.type == 2)
					{
						d = musicData3.tick + musicData3.noteData.right_perfect_range;
					}
					d = decimal.Ceiling(d / 0.01m) * 0.01m;
					int num2 = (int)(d / 0.01m);
					int[] array;
					if (num2 >= m_MissMap.Length)
					{
						array = new int[1]
						{
							musicData3.objId
						};
						Array.Resize(ref m_MissMap, num2 + 1);
					}
					else
					{
						array = m_MissMap[num2];
						array = ((array != null) ? array.Add(musicData3.objId) : new int[1]
						{
							musicData3.objId
						});
					}
					m_MissMap[num2] = array;
					objTimer.AddTickEvent(d, 0u);
				}
			}
			if (stepTimer == null)
			{
				Debug.Log("Load music with null step timer, before LoadMusicDataByFileName, call method SetStepTimer.");
				return;
			}
			total += Singleton<StageBattleComponent>.instance.GetEndTimePlus() + 0.5m;
			stepTimer.ClearTickEvent();
			stepTimer.Init(total, 1);
			stepTimer.AddTickEvent(0m, 4u);
			m_MulHitHardTime = 1f / float.Parse(SingletonScriptableObject<ConstanceManager>.instance["mulHitAutoThreshold"]);
		}

		private void ResetMusicData()
		{
			Singleton<StageBattleComponent>.instance.ResetAll();
		}

		private void InitEventTrigger()
		{
			GTrigger.UnRegEvent(0u);
			GTrigger.UnRegEvent(4u);
			EventTrigger eventTrigger = GTrigger.RegEvent(0u);
			eventTrigger.Trigger += TimerTrigger;
			EventTrigger eventTrigger2 = GTrigger.RegEvent(4u);
			eventTrigger2.TriggerDecimal += TimerStepTrigger;
		}
	}
}
