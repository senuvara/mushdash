using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using DG.Tweening;
using E7.Native;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Managers
{
	public class AudioManager : Singleton<AudioManager>
	{
		private Tweener m_BGMTweener;

		private Dictionary<string, NativeAudioPointer> m_AndroidAudios;

		public FastPool pool
		{
			get;
			private set;
		}

		public AudioSource bgm
		{
			get;
			private set;
		}

		public GameObject gameObject
		{
			get;
			private set;
		}

		private void Init()
		{
			int dspBufferSize = 384;
			AudioConfiguration configuration = AudioSettings.GetConfiguration();
			configuration.dspBufferSize = dspBufferSize;
			AudioSettings.Reset(configuration);
			this.gameObject = new GameObject("AudioManager");
			this.gameObject.transform.SetParent(SingletonMonoBehaviour<UnityGameManager>.instance.persistenceRoot);
			GameObject gameObject = new GameObject("BGM");
			gameObject.transform.SetParent(this.gameObject.transform);
			bgm = gameObject.AddComponent<AudioSource>();
			bgm.loop = true;
			bgm.volume = 1f;
			GameObject gameObject2 = new GameObject("Sfx");
			gameObject2.transform.SetParent(this.gameObject.transform);
			AudioSource audioSource = gameObject2.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
			pool = Singleton<PoolManager>.instance.MakeFastPool(audioSource, 10, 20, this.gameObject.transform);
			gameObject2.SetActive(false);
			Singleton<SceneManager>.instance.onSceneChanged += delegate
			{
				StopBGM();
				StopSound();
			};
		}

		public void Preload(string assetName, bool isAndroid = false)
		{
			AudioClip audioClip = Singleton<AssetBundleManager>.instance.LoadFromName<AudioClip>(assetName);
			if (!isAndroid)
			{
			}
		}

		public void Preload(IList<string> assetNames, bool isAndroid)
		{
			if (assetNames != null)
			{
				for (int i = 0; i < assetNames.Count; i++)
				{
					Preload(assetNames[i], isAndroid);
				}
			}
		}

		public void Unload(IList<string> unloadUids)
		{
		}

		public void Unload()
		{
		}

		public AudioSource PlayLoop(AudioClip clip)
		{
			if (pool == null)
			{
				return null;
			}
			AudioSource audioSource = pool.FastInstantiate<AudioSource>(gameObject.transform);
			audioSource.clip = clip;
			audioSource.loop = true;
			audioSource.Play();
			return audioSource;
		}

		public AudioSource PlayLoop(string name)
		{
			return PlayLoop(Singleton<AssetBundleManager>.instance.LoadFromName<AudioClip>(name));
		}

		public AudioSource PlayOneShot(AudioClip clip, float volume = 1f, Action callback = null)
		{
			if (pool == null)
			{
				return null;
			}
			AudioSource audioSource = pool.FastInstantiate<AudioSource>(gameObject.transform);
			audioSource.gameObject.SetActive(true);
			audioSource.enabled = true;
			audioSource.mute = false;
			audioSource.PlayOneShot(clip, volume);
			if (callback != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
				{
					if (audioSource.gameObject.activeSelf)
					{
						callback();
						Singleton<PoolManager>.instance.FastDestroy(audioSource.gameObject);
					}
				}, () => !audioSource.gameObject.activeSelf || !audioSource.isPlaying);
			}
			return audioSource;
		}

		public AudioSource PlayOneShot(string name, float volume = 1f, Action callback = null)
		{
			return PlayOneShot(Singleton<AssetBundleManager>.instance.LoadFromName<AudioClip>(name), volume, callback);
		}

		public void PauseSfx()
		{
			pool.gameObjects.For(delegate(GameObject go)
			{
				if ((bool)go && go.activeInHierarchy)
				{
					AudioSource component = go.GetComponent<AudioSource>();
					component.Pause();
				}
			});
		}

		public void ResumeSfx()
		{
			pool.gameObjects.For(delegate(GameObject go)
			{
				if ((bool)go && go.activeInHierarchy)
				{
					AudioSource component = go.GetComponent<AudioSource>();
					component.UnPause();
				}
			});
		}

		public NativeAudioController PlayAndroidOneShot(string name, float volume = 1f, int index = -1, Action callback = null)
		{
			return null;
		}

		public void PlayBGM(string name, float fadeTime = 0f, float volumeFrom = 0f, Ease ease = Ease.Linear, bool forceReplay = false)
		{
			AudioClip clip = Singleton<AssetBundleManager>.instance.LoadFromName<AudioClip>(name);
			PlayBGM(clip, fadeTime, volumeFrom, ease, forceReplay);
		}

		public void PlayBGM(AudioClip clip, float fadeTime = 0f, float volumeFrom = 0f, Ease ease = Ease.Linear, bool forceReplay = false)
		{
			if (!forceReplay && bgm.clip == clip)
			{
				return;
			}
			if (fadeTime > 0f)
			{
				StopBGM();
				bgm.timeSamples = 0;
				bgm.clip = clip;
				bgm.loop = true;
				bgm.Play();
				if (m_BGMTweener != null)
				{
					m_BGMTweener.Complete(true);
				}
				float volume = bgm.volume;
				bgm.volume = volumeFrom;
				m_BGMTweener = bgm.DOFade(volume, fadeTime).SetEase(ease).OnComplete(delegate
				{
					bgm.volume = volume;
					m_BGMTweener = null;
				});
			}
			else
			{
				bgm.timeSamples = 0;
				bgm.clip = clip;
				bgm.Play();
			}
		}

		public void StopSound()
		{
			pool.FastDestroyAll();
		}

		public void StopBGM()
		{
			if (bgm != null && bgm.clip != null)
			{
				bgm.Stop();
				bgm.clip = null;
			}
		}
	}
}
