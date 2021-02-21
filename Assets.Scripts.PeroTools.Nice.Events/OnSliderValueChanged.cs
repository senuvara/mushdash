using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[DisallowMultipleComponent]
	public class OnSliderValueChanged : Event
	{
		[SerializeField]
		[Required]
		[PropertyOrder(-1)]
		private Slider m_Slider;

		protected override void OnEnter()
		{
			m_Slider.onValueChanged.AddListener(Listener);
		}

		protected override void OnExit()
		{
			m_Slider.onValueChanged.RemoveListener(Listener);
		}

		private void Listener(float value)
		{
			Execute();
		}
	}
}
