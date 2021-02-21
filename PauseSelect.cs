using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseSelect : UISelectManage
{
	public Transform buttons;

	public override void OnSelect(GameObject currentObj)
	{
		lastSelectedObj.transform.GetChild(0).gameObject.SetActive(false);
		currentObj.transform.GetChild(0).gameObject.SetActive(true);
	}

	public override List<GameObject> SetSelectableObj()
	{
		List<Button> list = GameUtils.FindObjectsOfType<Button>(buttons);
		List<GameObject> list2 = new List<GameObject>();
		for (int i = 0; i < list.Count; i++)
		{
			list2.Add(list[i].gameObject);
		}
		return list2;
	}

	public override void OnEnablePnl()
	{
		defaultSelect.transform.GetChild(0).gameObject.SetActive(true);
	}

	public override void SetKeyBindings(GameObject obj, bool enable, bool isStart = false)
	{
		if (!obj.GetComponent<InputKeyBinding>())
		{
			return;
		}
		if (obj.GetComponents<InputKeyBinding>().Length > 1)
		{
			if (obj.name == "BtnResume" && isStart && enable)
			{
				return;
			}
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
			obj.GetComponent<InputKeyBinding>().enabled = enable;
		}
	}

	public override Transform SetEdgeObj(GameObject currentObj)
	{
		return currentObj.transform.GetChild(0);
	}
}
