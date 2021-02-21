using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using FormulaBase;
using GameLogic;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameCore.Managers
{
	public class AchievementManager : Singleton<AchievementManager>
	{
		public List<string> achievements
		{
			get;
			private set;
		}

		private void Init()
		{
			achievements = Singleton<DataManager>.instance["Achievement"]["achievements"].GetResult<List<string>>();
		}

		public bool IsDone(string uid)
		{
			return achievements.Contains(uid);
		}

		public void Do(bool isSuccuess)
		{
			bool isFail = Singleton<StageBattleComponent>.instance.failTime >= 60f && !isSuccuess;
			int hideBMSDifficulty = Singleton<StageBattleComponent>.instance.GetHideBMSDifficulty();
			string uid = string.Format("{0}_{1}", Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>(), hideBMSDifficulty);
			string evaluate = Singleton<StageBattleComponent>.instance.evaluate;
			int evaluateNum = Singleton<StageBattleComponent>.instance.evaluateNum;
			int score = Singleton<TaskStageTarget>.instance.GetScore();
			int comboMax = Singleton<TaskStageTarget>.instance.GetComboMax();
			float accuracy = Singleton<TaskStageTarget>.instance.GetAccuracy();
			string diffcultStr = Singleton<StageBattleComponent>.instance.GetDiffcultStr();
			string result = Singleton<DataManager>.instance["Account"]["SelectedMusicLevel"].GetResult<string>();
			int level = 0;
			int.TryParse(result, out level);
			int blood = Singleton<TaskStageTarget>.instance.GetBlood();
			int hitCount = Singleton<TaskStageTarget>.instance.GetHitCount();
			int perfectCount = Singleton<TaskStageTarget>.instance.GetHitCountByResult(4u);
			int normalNoteTotalCount = Singleton<StageBattleComponent>.instance.GetMusicData().Count((MusicData m) => m.noteData.addCombo && !m.isLongPressing);
			bool isFullCombo = Singleton<TaskStageTarget>.instance.IsFullCombo();
			string item = Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>().ToString();
			List<string> list = new List<string>();
			if (isFullCombo && isSuccuess)
			{
				IVariable data = Singleton<DataManager>.instance["Achievement"]["full_combo_music"];
				list = data.GetResult<List<string>>();
				if (!list.Contains(uid))
				{
					list.Add(uid);
				}
			}
			int comboMiss = Singleton<TaskStageTarget>.instance.GetComboMiss();
			IVariable data2 = Singleton<DataManager>.instance["Achievement"]["long_press_count"];
			if (isSuccuess)
			{
				data2.SetResult(data2.GetResult<int>() + Singleton<TaskStageTarget>.instance.GetLongPressFinishCount());
			}
			int result2 = data2.GetResult<int>();
			IVariable data3 = Singleton<DataManager>.instance["Achievement"]["block_count"];
			if (isSuccuess)
			{
				data3.SetResult(data3.GetResult<int>() + Singleton<TaskStageTarget>.instance.GetBlock());
			}
			int result3 = data3.GetResult<int>();
			IVariable data4 = Singleton<DataManager>.instance["Achievement"]["boss_near_to_count"];
			if (isSuccuess)
			{
				data4.SetResult(data3.GetResult<int>() + Singleton<TaskStageTarget>.instance.GetBossNearHit());
			}
			int result4 = data4.GetResult<int>();
			IVariable data5 = Singleton<DataManager>.instance["Achievement"]["note_hit_count"];
			if (isSuccuess)
			{
				data5.SetResult(data5.GetResult<int>() + Singleton<TaskStageTarget>.instance.GetHitEnemy());
			}
			int result5 = data5.GetResult<int>();
			IVariable data6 = Singleton<DataManager>.instance["Achievement"]["hide_note_count"];
			if (isSuccuess)
			{
				data6.SetResult(data6.GetResult<int>() + Singleton<TaskStageTarget>.instance.GetHideNoteHitCount());
			}
			int result6 = data6.GetResult<int>();
			IVariable data7 = Singleton<DataManager>.instance["Achievement"]["blood_count"];
			if (isSuccuess)
			{
				data7.SetResult(data7.GetResult<int>() + blood);
			}
			int result7 = data7.GetResult<int>();
			IVariable data8 = Singleton<DataManager>.instance["Achievement"]["music_note_count"];
			if (isSuccuess)
			{
				data8.SetResult(data8.GetResult<int>() + Singleton<TaskStageTarget>.instance.GetNoteItemCount());
			}
			IVariable data9 = Singleton<DataManager>.instance["Achievement"]["long_press_hit"];
			if (isSuccuess)
			{
				data9.SetResult(data9.GetResult<int>() + Singleton<TaskStageTarget>.instance.GetLongPressHit());
			}
			int result8 = data9.GetResult<int>();
			int result9 = data8.GetResult<int>();
			List<IData> result10 = Singleton<DataManager>.instance["Achievement"]["fail_count"].GetResult<List<IData>>();
			IData data10 = result10.Find((IData v) => v["uid"].GetResult<string>() == uid);
			if (isFail)
			{
				if (data10 != null)
				{
					IVariable data11 = data10["count"];
					data11.SetResult(data11.GetResult<int>() + 1);
				}
				else
				{
					data10 = new Data();
					data10["uid"].SetResult(uid);
					data10["count"].SetResult(1);
					result10.Add(data10);
				}
			}
			int value = data10?["count"].GetResult<int>() ?? 0;
			int value2 = result10.Sum((IData f) => f["count"].GetResult<int>());
			IVariable data12 = Singleton<DataManager>.instance["Achievement"]["pass_count"];
			if (isSuccuess)
			{
				data12.SetResult(data12.GetResult<int>() + 1);
			}
			IVariable data13 = Singleton<DataManager>.instance["Achievement"]["easy_pass"];
			int value3 = 0;
			if (diffcultStr == "easy" && isSuccuess)
			{
				List<string> result11 = data13.GetResult<List<string>>();
				if (!result11.Contains(uid))
				{
					result11.Add(uid);
				}
				value3 = result11.Count;
			}
			IVariable data14 = Singleton<DataManager>.instance["Achievement"]["hard_pass"];
			int value4 = 0;
			if (diffcultStr == "hard" && isSuccuess)
			{
				List<string> result12 = data14.GetResult<List<string>>();
				if (!result12.Contains(uid))
				{
					result12.Add(uid);
				}
				value4 = result12.Count;
			}
			IVariable data15 = Singleton<DataManager>.instance["Achievement"]["master_pass"];
			int value5 = 0;
			if (diffcultStr == "master" && isSuccuess)
			{
				List<string> result13 = data15.GetResult<List<string>>();
				if (!result13.Contains(uid))
				{
					result13.Add(uid);
				}
				value5 = result13.Count;
			}
			IVariable data16 = Singleton<DataManager>.instance["Achievement"]["d_pass"];
			if (evaluate == "d" && isSuccuess)
			{
				data16.SetResult(data16.GetResult<int>() + 1);
			}
			int result14 = data16.GetResult<int>();
			IVariable data17 = Singleton<DataManager>.instance["Achievement"]["c_pass"];
			if (evaluate == "c" && isSuccuess)
			{
				data17.SetResult(data17.GetResult<int>() + 1);
			}
			int result15 = data17.GetResult<int>();
			IVariable data18 = Singleton<DataManager>.instance["Achievement"]["b_pass"];
			if (evaluate == "b" && isSuccuess)
			{
				data18.SetResult(data18.GetResult<int>() + 1);
			}
			int result16 = data18.GetResult<int>();
			IVariable data19 = Singleton<DataManager>.instance["Achievement"]["a_pass"];
			if (evaluate == "a" && isSuccuess)
			{
				data19.SetResult(data19.GetResult<int>() + 1);
			}
			int result17 = data19.GetResult<int>();
			if (isSuccuess)
			{
				List<IData> result18 = Singleton<DataManager>.instance["Achievement"]["highest"].GetResult<List<IData>>();
				IData data20 = result18.Find((IData d) => d["uid"].GetResult<string>() == uid);
				if (data20 == null)
				{
					data20 = new Data();
					result18.Add(data20);
					data20["uid"].SetResult(uid);
				}
				int result19 = data20["evaluate"].GetResult<int>();
				if (result19 < evaluateNum)
				{
					data20["evaluate"].SetResult(evaluateNum);
					if (evaluateNum >= 4 && result19 < 4 && hideBMSDifficulty == 2)
					{
						string result20 = Singleton<DataManager>.instance["Account"]["SelectedAlbumName"].GetResult<string>();
						string result21 = Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>();
						int configIntValue = Singleton<ConfigManager>.instance.GetConfigIntValue(result20, "uid", "difficulty3", result21);
						if (configIntValue > 0)
						{
							Singleton<StageBattleComponent>.instance.unlockNextLevel = true;
							List<string> result22 = Singleton<DataManager>.instance["Account"]["UnlockMasters"].GetResult<List<string>>();
							string result23 = Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>();
							if (!result22.Contains(result23))
							{
								result22.Add(result23);
							}
						}
					}
				}
				int result24 = data20["score"].GetResult<int>();
				if (result24 < score)
				{
					data20["score"].SetResult(score);
				}
				int result25 = data20["combo"].GetResult<int>();
				if (result25 < comboMax)
				{
					data20["combo"].SetResult(comboMax);
				}
				float result26 = data20["accuracy"].GetResult<float>();
				if (result26 < accuracy)
				{
					data20["accuracy"].SetResult(accuracy);
					data20["accuracyStr"].SetResult(accuracy.ToString("p"));
				}
				data20["clear"].SetResult(data20["clear"].GetResult<float>() + 1f);
			}
			int value6 = 0;
			if (isFail)
			{
				List<string> result27 = Singleton<DataManager>.instance["Achievement"]["fail_characters"].GetResult<List<string>>();
				if (!result27.Contains(item))
				{
					result27.Add(item);
				}
				value6 = result27.Count;
			}
			int value7 = Singleton<DataManager>.instance["StageAchievement"]["stage_achievements"].GetResult<List<string>>().Count((string s) => s.StartsWith("0"));
			DoAchievement("1", new int[4]
			{
				0,
				4,
				7,
				10
			}, level, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("2", new int[3]
			{
				10,
				20,
				30
			}, value3, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("3", new int[3]
			{
				10,
				20,
				30
			}, value4, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("4", new int[3]
			{
				10,
				20,
				30
			}, value5, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("9", new int[1]
			{
				10
			}, result14, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("10", new int[1]
			{
				10
			}, result15, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("11", new int[1]
			{
				10
			}, result16, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("12", new int[1]
			{
				10
			}, result17, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("13", new int[4]
			{
				0,
				4,
				7,
				10
			}, level, (int v1, int v2) => v1 >= v2, () => isSuccuess && (evaluate == "s" || evaluate == "ss" || evaluate == "sss"));
			DoAchievement("15", new int[4]
			{
				0,
				4,
				7,
				10
			}, level, (int v1, int v2) => v1 >= v2, () => isSuccuess && isFullCombo);
			DoAchievement("16", new int[4]
			{
				10,
				20,
				50,
				80
			}, list.Count, (int v1, int v2) => v1 >= v2, () => isSuccuess && isFullCombo);
			DoAchievement("20", new int[4]
			{
				100,
				200,
				300,
				500
			}, Singleton<TaskStageTarget>.instance.GetComboMax(), (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("21", new int[4]
			{
				2000,
				5000,
				10000,
				20000
			}, result5, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("22", new int[4]
			{
				20,
				50,
				100,
				200
			}, result4, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("23", new int[4]
			{
				50,
				100,
				200,
				500
			}, result2, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("24", new int[4]
			{
				200,
				500,
				1000,
				2000
			}, result3, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("25", new int[4]
			{
				50,
				100,
				250,
				500
			}, result6, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("26", new int[4]
			{
				50,
				100,
				250,
				500
			}, result7, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("27", new int[4]
			{
				50,
				100,
				250,
				500
			}, result9, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("28", new int[4]
			{
				50,
				100,
				250,
				500
			}, result8, (int v1, int v2) => v1 >= v2, () => isSuccuess);
			DoAchievement("5", new int[1], blood, (int v1, int v2) => v1 == v2, () => isSuccuess && level >= 7);
			DoAchievement("6", new int[1], hitCount, (int v1, int v2) => v1 == v2, () => isSuccuess);
			DoAchievement("7", new int[2]
			{
				5,
				10
			}, value, (int v1, int v2) => v1 >= v2, () => isFail);
			DoAchievement("8", new int[1]
			{
				20
			}, value2, (int v1, int v2) => v1 >= v2, () => isFail);
			DoAchievement("14", new int[2]
			{
				7,
				10
			}, level, (int v1, int v2) => v1 >= v2, () => isSuccuess && evaluate == "sss");
			DoAchievement("17", new int[1]
			{
				1
			}, comboMiss, (int v1, int v2) => v1 == v2, () => isSuccuess && level >= 7);
			DoAchievement("18", new byte[1]
			{
				1
			}, Singleton<BattleEnemyManager>.instance.GetPlayResult(Singleton<StageBattleComponent>.instance.lastTnoIndex), (byte v1, byte v2) => v1 == v2, () => isFail && level >= 7);
			DoAchievement("19", new byte[1]
			{
				3
			}, Singleton<BattleEnemyManager>.instance.GetPlayResult(Singleton<StageBattleComponent>.instance.lastTnoIndex), (byte v1, byte v2) => v1 == v2, () => isSuccuess && level >= 7 && isFullCombo && perfectCount == normalNoteTotalCount - 1);
			DoAchievement("34", new int[1]
			{
				Singleton<ItemManager>.instance.totalCharacterCount - 1
			}, value6, (int v1, int v2) => v1 >= v2);
			DoAchievement("32", new int[1]
			{
				159
			}, value7, (int v1, int v2) => v1 == v2);
			DoAchievement("33", new int[1]
			{
				85
			}, achievements.Count, (int v1, int v2) => v1 == v2);
		}

		public void Do()
		{
			List<IData> result = Singleton<DataManager>.instance["Account"]["Items"].GetResult<List<IData>>();
			List<IData> array = result.Where((IData i) => i["isUnlock"].GetResult<bool>());
			int value = array.Count((IData i) => i["type"].GetResult<string>() == "character");
			int value2 = array.Count((IData i) => i["type"].GetResult<string>() == "elfin");
			int value3 = array.Count((IData i) => i["type"].GetResult<string>() == "loading");
			DoAchievement("29", new int[3]
			{
				2,
				7,
				12
			}, value, (int v1, int v2) => v1 >= v2);
			DoAchievement("30", new int[3]
			{
				1,
				4,
				8
			}, value2, (int v1, int v2) => v1 >= v2);
			DoAchievement("31", new int[3]
			{
				2,
				6,
				10
			}, value3, (int v1, int v2) => v1 >= v2);
			DoAchievement("33", new int[1]
			{
				85
			}, achievements.Count, (int v1, int v2) => v1 == v2);
		}

		private void DoAchievement<T>(string uid, T[] targets, T value, Func<T, T, bool> compareFunc, Func<bool> isDo = null)
		{
			if (isDo != null && !isDo())
			{
				return;
			}
			int num = 0;
			string text;
			while (true)
			{
				if (num < targets.Length)
				{
					text = $"{uid}-{num + 1}";
					if (!achievements.Contains(text))
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			if (compareFunc(value, targets[num]))
			{
				Reward(text);
			}
		}

		private void Reward(string uid)
		{
			achievements.Add(uid);
			SingletonMonoBehaviour<MessageManager>.instance.Send("achievement", uid);
		}
	}
}
