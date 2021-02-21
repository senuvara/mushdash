using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChangeHealthValue : MonoBehaviour
{
	private Text m_Text;

	private EventManager m_EventManager;

	private Slider m_Slider;

	private void Awake()
	{
		Singleton<EventManager>.instance.RegEvent("Battle/OnHpRateChanged").trigger += OnHpRateChange;
		Singleton<EventManager>.instance.RegEvent("Battle/OnHpDeduct").trigger += OnHpDeduct;
		Singleton<EventManager>.instance.RegEvent("Battle/OnHpAdd").trigger += OnHpAdd;
		Singleton<EventManager>.instance.RegEvent("Battle/OnFail").trigger += OnFail;
		m_Text = GetComponent<Text>();
		m_Slider = GetComponent<Slider>();
		m_EventManager = Singleton<EventManager>.instance;
	}

	private void OnDestroy()
	{
		if (m_EventManager != null)
		{
			m_EventManager.RegEvent("Battle/OnHpRateChanged").trigger -= OnHpRateChange;
			Singleton<EventManager>.instance.RegEvent("Battle/OnHpDeduct").trigger -= OnHpDeduct;
			Singleton<EventManager>.instance.RegEvent("Battle/OnHpAdd").trigger -= OnHpAdd;
			Singleton<EventManager>.instance.RegEvent("Battle/OnFail").trigger -= OnFail;
		}
	}

	private void OnHpRateChange(object sender, object reciever, object[] args)
	{
		if ((bool)m_Text)
		{
			m_Text.text = Mathf.RoundToInt(BattleRoleAttributeComponent.instance.GetHp()) + "/" + BattleRoleAttributeComponent.instance.GetHpMax();
		}
	}

	private void OnHpDeduct(object sender, object reciever, object[] args)
	{
		if ((bool)m_Slider)
		{
			m_Slider.value = BattleRoleAttributeComponent.instance.HpRate();
		}
	}

	private void OnHpAdd(object sender, object reciever, object[] args)
	{
		if ((bool)m_Slider)
		{
			DOTween.To(() => m_Slider.value, delegate(float x)
			{
				m_Slider.value = x;
			}, BattleRoleAttributeComponent.instance.HpRate(), 0.3f);
		}
	}

	private void OnFail(object sender, object reciever, object[] args)
	{
		if ((bool)m_Slider)
		{
			DOTween.To(() => m_Slider.value, delegate(float x)
			{
				m_Slider.value = x;
			}, 0f, 0.3f);
		}
	}
}
