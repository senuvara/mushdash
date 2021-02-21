using System;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	[Serializable]
	public class State
	{
		public UnityEngine.Object txtObj;

		public Vector2 range;

		public StateAnim txtAnim = new StateAnim();

		public StateAnim otherAnim = new StateAnim();
	}
}
