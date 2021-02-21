using Assets.Scripts.PeroTools.Commons;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.PeroTools.Managers
{
	public class TimerManager : Singleton<TimerManager>
	{
		private readonly List<Timer> m_FixedTimers = new List<Timer>();

		private readonly List<Timer> m_UpdateTimers = new List<Timer>();

		public Timer this[string uid]
		{
			get
			{
				List<Timer> list = new List<Timer>(m_FixedTimers);
				list.AddRange(m_UpdateTimers);
				return list.Find((Timer t) => t.uid == uid);
			}
		}

		private void Init()
		{
			SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("UpdateTimers", delegate
			{
				for (int j = 0; j < m_UpdateTimers.Count; j++)
				{
					Timer timer2 = m_UpdateTimers[j];
					if (!timer2.isPause)
					{
						timer2.Tick();
					}
				}
			});
			SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("FixedTimers", delegate
			{
				for (int i = 0; i < m_FixedTimers.Count; i++)
				{
					Timer timer = m_FixedTimers[i];
					if (!timer.isPause)
					{
						timer.Tick();
					}
				}
			});
		}

		public Timer Delay(Action callFunc, float dt)
		{
			if (dt <= 0f)
			{
				callFunc();
				return null;
			}
			string u = Guid.NewGuid().ToString();
			Timer timer = new Timer(u, UnityGameManager.LoopType.FixedUpdate, dt + 1f);
			timer.Play();
			timer.AddTickEvent(delegate
			{
				callFunc();
			}, dt);
			return timer;
		}

		public void Pause()
		{
			for (int i = 0; i < m_UpdateTimers.Count; i++)
			{
				Timer timer = m_UpdateTimers[i];
				timer.Pause();
			}
			for (int j = 0; j < m_FixedTimers.Count; j++)
			{
				Timer timer2 = m_FixedTimers[j];
				timer2.Pause();
			}
		}

		public void Resume()
		{
			for (int i = 0; i < m_UpdateTimers.Count; i++)
			{
				Timer timer = m_UpdateTimers[i];
				timer.Resume();
			}
			for (int j = 0; j < m_FixedTimers.Count; j++)
			{
				Timer timer2 = m_FixedTimers[j];
				timer2.Resume();
			}
		}

		public void Kill()
		{
			for (int i = 0; i < m_UpdateTimers.Count; i++)
			{
				Timer timer = m_UpdateTimers[i];
				timer.Kill();
			}
			for (int j = 0; j < m_FixedTimers.Count; j++)
			{
				Timer timer2 = m_FixedTimers[j];
				timer2.Kill();
			}
		}

		public void AddTimer(Timer timer)
		{
			switch (timer.type)
			{
			case UnityGameManager.LoopType.Update:
				m_UpdateTimers.Add(timer);
				break;
			case UnityGameManager.LoopType.FixedUpdate:
				m_FixedTimers.Add(timer);
				break;
			}
		}

		public void RemoveTimer(Timer timer)
		{
			switch (timer.type)
			{
			case UnityGameManager.LoopType.Update:
				m_UpdateTimers.Remove(timer);
				break;
			case UnityGameManager.LoopType.FixedUpdate:
				m_FixedTimers.Remove(timer);
				break;
			}
		}
	}
}
