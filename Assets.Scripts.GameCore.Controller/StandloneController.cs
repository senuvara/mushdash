using Assets.Scripts.GameCore.Controller.Configs;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using GameLogic;
using Newtonsoft.Json.Linq;
using Rewired;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.GameCore.Controller
{
	public class StandloneController : Controller<StandloneCtrlConfig>
	{
		private Dictionary<string, List<KeyCode>> m_Dict;

		private List<KeyCode> m_List;

		private List<string> proposalList;

		private List<int> m_DownList;

		private List<int> m_MoveList;

		private List<int> m_UpList;

		private string m_CurrentClickKeyButtonName;

		private InputMapper m_ListenCurKey = new InputMapper();

		private InputMapper m_ListenCurMouse = new InputMapper();

		private List<string> m_CheckRepeatKeyList = new List<string>();

		private bool m_IsKey;

		private int m_RepeatIndex;

		private JObject m_KeyList_JObj;

		public static Dictionary<string, float> values = new Dictionary<string, float>
		{
			{
				"Default",
				0.5f
			},
			{
				"MulHitting",
				1f
			},
			{
				"LongPress",
				1f
			},
			{
				"LongPressStartOrEnd",
				1f
			}
		};

		private List<string> m_HandleGroundActionNameList = new List<string>
		{
			"BattleGround",
			"BattleGround1",
			"BattleGround2",
			"BattleGround3",
			"BattleGround4",
			"BattleGround5",
			"BattleGround6",
			"BattleGround7",
			"BattleGround8",
			"BattleGround9",
			"BattleGround10",
			"BattleGround11",
			"BattleGround12",
			"BattleGround13",
			"BattleGround14",
			"BattleGround15"
		};

		private List<string> m_HandleAirActionNameList = new List<string>
		{
			"BattleAir",
			"BattleAir1",
			"BattleAir2",
			"BattleAir3",
			"BattleAir4",
			"BattleAir5",
			"BattleAir6",
			"BattleAir7",
			"BattleAir8",
			"BattleAir9",
			"BattleAir10",
			"BattleAir11",
			"BattleAir12",
			"BattleAir13"
		};

		private Player m_Player => ReInput.players.GetPlayer(0);

		public override void OnInit()
		{
			GetDefaultKeyList();
			SwitchProposal(configs.CurrentProposal);
			m_ListenCurKey.InputMappedEvent += OnInputMapper;
			m_ListenCurMouse.InputMappedEvent += OnInputMapper;
			m_ListenCurMouse.options.ignoreMouseXAxis = true;
			m_ListenCurMouse.options.ignoreMouseYAxis = true;
			Assets.Scripts.PeroTools.Managers.InputManager instance = Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance;
			instance.onActiveControllerChange = (Assets.Scripts.PeroTools.Managers.InputManager.OnActiveControllerChange)Delegate.Combine(instance.onActiveControllerChange, new Assets.Scripts.PeroTools.Managers.InputManager.OnActiveControllerChange(SetHandleReverse));
			if (!m_Dict["BattleAir"].Contains(KeyCode.Mouse1) && !m_Dict["BattleGround"].Contains(KeyCode.Mouse1))
			{
				Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.isCustomMouse = true;
			}
			else
			{
				Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.isCustomMouse = false;
			}
			m_DownList = new List<int>(16);
			m_MoveList = new List<int>(16);
			m_UpList = new List<int>(16);
		}

		public override void OnDisableController()
		{
			m_ListenCurKey.Stop();
			m_ListenCurMouse.Stop();
		}

		public override List<int> GetButtonDown(string buttonName)
		{
			m_DownList.Clear();
			if (Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.currentControllerName != "Keyboard")
			{
				switch (buttonName)
				{
				case "Fever":
					if (ReInput.players.GetPlayer(0).GetButtonDown("Fever"))
					{
						m_DownList.Add(0);
						return m_DownList;
					}
					break;
				case "BattleGround":
				{
					for (int j = 0; j < m_HandleGroundActionNameList.Count; j++)
					{
						if (ReInput.players.GetPlayer(0).GetButtonDown(m_HandleGroundActionNameList[j]) && !SpecialCheckJoystickFeverBtn(m_HandleGroundActionNameList[j]))
						{
							m_DownList.Add(j);
						}
					}
					return m_DownList;
				}
				case "BattleAir":
				{
					for (int i = 0; i < m_HandleAirActionNameList.Count; i++)
					{
						if (ReInput.players.GetPlayer(0).GetButtonDown(m_HandleAirActionNameList[i]) && !SpecialCheckJoystickFeverBtn(m_HandleAirActionNameList[i]))
						{
							m_DownList.Add(m_HandleGroundActionNameList.Count + i);
						}
					}
					return m_DownList;
				}
				}
			}
			else
			{
				switch (buttonName)
				{
				case "BattleAir":
					if (Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.isCustomMouse && Input.GetKeyDown(KeyCode.Mouse1) && !Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.CheckMouseClickUI())
					{
						m_DownList.Add(16);
						return m_DownList;
					}
					break;
				case "BattleGround":
					if (Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.isCustomMouse && Input.GetKeyDown(KeyCode.Mouse0) && !Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.CheckMouseClickUI())
					{
						m_DownList.Add(0);
						return m_DownList;
					}
					break;
				}
				if ((buttonName.CompareTo("BattleAir") == 0 || buttonName.CompareTo("BattleGround") == 0 || buttonName.CompareTo("Fever") == 0) && GetButtonProposal(buttonName))
				{
					for (int k = 0; k < m_List.Count; k++)
					{
						if (Input.GetKeyDown(m_List[k]))
						{
							if (m_List[k] == KeyCode.Mouse0 && Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.CheckMouseClickUI())
							{
								return m_DownList;
							}
							m_DownList.Add(k + ((buttonName.CompareTo("BattleAir") == 0) ? 16 : 0));
						}
					}
					return m_DownList;
				}
			}
			return m_DownList;
		}

		public override List<int> GetButton(string buttonName)
		{
			m_MoveList.Clear();
			if (Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.currentControllerName != "Keyboard")
			{
				switch (buttonName)
				{
				case "Fever":
					if (ReInput.players.GetPlayer(0).GetButton("Fever"))
					{
						m_MoveList.Add(0);
						return m_MoveList;
					}
					break;
				case "BattleGround":
				{
					for (int j = 0; j < m_HandleGroundActionNameList.Count; j++)
					{
						if (ReInput.players.GetPlayer(0).GetButton(m_HandleGroundActionNameList[j]) && !SpecialCheckJoystickFeverBtn(m_HandleGroundActionNameList[j]))
						{
							m_MoveList.Add(j);
						}
					}
					return m_MoveList;
				}
				case "BattleAir":
				{
					for (int i = 0; i < m_HandleAirActionNameList.Count; i++)
					{
						if (ReInput.players.GetPlayer(0).GetButton(m_HandleAirActionNameList[i]) && !SpecialCheckJoystickFeverBtn(m_HandleAirActionNameList[i]))
						{
							m_MoveList.Add(i + m_HandleGroundActionNameList.Count);
						}
					}
					return m_MoveList;
				}
				}
			}
			else
			{
				switch (buttonName)
				{
				case "BattleAir":
					if (Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.isCustomMouse && Input.GetKey(KeyCode.Mouse1))
					{
						m_MoveList.Add(16);
						return m_MoveList;
					}
					break;
				case "BattleGround":
					if (Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.isCustomMouse && Input.GetKey(KeyCode.Mouse0))
					{
						m_MoveList.Add(0);
						return m_MoveList;
					}
					break;
				}
				if ((buttonName.CompareTo("BattleAir") == 0 || buttonName.CompareTo("BattleGround") == 0 || buttonName.CompareTo("Fever") == 0) && GetButtonProposal(buttonName))
				{
					for (int k = 0; k < m_List.Count; k++)
					{
						if (Input.GetKey(m_List[k]))
						{
							m_MoveList.Add(k + ((buttonName.CompareTo("BattleAir") == 0) ? 16 : 0));
						}
					}
					return m_MoveList;
				}
			}
			return m_MoveList;
		}

		public override List<int> GetButtonUp(string buttonName)
		{
			m_UpList.Clear();
			if (Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.currentControllerName != "Keyboard")
			{
				switch (buttonName)
				{
				case "Fever":
					if (ReInput.players.GetPlayer(0).GetButtonUp("Fever"))
					{
						m_UpList.Add(0);
						return m_UpList;
					}
					break;
				case "BattleGround":
				{
					for (int j = 0; j < m_HandleGroundActionNameList.Count; j++)
					{
						if (ReInput.players.GetPlayer(0).GetButtonUp(m_HandleGroundActionNameList[j]) && !SpecialCheckJoystickFeverBtn(m_HandleGroundActionNameList[j]))
						{
							m_UpList.Add(j);
						}
					}
					return m_UpList;
				}
				case "BattleAir":
				{
					for (int i = 0; i < m_HandleAirActionNameList.Count; i++)
					{
						if (ReInput.players.GetPlayer(0).GetButtonUp(m_HandleAirActionNameList[i]) && !SpecialCheckJoystickFeverBtn(m_HandleAirActionNameList[i]))
						{
							m_UpList.Add(i + m_HandleGroundActionNameList.Count);
						}
					}
					return m_UpList;
				}
				}
			}
			else
			{
				switch (buttonName)
				{
				case "BattleAir":
					if (Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.isCustomMouse && Input.GetKeyUp(KeyCode.Mouse1))
					{
						m_UpList.Add(16);
						return m_UpList;
					}
					break;
				case "BattleGround":
					if (Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.isCustomMouse && Input.GetKeyUp(KeyCode.Mouse0))
					{
						m_UpList.Add(0);
						return m_UpList;
					}
					break;
				}
				if ((buttonName.CompareTo("BattleAir") == 0 || buttonName.CompareTo("BattleGround") == 0 || buttonName.CompareTo("Fever") == 0) && GetButtonProposal(buttonName))
				{
					for (int k = 0; k < m_List.Count; k++)
					{
						if (Input.GetKeyUp(m_List[k]))
						{
							m_UpList.Add(k + ((buttonName.CompareTo("BattleAir") == 0) ? 16 : 0));
						}
					}
					return m_UpList;
				}
			}
			return m_UpList;
		}

		private bool GetButtonProposal(string buttonName)
		{
			if (!m_Dict.TryGetValue(buttonName, out m_List))
			{
				return false;
			}
			return true;
		}

		public override void SwitchProposal(string proposalName)
		{
			if (!configs.buttonKeyEnties.TryGetValue(proposalName, out m_Dict))
			{
				Debug.LogErrorFormat("Get Button Proposal by Key [{0}] Failed", proposalName);
			}
			configs.CurrentProposal = proposalName;
		}

		public override void CustomButton(string buttonName, string key)
		{
			List<KeyCode> list = configs.buttonKeyEnties[Assets.Scripts.PeroTools.Managers.InputManager.MDButtonProposalName.Custom][buttonName];
			KeyCode value = (KeyCode)Enum.Parse(typeof(KeyCode), key);
			int num = PnlInputPc.Instance().currentSelectedCustomKeyIndex;
			if (num > 3)
			{
				num -= 4;
			}
			list[num] = value;
			m_CheckRepeatKeyList[PnlInputPc.Instance().currentSelectedCustomKeyIndex] = key;
			PnlInputPc.Instance().ChangeCustomKeyUI(key);
		}

		public override void OnClickChangeButton(string buttonName, string keyName)
		{
			PnlInputPc.Instance().customizeSelect.SetEnable(false);
			Singleton<EventManager>.instance.Invoke("UI/DisableInputKey");
			PnlInputPc.Instance().customizeSelect.OnCustomKey(true);
			Debug.Log(EventSystem.current.currentSelectedGameObject);
			PnlInputPc.Instance().allowListenKey = true;
			m_IsKey = true;
			m_CurrentClickKeyButtonName = buttonName;
			ControllerMap map = m_Player.controllers.maps.GetMap(ControllerType.Keyboard, 0, "Default", "Default");
			ControllerMap map2 = m_Player.controllers.maps.GetMap(ControllerType.Mouse, 0, "Default", "Default");
			m_ListenCurKey.Start(new InputMapper.Context
			{
				actionId = 0,
				controllerMap = map,
				actionElementMapToReplace = map.GetElementMap(0)
			});
			m_ListenCurMouse.Start(new InputMapper.Context
			{
				actionId = 0,
				controllerMap = map2,
				actionElementMapToReplace = map2.GetElementMap(0)
			});
		}

		public override void OnClickNoKeyToChange(string buttonName)
		{
			PnlInputPc.Instance().customizeSelect.SetEnable(false);
			Singleton<EventManager>.instance.Invoke("UI/DisableInputKey");
			PnlInputPc.Instance().customizeSelect.OnCustomKey(true);
			PnlInputPc.Instance().allowListenKey = true;
			m_IsKey = false;
			m_CurrentClickKeyButtonName = buttonName;
			ControllerMap map = m_Player.controllers.maps.GetMap(ControllerType.Keyboard, 0, "Default", "Default");
			ControllerMap map2 = m_Player.controllers.maps.GetMap(ControllerType.Mouse, 0, "Default", "Default");
			m_ListenCurKey.Start(new InputMapper.Context
			{
				actionId = 0,
				controllerMap = map,
				actionElementMapToReplace = map.GetElementMap(0)
			});
			m_ListenCurMouse.Start(new InputMapper.Context
			{
				actionId = 0,
				controllerMap = map2,
				actionElementMapToReplace = map2.GetElementMap(0)
			});
		}

		public override void ChangeButton(string buttonName, string newKey, string uiName)
		{
			m_ListenCurKey.Stop();
			m_ListenCurMouse.Stop();
			List<KeyCode> list = configs.buttonKeyEnties["Custom"][buttonName];
			KeyCode value = (KeyCode)Enum.Parse(typeof(KeyCode), newKey);
			int num = 0;
			if (buttonName != "Fever")
			{
				num = PnlInputPc.Instance().currentSelectedCustomKeyIndex;
				if (num > 3)
				{
					num -= 4;
				}
			}
			if (CheckInputKeyRepeat(newKey))
			{
				PnlInputPc.Instance().SetRepeatKeyToNoUI(m_RepeatIndex);
				KeyCode keyCode = (KeyCode)Enum.Parse(typeof(KeyCode), m_CheckRepeatKeyList[m_RepeatIndex]);
				m_CheckRepeatKeyList[m_RepeatIndex] = "None";
				if (m_RepeatIndex < m_CheckRepeatKeyList.Count - 1)
				{
					ChangeJsonKeyList(m_RepeatIndex, "None");
				}
				else
				{
					ChangeJsonFever("None");
				}
				string text = "BattleAir";
				if (m_RepeatIndex > 3)
				{
					text = "BattleGround";
					m_RepeatIndex -= 4;
				}
				else
				{
					text = "BattleAir";
				}
				if (m_RepeatIndex >= configs.buttonKeyEnties["Custom"][text].Count)
				{
					configs.buttonKeyEnties["Custom"]["Fever"][0] = KeyCode.None;
				}
				else
				{
					configs.buttonKeyEnties["Custom"][text][m_RepeatIndex] = KeyCode.None;
				}
			}
			list[num] = value;
			m_CheckRepeatKeyList[PnlInputPc.Instance().currentSelectedCustomKeyIndex] = list[num].ToString();
			string str = string.Empty;
			for (int i = 0; i < m_CheckRepeatKeyList.Count; i++)
			{
				str = str + "," + m_CheckRepeatKeyList[i];
			}
			PnlInputPc.Instance().ChangeCustomKeyUI(uiName);
			if (buttonName != "Fever")
			{
				ChangeJsonKeyList(newKey);
			}
			else
			{
				ChangeJsonFever(newKey);
			}
			if (!Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.customIsChanged)
			{
				ChangeJsonOther("IsChanged", "true");
				Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.customIsChanged = true;
			}
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				PnlInputPc.Instance().customizeSelect.SetEnable(true, false);
				Singleton<EventManager>.instance.Invoke("UI/EnableInputKey");
				PnlInputPc.Instance().customizeSelect.OnCustomKey(false);
				Debug.Log(PnlInputPc.Instance().GetCurrentSelectedCustomKey());
				EventSystem.current.SetSelectedGameObject(PnlInputPc.Instance().GetCurrentSelectedCustomKey());
			}, 0.1f);
			PnlInputPc.Instance().shiftAndCtrlCheck = false;
			PnlInputPc.Instance().shiftOrCtrl = string.Empty;
			PnlInputPc.Instance().allowListenKey = false;
		}

		public void GetDefaultKeyList()
		{
			m_KeyList_JObj = new JObject();
			string empty = string.Empty;
			if (PlayerPrefs.HasKey("Controller"))
			{
				empty = Singleton<ConfigManager>.instance.GetString("Controller");
			}
			else
			{
				empty = "{\"Keylist\":{ \"Custom\":[{\"Key\":\"None\",\"Type\":\"BattleAir\"},{\"Key\":\"None\",\"Type\":\"BattleAir\"},{\"Key\":\"None\",\"Type\":\"BattleAir\"},{\"Key\":\"None\",\"Type\":\"BattleAir\"},{\"Key\":\"None\",\"Type\":\"BattleGround\"},{\"Key\":\"None\",\"Type\":\"BattleGround\"},{\"Key\":\"None\",\"Type\":\"BattleGround\"},{\"Key\":\"None\",\"Type\":\"BattleGround\"}]},\"IsChanged\":\"false\",\"KeyBoardProposal\":\"Default\",\"HandleProposal\":\"Default\",\"IsVibration\":\"true\",\"FeverKey\":\"Space\"}";
				Singleton<ConfigManager>.instance.SaveString("Controller", empty);
			}
			m_KeyList_JObj = JsonUtils.Deserialize<JObject>(empty);
			for (int i = 0; i < m_KeyList_JObj["Keylist"]["Custom"].Count(); i++)
			{
				int num = i;
				if (num > 3)
				{
					num -= 4;
				}
				m_CheckRepeatKeyList.Add((string)m_KeyList_JObj["Keylist"]["Custom"][i]["Key"]);
				configs.buttonKeyEnties["Custom"][(string)m_KeyList_JObj["Keylist"]["Custom"][i]["Type"]][num] = (KeyCode)Enum.Parse(typeof(KeyCode), (string)m_KeyList_JObj["Keylist"]["Custom"][i]["Key"]);
			}
			if (m_KeyList_JObj["FeverKey"] != null)
			{
				m_CheckRepeatKeyList.Add((string)m_KeyList_JObj["FeverKey"]);
				configs.buttonKeyEnties["Custom"]["Fever"][0] = (KeyCode)Enum.Parse(typeof(KeyCode), (string)m_KeyList_JObj["FeverKey"]);
			}
			else
			{
				m_KeyList_JObj["FeverKey"] = "Space";
				Singleton<ConfigManager>.instance.SaveString("Controller", m_KeyList_JObj.ToString());
				m_CheckRepeatKeyList.Add((string)m_KeyList_JObj["FeverKey"]);
				configs.buttonKeyEnties["Custom"]["Fever"][0] = (KeyCode)Enum.Parse(typeof(KeyCode), (string)m_KeyList_JObj["FeverKey"]);
			}
			if ((string)m_KeyList_JObj["IsChanged"] == "false")
			{
				Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.customIsChanged = false;
			}
			else if ((string)m_KeyList_JObj["IsChanged"] == "true")
			{
				Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.customIsChanged = true;
			}
			Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.keyBoardProposal = (string)m_KeyList_JObj["KeyBoardProposal"];
			configs.CurrentProposal = (string)m_KeyList_JObj["KeyBoardProposal"];
			Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.handleProposal = (string)m_KeyList_JObj["HandleProposal"];
			if ((string)m_KeyList_JObj["IsVibration"] == "false")
			{
				Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.isVibration = false;
			}
			else if ((string)m_KeyList_JObj["IsVibration"] == "true")
			{
				Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.isVibration = true;
			}
		}

		private void OnInputMapper(InputMapper.InputMappedEventData data)
		{
			if (!PnlInputPc.Instance().shiftAndCtrlCheck)
			{
				m_ListenCurKey.Stop();
				m_ListenCurMouse.Stop();
				string empty = string.Empty;
				empty = data.actionElementMap.keyCode.ToString();
				string uiName = PnlInputPc.Instance().SimplifyKey(data.actionElementMap.elementIdentifierName);
				if (data.actionElementMap.elementIdentifierName == "Left Mouse Button")
				{
					empty = "Mouse0";
					uiName = empty;
				}
				if (data.actionElementMap.elementIdentifierName == "Right Mouse Button")
				{
					empty = "Mouse1";
					uiName = empty;
				}
				if (!CheckDisableKey(empty) && data.actionElementMap.elementIdentifierName != "Mouse Wheel Down" && data.actionElementMap.elementIdentifierName != "Mouse Wheel Up")
				{
					ChangeButton(m_CurrentClickKeyButtonName, empty, uiName);
				}
				else if (CheckDisableKey(empty))
				{
					ChangeButton(m_CurrentClickKeyButtonName, "None", "None");
				}
			}
		}

		private bool CheckInputKeyRepeat(string key)
		{
			bool result = false;
			for (int i = 0; i < m_CheckRepeatKeyList.Count; i++)
			{
				if (key == m_CheckRepeatKeyList[i])
				{
					m_RepeatIndex = i;
					result = true;
				}
			}
			return result;
		}

		public override void UpdateRepeatList(List<string> list)
		{
			m_CheckRepeatKeyList = list;
			for (int i = 0; i < m_CheckRepeatKeyList.Count - 1; i++)
			{
				m_KeyList_JObj["Keylist"]["Custom"][i]["Key"] = m_CheckRepeatKeyList[i];
			}
			m_KeyList_JObj["IsChanged"] = "true";
			Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.customIsChanged = true;
			Singleton<ConfigManager>.instance.SaveString("Controller", m_KeyList_JObj.ToString());
		}

		private void ChangeJsonKeyList(string newKey)
		{
			m_KeyList_JObj["Keylist"]["Custom"][PnlInputPc.Instance().currentSelectedCustomKeyIndex]["Key"] = newKey;
			Singleton<ConfigManager>.instance.SaveString("Controller", m_KeyList_JObj.ToString());
		}

		private void ChangeJsonKeyList(int index, string newKey)
		{
			m_KeyList_JObj["Keylist"]["Custom"][index]["Key"] = newKey;
			Singleton<ConfigManager>.instance.SaveString("Controller", m_KeyList_JObj.ToString());
		}

		private void ChangeJsonOther(string key, string value)
		{
			m_KeyList_JObj[key] = value;
			Singleton<ConfigManager>.instance.SaveString("Controller", m_KeyList_JObj.ToString());
		}

		private void ChangeJsonFever(string key)
		{
			m_KeyList_JObj["FeverKey"] = key;
			Singleton<ConfigManager>.instance.SaveString("Controller", m_KeyList_JObj.ToString());
		}

		public override void SaveJsonProposal(string keyboardProposal, string handleProposal, bool isVibration)
		{
			ChangeJsonOther("KeyBoardProposal", keyboardProposal);
			Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.keyBoardProposal = keyboardProposal;
			configs.CurrentProposal = keyboardProposal;
			SwitchProposal(keyboardProposal);
			ChangeJsonOther("HandleProposal", handleProposal);
			Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.handleProposal = handleProposal;
			if (isVibration)
			{
				ChangeJsonOther("IsVibration", "true");
			}
			else
			{
				ChangeJsonOther("IsVibration", "false");
			}
			Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.isVibration = isVibration;
		}

		private void SetHandleReverse(string controllerName)
		{
			if (GameTouchPlay.isReverse && controllerName != "Keyboard")
			{
				GameTouchPlay.isBtnLeftRight = false;
			}
			else if (controllerName == "Keyboard")
			{
				GameTouchPlay.isBtnLeftRight = true;
			}
		}

		public override void SetVibration(Assets.Scripts.PeroTools.Managers.InputManager.VibrationVaule vaule, Assets.Scripts.PeroTools.Managers.InputManager.DeviceHandles handles, float duration)
		{
			ReInput.players.GetPlayer(0).SetVibration(1, values[vaule.ToString()], duration);
		}

		private bool CheckDisableKey(string key)
		{
			if (key != "F1" && key != "F2" && key != "F3" && key != "F4" && key != "F5" && key != "F6" && key != "F7" && key != "F8" && key != "F9" && key != "F10" && key != "F11" && key != "F12" && key != "Pause" && key != "ScrollLock" && key != "BackQuote" && key != "Escape" && key != "Alt" && key != "Numlock" && key != "None")
			{
				return false;
			}
			return true;
		}

		private bool SpecialCheckJoystickFeverBtn(string btnName)
		{
			if (!FeverManager.Instance.GetIsAutoFever() && Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.currentControllerName != "YuanCon")
			{
				if (Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.handleProposal == "Default")
				{
					if (btnName == "BattleGround" || btnName == "BattleAir")
					{
						return true;
					}
					return false;
				}
				if (btnName == "BattleAir1" || btnName == "BattleAir")
				{
					return true;
				}
				return false;
			}
			return false;
		}
	}
}
