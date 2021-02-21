using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Actions;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.PreWarm;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[HideMonoScript]
	public abstract class Event : SerializedMonoBehaviour, IPlayable, IPreWarm
	{
		[SerializeField]
		[ShowIf("IsShowActionGroup", true)]
		[HideLabel]
		[HideReferenceObjectPicker]
		private Group m_Group = new MeanWhile();

		private bool m_HasEnter;

		protected EventInvoker m_Invoker = new EventInvoker();

		public List<IPlayable> playables => m_Group.playables;

		public float duration => m_Group.duration;

		public static EventInvoker OnEvent(GameObject gameObject, Type eventType)
		{
			Event @event = gameObject.GetOrAddComponent(eventType) as Event;
			if (@event != null)
			{
				return @event.m_Invoker;
			}
			return null;
		}

		public List<T> GetPlayables<T>() where T : IPlayable
		{
			return m_Group.GetPlayables<T>();
		}

		public virtual void PreWarm(int slice)
		{
			if (slice == 0)
			{
				Enter();
			}
		}

		private void Awake()
		{
			Enter();
		}

		private void OnDestroy()
		{
			Exit();
		}

		protected virtual void OnEnter()
		{
		}

		protected virtual void OnExecute()
		{
		}

		protected virtual void OnExit()
		{
		}

		public void Enter()
		{
			if (!m_HasEnter)
			{
				m_HasEnter = true;
				try
				{
					OnEnter();
				}
				catch (Exception)
				{
				}
				if (m_Group != null)
				{
					m_Group.Enter();
				}
			}
		}

		public void Execute()
		{
			m_Invoker.Invoke(this);
			try
			{
				OnExecute();
			}
			catch (Exception)
			{
			}
			if (m_Group != null)
			{
				m_Group.Execute();
			}
		}

		public void Exit()
		{
			try
			{
				OnExit();
			}
			catch (Exception)
			{
			}
			if (m_Group != null)
			{
				m_Group.Exit();
			}
		}

		public void Pause()
		{
			m_Group.Pause();
		}

		public void Resume()
		{
			m_Group.Resume();
		}
	}
}
