using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using UnityEngine;

public class OnCharactorJumpStart : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		SingletonMonoBehaviour<GirlManager>.instance.SetJumpingAction(true);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		SingletonMonoBehaviour<GirlManager>.instance.SetJumpingAction(false);
		Singleton<EffectManager>.instance.Play("dust_fx");
		if (!Singleton<BattleEnemyManager>.instance.isAirPressing && !Singleton<BattleEnemyManager>.instance.isGroundPressing)
		{
			SpineActionController.Play("char_run", -1);
		}
	}
}
