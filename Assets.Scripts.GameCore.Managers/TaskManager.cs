using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using FormulaBase;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameCore.Managers
{
	public class TaskManager : Singleton<TaskManager>
	{
		private SingletonDataObject m_Task;

		private DateTime m_RefreshTime;

		private readonly TimeSpan m_RefreshGap = new TimeSpan(6, 0, 0);

		private int m_MusicLevel;

		public List<string> tasks
		{
			get;
			private set;
		}

		public TimeSpan refreshGap => m_RefreshTime - SingletonMonoBehaviour<MainManager>.instance.dateTime;

		private int m_State
		{
			get
			{
				int result = 1;
				if (Singleton<AchievementManager>.instance.IsDone("15-2") && Singleton<AchievementManager>.instance.IsDone("1-3"))
				{
					result = 2;
				}
				if (Singleton<AchievementManager>.instance.IsDone("15-3") && Singleton<AchievementManager>.instance.IsDone("1-4"))
				{
					result = 3;
				}
				return result;
			}
		}

		private void Init()
		{
			m_Task = Singleton<DataManager>.instance["Task"];
			tasks = m_Task["Tasks"].GetResult<List<string>>();
			string result = m_Task["RefreshTime"].GetResult<string>();
			m_RefreshTime = ((!string.IsNullOrEmpty(result)) ? JsonUtils.Deserialize<DateTime>(result) : DateTime.MinValue);
			SingletonMonoBehaviour<MainManager>.instance.onTimeChanged -= OnTimeChanged;
			SingletonMonoBehaviour<MainManager>.instance.onTimeChanged += OnTimeChanged;
		}

		private void OnTimeChanged(DateTime time)
		{
			if (refreshGap.TotalSeconds <= 0.0 && tasks.Count < 3)
			{
				RefreshTasks();
			}
		}

		public void RefreshTasks()
		{
			int state = m_State;
			List<string> list = new List<string>();
			List<float> list2 = new List<float>();
			JArray json = Singleton<ConfigManager>.instance.GetJson("task", false);
			for (int i = 0; i < json.Count; i++)
			{
				JToken jToken = json[i];
				string text = (string)jToken["uid"];
				if (text[0].ToString() == state.ToString() && !tasks.Contains(text))
				{
					list.Add(text);
					list2.Add((float)jToken["piority"]);
				}
			}
			string index = string.Empty;
			List<Action> list3 = new List<Action>();
			for (int j = 0; j < list.Count; j++)
			{
				string uid = list[j];
				list3.Add(delegate
				{
					index = uid;
				});
			}
			int num = Mathf.Min((int)Math.Floor((SingletonMonoBehaviour<MainManager>.instance.dateTime - m_RefreshTime).TotalSeconds / m_RefreshGap.TotalSeconds) + 1, 3 - tasks.Count);
			for (int k = 0; k < num; k++)
			{
				int index2 = RandomUtils.RandomEvent(list2, list3.ToArray());
				tasks.Add(index);
				list2.RemoveAt(index2);
				list3.RemoveAt(index2);
				m_RefreshTime += m_RefreshGap;
			}
			TimeSpan timeSpan = SingletonMonoBehaviour<MainManager>.instance.dateTime - m_RefreshTime;
			if (timeSpan.TotalSeconds > 0.0)
			{
				m_RefreshTime = SingletonMonoBehaviour<MainManager>.instance.dateTime + new TimeSpan(0, 0, (int)m_RefreshGap.TotalSeconds - (int)(timeSpan.TotalSeconds % m_RefreshGap.TotalSeconds));
			}
			m_Task["RefreshTime"].SetResult(JsonUtils.Serialize(m_RefreshTime));
			Singleton<EventManager>.instance.Invoke("UI/OnTaskRefresh");
			Singleton<DataManager>.instance.Save();
		}

		private void Reward(string uid)
		{
			if (tasks.Count == 2)
			{
				RefreshTaskTime();
			}
			SingletonMonoBehaviour<MessageManager>.instance.Send("task", uid);
		}

		public void RefreshTaskTime()
		{
			m_RefreshTime = SingletonMonoBehaviour<MainManager>.instance.dateTime + m_RefreshGap;
			m_Task["RefreshTime"].SetResult(JsonUtils.Serialize(m_RefreshTime));
		}

		public void Do(bool isSucceed, bool isExit = false)
		{
			string musicUid = string.Format("{0}{1}", Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>(), Singleton<DataManager>.instance["Account"]["SelectedDifficulty"].GetResult<int>());
			string result = Singleton<DataManager>.instance["Account"]["SelectedMusicLevel"].GetResult<string>();
			int.TryParse(result, out m_MusicLevel);
			switch (m_State)
			{
			case 1:
				Do1(musicUid, isSucceed, isExit);
				break;
			case 2:
				Do1(musicUid, isSucceed, isExit);
				Do2(musicUid, isSucceed, isExit);
				break;
			case 3:
				Do1(musicUid, isSucceed, isExit);
				Do2(musicUid, isSucceed, isExit);
				Do3(musicUid, isSucceed, isExit);
				break;
			}
		}

		private void Do1(string musicUid, bool isSucceed, bool isExit)
		{
			string text = "1-1-1";
			if (tasks.Contains(text))
			{
				string passKey = text;
				Func<int, bool> targetFunc = (int v) => true;
				string musicUid2 = musicUid;
				bool isSucceed2 = isSucceed;
				int targetValue = 1;
				int passTarget = 3;
				string uid = text;
				Do(passKey, targetFunc, musicUid2, isSucceed2, targetValue, passTarget, -1, uid, isExit);
			}
			string text2 = "1-2-1";
			if (tasks.Contains(text2))
			{
				string evaluate = Singleton<StageBattleComponent>.instance.evaluate;
				string uid = text2;
				Func<int, bool> targetFunc = (int v) => true;
				string musicUid2 = musicUid;
				int num;
				if (isSucceed)
				{
					switch (evaluate)
					{
					default:
						num = ((evaluate == "sss") ? 1 : 0);
						break;
					case "a":
					case "s":
					case "ss":
						num = 1;
						break;
					}
				}
				else
				{
					num = 0;
				}
				bool isSucceed2 = (byte)num != 0;
				int passTarget = 1;
				bool isExit2 = isExit;
				Do(uid, targetFunc, musicUid2, isSucceed2, -1, passTarget, -1, text2, isExit2);
			}
			string text3 = "1-2-2";
			if (tasks.Contains(text3))
			{
				string evaluate2 = Singleton<StageBattleComponent>.instance.evaluate;
				string musicUid2 = text3;
				Func<int, bool> targetFunc = (int v) => true;
				string uid = musicUid;
				int num2;
				if (isSucceed)
				{
					switch (evaluate2)
					{
					default:
						num2 = ((evaluate2 == "sss") ? 1 : 0);
						break;
					case "b":
					case "a":
					case "s":
					case "ss":
						num2 = 1;
						break;
					}
				}
				else
				{
					num2 = 0;
				}
				bool isExit2 = (byte)num2 != 0;
				string passKey = text3;
				bool isSucceed2 = isExit;
				Do(musicUid2, targetFunc, uid, isExit2, -1, 2, -1, passKey, isSucceed2);
			}
			string text4 = "1-3-1";
			if (tasks.Contains(text4))
			{
				string passKey = text4;
				Func<int, bool> targetFunc = (int v) => true;
				string uid = musicUid;
				bool isSucceed2 = isSucceed && Singleton<TaskStageTarget>.instance.GetHitCount() >= 100;
				int passTarget = 1;
				bool isExit2 = isExit;
				Do(passKey, targetFunc, uid, isSucceed2, -1, passTarget, -1, text4, isExit2);
			}
			string text5 = "1-3-2";
			if (tasks.Contains(text5))
			{
				int value5 = Singleton<TaskStageTarget>.instance.GetHitCount();
				string uid = text5;
				Func<int, bool> targetFunc = (int v) => v + value5 >= 180;
				string passKey = musicUid;
				bool isExit2 = isSucceed;
				int passTarget = value5;
				string musicUid2 = text5;
				bool isSucceed2 = isExit;
				Do(uid, targetFunc, passKey, isExit2, passTarget, 2, -1, musicUid2, isSucceed2);
			}
			string text6 = "1-4-1";
			if (tasks.Contains(text6))
			{
				string musicUid2 = text6;
				Func<int, bool> targetFunc = (int v) => true;
				string passKey = musicUid;
				bool isSucceed2 = isSucceed && Singleton<TaskStageTarget>.instance.GetHitCountByResult(4u) >= 60;
				int passTarget = 1;
				bool isExit2 = isExit;
				Do(musicUid2, targetFunc, passKey, isSucceed2, -1, passTarget, -1, text6, isExit2);
			}
			string text7 = "1-4-2";
			if (tasks.Contains(text7))
			{
				int value4 = Singleton<TaskStageTarget>.instance.GetHitCountByResult(4u);
				string passKey = text7;
				Func<int, bool> targetFunc = (int v) => value4 + v >= 110;
				string musicUid2 = musicUid;
				bool isExit2 = isSucceed;
				int passTarget = value4;
				string uid = text7;
				bool isSucceed2 = isExit;
				Do(passKey, targetFunc, musicUid2, isExit2, passTarget, 2, -1, uid, isSucceed2);
			}
			string text8 = "1-5-1";
			if (tasks.Contains(text8))
			{
				string uid = text8;
				Func<int, bool> targetFunc = (int v) => true;
				string musicUid2 = musicUid;
				bool isSucceed2 = isSucceed && FeverManager.Instance.feverCount >= 2;
				int passTarget = 1;
				bool isExit2 = isExit;
				Do(uid, targetFunc, musicUid2, isSucceed2, -1, passTarget, -1, text8, isExit2);
			}
			string text9 = "1-5-2";
			if (tasks.Contains(text9))
			{
				int value3 = FeverManager.Instance.feverCount;
				string musicUid2 = text9;
				Func<int, bool> targetFunc = (int v) => value3 + v >= 3;
				string uid = musicUid;
				bool isExit2 = isSucceed;
				int passTarget = value3;
				string passKey = text9;
				bool isSucceed2 = isExit;
				Do(musicUid2, targetFunc, uid, isExit2, passTarget, 2, -1, passKey, isSucceed2);
			}
			string text10 = "1-6-1";
			if (tasks.Contains(text10))
			{
				string passKey = text10;
				Func<int, bool> targetFunc = (int v) => true;
				string uid = musicUid;
				bool isSucceed2 = isSucceed && Singleton<TaskStageTarget>.instance.GetComboMax() >= 80;
				int passTarget = 1;
				bool isExit2 = isExit;
				Do(passKey, targetFunc, uid, isSucceed2, -1, passTarget, -1, text10, isExit2);
			}
			string text11 = "1-6-2";
			if (tasks.Contains(text11))
			{
				string uid = text11;
				Func<int, bool> targetFunc = (int v) => true;
				string passKey = musicUid;
				bool isExit2 = isSucceed && Singleton<TaskStageTarget>.instance.GetComboMax() >= 60;
				string musicUid2 = text11;
				bool isSucceed2 = isExit;
				Do(uid, targetFunc, passKey, isExit2, -1, 2, -1, musicUid2, isSucceed2);
			}
			string text12 = "1-7-1";
			if (tasks.Contains(text12))
			{
				int value2 = Singleton<TaskStageTarget>.instance.GetBlock();
				string musicUid2 = text12;
				Func<int, bool> targetFunc = (int v) => v + value2 >= 10;
				string passKey = musicUid;
				bool isSucceed2 = isSucceed;
				int passTarget = value2;
				string uid = text12;
				bool isExit2 = isExit;
				Do(musicUid2, targetFunc, passKey, isSucceed2, passTarget, 2, -1, uid, isExit2);
			}
			string text13 = "1-8-1";
			if (tasks.Contains(text13))
			{
				int value = Singleton<TaskStageTarget>.instance.GetComboMiss();
				string uid = text13;
				Func<int, bool> targetFunc = (int v) => v + value <= 20;
				string passKey = musicUid;
				bool isExit2 = isSucceed;
				int passTarget = value;
				string musicUid2 = text13;
				bool isSucceed2 = isExit;
				Do(uid, targetFunc, passKey, isExit2, passTarget, 2, -1, musicUid2, isSucceed2);
			}
		}

		private void Do2(string musicUid, bool isSucceed, bool isExit)
		{
			string text = "2-1-1";
			if (tasks.Contains(text))
			{
				Do(text, (int v) => true, musicUid, isSucceed && Singleton<StageBattleComponent>.instance.leastHpRate >= 0.25f, -1, 4, 4, text, isExit);
			}
			string text2 = "2-2-1";
			if (tasks.Contains(text2))
			{
				string evaluate = Singleton<StageBattleComponent>.instance.evaluate;
				string passKey = text2;
				Func<int, bool> targetFunc = (int v) => true;
				bool isSucceed2 = isSucceed && (evaluate == "s" || evaluate == "ss" || evaluate == "sss");
				string uid = text2;
				Do(passKey, targetFunc, musicUid, isSucceed2, -1, 2, 4, uid, isExit);
			}
			string text3 = "2-2-2";
			if (tasks.Contains(text3))
			{
				string evaluate2 = Singleton<StageBattleComponent>.instance.evaluate;
				Func<int, bool> targetFunc2 = (int v) => true;
				int isSucceed3;
				if (isSucceed)
				{
					switch (evaluate2)
					{
					default:
						isSucceed3 = ((evaluate2 == "sss") ? 1 : 0);
						break;
					case "a":
					case "s":
					case "ss":
						isSucceed3 = 1;
						break;
					}
				}
				else
				{
					isSucceed3 = 0;
				}
				Do(text3, targetFunc2, musicUid, (byte)isSucceed3 != 0, -1, 3, 4, text3, isExit);
			}
			string text4 = "2-3-1";
			if (tasks.Contains(text4))
			{
				int value10 = Singleton<TaskStageTarget>.instance.GetHitCount();
				Do(text4, (int v) => v + value10 >= 280, musicUid, isSucceed, value10, 2, 4, text4, isExit);
			}
			string text5 = "2-3-2";
			if (tasks.Contains(text5))
			{
				int value9 = Singleton<TaskStageTarget>.instance.GetHitCount();
				Do(text5, (int v) => v + value9 >= 400, musicUid, isSucceed, value9, 3, 4, text5, isExit);
			}
			string text6 = "2-4-1";
			if (tasks.Contains(text6))
			{
				int value8 = Singleton<TaskStageTarget>.instance.GetHitCountByResult(4u);
				Do(text6, (int v) => v + value8 >= 200, musicUid, isSucceed, value8, 2, 4, text6, isExit);
			}
			string text7 = "2-4-2";
			if (tasks.Contains(text7))
			{
				int value7 = Singleton<TaskStageTarget>.instance.GetHitCountByResult(4u);
				Do(text7, (int v) => v + value7 >= 400, musicUid, isSucceed, value7, 3, 4, text7, isExit);
			}
			string text8 = "2-5-1";
			if (tasks.Contains(text8))
			{
				int value6 = FeverManager.Instance.feverCount;
				Do(text8, (int v) => v + value6 >= 5, musicUid, isSucceed, value6, 2, 4, text8, isExit);
			}
			string text9 = "2-5-2";
			if (tasks.Contains(text9))
			{
				int value5 = FeverManager.Instance.feverCount;
				Do(text9, (int v) => v + value5 >= 7, musicUid, isSucceed, value5, 3, 4, text9, isExit);
			}
			string text10 = "2-6-1";
			if (tasks.Contains(text10))
			{
				Do(text10, (int v) => true, musicUid, isSucceed && Singleton<TaskStageTarget>.instance.GetComboMax() > 150, -1, 2, 4, text10, isExit);
			}
			string text11 = "2-6-2";
			if (tasks.Contains(text11))
			{
				Do(text11, (int v) => true, musicUid, isSucceed && Singleton<TaskStageTarget>.instance.GetComboMax() >= 120, -1, 3, 4, text11, isExit);
			}
			string text12 = "2-7-1";
			if (tasks.Contains(text12))
			{
				int value4 = Singleton<TaskStageTarget>.instance.GetBlock();
				Do(text12, (int v) => v + value4 >= 15, musicUid, isSucceed, value4, 2, 4, text12, isExit);
			}
			string text13 = "2-7-2";
			if (tasks.Contains(text13))
			{
				int value3 = Singleton<TaskStageTarget>.instance.GetBlock();
				Do(text13, (int v) => v + value3 >= 20, musicUid, isSucceed, value3, 3, 4, text13, isExit);
			}
			string text14 = "2-8-1";
			if (tasks.Contains(text14))
			{
				int value2 = Singleton<TaskStageTarget>.instance.GetComboMiss();
				Do(text14, (int v) => v + value2 <= 15, musicUid, isSucceed, value2, 2, 4, text14, isExit);
			}
			string text15 = "2-8-2";
			if (tasks.Contains(text15))
			{
				int value = Singleton<TaskStageTarget>.instance.GetComboMiss();
				Do(text15, (int v) => v + value <= 25, musicUid, isSucceed, value, 3, 4, text15, isExit);
			}
		}

		private void Do3(string musicUid, bool isSucceed, bool isExit)
		{
			string text = "3-1-1";
			if (tasks.Contains(text))
			{
				Do(text, (int v) => true, musicUid, isSucceed && Singleton<StageBattleComponent>.instance.leastHpRate >= 0.5f, -1, 5, 7, text, isExit);
			}
			string text2 = "3-2-1";
			if (tasks.Contains(text2))
			{
				string evaluate = Singleton<StageBattleComponent>.instance.evaluate;
				Do(text2, (int v) => true, musicUid, isSucceed && (evaluate == "s" || evaluate == "ss" || evaluate == "sss"), -1, 3, 7, text2, isExit);
			}
			string text3 = "3-3-1";
			if (tasks.Contains(text3))
			{
				int value7 = Singleton<TaskStageTarget>.instance.GetHitCount();
				Do(text3, (int v) => v + value7 >= 800, musicUid, isSucceed, value7, 3, 7, text3, isExit);
			}
			string text4 = "3-4-1";
			if (tasks.Contains(text4))
			{
				int value6 = Singleton<TaskStageTarget>.instance.GetHitCountByResult(4u);
				Do(text4, (int v) => v + value6 >= 650, musicUid, isSucceed, value6, 3, 7, text4, isExit);
			}
			string text5 = "3-5-1";
			if (tasks.Contains(text5))
			{
				int value5 = FeverManager.Instance.feverCount;
				Do(text5, (int v) => v + value5 >= 10, musicUid, isSucceed, value5, 3, 7, text5, isExit);
			}
			string text6 = "3-6-1";
			if (tasks.Contains(text6))
			{
				Do(text6, (int v) => true, musicUid, isSucceed && Singleton<TaskStageTarget>.instance.GetComboMax() >= 300, -1, 3, 7, text6, isExit);
			}
			string text7 = "3-7-1";
			if (tasks.Contains(text7))
			{
				int value4 = Singleton<TaskStageTarget>.instance.GetBlock();
				Do(text7, (int v) => v + value4 >= 30, musicUid, isSucceed, value4, 2, 7, text7, isExit);
			}
			string text8 = "3-7-2";
			if (tasks.Contains(text8))
			{
				int value3 = Singleton<TaskStageTarget>.instance.GetBlock();
				Do(text8, (int v) => v + value3 >= 40, musicUid, isSucceed, value3, 3, 7, text8, isExit);
			}
			string text9 = "3-8-1";
			if (tasks.Contains(text9))
			{
				int value2 = Singleton<TaskStageTarget>.instance.GetComboMiss();
				Do(text9, (int v) => v + value2 <= 10, musicUid, isSucceed, value2, 2, 7, text9, isExit);
			}
			string text10 = "3-8-2";
			if (tasks.Contains(text10))
			{
				int value = Singleton<TaskStageTarget>.instance.GetComboMiss();
				Do(text10, (int v) => v + value <= 20, musicUid, isSucceed, value, 3, 7, text10, isExit);
			}
		}

		private void Do(string passKey, Func<int, bool> targetFunc, string musicUid, bool isSucceed, int targetValue = -1, int passTarget = 2, int level = -1, string uid = "", bool isExit = false)
		{
			IVariable data = m_Task[passKey];
			if (m_MusicLevel < level || !isSucceed)
			{
				if (isExit)
				{
					data.SetResult(new List<IData>());
				}
				return;
			}
			List<IData> result = data.GetResult<List<IData>>();
			int num = result.FindIndex((IData l) => l.Exists("uid") && l["uid"].GetResult<string>() == musicUid);
			if (num != -1)
			{
				if (!isExit)
				{
					List<IData> removeLists = new List<IData>();
					for (int i = 0; i <= num; i++)
					{
						removeLists.Add(result[i]);
					}
					result.RemoveAll((IData l) => removeLists.Contains(l));
				}
				else
				{
					result.RemoveAt(num);
				}
			}
			Data data2 = new Data();
			data2["uid"].SetResult(musicUid);
			data2["count"].SetResult(targetValue);
			result.Add(data2);
			if (result.Count >= passTarget)
			{
				if (!targetFunc(result.Sum((IData l) => l["count"].GetResult<int>())))
				{
					result.RemoveAt(0);
					return;
				}
				result.Clear();
				tasks.Remove(uid);
				Reward(uid);
			}
		}
	}
}
