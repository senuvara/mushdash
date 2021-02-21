using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class Quaternion : IValue
	{
		[SerializeField]
		private UnityEngine.Quaternion m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (UnityEngine.Quaternion)value;
			}
		}
	}
}
