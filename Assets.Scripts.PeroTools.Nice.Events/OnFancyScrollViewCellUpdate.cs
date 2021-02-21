using Assets.Scripts.PeroTools.Nice.Actions;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(VariableBehaviour))]
	public class OnFancyScrollViewCellUpdate : Event
	{
		[NonSerialized]
		public bool isInViewport;

		private Animator m_Animator;

		private int m_StoreHash;

		private VariableBehaviour m_Variables;

		public bool isPlaceHolder
		{
			get;
			set;
		}

		public float animPos
		{
			get;
			private set;
		}

		public int GetDataIndex()
		{
			return m_Variables.GetResult<int>();
		}

		private void OnEnable()
		{
			if (m_StoreHash != 0)
			{
				UpdatePosition(m_StoreHash, animPos);
			}
		}

		public void UpdatePosition(int animHash, float position)
		{
			m_StoreHash = animHash;
			animPos = position;
			if (base.isActiveAndEnabled)
			{
				m_Animator.Play(animHash, -1, position);
			}
		}

		public void UpdateData(int dataInd)
		{
			if (m_Variables != null)
			{
				m_Variables.result = dataInd;
			}
			List<CreateOne> playables = GetPlayables<CreateOne>();
			for (int i = 0; i < playables.Count; i++)
			{
				playables[i].SetIndex(dataInd);
			}
			Execute();
		}

		public void SetVisible(bool visible)
		{
			base.gameObject.SetActive(visible);
		}

		public bool GetVisible()
		{
			return base.gameObject.activeSelf;
		}

		public void Init()
		{
			m_Animator = GetComponent<Animator>();
			m_Variables = GetComponent<VariableBehaviour>();
			if (m_Variables.variable == null || m_Variables.variable.result == null || m_Variables.variable.GetType() != typeof(Constance))
			{
				m_Variables = null;
				Debug.LogError("An int variable required! from gameobject " + base.gameObject.name);
			}
			m_Animator.speed = 0f;
		}
	}
}
