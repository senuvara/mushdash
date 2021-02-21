using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Events;
using Assets.Scripts.UI.Specials;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrovesSelect : UISelectManage
{
	public Transform selectableObjParent;

	private List<GameObject> m_SelectedObjects = new List<GameObject>();

	public Scrollbar scrollbar;

	public float animTime = 0.2f;

	public OnCustomEvent customEvent;

	private int m_Count;

	private int m_Line;

	private float m_MovingInterval;

	private int m_ActivateItemIndex;

	private GameObject m_LastSelectObj;

	private bool m_IsPress;

	private bool m_RefreshSelectableObj;

	public override void OnInit()
	{
		int childCount = selectableObjParent.childCount;
		RefreshSelectedObj();
		m_Line = GetCellCount(selectableObjParent.GetComponent<GridLayoutGroup>());
		m_Count = childCount / m_Line;
		if (childCount % m_Line > 0)
		{
			m_Count++;
		}
		m_MovingInterval = 1f / (float)(m_Count - 2);
		AdjuseNaviOnShouTai();
	}

	private Selectable GetSelectable(int index)
	{
		return selectableObjParent.GetChild(index).Find("TglItem").GetComponent<Selectable>();
	}

	private int GetCellCount(GridLayoutGroup g)
	{
		float width = g.GetComponent<RectTransform>().rect.width;
		for (int i = 1; i < 15; i++)
		{
			Vector2 cellSize = g.cellSize;
			int num = (int)cellSize.x * i;
			Vector2 spacing = g.spacing;
			int num2 = num + (int)spacing.x * (i - 1);
			if (width < (float)num2)
			{
				return i - 1;
			}
		}
		return 0;
	}

	public override GameObject DefaultSelectObj()
	{
		GameObject gameObject = m_LastSelectObj ?? selectableObjParent.GetChild(0).gameObject;
		Toggle componentInChildren = gameObject.GetComponentInChildren<Toggle>();
		return (!(componentInChildren != null)) ? null : componentInChildren.gameObject;
	}

	public override void OnSelect(GameObject currentObj)
	{
		if (m_LastSelectObj != null)
		{
			Transform last = m_LastSelectObj.transform.parent;
			DOTween.To(() => last.localScale, delegate(Vector3 l)
			{
				last.localScale = l;
			}, Vector3.one, animTime);
		}
		m_LastSelectObj = currentObj;
		Toggle component = currentObj.GetComponent<Toggle>();
		if (!(component != null))
		{
			return;
		}
		component.isOn = true;
		if (m_IsPress)
		{
			return;
		}
		if (m_SelectedObjects.Count > 0)
		{
			int num = m_SelectedObjects.IndexOf(currentObj) + 1;
			int num2 = num / m_Line;
			if (num % m_Line > 0)
			{
				num2++;
			}
			float num3 = 0f;
			DOTween.To(endValue: (num2 <= 2) ? 1f : ((num2 < 3 || num2 > m_Count - 2) ? 0f : ((float)(m_Count - num2) * m_MovingInterval)), getter: () => scrollbar.value, setter: delegate(float x)
			{
				scrollbar.value = x;
			}, duration: animTime);
		}
		else
		{
			Debug.Log("Selected Toggle List is Null");
		}
	}

	public override List<GameObject> SetSelectableObj()
	{
		return m_SelectedObjects;
	}

	public override Transform SetEdgeObj(GameObject currentObj)
	{
		return currentObj.transform.parent.Find("ImgSelected").transform;
	}

	public override void OnEnablePnl()
	{
		selectableObjParent.GetComponent<TroveItemIndexs>().PreWarm(0);
		for (int i = 0; i < selectableObjParent.childCount; i++)
		{
			selectableObjParent.GetChild(i).GetComponent<PnlTroveItem>().SetActivate(true);
		}
		RefreshSelectedObj();
		customEvent.Execute();
		AdjuseNaviOnShouTai();
	}

	private IEnumerator UpdateActivate()
	{
		yield return new WaitForEndOfFrame();
		SetActivateEnable(m_ActivateItemIndex);
		if (++m_ActivateItemIndex < selectableObjParent.childCount)
		{
			StartCoroutine(UpdateActivate());
		}
	}

	private void SetActivateEnable(int idx)
	{
		selectableObjParent.GetChild(idx).GetComponent<PnlTroveItem>().SetActivate(true);
	}

	public void ReactivateItem()
	{
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

	private void RefreshSelectedObj()
	{
		int childCount = selectableObjParent.childCount;
		m_SelectedObjects.Clear();
		for (int i = 0; i < childCount; i++)
		{
			Toggle component = selectableObjParent.GetChild(i).GetChild(0).GetComponent<Toggle>();
			if (selectableObjParent.GetChild(i).GetComponent<PnlTroveItem>().isEmpty())
			{
				component.interactable = false;
				continue;
			}
			component.interactable = true;
			m_SelectedObjects.Add(component.gameObject);
		}
		SetSelectableObjList(m_SelectedObjects);
	}

	private void AdjustNavi(Toggle obj)
	{
		if (m_Count > 1)
		{
			int siblingIndex = obj.transform.parent.GetSiblingIndex();
			Navigation navigation = obj.navigation;
			navigation.mode = Navigation.Mode.Explicit;
			navigation.selectOnUp = m_SelectedObjects[siblingIndex - m_Line].GetComponent<Toggle>();
			navigation.selectOnLeft = m_SelectedObjects[siblingIndex - 1].GetComponent<Toggle>();
			navigation.selectOnDown = null;
			navigation.selectOnRight = m_SelectedObjects[siblingIndex - m_Line + 1].GetComponent<Toggle>();
			obj.navigation = navigation;
		}
	}

	private void AdjustNavitoAuto(Toggle obj)
	{
		Navigation navigation = obj.navigation;
		navigation.mode = Navigation.Mode.Automatic;
		obj.navigation = navigation;
	}

	private void AdjuseNaviOnShouTai()
	{
		if (!Singleton<InputManager>.instance.IsConnetShouTai())
		{
			return;
		}
		for (int i = 1; i < m_Count; i++)
		{
			int num = m_Line * i - 1;
			for (int j = 0; j <= 1; j++)
			{
				PnlTroveItem component = selectableObjParent.GetChild(num + j).GetComponent<PnlTroveItem>();
				if (!component.isEmpty())
				{
					Selectable component2 = component.transform.Find("TglItem").GetComponent<Selectable>();
					Navigation navigation = component2.navigation;
					navigation.mode = Navigation.Mode.Explicit;
					navigation.selectOnLeft = GetSelectable(num - 1 + j);
					navigation.selectOnRight = GetSelectable(num + 1 + j);
					int num2 = num - m_Line + j;
					if (num2 >= 0)
					{
						navigation.selectOnUp = GetSelectable(num2);
					}
					int num3 = num + m_Line + j;
					if (num3 <= selectableObjParent.childCount - 1)
					{
						navigation.selectOnDown = GetSelectable(num3);
					}
					component2.navigation = navigation;
				}
			}
		}
	}
}
