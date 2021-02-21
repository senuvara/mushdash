using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[DisallowMultipleComponent]
	public class OnDeactivate : Event
	{
		private void OnDisable()
		{
			Execute();
		}
	}
}
