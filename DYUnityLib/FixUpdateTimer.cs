using Assets.Scripts.PeroTools.Commons;
using FormulaBase;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DYUnityLib
{
	public class FixUpdateTimer
	{
		private static bool _isPausing = false;

		public static List<FixUpdateTimer> timers = new List<FixUpdateTimer>();

		private uint[][] m_OldIdx = new uint[100][];

		private int[] m_Ticks = new int[100];

		public const int TIMER_TYPE_EVENT_ARRAY = 0;

		public const int TIMER_TYPE_STEP_ARRAY = 1;

		private int iType;

		private const int precision = 100;

		public const decimal dInterval = 0.01m;

		public const float fInterval = 0.01f;

		private int passedTick = -1;

		private int prePassedTick;

		private int passedCount;

		private int totalTick;

		private uint defaultEvent;

		private bool isPause;

		private int m_curPassedTick;

		private uint[][] events;

		public static void RollTimer()
		{
			for (int i = 0; i < timers.Count; i++)
			{
				timers[i].OnTick();
			}
		}

		public static void PauseTimer()
		{
			for (int i = 0; i < timers.Count; i++)
			{
				timers[i].Pause();
			}
			_isPausing = true;
		}

		public static void ResumeTimer()
		{
			for (int i = 0; i < timers.Count; i++)
			{
				timers[i].Resume();
			}
			_isPausing = false;
		}

		public static bool IsPausing()
		{
			return _isPausing;
		}

		public void Init(decimal _totalTick, int timerType = 0)
		{
			isPause = true;
			passedTick = -1;
			passedCount = 0;
			iType = timerType;
			totalTick = (int)(_totalTick * 100m);
			if (!timers.Contains(this))
			{
				timers.Add(this);
			}
		}

		public void Run()
		{
			passedTick = -1;
			passedCount = 0;
			prePassedTick = 0;
			Resume();
		}

		public bool IsRunning()
		{
			return !isPause;
		}

		public void Cancel()
		{
			passedTick = -1;
			passedCount = 0;
			if (timers.Contains(this))
			{
				timers.Remove(this);
			}
		}

		public void Pause()
		{
			isPause = true;
		}

		public void Resume()
		{
			isPause = false;
		}

		public void AddTickEvent(decimal tick, uint eventIndex)
		{
			if (iType == 1)
			{
				defaultEvent = eventIndex;
			}
			int num = Mathf.RoundToInt((float)(tick * 100m));
			if (num >= events.Length)
			{
				Array.Resize(ref events, num + Singleton<StageBattleComponent>.instance.resizeAdd);
				events[num] = new uint[1]
				{
					eventIndex
				};
			}
			else
			{
				uint[] array = events[num] ?? new uint[0];
				events[num] = array.Add(eventIndex);
			}
		}

		public void ClearTickEvent()
		{
			defaultEvent = 0u;
			events = new uint[0][];
		}

		public decimal GetPassTick()
		{
			return (decimal)passedTick * 0.01m;
		}

		private void __OnTickEventArray()
		{
			if (totalTick >= 0 && passedTick > totalTick)
			{
				Cancel();
				return;
			}
			int num = 0;
			for (int i = prePassedTick + 1; i <= passedTick; i++)
			{
				if (i < events.Length)
				{
					uint[] array = events[i];
					if (array != null && array.Length > 0)
					{
						m_Ticks[num] = i;
						m_OldIdx[num] = array;
						num++;
					}
				}
			}
			if (num <= 0)
			{
				return;
			}
			for (int j = 0; j < num; j++)
			{
				decimal tick = (decimal)m_Ticks[j] * 0.01m;
				uint[] array2 = m_OldIdx[j];
				foreach (uint eventIndex in array2)
				{
					GTrigger.FireEvent(eventIndex, tick);
				}
			}
			passedCount++;
		}

		private void __OnTickStepArray()
		{
			if (totalTick >= 0 && passedTick > totalTick)
			{
				Cancel();
				return;
			}
			for (int i = prePassedTick + 1; i <= passedTick; i++)
			{
				GTrigger.FireEvent(defaultEvent, (decimal)i * 0.01m);
			}
		}

		private void OnTick()
		{
			if (isPause)
			{
				return;
			}
			m_curPassedTick = Mathf.RoundToInt(Singleton<StageBattleComponent>.instance.timeFromMusicStart * 100f);
			if (m_curPassedTick > passedTick)
			{
				SetPassTick(m_curPassedTick);
				if (iType == 1)
				{
					__OnTickStepArray();
				}
				else if (iType == 0)
				{
					__OnTickEventArray();
				}
			}
		}

		private void SetPassTick(int tick)
		{
			prePassedTick = passedTick;
			passedTick = tick;
		}
	}
}
