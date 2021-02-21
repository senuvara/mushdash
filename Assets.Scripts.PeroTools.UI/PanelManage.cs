using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.UI
{
	public class PanelManage
	{
		public static bool isInputEnable = true;

		public static Stack<PanelType> panel = new Stack<PanelType>();

		public static Stack<UISelectManage> uiSelectManagers = new Stack<UISelectManage>();

		public static Stack<GameObject> optionSelectStack = new Stack<GameObject>();

		public PanelManage()
		{
			Singleton<EventManager>.instance.RegEvent("UI/DisableInputKey").trigger += DisableInput;
			Singleton<EventManager>.instance.RegEvent("UI/EnableInputKey").trigger += EnableInput;
		}

		public static void DisableInput(object sender, object reciever, object[] args)
		{
			if (panel.Peek() != 0)
			{
				panel.Push(PanelType.None);
				isInputEnable = false;
				Debug.Log("Disable Input");
			}
		}

		public static void EnableInput(object sender, object reciever, object[] args)
		{
			if (panel.Peek() == PanelType.None)
			{
				panel.Pop();
				isInputEnable = true;
				Debug.Log("Enable Input");
			}
		}

		public static void PushPanel(PanelType p, UISelectManage manager)
		{
			if (!isInputEnable)
			{
				panel.Pop();
				panel.Push(p);
				uiSelectManagers.Push(manager);
				panel.Push(PanelType.None);
			}
			else
			{
				panel.Push(p);
				uiSelectManagers.Push(manager);
			}
		}

		public static void PopPanel()
		{
			if (!isInputEnable)
			{
				panel.Pop();
				panel.Pop();
				uiSelectManagers.Pop();
				panel.Push(PanelType.None);
			}
			else
			{
				panel.Pop();
				uiSelectManagers.Pop();
			}
		}
	}
}
