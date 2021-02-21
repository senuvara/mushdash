using Assets.Scripts.Graphics;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using DYUnityLib;
using FormulaBase;
using GameLogic;
using System;
using System.Collections;
using UnityEngine;

public class GameSceneMainController : SingletonMonoBehaviour<GameSceneMainController>
{
	public static int curResolutionWidth;

	public static int curResolutionHeight;

	[HideInInspector]
	public static bool isEditorMode => bool.Parse(SingletonScriptableObject<ConstanceManager>.instance["IsEditorMode"]);

	private void Awake()
	{
		Singleton<EffectManager>.instance.Preload("dust_fx");
	}

	private void Start()
	{
		if (isEditorMode)
		{
			curResolutionWidth = Screen.width;
			curResolutionHeight = Screen.height;
		}
		else
		{
			curResolutionWidth = GraphicSettings.curScreenWidth;
			curResolutionHeight = GraphicSettings.curScreenHeight;
			GameInit();
		}
	}

	public void Delay(float t, Action callback)
	{
		StartCoroutine(DelayAction(t, callback));
	}

	private IEnumerator DelayAction(float t, Action callback)
	{
		yield return new WaitForSeconds(t);
		callback();
	}

	public void GameInit()
	{
		Singleton<StageBattleComponent>.instance.InitById(Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].GetResult<int>());
		Singleton<StageBattleComponent>.instance.InitGame();
		SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("GameSceneMainController_1", delegate
		{
			GameGlobal.gTouch.TouchEventPhaseUpdate();
		}, UnityGameManager.LoopType.Update);
		SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("GameSceneMainController", delegate
		{
			if (!FixUpdateTimer.IsPausing() && GameGlobal.gTouch != null && GameGlobal.gGameMusic != null)
			{
				GameGlobal.gTouch.TouchEventPhase();
				GameGlobal.gGameMusic.GameMusicFixTimerUpdate();
			}
		});
	}

	private void OnDisable()
	{
		SingletonMonoBehaviour<UnityGameManager>.instance.UnregLoop("GameSceneMainController_1");
		SingletonMonoBehaviour<UnityGameManager>.instance.UnregLoop("GameSceneMainController");
	}

	private void OnDestroy()
	{
	}

	private void OnApplicationPause()
	{
		if (Singleton<StageBattleComponent>.instance != null)
		{
			Singleton<StageBattleComponent>.instance.OnInterrupt();
		}
	}
}
