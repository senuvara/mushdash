using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[DisallowMultipleComponent]
	public class OnActivate : Event
	{
		private void OnEnable()
		{
			Execute();
		}
	}
}
