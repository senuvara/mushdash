using Assets.Scripts.UI.Panels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffsetAskSelect : UISelectManage
{
	public Button tvMode;

	public Button handheldMode;

	public PnlOffsetOption offsetOption;

	public override List<GameObject> SetSelectableObj()
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(tvMode.gameObject);
		list.Add(handheldMode.gameObject);
		return list;
	}

	public override GameObject DefaultSelectObj()
	{
		GameObject gameObject = handheldMode.gameObject;
		gameObject.transform.Find("ImgSelected").gameObject.SetActive(true);
		return gameObject;
	}

	public override void OnSelect(GameObject currentObj)
	{
		currentObj.transform.Find("ImgSelected").gameObject.SetActive(true);
		if (lastSelectedObj != null)
		{
			lastSelectedObj.transform.Find("ImgSelected").gameObject.SetActive(false);
		}
		offsetOption.isTvMode = (currentObj.GetComponent<Button>() == tvMode);
	}
}
