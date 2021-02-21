using Assets.Scripts.PeroTools.Commons;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	[RequireComponent(typeof(Toggle))]
	public class ToggleAnimation : MonoBehaviour
	{
		private Toggle m_Toggle;

		private Animator m_Animator;

		private int m_HashOn;

		private int m_HashOff;

		private string m_CurrentAnimName;

		private void Awake()
		{
			m_Toggle = GetComponent<Toggle>();
			m_Toggle.onValueChanged.AddListener(OnValueChange);
			m_Animator = GetComponent<Animator>();
			m_HashOn = Animator.StringToHash("On");
			m_HashOff = Animator.StringToHash("Off");
		}

		private void OnEnable()
		{
			m_Animator.Play((!m_Toggle.isOn) ? m_HashOff : m_HashOn, 0);
		}

		private void OnValueChange(bool state)
		{
			if (m_Animator.GetCurrentAnimName() == m_CurrentAnimName)
			{
				m_Animator.Play((!m_Toggle.isOn) ? m_HashOff : m_HashOn, 0, 1f);
			}
			else
			{
				m_Animator.Play((!m_Toggle.isOn) ? m_HashOff : m_HashOn, 0, 0f);
			}
			m_CurrentAnimName = m_Animator.GetCurrentAnimName();
		}
	}
}
