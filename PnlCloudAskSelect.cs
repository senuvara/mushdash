using Assets.Scripts.PeroTools.Commons;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PnlCloudAskSelect : UISelectManage
{
	public Button btnYes;

	private Button m_CurrentSelect;

	public override void OnInit()
	{
		btnYes.onClick.AddListener(delegate
		{
			lastSelectedObj.GetComponent<Button>().onClick.Invoke();
		});
	}

	public override List<GameObject> SetSelectableObj()
	{
		List<GameObject> objs = new List<GameObject>();
		GameUtils.FindObjectsOfType<Button>(base.transform).For(delegate(Button btn)
		{
			objs.Add(btn.gameObject);
		});
		if (objs.Contains(btnYes.gameObject))
		{
			objs.Remove(btnYes.gameObject);
		}
		return objs;
	}

	public override void OnSelect(GameObject currentObj)
	{
		currentObj.transform.Find("ImgSelect").gameObject.SetActive(true);
		lastSelectedObj.transform.Find("ImgSelect").gameObject.SetActive(false);
	}
}
