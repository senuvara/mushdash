using Assets.Scripts.PeroTools.Commons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Managers
{
	public class Timer
	{
		public const float infinite = float.MaxValue;

		public UnityGameManager.LoopType type;

		public bool isPause;

		private int m_PrePassTick;

		private readonly float m_Radio;

		private readonly Dictionary<int, Action<int>> m_Events = new Dictionary<int, Action<int>>();

		private readonly List<Action<int>> m_Steps = new List<Action<int>>();

		public static float fixedInterval => Time.fixedDeltaTime;

		public static float updateInterval => Time.deltaTime;

		public float interval
		{
			get;
			private set;
		}

		public static int length => fixedInterval.ToString(CultureInfo.InvariantCulture).Split('.')[1].Length;

		public string uid
		{
			get;
			private set;
		}

		public float totalTime
		{
			get;
			private set;
		}

		public float passedTime => (float)passedTick * interval;

		public int passedTick => (int)((float)stopWatch.ElapsedMilliseconds / m_Radio);

		public Stopwatch stopWatch
		{
			get;
			private set;
		}

		public Timer(string u, UnityGameManager.LoopType t = UnityGameManager.LoopType.FixedUpdate, float time = float.MaxValue)
		{
			uid = u;
			type = t;
			totalTime = time;
			isPause = true;
			m_PrePassTick = -1;
			interval = ((type != 0) ? fixedInterval : updateInterval);
			m_Radio = interval / 0.001f;
			stopWatch = new Stopwatch();
			Singleton<TimerManager>.instance.AddTimer(this);
		}

		public void AddTickEvent(Action<int> action, float time = -1f)
		{
			int key = Mathf.RoundToInt(time / interval);
			if (time < 0f)
			{
				if (!m_Steps.Contains(action))
				{
					m_Steps.Add(action);
				}
			}
			else if (!m_Events.ContainsKey(key))
			{
				m_Events.Add(key, action);
			}
		}

		public void RemoveTickEvent(Action<int> action)
		{
			if (m_Events.ContainsValue(action))
			{
				int key = m_Events.ToList().Find((KeyValuePair<int, Action<int>> e) => e.Value == action).Key;
				m_Events.Remove(key);
			}
			else if (m_Steps.Contains(action))
			{
				m_Steps.Remove(action);
			}
		}

		public void Play()
		{
			isPause = false;
			stopWatch.Reset();
			stopWatch.Start();
		}

		public void Pause()
		{
			isPause = true;
			stopWatch.Stop();
		}

		public void Resume()
		{
			isPause = false;
			stopWatch.Start();
		}

		public void Kill()
		{
			isPause = true;
			stopWatch.Stop();
			Singleton<TimerManager>.instance.RemoveTimer(this);
		}

		public void Tick()
		{
			int passedTick = this.passedTick;
			if (totalTime > 0f && (float)passedTick * interval > totalTime)
			{
				Kill();
			}
			else if (passedTick != m_PrePassTick)
			{
				TickStep(passedTick);
				TickEvent(passedTick);
				m_PrePassTick = passedTick;
			}
		}

		private void TickStep(int curTick)
		{
			if (m_Steps.Count == 0)
			{
				return;
			}
			for (int i = m_PrePassTick + 1; i <= curTick; i++)
			{
				for (int j = 0; j < m_Steps.Count; j++)
				{
					m_Steps[j](i);
				}
			}
		}

		private void TickEvent(int curTick)
		{
			if (m_Events.Count == 0)
			{
				return;
			}
			for (int i = m_PrePassTick + 1; i <= curTick; i++)
			{
				if (m_Events.ContainsKey(i))
				{
					m_Events[i](i);
				}
			}
		}
	}
}
