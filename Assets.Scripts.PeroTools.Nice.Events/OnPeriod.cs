using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[DisallowMultipleComponent]
	public class OnPeriod : Event
	{
		[PropertyOrder(-1)]
		public float frequency;

		protected override void OnEnter()
		{
			InvokeRepeating("Execute", frequency, frequency);
		}

		protected override void OnExit()
		{
			CancelInvoke("Execute");
		}
	}
}
