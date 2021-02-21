using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class SetSliderValue : Action
	{
		[SerializeField]
		[Variable(typeof(Object), null, false)]
		private IVariable m_Object;

		[SerializeField]
		[Variable(typeof(float), null, false)]
		private IVariable m_Percent;

		[SerializeField]
		[Variable(typeof(float), null, false)]
		private IVariable m_Duration;

		[SerializeField]
		[Variable(typeof(AnimationCurve), null, false)]
		[HideIf("duration", 0f, true)]
		private IVariable m_Curve;

		private Tweener m_Tweener;

		private DG.Tweening.Sequence m_Sequence;

		private Slider m_Slider;

		private Image m_Image;

		public override float duration
		{
			get
			{
				if (m_Duration == null || m_Duration.result == null)
				{
					return 0f;
				}
				return m_Duration.GetResult<float>();
			}
		}

		public override void Execute()
		{
			if (!m_Slider && !m_Image)
			{
				Object result = m_Object.GetResult<Object>();
				m_Slider = result.GetObject<Slider>();
				m_Image = result.GetObject<Image>();
			}
			if (!m_Slider && !m_Image)
			{
				return;
			}
			float value = m_Percent.GetResult<float>();
			AnimationCurve animationCurve = null;
			if (duration > 0f && m_Curve.result != null)
			{
				animationCurve = m_Curve.GetResult<AnimationCurve>();
			}
			if ((bool)m_Slider)
			{
				if (duration <= 0f)
				{
					m_Slider.value = value;
					return;
				}
				float value2 = m_Slider.value;
				if (value > 1f)
				{
					if (m_Sequence != null)
					{
						m_Sequence.Complete(true);
					}
					m_Sequence = DOTween.Sequence();
					if (animationCurve != null)
					{
						m_Sequence.SetEase(animationCurve);
					}
					int num = Mathf.FloorToInt(value);
					for (int i = 0; i < num; i++)
					{
						float num2 = duration / value;
						if (i == 0)
						{
							num2 *= 1f - value2;
						}
						Tweener t = m_Slider.DOValue(1f, num2).OnComplete(delegate
						{
							m_Slider.value = 0f;
						}).SetEase(Ease.Linear);
						m_Sequence.Append(t);
					}
					float v = value - (float)num;
					Tweener t2 = m_Slider.DOValue(v, duration * v / value).SetEase(Ease.Linear);
					m_Sequence.Append(t2);
					m_Sequence.Play();
					m_Sequence.OnComplete(delegate
					{
						m_Slider.value = v;
					});
				}
				else
				{
					if (m_Tweener != null)
					{
						m_Tweener.Complete(true);
					}
					m_Tweener = m_Slider.DOValue(value, duration);
					m_Tweener.OnComplete(delegate
					{
						m_Slider.value = value;
					});
					if (animationCurve != null)
					{
						m_Tweener.SetEase(animationCurve);
					}
				}
			}
			if (!m_Image)
			{
				return;
			}
			if (duration <= 0f)
			{
				m_Image.fillAmount = value;
				return;
			}
			float fillAmount = m_Image.fillAmount;
			if (value > 1f)
			{
				if (m_Sequence != null)
				{
					m_Sequence.Complete(true);
				}
				m_Sequence = DOTween.Sequence();
				if (animationCurve != null)
				{
					m_Sequence.SetEase(animationCurve);
				}
				int num3 = Mathf.FloorToInt(value);
				for (int j = 0; j < num3; j++)
				{
					float num4 = duration / value;
					if (j == 0)
					{
						num4 *= 1f - fillAmount;
					}
					Tweener t3 = m_Image.DOFillAmount(1f, num4).OnComplete(delegate
					{
						m_Image.fillAmount = 0f;
					}).SetEase(Ease.Linear);
					m_Sequence.Append(t3);
				}
				float v2 = value - (float)num3;
				Tweener t4 = m_Image.DOFillAmount(v2, duration * v2 / value).SetEase(Ease.Linear);
				m_Sequence.Append(t4);
				m_Sequence.Play();
				m_Sequence.OnComplete(delegate
				{
					m_Image.fillAmount = v2;
				});
			}
			else
			{
				if (m_Tweener != null)
				{
					m_Tweener.Complete(true);
				}
				m_Tweener = m_Image.DOFillAmount(value, duration);
				m_Tweener.OnComplete(delegate
				{
					m_Image.fillAmount = value;
				});
				if (animationCurve != null)
				{
					m_Tweener.SetEase(animationCurve);
				}
			}
		}

		public override void Exit()
		{
			if (m_Sequence != null)
			{
				m_Sequence.Kill();
			}
			if (m_Tweener != null)
			{
				m_Tweener.Kill();
			}
		}

		public override void Pause()
		{
			if (m_Sequence != null && m_Sequence.IsPlaying())
			{
				m_Sequence.Pause();
			}
			if (m_Tweener != null && m_Tweener.IsPlaying())
			{
				m_Tweener.Pause();
			}
		}

		public override void Resume()
		{
			if (m_Sequence != null && !m_Sequence.IsComplete())
			{
				m_Sequence.Play();
			}
			if (m_Tweener != null && !m_Tweener.IsComplete())
			{
				m_Tweener.Play();
			}
		}
	}
}
