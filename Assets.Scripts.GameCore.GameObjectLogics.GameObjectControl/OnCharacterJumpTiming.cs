using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using UnityEngine;

namespace Assets.Scripts.GameCore.GameObjectLogics.GameObjectControl
{
	public class OnCharacterJumpTiming : StateMachineBehaviour
	{
		public enum JumpState
		{
			Start,
			End
		}

		[Range(0f, 1f)]
		public float percent;

		public JumpState state;

		public bool endDust = true;

		private static Coroutine m_StartCoroutine;

		private static Coroutine m_EndCoroutine;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (m_StartCoroutine != null && state == JumpState.Start)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_StartCoroutine);
				m_StartCoroutine = null;
			}
			if (m_EndCoroutine != null && state == JumpState.End)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_EndCoroutine);
				m_EndCoroutine = null;
			}
			if (state == JumpState.Start)
			{
				m_StartCoroutine = Invoke(animator, stateInfo);
			}
			else
			{
				m_EndCoroutine = Invoke(animator, stateInfo);
			}
		}

		private Coroutine Invoke(Animator animator, AnimatorStateInfo stateInfo)
		{
			return SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				if ((bool)animator && animator.GetCurrentAnimatorStateInfo(0).shortNameHash == stateInfo.shortNameHash)
				{
					if (endDust)
					{
						Singleton<EffectManager>.instance.Play("dust_fx");
					}
					if (state == JumpState.Start)
					{
						SingletonMonoBehaviour<GirlManager>.instance.SetJumpingAction(true);
					}
					else
					{
						SingletonMonoBehaviour<GirlManager>.instance.SetJumpingAction(false);
					}
				}
			}, percent * stateInfo.length);
		}
	}
}
