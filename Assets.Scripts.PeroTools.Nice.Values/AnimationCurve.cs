using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class AnimationCurve : IValue
	{
		[SerializeField]
		private UnityEngine.AnimationCurve m_Result = new UnityEngine.AnimationCurve();

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (UnityEngine.AnimationCurve)value;
			}
		}
	}
}
