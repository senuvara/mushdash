using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class Object : IValue
	{
		[SerializeField]
		private UnityEngine.Object m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (UnityEngine.Object)value;
			}
		}
	}
}
