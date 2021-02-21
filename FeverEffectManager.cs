using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using UnityEngine;

public class FeverEffectManager : MonoBehaviour
{
	private GameObject backGround;

	private Animator m_BkgAnimator;

	private SpriteRenderer whitBoardRender;

	private Vector3 outScenePosition = new Vector3(14.08f, 0.96f, 0f);

	private GameObject[] particles = new GameObject[7];

	private const float COME_OUT_DURING_TIME = 0.3f;

	private bool isActivatedComeOut;

	private bool ifShow;

	private UnityGameManager m_UnityGameManager;

	private static FeverEffectManager m_Instance;

	public static FeverEffectManager instance => m_Instance;

	private void Start()
	{
		InitFeverEffect();
		m_Instance = this;
		base.gameObject.SetActive(false);
		m_BkgAnimator = backGround.GetComponent<Animator>();
		m_BkgAnimator.Rebind();
		m_UnityGameManager = SingletonMonoBehaviour<UnityGameManager>.instance;
		m_UnityGameManager.RegLoop("FeverEffectManager", delegate
		{
			if (isActivatedComeOut)
			{
				if (ifShow)
				{
					whitBoardRender.enabled = true;
					float num = 1f * (Time.deltaTime / 0.15f);
					Color color = whitBoardRender.color;
					if (color.a + num <= 1f)
					{
						SpriteRenderer spriteRenderer = whitBoardRender;
						Color color2 = whitBoardRender.color;
						spriteRenderer.color = new Color(1f, 1f, 1f, color2.a + num);
					}
					else
					{
						whitBoardRender.color = new Color(1f, 1f, 1f, 1f);
						backGround.transform.position = outScenePosition;
						backGround.SetActive(false);
						GameObject[] array = particles;
						foreach (GameObject gameObject in array)
						{
							if (gameObject != null)
							{
								gameObject.SetActive(false);
							}
						}
						ifShow = false;
					}
				}
				else
				{
					float num2 = 1f * (Time.deltaTime / 0.15f);
					Color color3 = whitBoardRender.color;
					if (color3.a - num2 >= 0f)
					{
						SpriteRenderer spriteRenderer2 = whitBoardRender;
						Color color4 = whitBoardRender.color;
						spriteRenderer2.color = new Color(1f, 1f, 1f, color4.a - num2);
					}
					else
					{
						whitBoardRender.color = new Color(1f, 1f, 1f, 0f);
						whitBoardRender.enabled = false;
						isActivatedComeOut = false;
						m_BkgAnimator.gameObject.SetActive(true);
						m_BkgAnimator.Rebind();
						m_BkgAnimator.Play("waiting_outside");
						ifShow = true;
						base.gameObject.SetActive(false);
						GameObject gameObject2 = GameObject.Find("ClossShot_layer");
						if (gameObject2 != null)
						{
							SpriteRenderer[] componentsInChildren = gameObject2.GetComponentsInChildren<SpriteRenderer>();
							SpriteRenderer[] array2 = componentsInChildren;
							foreach (SpriteRenderer spriteRenderer3 in array2)
							{
								spriteRenderer3.sortingOrder = 6;
							}
						}
					}
				}
			}
		}, UnityGameManager.LoopType.Update);
	}

	private void OnDestroy()
	{
		if (m_UnityGameManager != null)
		{
			m_UnityGameManager.UnregLoop("FeverEffectManager");
		}
	}

	public void InitFeverEffect()
	{
		isActivatedComeOut = false;
		ifShow = true;
		backGround = GameObject.Find("bg_S");
		backGround.transform.position = outScenePosition;
		whitBoardRender = GameObject.Find("whitBoard").GetComponent<SpriteRenderer>();
		whitBoardRender.color = new Color(1f, 1f, 1f, 0f);
		whitBoardRender.enabled = false;
		particles[0] = GameObject.Find("fever_fx_1_start");
		particles[1] = GameObject.Find("fever_fx_1");
		particles[2] = GameObject.Find("fever_streamer");
		particles[3] = GameObject.Find("fever_fx_star_big2");
		particles[4] = GameObject.Find("fever_fx_star_big1");
		particles[5] = GameObject.Find("fever_fx_star_small1");
		particles[6] = GameObject.Find("fever_fx_star_small2");
		GameObject[] array = particles;
		foreach (GameObject gameObject in array)
		{
			if (gameObject != null)
			{
				gameObject.SetActive(false);
				gameObject.transform.SetParent(base.transform);
			}
		}
	}

	public void ActivateFever()
	{
		if (base.gameObject.activeSelf)
		{
			return;
		}
		base.gameObject.SetActive(true);
		backGround.SetActive(true);
		Animator component = backGround.GetComponent<Animator>();
		component.Rebind();
		component.Play("come_in");
		isActivatedComeOut = false;
		ifShow = true;
		GameObject gameObject = GameObject.Find("ClossShot_layer");
		if (gameObject != null)
		{
			SpriteRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<SpriteRenderer>();
			Debug.Log("renders number is " + componentsInChildren.Length);
			SpriteRenderer[] array = componentsInChildren;
			foreach (SpriteRenderer spriteRenderer in array)
			{
				spriteRenderer.sortingOrder = -8;
			}
		}
		GameObject[] array2 = particles;
		foreach (GameObject gameObject2 in array2)
		{
			if (gameObject2 != null)
			{
				gameObject2.SetActive(true);
			}
		}
	}

	public void CancelFeverEffect()
	{
		isActivatedComeOut = true;
	}
}
