using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class Wait : Action
	{
		[SerializeField]
		[Variable(0f, null, false)]
		private IVariable m_Duration;

		public override float duration => m_Duration.GetResult<float>();
	}
}
