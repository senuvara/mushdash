using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class PlaySound : Action
	{
		[SerializeField]
		[Variable(typeof(Object), null, false)]
		[OnConstanceChanged("OnConstanceChanged")]
		private IVariable m_AudioClipSource;

		[SerializeField]
		[Variable(1f, null, false)]
		private IVariable m_Volume;

		private AudioClip m_AudioClip;

		[SerializeField]
		[HideInInspector]
		private List<string> m_AudioClipNames;

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

		public override void Enter()
		{
			if (m_AudioClipNames != null)
			{
				m_AudioClipNames.For(delegate(string name)
				{
					Singleton<AssetBundleManager>.instance.LoadFromName<AudioClip>(name);
				});
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
				Singleton<AudioManager>.instance.PlayOneShot(audioClip, (m_Volume == null) ? 1f : m_Volume.GetResult<float>());
			}
		}
	}
}
