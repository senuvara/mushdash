using Assets.Scripts.PeroTools.Commons;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PnlQaSelect : UISelectManage
{
	public RectTransform content;

	public Scrollbar scrollbar;

	private List<GameObject> m_SelectableGameObjects = new List<GameObject>();

	private bool m_IsPress;

	private int m_LastIndex;

	public GameObject Question8;

	public GameObject Question9;

	public Text Question12_Simple;

	public Button question9_Simple;

	public Button question12_Simple;

	public void Awake()
	{
		List<Button> btns = GameUtils.FindObjectsOfType<Button>(content);
		for (int j = 0; j < btns.Count; j++)
		{
			int i1 = j;
			btns[j].onClick.AddListener(delegate
			{
				if (!isOnEdgeAnim)
				{
					btns[i1].transform.parent.GetComponent<QAButton>().OnClick();
					if (i1 == btns.Count - 1)
					{
						SetScrollBar(i1);
					}
				}
			});
			m_SelectableGameObjects.Add(btns[j].gameObject);
		}
	}

	public override List<GameObject> SetSelectableObj()
	{
		return m_SelectableGameObjects;
	}

	public override Transform SetEdgeObj(GameObject currentObj)
	{
		QAButton component = currentObj.transform.parent.GetComponent<QAButton>();
		return (!component.inAnim()) ? component.imgSelect : null;
	}

	public override void OnSelect(GameObject currentObj)
	{
		m_LastIndex = currentObj.transform.parent.GetSiblingIndex();
		currentObj.transform.parent.GetComponent<QAButton>().SetSelect(true);
		if (lastSelectedObj != null && currentObj != lastSelectedObj)
		{
			lastSelectedObj.transform.parent.GetComponent<QAButton>().SetSelect(false);
		}
		SetScrollBar(currentObj.transform.parent.GetSiblingIndex());
	}

	public override GameObject DefaultSelectObj()
	{
		OnSelect(m_SelectableGameObjects[m_LastIndex]);
		return m_SelectableGameObjects[m_LastIndex];
	}

	public override void OnUpdatePnl()
	{
		if (Input.GetMouseButtonUp(0) && m_IsPress)
		{
			m_IsPress = false;
		}
		if (Input.GetMouseButtonDown(0))
		{
			m_IsPress = true;
		}
	}

	private void SetScrollBar(int currentIndex)
	{
		if (!m_IsPress)
		{
			float endValue;
			if (currentIndex < 2)
			{
				endValue = 1f;
			}
			else if (currentIndex > m_SelectableGameObjects.Count - 3)
			{
				endValue = 0f;
			}
			else
			{
				Vector2 anchoredPosition = m_SelectableGameObjects[2].transform.parent.GetComponent<RectTransform>().anchoredPosition;
				float y = anchoredPosition.y;
				Vector2 anchoredPosition2 = m_SelectableGameObjects[m_SelectableGameObjects.Count - 3].transform.parent.GetComponent<RectTransform>().anchoredPosition;
				float y2 = anchoredPosition2.y;
				Vector2 anchoredPosition3 = m_SelectableGameObjects[currentIndex].transform.parent.GetComponent<RectTransform>().anchoredPosition;
				float y3 = anchoredPosition3.y;
				Vector2 sizeDelta = m_SelectableGameObjects[currentIndex].transform.parent.GetComponent<RectTransform>().sizeDelta;
				float num = y3 - sizeDelta.y / 2f;
				Vector2 sizeDelta2 = content.GetComponent<RectTransform>().sizeDelta;
				float f = num / sizeDelta2.y;
				endValue = 1f - Mathf.Abs(f);
			}
			DOTween.To(() => scrollbar.value, delegate(float x)
			{
				scrollbar.value = x;
			}, endValue, 0.2f);
		}
	}
}
