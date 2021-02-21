using DG.Tweening;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.PeroTools.Commons
{
	public class DOTweenUtils
	{
		private static readonly List<Sequence> m_Seqs = new List<Sequence>();

		public static void Kill()
		{
			for (int i = 0; i < m_Seqs.Count; i++)
			{
				m_Seqs[i].Kill();
			}
		}

		public static void Pause()
		{
			for (int i = 0; i < m_Seqs.Count; i++)
			{
				m_Seqs[i].Pause();
			}
		}

		public static void Resume()
		{
			for (int i = 0; i < m_Seqs.Count; i++)
			{
				m_Seqs[i].Play();
			}
		}

		public static Sequence Delay(Action callFunc, float dt)
		{
			if (Math.Abs(dt) <= 0f)
			{
				callFunc();
				return null;
			}
			Sequence seq = DOTween.Sequence();
			seq.AppendInterval(dt);
			seq.AppendCallback(delegate
			{
				callFunc();
				m_Seqs.Remove(seq);
				seq.Kill();
			});
			seq.Play();
			m_Seqs.Add(seq);
			return seq;
		}

		public static Sequence Update(Action completeFunc, Func<bool> stopFunc)
		{
			Sequence seq = DOTween.Sequence();
			seq.AppendInterval(float.MaxValue);
			seq.OnUpdate(delegate
			{
				if (stopFunc())
				{
					if (completeFunc != null)
					{
						completeFunc();
					}
					seq.Kill();
				}
			});
			seq.Play();
			return seq;
		}
	}
}
