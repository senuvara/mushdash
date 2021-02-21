using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Variables
{
	public class Constance : IVariable, IValue
	{
		[SerializeField]
		private IValue m_Value;

		public object result
		{
			get
			{
				if (m_Value == null)
				{
					return null;
				}
				return m_Value.result;
			}
			set
			{
				if (m_Value != null)
				{
					m_Value.result = value;
				}
				else
				{
					m_Value = VariableUtils.CreateIValue(value);
				}
			}
		}

		public Constance(IValue value)
		{
			m_Value = value;
		}

		public Constance()
		{
		}
	}
}
