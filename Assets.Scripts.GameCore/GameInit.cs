using Assets.Scripts.Common.XDSDK;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.UI;
using FormulaBase;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.GameCore
{
	public class GameInit : MonoBehaviour
	{
		public static int maxAlbumIndex;

		private void Awake()
		{
			TaskManager instance = Singleton<TaskManager>.instance;
			StageManager instance2 = Singleton<StageManager>.instance;
			Screen.sleepTimeout = -2;
			Screen.autorotateToLandscapeLeft = true;
			Screen.autorotateToLandscapeRight = true;
			SpecialLogic();
			if (Singleton<DataManager>.instance["Account"]["UseLoadingIndex"].GetResult<List<int>>().Count <= 0)
			{
				Singleton<DataManager>.instance["Account"]["LoadingNull"].SetResult(false);
			}
			if (Singleton<XDSDKManager>.instance.isLogined || Singleton<XDSDKManager>.instance.IsXDLoggedIn())
			{
				SingletonMonoBehaviour<MainManager>.instance.InitLoading("LoadingFirstLogin");
			}
		}

		private void SpecialLogic()
		{
			maxAlbumIndex = int.Parse(Singleton<ConfigManager>.instance.GetJson("albums", false)[2]["jsonName"].ToString().Replace("ALBUM", string.Empty));
			int result = Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>();
			if (Singleton<ConfigManager>.instance.GetConfigBoolValue("character", result, "hide"))
			{
				Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].SetResult(0);
			}
			List<IData> result2 = Singleton<DataManager>.instance["Account"]["Items"].GetResult<List<IData>>();
			List<string> list = new List<string>();
			for (int j = 0; j < result2.Count; j++)
			{
				IData data = result2[j];
				int result3 = data["chipAmount"].GetResult<int>();
				int result4 = data["count"].GetResult<int>();
				if (result4 > result3 || result4 == 0)
				{
					data["count"].SetResult(result3);
				}
				string item = data["type"].GetResult<string>() + data["index"].GetResult<int>();
				if (list.Contains(item))
				{
					result2.Remove(data);
				}
				else
				{
					list.Add(item);
				}
			}
			result2.RemoveAll((IData i) => i["type"].GetResult<string>() == "character" && i["index"].GetResult<int>() == 13 && !i["isUnlock"].GetResult<bool>());
			if (!result2.Exists((IData i) => i["type"].GetResult<string>() == "character" && i["index"].GetResult<int>() == 13 && i["isUnlock"].GetResult<bool>()))
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					Singleton<ItemManager>.instance.AddExtraItem("character", 13, 8, false);
				}, 1f);
			}
			Singleton<ItemManager>.instance.ChristmasItemLogic("character");
			if (Singleton<ItemManager>.instance.IsChirstmas())
			{
				Singleton<ItemManager>.instance.CheckAndAddWelcome(3, 5, false);
			}
			List<string> result5 = Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>();
			for (int k = 0; k < result5.Count; k++)
			{
				if (string.IsNullOrEmpty(result5[k]) || result5[k].StartsWith("A"))
				{
					result5.Remove(result5[k]);
					Debug.Log("collection Remove.");
				}
				Singleton<DataManager>.instance.Save();
			}
			if (Singleton<XDSDKManager>.instance.IsLoggedIn())
			{
				if (Singleton<DataManager>.instance["IAP"]["music_package_25"].GetResult<bool>() || BtnIAP.IsUnlockAll())
				{
					Singleton<ItemManager>.instance.AddExtraItem("character", 14, 8, false);
					Singleton<ItemManager>.instance.AddExtraItem("loading", 29, 5, false);
				}
				else
				{
					if (Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>() == 14)
					{
						Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].SetResult(0);
					}
					Singleton<ItemManager>.instance.RemoveItem("character", 14);
				}
				if (Singleton<DataManager>.instance["IAP"]["music_package_29"].GetResult<bool>() || BtnIAP.IsUnlockAll())
				{
					Singleton<ItemManager>.instance.AddExtraItem("character", 15, 8, false);
					Singleton<ItemManager>.instance.AddExtraItem("loading", 34, 5, false);
				}
				else
				{
					if (Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>() == 15)
					{
						Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].SetResult(0);
					}
					Singleton<ItemManager>.instance.RemoveItem("character", 15);
				}
				if (Singleton<DataManager>.instance["IAP"]["music_package_33"].GetResult<bool>() || BtnIAP.IsUnlockAll())
				{
					Singleton<ItemManager>.instance.AddExtraItem("character", 16, 8, false);
					Singleton<ItemManager>.instance.CheckAndAddWelcome(8, 5, false);
				}
				else
				{
					if (Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>() == 16)
					{
						Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].SetResult(0);
					}
					Singleton<ItemManager>.instance.RemoveItem("character", 16);
				}
			}
			else
			{
				if (Singleton<DataManager>.instance["IAP"]["music_package_25"].GetResult<bool>() || BtnIAP.IsUnlockAll())
				{
					Singleton<ItemManager>.instance.AddExtraItem("character", 14, 8, false);
					Singleton<ItemManager>.instance.AddExtraItem("loading", 29, 5, false);
				}
				else
				{
					if (Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>() == 14)
					{
						Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].SetResult(0);
					}
					Singleton<ItemManager>.instance.RemoveItem("character", 14);
				}
				if (Singleton<DataManager>.instance["IAP"]["music_package_29"].GetResult<bool>() || BtnIAP.IsUnlockAll())
				{
					Singleton<ItemManager>.instance.AddExtraItem("character", 15, 8, false);
					Singleton<ItemManager>.instance.AddExtraItem("loading", 34, 5, false);
				}
				else
				{
					if (Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>() == 15)
					{
						Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].SetResult(0);
					}
					Singleton<ItemManager>.instance.RemoveItem("character", 15);
				}
				if (Singleton<DataManager>.instance["IAP"]["music_package_33"].GetResult<bool>() || BtnIAP.IsUnlockAll())
				{
					Singleton<ItemManager>.instance.AddExtraItem("character", 16, 8, false);
					Singleton<ItemManager>.instance.CheckAndAddWelcome(8, 5, false);
				}
				else
				{
					if (Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>() == 16)
					{
						Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].SetResult(0);
					}
					Singleton<ItemManager>.instance.RemoveItem("character", 16);
				}
			}
			if (!Singleton<ItemManager>.instance.TimeIsNanahira() && (Singleton<DataManager>.instance["IAP"]["music_package_27"].GetResult<bool>() || BtnIAP.IsUnlockAll()))
			{
				Singleton<ItemManager>.instance.CheckAndAddWelcome(5, 5, false);
			}
			if (Singleton<DataManager>.instance["IAP"]["music_package_33"].GetResult<bool>() || BtnIAP.IsUnlockAll())
			{
				Singleton<ItemManager>.instance.CheckAndAddWelcome(8, 5, false);
			}
			if (Singleton<DataManager>.instance["IAP"]["music_package_32"].GetResult<bool>() || BtnIAP.IsUnlockAll())
			{
				Singleton<ItemManager>.instance.CheckAndAddWelcome(6, 5, false);
			}
			DateTime today = DateTime.Today;
			if (today.Year == 2020 && today.Month == 6 && today.Day >= 12 && today.Day <= 19)
			{
				Singleton<ItemManager>.instance.AddExtraItem("loading", 30, 5);
			}
			if ((today.Month == 10 && today.Day >= 30) || (today.Month == 11 && today.Day <= 6))
			{
				Singleton<ItemManager>.instance.AddExtraItem("loading", 35, 5);
			}
			if (today.Month == 2 && today.Day >= 5 && today.Day <= 18)
			{
				Singleton<ItemManager>.instance.CheckAndAddWelcome(7, 5, false);
			}
			Singleton<DataManager>.instance["Account"]["IsNekoSkillAvailable"].SetResult(true);
		}

		private void WaitBackInit()
		{
			float waitTime = float.Parse(SingletonScriptableObject<ConstanceManager>.instance["WaitBackTime"]);
			float time = 0f;
			SingletonMonoBehaviour<UnityGameManager>.instance.UnregLoop("IsTapping");
			SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("IsTapping", delegate
			{
				EventSystem current = EventSystem.current;
				if ((bool)current && current.IsInvoking())
				{
					time = 0f;
				}
				else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
				{
					time = 0f;
				}
			}, UnityGameManager.LoopType.Update);
			SingletonMonoBehaviour<UnityGameManager>.instance.UnregLoop("WaitToBack");
			SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("WaitToBack", delegate
			{
				if (!Singleton<SceneManager>.instance.scene.name.Contains("Welcome") && !Singleton<StageBattleComponent>.instance.IsAutoPlay())
				{
					if (time < waitTime)
					{
						time += Time.deltaTime;
					}
					else
					{
						time = 0f;
						GameObject gameObject = Singleton<UIManager>.instance["PnlHome"];
						GameObject gameObject2 = Singleton<UIManager>.instance["PnlMenu"];
						if ((bool)gameObject2 && gameObject2.activeInHierarchy)
						{
							ComeBack();
						}
						if (gameObject == null || !gameObject.activeInHierarchy)
						{
							ComeBack();
						}
					}
				}
			}, UnityGameManager.LoopType.Update);
		}

		private void ComeBack()
		{
			if (Singleton<SceneManager>.instance.sceneName.Contains("GameMain"))
			{
				Singleton<StageBattleComponent>.instance.Exit("UISystem_PC", null, false);
				return;
			}
			Singleton<AudioManager>.instance.bgm.Stop();
			Singleton<SceneManager>.instance.LoadSceneViaLoadingScene("UISystem_PC");
		}
	}
}
