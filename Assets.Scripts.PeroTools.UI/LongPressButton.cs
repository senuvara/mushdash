using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.UI
{
	[RequireComponent(typeof(Button))]
	public class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
	{
		public bool longPressIncludePointerDown = true;

		public float delta = 0.75f;

		public float triggerDelta;

		public Action onPressDown;

		public Action<bool> onPress;

		public Action onPressUp;

		private Button m_Button;

		private bool m_Flag;

		private float m_PressStartTime;

		private float m_LastPressTime;

		public void OnPointerDown(PointerEventData eventData)
		{
			if (IsActive() && IsInteractable())
			{
				m_PressStartTime = Time.unscaledTime;
				m_LastPressTime = Time.unscaledTime;
				m_Flag = true;
				if (onPressDown != null)
				{
					onPressDown();
				}
				if (longPressIncludePointerDown && onPress != null)
				{
					onPress(true);
				}
			}
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			if (IsActive() && IsInteractable())
			{
				m_Flag = false;
				if (onPressUp != null)
				{
					onPressUp();
				}
			}
		}

		public bool IsActive()
		{
			return m_Button.IsActive();
		}

		public bool IsInteractable()
		{
			return m_Button.IsInteractable();
		}

		private void Update()
		{
			if (IsInteractable() && onPress != null && m_Flag && Time.unscaledTime - m_LastPressTime >= triggerDelta)
			{
				if (Time.unscaledTime - m_PressStartTime > delta)
				{
					onPress(true);
				}
				else
				{
					onPress(false);
				}
				m_LastPressTime = Time.unscaledTime;
			}
		}

		private void OnEnable()
		{
			m_Flag = false;
		}

		private void Awake()
		{
			m_Button = GetComponent<Button>();
		}
	}
}
