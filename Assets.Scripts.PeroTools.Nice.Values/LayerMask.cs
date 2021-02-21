using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class LayerMask : IValue
	{
		[SerializeField]
		private UnityEngine.LayerMask m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (UnityEngine.LayerMask)value;
			}
		}
	}
}
