using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.GeneralLocalization;
using Rewired;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.PeroTools.Managers
{
	public class InputManager : Singleton<InputManager>
	{
		public delegate void OnActiveControllerChange(string controllerName);

		public enum ButtonState
		{
			Down,
			Up,
			Stay
		}

		public static class MDButtonName
		{
			public const string BattleGround = "BattleGround";

			public const string BattleAir = "BattleAir";

			public const string Fever = "Fever";
		}

		public static class MDButtonProposalName
		{
			public static string Custom = "Custom";

			public static string Default = "Default";

			public static string DefaultReverse = "DefaultReverse";

			public static string ModeB = "ModeB";

			public static string ModeBReverse = "ModeBReverse";

			public static string ModeC = "ModeC";

			public static string ModeCReverse = "ModeCReverse";

			public static string ModeD = "ModeD";

			public static string ModeDReverse = "ModeDReverse";
		}

		public enum DeviceHandles
		{
			Left = 1,
			Right,
			Both
		}

		public enum VibrationVaule
		{
			Default,
			MulHitting,
			LongPress,
			LongPressStartOrEnd
		}

		private IControlable m_Controller;

		public bool customIsChanged;

		public string keyBoardProposal;

		public string handleProposal;

		public string currentControllerName = "Keyboard";

		public string currentControllerHardwareName = "Keyboard";

		public bool isPauseMap;

		public bool isVibration;

		public bool isCustomMouse;

		private string m_CurrentLastActiveController;

		public OnActiveControllerChange onActiveControllerChange;

		public List<int> GetButtonWithState(string buttonName, ButtonState buttonState, ref int count)
		{
			switch (buttonState)
			{
			case ButtonState.Down:
				return GetButtonDown(buttonName);
			case ButtonState.Up:
				return GetButtonUp(buttonName);
			case ButtonState.Stay:
				return GetButton(buttonName);
			default:
				return null;
			}
		}

		public List<int> GetButtonDown(string buttonName)
		{
			return m_Controller.GetButtonDown(buttonName);
		}

		public List<int> GetButtonUp(string buttonName)
		{
			return m_Controller.GetButtonUp(buttonName);
		}

		public List<int> GetButton(string buttonName)
		{
			return m_Controller.GetButton(buttonName);
		}

		public void SwitchButtonProposal(string name)
		{
			m_Controller.SwitchProposal(name);
		}

		public void CustomButton(string buttonName, string key)
		{
			m_Controller.CustomButton(buttonName, key);
		}

		public void SetNsVibration(VibrationVaule vaule, DeviceHandles handles = DeviceHandles.Both, float time = 0.05f)
		{
			m_Controller.SetVibration(vaule, handles, time);
		}

		public void SetController(IControlable controller, string inputCtrlConfigName, bool updateControl = true)
		{
			m_Controller = controller;
			if (!string.IsNullOrEmpty(inputCtrlConfigName))
			{
				m_Controller.Init(inputCtrlConfigName);
			}
			if (updateControl)
			{
				SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop(Loop, UnityGameManager.LoopType.Update);
			}
		}

		private void Loop(float time)
		{
			if (Input.anyKeyDown || RewiredGetAnyButton())
			{
				UpdateLastActiveInputController();
			}
		}

		public void Disable()
		{
			m_Controller.OnDisableController();
		}

		public void OnClickChangeButton(string buttonName, string keyName)
		{
			m_Controller.OnClickChangeButton(buttonName, keyName);
		}

		public void OnChangeSpecialButton(string buttonName, string newKey, string uiName)
		{
			m_Controller.ChangeButton(buttonName, newKey, uiName);
		}

		public void OnClickNoKeyChangeButton(string buttonName)
		{
			m_Controller.OnClickNoKeyToChange(buttonName);
		}

		public void OnSetCustomRepeatList(List<string> list)
		{
			m_Controller.UpdateRepeatList(list);
		}

		public void OnSaveJsonProposal(string keyboardProposal, string handleProposal, bool isVibration)
		{
			m_Controller.SaveJsonProposal(keyboardProposal, handleProposal, isVibration);
		}

		public bool RewiredGetAnyButton()
		{
			if (!ReInput.isReady)
			{
				return false;
			}
			if (ReInput.players.GetPlayer(0).GetAnyNegativeButtonDown() || ReInput.players.GetPlayer(0).GetAnyButtonDown())
			{
				return true;
			}
			return false;
		}

		public bool RewiredGetButtonDown(string actionName)
		{
			if (!ReInput.isReady)
			{
				return false;
			}
			return ReInput.players.GetPlayer(0).GetButtonDown(actionName);
		}

		public bool RewiredGetButton(string actionName)
		{
			if (!ReInput.isReady)
			{
				return false;
			}
			return ReInput.players.GetPlayer(0).GetButton(actionName);
		}

		public bool RewiredGetButtonUp(string actionName)
		{
			if (!ReInput.isReady)
			{
				return false;
			}
			return ReInput.players.GetPlayer(0).GetButtonUp(actionName);
		}

		public float RewiredGetAxisRaw(string actionName)
		{
			if (!ReInput.isReady)
			{
				return 0f;
			}
			return ReInput.players.GetPlayer(0).GetAxisRaw(actionName);
		}

		public void UpdateLastActiveInputController()
		{
			m_CurrentLastActiveController = ReInput.controllers.GetLastActiveController().name;
			currentControllerHardwareName = ReInput.controllers.GetLastActiveController().name;
			if (m_CurrentLastActiveController == "Mouse")
			{
				m_CurrentLastActiveController = "Keyboard";
				currentControllerHardwareName = "Keyboard";
			}
			if (m_CurrentLastActiveController.StartsWith("Sony DualShock"))
			{
				m_CurrentLastActiveController = "PS4";
				currentControllerHardwareName = "PS4";
			}
			if (m_CurrentLastActiveController != "Mouse" && m_CurrentLastActiveController != "Keyboard" && m_CurrentLastActiveController != "PS4")
			{
				if (m_CurrentLastActiveController == "YuanCon")
				{
					m_CurrentLastActiveController = "YuanCon";
					currentControllerHardwareName = "CustomController";
				}
				else if (m_CurrentLastActiveController.StartsWith("Pro Controller") || m_CurrentLastActiveController.StartsWith("Nintendo Switch"))
				{
					m_CurrentLastActiveController = "Ns";
					currentControllerHardwareName = "NsPro";
				}
				else
				{
					m_CurrentLastActiveController = "XBox";
					currentControllerHardwareName = ((!currentControllerHardwareName.StartsWith("XInput")) ? "Other" : "Xbox");
				}
			}
			if (m_CurrentLastActiveController != currentControllerName)
			{
				currentControllerName = m_CurrentLastActiveController;
				if (m_CurrentLastActiveController == "YuanCon")
				{
					SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption("Controller", "Custom Controller");
				}
				else if (m_CurrentLastActiveController.StartsWith("Pro Controller") || m_CurrentLastActiveController.StartsWith("Nintendo Switch"))
				{
					SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption("Controller", "Ns");
					m_CurrentLastActiveController = "PS4";
					currentControllerName = m_CurrentLastActiveController;
				}
				else
				{
					SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption("Controller", m_CurrentLastActiveController);
				}
				onActiveControllerChange(m_CurrentLastActiveController);
				if (Singleton<SceneManager>.instance.curScene.name != "GameMain")
				{
					RewiredJoystickControllerMapSetEnable(true, "UI", m_CurrentLastActiveController);
				}
				else if (Singleton<SceneManager>.instance.curScene.name == "GameMain" && isPauseMap)
				{
					RewiredJoystickControllerMapSetEnable(true, "UI", m_CurrentLastActiveController);
				}
			}
		}

		public void RewiredJoystickControllerMapSetEnable(bool able, string categoryName)
		{
			if (currentControllerName == "Keyboard")
			{
				return;
			}
			if (categoryName == "UI")
			{
				if (currentControllerName == "PS4" || currentControllerName == "Ns")
				{
					categoryName = "PS4UI";
					ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(false, ControllerType.Joystick, "XBoxUI");
					ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(false, ControllerType.Joystick, "CustomControllerUI");
				}
				else
				{
					if (currentControllerName == "YuanCon")
					{
						categoryName = "CustomControllerUI";
						ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(false, ControllerType.Joystick, "XBoxUI");
					}
					else
					{
						categoryName = "XBoxUI";
					}
					ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(false, ControllerType.Joystick, "PS4UI");
					ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(false, ControllerType.Joystick, "CustomControllerUI");
				}
			}
			ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(able, ControllerType.Joystick, categoryName);
		}

		public void RewiredJoystickControllerMapSetEnable(bool able, string categoryName, string controllerName)
		{
			if (currentControllerName == "Keyboard")
			{
				return;
			}
			if (categoryName == "UI")
			{
				if (controllerName == "PS4" || controllerName == "Ns")
				{
					categoryName = "PS4UI";
					ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(false, ControllerType.Joystick, "XBoxUI");
					ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(false, ControllerType.Joystick, "CustomControllerUI");
				}
				else
				{
					if (currentControllerName == "YuanCon")
					{
						categoryName = "CustomControllerUI";
						ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(false, ControllerType.Joystick, "XBoxUI");
					}
					else
					{
						categoryName = "XBoxUI";
					}
					ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(false, ControllerType.Joystick, "PS4UI");
					ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(false, ControllerType.Joystick, "CustomControllerUI");
				}
			}
			ReInput.players.GetPlayer(0).controllers.maps.SetMapsEnabled(able, ControllerType.Joystick, categoryName);
		}

		public void SwitchHandleProposal(string proposal)
		{
			if (ReInput.controllers.GetControllerCount(ControllerType.Joystick) == 0)
			{
				return;
			}
			Player player = ReInput.players.GetPlayer(0);
			player.controllers.maps.SetMapsEnabled(false, ControllerType.Joystick, 0);
			player.controllers.maps.SetMapsEnabled(false, ControllerType.Joystick, 1);
			player.controllers.maps.SetMapsEnabled(false, ControllerType.Joystick, 2);
			player.controllers.maps.SetMapsEnabled(false, ControllerType.Joystick, 3);
			player.controllers.maps.SetMapsEnabled(true, ControllerType.Joystick, proposal);
			Controller shouTaiController = ReInput.controllers.GetController(ControllerType.Joystick, 0);
			if (IsConnetShouTai(out shouTaiController))
			{
				player.controllers.maps.SetMapsEnabled(false, shouTaiController, proposal);
				if (handleProposal == "ModeB" || handleProposal == "ModeD")
				{
					player.controllers.maps.SetMapsEnabled(true, shouTaiController, "ShouTaiDefault");
					player.controllers.maps.SetMapsEnabled(false, shouTaiController, "ShouTaiModeB");
					player.controllers.maps.SetMapsEnabled(false, shouTaiController, "CustomControllerUI");
				}
				else
				{
					player.controllers.maps.SetMapsEnabled(false, shouTaiController, "ShouTaiDefault");
					player.controllers.maps.SetMapsEnabled(true, shouTaiController, "ShouTaiModeB");
					player.controllers.maps.SetMapsEnabled(false, shouTaiController, "CustomControllerUI");
				}
			}
		}

		public bool IsConnetShouTai()
		{
			IList<Controller> controllers = ReInput.controllers.Controllers;
			foreach (Controller item in controllers)
			{
				if (item.name == "YuanCon")
				{
					return true;
				}
			}
			return false;
		}

		public bool IsConnetShouTai(out Controller shouTaiController)
		{
			IList<Controller> controllers = ReInput.controllers.Controllers;
			foreach (Controller item in controllers)
			{
				if (item.name == "YuanCon")
				{
					shouTaiController = item;
					return true;
				}
			}
			shouTaiController = controllers[0];
			return false;
		}

		public void OnHandleConnet(string battleSceneName, bool isSucceed, bool isDead, bool isPause, bool isTutorial)
		{
			if (battleSceneName == "GameMain" && !isSucceed && !isDead && !isPause)
			{
				SwitchHandleProposal((!isTutorial) ? handleProposal : "ModeA");
				RewiredJoystickControllerMapSetEnable(false, "UI");
			}
			else
			{
				RewiredJoystickControllerMapSetEnable(true, "UI");
				RewiredJoystickControllerMapSetEnable(false, handleProposal);
			}
		}

		public bool CheckMouseClickUI()
		{
			if (EventSystem.current == null)
			{
				return false;
			}
			return EventSystem.current.IsPointerOverGameObject();
		}
	}
}
