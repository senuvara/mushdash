using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.UI;
using DG.Tweening;
using DG.Tweening.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISelectManage : SerializedMonoBehaviour
{
	public class CloseButton
	{
		public Button button;

		public int popupCall;

		public CloseButton(Button btn, int call)
		{
			button = btn;
			popupCall = call;
		}
	}

	[Tooltip("指定此面板的层级，处于相应层级可以触发按钮和导航\nPopup:弹窗,关闭此面板时返回之前的面板层级")]
	public PanelType panel;

	[ShowIf("m_IsPopup", true)]
	[Tooltip("出现多个连续弹窗时，需要勾选此选项")]
	public bool isStackPopup;

	[ShowIf("isStackPopup", true)]
	[Tooltip("指定关闭当前面板的按钮和关闭后回到指定面板需要的次数")]
	public List<CloseButton> closeButtons = new List<CloseButton>();

	[ShowIf("isStackPopup", true)]
	[Tooltip("指定关闭当前所有弹窗面板的按钮")]
	public List<Button> closeAllButtons = new List<Button>();

	[FoldoutGroup("Navigation Funciton", 0)]
	public bool hasNavigationFunciton;

	[ShowIf("hasNavigationFunciton", true)]
	[FoldoutGroup("Navigation Funciton", 0)]
	public GameObject defaultSelect;

	[ShowIf("hasNavigationFunciton", true)]
	[FoldoutGroup("Navigation Funciton", 0)]
	[Tooltip("是否固定焦点在默认物体")]
	public bool forceOnDefaultSelect;

	[ShowIf("hasNavigationFunciton", true)]
	[FoldoutGroup("Navigation Funciton", 0)]
	[Tooltip("指定需要屏蔽的面板")]
	public UISelectManage deactivatePanel;

	[ShowIf("hasNavigationFunciton", true)]
	[FoldoutGroup("Navigation Funciton", 0)]
	public AudioClip onSelectAudio;

	[ShowIf("m_IsHasSelectAudio", true)]
	[FoldoutGroup("Navigation Funciton", 0)]
	public bool playSelectAudioOnInit;

	private bool m_PlayOnInit;

	[FoldoutGroup("Button Blinding", 0)]
	public bool hasButtonBlinding;

	[ShowIf("hasButtonBlinding", true)]
	[FoldoutGroup("Button Blinding", 0)]
	public string buttonUp;

	[ShowIf("hasButtonBlinding", true)]
	[FoldoutGroup("Button Blinding", 0)]
	public string buttonDown;

	[ShowIf("hasButtonBlinding", true)]
	[FoldoutGroup("Button Blinding", 0)]
	public string buttonLeft;

	[ShowIf("hasButtonBlinding", true)]
	[FoldoutGroup("Button Blinding", 0)]
	public string buttonRight;

	[FoldoutGroup("Edge Anim", 0)]
	public bool hasEdgeAnim;

	[ShowIf("hasEdgeAnim", true)]
	[FoldoutGroup("Edge Anim", 0)]
	[Tooltip("\"X\"代表振幅\n\"Y\"代表时长")]
	public List<Vector2> edgeAnimManager;

	[ShowIf("hasEdgeAnim", true)]
	[FoldoutGroup("Edge Anim", 0)]
	public AudioClip onEdgeAudio;

	protected bool isOnEdgeAnim;

	private ButtonDirection m_NullDir;

	private bool m_IsGetAxis;

	private Sequence m_Sequence;

	private Vector3 m_OrgPos;

	private bool m_IsExcuteAnim;

	private bool[] m_FunctionBools = new bool[3];

	protected GameObject lastSelectedObj;

	private List<GameObject> m_SelectableObj;

	private bool m_IsPopup()
	{
		return (panel & PanelType.Popup) != 0;
	}

	private bool m_IsHasSelectAudio()
	{
		return onSelectAudio != null;
	}

	private void Awake()
	{
		m_FunctionBools = new bool[3]
		{
			hasNavigationFunciton,
			hasButtonBlinding,
			hasEdgeAnim
		};
	}

	private void Start()
	{
		SetPanelBindings();
		if (isStackPopup)
		{
			if (closeButtons != null)
			{
				for (int j = 0; j < closeButtons.Count; j++)
				{
					int i1 = j;
					closeButtons[j].button.onClick.AddListener(delegate
					{
						CloseStackPopup(closeButtons[i1].popupCall);
					});
				}
			}
			if (closeAllButtons != null)
			{
				for (int k = 0; k < closeAllButtons.Count; k++)
				{
					closeAllButtons[k].onClick.AddListener(CloseAllStackPopup);
				}
			}
		}
		if (!hasNavigationFunciton)
		{
			return;
		}
		OnInit();
		m_SelectableObj = SetSelectableObj();
		lastSelectedObj = defaultSelect;
		m_PlayOnInit = true;
		if (!(EventSystem.current == null))
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject != null)
			{
				SetKeyBindings(currentSelectedGameObject, true, true);
			}
		}
	}

	private void Update()
	{
		OnUpdatePnl();
		if (hasButtonBlinding)
		{
			if (!string.IsNullOrEmpty(buttonUp) && Input.GetButtonDown(buttonUp))
			{
				GetButtonDown(ButtonDirection.Up);
			}
			if (!string.IsNullOrEmpty(buttonDown) && Input.GetButtonDown(buttonDown))
			{
				GetButtonDown(ButtonDirection.Down);
			}
			if (!string.IsNullOrEmpty(buttonLeft) && Input.GetButtonDown(buttonLeft))
			{
				GetButtonDown(ButtonDirection.Left);
			}
			if (!string.IsNullOrEmpty(buttonRight) && Input.GetButtonDown(buttonRight))
			{
				GetButtonDown(ButtonDirection.Right);
			}
		}
		if (!hasNavigationFunciton)
		{
			return;
		}
		if (EventSystem.current == null)
		{
			return;
		}
		GameObject current = EventSystem.current.currentSelectedGameObject;
		ButtonDirection axisDown = GetAxisDown(delegate(ButtonDirection d)
		{
			if ((d & m_NullDir) != 0)
			{
				ExcuteEdgeAnim(lastSelectedObj, d);
			}
			else if (Singleton<InputManager>.instance.currentControllerName != "Keyboard")
			{
				MoveToSelectable(current.GetComponent<Selectable>(), d);
			}
		});
		if (current == null || lastSelectedObj == current)
		{
			return;
		}
		if (current.activeSelf && IsOnSelectableObj())
		{
			if (lastSelectedObj != null)
			{
				SetKeyBindings(lastSelectedObj, false);
			}
			OnSelect(current);
			if (!playSelectAudioOnInit && m_PlayOnInit)
			{
				m_PlayOnInit = false;
			}
			else
			{
				PlayAudio(onSelectAudio);
			}
			SetKeyBindings(current, true);
			lastSelectedObj = current;
			m_NullDir = CheckDirIsNull(lastSelectedObj);
		}
		else if (!(lastSelectedObj == null))
		{
			if (axisDown != 0 && !m_IsExcuteAnim)
			{
				m_IsExcuteAnim = true;
				ExcuteEdgeAnim(lastSelectedObj, axisDown);
			}
			EventSystem.current.SetSelectedGameObject(lastSelectedObj);
		}
	}

	private void OnEnable()
	{
		OnEnablePnl();
		PanelManage.PushPanel(panel, this);
		if (!hasNavigationFunciton)
		{
			return;
		}
		if (IsOnCurrentPanel())
		{
			GameObject gameObject = DefaultSelectObj();
			if (gameObject == null)
			{
				if (defaultSelect != null)
				{
					EventSystem.current.SetSelectedGameObject(defaultSelect);
				}
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(gameObject);
			}
		}
		if (deactivatePanel != null)
		{
			deactivatePanel.SetEnable(false);
		}
	}

	private void OnDisable()
	{
		OnDisablePnl();
		if ((panel & PanelType.Popup) != 0 && !isStackPopup)
		{
			PanelManage.PopPanel();
		}
		if (hasNavigationFunciton && !isStackPopup && deactivatePanel != null)
		{
			deactivatePanel.SetEnable(true);
		}
	}

	private bool IsOnCurrentPanel()
	{
		return (PanelManage.panel.Peek() & panel) != 0 && (PanelManage.panel.Peek() & panel) != PanelType.Popup;
	}

	public void SetEnable(bool enable, bool needToSetDefault = true)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		if (!enable)
		{
			hasNavigationFunciton = false;
			hasButtonBlinding = false;
			hasEdgeAnim = false;
			return;
		}
		hasNavigationFunciton = m_FunctionBools[0];
		hasButtonBlinding = m_FunctionBools[1];
		hasEdgeAnim = m_FunctionBools[2];
		if (needToSetDefault && EventSystem.current != null)
		{
			EventSystem.current.SetSelectedGameObject(DefaultSelectObj());
		}
	}

	private bool IsOnSelectableObj()
	{
		if (m_SelectableObj == null)
		{
			return false;
		}
		foreach (GameObject item in m_SelectableObj)
		{
			if (EventSystem.current.currentSelectedGameObject == item)
			{
				return true;
			}
		}
		return false;
	}

	protected void SetPanelBindings()
	{
		List<InputKeyBinding> list = GameUtils.FindObjectsOfType<InputKeyBinding>(base.transform);
		foreach (InputKeyBinding item in list)
		{
			if (item.GetComponent<UISelectManage>() == null || item.GetComponent<UISelectManage>() == this)
			{
				item.SetPanelOrder(panel);
			}
		}
		List<InputAxisBinding> list2 = GameUtils.FindObjectsOfType<InputAxisBinding>(base.transform);
		foreach (InputAxisBinding item2 in list2)
		{
			if (item2.GetComponent<UISelectManage>() == null || item2.GetComponent<UISelectManage>() == this)
			{
				item2.SetPanelOrder(panel);
			}
		}
	}

	public virtual void SetKeyBindings(GameObject obj, bool enable, bool isStart = false)
	{
		if (!obj.GetComponent<InputKeyBinding>())
		{
			return;
		}
		if (obj.GetComponents<InputKeyBinding>().Length > 1)
		{
			InputKeyBinding[] components = obj.GetComponents<InputKeyBinding>();
			InputKeyBinding[] array = components;
			foreach (InputKeyBinding inputKeyBinding in array)
			{
				if (inputKeyBinding.buttonName == "Submit")
				{
					inputKeyBinding.enabled = enable;
				}
			}
		}
		else
		{
			InputKeyBinding component = obj.GetComponent<InputKeyBinding>();
			if (component.buttonName == "Submit")
			{
				component.enabled = enable;
			}
		}
	}

	protected void SetSelectableObjList(List<GameObject> objs)
	{
		m_SelectableObj = objs;
	}

	protected void CloseStackPopup(int popupCall)
	{
		if ((bool)deactivatePanel)
		{
			deactivatePanel.SetEnable(true);
		}
		for (int i = 0; i < popupCall; i++)
		{
			PanelManage.PopPanel();
		}
		Debug.LogFormat("[{1}] CloseStackPopup current [{0}]", PanelManage.panel.Peek(), base.gameObject.name);
	}

	protected void CloseAllStackPopup()
	{
		if ((bool)deactivatePanel)
		{
			deactivatePanel.SetEnable(true);
		}
		while ((PanelManage.panel.Peek() & PanelType.Popup) != 0)
		{
			PanelManage.PopPanel();
		}
		Debug.LogFormat("[{1}] CloseAllStackPopup current [{0}]", PanelManage.panel.Peek(), base.gameObject.name);
	}

	protected void GetButtonDown(ButtonDirection dir)
	{
		if (!(EventSystem.current == null))
		{
			Selectable currentSelectable = GetCurrentSelectable(EventSystem.current.currentSelectedGameObject);
			switch (dir)
			{
			case ButtonDirection.Up | ButtonDirection.Down:
			case ButtonDirection.Up | ButtonDirection.Left:
			case ButtonDirection.Down | ButtonDirection.Left:
			case ButtonDirection.Up | ButtonDirection.Down | ButtonDirection.Left:
				break;
			case ButtonDirection.Up:
				MoveToSelectable(currentSelectable, ButtonDirection.Up);
				break;
			case ButtonDirection.Down:
				MoveToSelectable(currentSelectable, ButtonDirection.Down);
				break;
			case ButtonDirection.Left:
				MoveToSelectable(currentSelectable, ButtonDirection.Left);
				break;
			case ButtonDirection.Right:
				MoveToSelectable(currentSelectable, ButtonDirection.Right);
				break;
			}
		}
	}

	private Selectable GetCurrentSelectable(GameObject obj)
	{
		Selectable result = null;
		Button component = obj.GetComponent<Button>();
		if (component != null)
		{
			result = component;
		}
		Toggle component2 = obj.GetComponent<Toggle>();
		if (component2 != null)
		{
			result = component2;
		}
		Slider component3 = obj.GetComponent<Slider>();
		if (component3 != null)
		{
			result = component3;
		}
		return result;
	}

	private void MoveToSelectable(Selectable obj, ButtonDirection dir)
	{
		Selectable selectable = FindSelect(obj, dir);
		if (m_SelectableObj != null)
		{
			if (selectable != null && m_SelectableObj.Contains(selectable.gameObject))
			{
				EventSystem.current.SetSelectedGameObject(selectable.gameObject);
			}
			else if (dir != 0)
			{
				ExcuteEdgeAnim(lastSelectedObj, dir);
			}
		}
	}

	private Selectable FindSelect(Selectable obj, ButtonDirection dir)
	{
		if (obj == null)
		{
			return null;
		}
		Navigation navigation = obj.navigation;
		Selectable result = null;
		if (navigation.mode == Navigation.Mode.Explicit)
		{
			if (dir == ButtonDirection.Up)
			{
				result = navigation.selectOnUp;
			}
			if (dir == ButtonDirection.Down)
			{
				result = navigation.selectOnDown;
			}
			if (dir == ButtonDirection.Left)
			{
				result = navigation.selectOnLeft;
			}
			if (dir == ButtonDirection.Right)
			{
				result = navigation.selectOnRight;
			}
		}
		else
		{
			if (dir == ButtonDirection.Up)
			{
				result = obj.FindSelectableOnUp();
			}
			if (dir == ButtonDirection.Down)
			{
				result = obj.FindSelectableOnDown();
			}
			if (dir == ButtonDirection.Left)
			{
				result = obj.FindSelectableOnLeft();
			}
			if (dir == ButtonDirection.Right)
			{
				result = obj.FindSelectableOnRight();
			}
		}
		return result;
	}

	private ButtonDirection AxisToButton(float h, float v)
	{
		if (h > 0.7f)
		{
			return ButtonDirection.Right;
		}
		if (h < -0.7f)
		{
			return ButtonDirection.Left;
		}
		if (v > 0.7f)
		{
			return ButtonDirection.Up;
		}
		if (v < -0.7f)
		{
			return ButtonDirection.Down;
		}
		return ButtonDirection.None;
	}

	private ButtonDirection GetAxisDown(Action<ButtonDirection> onClick)
	{
		ButtonDirection buttonDirection = AxisToButton(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if (Singleton<InputManager>.instance.currentControllerName != "Keyboard")
		{
			buttonDirection = ((!(Singleton<InputManager>.instance.currentControllerName == "YuanCon")) ? AxisToButton(Singleton<InputManager>.instance.RewiredGetAxisRaw("Horizontal"), Singleton<InputManager>.instance.RewiredGetAxisRaw("Vertical")) : ((base.gameObject.name == "PnlAchv") ? AxisToButton(0f, 0f - Singleton<InputManager>.instance.RewiredGetAxisRaw("Horizontal")) : ((!(base.gameObject.name == "PnlBulletin")) ? AxisToButton(Singleton<InputManager>.instance.RewiredGetAxisRaw("Horizontal"), 0f) : AxisToButton(Singleton<InputManager>.instance.RewiredGetAxisRaw("Horizontal"), Singleton<InputManager>.instance.RewiredGetAxisRaw("Vertical")))));
		}
		if (buttonDirection != 0)
		{
			if (!m_IsGetAxis)
			{
				m_IsGetAxis = true;
				onClick(buttonDirection);
			}
		}
		else
		{
			m_IsGetAxis = false;
			m_IsExcuteAnim = false;
		}
		return buttonDirection;
	}

	private ButtonDirection CheckDirIsNull(GameObject obj)
	{
		ButtonDirection[] array = new ButtonDirection[4]
		{
			ButtonDirection.Up,
			ButtonDirection.Down,
			ButtonDirection.Left,
			ButtonDirection.Right
		};
		ButtonDirection buttonDirection = ButtonDirection.None;
		for (int i = 0; i < array.Length; i++)
		{
			Selectable x = FindSelect(GetCurrentSelectable(obj), array[i]);
			if (x == null)
			{
				buttonDirection = (ButtonDirection)((int)buttonDirection + (int)array[i]);
			}
		}
		return buttonDirection;
	}

	private void ExcuteEdgeAnim(GameObject current, ButtonDirection dir)
	{
		if (!hasEdgeAnim)
		{
			return;
		}
		Transform obj = SetEdgeObj(current);
		if (obj == null)
		{
			return;
		}
		if (isOnEdgeAnim)
		{
			CleanEdgeAnim(current);
		}
		else
		{
			isOnEdgeAnim = true;
		}
		PlayAudio(onEdgeAudio);
		m_OrgPos = obj.localPosition;
		m_Sequence = DOTween.Sequence();
		for (int i = 0; i < edgeAnimManager.Count; i++)
		{
			Vector3 orgPos = m_OrgPos;
			Vector2 vector = edgeAnimManager[i];
			Vector3 endValue = orgPos + GetEdgeAnimPos(dir, vector.x);
			Sequence sequence = m_Sequence;
			DOGetter<Vector3> getter = () => obj.localPosition;
			DOSetter<Vector3> setter = delegate(Vector3 value)
			{
				obj.localPosition = value;
			};
			Vector2 vector2 = edgeAnimManager[i];
			sequence.Append(DOTween.To(getter, setter, endValue, vector2.y));
		}
		m_Sequence.AppendCallback(delegate
		{
			isOnEdgeAnim = false;
			CleanEdgeAnim(current);
		});
	}

	protected Vector3 GetEdgeAnimPos(ButtonDirection dir, float vaule)
	{
		Vector3 result = Vector3.zero;
		switch (dir)
		{
		case ButtonDirection.Up:
			result = new Vector3(0f, vaule);
			break;
		case ButtonDirection.Down:
			result = new Vector3(0f, 0f - vaule);
			break;
		case ButtonDirection.Left:
			result = new Vector3(0f - vaule, 0f);
			break;
		case ButtonDirection.Right:
			result = new Vector3(vaule, 0f);
			break;
		}
		return result;
	}

	protected void CleanEdgeAnim(GameObject current)
	{
		m_Sequence.Pause();
		m_Sequence.Kill();
		Transform transform = SetEdgeObj(current);
		if (transform != null)
		{
			transform.localPosition = m_OrgPos;
		}
	}

	private void PlayAudio(AudioClip clip)
	{
		if (!(clip == null))
		{
			Singleton<AudioManager>.instance.PlayOneShot(clip, Singleton<DataManager>.instance["GameConfig"]["SfxVolume"].GetResult<float>());
		}
	}

	public virtual void OnInit()
	{
	}

	public virtual GameObject DefaultSelectObj()
	{
		return null;
	}

	public virtual void OnSelect(GameObject currentObj)
	{
	}

	public virtual Transform SetEdgeObj(GameObject currentObj)
	{
		return null;
	}

	public virtual List<GameObject> SetSelectableObj()
	{
		if (forceOnDefaultSelect && defaultSelect != null)
		{
			List<GameObject> list = new List<GameObject>();
			list.Add(defaultSelect);
			return list;
		}
		return null;
	}

	public virtual void OnEnablePnl()
	{
	}

	public virtual void OnDisablePnl()
	{
	}

	public virtual void OnUpdatePnl()
	{
	}

	public virtual void OnCheckNullDir()
	{
		if (!(EventSystem.current == null))
		{
			m_NullDir = CheckDirIsNull(EventSystem.current.currentSelectedGameObject);
		}
	}
}
