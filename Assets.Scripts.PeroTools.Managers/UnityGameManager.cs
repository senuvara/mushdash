using Assets.Scripts.PeroTools.Commons;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.PeroTools.Managers
{
	[ExecuteInEditMode]
	public class UnityGameManager : SingletonMonoBehaviour<UnityGameManager>
	{
		public class Looper
		{
			public const float infinite = float.MaxValue;

			public bool isPause;

			private readonly UnityAction<float> m_Action;

			private readonly float m_LoopTime;

			private float m_RunTime;

			private readonly float m_DeltaTime;

			public string uid
			{
				get;
				private set;
			}

			public LoopType type
			{
				get;
				private set;
			}

			public bool isActive => m_RunTime <= m_LoopTime;

			public Looper(string u, UnityAction<float> a, LoopType t, float time = float.MaxValue)
			{
				uid = u;
				m_Action = a;
				type = t;
				m_LoopTime = time;
				m_DeltaTime = ((type != 0) ? Timer.fixedInterval : Timer.updateInterval);
			}

			public void Loop()
			{
				if (!isPause)
				{
					m_RunTime += m_DeltaTime;
					m_Action(m_RunTime);
				}
			}
		}

		public enum LoopType
		{
			Update,
			FixedUpdate
		}

		public const string touches = "Touches";

		public const string keys = "Keys";

		public const string showFps = "ShowFps";

		public const string updateTimers = "UpdateTimers";

		public const string fixedTimer = "FixedTimers";

		private readonly List<Looper> m_UpdateLoops = new List<Looper>();

		private readonly List<Looper> m_FixedLoops = new List<Looper>();

		private Looper m_LoopFixedUpdate;

		private Looper m_LoopUpdate;

		private Looper m_UnregLooper;

		public Transform persistenceRoot => base.transform;

		public event Action onApplicationQuit;

		public event Action<bool> onApplicationFocusChange;

		public Looper RegLoop(UnityAction<float> action, LoopType type = LoopType.FixedUpdate, float loopTime = float.MaxValue)
		{
			return RegLoop(Guid.NewGuid().ToString(), action, type, loopTime);
		}

		public Looper RegLoop(string uid, UnityAction<float> action, LoopType type = LoopType.FixedUpdate, float loopTime = float.MaxValue)
		{
			Looper looper = new Looper(uid, action, type, loopTime);
			List<Looper> list = (type != 0) ? m_FixedLoops : m_UpdateLoops;
			if (!list.Exists((Looper l) => l.uid == uid))
			{
				list.Add(looper);
			}
			return looper;
		}

		public void UnregLoop(string uid)
		{
			m_UnregLooper = m_UpdateLoops.Find((Looper l) => l.uid == uid);
			if (m_UnregLooper != null)
			{
				m_UpdateLoops.Remove(m_UnregLooper);
				return;
			}
			m_FixedLoops.RemoveAll((Looper l) => l.uid == uid);
		}

		public void LoopFixedUpdate()
		{
			for (int i = 0; i < m_FixedLoops.Count; i++)
			{
				m_LoopFixedUpdate = m_FixedLoops[i];
				if (!m_LoopFixedUpdate.isActive)
				{
					UnregLoop(m_LoopFixedUpdate.uid);
				}
				else
				{
					m_LoopFixedUpdate.Loop();
				}
			}
		}

		public void LoopUpdate()
		{
			for (int i = 0; i < m_UpdateLoops.Count; i++)
			{
				m_LoopUpdate = m_UpdateLoops[i];
				if (!m_LoopUpdate.isActive)
				{
					UnregLoop(m_LoopUpdate.uid);
				}
				else
				{
					m_LoopUpdate.Loop();
				}
			}
		}

		private void OnApplicationQuit()
		{
			if (this.onApplicationQuit != null)
			{
				this.onApplicationQuit();
			}
		}

		private void Update()
		{
			LoopUpdate();
		}

		private void FixedUpdate()
		{
			LoopFixedUpdate();
		}

		private void Init()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			if (this.onApplicationFocusChange != null)
			{
				this.onApplicationFocusChange(hasFocus);
			}
		}
	}
}
