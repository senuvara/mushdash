using GameLogic;
using UnityEngine;

namespace Assets.Scripts.GameCore.GameObjectLogics.GameObjectControl
{
	public class OnBPMChangeSpeed : StateMachineBehaviour
	{
		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			animator.speed = stateInfo.length * MusicConfigReader.Instance.bpm / 60f;
		}
	}
}
