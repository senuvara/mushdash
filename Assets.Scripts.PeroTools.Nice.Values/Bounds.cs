using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class Bounds : IValue
	{
		[SerializeField]
		private UnityEngine.Bounds m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (UnityEngine.Bounds)value;
			}
		}
	}
}
