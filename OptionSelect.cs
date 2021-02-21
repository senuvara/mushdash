using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionSelect : UISelectManage
{
	private List<Button> m_SelectedButtonList;

	public Selectable offsetSelectable;

	public Selectable inputSelectable;

	public Selectable creditSelectable;

	public Selectable accountSelectable;

	public Selectable howToPlaySelectable;

	public Selectable followUsSelectable;

	public Selectable QASelectable;

	public Selectable bulletinSelectable;

	public Selectable displaySelectable;

	public GameObject terminalObj;

	public GameObject blankObj;

	public override void OnInit()
	{
		m_SelectedButtonList = GameUtils.FindObjectsOfType<Button>(base.transform);
		if (Singleton<InputManager>.instance.IsConnetShouTai())
		{
			Navigation navigation = offsetSelectable.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnLeft = inputSelectable;
			navigation.selectOnRight = creditSelectable;
			navigation.selectOnDown = accountSelectable;
			offsetSelectable.navigation = navigation;
			Navigation navigation2 = creditSelectable.navigation;
			navigation2.mode = Navigation.Mode.Explicit;
			navigation2.selectOnLeft = offsetSelectable;
			navigation2.selectOnRight = followUsSelectable;
			navigation2.selectOnUp = howToPlaySelectable;
			creditSelectable.navigation = navigation2;
		}
		terminalObj.SetActive(true);
		blankObj.SetActive(false);
		SetBtnNavi(QASelectable, "Right", terminalObj.GetComponent<Button>());
		SetBtnNavi(bulletinSelectable, "Left", terminalObj.GetComponent<Button>());
		SetBtnNavi(displaySelectable, "Down", terminalObj.GetComponent<Button>());
	}

	public override void OnEnablePnl()
	{
		if (m_SelectedButtonList != null && m_SelectedButtonList.Count != 0 && Singleton<InputManager>.instance.IsConnetShouTai())
		{
			Navigation navigation = offsetSelectable.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnLeft = inputSelectable;
			navigation.selectOnRight = creditSelectable;
			navigation.selectOnDown = accountSelectable;
			offsetSelectable.navigation = navigation;
			Navigation navigation2 = creditSelectable.navigation;
			navigation2.mode = Navigation.Mode.Explicit;
			navigation2.selectOnLeft = offsetSelectable;
			navigation2.selectOnRight = followUsSelectable;
			navigation2.selectOnUp = howToPlaySelectable;
			creditSelectable.navigation = navigation2;
		}
	}

	public override GameObject DefaultSelectObj()
	{
		if (PanelManage.optionSelectStack.Count > 0 && PanelManage.optionSelectStack.Peek() != null)
		{
			GameObject gameObject = PanelManage.optionSelectStack.Peek();
			gameObject.transform.Find("ImgSelected").gameObject.SetActive(true);
			return gameObject;
		}
		if (defaultSelect != null)
		{
			defaultSelect.transform.Find("ImgSelected").gameObject.SetActive(true);
			PanelManage.optionSelectStack.Push(defaultSelect);
		}
		return null;
	}

	public override List<GameObject> SetSelectableObj()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (Button selectedButton in m_SelectedButtonList)
		{
			if (selectedButton.transform.parent.name != "GmButtons")
			{
				list.Add(selectedButton.gameObject);
			}
		}
		return list;
	}

	public override void OnSelect(GameObject currentObj)
	{
		PanelManage.optionSelectStack.Push(currentObj);
		currentObj.transform.Find("ImgSelected").gameObject.SetActive(true);
		if ((bool)lastSelectedObj)
		{
			lastSelectedObj.transform.Find("ImgSelected").gameObject.SetActive(false);
		}
	}

	public override Transform SetEdgeObj(GameObject currentObj)
	{
		return (!(currentObj.transform.parent.name == "GmButtons")) ? currentObj.transform.Find("ImgSelected").transform : null;
	}

	public void SetBtnNavi(Selectable sourceBtn, string naviBtn, Selectable targetBtn)
	{
		Navigation navigation = sourceBtn.navigation;
		switch (naviBtn)
		{
		case "Up":
			navigation.selectOnUp = targetBtn;
			break;
		case "Down":
			navigation.selectOnDown = targetBtn;
			break;
		case "Left":
			navigation.selectOnLeft = targetBtn;
			break;
		case "Right":
			navigation.selectOnRight = targetBtn;
			break;
		}
		sourceBtn.navigation = navigation;
	}
}
