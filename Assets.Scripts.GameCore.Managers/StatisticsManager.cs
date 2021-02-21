using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using FormulaBase;
using GameLogic;
using Newtonsoft.Json.Linq;
using SA.Common.Pattern;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameCore.Managers
{
	public class StatisticsManager : Assets.Scripts.PeroTools.Commons.Singleton<StatisticsManager>
	{
		public class Result
		{
			public int id;

			public string noteType;

			public int offset;

			public int score;

			public string side;
		}

		private List<Result> m_Results;

		private int m_ResultIndex;

		public List<int> m_TutorialArray;

		public int curScore;

		private LockRotate m_ScreenLockTest = new LockRotate();

		public bool isTutorial
		{
			get;
			private set;
		}

		public void OnBattleStart()
		{
			m_Results = new List<Result>();
			m_ResultIndex = 0;
			for (int i = 0; i < Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.GetMusicData().Count; i++)
			{
				Result item = new Result();
				m_Results.Add(item);
			}
			isTutorial = ((bool)MusicConfigReader.Instance.stageInfo && MusicConfigReader.Instance.stageInfo.mapName == "tutorial_v2_map1");
			if (isTutorial)
			{
				m_TutorialArray = new List<int>();
			}
			Assets.Scripts.PeroTools.Commons.Singleton<LockRotate>.instance.BattleStartLockRotate();
			SingletonDataObject singletonDataObject = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"];
			string configStringValue = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue(singletonDataObject["SelectedAlbumName"].GetResult<string>(), "uid", "author", singletonDataObject["SelectedMusicUid"].GetResult<string>());
			SA.Common.Pattern.Singleton<DiscordManager>.Instance.SetUpdateActivity(true, Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicName"].GetResult<string>() + " - " + configStringValue);
		}

		public void OnBattleEnd(bool fail = false)
		{
			Assets.Scripts.PeroTools.Commons.Singleton<LockRotate>.instance.BattleStartUnlockRotate();
			if (GameSceneMainController.isEditorMode)
			{
				return;
			}
			SingletonDataObject singletonDataObject = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"];
			string configStringValue = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue(singletonDataObject["SelectedAlbumName"].GetResult<string>(), "uid", "author", singletonDataObject["SelectedMusicUid"].GetResult<string>());
			SA.Common.Pattern.Singleton<DiscordManager>.Instance.SetUpdateActivity(false, Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicName"].GetResult<string>() + " - " + configStringValue);
			int result2 = singletonDataObject["Level"].GetResult<int>();
			int result3 = singletonDataObject["SelectedRoleIndex"].GetResult<int>();
			int result4 = singletonDataObject["SelectedElfinIndex"].GetResult<int>();
			string result5 = singletonDataObject["UserID"].GetResult<string>();
			result5 = ((!(result5 == "0")) ? result5 : $"_{Assets.Scripts.PeroTools.Commons.Singleton<ServerManager>.instance.GetDeviceID()} ");
			string result6 = singletonDataObject["SelectedMusicUid"].GetResult<string>();
			string result7 = singletonDataObject["SelectedMusicName"].GetResult<string>();
			string result8 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicLevel"].GetResult<string>();
			int result9 = 0;
			int.TryParse(result8, out result9);
			int hideBMSDifficulty = Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.GetHideBMSDifficulty();
			string text = result3.ToString();
			string value = string.Format("{0}-{1}", Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue("character_ChineseS", result3, "characterName"), Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue("character_ChineseS", result3, "cosName"));
			string text2 = (result4 != -1) ? result4.ToString() : null;
			string configStringValue2 = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue("elfin_ChineseS", result4, "name");
			bool isSucceed = Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isSucceed;
			float num = Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetAccuracy() * 100f;
			int comboMax = Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetComboMax();
			int comboMiss = Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetComboMiss();
			string key = Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetStageEvaluate().Key;
			int score = Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetScore();
			int hp = BattleRoleAttributeComponent.instance.GetHp();
			bool flag = Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.IsFullCombo();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("player", result5);
			dictionary.Add("player_level", result9);
			dictionary.Add("music_uid", result6);
			dictionary.Add("music_name", result7);
			dictionary.Add("music_level", result9);
			dictionary.Add("character_uid", text);
			dictionary.Add("character_name", value);
			dictionary.Add("elfin_uid", text2);
			dictionary.Add("elfin_name", configStringValue2);
			dictionary.Add("result_finished", isSucceed);
			Dictionary<string, object> dictionary2 = dictionary;
			if (isSucceed)
			{
				dictionary2.Add("result_acc", num);
				dictionary2.Add("result_score", score);
				dictionary2.Add("result_combo", comboMax);
				dictionary2.Add("result_full_combo", flag);
			}
			dictionary2.Add("controller_name", GetControllerName());
			Debug.Log("Controller Name " + GetControllerName());
			Assets.Scripts.PeroTools.Commons.Singleton<ServerManager>.instance.SendToUrl("statistics/pc-play-statistics-feedback", "POST", dictionary2);
			if (fail)
			{
				return;
			}
			List<Result> list = new List<Result>();
			for (int i = 0; i < m_Results.Count; i++)
			{
				Result result = m_Results[i];
				Result result10 = list.Find((Result r) => r.id == result.id);
				if (result10 != null)
				{
					result10.score += result.score;
				}
				else
				{
					list.Add(result);
				}
			}
			JArray jArray = new JArray();
			int num2 = 0;
			for (int j = 0; j < list.Count; j++)
			{
				Result result11 = list[j];
				JObject jObject = new JObject();
				jObject["note_type"] = result11.noteType;
				jObject["offset"] = result11.offset;
				jObject["score"] = result11.score;
				num2 += result11.score;
				jObject["side"] = result11.side;
				jArray.Add(jObject);
			}
			SingletonMonoBehaviour<MessageManager>.instance.messages.RemoveAll((IData m) => m["type"].GetResult<string>() == "rank");
			Assets.Scripts.PeroTools.Commons.Singleton<ServerManager>.instance.UploadScore(result6, hideBMSDifficulty, text, text2, hp, score, num, comboMax, key, comboMiss, jArray, MusicConfigReader.Instance.stageInfo.md5, delegate(int rank)
			{
				Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.rank = rank + 1;
				if (rank >= 0 && Assets.Scripts.PeroTools.Commons.Singleton<SceneManager>.instance.curScene.name.Contains("GameMain"))
				{
					SingletonMonoBehaviour<MessageManager>.instance.Send("rank");
				}
			});
		}

		public void OnChallengeAchievementEnd()
		{
			try
			{
				List<IData> messages = SingletonMonoBehaviour<MessageManager>.instance.messages;
				List<Dictionary<string, object>> achievements = new List<Dictionary<string, object>>();
				string musicUid = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>();
				string musicName = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicName"].GetResult<string>();
				string result = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicLevel"].GetResult<string>();
				int musicLevel = 0;
				int.TryParse(result, out musicLevel);
				string userId = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["UserID"].GetResult<string>();
				int result2 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
				userId = ((!(userId == "0")) ? userId : $"_{Assets.Scripts.PeroTools.Commons.Singleton<ServerManager>.instance.GetDeviceID()}");
				messages.For(delegate(IData d)
				{
					string result3 = d["type"].GetResult<string>();
					string result4 = d["uid"].GetResult<string>();
					switch (result3)
					{
					case "achievement":
					{
						int configIndex3 = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigIndex("achievement", "uid", result4);
						Dictionary<string, object> item2 = new Dictionary<string, object>
						{
							{
								"achievement_uid",
								result4
							},
							{
								"achievement_name",
								string.Format("{0}-{1}", Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue("achievement_ChineseS", configIndex3, "title"), result4.LastAfter('-'))
							}
						};
						achievements.Add(item2);
						break;
					}
					case "stage_achievement":
					{
						string[] array = result4.Split('-');
						int configIndex2 = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigIndex("stage_achievement", "uid", array[0] + "-" + array[1]);
						JToken jToken = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance["stage_achievement_ChineseS"][configIndex2][array[2]];
						int num = int.Parse(array[3]);
						if (num > jToken.Count() - 1)
						{
							num = 0;
						}
						JToken value2 = jToken[num];
						Dictionary<string, object> item = new Dictionary<string, object>
						{
							{
								"achievement_uid",
								result4
							},
							{
								"achievement_name",
								value2
							},
							{
								"music_uid",
								musicUid
							},
							{
								"music_name",
								musicName
							},
							{
								"music_level",
								musicLevel
							}
						};
						achievements.Add(item);
						break;
					}
					case "task":
					{
						int configIndex = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigIndex("task", "uid", result4);
						Dictionary<string, object> datas2 = new Dictionary<string, object>
						{
							{
								"player",
								userId
							},
							{
								"challenge_uid",
								result4
							},
							{
								"challenge_name",
								Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue("task_ChineseS", configIndex, "description")
							}
						};
						Assets.Scripts.PeroTools.Commons.Singleton<ServerManager>.instance.SendToUrl("statistics/challenge-statistics-feedback", "POST", datas2);
						break;
					}
					}
				});
				if (achievements.Count > 0)
				{
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary.Add("player", userId);
					dictionary.Add("player_level", result2);
					dictionary.Add("achievements", achievements);
					Dictionary<string, object> datas = dictionary;
					Assets.Scripts.PeroTools.Commons.Singleton<ServerManager>.instance.SendToUrl("statistics/achievement-statistics-feedback", "POST", datas);
				}
			}
			catch (Exception value)
			{
				Console.WriteLine(value);
			}
		}

		public void OnTutorialEnd()
		{
			Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.RewiredJoystickControllerMapSetEnable(true, "UI");
			Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.RewiredJoystickControllerMapSetEnable(false, "Default");
			Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.SwitchButtonProposal(Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.keyBoardProposal);
			if (isTutorial && m_TutorialArray.Count == 36)
			{
				string text = "pc";
				text = "pc";
				string result = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>();
				Assets.Scripts.PeroTools.Commons.Singleton<ServerManager>.instance.SendToUrl("statistics/beginner_tutorial_play_statistics_feedback_v3", "POST", new Dictionary<string, object>
				{
					{
						"note_results",
						m_TutorialArray
					},
					{
						"acc",
						Assets.Scripts.PeroTools.Commons.Singleton<TaskStageTarget>.instance.GetTrueAccuracy()
					},
					{
						"platform",
						text
					},
					{
						"Authorization",
						(!string.IsNullOrEmpty(result)) ? result : null
					}
				});
			}
		}

		public void OnGetScore(int id, string noteType, int offset, int score, string side)
		{
			Result result;
			if (m_Results.Count > m_ResultIndex)
			{
				result = m_Results[m_ResultIndex++];
			}
			else
			{
				result = new Result();
				m_Results.Add(result);
				m_ResultIndex++;
			}
			result.id = id;
			result.noteType = noteType;
			result.offset = offset;
			result.score = score;
			result.side = side;
		}

		public void OnNoteResult(int result)
		{
			if (isTutorial && !Assets.Scripts.PeroTools.Commons.Singleton<BattleProperty>.instance.isAutoPlay)
			{
				m_TutorialArray.Add(result);
			}
		}

		public string GetControllerName()
		{
			string currentControllerHardwareName = Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.currentControllerHardwareName;
			string text = (!(currentControllerHardwareName == "Keyboard")) ? Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.handleProposal : Assets.Scripts.PeroTools.Commons.Singleton<InputManager>.instance.keyBoardProposal;
			text = ((!(text == "Default")) ? text.Replace("Mode", string.Empty) : "A");
			string str = (!(currentControllerHardwareName == "Keyboard")) ? ((!Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["IsReverse"].GetResult<bool>()) ? "0" : "1") : "0";
			return currentControllerHardwareName + text + str;
		}
	}
}
