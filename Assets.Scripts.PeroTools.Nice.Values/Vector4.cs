using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class Vector4 : IValue
	{
		[SerializeField]
		private UnityEngine.Vector4 m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (UnityEngine.Vector4)value;
			}
		}
	}
}
