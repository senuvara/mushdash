using Assets.Scripts.PeroTools.Commons;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.PeroTools.Managers
{
	public class EffectManager : Singleton<EffectManager>
	{
		private readonly List<Effect> m_Effects = new List<Effect>();

		private GameObject m_Center;

		public GameObject center
		{
			get
			{
				if (!m_Center)
				{
					m_Center = new GameObject("Effects");
				}
				return m_Center;
			}
		}

		public Effect this[string uid] => m_Effects.Find((Effect e) => e.uid == uid);

		private void Init()
		{
			Singleton<SceneManager>.instance.onSceneChanged += delegate(Scene scene1, Scene scene2)
			{
				if (scene2.name.Contains("Loading"))
				{
					UnloadAll();
				}
			};
		}

		public void Pause()
		{
			m_Effects.For(delegate(Effect e)
			{
				e.Pause();
			});
		}

		public void Resume()
		{
			m_Effects.For(delegate(Effect e)
			{
				e.Resume();
			});
		}

		public void Preload(string[] preloadUids, int preload = 5, int capacity = -1, Transform parent = null)
		{
			preloadUids.For(delegate(string preloadEffect)
			{
				Preload(preloadEffect, preload, capacity, parent);
			});
		}

		public void Preload(GameObject[] gos, int preload = 5, int capacity = -1, Transform parent = null)
		{
			gos.For(delegate(GameObject go)
			{
				Preload(go, preload, capacity, parent);
			});
		}

		public Effect Preload(string uid, int preload = 5, int capacity = -1, Transform parent = null)
		{
			Effect effect = this[uid];
			if (effect == null)
			{
				effect = new Effect(uid, preload, capacity, parent);
				m_Effects.Add(effect);
				return effect;
			}
			return effect;
		}

		public Effect Preload(GameObject go, int preload = 5, int capacity = -1, Transform parent = null)
		{
			Effect effect = this[go.name];
			if (effect == null)
			{
				effect = new Effect(go, preload, capacity, parent);
				m_Effects.Add(effect);
				return effect;
			}
			return effect;
		}

		public GameObject Play(string uid, int preload = 5, int capacity = -1, Transform parent = null)
		{
			if (string.IsNullOrEmpty(uid))
			{
				return null;
			}
			Effect effect = this[uid];
			if (effect == null)
			{
				effect = new Effect(uid, preload, capacity, parent);
				m_Effects.Add(effect);
			}
			return effect.CreateInstance();
		}

		public GameObject Play(GameObject go, int preload = 5, int capacity = -1, Transform parent = null)
		{
			Effect effect = this[go.name];
			if (effect == null)
			{
				effect = new Effect(go, preload, capacity, parent);
				m_Effects.Add(effect);
			}
			return effect.CreateInstance();
		}

		public void Unload(string uid)
		{
			Unload(this[uid]);
		}

		public void Unload(Effect effect)
		{
			effect.Clear();
			m_Effects.Remove(effect);
		}

		public void UnloadAll()
		{
			Debug.Log("Unload all effects!");
			List<Effect> array = new List<Effect>(m_Effects);
			array.For(Unload);
			m_Effects.Clear();
		}
	}
}
