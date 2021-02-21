using Assets.Scripts.PeroTools.Commons;
using DG.Tweening;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameCore.GameObjectLogics.GameObjectControl
{
	public class PlayAnimation : StateMachineBehaviour
	{
		public enum Timing
		{
			Enter,
			Exit
		}

		public enum PlayType
		{
			Skeleton,
			DOTween,
			Animation,
			Animator
		}

		public enum ActionType
		{
			Reset,
			KeepCurrent
		}

		public PlayType playType;

		public List<string> animationNames;

		public bool isLoop;

		public ActionType actionType;

		public Timing timing;

		public float delay;

		private SkeletonAnimation m_SAnimation;

		private UnityEngine.Animation m_Animation;

		private Sequence m_DelaySeq;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (timing == Timing.Enter)
			{
				Play(animator);
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (timing == Timing.Exit)
			{
				Play(animator);
			}
		}

		private void Play(Animator animator)
		{
			string sName = animationNames.Random();
			if (string.IsNullOrEmpty(sName))
			{
				return;
			}
			switch (playType)
			{
			case PlayType.DOTween:
				break;
			case PlayType.Animator:
				if (m_DelaySeq != null)
				{
					m_DelaySeq.Kill();
				}
				m_DelaySeq = DOTweenUtils.Delay(delegate
				{
					animator.Play(sName, 0, 0f);
				}, delay);
				break;
			case PlayType.Skeleton:
			{
				if (!m_SAnimation)
				{
					m_SAnimation = animator.gameObject.GetComponent<SkeletonAnimation>();
				}
				if (actionType == ActionType.Reset)
				{
					m_SAnimation.state.SetAnimation(0, sName, isLoop);
					break;
				}
				TrackEntry current = m_SAnimation.state.GetCurrent(0);
				if (current == null || !current.Loop)
				{
					m_SAnimation.state.AddAnimation(0, sName, isLoop, delay);
				}
				break;
			}
			case PlayType.Animation:
				if (!m_Animation)
				{
					m_Animation = animator.gameObject.GetComponent<UnityEngine.Animation>();
				}
				if (m_DelaySeq != null)
				{
					m_DelaySeq.Kill();
				}
				m_DelaySeq = DOTweenUtils.Delay(delegate
				{
					if (actionType == ActionType.Reset)
					{
						if (m_Animation.isPlaying && !m_Animation.IsPlaying(sName))
						{
							m_Animation.Stop();
						}
						m_Animation.Play(sName, PlayMode.StopAll);
					}
					else if (!m_Animation.IsPlaying(sName))
					{
						m_Animation.Play(sName, PlayMode.StopAll);
					}
				}, delay);
				break;
			}
		}
	}
}
