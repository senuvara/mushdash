using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;
using UnityEngine.UI;

public class ChangeFeverValue : MonoBehaviour
{
	private Text m_Text;

	private EventManager m_EventManager;

	private Slider m_Slider;

	public Animator feverAnim;

	private void Awake()
	{
		Singleton<EventManager>.instance.RegEvent("Battle/OnFeverRateChanged").trigger += OnFeverRateChange;
		Singleton<EventManager>.instance.RegEvent("Battle/OnFever").trigger += OnFever;
		Singleton<EventManager>.instance.RegEvent("Battle/OnFeverMax").trigger += OnFeverMax;
		Singleton<EventManager>.instance.RegEvent("Battle/OnFail").trigger += OnFeverFail;
		m_Text = GetComponent<Text>();
		m_Slider = GetComponent<Slider>();
		m_EventManager = Singleton<EventManager>.instance;
	}

	private void OnDestroy()
	{
		if (m_EventManager != null)
		{
			m_EventManager.RegEvent("Battle/OnFeverRateChanged").trigger -= OnFeverRateChange;
			Singleton<EventManager>.instance.RegEvent("Battle/OnFever").trigger -= OnFever;
			Singleton<EventManager>.instance.RegEvent("Battle/OnFeverMax").trigger -= OnFeverMax;
			Singleton<EventManager>.instance.RegEvent("Battle/OnFail").trigger -= OnFeverFail;
		}
	}

	private void OnFeverRateChange(object sender, object reciever, object[] args)
	{
		if ((bool)m_Text)
		{
			m_Text.text = Mathf.RoundToInt(FeverManager.Instance.GetWholeFever()) + "/" + Singleton<BattleProperty>.instance.maxFever;
		}
		if ((bool)m_Slider)
		{
			m_Slider.value = FeverManager.Instance.GetFeverRate();
		}
	}

	private void OnFever(object sender, object reciever, object[] args)
	{
		if (!Singleton<DataManager>.instance["Account"]["IsAutoFever"].GetResult<bool>() && feverAnim != null)
		{
			feverAnim.Play("FeverTipsEnd", 0);
		}
	}

	private void OnFeverMax(object sender, object reciever, object[] args)
	{
		if (!Singleton<DataManager>.instance["Account"]["IsAutoFever"].GetResult<bool>() && feverAnim != null)
		{
			feverAnim.Play("FeverTipsStart", 0);
		}
	}

	private void OnFeverFail(object sender, object reciever, object[] args)
	{
		if (!Singleton<DataManager>.instance["Account"]["IsAutoFever"].GetResult<bool>() && feverAnim != null)
		{
			feverAnim.Play("FeverTipsFailed", 0);
		}
	}
}
