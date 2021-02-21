using Assets.Scripts.Graphics;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Components;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GraphicSettingSelect : UISelectManage
{
	public Transform windowToggles;

	public Transform fxToggles;

	public FancyScrollView dpiFullScreen;

	public FancyScrollView dpiWindow;

	private FancyScrollView screenMode;

	public FancyScrollView screenMode_Win;

	public FancyScrollView screenMode_Mac;

	public FancyScrollView fpsFsv;

	public GameObject confirm;

	public GameObject enterBackgroundBtn;

	public Toggle advancedOn;

	public Toggle standardOn;

	private List<Toggle> m_FxToggles = new List<Toggle>();

	public Color highlightColor;

	public Color normalColor;

	public GameObject enterBackgroundSelectImg;

	private int m_DpiFullScreenIndex = 6;

	private int m_DpiWindowIndex = 3;

	private int m_ScreenModeIndex;

	private bool m_IsFullScreen;

	private bool m_HasBorder;

	private bool m_IsInit;

	private Vector2Int[] m_FullScreenResolutions = new Vector2Int[8]
	{
		new Vector2Int(1280, 720),
		new Vector2Int(1280, 800),
		new Vector2Int(1360, 768),
		new Vector2Int(1366, 768),
		new Vector2Int(1440, 900),
		new Vector2Int(1600, 900),
		new Vector2Int(1920, 1080),
		new Vector2Int(3840, 2160)
	};

	private Vector2Int[] m_WindowsResolutions = new Vector2Int[4]
	{
		new Vector2Int(1280, 720),
		new Vector2Int(1360, 768),
		new Vector2Int(1600, 900),
		new Vector2Int(1920, 1080)
	};

	private int[] m_Fps = new int[4]
	{
		60,
		120,
		240,
		-1
	};

	public override void OnInit()
	{
		m_FxToggles = GameUtils.FindObjectsOfType<Toggle>(fxToggles);
		for (int i = 0; i < m_FxToggles.Count; i++)
		{
			m_FxToggles[i].graphic.color = normalColor;
		}
		advancedOn.graphic.color = normalColor;
		standardOn.graphic.color = normalColor;
		m_IsFullScreen = Singleton<DataManager>.instance["GameConfig"]["FullScreen"].GetResult<bool>();
		m_HasBorder = Singleton<DataManager>.instance["GameConfig"]["HasBorder"].GetResult<bool>();
		string result = Singleton<DataManager>.instance["GameConfig"]["ScreenResolutions"].GetResult<string>();
		int resolutionsIndex = GetResolutionsIndex(result);
		dpiFullScreen.Rebuild();
		dpiWindow.Rebuild();
		screenMode = screenMode_Win;
		if (screenMode_Mac != null)
		{
			screenMode_Mac.gameObject.SetActive(false);
		}
		screenMode.Rebuild();
		if (resolutionsIndex >= 0)
		{
			if (!m_HasBorder)
			{
				m_ScreenModeIndex = 2;
				screenMode.ScrollToDataIndex(m_ScreenModeIndex, 0f);
			}
			else if (m_IsFullScreen)
			{
				m_ScreenModeIndex = 0;
				screenMode.ScrollToDataIndex(m_ScreenModeIndex, 0f);
			}
			else
			{
				m_ScreenModeIndex = 1;
				screenMode.ScrollToDataIndex(m_ScreenModeIndex, 0f);
			}
			if (m_IsFullScreen)
			{
				m_DpiFullScreenIndex = resolutionsIndex;
				dpiFullScreen.ScrollToDataIndex(m_DpiFullScreenIndex, 0f);
			}
			else
			{
				m_DpiWindowIndex = resolutionsIndex;
				dpiWindow.ScrollToDataIndex(m_DpiWindowIndex, 0f);
			}
		}
		fpsFsv.Rebuild();
		fpsFsv.ScrollToDataIndex(GetFpsIndex(), 0f);
		dpiFullScreen.transform.parent.gameObject.SetActive(m_IsFullScreen);
		dpiWindow.transform.parent.gameObject.SetActive(!m_IsFullScreen);
		EventSystem.current.SetSelectedGameObject(((!m_IsFullScreen) ? dpiWindow : dpiFullScreen).btnPrevious.gameObject);
		AdjustNavi((!m_IsFullScreen) ? dpiWindow : dpiFullScreen);
		SetHighlight((!m_IsFullScreen) ? dpiWindow : dpiFullScreen, true);
		dpiFullScreen.onFinalItemIndexChange += OnFullScreenDpiChange;
		dpiWindow.onFinalItemIndexChange += OnWindowDpiChange;
		screenMode.onFinalItemIndexChange += OnScreenModeChange;
		fpsFsv.onFinalItemIndexChange += OnFpsChange;
		dpiFullScreen.btnNext.onClick.AddListener(delegate
		{
			EventSystem.current.SetSelectedGameObject(dpiFullScreen.btnPrevious.gameObject);
		});
		dpiWindow.btnNext.onClick.AddListener(delegate
		{
			EventSystem.current.SetSelectedGameObject(dpiWindow.btnPrevious.gameObject);
		});
		screenMode.btnNext.onClick.AddListener(delegate
		{
			EventSystem.current.SetSelectedGameObject(screenMode.btnPrevious.gameObject);
		});
		fpsFsv.btnNext.onClick.AddListener(delegate
		{
			EventSystem.current.SetSelectedGameObject(fpsFsv.btnPrevious.gameObject);
		});
		confirm.GetComponent<Button>().onClick.AddListener(OnClickConfirm);
		m_IsInit = true;
	}

	private void OnFullScreenDpiChange(int i)
	{
		confirm.SetActive(m_DpiFullScreenIndex != i);
		SetConfirmHighlight(m_DpiFullScreenIndex != i);
	}

	private void OnWindowDpiChange(int i)
	{
		confirm.SetActive(m_DpiWindowIndex != i);
		SetConfirmHighlight(m_DpiWindowIndex != i);
	}

	private void OnScreenModeChange(int i)
	{
		if (m_ScreenModeIndex == i)
		{
			return;
		}
		Vector2Int resolution = (!m_IsFullScreen) ? m_WindowsResolutions[m_DpiWindowIndex] : m_FullScreenResolutions[m_DpiFullScreenIndex];
		switch (i)
		{
		case 0:
		{
			m_IsFullScreen = true;
			dpiFullScreen.transform.parent.gameObject.SetActive(m_IsFullScreen);
			dpiWindow.transform.parent.gameObject.SetActive(!m_IsFullScreen);
			m_HasBorder = true;
			Vector2Int value2 = m_WindowsResolutions[m_DpiWindowIndex];
			if (m_FullScreenResolutions.Contains(value2))
			{
				m_DpiFullScreenIndex = m_FullScreenResolutions.IndexOf(value2);
			}
			dpiFullScreen.ScrollToDataIndex(m_DpiFullScreenIndex, 0f);
			m_ScreenModeIndex = i;
			break;
		}
		case 1:
		{
			m_IsFullScreen = false;
			dpiFullScreen.transform.parent.gameObject.SetActive(m_IsFullScreen);
			dpiWindow.transform.parent.gameObject.SetActive(!m_IsFullScreen);
			m_HasBorder = true;
			Vector2Int value3 = m_FullScreenResolutions[m_DpiFullScreenIndex];
			if (m_WindowsResolutions.Contains(value3))
			{
				m_DpiWindowIndex = m_WindowsResolutions.IndexOf(value3);
			}
			dpiWindow.ScrollToDataIndex(m_DpiWindowIndex, 0f);
			m_ScreenModeIndex = i;
			break;
		}
		case 2:
		{
			m_IsFullScreen = false;
			dpiFullScreen.transform.parent.gameObject.SetActive(m_IsFullScreen);
			dpiWindow.transform.parent.gameObject.SetActive(!m_IsFullScreen);
			m_HasBorder = false;
			Vector2Int value = m_FullScreenResolutions[m_DpiFullScreenIndex];
			if (m_WindowsResolutions.Contains(value))
			{
				m_DpiWindowIndex = m_WindowsResolutions.IndexOf(value);
			}
			dpiWindow.ScrollToDataIndex(m_DpiWindowIndex, 0f);
			m_ScreenModeIndex = i;
			break;
		}
		}
		SetHighlight((!m_IsFullScreen) ? dpiWindow : dpiFullScreen, false);
		AdjustNavi((!m_IsFullScreen) ? dpiWindow : dpiFullScreen);
		Singleton<DataManager>.instance["GameConfig"]["FullScreen"].SetResult(m_IsFullScreen);
		Singleton<DataManager>.instance["GameConfig"]["HasBorder"].SetResult(m_HasBorder);
		resolution = ((!m_IsFullScreen) ? m_WindowsResolutions[m_DpiWindowIndex] : m_FullScreenResolutions[m_DpiFullScreenIndex]);
		Singleton<DataManager>.instance["GameConfig"]["ScreenResolutions"].SetResult(GetResolutionsUid(resolution));
		Singleton<DataManager>.instance.Save();
		if (!m_HasBorder)
		{
			GraphicSettings.SetResolution(resolution.x, resolution.y, m_IsFullScreen);
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				GraphicSettings.SetNoBorder(resolution.x, resolution.y);
			}, 0.1f);
			return;
		}
		GraphicSettings.SetResolution(resolution.x, resolution.y, m_IsFullScreen);
		SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
		{
			if (!m_IsFullScreen)
			{
				GraphicSettings.SetHasBorder(resolution.x, resolution.y);
			}
			GraphicSettings.SetResolution(resolution.x, resolution.y, m_IsFullScreen);
		}, 0.1f);
	}

	private void OnFpsChange(int i)
	{
		GraphicSettings.SetFps((i != 0) ? m_Fps[i - 1] : 60, i == 0);
		Singleton<DataManager>.instance["GameConfig"]["Fps"].SetResult((i != 0) ? m_Fps[i - 1] : 0);
		Singleton<DataManager>.instance.Save();
	}

	private void OnClickConfirm()
	{
		if (m_IsFullScreen)
		{
			m_DpiFullScreenIndex = dpiFullScreen.selectItemIndex;
		}
		else
		{
			m_DpiWindowIndex = dpiWindow.selectItemIndex;
		}
		Vector2Int v = (!m_IsFullScreen) ? m_WindowsResolutions[m_DpiWindowIndex] : m_FullScreenResolutions[m_DpiFullScreenIndex];
		if (!m_HasBorder)
		{
			GraphicSettings.SetNoBorder(v.x, v.y);
		}
		else
		{
			GraphicSettings.SetResolution(v.x, v.y, m_IsFullScreen);
		}
		confirm.SetActive(false);
		Singleton<DataManager>.instance["GameConfig"]["FullScreen"].SetResult(m_IsFullScreen);
		Singleton<DataManager>.instance["GameConfig"]["ScreenResolutions"].SetResult(GetResolutionsUid(v));
		Singleton<DataManager>.instance.Save();
	}

	public override GameObject DefaultSelectObj()
	{
		if (m_IsInit)
		{
			return (!m_IsFullScreen) ? dpiWindow.btnPrevious.gameObject : dpiFullScreen.btnPrevious.gameObject;
		}
		return null;
	}

	public override void OnDisablePnl()
	{
		if (dpiFullScreen.selectItemIndex != m_DpiFullScreenIndex)
		{
			dpiFullScreen.ScrollToDataIndex(m_DpiFullScreenIndex, 0f);
		}
		if (dpiWindow.selectItemIndex != m_DpiWindowIndex)
		{
			dpiWindow.ScrollToDataIndex(m_DpiWindowIndex, 0f);
		}
		if (screenMode.selectItemIndex != m_ScreenModeIndex)
		{
			screenMode.ScrollToDataIndex(m_ScreenModeIndex, 0f);
		}
	}

	public override List<GameObject> SetSelectableObj()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < m_FxToggles.Count; i++)
		{
			list.Add(m_FxToggles[i].gameObject);
		}
		list.Add(advancedOn.gameObject);
		list.Add(standardOn.gameObject);
		list.Add(screenMode.btnPrevious.gameObject);
		list.Add(dpiFullScreen.btnPrevious.gameObject);
		list.Add(dpiWindow.btnPrevious.gameObject);
		list.Add(fpsFsv.btnPrevious.gameObject);
		list.Add(enterBackgroundBtn);
		return list;
	}

	public override void OnSelect(GameObject currentObj)
	{
		FancyScrollView fancyScrollView = (!m_IsFullScreen) ? dpiWindow : dpiFullScreen;
		if (currentObj == fancyScrollView.btnPrevious.gameObject)
		{
			SetHighlight(fancyScrollView, true);
		}
		else if (currentObj == screenMode.btnPrevious.gameObject)
		{
			SetHighlight(screenMode, true);
		}
		else if (currentObj == fpsFsv.btnPrevious.gameObject)
		{
			SetHighlight(fpsFsv, true);
		}
		else if (currentObj == enterBackgroundBtn)
		{
			enterBackgroundSelectImg.SetActive(true);
		}
		else
		{
			Toggle component = currentObj.GetComponent<Toggle>();
			if ((bool)component)
			{
				SetHighlight(component, true);
			}
		}
		if (!lastSelectedObj)
		{
			return;
		}
		if (lastSelectedObj == dpiFullScreen.btnPrevious.gameObject)
		{
			SetHighlight(dpiFullScreen, false);
			return;
		}
		if (lastSelectedObj == dpiWindow.btnPrevious.gameObject)
		{
			SetHighlight(dpiWindow, false);
			return;
		}
		if (lastSelectedObj == screenMode.btnPrevious.gameObject)
		{
			SetHighlight(screenMode, false);
			return;
		}
		if (lastSelectedObj == fpsFsv.btnPrevious.gameObject)
		{
			SetHighlight(fpsFsv, false);
			return;
		}
		if (lastSelectedObj == enterBackgroundBtn)
		{
			enterBackgroundSelectImg.SetActive(false);
			return;
		}
		Toggle component2 = lastSelectedObj.GetComponent<Toggle>();
		if ((bool)component2)
		{
			SetHighlight(component2, false);
		}
	}

	private void AdjustNavi(FancyScrollView fsv)
	{
		Navigation navigation = fsv.btnPrevious.navigation;
		navigation.mode = Navigation.Mode.Explicit;
		navigation.selectOnDown = screenMode.btnPrevious;
		fsv.btnPrevious.navigation = navigation;
		Navigation navigation2 = screenMode.btnPrevious.navigation;
		navigation2.mode = Navigation.Mode.Explicit;
		navigation2.selectOnUp = fsv.btnPrevious;
		screenMode.btnPrevious.navigation = navigation2;
	}

	private void SetHighlight(FancyScrollView fsv, bool enable)
	{
		fsv.btnPrevious.image.color = ((!enable) ? normalColor : highlightColor);
		fsv.btnNext.image.color = ((!enable) ? normalColor : highlightColor);
		for (int i = 0; i < fsv.content.childCount; i++)
		{
			fsv.content.GetChild(i).GetComponent<Text>().color = ((!enable) ? normalColor : highlightColor);
		}
		List<InputKeyBinding> list = GameUtils.FindObjectsOfType<InputKeyBinding>(fsv.transform);
		foreach (InputKeyBinding item in list)
		{
			item.enabled = enable;
		}
		if (confirm.activeInHierarchy && (fsv == dpiFullScreen || fsv == dpiWindow))
		{
			SetConfirmHighlight(enable);
		}
	}

	private void SetHighlight(Toggle tgl, bool enable)
	{
		tgl.targetGraphic.color = ((!enable) ? normalColor : highlightColor);
		tgl.graphic.color = ((!enable) ? normalColor : highlightColor);
		tgl.transform.Find("Txt").GetComponent<Text>().color = ((!enable) ? normalColor : highlightColor);
	}

	private void SetConfirmHighlight(bool enable)
	{
		confirm.GetComponent<Text>().color = ((!enable) ? normalColor : highlightColor);
		confirm.GetComponent<InputKeyBinding>().enabled = enable;
		confirm.transform.Find("ImgConfirm").GetComponent<Image>().color = ((!enable) ? normalColor : highlightColor);
	}

	private Vector2Int GetResolutions(string uid)
	{
		string[] array = uid.Split('x');
		return new Vector2Int(int.Parse(array[0]), int.Parse(array[1]));
	}

	private string GetResolutionsUid(Vector2 resolution)
	{
		return resolution.x + "x" + resolution.y;
	}

	private int GetResolutionsIndex(string uid)
	{
		Vector2Int[] array = (!m_IsFullScreen) ? m_WindowsResolutions : m_FullScreenResolutions;
		Vector2Int resolutions = GetResolutions(uid);
		if (array.Contains(resolutions))
		{
			return array.IndexOf(resolutions);
		}
		return -1;
	}

	private int GetFpsIndex()
	{
		int result = Singleton<DataManager>.instance["GameConfig"]["Fps"].GetResult<int>();
		if (m_Fps.Contains(result))
		{
			return m_Fps.IndexOf(result) + 1;
		}
		return result;
	}

	public override Transform SetEdgeObj(GameObject currentObj)
	{
		return enterBackgroundSelectImg.transform;
	}
}
