using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class AudioClip : IValue
	{
		[SerializeField]
		private UnityEngine.AudioClip m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (UnityEngine.AudioClip)value;
			}
		}
	}
}
