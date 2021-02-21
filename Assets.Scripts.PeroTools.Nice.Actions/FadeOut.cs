using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class FadeOut : Action
	{
		[SerializeField]
		[Variable(typeof(GameObject), null, false)]
		private IVariable m_GameObejct;

		[InfoBox("UI only!", InfoMessageType.Info, null)]
		[SerializeField]
		[Variable(typeof(float), "OnFloatDraw", false)]
		private IVariable m_To;

		[SerializeField]
		[Variable(0.5f, "OnFloatDraw", false)]
		private IVariable m_Duration;

		[SerializeField]
		[HideIf("duration", 0f, true)]
		private Ease m_Ease = Ease.Linear;

		private Tweener m_Tweener;

		private CanvasGroup m_CanvasGroup;

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
			GameObject result = m_GameObejct.GetResult<GameObject>();
			if ((bool)m_CanvasGroup && m_CanvasGroup.gameObject != result)
			{
				Object.Destroy(m_CanvasGroup);
			}
			if (!m_CanvasGroup)
			{
				m_CanvasGroup = result.GetOrAddComponent<CanvasGroup>();
			}
			m_Tweener = m_CanvasGroup.DOFade(m_To.GetResult<float>(), duration).SetEase(m_Ease);
		}

		public override void Pause()
		{
			if (m_Tweener != null && m_Tweener.IsPlaying())
			{
				m_Tweener.Pause();
			}
		}

		public override void Resume()
		{
			if (m_Tweener != null && !m_Tweener.IsComplete())
			{
				m_Tweener.Play();
			}
		}
	}
}
