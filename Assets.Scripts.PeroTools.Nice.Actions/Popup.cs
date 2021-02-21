using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class Popup : Action
	{
		public static readonly Stack<PlayPopuper> popups = new Stack<PlayPopuper>();

		[SerializeField]
		private GameObject m_Panel;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private float m_InTime = 0.3f;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private float m_InDistance = -100f;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private Ease m_MoveInEase = Ease.OutElastic;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private bool m_IsFadeIn = true;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private Ease m_FadeInEase = Ease.OutExpo;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private float m_OutTime = 0.15f;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private float m_OutDistance = 100f;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private Ease m_MoveOutEase = Ease.InExpo;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private bool m_IsFadeOut = true;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private Ease m_FadeOutEase = Ease.Linear;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private Color m_MaskColor = new Color(0f, 0f, 0f, 0.7f);

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private bool m_Shut = true;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private bool m_ShutAll;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private PopupType m_Type;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private List<Button> m_ShutButtons = new List<Button>();

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private List<Button> m_ShutAllShutButtons = new List<Button>();

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		private AudioClip m_ShutClip;

		[SerializeField]
		[Variable(1f, "OnVolumeDraw", false)]
		[ShowIf("m_IsPro", true)]
		private IVariable m_Volume;

		private Vector3 m_OriginPos = Vector3.zero;

		private bool m_Flag;

		private GameObject m_BtnCancell;

		private List<Tweener> m_Twners;

		private CanvasGroup m_CanvasGroup;

		private Tweener m_CurrentObjTweener;

		private Tweener m_CurrentObjMoveTweener;

		private Tweener m_CurrentImgTweener;

		public bool isClickShut
		{
			get;
			private set;
		}

		public override void Enter()
		{
			isClickShut = false;
			m_Flag = false;
			m_Twners = new List<Tweener>();
		}

		public override void Execute()
		{
			isClickShut = false;
			if (popups.Count != 0)
			{
				PlayPopuper playPopuper = popups.Peek();
				if (m_Type == PopupType.Stack)
				{
					playPopuper.Exit(Push);
				}
				else
				{
					if (m_Type != 0)
					{
						return;
					}
					if (playPopuper.popup.isClickShut)
					{
						popups.Pop().Exit(Push);
						return;
					}
					List<PlayPopuper> list = popups.ToList();
					list.Add(new PlayPopuper(this));
					popups.Clear();
					for (int num = list.Count - 1; num >= 0; num--)
					{
						popups.Push(list[num]);
					}
				}
			}
			else
			{
				Push();
			}
		}

		public override void Exit()
		{
			popups.Remove((PlayPopuper p) => p.popup == this);
		}

		public void Play()
		{
			SetShutButtonEnable(false);
			KillAllTwns();
			GameObject panel = m_Panel;
			if (!m_CanvasGroup)
			{
				m_CanvasGroup = panel.GetOrAddComponent<CanvasGroup>();
			}
			panel.SetActive(true);
			if (!m_Flag)
			{
				m_Flag = true;
				m_OriginPos = panel.transform.localPosition;
			}
			panel.transform.localPosition = m_OriginPos;
			GameObject gameObject = new GameObject("BtnCancel");
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			Transform parent = panel.transform.parent;
			gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
			CanvasScaler canvasScaler = UnityEngine.Object.FindObjectsOfType<CanvasScaler>().Find((CanvasScaler c) => c.GetComponent<Canvas>().worldCamera == Camera.main) ?? UnityEngine.Object.FindObjectsOfType<CanvasScaler>().First();
			if ((bool)canvasScaler)
			{
				rectTransform.sizeDelta = canvasScaler.gameObject.GetComponent<RectTransform>().sizeDelta;
			}
			gameObject.transform.SetParent(parent.transform, false);
			int siblingIndex = panel.transform.GetSiblingIndex();
			gameObject.transform.SetSiblingIndex(Mathf.Max(siblingIndex, 0));
			float num = 0.2f;
			Image image = gameObject.AddComponent<Image>();
			image.color = new Color(m_MaskColor.r, m_MaskColor.g, m_MaskColor.b, 0f);
			if (popups.Count == 0)
			{
				m_CurrentImgTweener = image.DOFade(m_MaskColor.a, num);
				CheckImgTweenPause();
			}
			else
			{
				image.color = m_MaskColor;
			}
			m_CurrentObjMoveTweener = panel.transform.DOLocalMoveY(m_InDistance, m_InTime).From().SetEase(m_MoveInEase)
				.SetDelay(num)
				.OnComplete(delegate
				{
					SetShutButtonEnable(true);
				});
			m_Twners.Add(m_CurrentObjMoveTweener);
			if (m_IsFadeIn)
			{
				Tweener item = m_CurrentObjTweener = m_CanvasGroup.DOFade(0f, m_InTime).From().SetDelay(num)
					.SetEase(m_FadeInEase);
				CheckObjTweenPause();
				m_Twners.Add(item);
			}
			Button btnCancell = gameObject.AddComponent<Button>();
			btnCancell.transition = Selectable.Transition.None;
			m_BtnCancell = gameObject;
			if (m_Shut && (bool)btnCancell)
			{
				btnCancell.onClick.AddListener(OnBgShutButtonClick);
				btnCancell.interactable = false;
			}
			m_ShutButtons.For(delegate(Button b)
			{
				b.onClick.RemoveListener(OnShutButtonsClick);
				b.onClick.AddListener(OnShutButtonsClick);
			});
			if (m_ShutAllShutButtons != null)
			{
				m_ShutAllShutButtons.For(delegate(Button b)
				{
					b.onClick.RemoveListener(OnShutAllShutButtonsClick);
					b.onClick.AddListener(OnShutAllShutButtonsClick);
				});
			}
			m_Twners.Sort(delegate(Tweener l, Tweener r)
			{
				float num2 = r.Duration() + r.Delay() - (l.Duration() + l.Delay());
				return (!(num2 < 0f)) ? 1 : (-1);
			});
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				if ((bool)btnCancell)
				{
					btnCancell.interactable = true;
				}
			}, 0.5f);
		}

		private void SetShutButtonEnable(bool enable)
		{
			m_ShutButtons.For(delegate(Button b)
			{
				b.enabled = enable;
			});
		}

		public void OnShutAllShutButtonsClick()
		{
			OnShutButtonClick(true);
		}

		public void OnShutButtonsClick()
		{
			OnShutButtonClick();
		}

		public void OnBgShutButtonClick()
		{
			OnShutButtonClick(m_ShutAll);
		}

		public void OnShutButtonClick(bool isShutAll = false)
		{
			if (isClickShut)
			{
				return;
			}
			isClickShut = true;
			if ((bool)m_BtnCancell)
			{
				Button component = m_BtnCancell.GetComponent<Button>();
				if ((bool)component)
				{
					component.enabled = false;
				}
			}
			if ((bool)m_ShutClip)
			{
				Singleton<AudioManager>.instance.PlayOneShot(m_ShutClip, (m_Volume == null) ? 1f : m_Volume.GetResult<float>());
			}
			if (popups.Count == 0)
			{
				return;
			}
			PlayPopuper playPopuper = popups.Pop();
			PlayPopuper peek = (popups.Count == 0) ? null : popups.Peek();
			playPopuper.Exit(delegate
			{
				if (isShutAll)
				{
					popups.Clear();
				}
				else if (peek != null)
				{
					peek.popup.Play();
				}
			});
		}

		public void Leave(System.Action callback)
		{
			SetShutButtonEnable(false);
			KillAllTwns();
			GameObject gameObject = m_Panel;
			Image image = (!(m_BtnCancell != null)) ? null : m_BtnCancell.GetComponent<Image>();
			float num = 0.05f;
			Tweener item = gameObject.transform.DOLocalMoveY(m_OutDistance, m_OutTime).SetEase(m_MoveOutEase).SetDelay(num);
			m_Twners.Add(item);
			if (m_IsFadeOut)
			{
				Tweener item2 = m_CanvasGroup.DOFade(0f, m_OutTime).SetDelay(num).SetEase(m_FadeOutEase);
				m_Twners.Add(item2);
			}
			System.Action finishCallback = delegate
			{
				UnityEngine.Object.Destroy(m_BtnCancell);
				m_BtnCancell = null;
				m_CanvasGroup.alpha = 1f;
				gameObject.SetActive(false);
				gameObject.transform.localPosition = m_OriginPos;
				if (callback != null)
				{
					callback();
				}
			};
			if (popups.Count == 0 && image != null)
			{
				Tweener item3 = image.DOFade(0f, 0.2f).SetDelay(m_OutTime + num);
				m_Twners.Add(item3);
			}
			m_Twners.Sort(delegate(Tweener l, Tweener r)
			{
				float num2 = r.Duration() + r.Delay() - (l.Duration() + l.Delay());
				return (!(num2 < 0f)) ? 1 : (-1);
			});
			m_Twners[0].OnComplete(delegate
			{
				m_Twners.For(delegate(Tweener t)
				{
					t.Kill(true);
				});
				finishCallback();
			});
		}

		private void Push()
		{
			Play();
			popups.Push(new PlayPopuper(this));
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

		private void CheckObjTweenPause()
		{
			SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop(ObjLoop, UnityGameManager.LoopType.Update, 2f);
		}

		private void CheckImgTweenPause()
		{
			SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop(ImgLoop, UnityGameManager.LoopType.Update, 2f);
		}

		private void ObjLoop(float time)
		{
			if (m_CurrentObjTweener.IsActive() && !m_CurrentObjTweener.IsPlaying())
			{
				m_CurrentObjTweener.Play();
			}
			if (m_CurrentObjMoveTweener.IsActive() && !m_CurrentObjMoveTweener.IsPlaying())
			{
				m_CurrentObjMoveTweener.Play();
			}
		}

		private void ImgLoop(float time)
		{
			if (m_CurrentImgTweener.IsActive() && !m_CurrentImgTweener.IsPlaying())
			{
				m_CurrentImgTweener.Play();
			}
		}
	}
}
