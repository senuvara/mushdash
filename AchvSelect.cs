using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.UI;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AchvSelect : UISelectManage
{
	public Transform challenges;

	public Transform achv;

	public Scrollbar scrollbar;

	public Color highLight;

	public Color normal;

	public float animTime = 0.2f;

	private readonly List<GameObject> m_SelectableGameObjects = new List<GameObject>();

	private readonly List<GameObject> m_SeeableObj = new List<GameObject>();

	private GameObject m_LastObj;

	private int m_ChallengeCount;

	private bool m_IsPress;

	private InputKeyBinding m_DeleteButton;

	public override void OnInit()
	{
		m_ChallengeCount = challenges.childCount;
		SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
		{
			OnCheckNullDir();
		}, 0.2f);
	}

	public override GameObject DefaultSelectObj()
	{
		if (m_LastObj != null)
		{
			OnSelect(m_LastObj);
			return m_LastObj;
		}
		UpdateSelectableObj();
		if (m_SelectableGameObjects.Count == 0)
		{
			return null;
		}
		m_LastObj = m_SelectableGameObjects[0];
		OnSelect(m_SelectableGameObjects[0]);
		return m_SelectableGameObjects[0];
	}

	public override List<GameObject> SetSelectableObj()
	{
		return m_SelectableGameObjects;
	}

	public override void OnSelect(GameObject currentObj)
	{
		if (m_LastObj != null)
		{
			SetObjHighLight(m_LastObj, false);
		}
		SetObjHighLight(currentObj, true);
		m_LastObj = currentObj;
		SetScrollBar(m_SeeableObj.IndexOf(currentObj));
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
		if (challenges.childCount != m_ChallengeCount)
		{
			Debug.Log("challengeCountChange");
			UpdateSelectableObj();
			m_ChallengeCount = challenges.childCount;
		}
		if (defaultSelect == null && m_LastObj == null)
		{
			Button button = GameUtils.FindObjectOfType<Button>(achv);
			if (!(button == null) && !(EventSystem.current == null))
			{
				EventSystem.current.SetSelectedGameObject(button.gameObject);
				OnSelect(button.gameObject);
				UpdateSelectableObj();
			}
		}
	}

	public override Transform SetEdgeObj(GameObject currentObj)
	{
		if (currentObj == null)
		{
			Debug.Log("null");
			return null;
		}
		return currentObj.transform.GetChild(0);
	}

	private void SetObjHighLight(GameObject obj, bool isSelect)
	{
		Image imgSelect = obj.transform.GetChild(0).GetComponent<Image>();
		DOTween.To(() => imgSelect.color, delegate(Color selectColor)
		{
			imgSelect.color = selectColor;
		}, new Color(highLight.r, highLight.g, highLight.b, isSelect ? 1 : 0), animTime);
		if (obj.transform.IsChildOf(challenges))
		{
			InputKeyBinding inputKeyBinding = GameUtils.FindObjectOfType<InputKeyBinding>(obj.transform.parent);
			inputKeyBinding.enabled = isSelect;
			Image imgDelete = obj.transform.parent.Find("TwnMoveLeft/Challenge/BtnDelete").GetComponent<Image>();
			DOTween.To(() => imgDelete.color, delegate(Color c)
			{
				imgDelete.color = c;
			}, (!isSelect) ? normal : highLight, animTime);
		}
	}

	public void UpdateSelectableObj()
	{
		List<Button> list = GameUtils.FindObjectsOfType<Button>(base.transform);
		if (list.Count == 0)
		{
			return;
		}
		m_SelectableGameObjects.Clear();
		m_SeeableObj.Clear();
		foreach (Button item in list)
		{
			Button btn1 = item;
			if (item.name == "BtnEmpty")
			{
				m_SelectableGameObjects.Add(item.gameObject);
				if (item.gameObject.activeInHierarchy)
				{
					item.onClick.AddListener(delegate
					{
						EventSystem.current.SetSelectedGameObject(btn1.gameObject);
					});
					m_SeeableObj.Add(item.gameObject);
				}
			}
			else if (item.name == "BtnDelete")
			{
				item.onClick.AddListener(delegate
				{
					btn1.GetComponent<InputKeyBinding>().enabled = false;
					m_DeleteButton = btn1.GetComponent<InputKeyBinding>();
				});
				bool flag = m_LastObj != null && m_LastObj.transform.IsChildOf(item.transform.parent.parent.parent);
				item.targetGraphic.color = ((!flag) ? normal : highLight);
			}
		}
		SetSelectableObjList(m_SelectableGameObjects);
		SetPanelBindings();
	}

	private void SetScrollBar(int currentIndex)
	{
		if (!m_IsPress)
		{
			int num = 0;
			if (challenges.childCount > 0)
			{
				num = challenges.childCount;
			}
			float endValue;
			if (currentIndex <= num)
			{
				endValue = 1f;
			}
			else if (currentIndex > m_SeeableObj.Count - 5)
			{
				endValue = 0f;
			}
			else
			{
				float num2 = 1f / (float)(m_SeeableObj.Count - 4 - num);
				endValue = 1f - num2 * (float)(currentIndex - num);
			}
			DOTween.To(() => scrollbar.value, delegate(float x)
			{
				scrollbar.value = x;
			}, endValue, animTime);
		}
	}

	public void ScrollToTop()
	{
		if (m_SeeableObj.Count > 0)
		{
			OnSelect(m_SeeableObj[0]);
			scrollbar.value = 1f;
		}
	}

	public void RemuseDeleteButton()
	{
		if ((bool)m_DeleteButton)
		{
			m_DeleteButton.enabled = true;
		}
	}
}
