using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Rewired;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StageLikeToggle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	[HideInInspector]
	public bool m_IsInHideTag;

	[HideInInspector]
	public bool m_IsInRandom;

	private bool m_IsPointDown;

	private float m_LongHoldTimer;

	public Image holdImg;

	[HideInInspector]
	public bool m_IsNoMusic;

	[HideInInspector]
	public bool isUnKnow;

	private Toggle m_Toggle;

	private void Awake()
	{
		m_Toggle = GetComponent<Toggle>();
	}

	public void OnPointerDown(PointerEventData data)
	{
		m_IsPointDown = true;
		if (!isUnKnow && !m_IsInRandom)
		{
			m_Toggle.interactable = true;
		}
	}

	public void OnPointerUp(PointerEventData data)
	{
		m_IsPointDown = false;
	}

	private void Update()
	{
		if (m_IsInHideTag || m_IsInRandom || m_IsNoMusic || isUnKnow || m_Toggle.isOn)
		{
			return;
		}
		if (Input.GetButtonDown("Space") || ReInput.players.GetPlayer(0).GetButtonDown("Space"))
		{
			m_IsPointDown = true;
			if (!isUnKnow && !m_IsInRandom)
			{
				m_Toggle.interactable = true;
			}
		}
		if (Input.GetButtonUp("Space") || ReInput.players.GetPlayer(0).GetButtonUp("Space"))
		{
			m_IsPointDown = false;
		}
		if (m_IsPointDown)
		{
			m_LongHoldTimer += Time.deltaTime;
			if (m_LongHoldTimer > 0.1f)
			{
				holdImg.fillAmount = m_LongHoldTimer;
			}
			if (m_LongHoldTimer > 0.2f)
			{
				m_Toggle.interactable = false;
			}
			if (m_LongHoldTimer >= 1f)
			{
				m_IsPointDown = false;
				m_LongHoldTimer = 0f;
				Singleton<EventManager>.instance.Invoke("UI/OnAskHide");
				Singleton<EventManager>.instance.Invoke("UI/OnClickHide");
			}
		}
		else
		{
			m_LongHoldTimer = 0f;
			if (holdImg.fillAmount != 0f)
			{
				holdImg.fillAmount = 0f;
			}
		}
	}

	public void ResetHoldImg()
	{
		holdImg.fillAmount = 0f;
	}
}
