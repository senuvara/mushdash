using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[DisallowMultipleComponent]
	public abstract class OnDestroyEvent : Event
	{
		private void OnDestroy()
		{
			Execute();
		}
	}
}
