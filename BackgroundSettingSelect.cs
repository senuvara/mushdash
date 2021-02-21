using Assets.Scripts.PeroTools.Nice.Components;
using Assets.Scripts.PeroTools.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackgroundSettingSelect : UISelectManage
{
	public FancyScrollView imgFsv;

	public Slider darkSlider;

	public GameObject darkBtnNext;

	public GameObject darkBtnPrevious;

	public Color highLight;

	public Color normal;

	public List<Image> sceneBgList;

	private int m_ImgFsvIndex;

	public override void OnInit()
	{
		darkSlider.targetGraphic.GetComponent<Image>().color = normal;
		darkBtnPrevious.GetComponent<Image>().color = normal;
		darkBtnNext.GetComponent<Image>().color = normal;
		SetSelectHighlight(imgFsv, true);
		imgFsv.onItemIndexChange += OnImgFsvChange;
		imgFsv.Rebuild();
		darkBtnNext.GetComponent<Button>().onClick.AddListener(delegate
		{
			EventSystem.current.SetSelectedGameObject(darkBtnPrevious);
		});
	}

	public override void OnEnablePnl()
	{
		m_ImgFsvIndex = imgFsv.selectItemIndex;
	}

	public override GameObject DefaultSelectObj()
	{
		return imgFsv.btnPrevious.gameObject;
	}

	public override List<GameObject> SetSelectableObj()
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(imgFsv.btnPrevious.gameObject);
		list.Add(darkSlider.gameObject);
		return list;
	}

	public override void OnSelect(GameObject currentObj)
	{
		if (currentObj == imgFsv.btnPrevious.gameObject)
		{
			SetSelectHighlight(imgFsv, true);
			if ((bool)imgFsv.btnPrevious.GetComponent<LongPressButton>())
			{
				imgFsv.btnPrevious.GetComponent<LongPressButton>().enabled = true;
				imgFsv.btnNext.GetComponent<LongPressButton>().enabled = true;
			}
		}
		else if (currentObj == darkSlider.gameObject)
		{
			SetSelectHighlight(darkSlider.gameObject, true);
		}
		if (!lastSelectedObj)
		{
			return;
		}
		if (lastSelectedObj == imgFsv.btnPrevious.gameObject)
		{
			SetSelectHighlight(imgFsv, false);
			if ((bool)imgFsv.btnPrevious.GetComponent<LongPressButton>())
			{
				imgFsv.btnPrevious.GetComponent<LongPressButton>().enabled = false;
				imgFsv.btnNext.GetComponent<LongPressButton>().enabled = false;
			}
		}
		else if (lastSelectedObj == darkSlider.gameObject)
		{
			SetSelectHighlight(darkSlider.gameObject, false);
		}
	}

	public void SetSelectHighlight(FancyScrollView fsv, bool enable)
	{
		fsv.btnPrevious.image.color = ((!enable) ? normal : highLight);
		fsv.btnPrevious.GetComponent<InputKeyBinding>().enabled = enable;
		fsv.btnNext.image.color = ((!enable) ? normal : highLight);
		fsv.btnNext.GetComponent<InputKeyBinding>().enabled = enable;
		sceneBgList[m_ImgFsvIndex].color = ((!enable) ? normal : highLight);
	}

	public void SetSelectHighlight(GameObject currentSelectObj, bool enable)
	{
		darkSlider.targetGraphic.GetComponent<Image>().color = ((!enable) ? normal : highLight);
		darkBtnPrevious.GetComponent<Image>().color = ((!enable) ? normal : highLight);
		darkBtnPrevious.GetComponent<InputKeyBinding>().enabled = enable;
		darkBtnNext.GetComponent<Image>().color = ((!enable) ? normal : highLight);
		darkBtnNext.GetComponent<InputKeyBinding>().enabled = enable;
		InputKeyBinding[] components = darkSlider.GetComponents<InputKeyBinding>();
		foreach (InputKeyBinding inputKeyBinding in components)
		{
			inputKeyBinding.enabled = enable;
		}
	}

	private void OnImgFsvChange(int i)
	{
		bool flag = i == m_ImgFsvIndex;
		sceneBgList[i].color = ((!flag) ? highLight : normal);
		sceneBgList[m_ImgFsvIndex].color = ((!flag) ? normal : highLight);
		m_ImgFsvIndex = i;
	}
}
