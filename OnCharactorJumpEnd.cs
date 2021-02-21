using Assets.Scripts.PeroTools.Commons;
using UnityEngine;

public class OnCharactorJumpEnd : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		SingletonMonoBehaviour<GirlManager>.instance.SetJumpingAction(false);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}
}
