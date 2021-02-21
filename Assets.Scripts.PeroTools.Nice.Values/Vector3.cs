using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class Vector3 : IValue
	{
		[SerializeField]
		private UnityEngine.Vector3 m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (UnityEngine.Vector3)value;
			}
		}
	}
}
