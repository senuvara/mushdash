using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.UI.Panels;
using DG.Tweening;
using FormulaBase;
using UnityEngine;
using UnityEngine.UI;

public class PauseGradient : MonoBehaviour
{
	public GameObject btnPauseActivity;

	public GameObject btnPauseActivityiPhoneX;

	public GameObject btnTutorialSkipActivity;

	public GameObject btnTutorialSkipActivityiPhoneX;

	private float gradientTime = 0.15f;

	private Image m_BtnImageActivity;

	private Color m_Color;

	private static PauseGradient m_Instance;

	public bool isBtnActivity
	{
		get;
		private set;
	}

	public static PauseGradient instance
	{
		get
		{
			if (!m_Instance)
			{
				m_Instance = GameUtils.FindObjectOfType<PauseGradient>();
			}
			return m_Instance;
		}
	}

	private void Start()
	{
		if (Singleton<StageBattleComponent>.instance.isTutorial)
		{
			if (!PnlBattle.instance.IsIPhoneX())
			{
				m_BtnImageActivity = btnTutorialSkipActivity.GetComponent<Image>();
			}
			else
			{
				m_BtnImageActivity = btnTutorialSkipActivityiPhoneX.GetComponent<Image>();
			}
		}
		else if (!PnlBattle.instance.IsIPhoneX())
		{
			m_BtnImageActivity = btnPauseActivity.GetComponent<Image>();
		}
		else
		{
			m_BtnImageActivity = btnPauseActivityiPhoneX.GetComponent<Image>();
		}
		m_Color = m_BtnImageActivity.color;
	}

	public void ChangeBtnPauseStage(bool isActivity = false)
	{
		isBtnActivity = isActivity;
		if (isActivity)
		{
			Singleton<AudioManager>.instance.PlayOneShot("sfx_switch", Singleton<DataManager>.instance["GameConfig"]["SfxVolume"].GetResult<float>());
			m_BtnImageActivity.DOFade(0.6f, gradientTime);
			m_BtnImageActivity.transform.DOScale(1.4f, gradientTime / 2f).OnComplete(delegate
			{
				m_BtnImageActivity.DOFade(0.8f, gradientTime / 2f);
				m_BtnImageActivity.transform.DOScale(1f, gradientTime / 2f);
			});
		}
		else
		{
			m_BtnImageActivity.DOFade(m_Color.a, gradientTime);
		}
	}
}
