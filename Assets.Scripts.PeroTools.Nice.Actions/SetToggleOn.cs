using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class SetToggleOn : Action
	{
		[SerializeField]
		[Required]
		private Toggle m_Toggle;

		public override void Execute()
		{
			m_Toggle.isOn = true;
		}
	}
}
