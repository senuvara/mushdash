using Assets.Scripts.PeroTools.Nice.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class List : IValue
	{
		[SerializeField]
		private object m_Result = new List<IValue>();

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = value;
			}
		}
	}
}
