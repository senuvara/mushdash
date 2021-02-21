using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class Ref : IValue
	{
		[SerializeField]
		private object m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = value;
			}
		}
	}
}
