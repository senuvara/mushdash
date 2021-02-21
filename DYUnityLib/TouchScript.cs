using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.Graphics;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using FormulaBase;
using GameLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DYUnityLib
{
	public class TouchScript
	{
		public float tickTime;

		private readonly float[][] m_Touchs;

		private const int m_Began1 = 0;

		private const int m_Moved1 = 1;

		private const int m_Ended1 = 2;

		private const int m_Began2 = 3;

		private const int m_Moved2 = 4;

		private const int m_Ended2 = 5;

		private int m_FrameCount;

		private const int m_Length = 5;

		private readonly ArrayList m_CustomEvent = new ArrayList();

		private string m_ButtonName;

		private float m_TouchEventPhaseP;

		private int m_TouchEventPhaseBtnDownIndex;

		private int m_TouchEventPhaseBtnIndex;

		private int m_TouchEventPhaseBtnUpIndex;

		private int m_JumpIndex;

		private int m_JumpBound;

		private int m_PunchIndex;

		private int m_PunchBound;

		private float[] m_JumpTouchs;

		private float[] m_PunchTouchs;

		private List<int> m_IndexChangeIndexs;

		private List<CatchInput> m_CatchInputs;

		private List<int> m_DownList;

		private List<int> m_LongDownList;

		private List<int> m_MoveList;

		private List<int> m_UpList;

		private readonly string[] m_ButtonNames = new string[2]
		{
			"BattleAir",
			"BattleGround"
		};

		private Dictionary<string, bool[]> m_ButtonStates;

		private int m_ListCount;

		private bool m_IsInvoke;

		public TouchScript()
		{
			m_FrameCount = -1;
			tickTime = -1f;
			m_Touchs = new float[6][];
			for (int i = 0; i < 6; i++)
			{
				m_Touchs[i] = new float[5];
			}
		}

		private void Reset()
		{
			if (GameTouchPlay.isLeftRight || GameTouchPlay.isBtnLeftRight)
			{
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < m_Touchs[i].Length; j++)
					{
						m_Touchs[i][j] = 0f;
					}
				}
			}
			if (GameTouchPlay.isLeftRight && GameTouchPlay.isBtnLeftRight)
			{
				return;
			}
			for (int k = 3; k < 6; k++)
			{
				for (int l = 0; l < m_Touchs[k].Length; l++)
				{
					m_Touchs[k][l] = 0f;
				}
			}
		}

		public float[][] GetTouchs()
		{
			return m_Touchs;
		}

		private void ResetEndTouch()
		{
			GameGlobal.gGameTouchPlay.isPunchEnded = false;
			GameGlobal.gGameTouchPlay.isJumpEnded = false;
			if (GameTouchPlay.isLeftRight || GameTouchPlay.isBtnLeftRight)
			{
				for (int i = 0; i < m_Touchs[2].Length; i++)
				{
					m_Touchs[2][i] = 0f;
				}
			}
			if (!GameTouchPlay.isLeftRight || !GameTouchPlay.isBtnLeftRight)
			{
				for (int j = 0; j < m_Touchs[5].Length; j++)
				{
					m_Touchs[5][j] = 0f;
				}
			}
		}

		private void ResetBeganTouch()
		{
			GameGlobal.gGameTouchPlay.isPunchBegan = false;
			GameGlobal.gGameTouchPlay.isJumpBegan = false;
			if (GameTouchPlay.isLeftRight || GameTouchPlay.isBtnLeftRight)
			{
				for (int i = 0; i < m_Touchs[0].Length; i++)
				{
					m_Touchs[0][i] = 0f;
				}
			}
			if (!GameTouchPlay.isLeftRight || !GameTouchPlay.isBtnLeftRight)
			{
				for (int j = 0; j < m_Touchs[3].Length; j++)
				{
					m_Touchs[3][j] = 0f;
				}
			}
		}

		public bool IsJumpTouch(TouchPhase phase = TouchPhase.Began)
		{
			if (GameTouchPlay.isLeftRight)
			{
				int jumpIndex;
				switch (phase)
				{
				case TouchPhase.Began:
					jumpIndex = 0;
					break;
				case TouchPhase.Moved:
				case TouchPhase.Stationary:
					jumpIndex = 1;
					break;
				default:
					jumpIndex = 2;
					break;
				}
				m_JumpIndex = jumpIndex;
				m_JumpBound = GameGlobal.gGameTouchPlay.widthMid;
			}
			else
			{
				int jumpIndex2;
				switch (phase)
				{
				case TouchPhase.Began:
					jumpIndex2 = 3;
					break;
				case TouchPhase.Moved:
				case TouchPhase.Stationary:
					jumpIndex2 = 4;
					break;
				default:
					jumpIndex2 = 5;
					break;
				}
				m_JumpIndex = jumpIndex2;
				m_JumpBound = GameGlobal.gGameTouchPlay.heightMid;
			}
			m_JumpTouchs = m_Touchs[m_JumpIndex];
			for (int i = 0; i < m_JumpTouchs.Length; i++)
			{
				if (GameTouchPlay.isLeftRight)
				{
					if (m_JumpTouchs[i] <= (float)m_JumpBound && m_JumpTouchs[i] > 0f)
					{
						return true;
					}
				}
				else if (m_JumpTouchs[i] > (float)m_JumpBound)
				{
					return true;
				}
			}
			if (GameTouchPlay.isLeftRight != GameTouchPlay.isBtnLeftRight)
			{
				if (GameTouchPlay.isBtnLeftRight)
				{
					int jumpIndex3;
					switch (phase)
					{
					case TouchPhase.Began:
						jumpIndex3 = 0;
						break;
					case TouchPhase.Moved:
					case TouchPhase.Stationary:
						jumpIndex3 = 1;
						break;
					default:
						jumpIndex3 = 2;
						break;
					}
					m_JumpIndex = jumpIndex3;
					m_JumpBound = GameGlobal.gGameTouchPlay.widthMid;
				}
				else
				{
					int jumpIndex4;
					switch (phase)
					{
					case TouchPhase.Began:
						jumpIndex4 = 3;
						break;
					case TouchPhase.Moved:
					case TouchPhase.Stationary:
						jumpIndex4 = 4;
						break;
					default:
						jumpIndex4 = 5;
						break;
					}
					m_JumpIndex = jumpIndex4;
					m_JumpBound = GameGlobal.gGameTouchPlay.heightMid;
				}
				m_JumpTouchs = m_Touchs[m_JumpIndex];
				for (int j = 0; j < m_JumpTouchs.Length; j++)
				{
					if (GameTouchPlay.isBtnLeftRight)
					{
						if (m_JumpTouchs[j] <= (float)m_JumpBound && m_JumpTouchs[j] > 0f)
						{
							return true;
						}
					}
					else if (m_JumpTouchs[j] > (float)m_JumpBound)
					{
						return true;
					}
				}
			}
			return false;
		}

		public bool IsPunchTouch(TouchPhase phase = TouchPhase.Began)
		{
			if (GameTouchPlay.isLeftRight)
			{
				int punchIndex;
				switch (phase)
				{
				case TouchPhase.Began:
					punchIndex = 0;
					break;
				case TouchPhase.Moved:
				case TouchPhase.Stationary:
					punchIndex = 1;
					break;
				default:
					punchIndex = 2;
					break;
				}
				m_PunchIndex = punchIndex;
				m_PunchBound = GameGlobal.gGameTouchPlay.widthMid;
			}
			else
			{
				int punchIndex2;
				switch (phase)
				{
				case TouchPhase.Began:
					punchIndex2 = 3;
					break;
				case TouchPhase.Moved:
				case TouchPhase.Stationary:
					punchIndex2 = 4;
					break;
				default:
					punchIndex2 = 5;
					break;
				}
				m_PunchIndex = punchIndex2;
				m_PunchBound = GameGlobal.gGameTouchPlay.heightMid;
			}
			m_PunchTouchs = m_Touchs[m_PunchIndex];
			for (int i = 0; i < m_PunchTouchs.Length; i++)
			{
				if (GameTouchPlay.isLeftRight)
				{
					if (m_PunchTouchs[i] > (float)m_PunchBound)
					{
						return true;
					}
				}
				else if (m_PunchTouchs[i] <= (float)m_PunchBound && m_PunchTouchs[i] > 0f)
				{
					return true;
				}
			}
			if (GameTouchPlay.isLeftRight != GameTouchPlay.isBtnLeftRight)
			{
				if (GameTouchPlay.isBtnLeftRight)
				{
					int punchIndex3;
					switch (phase)
					{
					case TouchPhase.Began:
						punchIndex3 = 0;
						break;
					case TouchPhase.Moved:
					case TouchPhase.Stationary:
						punchIndex3 = 1;
						break;
					default:
						punchIndex3 = 2;
						break;
					}
					m_PunchIndex = punchIndex3;
					m_PunchBound = GameGlobal.gGameTouchPlay.widthMid;
				}
				else
				{
					int punchIndex4;
					switch (phase)
					{
					case TouchPhase.Began:
						punchIndex4 = 3;
						break;
					case TouchPhase.Moved:
					case TouchPhase.Stationary:
						punchIndex4 = 4;
						break;
					default:
						punchIndex4 = 5;
						break;
					}
					m_PunchIndex = punchIndex4;
					m_PunchBound = GameGlobal.gGameTouchPlay.heightMid;
				}
				m_PunchTouchs = m_Touchs[m_PunchIndex];
				for (int j = 0; j < m_PunchTouchs.Length; j++)
				{
					if (GameTouchPlay.isBtnLeftRight)
					{
						if (m_PunchTouchs[j] > (float)m_PunchBound)
						{
							return true;
						}
					}
					else if (m_PunchTouchs[j] <= (float)m_PunchBound && m_PunchTouchs[j] > 0f)
					{
						return true;
					}
				}
			}
			return false;
		}

		private void IndexChange(int i, TouchPhase phase, bool isPunch)
		{
			m_IndexChangeIndexs = ((!isPunch) ? GameGlobal.gGameTouchPlay.beganAirIndexs : GameGlobal.gGameTouchPlay.beganGroundIndexs);
			switch (phase)
			{
			case TouchPhase.Began:
				if (!m_IndexChangeIndexs.Contains(i))
				{
					m_IndexChangeIndexs.Add(i);
				}
				break;
			case TouchPhase.Ended:
				if (m_IndexChangeIndexs.Contains(i))
				{
					m_IndexChangeIndexs.Remove(i);
				}
				break;
			}
		}

		private bool IsPunch(float p, bool isBtn)
		{
			int num;
			if (isBtn)
			{
				num = ((!GameTouchPlay.isBtnLeftRight) ? GameGlobal.gGameTouchPlay.heightMid : GameGlobal.gGameTouchPlay.widthMid);
				if (GameTouchPlay.isBtnLeftRight)
				{
					if (p > (float)num)
					{
						return true;
					}
				}
				else if (p <= (float)num && p > 0f)
				{
					return true;
				}
				return false;
			}
			num = ((!GameTouchPlay.isLeftRight) ? GameGlobal.gGameTouchPlay.heightMid : GameGlobal.gGameTouchPlay.widthMid);
			if (GameTouchPlay.isLeftRight)
			{
				if (p > (float)num)
				{
					return true;
				}
			}
			else if (p <= (float)num && p > 0f)
			{
				return true;
			}
			return false;
		}

		public void AddCustomEvent(EventTrigger e)
		{
			if (e != null && !m_CustomEvent.Contains(e))
			{
				m_CustomEvent.Add(e);
			}
		}

		public void RemoveCustomEvent(EventTrigger e)
		{
			if (e != null && m_CustomEvent.Contains(e))
			{
				m_CustomEvent.Remove(e);
			}
		}

		public void ClearCustomEvent()
		{
			if (m_CustomEvent != null)
			{
				m_CustomEvent.Clear();
			}
		}

		public void TouchTrigger(object sender, uint triggerId, params object[] args)
		{
			if (m_CustomEvent == null || m_CustomEvent.Count <= 0)
			{
				return;
			}
			IEnumerator enumerator = m_CustomEvent.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					EventTrigger eventTrigger = (EventTrigger)enumerator.Current;
					if (eventTrigger != null)
					{
						GTrigger.FireEvent(eventTrigger.id, new object[1]
						{
							triggerId
						});
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		public void OnStart()
		{
			EventTrigger eventTrigger = GTrigger.RegEvent(6u);
			EventTrigger eventTrigger2 = GTrigger.RegEvent(8u);
			EventTrigger eventTrigger3 = GTrigger.RegEvent(7u);
			eventTrigger.Trigger += TouchTrigger;
			eventTrigger2.Trigger += TouchTrigger;
			eventTrigger3.Trigger += TouchTrigger;
			m_ButtonStates = new Dictionary<string, bool[]>();
			for (int i = 0; i < m_ButtonNames.Length; i++)
			{
				m_ButtonStates.Add(m_ButtonNames[i], new bool[3]
				{
					false,
					false,
					false
				});
			}
			m_ButtonStates.Add("Fever", new bool[3]
			{
				false,
				false,
				false
			});
			m_CatchInputs = new List<CatchInput>();
			for (int j = 0; j < 32; j++)
			{
				m_CatchInputs.Add(new CatchInput
				{
					isGroundCatching = false,
					isAirCatching = false
				});
			}
			m_DownList = new List<int>(32);
			m_LongDownList = new List<int>(32);
			m_MoveList = new List<int>(32);
			m_UpList = new List<int>(32);
		}

		public void TouchEventPhaseUpdate()
		{
			if (!Singleton<StageBattleComponent>.instance.isInGame)
			{
				return;
			}
			for (int i = 0; i < m_ButtonNames.Length; i++)
			{
				string text = m_ButtonNames[i];
				List<int> buttonDown = Singleton<InputManager>.instance.GetButtonDown(text);
				List<int> button = Singleton<InputManager>.instance.GetButton(text);
				List<int> buttonUp = Singleton<InputManager>.instance.GetButtonUp(text);
				m_ButtonStates[text][0] = (m_ButtonStates[text][0] || buttonDown.Count > 0);
				m_ButtonStates[text][1] = (m_ButtonStates[text][1] || button.Count > 0);
				m_ButtonStates[text][2] = (m_ButtonStates[text][2] || buttonUp.Count > 0);
				if (buttonDown.Count > 0)
				{
					for (int j = 0; j < buttonDown.Count; j++)
					{
						int item = buttonDown[j];
						m_DownList.Add(item);
						m_LongDownList.Add(item);
					}
				}
				if (button.Count > 0)
				{
					for (int k = 0; k < button.Count; k++)
					{
						m_MoveList.Add(button[k]);
					}
				}
				if (buttonUp.Count > 0)
				{
					for (int l = 0; l < buttonUp.Count; l++)
					{
						m_UpList.Add(buttonUp[l]);
					}
				}
			}
			m_ButtonStates["Fever"][0] = (m_ButtonStates["Fever"][0] || Singleton<InputManager>.instance.GetButtonDown("Fever").Count > 0);
			m_ButtonStates["Fever"][1] = (m_ButtonStates["Fever"][1] || Singleton<InputManager>.instance.GetButton("Fever").Count > 0);
			m_ButtonStates["Fever"][2] = (m_ButtonStates["Fever"][2] || Singleton<InputManager>.instance.GetButtonUp("Fever").Count > 0);
		}

		public bool GetButtonDown(string name)
		{
			m_ListCount = 0;
			if (!GraphicSettings.isOverOneHundred)
			{
				if (name == "Fever")
				{
					return Singleton<InputManager>.instance.GetButtonDown("Fever").Count > 0;
				}
				for (int i = 0; i < m_DownList.Count; i++)
				{
					int num = m_DownList[i];
					if (name == "BattleGround")
					{
						if (CheckClickCount(m_DownList, true) && num >= 16)
						{
							CheckButtonDown("BattleAir", 0);
							break;
						}
						if (num < 16)
						{
							return true;
						}
					}
					else
					{
						if (CheckClickCount(m_DownList, false) && num < 16)
						{
							CheckButtonDown("BattleGround", 1);
							break;
						}
						if (num >= 16)
						{
							return true;
						}
					}
				}
				return false;
			}
			if (m_ButtonStates[name][0])
			{
				m_ButtonStates[name][0] = false;
				if (m_DownList.Count > 1)
				{
					if (name == "BattleAir")
					{
						if (CheckClickCount(m_DownList, true))
						{
							m_ButtonStates[name][0] = true;
						}
					}
					else if (name == "BattleGround" && CheckClickCount(m_DownList, false))
					{
						m_ButtonStates[name][0] = true;
					}
				}
				return true;
			}
			return false;
		}

		private bool CheckClickCount(List<int> list, bool isAir)
		{
			m_ListCount = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (isAir)
				{
					if (list[i] >= 16)
					{
						m_ListCount++;
					}
				}
				else if (list[i] < 16)
				{
					m_ListCount++;
				}
			}
			if (m_ListCount > 1)
			{
				return true;
			}
			return false;
		}

		private void CheckButtonDown(string name, int i)
		{
			if (GameTouchPlay.isBtnLeftRight)
			{
				m_TouchEventPhaseP = ((i % 2 != 0) ? ((float)GraphicSettings.curScreenWidth) : 1f);
			}
			else
			{
				m_TouchEventPhaseP = ((i % 2 != 0) ? ((float)GraphicSettings.curScreenHeight) : 1f);
			}
			if (i == 0)
			{
				ResetBeganTouch();
			}
			m_TouchEventPhaseBtnDownIndex = ((!GameTouchPlay.isBtnLeftRight) ? 3 : 0);
			m_Touchs[m_TouchEventPhaseBtnDownIndex][i] = m_TouchEventPhaseP;
			tickTime = Singleton<StageBattleComponent>.instance.timeFromMusicStart;
			if (!m_IsInvoke)
			{
				return;
			}
			GameGlobal.gGameTouchPlay.isPunch = IsPunch(m_TouchEventPhaseP, true);
			IndexChange(i, TouchPhase.Began, GameGlobal.gGameTouchPlay.isPunch);
			GTrigger.FireEvent(6u);
			for (int j = 0; j < m_DownList.Count; j++)
			{
				int index = m_DownList[j];
				CatchInput catchInput = m_CatchInputs[index];
				if (GameGlobal.gGameTouchPlay.catchReactGroundTime > 0f)
				{
					catchInput.isGroundCatching = true;
					catchInput.isAirCatching = false;
					RefreshCatchTime(index, GameGlobal.gGameTouchPlay.catchReactGroundTime);
				}
				if (GameGlobal.gGameTouchPlay.catchReactAirTime > 0f)
				{
					catchInput.isGroundCatching = false;
					catchInput.isAirCatching = true;
					RefreshCatchTime(index, GameGlobal.gGameTouchPlay.catchReactAirTime);
				}
			}
		}

		private bool GetButton(string name)
		{
			if (!GraphicSettings.isOverOneHundred)
			{
				if (name == "Fever")
				{
					return Singleton<InputManager>.instance.GetButton("Fever").Count > 0;
				}
				for (int i = 0; i < m_MoveList.Count; i++)
				{
					int num = m_MoveList[i];
					if (name == "BattleGround")
					{
						if (num < 16)
						{
							return true;
						}
					}
					else if (num >= 16)
					{
						return true;
					}
				}
				return false;
			}
			if (m_ButtonStates[name][1])
			{
				m_ButtonStates[name][1] = false;
				if (m_MoveList.Count > 1)
				{
					if (name == "BattleAir")
					{
						if (CheckClickCount(m_MoveList, true))
						{
							m_ButtonStates[name][1] = true;
						}
					}
					else if (name == "BattleGround" && CheckClickCount(m_MoveList, false))
					{
						m_ButtonStates[name][1] = true;
					}
				}
				return true;
			}
			return false;
		}

		private void CheckButton(int i)
		{
			if (GameTouchPlay.isBtnLeftRight)
			{
				m_TouchEventPhaseP = ((i % 2 != 0) ? ((float)GraphicSettings.curScreenWidth) : 1f);
			}
			else
			{
				m_TouchEventPhaseP = ((i % 2 != 0) ? ((float)GraphicSettings.curScreenHeight) : 1f);
			}
			m_TouchEventPhaseBtnIndex = (GameTouchPlay.isBtnLeftRight ? 1 : 4);
			m_Touchs[m_TouchEventPhaseBtnIndex][i] = m_TouchEventPhaseP;
			tickTime = Singleton<StageBattleComponent>.instance.timeFromMusicStart;
			if (m_IsInvoke)
			{
				GTrigger.FireEvent(7u);
			}
		}

		private bool GetButtonUp(string name)
		{
			if (!GraphicSettings.isOverOneHundred)
			{
				if (name == "Fever")
				{
					return Singleton<InputManager>.instance.GetButtonUp("Fever").Count > 0;
				}
				for (int i = 0; i < m_UpList.Count; i++)
				{
					int num = m_UpList[i];
					if (name == "BattleGround")
					{
						if (CheckClickCount(m_UpList, true) && num >= 16)
						{
							CheckButtonUp(0);
							break;
						}
						if (num < 16)
						{
							return true;
						}
					}
					else
					{
						if (CheckClickCount(m_UpList, false) && num < 16)
						{
							CheckButtonUp(1);
							break;
						}
						if (num >= 16)
						{
							return true;
						}
					}
				}
				return false;
			}
			if (m_ButtonStates[name][2])
			{
				m_ButtonStates[name][2] = false;
				if (m_UpList.Count > 1)
				{
					if (name == "BattleAir")
					{
						if (CheckClickCount(m_UpList, true))
						{
							m_ButtonStates[name][2] = true;
						}
					}
					else if (name == "BattleGround" && CheckClickCount(m_UpList, false))
					{
						m_ButtonStates[name][2] = true;
					}
				}
				return true;
			}
			return false;
		}

		private void CheckButtonUp(int i)
		{
			if (GameTouchPlay.isBtnLeftRight)
			{
				m_TouchEventPhaseP = ((i % 2 != 0) ? ((float)GraphicSettings.curScreenWidth) : 1f);
			}
			else
			{
				m_TouchEventPhaseP = ((i % 2 != 0) ? ((float)GraphicSettings.curScreenHeight) : 1f);
			}
			ResetEndTouch();
			m_TouchEventPhaseBtnUpIndex = ((!GameTouchPlay.isBtnLeftRight) ? 5 : 2);
			m_Touchs[m_TouchEventPhaseBtnUpIndex][i] = m_TouchEventPhaseP;
			tickTime = Singleton<StageBattleComponent>.instance.timeFromMusicStart;
			if (m_IsInvoke)
			{
				IndexChange(i, TouchPhase.Began, IsPunch(m_TouchEventPhaseP, true));
				GTrigger.FireEvent(8u);
				for (int j = 0; j < m_UpList.Count; j++)
				{
					int index = m_UpList[j];
					CatchInput catchInput = m_CatchInputs[index];
					catchInput.isGroundCatching = false;
					catchInput.isAirCatching = false;
				}
			}
		}

		public void TouchEventPhase()
		{
			if (!Singleton<StageBattleComponent>.instance.isInGame)
			{
				return;
			}
			m_IsInvoke = false;
			if (GraphicSettings.isOverOneHundred && GraphicSettings.isFrameOverOneHundred)
			{
				m_IsInvoke = true;
				if (GameGlobal.gGameMusic.invoke)
				{
					GameGlobal.gGameMusic.invoke = false;
					Reset();
					GameGlobal.gGameTouchPlay.Reset();
				}
			}
			else
			{
				m_IsInvoke = (m_FrameCount != Time.frameCount && Singleton<StageBattleComponent>.instance.isInGame);
				m_FrameCount = Time.frameCount;
				if (m_IsInvoke)
				{
					Reset();
					GameGlobal.gGameTouchPlay.Reset();
				}
			}
			if (GetButtonDown("Fever"))
			{
				FeverManager.Instance.InvokeFever();
			}
			for (int i = 0; i < m_ButtonNames.Length; i++)
			{
				m_ButtonName = m_ButtonNames[i];
				if (GameTouchPlay.isBtnLeftRight)
				{
					m_TouchEventPhaseP = ((i % 2 != 0) ? ((float)GraphicSettings.curScreenWidth) : 1f);
				}
				else
				{
					m_TouchEventPhaseP = ((i % 2 != 0) ? ((float)GraphicSettings.curScreenHeight) : 1f);
				}
				if (GetButtonDown(m_ButtonName))
				{
					if (i == 0)
					{
						ResetBeganTouch();
					}
					m_TouchEventPhaseBtnDownIndex = ((!GameTouchPlay.isBtnLeftRight) ? 3 : 0);
					m_Touchs[m_TouchEventPhaseBtnDownIndex][i] = m_TouchEventPhaseP;
					tickTime = Singleton<StageBattleComponent>.instance.timeFromMusicStart;
					if (m_IsInvoke)
					{
						GameGlobal.gGameTouchPlay.isPunch = IsPunch(m_TouchEventPhaseP, true);
						IndexChange(i, TouchPhase.Began, GameGlobal.gGameTouchPlay.isPunch);
						GTrigger.FireEvent(6u);
						for (int j = 0; j < m_DownList.Count; j++)
						{
							int index = m_DownList[j];
							CatchInput catchInput = m_CatchInputs[index];
							if (GameGlobal.gGameTouchPlay.catchReactGroundTime > 0f)
							{
								catchInput.isGroundCatching = true;
								catchInput.isAirCatching = false;
								RefreshCatchTime(index, GameGlobal.gGameTouchPlay.catchReactGroundTime);
							}
							if (GameGlobal.gGameTouchPlay.catchReactAirTime > 0f)
							{
								catchInput.isGroundCatching = false;
								catchInput.isAirCatching = true;
								RefreshCatchTime(index, GameGlobal.gGameTouchPlay.catchReactAirTime);
							}
						}
					}
				}
				if (GetButton(m_ButtonName))
				{
					m_TouchEventPhaseBtnIndex = (GameTouchPlay.isBtnLeftRight ? 1 : 4);
					m_Touchs[m_TouchEventPhaseBtnIndex][i] = m_TouchEventPhaseP;
					tickTime = Singleton<StageBattleComponent>.instance.timeFromMusicStart;
					if (m_IsInvoke)
					{
						GTrigger.FireEvent(7u);
					}
				}
				if (!GetButtonUp(m_ButtonName))
				{
					continue;
				}
				ResetEndTouch();
				m_TouchEventPhaseBtnUpIndex = ((!GameTouchPlay.isBtnLeftRight) ? 5 : 2);
				m_Touchs[m_TouchEventPhaseBtnUpIndex][i] = m_TouchEventPhaseP;
				tickTime = Singleton<StageBattleComponent>.instance.timeFromMusicStart;
				if (m_IsInvoke)
				{
					IndexChange(i, TouchPhase.Began, IsPunch(m_TouchEventPhaseP, true));
					GTrigger.FireEvent(8u);
					for (int k = 0; k < m_UpList.Count; k++)
					{
						int index2 = m_UpList[k];
						CatchInput catchInput2 = m_CatchInputs[index2];
						catchInput2.isGroundCatching = false;
						catchInput2.isAirCatching = false;
					}
				}
			}
			m_DownList.Clear();
			m_MoveList.Clear();
			m_UpList.Clear();
		}

		public void RefreshCatch()
		{
			for (int i = 0; i < m_ButtonNames.Length; i++)
			{
				if (m_LongDownList.Count <= 0)
				{
					continue;
				}
				for (int j = 0; j < m_LongDownList.Count; j++)
				{
					int num = m_LongDownList[j];
					if (num != -1)
					{
						CatchInput catchInput = m_CatchInputs[num];
						if (GameGlobal.gGameTouchPlay.catchReactGroundTime > 0f)
						{
							catchInput.isGroundCatching = true;
							catchInput.isAirCatching = false;
							RefreshCatchTime(num, GameGlobal.gGameTouchPlay.catchReactGroundTime);
						}
						if (GameGlobal.gGameTouchPlay.catchReactAirTime > 0f)
						{
							catchInput.isGroundCatching = false;
							catchInput.isAirCatching = true;
							RefreshCatchTime(num, GameGlobal.gGameTouchPlay.catchReactAirTime);
						}
					}
				}
				m_LongDownList.Clear();
			}
			Touch[] touches = Input.touches;
			for (int k = 0; k < touches.Length; k++)
			{
				Touch touch = touches[k];
				if (touch.phase == TouchPhase.Began)
				{
					CatchInput catchInput2 = m_CatchInputs[touch.fingerId];
					if (GameGlobal.gGameTouchPlay.catchReactGroundTime > 0f)
					{
						catchInput2.isGroundCatching = true;
						catchInput2.isAirCatching = false;
						RefreshCatchTime(touch.fingerId, GameGlobal.gGameTouchPlay.catchReactGroundTime);
					}
					if (GameGlobal.gGameTouchPlay.catchReactAirTime > 0f)
					{
						catchInput2.isGroundCatching = false;
						catchInput2.isAirCatching = true;
						RefreshCatchTime(touch.fingerId, GameGlobal.gGameTouchPlay.catchReactAirTime);
					}
				}
			}
		}

		public int IsCatching(bool isGround, float time)
		{
			for (int i = 0; i < m_CatchInputs.Count; i++)
			{
				CatchInput catchInput = m_CatchInputs[i];
				if (!(time - catchInput.time - 0.01f <= Singleton<BattleProperty>.instance.catchGapThreshold))
				{
					continue;
				}
				if (isGround)
				{
					if (catchInput.isGroundCatching)
					{
						if (m_LongDownList.IndexOf(i) != -1)
						{
							m_LongDownList[m_LongDownList.IndexOf(i)] = -1;
						}
						return i;
					}
				}
				else if (catchInput.isAirCatching)
				{
					if (m_LongDownList.IndexOf(i) != -1)
					{
						m_LongDownList[m_LongDownList.IndexOf(i)] = -1;
					}
					return i;
				}
			}
			return -1;
		}

		public void RefreshCatchTime(int index, float time)
		{
			m_CatchInputs[index].time = time;
		}
	}
}
