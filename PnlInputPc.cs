using Assets.Scripts.GameCore.Controller.Configs;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Components;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.UI;
using DG.Tweening;
using GameLogic;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PnlInputPc : UISelectManage
{
	[HideInInspector]
	public enum keyType
	{
		NormalKey,
		SpecialKey,
		SettingKey,
		NoKey
	}

	public Button switchModeL;

	public Button switchModeR;

	public Text txtButton;

	public Text txtScreen;

	private bool m_IsButton = true;

	[Required]
	public FancyScrollView modeFancyScrollView;

	[Required]
	public FancyScrollView buttonFancyScrollView;

	[Required]
	public FancyScrollView screenFancyScrollView;

	[Required]
	public FancyScrollView reverseFancyScrollView;

	[Required]
	public FancyScrollView vibrationFancyScrollView;

	[Required]
	public FancyScrollView keyboardFeverFancyScrollView;

	[Required]
	public FancyScrollView joystickFeverFancyScrollView;

	[Required]
	public Transform keyboardFeverTipsBtn;

	[Required]
	public Transform joystickFeverTipsBtn;

	[Required]
	public PointsBar buttonPointsBar;

	[Required]
	public PointsBar screenPointsBar;

	private static PnlInputPc m_Instance;

	public Color highLightColor;

	public Color normalColor;

	public AudioClip scrollAudio;

	public float titleAnimTime;

	public static readonly string[] buttonProposal = new string[5]
	{
		InputManager.MDButtonProposalName.Default,
		InputManager.MDButtonProposalName.ModeB,
		InputManager.MDButtonProposalName.ModeC,
		InputManager.MDButtonProposalName.ModeD,
		InputManager.MDButtonProposalName.Custom
	};

	public static readonly string[] revertButtonProposal = new string[4]
	{
		InputManager.MDButtonProposalName.DefaultReverse,
		InputManager.MDButtonProposalName.ModeBReverse,
		InputManager.MDButtonProposalName.ModeCReverse,
		InputManager.MDButtonProposalName.ModeDReverse
	};

	public Image onCustomizeBg;

	[HideInInspector]
	public int buttonProposalIndex;

	[HideInInspector]
	public int screenProposalIndex;

	private bool m_IsReverse;

	private bool m_IsVibration;

	private bool m_IsManualFever;

	private bool m_IsInitReverse;

	private bool m_IsInitVibration;

	private bool m_IsInitManualFever;

	private bool m_IsInitManualFever_Joystick;

	public GameObject[] keyboardTypeList;

	public Button btnCustom;

	public Button btnKeyboardUI;

	public Button btnHandleUI;

	public Button btnCancelCustomize;

	public InputCustomizeSelect customizeSelect;

	public List<GameObject> m_CustomKeyList;

	public Selectable feverCustomKey;

	[HideInInspector]
	public int currentSelectedCustomKeyIndex;

	private GameObject m_CurrentSelectedCustomKey;

	public Dictionary<string, Sprite> specialBtnImgDict;

	[HideInInspector]
	public string currentButtonProposal;

	private Dictionary<string, List<KeyCode>> m_SourceKey_Dict = new Dictionary<string, List<KeyCode>>();

	private StandloneCtrlConfig m_Config;

	[HideInInspector]
	public bool customIsChanged;

	[HideInInspector]
	public bool allowListenKey;

	[HideInInspector]
	public bool shiftAndCtrlCheck;

	[HideInInspector]
	public string shiftOrCtrl = string.Empty;

	private string m_ShiftOrCtrlUiName = string.Empty;

	private Dictionary<string, string> m_SimplifyKeyDict = new Dictionary<string, string>
	{
		{
			"Delete",
			"Del"
		},
		{
			"Insert",
			"Ins"
		},
		{
			"Equals",
			"="
		},
		{
			"Minus",
			"-"
		},
		{
			"Alpha1",
			"1"
		},
		{
			"Alpha2",
			"2"
		},
		{
			"Alpha3",
			"3"
		},
		{
			"Alpha4",
			"4"
		},
		{
			"Alpha5",
			"5"
		},
		{
			"Alpha6",
			"6"
		},
		{
			"Alpha7",
			"7"
		},
		{
			"Alpha8",
			"8"
		},
		{
			"Alpha9",
			"9"
		},
		{
			"Alpha0",
			"0"
		},
		{
			"PageUp",
			"Page\nUp"
		},
		{
			"PageDown",
			"Page\nDown"
		},
		{
			"RightBracket",
			"]"
		},
		{
			"LeftBracket",
			"["
		},
		{
			"Backslash",
			"\\"
		},
		{
			"Quote",
			"'"
		},
		{
			"Semicolon",
			";"
		},
		{
			"Slash",
			"/"
		},
		{
			"Period",
			"."
		},
		{
			"Comma",
			","
		},
		{
			"Return",
			"Enter"
		},
		{
			"CapsLock",
			"Caps\nLock"
		},
		{
			"Up Arrow",
			"UpArrow"
		},
		{
			"Down Arrow",
			"DownArrow"
		},
		{
			"Right Arrow",
			"RightArrow"
		},
		{
			"Left Arrow",
			"LeftArrow"
		},
		{
			"KeyPad 1",
			"Num1"
		},
		{
			"KeyPad 2",
			"Num2"
		},
		{
			"KeyPad 3",
			"Num3"
		},
		{
			"KeyPad 4",
			"Num4"
		},
		{
			"KeyPad 5",
			"Num5"
		},
		{
			"KeyPad 6",
			"Num6"
		},
		{
			"KeyPad 7",
			"Num7"
		},
		{
			"KeyPad 8",
			"Num8"
		},
		{
			"KeyPad 9",
			"Num9"
		},
		{
			"KeyPad 0",
			"Num0"
		},
		{
			"KeyPad +",
			"Num+"
		},
		{
			"KeyPad -",
			"Num-"
		},
		{
			"KeyPad *",
			"Num*"
		},
		{
			"KeyPad .",
			"Num."
		},
		{
			"KeyPad Enter",
			"Enter"
		},
		{
			"LeftShift",
			"L\nShift"
		},
		{
			"RightShift",
			"R\nShift"
		},
		{
			"LeftControl",
			"L\nCtrl"
		},
		{
			"RightControl",
			"R\nCtrl"
		},
		{
			"Backspace",
			"Back\nspace"
		}
	};

	private bool m_IsInit;

	public GameObject feverBase;

	public List<GameObject> otherDefaultFeverBtn;

	public GameObject GetCurrentSelectedCustomKey()
	{
		return m_CurrentSelectedCustomKey;
	}

	public static PnlInputPc Instance()
	{
		return m_Instance;
	}

	public override void OnEnablePnl()
	{
		m_IsInitReverse = true;
		m_IsInitVibration = true;
		m_IsInitManualFever = true;
		m_Instance = this;
		if (m_IsInit)
		{
			if (Singleton<InputManager>.instance.currentControllerName != "Keyboard")
			{
				modeFancyScrollView.ScrollToDataIndex(1, 0f);
				m_IsButton = false;
			}
			else
			{
				modeFancyScrollView.ScrollToDataIndex(0, 0f);
				m_IsButton = true;
			}
		}
		EventSystem.current.SetSelectedGameObject((!m_IsButton) ? screenFancyScrollView.btnPrevious.gameObject : buttonFancyScrollView.btnPrevious.gameObject);
		SetModeTitleAnim();
	}

	public override void OnInit()
	{
		if (Singleton<InputManager>.instance.currentControllerName != "Keyboard")
		{
			modeFancyScrollView.ScrollToDataIndex(1, 0f);
			m_IsButton = false;
		}
		else
		{
			modeFancyScrollView.ScrollToDataIndex(0, 0f);
			m_IsButton = true;
		}
		m_Config = Singleton<AssetBundleManager>.instance.LoadFromName<StandloneCtrlConfig>("InputStandlone");
		m_IsReverse = Singleton<DataManager>.instance["Account"]["IsReverse"].GetResult<bool>();
		m_IsVibration = Singleton<InputManager>.instance.isVibration;
		m_IsManualFever = !Singleton<DataManager>.instance["Account"]["IsAutoFever"].GetResult<bool>();
		keyboardFeverFancyScrollView.ScrollToDataIndex((!m_IsManualFever) ? 1 : 0, 0f);
		string keyBoardProposal = Singleton<InputManager>.instance.keyBoardProposal;
		buttonProposalIndex = buttonProposal.IndexOf(keyBoardProposal);
		currentButtonProposal = buttonProposal[buttonProposalIndex];
		buttonFancyScrollView.ScrollToDataIndex(buttonProposalIndex, 0f);
		buttonPointsBar.SetDefaultPoint(buttonProposalIndex);
		string handleProposal = Singleton<InputManager>.instance.handleProposal;
		screenProposalIndex = buttonProposal.IndexOf(handleProposal);
		screenFancyScrollView.ScrollToDataIndex(screenProposalIndex, 0f);
		screenPointsBar.SetDefaultPoint(screenProposalIndex);
		joystickFeverFancyScrollView.Rebuild();
		modeFancyScrollView.onFinalItemIndexChange += OnModeSelectedInit;
		buttonFancyScrollView.onFinalItemIndexChange += SetButtonProposal;
		screenFancyScrollView.onFinalItemIndexChange += SetScreenProposal;
		reverseFancyScrollView.onFinalItemIndexChange += OnReverseFsvSelect;
		vibrationFancyScrollView.onFinalItemIndexChange += OnVibrationFsvSelect;
		keyboardFeverFancyScrollView.onFinalItemIndexChange += OnKeyboardFeverFsvSelect;
		joystickFeverFancyScrollView.onFinalItemIndexChange += OnJoystickFeverFsvSelect;
		SetSelectObjHighLight(buttonFancyScrollView, true);
		SetSelectObjHighLight(screenFancyScrollView, true);
		btnCustom.onClick.AddListener(OnClickBtnCustom);
		ChangeCutomKeyListUI();
		switchModeL.onClick.AddListener(delegate
		{
			if (!m_IsButton)
			{
				OnModeFsvSelect();
			}
		});
		switchModeR.onClick.AddListener(delegate
		{
			if (m_IsButton)
			{
				OnModeFsvSelect();
			}
		});
		keyboardFeverFancyScrollView.btnNext.onClick.AddListener(delegate
		{
			EventSystem.current.SetSelectedGameObject(keyboardFeverFancyScrollView.btnPrevious.gameObject);
		});
		joystickFeverFancyScrollView.btnNext.onClick.AddListener(delegate
		{
			EventSystem.current.SetSelectedGameObject(joystickFeverFancyScrollView.btnPrevious.gameObject);
		});
		keyboardFeverTipsBtn.GetComponent<Button>().onClick.AddListener(delegate
		{
			EventSystem.current.SetSelectedGameObject(keyboardFeverFancyScrollView.btnPrevious.gameObject);
		});
		joystickFeverTipsBtn.GetComponent<Button>().onClick.AddListener(delegate
		{
			EventSystem.current.SetSelectedGameObject(joystickFeverFancyScrollView.btnPrevious.gameObject);
		});
		SetModeTitleAnim();
		SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop(Loop, UnityGameManager.LoopType.Update);
		m_IsInit = true;
		SetCustomKeyNavigation();
		joystickFeverFancyScrollView.ScrollToDataIndex((!m_IsManualFever) ? 1 : 0, 0f);
	}

	private void Loop(float time)
	{
		if (allowListenKey && (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)))
		{
			shiftAndCtrlCheck = true;
			if (Input.GetKeyDown(KeyCode.LeftControl))
			{
				shiftOrCtrl = KeyCode.LeftControl.ToString();
				m_ShiftOrCtrlUiName = "L\nCtrl";
			}
			if (Input.GetKeyDown(KeyCode.RightControl))
			{
				shiftOrCtrl = KeyCode.RightControl.ToString();
				m_ShiftOrCtrlUiName = "R\nCtrl";
			}
			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				shiftOrCtrl = KeyCode.LeftShift.ToString();
				m_ShiftOrCtrlUiName = "L Shift";
			}
			if (Input.GetKeyDown(KeyCode.RightShift))
			{
				shiftOrCtrl = KeyCode.RightShift.ToString();
				m_ShiftOrCtrlUiName = "R Shift";
			}
			if (m_CurrentSelectedCustomKey.transform.parent.name == "Air")
			{
				Singleton<InputManager>.instance.OnChangeSpecialButton("BattleAir", shiftOrCtrl, m_ShiftOrCtrlUiName);
			}
			else if (m_CurrentSelectedCustomKey.transform.parent.name == "Ground")
			{
				Singleton<InputManager>.instance.OnChangeSpecialButton("BattleGround", shiftOrCtrl, m_ShiftOrCtrlUiName);
			}
		}
	}

	private void OnModeFsvSelect()
	{
		m_IsButton = !m_IsButton;
		EventSystem.current.SetSelectedGameObject((!m_IsButton) ? screenFancyScrollView.btnPrevious.gameObject : buttonFancyScrollView.btnPrevious.gameObject);
		m_IsInitReverse = true;
		m_IsInitVibration = true;
		m_IsInitManualFever = true;
		m_IsInitManualFever_Joystick = true;
		m_Instance = this;
		SetModeTitleAnim();
	}

	private void OnReverseFsvSelect(int index)
	{
		if (m_IsInitReverse)
		{
			m_IsInitReverse = false;
			return;
		}
		m_IsReverse = (index == 1);
		SetReverseUI();
		EventSystem.current.SetSelectedGameObject(reverseFancyScrollView.btnPrevious.gameObject);
		PlayAudio();
	}

	private void OnVibrationFsvSelect(int index)
	{
		if (m_IsInitVibration)
		{
			m_IsInitVibration = false;
			return;
		}
		m_IsVibration = (index == 0);
		if (m_IsVibration)
		{
			Singleton<InputManager>.instance.SetNsVibration(InputManager.VibrationVaule.Default, InputManager.DeviceHandles.Both, 0.3f);
		}
		EventSystem.current.SetSelectedGameObject(vibrationFancyScrollView.btnPrevious.gameObject);
		PlayAudio();
	}

	private void OnKeyboardFeverFsvSelect(int index)
	{
		if (m_IsInitManualFever_Joystick)
		{
			m_IsInitManualFever_Joystick = false;
		}
		else
		{
			m_IsManualFever = (index == 0);
		}
		if (m_IsManualFever)
		{
			joystickFeverFancyScrollView.ScrollToDataIndex(0, 0f);
			SetFeverUI(true);
		}
		else
		{
			joystickFeverFancyScrollView.ScrollToDataIndex(1, 0f);
			SetFeverUI(false);
		}
		SetCustomKeyNavigation();
	}

	private void OnModeSelectedInit(int index)
	{
		joystickFeverFancyScrollView.ScrollToDataIndex((!m_IsManualFever) ? 1 : 0, 0f);
	}

	private void OnJoystickFeverFsvSelect(int index)
	{
		if (m_IsInitManualFever_Joystick)
		{
			m_IsInitManualFever_Joystick = false;
		}
		else
		{
			m_IsManualFever = (index == 0);
		}
		if (m_IsManualFever)
		{
			keyboardFeverFancyScrollView.ScrollToDataIndex(0, 0f);
			SetFeverUI(true);
		}
		else
		{
			keyboardFeverFancyScrollView.ScrollToDataIndex(1, 0f);
			SetFeverUI(false);
		}
	}

	public void SetButtonProposal(int index)
	{
		keyboardTypeList[index].SetActive(true);
		for (int i = 0; i < keyboardTypeList.Length; i++)
		{
			if (i == index)
			{
				keyboardTypeList[index].SetActive(true);
			}
			else
			{
				keyboardTypeList[i].SetActive(false);
			}
		}
		buttonProposalIndex = index;
		currentButtonProposal = buttonProposal[buttonProposalIndex];
		EventSystem.current.SetSelectedGameObject(buttonFancyScrollView.btnPrevious.gameObject);
		PlayAudio();
	}

	public void SetScreenProposal(int index)
	{
		screenProposalIndex = index;
		EventSystem.current.SetSelectedGameObject(screenFancyScrollView.btnPrevious.gameObject);
		reverseFancyScrollView.ScrollToDataIndex(m_IsReverse ? 1 : 0, 0f);
		if (m_IsVibration)
		{
			vibrationFancyScrollView.ScrollToDataIndex(0, 0f);
		}
		else
		{
			vibrationFancyScrollView.ScrollToDataIndex(1, 0f);
		}
		PlayAudio();
	}

	private void SetReverseUI()
	{
		for (int i = 0; i < screenFancyScrollView.content.childCount; i++)
		{
			Transform child = screenFancyScrollView.content.GetChild(i);
			child.GetComponent<PCControllerCell>().SetReverse(m_IsReverse);
		}
	}

	public override List<GameObject> SetSelectableObj()
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(buttonFancyScrollView.btnPrevious.gameObject);
		list.Add(screenFancyScrollView.btnPrevious.gameObject);
		list.Add(reverseFancyScrollView.btnPrevious.gameObject);
		list.Add(vibrationFancyScrollView.btnPrevious.gameObject);
		list.Add(btnCustom.gameObject);
		list.Add(btnKeyboardUI.gameObject);
		list.Add(btnHandleUI.gameObject);
		list.Add(keyboardFeverFancyScrollView.btnPrevious.gameObject);
		list.Add(joystickFeverFancyScrollView.btnPrevious.gameObject);
		return list;
	}

	public override void OnSelect(GameObject currentObj)
	{
		FancyScrollView component = lastSelectedObj.transform.parent.GetComponent<FancyScrollView>();
		if (lastSelectedObj.transform.parent.name == "ImgCover")
		{
			component = lastSelectedObj.transform.parent.transform.parent.Find("ScrKeyboardType").GetComponent<FancyScrollView>();
		}
		else if (lastSelectedObj == btnCustom.gameObject || lastSelectedObj == btnKeyboardUI.gameObject || lastSelectedObj == btnHandleUI.gameObject)
		{
			SetSelectObjHighLight(lastSelectedObj, false);
		}
		if (component != null)
		{
			SetSelectObjHighLight(component, false);
		}
		if (currentObj.transform.parent.name == "ImgCover")
		{
			FancyScrollView component2 = currentObj.transform.parent.transform.parent.Find("ScrKeyboardType").GetComponent<FancyScrollView>();
			SetSelectObjHighLight(component2, true);
		}
		else if (currentObj == btnCustom.gameObject || currentObj == btnKeyboardUI.gameObject || currentObj == btnHandleUI.gameObject)
		{
			SetSelectObjHighLight(currentObj, true);
		}
		else
		{
			FancyScrollView component3 = currentObj.transform.parent.GetComponent<FancyScrollView>();
			SetSelectObjHighLight(component3, true);
		}
		if (lastSelectedObj != null && (bool)lastSelectedObj.GetComponent<InputKeyBinding>())
		{
			if (lastSelectedObj.GetComponents<InputKeyBinding>().Length > 1)
			{
				InputKeyBinding[] components = lastSelectedObj.GetComponents<InputKeyBinding>();
				InputKeyBinding[] array = components;
				foreach (InputKeyBinding inputKeyBinding in array)
				{
					if (inputKeyBinding.buttonName == "Submit")
					{
						inputKeyBinding.enabled = false;
					}
				}
			}
			else
			{
				lastSelectedObj.GetComponent<InputKeyBinding>().enabled = false;
			}
		}
		lastSelectedObj = currentObj;
	}

	public override GameObject DefaultSelectObj()
	{
		return (!m_IsButton) ? screenFancyScrollView.btnPrevious.gameObject : buttonFancyScrollView.btnPrevious.gameObject;
	}

	public override void OnDisablePnl()
	{
		string keyboardProposal = buttonProposal[buttonProposalIndex];
		string handleProposal = buttonProposal[screenProposalIndex];
		Singleton<DataManager>.instance["Account"]["IsReverse"].SetResult(m_IsReverse);
		Singleton<InputManager>.instance.OnSaveJsonProposal(keyboardProposal, handleProposal, m_IsVibration);
		Singleton<DataManager>.instance["Account"]["IsAutoFever"].SetResult(!m_IsManualFever);
		Singleton<DataManager>.instance["Account"].Save();
		Singleton<InputManager>.instance.Disable();
		if (Singleton<InputManager>.instance.currentControllerName != "Keyboard")
		{
			GameTouchPlay.isBtnLeftRight = m_IsReverse;
		}
		if (!m_Config.buttonKeyEnties["Custom"]["BattleAir"].Contains(KeyCode.Mouse1) && !m_Config.buttonKeyEnties["Custom"]["BattleGround"].Contains(KeyCode.Mouse1))
		{
			Singleton<InputManager>.instance.isCustomMouse = true;
		}
		else
		{
			Singleton<InputManager>.instance.isCustomMouse = false;
		}
	}

	private void SetSelectObjHighLight(FancyScrollView fsv, bool enable)
	{
		Color color = (!enable) ? normalColor : highLightColor;
		List<InputKeyBinding> list = new List<InputKeyBinding>();
		if (fsv == buttonFancyScrollView)
		{
			list.AddRange(buttonFancyScrollView.btnPrevious.GetComponents<InputKeyBinding>());
			list.AddRange(buttonFancyScrollView.btnNext.GetComponents<InputKeyBinding>());
		}
		else
		{
			list = GameUtils.FindObjectsOfType<InputKeyBinding>(fsv.transform);
		}
		foreach (InputKeyBinding item in list)
		{
			item.enabled = enable;
		}
		fsv.btnPrevious.GetComponent<Image>().color = color;
		fsv.btnNext.GetComponent<Image>().color = color;
		if (fsv == reverseFancyScrollView || fsv == vibrationFancyScrollView || fsv == keyboardFeverFancyScrollView || fsv == joystickFeverFancyScrollView)
		{
			int childCount = fsv.content.childCount;
			for (int i = 0; i < childCount; i++)
			{
				fsv.content.GetChild(i).GetComponent<Text>().color = color;
			}
			fsv.transform.GetChild(0).GetComponent<Text>().color = color;
			fsv.transform.GetChild(1).GetComponent<Image>().color = color;
		}
		if (fsv == keyboardFeverFancyScrollView || fsv == joystickFeverFancyScrollView)
		{
			SetFeverTipsBtnHighlight(fsv, color, enable);
		}
		if (!(fsv == buttonFancyScrollView) && !(fsv == screenFancyScrollView))
		{
			return;
		}
		int childCount2 = fsv.content.childCount;
		if (fsv.content.GetChild(0).name == "TxtTypeA")
		{
			for (int j = 0; j < childCount2; j++)
			{
				fsv.content.GetChild(j).GetComponent<Text>().color = color;
			}
		}
		else
		{
			for (int k = 0; k < childCount2; k++)
			{
				fsv.content.GetChild(k).GetComponent<PCControllerCell>().SetSelectColor(color);
			}
		}
		PointsBar pointsBar = (!(fsv == buttonFancyScrollView)) ? screenPointsBar : buttonPointsBar;
		pointsBar.SetCurrentPoint(enable);
	}

	private void SetFeverTipsBtnHighlight(FancyScrollView fsv, Color color, bool enable)
	{
		Transform transform = (!(fsv == keyboardFeverFancyScrollView)) ? joystickFeverTipsBtn : keyboardFeverTipsBtn;
		transform.GetChild(0).GetComponent<Image>().color = color;
		transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().color = color;
		transform.GetComponent<InputKeyBinding>().enabled = enable;
	}

	private void SetSelectObjHighLight(GameObject selectObj, bool enable)
	{
		Color color = (!enable) ? normalColor : highLightColor;
		selectObj.GetComponent<Text>().color = color;
	}

	private void SetModeTitleAnim()
	{
		txtButton.DOColor((!m_IsButton) ? new Color(1f, 1f, 1f, 0.5f) : Color.white, titleAnimTime);
		txtScreen.DOColor(m_IsButton ? new Color(1f, 1f, 1f, 0.5f) : Color.white, titleAnimTime);
		txtButton.transform.DOScale((!m_IsButton) ? new Vector3(0.85f, 0.85f, 1f) : Vector3.one, titleAnimTime);
		txtScreen.transform.DOScale(m_IsButton ? new Vector3(0.85f, 0.85f, 1f) : Vector3.one, titleAnimTime);
	}

	private void PlayAudio()
	{
	}

	public void OnClickBtnCustom()
	{
		buttonFancyScrollView.ScrollToDataIndex(4, 0.25f);
		if (!Singleton<InputManager>.instance.customIsChanged)
		{
			GetSourceKeyList();
		}
		keyboardTypeList[4].GetComponent<UISelectManage>().enabled = true;
		if ((bool)lastSelectedObj && (bool)lastSelectedObj.GetComponent<Text>())
		{
			SetSelectObjHighLight(lastSelectedObj, false);
		}
		if ((bool)customizeSelect.GetLastSelectObj())
		{
			customizeSelect.defaultSelect = customizeSelect.GetLastSelectObj();
		}
		if ((bool)onSelectAudio)
		{
			Singleton<AudioManager>.instance.PlayOneShot(onSelectAudio, Singleton<DataManager>.instance["GameConfig"]["SfxVolume"].GetResult<float>());
		}
		onCustomizeBg.DOFade(0.5f, 0.1f);
		onCustomizeBg.raycastTarget = true;
	}

	private void OnKeySetting(string buttonName)
	{
		PCCustomKeyCell component = m_CurrentSelectedCustomKey.GetComponent<PCCustomKeyCell>();
		string keyName = string.Empty;
		if (component.imgSpecialButton.gameObject.activeSelf)
		{
			keyName = component.imgSpecialButton.sprite.name;
			component.imgSpecialButton.gameObject.SetActive(false);
		}
		else if (component.txtKey.gameObject.activeSelf)
		{
			keyName = component.txtKey.text;
			component.txtKey.gameObject.SetActive(false);
		}
		component.objImg.DOFade(0f, 0f);
		component.txtKeySetting.gameObject.SetActive(true);
		Singleton<InputManager>.instance.OnClickChangeButton(buttonName, keyName);
	}

	public void SetCurrentCustomKey(GameObject obj)
	{
		m_CurrentSelectedCustomKey = obj;
		for (int i = 0; i < m_CustomKeyList.Count; i++)
		{
			if (m_CurrentSelectedCustomKey == m_CustomKeyList[i])
			{
				currentSelectedCustomKeyIndex = i;
			}
		}
		if (m_CurrentSelectedCustomKey == feverCustomKey.gameObject)
		{
			currentSelectedCustomKeyIndex = 8;
		}
		if (m_CurrentSelectedCustomKey.GetComponent<PCCustomKeyCell>().GetBtnType() == keyType.NoKey)
		{
			if (m_CurrentSelectedCustomKey.transform.parent.name == "Air")
			{
				Singleton<InputManager>.instance.OnClickNoKeyChangeButton("BattleAir");
			}
			else if (m_CurrentSelectedCustomKey.transform.parent.name == "Ground")
			{
				Singleton<InputManager>.instance.OnClickNoKeyChangeButton("BattleGround");
			}
			else if (m_CurrentSelectedCustomKey.transform.parent.name == "Fever")
			{
				Singleton<InputManager>.instance.OnClickNoKeyChangeButton("Fever");
			}
		}
		else if (m_CurrentSelectedCustomKey.transform.parent.name == "Air")
		{
			OnKeySetting("BattleAir");
		}
		else if (m_CurrentSelectedCustomKey.transform.parent.name == "Ground")
		{
			OnKeySetting("BattleGround");
		}
		else if (m_CurrentSelectedCustomKey.transform.parent.name == "Fever")
		{
			OnKeySetting("Fever");
		}
	}

	public void ChangeCustomKeyUI(string key)
	{
		if (key != "UpArrow" && key != "DownArrow" && key != "LeftArrow" && key != "RightArrow" && key != "Mouse0" && key != "Mouse1")
		{
			if (key == "None")
			{
				m_CurrentSelectedCustomKey.GetComponent<PCCustomKeyCell>().SetKeyToNo();
			}
			else
			{
				m_CurrentSelectedCustomKey.GetComponent<PCCustomKeyCell>().SetKeyToKey(SimplifyKey(key));
			}
		}
		else
		{
			m_CurrentSelectedCustomKey.GetComponent<PCCustomKeyCell>().SetKeyToSpecial(key);
		}
	}

	public void ChangeCustomKeyUI(GameObject obj, string key)
	{
		if (key != "UpArrow" && key != "DownArrow" && key != "LeftArrow" && key != "RightArrow" && key != "Mouse0" && key != "Mouse1")
		{
			if (key == "None")
			{
				obj.GetComponent<PCCustomKeyCell>().SetKeyToNo();
			}
			else
			{
				obj.GetComponent<PCCustomKeyCell>().SetKeyToKey(SimplifyKey(key));
			}
		}
		else
		{
			obj.GetComponent<PCCustomKeyCell>().SetKeyToSpecial(key);
		}
	}

	public void ChangeCutomKeyListUI()
	{
		for (int i = 0; i < m_CustomKeyList.Count; i++)
		{
			int num = i;
			if (i <= 3)
			{
				ChangeCustomKeyUI(m_CustomKeyList[i], m_Config.buttonKeyEnties["Custom"]["BattleAir"][i].ToString());
				continue;
			}
			num -= 4;
			ChangeCustomKeyUI(m_CustomKeyList[i], m_Config.buttonKeyEnties["Custom"]["BattleGround"][num].ToString());
		}
		ChangeCustomKeyUI(feverCustomKey.gameObject, m_Config.buttonKeyEnties["Custom"]["Fever"][0].ToString());
	}

	private void GetSourceKeyList()
	{
		m_SourceKey_Dict = m_Config.buttonKeyEnties[currentButtonProposal];
		SetCustomKeyList();
	}

	public void SetCustomKeyList()
	{
		for (int i = 0; i < m_SourceKey_Dict["BattleAir"].Count; i++)
		{
			m_Config.buttonKeyEnties["Custom"]["BattleAir"][i] = m_SourceKey_Dict["BattleAir"][i];
			if (m_SourceKey_Dict["BattleAir"][i].ToString() != "None")
			{
				ChangeCustomKeyUI(m_CustomKeyList[i], m_SourceKey_Dict["BattleAir"][i].ToString());
			}
		}
		for (int j = 0; j < m_SourceKey_Dict["BattleGround"].Count; j++)
		{
			m_Config.buttonKeyEnties["Custom"]["BattleGround"][j] = m_SourceKey_Dict["BattleGround"][j];
			if (m_SourceKey_Dict["BattleAir"][j].ToString() != "None")
			{
				ChangeCustomKeyUI(m_CustomKeyList[j + 4], m_SourceKey_Dict["BattleGround"][j].ToString());
			}
		}
		m_Config.buttonKeyEnties["Custom"]["Fever"][0] = m_SourceKey_Dict["Fever"][0];
		ChangeCustomKeyUI(feverCustomKey.gameObject, m_SourceKey_Dict["Fever"][0].ToString());
		SetSourceRepeatList();
	}

	public void SetRepeatKeyToNoUI(int repeatIndex)
	{
		if (repeatIndex >= m_CustomKeyList.Count)
		{
			feverCustomKey.GetComponent<PCCustomKeyCell>().SetKeyToNo();
		}
		else
		{
			m_CustomKeyList[repeatIndex].GetComponent<PCCustomKeyCell>().SetKeyToNo();
		}
	}

	public void SetSourceRepeatList()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < m_SourceKey_Dict["BattleAir"].Count; i++)
		{
			list.Add(m_SourceKey_Dict["BattleAir"][i].ToString());
		}
		for (int j = 0; j < m_SourceKey_Dict["BattleGround"].Count; j++)
		{
			list.Add(m_SourceKey_Dict["BattleGround"][j].ToString());
		}
		list.Add(m_SourceKey_Dict["Fever"][0].ToString());
		Singleton<InputManager>.instance.OnSetCustomRepeatList(list);
	}

	private void OnSelectObject()
	{
		m_CustomKeyList[0].GetComponent<PCCustomKeyCell>().imgSelected.gameObject.SetActive(true);
	}

	public void OnCancelCustomize()
	{
		EventSystem.current.SetSelectedGameObject(buttonFancyScrollView.btnPrevious.gameObject);
		SetSelectObjHighLight(buttonFancyScrollView, true);
		onCustomizeBg.DOFade(0f, 0.1f);
		onCustomizeBg.raycastTarget = false;
	}

	public string SimplifyKey(string keyName)
	{
		string text = keyName;
		if (m_SimplifyKeyDict.ContainsKey(text))
		{
			text = m_SimplifyKeyDict[text];
		}
		return text;
	}

	private void SetCustomKeyNavigation()
	{
		Navigation navigation = m_CustomKeyList[3].GetComponent<Button>().navigation;
		navigation.mode = Navigation.Mode.Explicit;
		Navigation navigation2 = m_CustomKeyList[4].GetComponent<Button>().navigation;
		navigation2.mode = Navigation.Mode.Explicit;
		if (feverCustomKey.gameObject.activeSelf)
		{
			navigation.selectOnRight = feverCustomKey;
			navigation2.selectOnLeft = feverCustomKey;
		}
		else
		{
			navigation.selectOnRight = m_CustomKeyList[4].GetComponent<Selectable>();
			navigation2.selectOnLeft = m_CustomKeyList[3].GetComponent<Selectable>();
		}
		m_CustomKeyList[3].GetComponent<Button>().navigation = navigation;
		m_CustomKeyList[4].GetComponent<Button>().navigation = navigation2;
	}

	private void SetFeverUI(bool enable)
	{
		feverCustomKey.gameObject.SetActive(enable);
		feverBase.SetActive(enable);
		SetOtherDefaultFeverBtns(enable);
	}

	private void SetOtherDefaultFeverBtns(bool enable)
	{
		for (int i = 0; i < otherDefaultFeverBtn.Count; i++)
		{
			otherDefaultFeverBtn[i].SetActive(enable);
		}
	}
}
