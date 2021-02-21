using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class QAButton : MonoBehaviour
{
	public RectTransform answer;

	public Button button;

	public RectTransform imgSelect;

	private float m_AnswerSapce = 85f;

	public Color highlight;

	public Color normal;

	public AudioClip onClick;

	private RectTransform m_Content;

	private RectTransform m_Rect;

	private Vector2 m_ContentSize = Vector2.zero;

	private bool m_Show;

	private CanvasGroup m_CanvasGroup;

	private Text m_Title;

	private bool m_InAnim;

	public bool inAnim()
	{
		return m_InAnim;
	}

	private void Awake()
	{
		m_Rect = GetComponent<RectTransform>();
		m_CanvasGroup = answer.GetComponent<CanvasGroup>();
		m_CanvasGroup.alpha = 0f;
		m_Title = button.GetComponent<Text>();
		m_Title.color = normal;
		m_Title.resizeTextForBestFit = false;
	}

	private void OnEnable()
	{
		if (answer.childCount > 0)
		{
			for (int i = 0; i < answer.childCount; i++)
			{
				GameObject gameObject = answer.GetChild(i).gameObject;
				if (gameObject.activeInHierarchy)
				{
					m_Content = gameObject.GetComponent<RectTransform>();
					Debug.Log(gameObject.name);
					break;
				}
			}
		}
		else
		{
			m_Content = answer.GetComponent<RectTransform>();
		}
		SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
		{
			m_Rect.sizeDelta = button.GetComponent<RectTransform>().sizeDelta;
			Vector2 sizeDelta = m_Rect.sizeDelta;
			m_AnswerSapce = sizeDelta.y + 20f;
		}, 1);
	}

	private void OnDisable()
	{
		if (m_Show)
		{
			OnClick(false);
		}
	}

	public void OnClick(bool playSound = true)
	{
		if (!m_InAnim)
		{
			m_InAnim = true;
			Invoke("FinishAnim", 0.2f);
			m_Show = !m_Show;
			if (playSound)
			{
				Singleton<AudioManager>.instance.PlayOneShot(onClick, Singleton<DataManager>.instance["GameConfig"]["SfxVolume"].GetResult<float>());
			}
			ref Vector2 contentSize = ref m_ContentSize;
			Vector2 sizeDelta = m_Content.sizeDelta;
			contentSize.y = sizeDelta.y + 35f;
			DOTween.To(() => m_Rect.sizeDelta, delegate(Vector2 a)
			{
				m_Rect.sizeDelta = a;
			}, m_Rect.sizeDelta + (m_Show ? 1 : (-1)) * m_ContentSize, 0.2f);
			DOTween.To(() => m_Content.anchoredPosition, delegate(Vector2 b)
			{
				m_Content.anchoredPosition = b;
			}, m_Content.anchoredPosition + ((!m_Show) ? 1 : (-1)) * new Vector2(0f, m_AnswerSapce), 0.2f);
			DOTween.To(() => m_CanvasGroup.alpha, delegate(float c)
			{
				m_CanvasGroup.alpha = c;
			}, (!m_Show) ? 0f : 1f, 0.1f);
		}
	}

	public void SetSelect(bool enable)
	{
		DOTween.To(() => m_Title.color, delegate(Color a)
		{
			m_Title.color = a;
		}, (!enable) ? normal : highlight, 0.2f);
		imgSelect.gameObject.SetActive(enable);
	}

	public void FinishAnim()
	{
		m_InAnim = false;
	}
}
