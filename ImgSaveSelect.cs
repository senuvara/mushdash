using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.UI;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgSaveSelect : UISelectManage
{
	public Text path;

	private List<Tweener> m_Twners;

	private CanvasGroup m_CanvasGroup;

	private GameObject m_BtnCancell;

	public Color m_MaskColor;

	private Vector3 m_OriginPos = Vector3.zero;

	public InputKeyBinding iiusCheckExitBtnSubmit;

	public InputKeyBinding iiusCheckExitBtnCancel;

	private void Awake()
	{
		m_Twners = new List<Tweener>();
		m_CanvasGroup = GetComponent<CanvasGroup>();
		m_OriginPos = base.gameObject.transform.localPosition;
	}

	private void OnEnable()
	{
		path.text = "C:\\Users\\" + Environment.UserName + "\\Pictures";
		iiusCheckExitBtnCancel.enabled = false;
		iiusCheckExitBtnSubmit.enabled = false;
		KillAllTwns();
		base.gameObject.transform.localPosition = m_OriginPos;
		GameObject gameObject = new GameObject("BtnCancel");
		RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
		Transform parent = base.gameObject.transform.parent;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		CanvasScaler canvasScaler = UnityEngine.Object.FindObjectsOfType<CanvasScaler>().Find((CanvasScaler c) => c.GetComponent<Canvas>().worldCamera == Camera.main) ?? UnityEngine.Object.FindObjectsOfType<CanvasScaler>().First();
		if ((bool)canvasScaler)
		{
			rectTransform.sizeDelta = canvasScaler.gameObject.GetComponent<RectTransform>().sizeDelta;
		}
		gameObject.transform.SetParent(parent.transform, false);
		int siblingIndex = base.gameObject.transform.GetSiblingIndex();
		gameObject.transform.SetSiblingIndex(Mathf.Max(siblingIndex, 0));
		Image image = gameObject.AddComponent<Image>();
		image.color = new Color(m_MaskColor.r, m_MaskColor.g, m_MaskColor.b, 0f);
		image.color = m_MaskColor;
		Tweener item = base.gameObject.transform.DOLocalMoveY(-100f, 0.3f).From().SetEase(Ease.OutElastic)
			.SetDelay(0.2f);
		m_Twners.Add(item);
		Tweener item2 = m_CanvasGroup.DOFade(0f, 0.3f).From().SetDelay(0.2f)
			.SetEase(Ease.OutExpo);
		m_Twners.Add(item2);
		Button button = gameObject.AddComponent<Button>();
		button.transition = Selectable.Transition.None;
		m_BtnCancell = gameObject;
		m_Twners.Sort(delegate(Tweener l, Tweener r)
		{
			float num = r.Duration() + r.Delay() - (l.Duration() + l.Delay());
			return (!(num < 0f)) ? 1 : (-1);
		});
	}

	private void OnDisable()
	{
		iiusCheckExitBtnCancel.enabled = true;
		iiusCheckExitBtnSubmit.enabled = true;
		KillAllTwns();
		Tweener item = base.gameObject.transform.DOLocalMoveY(100f, 0.15f).SetEase(Ease.InExpo).SetDelay(0.05f);
		m_Twners.Add(item);
		Tweener item2 = m_CanvasGroup.DOFade(0f, 0.15f).SetDelay(0.05f).SetEase(Ease.Linear);
		m_Twners.Add(item2);
		Image image = (!(m_BtnCancell != null)) ? null : m_BtnCancell.GetComponent<Image>();
		if (image != null)
		{
			Tweener item3 = image.DOFade(0f, 0.2f).SetDelay(0.2f);
			m_Twners.Add(item3);
		}
		m_Twners.Sort(delegate(Tweener l, Tweener r)
		{
			float num = r.Duration() + r.Delay() - (l.Duration() + l.Delay());
			return (!(num < 0f)) ? 1 : (-1);
		});
		m_Twners[0].OnComplete(delegate
		{
			m_Twners.For(delegate(Tweener t)
			{
				t.Kill(true);
			});
			UnityEngine.Object.Destroy(m_BtnCancell);
			m_BtnCancell = null;
			m_CanvasGroup.alpha = 1f;
			base.gameObject.transform.localPosition = m_OriginPos;
		});
	}

	private void KillAllTwns()
	{
		if (m_Twners != null)
		{
			m_Twners.For(delegate(Tweener t)
			{
				t.Kill(true);
			});
			m_Twners.Clear();
		}
	}
}
