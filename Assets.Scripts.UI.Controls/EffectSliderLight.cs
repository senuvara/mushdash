using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Controls
{
	[RequireComponent(typeof(Image))]
	public class EffectSliderLight : MonoBehaviour
	{
		public Slider slider;

		public Color from;

		public Color to;

		public float duration;

		public AnimationCurve curve;

		private Material m_Mtr;

		private int m_IdColor;

		private int m_IdXOffset;

		private TweenerCore<Color, Color, ColorOptions> m_Tween;

		public Color color
		{
			get
			{
				return m_Mtr.GetColor(m_IdColor);
			}
			set
			{
				m_Mtr.SetColor(m_IdColor, value);
			}
		}

		private void Awake()
		{
			Image component = GetComponent<Image>();
			Material material2 = m_Mtr = (component.material = Object.Instantiate(component.material));
			m_IdColor = Shader.PropertyToID("_Color");
			m_IdXOffset = Shader.PropertyToID("_XOffset");
			m_Tween = DOTween.To(() => m_Mtr.GetColor(m_IdColor), delegate(Color value)
			{
				m_Mtr.SetColor(m_IdColor, value);
			}, to, duration);
			m_Tween.SetAutoKill(false);
			Singleton<EventManager>.instance.RegEvent("Battle/OnFeverAdd").trigger += OnTrigger;
		}

		private void OnDestroy()
		{
			Singleton<EventManager>.instance.RegEvent("Battle/OnFeverAdd").trigger -= OnTrigger;
		}

		private void OnTrigger(object sender, object reciever, params object[] args)
		{
			Play();
		}

		public void SetXOffset(float value)
		{
			m_Mtr.SetFloat(m_IdXOffset, value);
		}

		public void Play()
		{
			if (slider != null && m_Tween != null)
			{
				SetXOffset(slider.normalizedValue);
				m_Mtr.SetColor(m_IdColor, from);
				m_Tween.Restart();
				m_Tween.Play();
			}
		}
	}
}
