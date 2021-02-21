using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.UI
{
	public class InputKeyBindingForWelcomeAnyKey : MonoBehaviour
	{
		public enum Type
		{
			Button,
			AxisPositive,
			AxisNegative
		}

		[ShowInInspector]
		[ReadOnly]
		private PanelType m_PanelOrder;

		public Type btnType;

		[ShowIf("btnType", Type.Button, true)]
		[Tooltip("指定按键名称或者 AnyKey ")]
		public string buttonName;

		[ShowIf("ExceptButton", true)]
		public string axisName;

		[ShowIf("ExceptButton", true)]
		public float axisSpeed = 0.01f;

		[ShowIf("btnType", Type.Button, true)]
		[Tooltip("指定不能同时按下的按键，例如确定和返回不能同时按下")]
		public InputKeyBinding excludeButton;

		private ReuseScorllRect m_ReuseScorllRect;

		private Scrollbar m_Scrollbar;

		private Slider m_Slider;

		public bool onlyHandleController;

		private bool m_IsAxisInUse;

		private static List<InputKeyBinding> m_InputKeyBindings;

		private bool m_AixsLongPress;

		[Tooltip("用于屏蔽键盘中JKZX等功能键，保留ESC和Enter的功能键")]
		public bool disableSimpleKey;

		private bool IsGetExcludeButtonDown()
		{
			return (bool)excludeButton && Input.GetButtonDown(excludeButton.buttonName);
		}

		private bool IsReuseScorllRect()
		{
			return m_ReuseScorllRect != null;
		}

		private bool IsScrollBar()
		{
			return m_Scrollbar != null;
		}

		private bool ExceptButton()
		{
			return btnType != Type.Button;
		}

		public void SetPanelOrder(PanelType type)
		{
			m_PanelOrder = type;
		}

		private bool IsOnCurrentPanel()
		{
			return (PanelManage.panel.Peek() & m_PanelOrder) != 0 && (PanelManage.panel.Peek() & m_PanelOrder) != PanelType.Popup;
		}

		private void PressDown()
		{
			if (!EventSystem.current)
			{
				return;
			}
			LongPressButton component = GetComponent<LongPressButton>();
			if (component != null && component.IsActive() && component.IsInteractable())
			{
				component.OnPointerDown(null);
			}
			Button component2 = GetComponent<Button>();
			if (component2 != null && component2.IsActive() && component2.IsInteractable())
			{
				component2.onClick.Invoke();
				return;
			}
			Toggle component3 = GetComponent<Toggle>();
			if (component3 != null && component3.IsActive() && component3.IsInteractable())
			{
				component3.isOn = !component3.isOn;
			}
		}

		private void PressUp()
		{
			if ((bool)EventSystem.current)
			{
				LongPressButton component = GetComponent<LongPressButton>();
				if (component != null && component.IsActive() && component.IsInteractable())
				{
					component.OnPointerUp(null);
				}
			}
		}

		private void Awake()
		{
			m_ReuseScorllRect = GetComponent<ReuseScorllRect>();
			m_Scrollbar = GetComponent<Scrollbar>();
			m_Slider = GetComponent<Slider>();
		}

		private void Update()
		{
			switch (btnType)
			{
			case Type.Button:
				if (buttonName == "AnyKey")
				{
					if (Input.anyKeyDown || Singleton<InputManager>.instance.RewiredGetAnyButton())
					{
						Debug.Log("Any Key Down");
						PressDown();
					}
				}
				else if ((Input.GetButtonDown(buttonName) && !IsGetExcludeButtonDown() && !onlyHandleController) || (Singleton<InputManager>.instance.RewiredGetButtonDown(buttonName) && !IsGetExcludeButtonDown()))
				{
					if (disableSimpleKey)
					{
						if (Singleton<InputManager>.instance.currentControllerName == "Keyboard")
						{
							if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return))
							{
								PressDown();
							}
						}
						else
						{
							PressDown();
						}
					}
					else
					{
						PressDown();
					}
				}
				else
				{
					if ((!Input.GetButtonUp(buttonName) || IsGetExcludeButtonDown() || onlyHandleController) && (!Singleton<InputManager>.instance.RewiredGetButtonUp(buttonName) || IsGetExcludeButtonDown()))
					{
						break;
					}
					if (disableSimpleKey)
					{
						if (Singleton<InputManager>.instance.currentControllerName == "Keyboard")
						{
							if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.Return))
							{
								PressUp();
							}
						}
						else
						{
							PressUp();
						}
					}
					else
					{
						PressUp();
					}
				}
				break;
			case Type.AxisPositive:
			{
				float num2 = 0f;
				if (!onlyHandleController)
				{
					num2 = Input.GetAxisRaw(axisName);
				}
				if (num2 == 0f)
				{
					num2 = Singleton<InputManager>.instance.RewiredGetAxisRaw(axisName);
				}
				if (Mathf.Abs(num2) < 0.3f && m_IsAxisInUse && IsOnCurrentPanel())
				{
					m_IsAxisInUse = false;
					PressUp();
					m_AixsLongPress = false;
				}
				else if (num2 > 0.7f && !m_IsAxisInUse && IsOnCurrentPanel())
				{
					m_IsAxisInUse = true;
					PressDown();
					m_AixsLongPress = true;
				}
				break;
			}
			case Type.AxisNegative:
			{
				float num = 0f;
				if (!onlyHandleController)
				{
					num = Input.GetAxisRaw(axisName);
				}
				if (num == 0f)
				{
					num = Singleton<InputManager>.instance.RewiredGetAxisRaw(axisName);
				}
				if (Mathf.Abs(num) < 0.3f && m_IsAxisInUse && IsOnCurrentPanel())
				{
					m_IsAxisInUse = false;
					PressUp();
					m_AixsLongPress = false;
				}
				else if (num < -0.7f && !m_IsAxisInUse && IsOnCurrentPanel())
				{
					m_IsAxisInUse = true;
					PressDown();
					m_AixsLongPress = true;
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
			if (!m_AixsLongPress)
			{
				return;
			}
			if (IsReuseScorllRect())
			{
				if (btnType == Type.AxisPositive)
				{
					m_ReuseScorllRect.ScrollToPrevious();
				}
				else
				{
					m_ReuseScorllRect.ScrollToNext();
				}
			}
			if (IsScrollBar())
			{
				switch (btnType)
				{
				case Type.AxisPositive:
					m_Scrollbar.value += axisSpeed;
					break;
				case Type.AxisNegative:
					m_Scrollbar.value -= axisSpeed;
					break;
				}
			}
			if ((bool)m_Slider && m_Slider.interactable)
			{
				switch (btnType)
				{
				case Type.AxisPositive:
					m_Slider.value += 0.002f;
					break;
				case Type.AxisNegative:
					m_Slider.value -= 0.002f;
					break;
				}
			}
		}
	}
}
