using Assets.Scripts.PeroTools.Nice.Components;
using Assets.Scripts.PeroTools.Nice.Variables;
using Assets.Scripts.PeroTools.PreWarm;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[RequireComponent(typeof(VariableBehaviour))]
	public class OnFancyScrollViewFinalSelectItem : Event, IPreWarm
	{
		private VariableBehaviour m_Variables;

		[Required]
		public FancyScrollView target;

		private void OnFinalSelectItem(int i)
		{
			if (m_Variables != null)
			{
				m_Variables.result = i;
			}
			Execute();
		}

		protected override void OnEnter()
		{
			m_Variables = GetComponent<VariableBehaviour>();
			if (m_Variables.variable == null || m_Variables.variable.result == null || m_Variables.variable.GetType() != typeof(Constance))
			{
				Debug.LogError("An int variable required! from gameobject " + base.gameObject.name);
			}
			else
			{
				target.onFinalItemIndexChange += OnFinalSelectItem;
			}
		}
	}
}
