using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Components;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PnlInputSwitch : UISelectManage
{
	public Button switchL;

	public Button switchR;

	public Text txtButton;

	public Text txtScreen;

	private bool m_IsButton = true;

	[Required]
	public FancyScrollView buttonFancyScrollView;

	[Required]
	public FancyScrollView screenFancyScrollView;

	[Required]
	public FancyScrollView reverseFancyScrollView;

	[Required]
	public FancyScrollView vibrationFancyScrollView;

	[Required]
	public PointsBar buttonPointsBar;

	[Required]
	public PointsBar screenPointsBar;

	public Color highLightColor;

	public Color normalColor;

	public AudioClip scrollAudio;

	private static readonly string[] buttonProposal = new string[4]
	{
		InputManager.MDButtonProposalName.Default,
		InputManager.MDButtonProposalName.ModeB,
		InputManager.MDButtonProposalName.ModeC,
		InputManager.MDButtonProposalName.ModeD
	};

	private static readonly string[] revertButtonProposal = new string[4]
	{
		InputManager.MDButtonProposalName.DefaultReverse,
		InputManager.MDButtonProposalName.ModeBReverse,
		InputManager.MDButtonProposalName.ModeCReverse,
		InputManager.MDButtonProposalName.ModeDReverse
	};

	private int m_ButtonProposalIndex;

	private int m_ScreenProposalIndex;

	private bool m_IsReverse;

	private bool m_IsVibration;

	private bool m_IsInitReverse;

	private bool m_IsInitVibration;

	private bool m_IsInitButton;

	private bool m_IsInitScreen;

	public override void OnEnablePnl()
	{
		m_IsInitReverse = true;
		m_IsInitVibration = true;
	}

	public override GameObject DefaultSelectObj()
	{
		FancyScrollView fancyScrollView = (!m_IsButton) ? screenFancyScrollView : buttonFancyScrollView;
		return fancyScrollView.btnPrevious.gameObject;
	}

	public override void OnInit()
	{
		m_IsReverse = Singleton<DataManager>.instance["Account"]["IsReverse"].GetResult<bool>();
		reverseFancyScrollView.ScrollToDataIndex(m_IsReverse ? 1 : 0, 0f);
		string result = Singleton<DataManager>.instance["Account"]["NSInputName"].GetResult<string>();
		m_ButtonProposalIndex = ((!m_IsReverse) ? buttonProposal.IndexOf(result) : revertButtonProposal.IndexOf(result));
		buttonPointsBar.SetDefaultPoint(m_ButtonProposalIndex);
		m_IsInitButton = true;
		m_ScreenProposalIndex = ((!Singleton<DataManager>.instance["Account"]["IsLeftRight"].GetResult<bool>()) ? 1 : 0);
		screenPointsBar.SetDefaultPoint(m_ScreenProposalIndex);
		m_IsInitScreen = true;
		vibrationFancyScrollView.ScrollToDataIndex((!m_IsVibration) ? 1 : 0, 0f);
		switchL.onClick.AddListener(delegate
		{
			if (!m_IsButton)
			{
				OnModeFsvSelect();
			}
		});
		switchR.onClick.AddListener(delegate
		{
			if (m_IsButton)
			{
				OnModeFsvSelect();
			}
		});
		buttonFancyScrollView.onFinalItemIndexChange += SetButtonProposal;
		screenFancyScrollView.onFinalItemIndexChange += SetScreenProposal;
		reverseFancyScrollView.onFinalItemIndexChange += OnReverseFsvSelect;
		vibrationFancyScrollView.onFinalItemIndexChange += OnVibrationFsvSelect;
		SetSelectObjHighLight(buttonFancyScrollView, true);
		DOTween.To(() => txtButton.color, delegate(Color x)
		{
			txtButton.color = x;
		}, (!m_IsButton) ? new Color(1f, 1f, 1f, 0.5f) : Color.white, 0.5f);
		DOTween.To(() => txtScreen.color, delegate(Color x)
		{
			txtScreen.color = x;
		}, m_IsButton ? new Color(1f, 1f, 1f, 0.5f) : Color.white, 0.5f);
	}

	private void OnModeFsvSelect()
	{
		m_IsButton = !m_IsButton;
		Button btnPrevious = reverseFancyScrollView.btnPrevious;
		Button btnNext = reverseFancyScrollView.btnNext;
		Button up = (!m_IsButton) ? screenFancyScrollView.btnPrevious : buttonFancyScrollView.btnPrevious;
		Button up2 = (!m_IsButton) ? screenFancyScrollView.btnNext : buttonFancyScrollView.btnNext;
		AdjustNavi(btnPrevious, up, vibrationFancyScrollView.btnPrevious, null, btnNext);
		AdjustNavi(btnNext, up2, vibrationFancyScrollView.btnNext, btnPrevious, null);
		EventSystem.current.SetSelectedGameObject((!m_IsButton) ? screenFancyScrollView.btnPrevious.gameObject : buttonFancyScrollView.btnPrevious.gameObject);
		DOTween.To(() => txtButton.color, delegate(Color x)
		{
			txtButton.color = x;
		}, (!m_IsButton) ? new Color(1f, 1f, 1f, 0.5f) : Color.white, 0.5f);
		DOTween.To(() => txtScreen.color, delegate(Color x)
		{
			txtScreen.color = x;
		}, m_IsButton ? new Color(1f, 1f, 1f, 0.5f) : Color.white, 0.5f);
	}

	private void OnReverseFsvSelect(int index)
	{
		m_IsReverse = (index == 1);
		SetReverseUI();
		if (m_IsInitReverse)
		{
			m_IsInitReverse = false;
		}
		else
		{
			EventSystem.current.SetSelectedGameObject(reverseFancyScrollView.btnPrevious.gameObject);
		}
	}

	private void OnVibrationFsvSelect(int index)
	{
		m_IsVibration = (index == 0);
		if (m_IsInitVibration)
		{
			m_IsInitVibration = false;
			return;
		}
		if (m_IsVibration)
		{
			Debug.Log("Vibration");
		}
		EventSystem.current.SetSelectedGameObject(vibrationFancyScrollView.btnPrevious.gameObject);
	}

	public void SetButtonProposal(int index)
	{
		if (m_IsInitButton)
		{
			buttonFancyScrollView.ScrollToDataIndex(m_ButtonProposalIndex, 0f);
			m_IsInitButton = false;
		}
		else
		{
			m_ButtonProposalIndex = index;
			EventSystem.current.SetSelectedGameObject(buttonFancyScrollView.btnPrevious.gameObject);
		}
	}

	public void SetScreenProposal(int index)
	{
		if (m_IsInitScreen)
		{
			screenFancyScrollView.ScrollToDataIndex(m_ScreenProposalIndex, 0f);
			m_IsInitScreen = false;
		}
		else
		{
			m_ScreenProposalIndex = index;
			EventSystem.current.SetSelectedGameObject(screenFancyScrollView.btnPrevious.gameObject);
		}
	}

	private void SetReverseUI()
	{
		for (int i = 0; i < buttonFancyScrollView.content.childCount; i++)
		{
			Transform child = buttonFancyScrollView.content.GetChild(i);
			child.GetComponent<ControllerCell>().SetReverse(m_IsReverse);
		}
		for (int j = 0; j < screenFancyScrollView.content.childCount; j++)
		{
			Transform child2 = screenFancyScrollView.content.GetChild(j);
			child2.GetComponent<ControllerCell>().SetReverse(m_IsReverse);
		}
	}

	public override List<GameObject> SetSelectableObj()
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(buttonFancyScrollView.btnPrevious.gameObject);
		list.Add(screenFancyScrollView.btnPrevious.gameObject);
		list.Add(reverseFancyScrollView.btnPrevious.gameObject);
		list.Add(vibrationFancyScrollView.btnPrevious.gameObject);
		return list;
	}

	public override void OnSelect(GameObject currentObj)
	{
		FancyScrollView component = lastSelectedObj.transform.parent.GetComponent<FancyScrollView>();
		if (component != null)
		{
			SetSelectObjHighLight(component, false);
		}
		FancyScrollView component2 = currentObj.transform.parent.GetComponent<FancyScrollView>();
		SetSelectObjHighLight(component2, true);
		lastSelectedObj = currentObj;
	}

	public override void OnDisablePnl()
	{
		string value = (!m_IsReverse) ? buttonProposal[m_ButtonProposalIndex] : revertButtonProposal[m_ButtonProposalIndex];
		Singleton<DataManager>.instance["Account"]["NSInputName"].SetResult(value);
		Singleton<DataManager>.instance["Account"]["IsBtnLeftRight"].SetResult(m_ScreenProposalIndex == 0);
		Singleton<DataManager>.instance["Account"]["IsLeftRight"].SetResult(m_ScreenProposalIndex == 0);
		Singleton<DataManager>.instance["Account"]["IsReverse"].SetResult(m_IsReverse);
	}

	private void SetSelectObjHighLight(FancyScrollView fsv, bool enable)
	{
		Color color = (!enable) ? normalColor : highLightColor;
		List<InputKeyBinding> list = GameUtils.FindObjectsOfType<InputKeyBinding>(fsv.transform);
		foreach (InputKeyBinding item in list)
		{
			item.enabled = enable;
		}
		fsv.btnPrevious.GetComponent<Image>().color = color;
		fsv.btnNext.GetComponent<Image>().color = color;
		if (fsv == reverseFancyScrollView || fsv == vibrationFancyScrollView)
		{
			int childCount = fsv.content.childCount;
			for (int i = 0; i < childCount; i++)
			{
				fsv.content.GetChild(i).GetComponent<Text>().color = color;
			}
			fsv.transform.GetChild(0).GetComponent<Text>().color = color;
			fsv.transform.GetChild(1).GetComponent<Image>().color = color;
		}
		if (fsv == buttonFancyScrollView || fsv == screenFancyScrollView)
		{
			int childCount2 = fsv.content.childCount;
			for (int j = 0; j < childCount2; j++)
			{
				fsv.content.GetChild(j).GetComponent<ControllerCell>().SetSelectColor(color);
			}
			PointsBar pointsBar = (!(fsv == buttonFancyScrollView)) ? screenPointsBar : buttonPointsBar;
			pointsBar.SetCurrentPoint(enable);
		}
	}

	private void AdjustNavi(Selectable btn, Selectable up, Selectable down, Selectable left, Selectable right)
	{
		Navigation navigation = btn.navigation;
		navigation.mode = Navigation.Mode.Explicit;
		navigation.selectOnUp = up;
		navigation.selectOnDown = down;
		navigation.selectOnLeft = left;
		navigation.selectOnRight = right;
		btn.navigation = navigation;
	}
}
