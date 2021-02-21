using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class PlayMusic : Action
	{
		[SerializeField]
		[Variable(typeof(Object), null, false)]
		[OnConstanceChanged("OnConstanceChanged")]
		private IVariable m_AudioClipSource;

		[SerializeField]
		[Variable(typeof(float), null, false)]
		private IVariable m_FadeInTime;

		[SerializeField]
		[ShowIf("HasFadeInTime", true)]
		private Ease m_Ease = Ease.Linear;

		[SerializeField]
		[HideInInspector]
		private List<string> m_AudioClipNames;

		private AudioClip m_AudioClip;

		public override float duration
		{
			get
			{
				if ((bool)m_AudioClip)
				{
					return m_AudioClip.length;
				}
				return 0f;
			}
		}

		public override void Execute()
		{
			object obj = m_AudioClipSource.result;
			if (m_AudioClipNames != null && m_AudioClipNames.Count > 0)
			{
				obj = m_AudioClipNames.Random();
			}
			if (obj != null)
			{
				AudioClip audioClip = obj as AudioClip;
				if (!audioClip)
				{
					audioClip = Singleton<AssetBundleManager>.instance.LoadFromName<AudioClip>(obj.ToString());
				}
				m_AudioClip = audioClip;
				float result = m_FadeInTime.GetResult<float>();
				AudioManager instance = Singleton<AudioManager>.instance;
				AudioClip clip = audioClip;
				float fadeTime = result;
				Ease ease = m_Ease;
				instance.PlayBGM(clip, fadeTime, 0f, ease);
			}
		}
	}
}
