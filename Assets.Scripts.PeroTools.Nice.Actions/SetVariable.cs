using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using System;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class SetVariable : Action
	{
		[SerializeField]
		private IVariable m_Variable;

		[SerializeField]
		private IVariable m_TargetVariable;

		public override void Execute()
		{
			IValue value = m_Variable.result as IValue;
			object result = m_TargetVariable.result;
			if (result == null)
			{
				return;
			}
			Type type = result.GetType();
			if (value != null)
			{
				Type type2 = value.result.GetType();
				value.result = ((type == type2) ? result : Convert.ChangeType(result, type2));
				return;
			}
			object result2 = m_Variable.result;
			if (result2 != null)
			{
				Type type3 = m_Variable.result.GetType();
				m_Variable.result = ((type == type3) ? result : Convert.ChangeType(result, type3));
			}
		}

		private bool IsVariableType()
		{
			return m_Variable is Variable;
		}
	}
}
