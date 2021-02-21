using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeSelect : UISelectManage
{
	public Color highLight;

	public Color normal;

	private List<Slider> m_SliderList = new List<Slider>();

	private Transform m_HandleBtnNext;

	private Transform m_HandleBtnPrevious;

	private GameObject m_LastSelectedObj;

	public override GameObject DefaultSelectObj()
	{
		if (m_SliderList.Count > 0)
		{
			SetSelectImage(m_SliderList[0].gameObject);
			return m_SliderList[0].gameObject;
		}
		return null;
	}

	public override void OnSelect(GameObject currentObj)
	{
		SetSelectImage(currentObj);
	}

	public override void OnInit()
	{
		m_SliderList = GameUtils.FindObjectsOfType<Slider>(base.transform);
		foreach (Slider slider in m_SliderList)
		{
			Slider slider2 = slider;
			slider.targetGraphic.GetComponent<Image>().color = normal;
			Button component = slider.transform.parent.Find("BtnNext").GetComponent<Button>();
			component.GetComponent<Image>().color = normal;
			component.onClick.AddListener(delegate
			{
				SetSelectImage(slider2.gameObject);
				EventSystem.current.SetSelectedGameObject(slider.gameObject);
			});
			component = slider.transform.parent.Find("BtnPrevious").GetComponent<Button>();
			component.GetComponent<Image>().color = normal;
			component.onClick.AddListener(delegate
			{
				SetSelectImage(slider2.gameObject);
				EventSystem.current.SetSelectedGameObject(slider.gameObject);
			});
		}
		DefaultSelectObj();
	}

	public override List<GameObject> SetSelectableObj()
	{
		List<GameObject> list = new List<GameObject>();
		foreach (Slider slider in m_SliderList)
		{
			list.Add(slider.gameObject);
		}
		return list;
	}

	private void SetSelectImage(GameObject currentObj)
	{
		foreach (Slider slider in m_SliderList)
		{
			bool flag = slider.gameObject == currentObj;
			slider.targetGraphic.GetComponent<Image>().color = ((!flag) ? normal : highLight);
			Transform transform = slider.transform.parent.Find("BtnNext");
			transform.GetComponent<Image>().color = ((!flag) ? normal : highLight);
			transform.GetComponent<InputKeyBinding>().enabled = flag;
			Transform transform2 = slider.transform.parent.Find("BtnPrevious");
			transform2.GetComponent<Image>().color = ((!flag) ? normal : highLight);
			transform2.GetComponent<InputKeyBinding>().enabled = flag;
			InputKeyBinding[] components = slider.GetComponents<InputKeyBinding>();
			foreach (InputKeyBinding inputKeyBinding in components)
			{
				inputKeyBinding.enabled = flag;
			}
		}
		m_LastSelectedObj = currentObj;
	}

	public override void OnDisablePnl()
	{
		Singleton<DataManager>.instance.Save();
	}
}
