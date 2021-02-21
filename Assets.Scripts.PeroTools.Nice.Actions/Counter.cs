using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class Counter : Action
	{
		[SerializeField]
		[Variable(typeof(int), null, false)]
		private IVariable m_Value;

		[SerializeField]
		private List<State> m_States = new List<State>();

		private List<Action<int>> m_Callbacks;

		public override void Enter()
		{
			m_Callbacks = new List<Action<int>>();
			int preCount = 0;
			int preLostCount = 0;
			for (int i = 0; i < m_States.Count; i++)
			{
				State state = m_States[i];
				GameObject txtGameObject = state.txtObj.GetObject<GameObject>();
				if ((bool)txtGameObject)
				{
					txtGameObject.SetActive(false);
				}
				GameObject otherGameObject = state.otherAnim.animator.GetObject<GameObject>();
				if ((bool)otherGameObject)
				{
					otherGameObject.SetActive(false);
				}
				bool isTxtActive = false;
				bool isOtherActive = false;
				Animator txtAnimator = state.txtAnim.animator;
				Animator otherAnimator = state.otherAnim.animator;
				string otherAnimName = string.Empty;
				Text text = state.txtObj.GetObject<Text>();
				if ((bool)text)
				{
					text.gameObject.SetActive(true);
					bool cull = text.canvasRenderer.cull;
					text.SetMaterialDirty();
					text.canvasRenderer.cull = false;
					text.Rebuild(CanvasUpdate.PreRender);
					text.canvasRenderer.cull = cull;
					text.gameObject.SetActive(false);
				}
				TextMeshPro txtMesh = state.txtObj.GetObject<TextMeshPro>();
				if ((bool)txtMesh)
				{
					txtMesh.gameObject.SetActive(true);
					bool cull2 = txtMesh.canvasRenderer.cull;
					txtMesh.SetMaterialDirty();
					txtMesh.canvasRenderer.cull = false;
					txtMesh.Rebuild(CanvasUpdate.PreRender);
					txtMesh.canvasRenderer.cull = cull2;
					txtMesh.gameObject.SetActive(false);
				}
				TextMeshProUGUI txtMeshUGUI = state.txtObj.GetObject<TextMeshProUGUI>();
				if ((bool)txtMeshUGUI)
				{
					txtMeshUGUI.gameObject.SetActive(true);
					bool cull3 = txtMeshUGUI.canvasRenderer.cull;
					txtMeshUGUI.SetMaterialDirty();
					txtMeshUGUI.canvasRenderer.cull = false;
					txtMeshUGUI.Rebuild(CanvasUpdate.PreRender);
					txtMeshUGUI.canvasRenderer.cull = cull3;
					txtMeshUGUI.gameObject.SetActive(false);
				}
				State txtState = state;
				Action<int> item = delegate(int value)
				{
					int num = value;
					if (num == 0)
					{
						preLostCount = preCount;
						if (text != null)
						{
							text.text = preCount.ToString();
						}
						else if (txtMesh != null)
						{
							txtMesh.text = preCount.ToString();
						}
						else if (txtMeshUGUI != null)
						{
							txtMeshUGUI.text = preCount.ToString();
						}
					}
					if (num != 0)
					{
						preCount = num;
					}
					string txtAnimName;
					if ((float)num >= txtState.range.x && (float)num <= txtState.range.y)
					{
						txtAnimName = txtState.txtAnim.inName;
						otherAnimName = ((num != (int)txtState.range.x) ? txtState.otherAnim.standbyName : txtState.otherAnim.inName);
						isTxtActive = true;
						isOtherActive = true;
					}
					else if ((otherAnimName == txtState.otherAnim.inName || otherAnimName == txtState.otherAnim.standbyName) && (float)num < txtState.range.x)
					{
						value = preLostCount;
						txtAnimName = txtState.txtAnim.outName;
						otherAnimName = txtState.otherAnim.outName;
						isTxtActive = true;
						isOtherActive = true;
					}
					else
					{
						value = preLostCount;
						txtAnimName = string.Empty;
						otherAnimName = string.Empty;
						if (isTxtActive)
						{
							isTxtActive = !txtAnimator.IsComplete();
						}
						if (isOtherActive)
						{
							isOtherActive = !otherAnimator.IsComplete();
						}
						if ((float)num > txtState.range.y)
						{
							isTxtActive = false;
							isOtherActive = false;
						}
					}
					if (txtGameObject != null)
					{
						txtGameObject.SetActive(isTxtActive);
					}
					if (isTxtActive && !string.IsNullOrEmpty(txtAnimName) && txtAnimator.isInitialized && txtAnimator.gameObject.activeInHierarchy)
					{
						txtAnimator.Rebind();
						txtAnimator.Play(txtAnimName, 0, 0f);
					}
					if (otherGameObject != null)
					{
						otherGameObject.SetActive(isOtherActive);
					}
					if (isOtherActive)
					{
						if (otherAnimName != txtState.otherAnim.standbyName)
						{
							if (!string.IsNullOrEmpty(otherAnimName) && otherAnimator.isInitialized && otherAnimator.gameObject.activeInHierarchy)
							{
								otherAnimator.Rebind();
								otherAnimator.Play(otherAnimName, 0, 0f);
							}
						}
						else if (otherAnimator.gameObject.activeInHierarchy && !otherAnimator.GetCurrentAnimatorStateInfo(0).IsName(txtState.otherAnim.inName) && !otherAnimator.GetCurrentAnimatorStateInfo(0).IsName(txtState.otherAnim.standbyName))
						{
							otherAnimName = txtState.otherAnim.inName;
							if (!string.IsNullOrEmpty(otherAnimName) && otherAnimator.isInitialized)
							{
								otherAnimator.Rebind();
								otherAnimator.Play(otherAnimName, 0, 0f);
							}
						}
						if (otherAnimName == txtState.otherAnim.outName)
						{
							otherAnimName = string.Empty;
						}
					}
					string text2 = value.ToString();
					if ((bool)text)
					{
						text.text = text2;
					}
					else if ((bool)txtMesh)
					{
						txtMesh.text = text2;
					}
					else if ((bool)txtMeshUGUI)
					{
						txtMeshUGUI.text = text2;
					}
				};
				m_Callbacks.Add(item);
			}
			for (int j = 0; j < m_Callbacks.Count; j++)
			{
				m_Callbacks[j](0);
			}
		}

		public override void Execute()
		{
			for (int i = 0; i < m_Callbacks.Count; i++)
			{
				int result = m_Value.GetResult<int>();
				m_Callbacks[i](result);
			}
		}

		public override void Pause()
		{
			for (int i = 0; i < m_States.Count; i++)
			{
				State state = m_States[i];
				Animator animator = state.txtAnim.animator;
				if ((bool)animator)
				{
					animator.enabled = false;
				}
				Animator animator2 = state.otherAnim.animator;
				if ((bool)animator2)
				{
					animator2.enabled = false;
				}
			}
		}

		public override void Resume()
		{
			for (int i = 0; i < m_States.Count; i++)
			{
				State state = m_States[i];
				Animator animator = state.txtAnim.animator;
				if ((bool)animator)
				{
					animator.enabled = true;
				}
				Animator animator2 = state.otherAnim.animator;
				if ((bool)animator2)
				{
					animator2.enabled = true;
				}
			}
		}
	}
}
