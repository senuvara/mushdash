using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.Graphics
{
	public class DynamicColor : SerializedMonoBehaviour
	{
		[InfoBox("本脚本用于动态改变目标颜色的值", InfoMessageType.Info, null)]
		[Variable(typeof(Color), null, false)]
		public IVariable source;

		[Space(15f)]
		[Tooltip("颜色变化周期")]
		public float timeLength = 1f;

		[Tooltip("颜色变化梯度")]
		public Gradient gradient;

		private float m_PassTime;

		private void Update()
		{
			m_PassTime += Time.deltaTime;
			source.result = gradient.Evaluate(m_PassTime / timeLength);
			if (m_PassTime >= timeLength)
			{
				m_PassTime -= timeLength;
			}
		}
	}
}
