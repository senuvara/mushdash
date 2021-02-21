using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using UnityEngine;

public class OnToneChange : StateMachineBehaviour
{
	public enum ToneState
	{
		Ground,
		Air
	}

	[Range(0f, 1f)]
	public float percent;

	public ToneState state;

	private static Coroutine m_AirCoroutine;

	private static Coroutine m_GroundCoroutine;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (m_AirCoroutine != null && state == ToneState.Air)
		{
			SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_AirCoroutine);
			m_AirCoroutine = null;
		}
		if (m_GroundCoroutine != null && state == ToneState.Ground)
		{
			SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_GroundCoroutine);
			m_GroundCoroutine = null;
		}
		if (state == ToneState.Air)
		{
			m_AirCoroutine = Invoke(animator, stateInfo);
		}
		else
		{
			m_GroundCoroutine = Invoke(animator, stateInfo);
		}
	}

	private Coroutine Invoke(Animator animator, AnimatorStateInfo stateInfo)
	{
		float value = (percent - stateInfo.normalizedTime) * stateInfo.length;
		return SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
		{
			if ((bool)animator && animator.GetCurrentAnimatorStateInfo(0).shortNameHash == stateInfo.shortNameHash)
			{
				SingletonMonoBehaviour<GirlManager>.instance.SetTone(state != ToneState.Ground);
			}
		}, (decimal)value);
	}
}
