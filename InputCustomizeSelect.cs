using Assets.Scripts.PeroTools.Commons;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputCustomizeSelect : UISelectManage
{
	private List<GameObject> m_Buttons = new List<GameObject>();

	public Transform buttons;

	public Button cancel;

	private int m_LastSelectButtonIndex;

	public Button empty;

	public override void OnInit()
	{
		cancel.onClick.AddListener(OnClickCancel);
	}

	public override GameObject DefaultSelectObj()
	{
		Debug.Log("Custom Default Select " + defaultSelect);
		defaultSelect.GetComponent<PCCustomKeyCell>().imgSelected.gameObject.SetActive(true);
		return null;
	}

	public override void OnEnablePnl()
	{
		cancel.gameObject.SetActive(true);
	}

	public override void OnDisablePnl()
	{
		m_LastSelectButtonIndex = m_Buttons.IndexOf(lastSelectedObj);
	}

	public override List<GameObject> SetSelectableObj()
	{
		List<Button> list = GameUtils.FindObjectsOfType<Button>(buttons);
		for (int i = 0; i < list.Count; i++)
		{
			m_Buttons.Add(list[i].gameObject);
		}
		return m_Buttons;
	}

	public override void OnSelect(GameObject currentObj)
	{
		currentObj.GetComponent<PCCustomKeyCell>().imgSelected.gameObject.SetActive(true);
		if ((bool)lastSelectedObj)
		{
			lastSelectedObj.GetComponent<PCCustomKeyCell>().imgSelected.gameObject.SetActive(false);
		}
	}

	private void OnClickCancel()
	{
		cancel.gameObject.SetActive(false);
		lastSelectedObj.GetComponent<PCCustomKeyCell>().imgSelected.gameObject.SetActive(false);
		PnlInputPc.Instance().OnCancelCustomize();
		base.enabled = false;
		Debug.Log("Cancel Customize");
	}

	public void OnCustomKey(bool enable)
	{
		EventSystem.current.SetSelectedGameObject((!enable) ? PnlInputPc.Instance().GetCurrentSelectedCustomKey() : empty.gameObject);
	}

	public GameObject GetLastSelectObj()
	{
		if (m_Buttons.Count <= 0)
		{
			return null;
		}
		return m_Buttons[m_LastSelectButtonIndex];
	}
}
