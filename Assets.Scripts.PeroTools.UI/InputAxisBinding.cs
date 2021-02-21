using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.UI
{
	public class InputAxisBinding : MonoBehaviour
	{
		public enum Type
		{
			Button,
			Axis
		}

		[ShowInInspector]
		[ReadOnly]
		private PanelType m_PanelOrder;

		public Type type;

		[ShowIf("type", Type.Button, true)]
		public string negative;

		[ShowIf("type", Type.Button, true)]
		public string positive;

		[ShowIf("type", Type.Axis, true)]
		public string axisName;

		public bool loopToggleGroup;

		private List<Toggle> m_AllToggles = new List<Toggle>();

		private ToggleGroup m_ToggleGroup;

		private string m_Guid;

		private bool m_IsAxisInUse;

		private int m_Appen;

		[ShowIf("m_IsHasScrollBar", true)]
		[Tooltip("是否开启惯性")]
		public bool hasInertia;

		[ShowIf("hasInertia", true)]
		public float inertia = 0.05f;

		[ShowIf("hasInertia", true)]
		public float inertiaTime = 0.2f;

		private int m_InertiaVaule;

		private Sequence m_Sequence;

		private UnityGameManager m_UnityGameManager;

		private bool m_IsHasScrollBar()
		{
			return GetComponent<Scrollbar>();
		}

		public void SetPanelOrder(PanelType type)
		{
			m_PanelOrder = type;
		}

		private bool IsOnCurrentPanel()
		{
			return (PanelManage.panel.Peek() & m_PanelOrder) != 0 && (PanelManage.panel.Peek() & m_PanelOrder) != PanelType.Popup;
		}

		private void Awake()
		{
			m_ToggleGroup = GetComponent<ToggleGroup>();
			m_Guid = Guid.NewGuid().ToString();
			m_UnityGameManager = SingletonMonoBehaviour<UnityGameManager>.instance;
			m_UnityGameManager.RegLoop(m_Guid, ListenKeyPress, UnityGameManager.LoopType.Update);
			if (loopToggleGroup && m_ToggleGroup != null)
			{
				m_AllToggles = GameUtils.FindObjectsOfType<Toggle>(m_ToggleGroup.transform);
			}
		}

		private void OnDestroy()
		{
			if ((bool)m_UnityGameManager)
			{
				m_UnityGameManager.UnregLoop(m_Guid);
			}
		}

		private void ListenKeyPress(float arg0)
		{
			switch (type)
			{
			case Type.Button:
				if ((Input.GetButtonDown(positive) && IsOnCurrentPanel()) || (Singleton<InputManager>.instance.RewiredGetButtonDown(positive) && IsOnCurrentPanel()))
				{
					if (m_ToggleGroup != null)
					{
						MoveToggleGroup(m_ToggleGroup, 1);
					}
				}
				else if (((Input.GetButtonDown(negative) && IsOnCurrentPanel()) || (Singleton<InputManager>.instance.RewiredGetButtonDown(negative) && IsOnCurrentPanel())) && m_ToggleGroup != null)
				{
					MoveToggleGroup(m_ToggleGroup, -1);
				}
				break;
			case Type.Axis:
			{
				if (!IsOnCurrentPanel())
				{
					break;
				}
				float axisRaw = Input.GetAxisRaw(axisName);
				if (Mathf.Abs(axisRaw) < 0.2f)
				{
					if (!m_IsAxisInUse)
					{
						break;
					}
					m_IsAxisInUse = false;
					if (hasInertia && m_InertiaVaule > 0)
					{
						Scrollbar bar = GetComponent<Scrollbar>();
						if (bar != null)
						{
							m_Sequence = DOTween.Sequence();
							m_Sequence.Append(DOTween.To(() => bar.value, delegate(float v)
							{
								bar.value = v;
							}, bar.value + (float)m_Appen * inertia * (float)m_InertiaVaule, inertiaTime + 0.02f * (float)m_InertiaVaule));
							m_Sequence.AppendCallback(delegate
							{
								m_Sequence.Kill();
							});
							m_InertiaVaule = 0;
						}
					}
					m_Appen = 0;
					break;
				}
				m_IsAxisInUse = true;
				if (hasInertia)
				{
					m_InertiaVaule++;
					if (m_Sequence != null && m_Sequence.IsActive())
					{
						m_Sequence.Kill();
					}
				}
				m_Appen = ((axisRaw > 0f) ? 1 : (-1));
				if (m_ToggleGroup != null)
				{
					MoveToggleGroup(m_ToggleGroup, m_Appen);
				}
				break;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		public void MoveToggleGroup(ToggleGroup tglGroup, int append)
		{
			if (tglGroup.ActiveToggles().Any())
			{
				int siblingIndex = Enumerable.ToArray(tglGroup.ActiveToggles())[0].transform.GetSiblingIndex();
				int index = Mathf.Clamp(siblingIndex + append, 0, tglGroup.transform.childCount - 1);
				Toggle component = base.transform.GetChild(index).GetComponent<Toggle>();
				if (loopToggleGroup && component == null)
				{
					m_AllToggles[(append < 0) ? (m_AllToggles.Count - 1) : 0].isOn = true;
				}
				if (component != null && component.IsInteractable())
				{
					component.isOn = true;
				}
			}
		}
	}
}
