using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class Vector2 : IValue
	{
		[SerializeField]
		private UnityEngine.Vector2 m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (UnityEngine.Vector2)value;
			}
		}
	}
}
