using Assets.Scripts.GameCore.GameObjectLogics.GameObjectManager;
using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.Graphics;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using DYUnityLib;
using FormulaBase;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
	public class GameTouchPlay
	{
		public static bool isLeftRight = true;

		public static bool isBtnLeftRight = true;

		public static bool isReverse;

		public bool isPunchBegan;

		public bool isJumpBegan;

		public bool isPunchStay;

		public bool isJumpStay;

		public bool isPunchEnded;

		public bool isJumpEnded;

		public bool isDoubleBegan;

		public bool isPunch;

		private float m_DoubleHardTime;

		private float m_PunchBeganTime;

		private float m_JumpBeganTime;

		public List<int> beganGroundIndexs;

		public List<int> beganAirIndexs;

		public int widthMid;

		public int heightMid;

		private readonly decimal m_PressHardTime = SingletonScriptableObject<ConstanceManager>.instance.GetDecimal("attackHardTime");

		private decimal m_PunchHardTime = -1m;

		private decimal m_JumpHardTime = -1m;

		private float m_MovePassTick;

		private byte m_ResultPunch;

		private byte m_ResultJump;

		private MusicData m_ParentMdPunch;

		private MusicData m_ParentMdJump;

		private SpineActionController m_SacPunch;

		private SpineActionController m_SacJump;

		private float m_PercentPunch;

		private float m_PercentJump;

		public bool hasReactAir;

		public bool hasReactGround;

		private List<TimeNodeOrder> m_TimeNodeOrders;

		public bool isReactAir
		{
			get;
			private set;
		}

		public bool isReactGround
		{
			get;
			private set;
		}

		public float catchReactAirTime
		{
			get;
			private set;
		}

		public float catchReactGroundTime
		{
			get;
			private set;
		}

		public void Init()
		{
			isLeftRight = Singleton<DataManager>.instance["Account"]["IsLeftRight"].GetResult<bool>();
			isReverse = Singleton<DataManager>.instance["Account"]["IsReverse"].GetResult<bool>();
			if (Singleton<StageBattleComponent>.instance.isTutorial)
			{
				isLeftRight = true;
				isBtnLeftRight = true;
				isReverse = false;
				Singleton<InputManager>.instance.SwitchHandleProposal((!Singleton<StageBattleComponent>.instance.isTutorial) ? Singleton<InputManager>.instance.handleProposal : "Default");
				Singleton<InputManager>.instance.SwitchButtonProposal("Default");
			}
			if (isReverse && Singleton<InputManager>.instance.currentControllerName != "Keyboard")
			{
				isBtnLeftRight = false;
			}
			else if (!isReverse && Singleton<InputManager>.instance.currentControllerName != "Keyboard")
			{
				isBtnLeftRight = true;
			}
			m_PunchHardTime = -1m;
			m_JumpHardTime = -1m;
			m_DoubleHardTime = 0.22f;
			GTrigger.UnRegEvent(2u);
			EventTrigger eventTrigger = GTrigger.RegEvent(2u);
			GameGlobal.gTouch.AddCustomEvent(eventTrigger);
			eventTrigger.Trigger += TouchTrigger;
			widthMid = GraphicSettings.curScreenWidth / 2;
			heightMid = GraphicSettings.curScreenHeight / 2;
			beganGroundIndexs = new List<int>();
			beganAirIndexs = new List<int>();
		}

		public void TimeStep()
		{
			if (m_PunchHardTime >= 0m)
			{
				m_PunchHardTime -= 0.01m;
			}
			if (m_JumpHardTime >= 0m)
			{
				m_JumpHardTime -= 0.01m;
			}
		}

		public void Reset()
		{
			isPunchBegan = false;
			isJumpBegan = false;
			isPunchStay = false;
			isJumpStay = false;
			isPunchEnded = false;
			isJumpEnded = false;
		}

		public void TouchTrigger(object sender, uint triggerId, params object[] args)
		{
			if (!Singleton<StageBattleComponent>.instance.isDead && Singleton<StageBattleComponent>.instance.isInGame && !SingletonMonoBehaviour<GirlManager>.instance.isCommingOut && !Singleton<StageBattleComponent>.instance.IsAutoPlay())
			{
				uint num = (uint)args[0];
				isJumpBegan = GameGlobal.gTouch.IsJumpTouch();
				isPunchBegan = GameGlobal.gTouch.IsPunchTouch();
				isJumpEnded = GameGlobal.gTouch.IsJumpTouch(TouchPhase.Ended);
				isPunchEnded = GameGlobal.gTouch.IsPunchTouch(TouchPhase.Ended);
				isJumpStay = GameGlobal.gTouch.IsJumpTouch(TouchPhase.Moved);
				isPunchStay = GameGlobal.gTouch.IsPunchTouch(TouchPhase.Moved);
				isDoubleBegan = false;
				switch (num)
				{
				case 6u:
					BeginTouchPhaser();
					break;
				case 7u:
					MoveTouchPhaser();
					break;
				case 8u:
					EndTouchPhaser();
					break;
				}
			}
		}

		public void EndTouchPhaser()
		{
			if (Singleton<StageBattleComponent>.instance.curPunchLpsIdx == -1 && Singleton<StageBattleComponent>.instance.curJumpLpsIdx == -1)
			{
				return;
			}
			bool isAir = isJumpEnded;
			float timeFromMusicStart = Singleton<StageBattleComponent>.instance.timeFromMusicStart;
			List<TimeNodeOrder> list = Singleton<StageBattleComponent>.instance.GetTimeNodeByTick(timeFromMusicStart);
			if (list != null)
			{
				list = list.Where((TimeNodeOrder t) => t.isLongPressType && t.isAir == isAir);
			}
			if (list == null || list.Count == 0)
			{
				MusicData musicData = (!isAir) ? Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(Singleton<StageBattleComponent>.instance.curPunchLpsIdx) : Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(Singleton<StageBattleComponent>.instance.curJumpLpsIdx);
				decimal tick = musicData.tick + decimal.Round(musicData.configData.length, 3);
				list = Singleton<StageBattleComponent>.instance.GetTimeNodeByTick(tick);
			}
			if (list != null && list.Count > 0)
			{
				if (isJumpEnded)
				{
					TimeNodeOrder timeNodeOrder = list.Find((TimeNodeOrder c) => c.isAir && c.isLongPressEnd);
					if (timeNodeOrder != null)
					{
						int curJumpLpsIdx = Singleton<StageBattleComponent>.instance.curJumpLpsIdx;
						if (curJumpLpsIdx != -1)
						{
							byte playResult = Singleton<BattleEnemyManager>.instance.GetPlayResult(curJumpLpsIdx);
							if (playResult > 1 && timeNodeOrder.idx < GameGlobal.gGameMusicScene.objCtrls.Length)
							{
								BaseSpineObjectController baseSpineObjectController = GameGlobal.gGameMusicScene.objCtrls[timeNodeOrder.idx];
								if ((bool)baseSpineObjectController)
								{
									LongPressController x = baseSpineObjectController as LongPressController;
									if (x != null)
									{
									}
									for (int i = curJumpLpsIdx; i < timeNodeOrder.idx; i++)
									{
										if (i >= GameGlobal.gGameMusicScene.objCtrls.Length)
										{
											continue;
										}
										BaseSpineObjectController baseSpineObjectController2 = GameGlobal.gGameMusicScene.objCtrls[i];
										if ((bool)baseSpineObjectController2)
										{
											LongPressController longPressController = baseSpineObjectController2 as LongPressController;
											if (longPressController != null && longPressController.isOnAir)
											{
												longPressController.isActive = false;
											}
										}
									}
								}
							}
						}
					}
					Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, true);
				}
				if (!isPunchEnded)
				{
					return;
				}
				TimeNodeOrder timeNodeOrder2 = list.Find((TimeNodeOrder c) => !c.isAir && c.isLongPressEnd);
				if (timeNodeOrder2 != null)
				{
					int curPunchLpsIdx = Singleton<StageBattleComponent>.instance.curPunchLpsIdx;
					if (curPunchLpsIdx != -1)
					{
						byte playResult2 = Singleton<BattleEnemyManager>.instance.GetPlayResult(curPunchLpsIdx);
						if (playResult2 > 1 && timeNodeOrder2.idx < GameGlobal.gGameMusicScene.objCtrls.Length)
						{
							BaseSpineObjectController baseSpineObjectController3 = GameGlobal.gGameMusicScene.objCtrls[timeNodeOrder2.idx];
							if ((bool)baseSpineObjectController3)
							{
								LongPressController x2 = baseSpineObjectController3 as LongPressController;
								if (x2 != null)
								{
								}
								for (int j = curPunchLpsIdx; j < timeNodeOrder2.idx; j++)
								{
									if (j >= GameGlobal.gGameMusicScene.objCtrls.Length)
									{
										continue;
									}
									BaseSpineObjectController baseSpineObjectController4 = GameGlobal.gGameMusicScene.objCtrls[j];
									if ((bool)baseSpineObjectController4)
									{
										LongPressController longPressController2 = baseSpineObjectController4 as LongPressController;
										if (longPressController2 != null && !longPressController2.isOnAir)
										{
											longPressController2.isActive = false;
										}
									}
								}
							}
						}
					}
				}
				Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false);
			}
			else
			{
				if (isJumpEnded)
				{
					Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, true);
				}
				if (isPunchEnded)
				{
					Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false);
				}
			}
		}

		public void MissHardTime()
		{
			GameGlobal.gGameTouchPlay.MissDoubleHardTime();
			if (isJumpBegan && !GameGlobal.gGameTouchPlay.isPunch)
			{
				m_JumpHardTime = m_PressHardTime;
			}
			if (isPunchBegan && GameGlobal.gGameTouchPlay.isPunch)
			{
				m_PunchHardTime = m_PressHardTime;
			}
		}

		public void DisMissHardTime()
		{
			if (!Singleton<BattleProperty>.instance.isAutoPlay)
			{
				m_JumpHardTime = -1m;
				m_PunchHardTime = -1m;
			}
		}

		public void MissDoubleHardTime()
		{
			m_PunchBeganTime = -1f;
			m_JumpBeganTime = -1f;
		}

		public void BeginTouchPhaser()
		{
			catchReactGroundTime = -1f;
			catchReactAirTime = -1f;
			if ((!isPunchBegan || !(m_PunchHardTime < 0m)) && (!isJumpBegan || !(m_JumpHardTime < 0m)))
			{
				return;
			}
			float tickTime = GameGlobal.gTouch.tickTime;
			if (isPunchBegan)
			{
				if (isPunch || Singleton<StageBattleComponent>.instance.IsAutoPlay())
				{
					isReactGround = false;
				}
				m_PunchBeganTime = tickTime;
				if (Mathf.Abs(m_PunchBeganTime - m_JumpBeganTime) < m_DoubleHardTime && m_JumpBeganTime > 0f)
				{
					m_PunchBeganTime = -1f;
					m_JumpBeganTime = -1f;
					isDoubleBegan = true;
				}
			}
			if (isJumpBegan)
			{
				if (!isPunch || Singleton<StageBattleComponent>.instance.IsAutoPlay())
				{
					isReactAir = false;
				}
				m_JumpBeganTime = tickTime;
				if (Mathf.Abs(m_PunchBeganTime - m_JumpBeganTime) < m_DoubleHardTime && m_PunchBeganTime > 0f)
				{
					m_PunchBeganTime = -1f;
					m_JumpBeganTime = -1f;
					isDoubleBegan = true;
				}
			}
			TouchActionResult(1u);
		}

		public void MoveTouchPhaser()
		{
			m_MovePassTick = Singleton<StageBattleComponent>.instance.timeFromMusicStart;
			if (isPunchStay || isPunchBegan)
			{
				m_ResultPunch = Singleton<BattleEnemyManager>.instance.GetPlayResult(Singleton<StageBattleComponent>.instance.curPunchLpsIdx);
				if (Singleton<StageBattleComponent>.instance.curPunchLpsIdx < 0 || m_ResultPunch < 2)
				{
					if (m_ParentMdPunch.configData.length > 0m && (bool)m_SacPunch)
					{
						m_PercentPunch = (m_MovePassTick - (float)m_ParentMdPunch.tick) / (float)m_ParentMdPunch.configData.length;
						if (m_PercentPunch >= 0f)
						{
							if (m_PercentPunch >= 1f)
							{
								m_SacPunch.Clip(m_PercentPunch, m_ResultPunch);
								m_SacPunch.gameObject.SetActive(false);
								Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false);
							}
							else if (!m_SacPunch.IsLongPressDestroy() && !m_SacPunch.IsLongPressAlpha())
							{
								Singleton<BattleEnemyManager>.instance.SetLongPressEffect(true);
								m_SacPunch.gameObject.SetActive(true);
								m_SacPunch.Clip(m_PercentPunch, m_ResultPunch);
							}
						}
						else if (isPunchBegan)
						{
							m_SacPunch.gameObject.SetActive(true);
							m_SacPunch.Clip(m_PercentPunch, m_ResultPunch);
						}
					}
				}
				else
				{
					m_ParentMdPunch = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(Singleton<StageBattleComponent>.instance.curPunchLpsIdx);
					if (m_ParentMdPunch.configData.length > 0m)
					{
						m_SacPunch = GameGlobal.gGameMusicScene.spineActionCtrls[Singleton<StageBattleComponent>.instance.curPunchLpsIdx];
						if ((bool)m_SacPunch)
						{
							m_PercentPunch = (m_MovePassTick - (float)m_ParentMdPunch.tick) / (float)m_ParentMdPunch.configData.length;
							if (m_PercentPunch >= 0f)
							{
								if (m_PercentPunch >= 1f)
								{
									m_SacPunch.Clip(m_PercentPunch, m_ResultPunch);
									m_SacPunch.gameObject.SetActive(false);
									Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false);
								}
								else if (!m_SacPunch.IsLongPressDestroy() && !m_SacPunch.IsLongPressAlpha())
								{
									Singleton<BattleEnemyManager>.instance.SetLongPressEffect(true);
									m_SacPunch.gameObject.SetActive(true);
									m_SacPunch.Clip(m_PercentPunch, m_ResultPunch);
								}
							}
							else if (isPunchBegan)
							{
								m_SacPunch.gameObject.SetActive(true);
								m_SacPunch.Clip(m_PercentPunch, m_ResultPunch);
							}
						}
					}
				}
			}
			if (!isJumpStay && !isJumpBegan)
			{
				return;
			}
			m_ResultJump = Singleton<BattleEnemyManager>.instance.GetPlayResult(Singleton<StageBattleComponent>.instance.curJumpLpsIdx);
			if (Singleton<StageBattleComponent>.instance.curJumpLpsIdx < 0 || m_ResultJump < 2)
			{
				if (!(m_ParentMdJump.configData.length > 0m) || !m_SacJump)
				{
					return;
				}
				m_PercentJump = (float)(((decimal)m_MovePassTick - m_ParentMdJump.tick) / m_ParentMdJump.configData.length);
				if (m_PercentJump >= 0f)
				{
					if (m_PercentJump >= 1f)
					{
						m_SacJump.Clip(m_PercentJump, m_ResultJump);
						m_SacJump.gameObject.SetActive(false);
						Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, true);
					}
					else if (!m_SacJump.IsLongPressDestroy() && !m_SacJump.IsLongPressAlpha())
					{
						Singleton<BattleEnemyManager>.instance.SetLongPressEffect(true, true);
						m_SacJump.gameObject.SetActive(true);
						m_SacJump.Clip(m_PercentJump, m_ResultJump);
					}
				}
				else if (isPunchBegan)
				{
					m_SacJump.gameObject.SetActive(true);
					m_SacJump.Clip(m_PercentJump, m_ResultJump);
				}
				return;
			}
			m_ParentMdJump = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(Singleton<StageBattleComponent>.instance.curJumpLpsIdx);
			if (!(m_ParentMdJump.configData.length > 0m))
			{
				return;
			}
			m_SacJump = GameGlobal.gGameMusicScene.spineActionCtrls[Singleton<StageBattleComponent>.instance.curJumpLpsIdx];
			if (!m_SacJump)
			{
				return;
			}
			m_PercentJump = (float)(((decimal)m_MovePassTick - m_ParentMdJump.tick) / m_ParentMdJump.configData.length);
			if (m_PercentJump >= 0f)
			{
				if (m_PercentJump >= 1f)
				{
					m_SacJump.Clip(m_PercentJump, m_ResultJump);
					m_SacJump.gameObject.SetActive(false);
					Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, true);
				}
				else if (!m_SacJump.IsLongPressDestroy() && !m_SacJump.IsLongPressAlpha())
				{
					Singleton<BattleEnemyManager>.instance.SetLongPressEffect(true, true);
					m_SacJump.gameObject.SetActive(true);
					m_SacJump.Clip(m_PercentJump, m_ResultJump);
				}
			}
			else if (isPunchBegan)
			{
				m_SacJump.gameObject.SetActive(true);
				m_SacJump.Clip(m_PercentJump, m_ResultJump);
			}
		}

		public void TouchActionResult(uint actionType, uint forcePressState = 0u)
		{
			List<TimeNodeOrder> timeNodeByTick = Singleton<StageBattleComponent>.instance.GetTimeNodeByTick(GameGlobal.gTouch.tickTime);
			if (timeNodeByTick == null || timeNodeByTick.Count <= 0 || !Singleton<StageBattleComponent>.instance.isInGame)
			{
				AttacksController.Instance.ShowAttack(-1, 0u, actionType);
				MissHardTime();
				return;
			}
			List<int> list = ListPool<int>.Get();
			List<uint> list2 = ListPool<uint>.Get();
			List<byte> list3 = ListPool<byte>.Get();
			List<TimeNodeOrder> list4 = ListPool<TimeNodeOrder>.Get();
			if (GameGlobal.gGameTouchPlay.isPunch)
			{
				hasReactGround = false;
			}
			else
			{
				hasReactAir = false;
			}
			int num = -1;
			for (int i = 0; i < timeNodeByTick.Count; i++)
			{
				TimeNodeOrder timeNodeOrder = timeNodeByTick[i];
				if (timeNodeOrder.isFucked || timeNodeOrder.md.noteData.type == 9 || timeNodeOrder.md.noteData.type == 10 || timeNodeOrder.md.noteData.type == 11)
				{
					continue;
				}
				if (timeNodeOrder.md.noteData.type == 6 || timeNodeOrder.md.noteData.type == 7 || timeNodeOrder.md.noteData.type == 2)
				{
					if (timeNodeOrder.md.noteData.type == 2 && timeNodeOrder.result == 4)
					{
						num = timeNodeOrder.idx;
					}
					continue;
				}
				int idx = timeNodeOrder.idx;
				if (!IsPlayEmpty(idx))
				{
					return;
				}
				byte result = timeNodeOrder.result;
				if (timeNodeOrder.isLongPressType)
				{
					if ((timeNodeByTick.Exists((TimeNodeOrder t) => t.md.noteData.type == 5 && t.md.noteData.addCombo) && !Singleton<BattleEnemyManager>.instance.isGroundPressing && !Singleton<BattleEnemyManager>.instance.isAirPressing) || ((timeNodeOrder.isAir ^ isPunchBegan) && Singleton<BattleEnemyManager>.instance.GetPlayResult(idx) == 0 && timeNodeOrder.isLongPressStart))
					{
						break;
					}
				}
				else if (timeNodeOrder.isMulType && list4.Count == 0)
				{
					result = Singleton<BattleEnemyManager>.instance.GetPlayResult(idx);
					if (timeNodeOrder.isMuling)
					{
						if (result <= 1)
						{
							AttacksController.Instance.ShowAttack(-1, 0u, actionType);
							MissHardTime();
							continue;
						}
					}
					else if (result == 0)
					{
						result = timeNodeOrder.result;
					}
					timeNodeOrder.isFucked = true;
					list4.Add(timeNodeOrder);
					list.Add(idx);
					list3.Add(result);
					list2.Add(actionType);
				}
				else if (Singleton<BattleEnemyManager>.instance.GetPlayResult(idx) <= 0)
				{
					bool flag = (timeNodeOrder.md.noteData.type == 5 || timeNodeOrder.md.noteData.type == 8) && timeNodeOrder.md.noteData.boss_action != "0" && !string.IsNullOrEmpty(timeNodeOrder.md.noteData.boss_action);
					if ((timeNodeOrder.isAir || flag) && isJumpBegan && !isReactAir)
					{
						isReactAir = true;
						hasReactAir = true;
						timeNodeOrder.isFucked = true;
						list4.Add(timeNodeOrder);
						list.Add(idx);
						list3.Add(result);
						list2.Add(actionType);
					}
					else if ((!timeNodeOrder.isAir || flag) && isPunchBegan && !isReactGround)
					{
						isReactGround = true;
						hasReactGround = true;
						timeNodeOrder.isFucked = true;
						list4.Add(timeNodeOrder);
						list.Add(idx);
						list3.Add(result);
						list2.Add(actionType);
					}
				}
			}
			float num2 = GameGlobal.gTouch.tickTime + 0.017f;
			List<TimeNodeOrder> timeNodeByTick2 = Singleton<StageBattleComponent>.instance.GetTimeNodeByTick(num2);
			for (int j = 0; j < list3.Count; j++)
			{
				TimeNodeOrder tno = list4[j];
				if (!(tno.isAir ^ SingletonMonoBehaviour<GirlManager>.instance.IsAir()))
				{
					continue;
				}
				int num3 = -1;
				if (tno.isRight || tno.result == 4 || tno.md.isMul)
				{
					for (int k = 0; k < 100; k++)
					{
						float tick = num2 + 0.001f * (float)k;
						timeNodeByTick2 = Singleton<StageBattleComponent>.instance.GetTimeNodeByTick(tick);
						if (timeNodeByTick2 == null)
						{
							continue;
						}
						for (int l = 0; l < timeNodeByTick2.Count; l++)
						{
							TimeNodeOrder timeNodeOrder2 = timeNodeByTick2[l];
							if (timeNodeOrder2.isAir == tno.isAir && timeNodeOrder2.md.noteData.type == 2 && timeNodeOrder2.result == 3)
							{
								Singleton<BattleEnemyManager>.instance.SetPlayResult(timeNodeOrder2.idx, 4);
							}
						}
					}
				}
				else if (timeNodeByTick2 != null && timeNodeByTick2.Count > 0)
				{
					for (int m = 0; m < timeNodeByTick2.Count; m++)
					{
						TimeNodeOrder timeNodeOrder3 = timeNodeByTick2[m];
						if (timeNodeOrder3.isAir == tno.isAir && (timeNodeOrder3.md.noteData.type == 6 || timeNodeOrder3.md.noteData.type == 7))
						{
							num3 = timeNodeOrder3.idx;
							break;
						}
					}
				}
				for (int n = 1; n <= 17; n++)
				{
					float num4 = GameGlobal.gTouch.tickTime + (float)n * 0.001f;
					List<TimeNodeOrder> timeNodeByTick3 = Singleton<StageBattleComponent>.instance.GetTimeNodeByTick(num4);
					if (timeNodeByTick3 != null)
					{
						TimeNodeOrder timeNodeOrder4 = timeNodeByTick3.Find((TimeNodeOrder t) => t.md.noteData.type == 2 && (tno.isRight || tno.isPerfectNode) && t.result == 3 && t.isAir == SingletonMonoBehaviour<GirlManager>.instance.IsAir());
						if (timeNodeOrder4 != null && tno.md.doubleIdx <= 0 && !MultHitEnemyController.isHitting)
						{
							GameGlobal.gGameMissPlay.MissCube(timeNodeOrder4.idx, (decimal)num4);
							break;
						}
					}
				}
				if ((num3 > -1 && num3 < j) || (num > -1 && num < j))
				{
					list4.RemoveAt(j);
					list.RemoveAt(j);
					list3.RemoveAt(j);
					list2.RemoveAt(j);
				}
			}
			if (list.Count == 0)
			{
				AttacksController.Instance.ShowAttack(-1, 0u, actionType);
				MissHardTime();
				if (num > -1)
				{
					SingletonMonoBehaviour<CoroutineManager>.instance.Delay(DisMissHardTime, 1);
				}
			}
			else
			{
				for (int num5 = 0; num5 < list.Count; num5++)
				{
					byte resultCode = (byte)((!Singleton<StageBattleComponent>.instance.IsAutoPlay()) ? list3[num5] : 4);
					TouchResult(list[num5], resultCode, list2[num5], list4[num5]);
				}
			}
			ListPool<int>.Release(list);
			ListPool<uint>.Release(list2);
			ListPool<byte>.Release(list3);
			ListPool<TimeNodeOrder>.Release(list4);
		}

		public void TouchResult(int idx, byte resultCode, uint actionType, TimeNodeOrder tno = null, bool isSkill = false)
		{
			MusicData md = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
			if (Singleton<BattleProperty>.instance.isCatchAvailable && (md.noteData.type == 3 || md.noteData.type == 1))
			{
				if (md.isAir)
				{
					catchReactAirTime = (float)md.tick;
				}
				else
				{
					catchReactGroundTime = (float)md.tick;
				}
			}
			if (md.noteData.type == 5 || md.noteData.type == 1 || md.noteData.type == 4)
			{
				if (resultCode == 3 && Singleton<BattleProperty>.instance.greatToPerfect > 0 && !isSkill)
				{
					Singleton<BattleProperty>.instance.greatToPerfect--;
					resultCode = 4;
					Singleton<EventManager>.instance.Invoke("Battle/OnGreat2Perfect");
					GameObject gameObject = AttackEffectManager.instance.rampageSkillEffect.CreateInstance();
					gameObject.transform.position = ((md.doubleIdx != -1) ? new Vector3(-5f, -0.75f, 0f) : new Vector3(-5f, (!md.isAir) ? (-0.75f) : 1.25f, 0f));
				}
				if (resultCode == 4 && Singleton<BattleProperty>.instance.perfectHpRevive > 0)
				{
					BattleRoleAttributeComponent.instance.AddHp(Singleton<BattleProperty>.instance.perfectHpRevive);
				}
			}
			if (!md.isAir && MultHitEnemyController.isMulHitEnding)
			{
				SingletonMonoBehaviour<GirlManager>.instance.SetJumpingAction(true);
				MultHitEnemyController.isMulHitEnding = false;
			}
			if (md.isMul)
			{
				MultHitEnemyController multHitEnemyController = GameGlobal.gGameMusicScene.objCtrls[idx] as MultHitEnemyController;
				if (multHitEnemyController != null && multHitEnemyController.isDead)
				{
					AttacksController.Instance.ShowAttack(-2, 0u, actionType);
					return;
				}
			}
			else
			{
				byte playResult = Singleton<BattleEnemyManager>.instance.GetPlayResult(idx);
				if (playResult > 0)
				{
					return;
				}
			}
			if (md.isLongPressEnd)
			{
				MusicData musicData = Singleton<StageBattleComponent>.instance.GetMusicData().Find((MusicData m) => m.isAir == md.isAir && m.configData.length > 0m && m.configData.time == md.longPressPTick);
				short objId = musicData.objId;
				if (objId < GameGlobal.gGameMusicScene.spineActionCtrls.Length && objId >= 0)
				{
					SpineActionController spineActionController = GameGlobal.gGameMusicScene.spineActionCtrls[objId];
					if ((bool)spineActionController)
					{
						spineActionController.DestroyLongPress();
					}
				}
			}
			m_PunchHardTime = -1m;
			m_JumpHardTime = -1m;
			bool flag = tno != null && !tno.isRight;
			bool flag2 = md.doubleIdx > 0 && md.doubleIdx != 9999;
			byte b = 0;
			if (flag2)
			{
				b = Singleton<BattleEnemyManager>.instance.GetPlayResult(md.doubleIdx);
				if (b > 1)
				{
					resultCode = Math.Min(b, resultCode);
					if (b < resultCode)
					{
						flag = Singleton<BattleEnemyManager>.instance.IsPlayLeft(md.doubleIdx);
					}
				}
			}
			if (resultCode > 1)
			{
				byte b2 = resultCode;
				if (b2 > 4)
				{
					b2 = 4;
				}
				BattleEnemyManager instance = Singleton<BattleEnemyManager>.instance;
				byte result = b2;
				bool isMul = md.isMul;
				bool isLeft = flag;
				instance.SetPlayResult(idx, result, isMul, false, isLeft);
			}
			if (resultCode == 0)
			{
				AttacksController.Instance.ShowAttack(-2, 0u, actionType);
				return;
			}
			ShowPlayResult(idx, actionType, resultCode);
			if (md.noteData.addCombo && !md.isLongPressing && !md.isMul)
			{
				if (flag2)
				{
					if (b > 1)
					{
						PlayComboPhaser(md, resultCode, true);
					}
				}
				else
				{
					PlayComboPhaser(md, resultCode);
				}
			}
			if (flag2)
			{
				if (b > 1)
				{
					BattleRoleAttributeComponent.instance.AttackScore(idx, resultCode, tno);
				}
			}
			else
			{
				BattleRoleAttributeComponent.instance.AttackScore(idx, resultCode, tno);
			}
		}

		public bool IsPlayEmpty(int idx)
		{
			if (idx < 0)
			{
				return false;
			}
			List<MusicData> musicData = Singleton<StageBattleComponent>.instance.GetMusicData();
			if (idx >= musicData.Count)
			{
				return false;
			}
			return true;
		}

		private void ShowPlayResult(int idx, uint actionType, uint resultCode)
		{
			if (FeverManager.Instance.IsOnFeverState())
			{
				resultCode = 6u;
			}
			AttacksController.Instance.ShowAttack(idx, resultCode, actionType);
			AttacksController.Instance.OnShowAttack(idx, resultCode);
		}

		private void PlayComboPhaser(MusicData md, uint resultCode, bool isDouble = false)
		{
			if (resultCode == 5)
			{
				return;
			}
			int num = (!isDouble) ? 1 : 2;
			int combo = Singleton<StageBattleComponent>.instance.GetCombo() + num;
			Singleton<StageBattleComponent>.instance.SetCombo(combo);
			if (md.noteData.type == 1 || md.noteData.type == 4)
			{
				Singleton<TaskStageTarget>.instance.AddHitEnemy(num);
				if (Singleton<BattleEnemyManager>.instance.isGroundPressing || Singleton<BattleEnemyManager>.instance.isAirPressing)
				{
					Singleton<TaskStageTarget>.instance.AddLongPressHit(num);
				}
			}
		}
	}
}
