using Assets.Scripts.Common.XDSDK;
using Assets.Scripts.GameCore;
using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.Graphics;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.UI;
using Assets.Scripts.UI.Controls;
using Assets.Scripts.UI.Panels;
using FormulaBase;
using GameLogic;
using Newtonsoft.Json.Linq;
using SA.Common.Pattern;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common
{
	public class CustomDefines
	{
		public static readonly Dictionary<string, object> entities = new Dictionary<string, object>
		{
			{
				"Net/IsLogin",
				(Func<object[], object>)((object[] param) => Assets.Scripts.PeroTools.Commons.Singleton<XDSDKManager>.instance.IsLoggedIn())
			},
			{
				"IAP/IsIAPEnabled",
				(Func<object[], object>)((object[] param) => BtnIAP.isAvaiable)
			},
			{
				"IAP/IsPurchased",
				(Func<object[], object>)delegate(object[] param)
				{
					string uid = (string)param[0];
					return Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["IAP"][uid].GetResult<bool>() || BtnIAP.IsUnlockAll();
				}
			},
			{
				"IAP/IsUnlockAll",
				(Func<object[], object>)((object[] param) => BtnIAP.IsUnlockAll())
			},
			{
				"IAP/IsPurchasedMusic",
				(Func<object[], object>)delegate(object[] param)
				{
					List<string> result25 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>();
					if (result25 == null || result25.Count <= 0)
					{
						return false;
					}
					int num7 = (int)param[0];
					if (num7 >= result25.Count)
					{
						return false;
					}
					string str = result25[(int)param[0]];
					string text9 = $"music_package_{str.BeginBefore('-')}";
					return (text9 == "music_package_0") ? ((object)true) : ((object)Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["IAP"][text9].GetResult<bool>());
				}
			},
			{
				"IAP/IsFirstDlc",
				(Func<object[], object>)delegate
				{
					int num5 = 0;
					int count = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance["albums"].Count;
					for (int num6 = 1; num6 < count; num6++)
					{
						string configStringValue3 = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue("albums", num6, "uid");
						if (Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["IAP"][configStringValue3].GetResult<bool>())
						{
							num5++;
						}
					}
					return num5 == 1;
				}
			},
			{
				"Game/IsPause",
				(Func<object[], object>)((object[] param) => Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isPause)
			},
			{
				"Game/SelectedMusicName",
				(Func<object>)delegate
				{
					string result22 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"].GetResult<string>();
					int result23 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].GetResult<int>();
					if (result22 != "collections" || result23 < 0)
					{
						return Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue(Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedAlbumName"].GetResult<string>(), "uid", "name", Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicUidFromInfoList"].GetResult<string>());
					}
					List<string> result24 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>();
					if (result24.Count == 0 || result24.Count < result23)
					{
						return "?????";
					}
					string text8 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>()[result23];
					string fileName4 = $"ALBUM{int.Parse(text8.Split('-')[0]) + 1}";
					return Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue(fileName4, "uid", "name", text8);
				}
			},
			{
				"Game/SelectedDemoName",
				(Func<object>)delegate
				{
					string result20 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"].GetResult<string>();
					int result21 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].GetResult<int>();
					if (result20 != "collections" || result21 < 0)
					{
						return Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue(Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedAlbumName"].GetResult<string>(), "uid", "demo", Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicUidFromInfoList"].GetResult<string>());
					}
					string text7 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>()[result21];
					string fileName3 = $"ALBUM{int.Parse(text7.Split('-')[0]) + 1}";
					return Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue(fileName3, "uid", "demo", text7);
				}
			},
			{
				"Game/SelectedMusicAuthor",
				(Func<object>)delegate
				{
					string result18 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"].GetResult<string>();
					if (result18 != "collections")
					{
						return Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue(Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedAlbumName"].GetResult<string>(), "uid", "author", Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicUidFromInfoList"].GetResult<string>());
					}
					List<string> result19 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>();
					if (result19.Count == 0 || result19.Count < Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].GetResult<int>())
					{
						return "???";
					}
					string text6 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>()[Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].GetResult<int>()];
					string fileName2 = $"ALBUM{int.Parse(text6.Split('-')[0]) + 1}";
					return Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue(fileName2, "uid", "author", text6);
				}
			},
			{
				"Game/SelectedMusicLevelDesignerName",
				(Func<object>)delegate
				{
					string result14 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"].GetResult<string>();
					int result15 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].GetResult<int>();
					int result16 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedDifficulty"].GetResult<int>();
					if (result14 != "collections" || result15 < 0)
					{
						string configStringValue = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue(Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedAlbumName"].GetResult<string>(), "uid", "levelDesigner", Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicUidFromInfoList"].GetResult<string>());
						return Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue(Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedAlbumName"].GetResult<string>(), "uid", (!string.IsNullOrEmpty(configStringValue)) ? "levelDesigner" : ("levelDesigner" + result16), Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicUidFromInfoList"].GetResult<string>());
					}
					List<string> result17 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>();
					if (result17.Count == 0 || result17.Count < result15)
					{
						return "?????";
					}
					string text5 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>()[result15];
					string fileName = $"ALBUM{int.Parse(text5.Split('-')[0]) + 1}";
					string configStringValue2 = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue(fileName, "uid", "levelDesigner", text5);
					return Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue(fileName, "uid", (!string.IsNullOrEmpty(configStringValue2)) ? "levelDesigner" : ("levelDesigner" + result16), text5);
				}
			},
			{
				"Game/IsFool",
				(Func<object>)(() => DateTime.Now.Day == 1 && DateTime.Now.Month == 4)
			},
			{
				"Battle/IsSceneChangeType",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isSceneChangeType)
			},
			{
				"Battle/CurSceneName",
				(Func<object>)(() => GameMusicScene.instance.curSceneName)
			},
			{
				"Battle/Score",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetScore())
			},
			{
				"Battle/AddScore",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetAddScore())
			},
			{
				"Battle/Combo",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.GetCombo())
			},
			{
				"Battle/MultiHit",
				(Func<object>)(() => MultHitEnemyController.hitCount)
			},
			{
				"Battle/ScoreGet",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetAddScore())
			},
			{
				"Battle/Hurt",
				(Func<object>)(() => BattleRoleAttributeComponent.instance.GetHurtValue())
			},
			{
				"Battle/HpGet",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetRecover())
			},
			{
				"Battle/Progress",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.GetTimeProgress())
			},
			{
				"Battle/Hp",
				(Func<object>)(() => BattleRoleAttributeComponent.instance.GetHp())
			},
			{
				"Battle/HpMax",
				(Func<object>)(() => BattleRoleAttributeComponent.instance.GetHpMax())
			},
			{
				"Battle/HpRate",
				(Func<object>)(() => BattleRoleAttributeComponent.instance.HpRate())
			},
			{
				"Battle/Fever",
				(Func<object>)(() => Mathf.RoundToInt(FeverManager.Instance.GetWholeFever()))
			},
			{
				"Battle/FeverMax",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<BattleProperty>.instance.maxFever)
			},
			{
				"Battle/FeverRate",
				(Func<object>)(() => FeverManager.Instance.GetFeverRate())
			},
			{
				"Battle/MaxCombo",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetComboMax())
			},
			{
				"Battle/LatencyState",
				(Func<object>)(() => BattleRoleAttributeComponent.instance.state)
			},
			{
				"Battle/Early",
				(Func<object>)(() => BattleRoleAttributeComponent.instance.early)
			},
			{
				"Battle/Late",
				(Func<object>)(() => BattleRoleAttributeComponent.instance.late)
			},
			{
				"Battle/HitCount",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetHitCount())
			},
			{
				"Battle/PerfectPercent",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetHitPercent(4u).ToString("P2"))
			},
			{
				"Battle/Accuracy",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetAccuracy().ToString("P2"))
			},
			{
				"Battle/AccuracyRate",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetAccuracy())
			},
			{
				"Battle/Perfect",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetHitCountByResult(4u))
			},
			{
				"Battle/Great",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetHitCountByResult(3u))
			},
			{
				"Battle/Pass",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetBlock())
			},
			{
				"Battle/Miss",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetComboMiss())
			},
			{
				"Battle/Evaluate",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetStageEvaluate().Key)
			},
			{
				"Battle/HideNoteCount",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetHideNoteHitCount())
			},
			{
				"Battle/IsAutoPlay",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.IsAutoPlay())
			},
			{
				"Battle/IsLeftRight",
				(Func<object>)(() => GameTouchPlay.isLeftRight)
			},
			{
				"Battle/IsTutorial",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isTutorial)
			},
			{
				"Battle/Miss2Great",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<BattleProperty>.instance.missToGreat)
			},
			{
				"Battle/Great2Perfect",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<BattleProperty>.instance.greatToPerfect)
			},
			{
				"Battle/SceneName",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.GetSceneName())
			},
			{
				"Battle/GetHideBMSDifficulty",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.GetHideBMSDifficulty())
			},
			{
				"Battle/CurScene",
				(Func<object>)(() => SceneChangeController.curScene)
			},
			{
				"Battle/IsGCScene",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<BattleProperty>.instance.isGCScene)
			},
			{
				"Game/IsEditorMode",
				(Func<object>)(() => GameSceneMainController.isEditorMode)
			},
			{
				"UI/StageAchievements",
				(Func<object[], object>)delegate(object[] param)
				{
					if (param == null)
					{
						return 0;
					}
					string cmpValue = Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.GetMusicIndex().ToString();
					int diffcult = (int)Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.GetDiffcult();
					return string.IsNullOrEmpty((string)param[0]) ? ((object)0) : ((object)Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigIntValue(Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.GetAlbumName(), "id", (string)param[0] + diffcult, cmpValue));
				}
			},
			{
				"Game/SystemTimeMonth",
				(Func<object>)(() => DateTime.Now.Month)
			},
			{
				"Game/SystemTimeDay",
				(Func<object>)(() => DateTime.Now.Day)
			},
			{
				"Game/SystemTimeYear",
				(Func<object>)(() => DateTime.Now.Year)
			},
			{
				"Game/IsInGame",
				(Func<object>)(() => Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isInGame)
			},
			{
				"Game/IsTapTap",
				(Func<object>)(() => false)
			},
			{
				"Game/IsAndroid",
				(Func<object>)(() => false)
			},
			{
				"Game/IsIOS",
				(Func<object>)(() => false)
			},
			{
				"Game/IsGoogle",
				(Func<object>)(() => false)
			},
			{
				"Game/IsNs",
				(Func<object>)(() => false)
			},
			{
				"Game/IsWegame",
				(Func<object>)(() => false)
			},
			{
				"Game/IsSteam",
				(Func<object>)(() => true)
			},
			{
				"Game/IsPC",
				(Func<object>)(() => true)
			},
			{
				"Game/IsCustomController",
				(Func<object>)(() => ControllerUtils.IsCustomController())
			},
			{
				"UI/CurCollectionAlbumName",
				(Func<object>)delegate
				{
					List<string> result12 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>();
					int result13 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].GetResult<int>();
					string text4 = (result13 >= result12.Count || result13 < 0) ? "0-0" : result12[result13];
					return $"ALBUM{int.Parse(text4.Split('-')[0]) + 1}";
				}
			},
			{
				"UI/CurCollectionUid",
				(Func<object>)delegate
				{
					List<string> list2 = new List<string>();
					List<string> result10 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>();
					for (int num4 = 0; num4 < result10.Count; num4++)
					{
						string text3 = result10[num4];
						try
						{
							if (int.Parse(text3.BeginBefore('-')) <= GameInit.maxAlbumIndex)
							{
								list2.Add(text3);
							}
						}
						catch (Exception)
						{
						}
					}
					int result11 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].GetResult<int>();
					return (result11 >= list2.Count || result11 < 0) ? "0-0" : list2[result11];
				}
			},
			{
				"UI/CollectionAlbumName",
				(Func<object[], object>)delegate(object[] param)
				{
					List<string> result9 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>();
					int num3 = (param.Length > 0) ? ((int)param[0]) : 0;
					string text2 = (num3 >= result9.Count || num3 < 0) ? "0-0" : result9[num3];
					return $"ALBUM{int.Parse(text2.Split('-')[0]) + 1}";
				}
			},
			{
				"UI/CollectionUid",
				(Func<object[], object>)delegate(object[] param)
				{
					List<string> list = new List<string>();
					List<string> result8 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>();
					for (int l = 0; l < result8.Count; l++)
					{
						string text = result8[l];
						if (int.Parse(text.BeginBefore('-')) <= GameInit.maxAlbumIndex)
						{
							list.Add(text);
						}
					}
					int num2 = (param.Length > 0) ? ((int)param[0]) : 0;
					result8 = list;
					return (num2 >= result8.Count || num2 < 0) ? "0-0" : result8[num2];
				}
			},
			{
				"UI/StoreUrl",
				(Func<object>)(() => RateOurGame.GetMuseDashStoreUrl())
			},
			{
				"UI/StoreDirectUrl",
				(Func<object>)(() => RateOurGame.GetMuseDashStoreUrl(false))
			},
			{
				"UI/BGMName",
				(Func<object>)(() => (Assets.Scripts.PeroTools.Commons.Singleton<AudioManager>.instance.bgm.clip != null) ? Assets.Scripts.PeroTools.Commons.Singleton<AudioManager>.instance.bgm.clip.name : null)
			},
			{
				"UI/CurBMSSInfo",
				(Func<object[], object>)delegate(object[] param)
				{
					if (param == null)
					{
						return 0;
					}
					if (!GameSceneMainController.isEditorMode)
					{
						return Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicName"].GetResult<string>();
					}
					return (MusicConfigReader.Instance.bms == null) ? ((object)0) : ((string)MusicConfigReader.Instance.bms.info[(string)param[0]]);
				}
			},
			{
				"UI/CurVersion",
				(Func<object[], object>)((object[] param) => "v" + Application.version)
			},
			{
				"UI/IsEroEroVersion",
				(Func<object[], object>)delegate
				{
					if (Application.version == "1.3.1" && !Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["IsEroEroVersion"].GetResult<bool>())
					{
						Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["IsEroEroVersion"].SetResult(true);
						return true;
					}
					return false;
				}
			},
			{
				"UI/IAPCount",
				(Func<object[], object>)((object[] param) => BtnIAP.GetIAPCount())
			},
			{
				"UI/BMSSInfo",
				(Func<object[], object>)((object[] param) => (param == null) ? ((object)0) : UITest.instance.GetValue((int)param[0], (string)param[1]))
			},
			{
				"UI/GetCoverByName",
				(Func<object[], object>)delegate(object[] param)
				{
					if (param == null)
					{
						return 0;
					}
					string b2 = (string)param[0];
					for (int i = 1; i < int.MaxValue; i++)
					{
						string name = $"ALBUM{i}";
						JArray json = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetJson(name, false);
						if (json == null)
						{
							break;
						}
						for (int k = 0; k < json.Count; k++)
						{
							JToken jToken = json[k];
							if ((string)jToken["name"] == b2)
							{
								return (string)jToken["cover"];
							}
						}
					}
					return null;
				}
			},
			{
				"UI/BMSSDifficultyInfo",
				(Func<object[], object>)((object[] param) => (param == null) ? ((object)0) : UITest.instance.GetValue((int)param[0], (int)param[1], (string)param[2]))
			},
			{
				"UI/WHRatio",
				(Func<object[], object>)((object[] param) => 1f * (float)GraphicSettings.curScreenWidth / (float)GraphicSettings.curScreenHeight - 0.01f)
			},
			{
				"UI/IsDoubleIndex",
				(Func<object[], object>)delegate(object[] param)
				{
					if (param == null)
					{
						return false;
					}
					int num = int.Parse((string)param[0]);
					return num % 2 == 1;
				}
			},
			{
				"UI/ExistDifficulty",
				(Func<object[], object>)((object[] param) => (param == null) ? ((object)false) : ((object)UITest.instance.ExistDifficulty((int)param[0], (int)param[1])))
			},
			{
				"UI/BMSIsIdxSelected",
				(Func<object[], object>)((object[] param) => (param == null) ? ((object)false) : ((object)(Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetObject<int>("EditorModeIndex") == (int)param[0])))
			},
			{
				"UI/BMSIsDiffSelected",
				(Func<object[], object>)((object[] param) => (param == null) ? ((object)false) : ((object)(Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetObject<int>("EditorModeDifficulty") == (int)param[0])))
			},
			{
				"UI/TaskRefreshTime",
				(Func<object[], object>)((object[] param) => Assets.Scripts.PeroTools.Commons.Singleton<TaskManager>.instance.refreshGap.ToString().BeginBefore('.'))
			},
			{
				"UI/Rank",
				(Func<object[], object>)((object[] param) => Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.rank)
			},
			{
				"UI/IsNewPlayer",
				(Func<object[], object>)((object[] param) => Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isNew)
			},
			{
				"UI/MusyncxURL",
				(Func<object[], object>)((object[] param) => "https://itunes.apple.com/cn/app/id935528099")
			},
			{
				"UI/NanoCoreURL",
				(Func<object[], object>)((object[] param) => "https://nc.xd.com/yx/")
			},
			{
				"UI/HasBulletin",
				(Func<object[], object>)((object[] param) => Assets.Scripts.PeroTools.Commons.Singleton<BulletinManager>.instance.bulletins != null && !Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isNew)
			},
			{
				"UI/CharacterCount",
				(Func<object[], object>)((object[] param) => Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetJson("character", false).Count((JToken j) => !(bool)j["hide"]))
			},
			{
				"UI/GameBrightnessColor",
				(Func<object[], object>)delegate
				{
					List<IData> result7 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["GameConfig"]["Brightnesses"].GetResult<List<IData>>();
					IData data2 = result7.Find((IData b) => b["Uid"].GetResult<string>() == Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.GetSceneName());
					return (data2 != null) ? ((object)new Color(0f, 0f, 0f, 1f - data2["Brightness"].GetResult<float>())) : ((object)new Color(0f, 0f, 0f, 0f));
				}
			},
			{
				"GM/IsDev",
				(Func<object[], object>)((object[] param) => false)
			},
			{
				"UI/ShowLoginTip",
				(Func<object[], object>)((object[] param) => Assets.Scripts.PeroTools.Commons.Singleton<XDSDKManager>.instance.showTip)
			},
			{
				"UI/IsWeekFreeSongUid",
				(Func<object[], object>)((object[] param) => Assets.Scripts.PeroTools.Commons.Singleton<WeekFreeManager>.instance.freeSongUids.Contains(param[0].ToString()))
			},
			{
				"UI/IsWeekFreeAlbumUid",
				(Func<object[], object>)((object[] param) => Assets.Scripts.PeroTools.Commons.Singleton<WeekFreeManager>.instance.freeAlbumUids.Contains(param[0].ToString()))
			},
			{
				"UI/AddItemCount",
				(Func<object[], object>)((object[] param) => Assets.Scripts.PeroTools.Commons.Singleton<ItemManager>.instance.addCount)
			}
		};

		public static readonly Dictionary<string, object> events = new Dictionary<string, object>
		{
			{
				"Net/Sync",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<ServerManager>.instance.Synchronize();
				}
			},
			{
				"Net/Login",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<XDSDKManager>.instance.Login();
				}
			},
			{
				"Net/Logout",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<XDSDKManager>.instance.Logout();
					SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
					{
						if (Assets.Scripts.PeroTools.Commons.Singleton<XDSDKManager>.instance.isOvearSea)
						{
						}
					}, 0.1f);
				}
			},
			{
				"Game/Save",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance.Save();
				}
			},
			{
				"Game/ResetUserData",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.ResetUserData();
				}
			},
			{
				"Game/UnlockAll",
				(EventManager.EventCallFunc)delegate
				{
					StageBattleComponent.UnlockAll();
				}
			},
			{
				"Game/BattleStart",
				(EventManager.EventCallFunc)delegate
				{
					if (!Assets.Scripts.PeroTools.Commons.Singleton<XDSDKManager>.instance.IsLoggedIn() && GameGlobal.onClickNotReachableNumber++ == 0 && !GameGlobal.isCytusHideBMS)
					{
						Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("UI/OnNoNetRankTip");
					}
					else
					{
						Assets.Scripts.PeroTools.Commons.Singleton<AudioManager>.instance.bgm.Stop();
						int result5 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].GetResult<int>();
						int result6 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedDifficulty"].GetResult<int>();
						Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.InitById(result5);
						Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.SetMusicIndex(result5);
						Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.SetDifficulty(result6);
						Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.Enter(result5);
						Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.RewiredJoystickControllerMapSetEnable(false, "UI");
						Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.SwitchHandleProposal((!Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isTutorial) ? Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.handleProposal : "Default");
					}
				}
			},
			{
				"Game/InvokeFever",
				(EventManager.EventCallFunc)delegate
				{
					FeverManager.Instance.InvokeFever();
				}
			},
			{
				"Game/SetGameIsOn",
				(EventManager.EventCallFunc)delegate(object[] param)
				{
					Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isInGame = (bool)param[0];
				}
			},
			{
				"Game/HowToPlay",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.TutorialSetting();
					Assets.Scripts.PeroTools.Commons.Singleton<AudioManager>.instance.bgm.Stop();
					if (Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["IsNew"].GetResult<bool>())
					{
						Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["GameConfig"].Reload();
						Assets.Scripts.PeroTools.Commons.Singleton<XDSDKManager>.instance.Logout();
						Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isNew = true;
					}
					Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.Enter(-1);
				}
			},
			{
				"Game/LoginAndLoadScene",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("UI/DisableInputKey");
					Assets.Scripts.PeroTools.Commons.Singleton<SceneManager>.instance.LoadSceneViaLoadingScene("UISystem_PC", delegate
					{
						Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("UI/EnableInputKey");
					});
				}
			},
			{
				"Game/SetAutoPlay",
				(EventManager.EventCallFunc)delegate(object[] param)
				{
					Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.SetAutoPlay(bool.Parse(param[0].ToString()));
				}
			},
			{
				"Game/Pause",
				(EventManager.EventCallFunc)delegate(object[] param)
				{
					if (Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isInGame)
					{
						Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("Battle/OnPause");
						if (param.Length > 0)
						{
							bool result4 = true;
							if (bool.TryParse(param[0].ToString(), out result4))
							{
								Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.Pause(result4);
								return;
							}
						}
						Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.Pause();
					}
				}
			},
			{
				"Game/ShowPnlPause",
				(EventManager.EventCallFunc)delegate
				{
					if (Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isInGame)
					{
						Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("UI/OnShowPnlPause");
					}
				}
			},
			{
				"Game/Resume",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("Battle/OnResume");
					Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.Resume();
				}
			},
			{
				"Game/Restart",
				(EventManager.EventCallFunc)delegate
				{
					if (GameSceneMainController.isEditorMode)
					{
						Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.EditorReEnter();
					}
					else
					{
						if (Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isTutorial)
						{
							Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.TutorialSetting();
						}
						Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.ReEnter();
					}
					Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.RewiredJoystickControllerMapSetEnable(false, "UI");
					Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.SwitchHandleProposal((!Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isTutorial) ? Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.handleProposal : "Default");
				}
			},
			{
				"Game/Quit",
				(EventManager.EventCallFunc)delegate
				{
					Application.Quit();
				}
			},
			{
				"Game/EditorReEnter",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.ReEnter();
				}
			},
			{
				"Game/TutorialStatic",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<StatisticsManager>.instance.OnTutorialEnd();
				}
			},
			{
				"Game/Revive",
				(EventManager.EventCallFunc)delegate
				{
					BattleRoleAttributeComponent.instance.Revive();
				}
			},
			{
				"Game/Finish",
				(EventManager.EventCallFunc)delegate
				{
					SA.Common.Pattern.Singleton<DiscordManager>.Instance.SetUpdateActivity(false, Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicName"].GetResult<string>());
					if (GameSceneMainController.isEditorMode)
					{
						Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.EditorExit();
					}
					else
					{
						Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.Exit();
					}
					Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.RewiredJoystickControllerMapSetEnable(true, "UI");
					Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.RewiredJoystickControllerMapSetEnable(false, Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.handleProposal);
				}
			},
			{
				"UI/DeleteTask",
				(EventManager.EventCallFunc)delegate
				{
					int result3 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedTaskIndex"].GetResult<int>();
					Assets.Scripts.PeroTools.Commons.Singleton<TaskManager>.instance.tasks.RemoveAt(result3);
					if (Assets.Scripts.PeroTools.Commons.Singleton<TaskManager>.instance.tasks.Count == 2)
					{
						Assets.Scripts.PeroTools.Commons.Singleton<TaskManager>.instance.RefreshTaskTime();
					}
					Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance.Save();
				}
			},
			{
				"UI/RateOurGame",
				(EventManager.EventCallFunc)delegate
				{
				}
			},
			{
				"UI/Reconnect",
				(EventManager.EventCallFunc)delegate
				{
					Debug.Log("Reconnect");
				}
			},
			{
				"UI/BacktoWelcome",
				(EventManager.EventCallFunc)delegate
				{
					Debug.Log("Back to Welcome");
				}
			},
			{
				"UI/ShowPanel",
				(EventManager.EventCallFunc)delegate(object[] param)
				{
					string n2 = param[0].ToString();
					Assets.Scripts.PeroTools.Commons.Singleton<UIManager>.instance[n2].SetActive(true);
				}
			},
			{
				"UI/HidePanel",
				(EventManager.EventCallFunc)delegate(object[] param)
				{
					string n = param[0].ToString();
					Assets.Scripts.PeroTools.Commons.Singleton<UIManager>.instance[n].SetActive(false);
				}
			},
			{
				"UI/HideCursor",
				(EventManager.EventCallFunc)delegate(object[] param)
				{
					Cursor.visible = (bool)param[0];
				}
			},
			{
				"UI/OnPnlNoPunchesAskUrl",
				(EventManager.EventCallFunc)delegate
				{
					Application.OpenURL("steam://advertise/1055810");
				}
			},
			{
				"UI/ShowText",
				(EventManager.EventCallFunc)delegate(object[] param)
				{
					Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("UI/OnShowText", param[0]);
				}
			},
			{
				"UI/ConnectStart",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("UI/OnConnectStart");
				}
			},
			{
				"UI/ConnectEnd",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("UI/OnConnectEnd");
				}
			},
			{
				"UI/ConnectFail",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("UI/OnConnectFail");
				}
			},
			{
				"UI/SetBMSIndex",
				(EventManager.EventCallFunc)delegate(object[] param)
				{
					UITest.instance.index = (int)param[0];
					UITest.BmgGroup bmgGroup = UITest.instance.bgs[UITest.instance.index];
					if (bmgGroup.easyBms != null)
					{
						UITest.instance.difficulty = 1;
					}
					else if (bmgGroup.normalBms != null)
					{
						UITest.instance.difficulty = 2;
					}
					else
					{
						UITest.instance.difficulty = 3;
					}
				}
			},
			{
				"UI/SetBMSDifficulty",
				(EventManager.EventCallFunc)delegate(object[] param)
				{
					UITest.instance.difficulty = (int)param[0];
				}
			},
			{
				"UI/BMSGameStart",
				(EventManager.EventCallFunc)delegate
				{
					UITest.instance.GameStart();
				}
			},
			{
				"UI/PopupBulletin",
				(EventManager.EventCallFunc)delegate
				{
					SingletonMonoBehaviour<PnlBulletin>.instance.Popup();
				}
			},
			{
				"UI/StopSfx",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<AudioManager>.instance.StopSound();
				}
			},
			{
				"GM/SetLeftRightMode",
				(EventManager.EventCallFunc)delegate(object[] param)
				{
					bool flag = GameTouchPlay.isLeftRight = bool.Parse(param[0].ToString());
				}
			},
			{
				"UI/CopyMailToClipboard",
				(EventManager.EventCallFunc)delegate
				{
					PeroClipboard.Copy("Player@peropero.work");
					ShowText.ShowInfo(Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "copySucceed"));
				}
			},
			{
				"UI/CopyUserUID",
				(EventManager.EventCallFunc)delegate
				{
					string result2 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["userUid"].GetResult<string>();
					if (!string.IsNullOrEmpty(result2))
					{
						PeroClipboard.Copy(result2);
						ShowText.ShowInfo(Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "copySucceed"));
					}
					else
					{
						ShowText.ShowInfo("COPY ERROR!");
					}
				}
			},
			{
				"UI/SetNekoSkillAvailableEffect",
				(EventManager.EventCallFunc)delegate
				{
					if (Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>() == 16 && Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["IsNekoSkillAvailable"].GetResult<bool>())
					{
						Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("UI/OnNekoSkillAvailable");
						Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["IsNekoSkillAvailable"].SetResult(false);
					}
				}
			},
			{
				"GM/LevelUp",
				(EventManager.EventCallFunc)delegate
				{
					int result = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["CurExp"].GetResult<int>();
					Data data = new Data();
					data["type"].SetResult("exp");
					if (!SingletonMonoBehaviour<MessageManager>.instance.messages.Exists((IData m) => m["type"].GetResult<string>() == "exp"))
					{
						data["count"].SetResult(100 - result);
					}
					else
					{
						data["count"].SetResult(100);
					}
					SingletonMonoBehaviour<MessageManager>.instance.messages.Add(data);
				}
			},
			{
				"GM/UnlockCheck",
				(EventManager.EventCallFunc)delegate(object[] param)
				{
					Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("Net/OnConnecting");
					Assets.Scripts.PeroTools.Commons.Singleton<ServerManager>.instance.DeleteString(param[0].ToString(), delegate(bool isSuccess)
					{
						Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("Net/OnConnectSucceed");
						if (isSuccess)
						{
							StageBattleComponent.UnlockAll();
							Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("UI/OnShowText");
							ShowText.ShowInfo(Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "unlockAll"));
						}
						else
						{
							Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("UI/OnShowText");
							ShowText.ShowInfo(Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "invaildCode"));
						}
					});
				}
			},
			{
				"UI/SetShowLoginTip",
				(EventManager.EventCallFunc)delegate
				{
					Assets.Scripts.PeroTools.Commons.Singleton<XDSDKManager>.instance.showTip = true;
				}
			},
			{
				"UI/SetOversea",
				(EventManager.EventCallFunc)delegate(object[] param)
				{
					Assets.Scripts.PeroTools.Commons.Singleton<XDSDKManager>.instance.SetOversea(bool.Parse(param[0].ToString()));
				}
			}
		};

		public static T GetEntityValue<T>(string key)
		{
			return (T)((Func<object>)entities[key])();
		}
	}
}
