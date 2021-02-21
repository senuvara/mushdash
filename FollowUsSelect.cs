using Assets.Scripts.PeroTools.Commons;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowUsSelect : UISelectManage
{
	public Transform buttons;

	private List<Button> m_Buttons = new List<Button>();

	public override void OnInit()
	{
		m_Buttons = GameUtils.FindObjectsOfType<Button>(buttons);
		defaultSelect.transform.Find("ImgSelected").gameObject.SetActive(true);
	}

	public override void OnSelect(GameObject currentObj)
	{
		currentObj.transform.Find("ImgSelected").gameObject.SetActive(true);
		if ((bool)lastSelectedObj)
		{
			lastSelectedObj.transform.Find("ImgSelected").gameObject.SetActive(false);
		}
	}

	public override List<GameObject> SetSelectableObj()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < m_Buttons.Count; i++)
		{
			list.Add(m_Buttons[i].gameObject);
		}
		return list;
	}

	public override Transform SetEdgeObj(GameObject currentObj)
	{
		return currentObj.transform.Find("ImgSelected");
	}
}
