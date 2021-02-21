using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class Float : IValue
	{
		[SerializeField]
		private float m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (float)value;
			}
		}
	}
}
