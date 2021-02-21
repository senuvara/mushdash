using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.PeroTools.Nice.Values
{
	public class UIBehavior : IValue
	{
		[SerializeField]
		private UIBehaviour m_Result;

		public object result
		{
			get
			{
				return m_Result;
			}
			set
			{
				m_Result = (UIBehaviour)value;
			}
		}
	}
}
