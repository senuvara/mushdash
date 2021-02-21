using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class Integer : IValue
	{
		[SerializeField]
		private int m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (int)value;
			}
		}
	}
}
