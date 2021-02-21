using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[DisallowMultipleComponent]
	public class OnUpdate : Event
	{
		private void Update()
		{
			Execute();
		}
	}
}
