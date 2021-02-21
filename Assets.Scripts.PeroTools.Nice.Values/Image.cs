using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class Image : IValue
	{
		[SerializeField]
		private UnityEngine.UI.Image m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (UnityEngine.UI.Image)value;
			}
		}
	}
}
