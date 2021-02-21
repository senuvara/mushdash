using Assets.Scripts.PeroTools.Nice.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.UI.Effects
{
	public class BgEffectImageRotation : BgEffect
	{
		[Required]
		public FancyScrollView fancyScrollView;

		public float staticRotateSpeed = 2f;

		public float speedFactor = 10f;

		private float m_Direction = 1f;

		private void Update()
		{
			float velocity = fancyScrollView.velocity;
			if (!Mathf.Approximately(velocity, 0f))
			{
				m_Direction = Mathf.Sign(velocity);
			}
			float num = Mathf.Max(Mathf.Abs(velocity) * speedFactor, staticRotateSpeed);
			base.transform.Rotate(Vector3.back, num * Time.deltaTime * m_Direction);
		}
	}
}
