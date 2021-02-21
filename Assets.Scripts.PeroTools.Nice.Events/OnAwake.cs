using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[DisallowMultipleComponent]
	public class OnAwake : Event
	{
		private void Awake()
		{
			Execute();
		}
	}
}
