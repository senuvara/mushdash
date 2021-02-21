using Assets.Scripts.PeroTools.Nice.Interface;
using System;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class Enum : IValue
	{
		[SerializeField]
		private System.Enum m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (System.Enum)value;
			}
		}
	}
}
