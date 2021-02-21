using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PnlBulletinSelect : UISelectManage
{
	public Transform tittleToggle;

	public GameObject scrollViewObj;

	public GameObject ScrollVertical;

	public Color yesColor;

	public Color noColor;

	private List<Toggle> m_TittleToggle;

	private int m_ToggleIndex;

	public override void OnInit()
	{
		m_TittleToggle = GameUtils.FindObjectsOfType<Toggle>(tittleToggle);
		for (int i = 0; i < m_TittleToggle.Count; i++)
		{
			Navigation navigation = m_TittleToggle[i].navigation;
			navigation.mode = Navigation.Mode.Explicit;
			if (i == 0)
			{
				navigation.selectOnUp = m_TittleToggle[m_TittleToggle.Count - 1];
				if (m_TittleToggle.Count > 1)
				{
					navigation.selectOnDown = m_TittleToggle[i + 1];
				}
			}
			else if (i == m_TittleToggle.Count - 1)
			{
				navigation.selectOnUp = m_TittleToggle[i - 1];
				navigation.selectOnDown = m_TittleToggle[0];
			}
			else
			{
				navigation.selectOnUp = m_TittleToggle[i - 1];
				navigation.selectOnDown = m_TittleToggle[i + 1];
			}
			navigation.selectOnRight = scrollViewObj.GetComponent<Button>();
			m_TittleToggle[i].navigation = navigation;
		}
		if (tittleToggle.childCount > 0)
		{
			defaultSelect = tittleToggle.GetChild(0).gameObject;
			if (EventSystem.current != null)
			{
				EventSystem.current.SetSelectedGameObject(defaultSelect);
			}
		}
		else
		{
			defaultSelect = base.gameObject;
			if (EventSystem.current != null)
			{
				EventSystem.current.SetSelectedGameObject(defaultSelect);
			}
		}
	}

	public override List<GameObject> SetSelectableObj()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < m_TittleToggle.Count; i++)
		{
			list.Add(m_TittleToggle[i].gameObject);
		}
		list.Add(scrollViewObj.gameObject);
		return list;
	}

	public override void OnSelect(GameObject currentObj)
	{
		for (int i = 0; i < m_TittleToggle.Count; i++)
		{
			if (currentObj == m_TittleToggle[i].gameObject)
			{
				m_ToggleIndex = i;
				currentObj.GetComponent<Toggle>().isOn = true;
				scrollViewObj.GetComponent<Outline>().enabled = false;
				m_TittleToggle[i].transform.GetChild(0).GetChild(1).GetComponent<Image>()
					.color = yesColor;
				for (int j = 0; j < ScrollVertical.GetComponents(typeof(InputKeyBinding)).Length; j++)
				{
					((InputKeyBinding)ScrollVertical.GetComponents(typeof(InputKeyBinding))[j]).enabled = false;
				}
				return;
			}
		}
		if (currentObj == scrollViewObj.gameObject && m_TittleToggle.Count > 0)
		{
			Navigation navigation = scrollViewObj.GetComponent<Button>().navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnLeft = m_TittleToggle[m_ToggleIndex];
			scrollViewObj.GetComponent<Button>().navigation = navigation;
			m_TittleToggle[m_ToggleIndex].transform.GetChild(0).GetChild(1).GetComponent<Image>()
				.color = noColor;
			scrollViewObj.GetComponent<Outline>().enabled = true;
			for (int k = 0; k < ScrollVertical.GetComponents(typeof(InputKeyBinding)).Length; k++)
			{
				((InputKeyBinding)ScrollVertical.GetComponents(typeof(InputKeyBinding))[k]).enabled = true;
			}
		}
	}

	public override void OnDisablePnl()
	{
		if (m_TittleToggle.Count == 0)
		{
			defaultSelect = base.gameObject;
			scrollViewObj.GetComponent<Outline>().enabled = false;
			return;
		}
		if (m_TittleToggle.Count == 1)
		{
			defaultSelect = m_TittleToggle[0].gameObject;
		}
		else if (m_ToggleIndex < m_TittleToggle.Count)
		{
			Toggle toggle = m_TittleToggle[m_ToggleIndex];
			if ((bool)toggle)
			{
				defaultSelect = toggle.gameObject;
			}
		}
		scrollViewObj.GetComponent<Outline>().enabled = false;
		if (m_ToggleIndex < m_TittleToggle.Count)
		{
			Toggle toggle2 = m_TittleToggle[m_ToggleIndex];
			if ((bool)toggle2)
			{
				toggle2.transform.GetChild(0).GetChild(1).GetComponent<Image>()
					.color = yesColor;
			}
		}
	}

	public void RefreshUISelectObj()
	{
		OnInit();
		SetSelectableObjList(SetSelectableObj());
	}
}
