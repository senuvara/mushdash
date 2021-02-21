using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Interface;
using System;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class Random : Group
	{
		private IPlayable m_Playable;

		public override float duration
		{
			get
			{
				if (m_Playable is Wait)
				{
					return m_Playable.duration;
				}
				return 0f;
			}
		}

		public override void Execute()
		{
			m_Playable = m_Playables.Random();
			try
			{
				m_Playable.Execute();
			}
			catch (Exception message)
			{
				Debug.LogWarning(message);
			}
		}
	}
}
