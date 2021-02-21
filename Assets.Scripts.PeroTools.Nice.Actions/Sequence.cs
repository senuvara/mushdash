using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Interface;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class Sequence : Group
	{
		public override float duration => m_Playables.Sum((IPlayable a) => a.duration);

		public override void Execute()
		{
			if (m_Sequence != null)
			{
				m_Sequence.Kill();
			}
			int index = 0;
			System.Action[] callback = new System.Action[1];
			IPlayable action = null;
			callback[0] = delegate
			{
				if (index < m_Playables.Count)
				{
					m_Sequence = DOTween.Sequence();
					m_Sequence.SetUpdate(UpdateType.Fixed);
					if (action != null)
					{
						float num = (!(m_Time > 0f)) ? 0f : ((float)Mathf.RoundToInt((Time.fixedTime - m_Time - m_Playables[index].duration) / Time.fixedDeltaTime) * Time.fixedDeltaTime);
						m_Time = Time.fixedTime;
						m_Sequence.AppendInterval(action.duration - num);
					}
					action = m_Playables[index++];
					m_Sequence.AppendCallback(delegate
					{
						try
						{
							action.Execute();
						}
						catch (Exception ex)
						{
							Debug.LogWarning(ex.ToString());
						}
					});
					m_Sequence.Play();
					DG.Tweening.Sequence sequence = m_Sequence;
					m_Sequence = DOTween.Sequence();
					m_Sequence.SetUpdate(UpdateType.Fixed);
					sequence.OnComplete(delegate
					{
						callback[0]();
					});
				}
			};
			callback[0]();
		}
	}
}
