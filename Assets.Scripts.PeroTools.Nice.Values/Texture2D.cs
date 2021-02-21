using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class Texture2D : IValue
	{
		[SerializeField]
		private UnityEngine.Texture2D m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (UnityEngine.Texture2D)value;
			}
		}
	}
}
