using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[DisallowMultipleComponent]
	public class OnStart : Event
	{
		private void Start()
		{
			Execute();
		}
	}
}
