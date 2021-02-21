using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using DG.Tweening;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class PlayAnimation : Action
	{
		[SerializeField]
		[Variable(typeof(UnityEngine.Object), null, false)]
		private IVariable m_Object;

		[SerializeField]
		[Variable(typeof(bool), null, false)]
		private IVariable m_Loop;

		[SerializeField]
		[Variable(1f, null, false)]
		private IVariable m_Speed;

		[SerializeField]
		private int m_Layer;

		[SerializeField]
		private AnimationType m_AnimationType;

		[SerializeField]
		private ActionType m_ActionType;

		[SerializeField]
		[Variable(typeof(string), "OnAnimNameGUI", false)]
		private List<IVariable> m_AnimNames = new List<IVariable>
		{
			new Constance()
		};

		[SerializeField]
		private UnityEngine.Object m_AnimObject;

		[SerializeField]
		private float m_Duration;

		private bool m_IsPlaying;

		public override float duration => m_Duration;

		public bool isComplete
		{
			get
			{
				SkeletonAnimation @object = GameUtils.GetObject<SkeletonAnimation>(m_Object.result);
				Animator object2 = GameUtils.GetObject<Animator>(m_Object.result);
				DOTweenAnimation object3 = GameUtils.GetObject<DOTweenAnimation>(m_Object.result);
				SkeletonGraphic object4 = GameUtils.GetObject<SkeletonGraphic>(m_Object.result);
				if (!m_IsPlaying)
				{
					return true;
				}
				if ((bool)object2)
				{
					AnimatorStateInfo currentAnimatorStateInfo = object2.GetCurrentAnimatorStateInfo(0);
					if (currentAnimatorStateInfo.loop)
					{
						return true;
					}
					return currentAnimatorStateInfo.normalizedTime >= 0.99f;
				}
				if ((bool)@object && @object.state.Tracks.Count > 0)
				{
					TrackEntry current = @object.state.GetCurrent(0);
					if (current.Loop)
					{
						return true;
					}
					return current.IsComplete;
				}
				if ((bool)object4 && object4.AnimationState.Tracks.Count > 0)
				{
					TrackEntry current2 = object4.AnimationState.GetCurrent(0);
					if (current2.Loop)
					{
						return true;
					}
					return current2.IsComplete;
				}
				if ((bool)object3)
				{
					if (object3.tween.Loops() == -1)
					{
						return true;
					}
					return object3.tween.IsComplete();
				}
				return true;
			}
		}

		public override void Execute()
		{
			SkeletonAnimation skeletonAnimation = m_AnimObject as SkeletonAnimation;
			Animator animator = m_AnimObject as Animator;
			DOTweenAnimation doTweenAnimation = m_AnimObject as DOTweenAnimation;
			SkeletonGraphic skeletonGraphic = m_AnimObject as SkeletonGraphic;
			bool result = m_Loop.GetResult<bool>();
			float result2 = m_Speed.GetResult<float>();
			if ((bool)skeletonAnimation)
			{
				string animationName = m_AnimNames.Select(VariableUtils.GetResult<string>).Random();
				skeletonAnimation.loop = result;
				skeletonAnimation.timeScale = result2;
				switch (m_ActionType)
				{
				case ActionType.KeepCurrent:
					m_Duration = skeletonAnimation.state.AddAnimation(m_Layer, animationName, result, 0f).Animation.Duration;
					break;
				case ActionType.Reset:
					m_Duration = skeletonAnimation.state.SetAnimation(m_Layer, animationName, result).Animation.Duration;
					break;
				}
			}
			else if ((bool)skeletonGraphic)
			{
				string animationName2 = m_AnimNames.Select(VariableUtils.GetResult<string>).Random();
				skeletonGraphic.timeScale = result2;
				switch (m_ActionType)
				{
				case ActionType.KeepCurrent:
					m_Duration = skeletonGraphic.AnimationState.AddAnimation(m_Layer, animationName2, result, 0f).Animation.Duration;
					break;
				case ActionType.Reset:
					m_Duration = skeletonGraphic.AnimationState.SetAnimation(m_Layer, animationName2, result).Animation.Duration;
					break;
				}
			}
			else if ((bool)animator)
			{
				string animName = m_AnimNames.Select(VariableUtils.GetResult<string>).Random();
				AnimationClip animationClip = animator.runtimeAnimatorController.animationClips.ToList().Find((AnimationClip a) => a.name == animName);
				if ((bool)animationClip)
				{
					m_Duration = animationClip.length;
					animationClip.wrapMode = (result ? WrapMode.Loop : WrapMode.Default);
				}
				animator.speed = result2;
				switch (m_ActionType)
				{
				case ActionType.KeepCurrent:
					SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
					{
						m_IsPlaying = false;
						if (animator.isInitialized)
						{
							animator.Play(animName, m_Layer, 0f);
						}
					}, new Func<bool>(this, __ldftn(PlayAnimation.get_isComplete)));
					break;
				case ActionType.Reset:
					if (animator.isInitialized)
					{
						animator.Play(animName, m_Layer, 0f);
					}
					break;
				}
			}
			else
			{
				if (!doTweenAnimation || doTweenAnimation.tween == null)
				{
					return;
				}
				doTweenAnimation.tween.timeScale = result2;
				m_Duration = doTweenAnimation.tween.Duration(false);
				switch (m_ActionType)
				{
				case ActionType.KeepCurrent:
					SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
					{
						m_IsPlaying = false;
						switch (m_AnimationType)
						{
						case AnimationType.Forward:
							doTweenAnimation.tween.PlayForward();
							break;
						case AnimationType.Reverse:
							doTweenAnimation.tween.PlayBackwards();
							break;
						}
					}, new Func<bool>(this, __ldftn(PlayAnimation.get_isComplete)));
					break;
				case ActionType.Reset:
					doTweenAnimation.tween.Restart();
					break;
				}
			}
		}
	}
}
