using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class GameObject : IValue
	{
		[SerializeField]
		private UnityEngine.GameObject m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (UnityEngine.GameObject)value;
			}
		}
	}
}
