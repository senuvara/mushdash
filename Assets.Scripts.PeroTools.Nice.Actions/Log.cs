using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class Log : Action
	{
		public string message;

		public override void Execute()
		{
			Debug.Log(message);
		}
	}
}
