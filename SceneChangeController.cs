using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Events;
using FormulaBase;
using GameLogic;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeController : BaseEnemyObjectController
{
	public static int curScene;

	public override void OnControllerStart()
	{
		base.OnControllerStart();
		int num = curScene = Singleton<StageBattleComponent>.instance.sceneInfo[m_MusicData.noteData.ibms_id];
		for (int i = 0; i < GameMusicScene.instance.scenes.Count; i++)
		{
			GameObject gameObject = GameMusicScene.instance.scenes[i];
			gameObject.GetComponent<OnStart>().enabled = false;
			if (gameObject.name == "scene_0" + num || gameObject.name == "scene_0" + num + "_christmas")
			{
				GameMusicScene.instance.curSceneName = gameObject.name;
				gameObject.SetActive(true);
				Boss.Instance.SceneBossChange(num);
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
		GameObject[] preloads = GameGlobal.gGameMusicScene.preloads;
		for (int j = 0; j < preloads.Length; j++)
		{
			GameObject gameObject2 = preloads[j];
			if (!gameObject2 || !gameObject2.activeSelf)
			{
				continue;
			}
			List<GameObject>[] preloads2 = GameGlobal.gGameMusicScene.preloads1;
			if (preloads2.Length - 1 >= j && preloads2[j] != null)
			{
				GameObject gameObject3 = preloads2[j][num];
				SpineActionController spineActionController = GameGlobal.gGameMusicScene.spineActionCtrls[j];
				if (gameObject3 != null && gameObject3 != gameObject2 && (string.IsNullOrEmpty(spineActionController.curActionKey) || spineActionController.curActionKey == "in"))
				{
					GameGlobal.gGameMusicScene.objCtrls1[j][curScene].SetVisible(false);
					gameObject3.SetActive(true);
					BaseSpineObjectController baseSpineObjectController = GameGlobal.gGameMusicScene.objCtrls[j];
					baseSpineObjectController.SetVisible(false);
					GameGlobal.gGameMusicScene.objCtrls1[j][curScene].SetVisible(true);
					float startDelay = GameGlobal.gGameMusicScene.spineActionCtrls[j].startDelay;
					float startDelay2 = GameGlobal.gGameMusicScene.spineActionCtrls1[j][curScene].startDelay;
					GameGlobal.gGameMusicScene.Replace(j, num);
					SpineActionController.Play(spineActionController.curActionKey, j, spineActionController.GetCurrentAnimationTime() + startDelay2 - startDelay);
				}
			}
		}
	}
}
