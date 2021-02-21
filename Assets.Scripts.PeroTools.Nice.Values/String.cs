using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class String : IValue
	{
		[SerializeField]
		private string m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (string)value;
			}
		}
	}
}
