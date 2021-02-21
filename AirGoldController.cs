using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using GameLogic;
using UnityEngine;

public class AirGoldController : BaseEnemyObjectController
{
	private bool isBeAttacked;

	public override bool OnControllerMiss(int idx)
	{
		return false;
	}

	public override void OnControllerAttacked(int result, bool isDeaded)
	{
		if (isBeAttacked)
		{
			return;
		}
		isBeAttacked = true;
		Singleton<BattleEnemyManager>.instance.SetPlayResult(idx, 4);
		SpineActionController.Play("out", idx);
		if (idx < GameMusicScene.instance.animators.Length)
		{
			Animator animator = GameMusicScene.instance.animators[idx];
			if ((bool)animator)
			{
				animator.speed = 0f;
			}
		}
		AttacksController.Instance.PlayOneShot(m_MusicData.noteData.key_audio, AttacksController.KeyAudioType.Touch);
	}
}
