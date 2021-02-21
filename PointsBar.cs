using Assets.Scripts.PeroTools.Nice.Components;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class PointsBar : MonoBehaviour
{
	[Required]
	public FancyScrollView fancyScrollView;

	private List<Animator> m_BarAnim = new List<Animator>();

	private int m_AnimHash;

	private int m_CurrentPoint;

	private void Awake()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Animator component = base.transform.GetChild(i).GetComponent<Animator>();
			component.speed = 0f;
			m_BarAnim.Add(component);
		}
		m_AnimHash = Animator.StringToHash("PointBar");
		fancyScrollView.onUpdatePosition += FsvOnOnUpdatePosition;
		fancyScrollView.onFinalItemIndexChange += FsvIntemIndexChange;
	}

	private void FsvIntemIndexChange(int index)
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			float pos = (i != index) ? 0.01f : 0.99f;
			PlayAnim(i, pos);
		}
		m_CurrentPoint = index;
	}

	public void SetDefaultPoint(int index)
	{
		PlayAnim(index, 0.99f);
		m_CurrentPoint = index;
	}

	private void FsvOnOnUpdatePosition(float pos)
	{
		float currentScrollPosition = fancyScrollView.currentScrollPosition;
		int num = Mathf.FloorToInt(currentScrollPosition);
		int num2 = Mathf.CeilToInt(currentScrollPosition);
		float pos2 = CheckDis(1f - Mathf.Abs(currentScrollPosition - (float)num));
		float pos3 = CheckDis(1f - Mathf.Abs(currentScrollPosition - (float)num2));
		if (fancyScrollView.movementType != 0)
		{
			if (num >= 0)
			{
				PlayAnim(num, pos2);
			}
			else
			{
				PlayAnim(num2, 0.99f);
			}
			if (num2 <= base.transform.childCount - 1)
			{
				PlayAnim(num2, pos3);
			}
			else
			{
				PlayAnim(num, pos2);
			}
		}
		else
		{
			PlayAnim(LoopCell(num), pos2);
			PlayAnim(LoopCell(num2), pos3);
		}
	}

	private int LoopCell(int index)
	{
		while (index < 0)
		{
			index += m_BarAnim.Count;
		}
		return Mathf.Abs(index % m_BarAnim.Count);
	}

	private float CheckDis(float dis)
	{
		if (dis <= 0.01f)
		{
			dis = 0.01f;
		}
		else if (dis >= 0.99f)
		{
			dis = 0.99f;
		}
		return dis;
	}

	private void PlayAnim(int index, float pos)
	{
		if (base.gameObject.activeInHierarchy)
		{
			m_BarAnim[index].Play(m_AnimHash, -1, pos);
		}
	}

	public void SetCurrentPoint(bool enable)
	{
		float pos = (!enable) ? 0.01f : 0.99f;
		PlayAnim(m_CurrentPoint, pos);
	}
}
