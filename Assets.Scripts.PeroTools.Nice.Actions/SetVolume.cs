using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class SetVolume : Action
	{
		public enum AudioType
		{
			BGM,
			Others
		}

		[SerializeField]
		[EnumToggleButtons]
		private AudioType m_AudioType;

		[SerializeField]
		[ShowIf("m_AudioType", AudioType.Others, true)]
		[Required]
		private AudioSource m_AudioSource;

		[SerializeField]
		[Variable(1f, "OnVolumeDraw", false)]
		private IVariable m_Volume;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		private float m_Duration;

		[SerializeField]
		[HideIf("m_Duration", 0f, true)]
		[ShowIf("m_IsPro", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		private Ease m_Ease;

		public override void Execute()
		{
			switch (m_AudioType)
			{
			case AudioType.BGM:
				SetAudioSourceVolume(Singleton<AudioManager>.instance.bgm);
				break;
			case AudioType.Others:
				SetAudioSourceVolume(m_AudioSource);
				break;
			}
		}

		private void SetAudioSourceVolume(AudioSource audioSource)
		{
			if (!audioSource)
			{
				return;
			}
			float volume = m_Volume.GetResult<float>();
			if (m_Duration > 0f)
			{
				audioSource.DOFade(volume, m_Duration).OnComplete(delegate
				{
					audioSource.volume = volume;
				});
			}
			else
			{
				audioSource.volume = volume;
			}
		}
	}
}
