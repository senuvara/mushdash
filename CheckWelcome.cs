using Assets.Scripts.Graphics;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckWelcome : MonoBehaviour
{
	public Image mask;

	public GameObject welcome;

	public VariableBehaviour selectedWelcomeIndex;

	public CanvasGroup pnlMenu;

	public Button btnExit;

	public Button btnCytusBack;

	public Camera camera3d;

	public CanvasScaler uiCanvas;

	private List<Tweener> m_Twners;

	private Vector3 m_OriginPos = Vector3.zero;

	private GameObject m_CurrentWelcome;

	private float m_MatchWidthOrHeight;

	private GameObject[] m_WelcomeGameObjects = new GameObject[9];

	private void Awake()
	{
		m_Twners = new List<Tweener>();
		m_OriginPos = base.gameObject.transform.localPosition;
		btnExit.onClick.AddListener(DisableWelcome);
		btnCytusBack.onClick.AddListener(DisableWelcome);
	}

	private void OnEnable()
	{
		SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
		{
			if (base.gameObject.activeInHierarchy)
			{
				if (!pnlMenu.gameObject.activeInHierarchy)
				{
					base.gameObject.SetActive(false);
				}
				else
				{
					InstanceWelcome();
				}
			}
		}, 0.167f);
	}

	private void InstanceWelcome()
	{
		KillAllTwns();
		base.gameObject.transform.localPosition = m_OriginPos;
		Tweener item = mask.DOFade(1f, 0.2f).SetEase(Ease.InCubic).OnComplete(delegate
		{
			pnlMenu.alpha = 0f;
			camera3d.GetComponent<GradientFogOptmization>().enabled = true;
			m_MatchWidthOrHeight = uiCanvas.matchWidthOrHeight;
			if (selectedWelcomeIndex.variable.GetResult<int>() == 1 || selectedWelcomeIndex.variable.GetResult<int>() == 8 || (double)(1f * (float)GraphicSettings.curScreenWidth / (float)GraphicSettings.curScreenHeight - 0.01f) < 1.7778)
			{
				uiCanvas.matchWidthOrHeight = 0f;
			}
			int result = selectedWelcomeIndex.variable.GetResult<int>();
			if (m_WelcomeGameObjects[result] == null)
			{
				m_WelcomeGameObjects[result] = Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>($"Welcome_0{result + 1}_Trove"), welcome.transform);
			}
			else
			{
				m_WelcomeGameObjects[result].SetActive(true);
			}
			mask.DOFade(0f, 0.2f).SetEase(Ease.OutCubic);
		});
		m_Twners.Add(item);
		m_Twners.Sort(delegate(Tweener l, Tweener r)
		{
			float num = r.Duration() + r.Delay() - (l.Duration() + l.Delay());
			return (!(num < 0f)) ? 1 : (-1);
		});
	}

	private void OnDisable()
	{
		KillAllTwns();
		Tweener item = mask.DOFade(1f, 0.1f).SetEase(Ease.InCubic);
		m_Twners.Add(item);
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
			base.gameObject.transform.localPosition = m_OriginPos;
		});
		pnlMenu.alpha = 1f;
		if ((bool)camera3d && (bool)camera3d.GetComponent<GradientFogOptmization>())
		{
			camera3d.GetComponent<GradientFogOptmization>().enabled = false;
		}
		uiCanvas.matchWidthOrHeight = m_MatchWidthOrHeight;
		Singleton<AudioManager>.instance.PlayBGM("TroveBgm-InARomanticMood-Lukyanov");
		DisableAllWelcome();
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

	private void DisableAllWelcome()
	{
		for (int i = 0; i < welcome.transform.childCount; i++)
		{
			Object.Destroy(welcome.transform.GetChild(i).gameObject);
		}
	}

	private void DisableWelcome()
	{
		mask.DOFade(1f, 0.1f).SetEase(Ease.InCubic).OnComplete(delegate
		{
			base.gameObject.SetActive(false);
		});
	}
}
