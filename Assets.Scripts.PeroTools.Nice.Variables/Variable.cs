using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Variables
{
	public class Variable : IVariable, IValue
	{
		[SerializeField]
		private VariableType m_Type;

		[SerializeField]
		private string m_Uid;

		[SerializeField]
		private IVariable m_Variable;

		public object result
		{
			get
			{
				object result = (m_Variable == null) ? null : m_Variable.result;
				if (m_Type == VariableType.Global)
				{
					result = Singleton<DataManager>.instance.GetVariable(m_Uid).result;
				}
				return result;
			}
			set
			{
				if (m_Type == VariableType.Global)
				{
					Singleton<DataManager>.instance.GetVariable(m_Uid).result = value;
				}
				else
				{
					m_Variable.result = value;
				}
			}
		}
	}
}
