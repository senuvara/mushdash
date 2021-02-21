using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[DisallowMultipleComponent]
	public class OnPreWarm : Event
	{
		protected override void OnEnter()
		{
			Execute();
		}
	}
}
