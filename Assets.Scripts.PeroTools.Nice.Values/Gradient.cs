using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class Gradient : IValue
	{
		[SerializeField]
		private UnityEngine.Gradient m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (UnityEngine.Gradient)value;
			}
		}
	}
}
