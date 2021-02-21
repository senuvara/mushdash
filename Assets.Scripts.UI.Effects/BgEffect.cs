using UnityEngine;

namespace Assets.Scripts.UI.Effects
{
	[RequireComponent(typeof(Animator))]
	public class BgEffect : MonoBehaviour
	{
		public string fadeInAnimName = "BgAlbumIn";

		public string fadeOutAnimName = "BgAlbumOut";

		private Animator m_Animator;

		private void Awake()
		{
			m_Animator = GetComponent<Animator>();
		}

		public void FadeIn()
		{
			if (m_Animator != null && !string.IsNullOrEmpty(fadeInAnimName))
			{
				m_Animator.Play(fadeInAnimName);
			}
		}

		public void FadeOut()
		{
			if (m_Animator != null && !string.IsNullOrEmpty(fadeOutAnimName))
			{
				m_Animator.Play(fadeOutAnimName);
			}
		}
	}
}
