using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class SetToggle : Action
	{
		[SerializeField]
		[Required]
		private Toggle m_Toggle;

		[SerializeField]
		[Variable(false, null, false)]
		private IVariable m_IsOn;

		public override void Execute()
		{
			m_Toggle.isOn = m_IsOn.GetResult<bool>();
		}
	}
}
