using UnityEngine;

namespace Assets.Scripts.Graphics
{
	[RequireComponent(typeof(GradientFog))]
	public class DynamicGradient : MonoBehaviour
	{
		public Gradient[] keyGradient;

		[Tooltip("变换到下一个阶段需要的时间")]
		public float timeStep = 1f;

		[Tooltip("变换到下一个阶段的变化趋势，X轴取值0-1，Y轴取值0-1。大于0-1的部分不会起作用。")]
		public AnimationCurve timeStepCurve;

		private Gradient m_Origin;

		private float m_PassTime;

		private GradientFog m_target;

		private void Start()
		{
			m_target = GetComponent<GradientFog>();
			m_Origin = new Gradient();
			m_Origin.SetKeys((GradientColorKey[])m_target.fogGradient.colorKeys.Clone(), (GradientAlphaKey[])m_target.fogGradient.alphaKeys.Clone());
		}

		private void Update()
		{
			m_PassTime += Time.deltaTime;
			float time = m_PassTime % timeStep / timeStep;
			float t = timeStepCurve.Evaluate(time);
			float f = m_PassTime / timeStep;
			int num = Mathf.FloorToInt(f) % keyGradient.Length;
			int num2 = Mathf.CeilToInt(f) % keyGradient.Length;
			GradientColorKey[] array = new GradientColorKey[m_Origin.colorKeys.Length];
			GradientAlphaKey[] array2 = new GradientAlphaKey[m_Origin.alphaKeys.Length];
			for (int i = 0; i < m_Origin.colorKeys.Length; i++)
			{
				float time2 = m_Origin.colorKeys[i].time;
				Color col = Color.Lerp(keyGradient[num].Evaluate(time2), keyGradient[num2].Evaluate(time2), t);
				array[i] = new GradientColorKey(col, time2);
			}
			for (int j = 0; j < m_Origin.alphaKeys.Length; j++)
			{
				float time3 = m_Origin.alphaKeys[j].time;
				Color color = Color.Lerp(keyGradient[num].Evaluate(time3), keyGradient[num2].Evaluate(time3), t);
				float a = color.a;
				array2[j] = new GradientAlphaKey(a, time3);
			}
			m_target.fogGradient.SetKeys(array, array2);
		}
	}
}
