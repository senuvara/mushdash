using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Interface;
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public abstract class Group : Action
	{
		[SerializeField]
		protected List<IPlayable> m_Playables = new List<IPlayable>();

		protected DG.Tweening.Sequence m_Sequence;

		protected float m_Time = -1f;

		public override float duration
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < m_Playables.Count; i++)
				{
					IPlayable playable = m_Playables[i];
					if (playable is Wait)
					{
						num += playable.duration;
					}
				}
				return num;
			}
		}

		public List<IPlayable> playables => m_Playables;

		public List<T> GetPlayables<T>() where T : IPlayable
		{
			return m_Playables.FindAll((IPlayable p) => p is T).Cast<T>();
		}

		public override void Enter()
		{
			if (m_Playables == null)
			{
				return;
			}
			for (int i = 0; i < m_Playables.Count; i++)
			{
				if (m_Playables[i] != null)
				{
					try
					{
						m_Playables[i].Enter();
					}
					catch (Exception message)
					{
						Debug.LogError(message);
					}
				}
			}
		}

		public override void Execute()
		{
			if (m_Sequence != null)
			{
				m_Sequence.Kill();
			}
			if (duration <= 0f)
			{
				if (m_Playables == null)
				{
					return;
				}
				try
				{
					for (int i = 0; i < m_Playables.Count; i++)
					{
						if (m_Playables[i] != null)
						{
							m_Playables[i].Execute();
						}
					}
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
				return;
			}
			m_Sequence = DOTween.Sequence();
			for (int j = 0; j < m_Playables.Count; j++)
			{
				IPlayable playable = m_Playables[j];
				if (playable is Wait)
				{
					m_Sequence.AppendInterval(playable.duration);
					continue;
				}
				m_Sequence.AppendCallback(delegate
				{
					try
					{
						playable.Execute();
					}
					catch (Exception ex2)
					{
						Debug.LogWarning(ex2.ToString());
					}
				});
			}
			m_Sequence.Play();
		}

		public override void Exit()
		{
			if (m_Playables == null)
			{
				return;
			}
			for (int i = 0; i < m_Playables.Count; i++)
			{
				if (m_Playables[i] != null)
				{
					try
					{
						m_Playables[i].Exit();
					}
					catch (Exception message)
					{
						Debug.LogWarning(message);
					}
				}
			}
		}

		public override void Pause()
		{
			if (m_Sequence != null)
			{
				m_Sequence.Pause();
			}
			if (m_Playables != null)
			{
				for (int i = 0; i < m_Playables.Count; i++)
				{
					m_Playables[i].Pause();
				}
			}
		}

		public override void Resume()
		{
			if (m_Sequence != null && m_Sequence.IsPlaying())
			{
				m_Sequence.Play();
			}
			if (m_Playables != null)
			{
				for (int i = 0; i < m_Playables.Count; i++)
				{
					m_Playables[i].Resume();
				}
			}
		}
	}
}
