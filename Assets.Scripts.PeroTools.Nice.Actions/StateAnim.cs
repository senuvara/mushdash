using System;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	[Serializable]
	public class StateAnim
	{
		public Animator animator;

		public bool isActive;

		public string inName;

		public string standbyName;

		public string outName;
	}
}
