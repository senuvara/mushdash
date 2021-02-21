using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using FormulaBase;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameCore.Managers
{
	public class ItemManager : Singleton<ItemManager>
	{
		private List<IData> m_RewardItems;

		public const string character = "character";

		public const string elfin = "elfin";

		public const string loading = "loading";

		public const string welcome = "welcome";

		private int m_TotalCount;

		public List<IData> items
		{
			get;
			private set;
		}

		public int totalCharacterCount
		{
			get;
			private set;
		}

		public int totalElfinCount
		{
			get;
			private set;
		}

		public int totalLoadingCount
		{
			get;
			private set;
		}

		public int totalWelcomeCount
		{
			get;
			private set;
		}

		public int addCount
		{
			get;
			private set;
		}

		private void Init()
		{
			items = Singleton<DataManager>.instance["Account"]["Items"].GetResult<List<IData>>();
			m_TotalCount = 0;
			totalCharacterCount = 0;
			totalElfinCount = 0;
			totalLoadingCount = 0;
			totalWelcomeCount = 0;
			JArray json = Singleton<ConfigManager>.instance.GetJson("character", false);
			for (int i = 0; i < json.Count; i++)
			{
				if ((bool)json[i]["free"] && !(bool)json[i]["hide"])
				{
					totalCharacterCount++;
				}
			}
			JArray json2 = Singleton<ConfigManager>.instance.GetJson("elfin", false);
			for (int j = 0; j < json2.Count; j++)
			{
				if ((bool)json2[j]["free"] && !(bool)json2[j]["hide"])
				{
					totalElfinCount++;
				}
			}
			JArray json3 = Singleton<ConfigManager>.instance.GetJson("loading", false);
			for (int k = 0; k < json3.Count; k++)
			{
				if ((bool)json3[k]["free"] && !(bool)json3[k]["hide"])
				{
					totalLoadingCount++;
				}
			}
			JArray json4 = Singleton<ConfigManager>.instance.GetJson("welcome", false);
			for (int l = 0; l < json4.Count; l++)
			{
				if ((bool)json4[l]["free"] && !(bool)json4[l]["hide"])
				{
					totalWelcomeCount++;
				}
			}
			m_TotalCount = totalCharacterCount + totalElfinCount + totalLoadingCount + totalWelcomeCount;
		}

		public bool Reward(int count = 2)
		{
			m_RewardItems = Singleton<DataManager>.instance["Account"]["RewardItems"].GetResult<List<IData>>();
			m_RewardItems.Clear();
			if (items.Count((IData i) => i["isUnlock"].GetResult<bool>() && i["free"].GetResult<bool>() && !i["hide"].GetResult<bool>()) >= m_TotalCount)
			{
				Singleton<AchievementManager>.instance.Do();
				if (SingletonMonoBehaviour<MessageManager>.instance.messages.Count((IData m) => m["type"].GetResult<string>() == "achievement") > 0)
				{
					Singleton<StatisticsManager>.instance.OnChallengeAchievementEnd();
				}
				return false;
			}
			int num = items.Count((IData i) => i["isUnlock"].GetResult<bool>() && i["type"].GetResult<string>() == "character" && i["free"].GetResult<bool>() && !i["hide"].GetResult<bool>());
			int num2 = items.Count((IData i) => i["isUnlock"].GetResult<bool>() && i["type"].GetResult<string>() == "elfin" && i["free"].GetResult<bool>() && !i["hide"].GetResult<bool>());
			int num3 = items.Count((IData i) => i["isUnlock"].GetResult<bool>() && i["type"].GetResult<string>() == "loading" && i["free"].GetResult<bool>() && !i["hide"].GetResult<bool>());
			int num4 = items.Count((IData i) => i["isUnlock"].GetResult<bool>() && i["type"].GetResult<string>() == "welcome" && i["free"].GetResult<bool>() && !i["hide"].GetResult<bool>());
			string[] array = new string[5]
			{
				string.Empty,
				"c",
				"b",
				"a",
				"s"
			};
			for (int j = 0; j < count; j++)
			{
				for (int k = 0; k < array.Length - 1; k++)
				{
					string rarity = array[k];
					string nextRarity = array[k + 1];
					if (OnStateReward(rarity, nextRarity))
					{
						break;
					}
				}
			}
			if (m_RewardItems.Count > 0)
			{
				m_RewardItems.Sort(delegate(IData l, IData r)
				{
					string result = l["type"].GetResult<string>();
					string result2 = r["type"].GetResult<string>();
					if (result != result2)
					{
						if (result == "character")
						{
							return -1;
						}
						if (result2 == "character")
						{
							return 1;
						}
						if (result == "elfin")
						{
							return -1;
						}
						if (result2 == "elfin")
						{
							return 1;
						}
						if (result == "loading")
						{
							return -1;
						}
						if (result2 == "loading")
						{
							return 1;
						}
						if (result == "welcome")
						{
							return -1;
						}
						if (result2 == "welcome")
						{
							return 1;
						}
					}
					return 0;
				});
				bool flag = items.Count((IData i) => i["isUnlock"].GetResult<bool>() && i["type"].GetResult<string>() == "character" && i["free"].GetResult<bool>() && !i["hide"].GetResult<bool>()) != num;
				bool flag2 = items.Count((IData i) => i["isUnlock"].GetResult<bool>() && i["type"].GetResult<string>() == "elfin" && i["free"].GetResult<bool>() && !i["hide"].GetResult<bool>()) != num2;
				bool flag3 = items.Count((IData i) => i["isUnlock"].GetResult<bool>() && i["type"].GetResult<string>() == "loading" && i["free"].GetResult<bool>() && !i["hide"].GetResult<bool>()) != num3;
				bool flag4 = items.Count((IData i) => i["isUnlock"].GetResult<bool>() && i["type"].GetResult<string>() == "welcome" && i["free"].GetResult<bool>() && !i["hide"].GetResult<bool>()) != num4;
				if (flag)
				{
					SingletonMonoBehaviour<MessageManager>.instance.Send("unlockRole");
					PnlNavigationBtnOption.UnlockNew(PnlNavigationBtnOption.MenuType.Role);
				}
				if (flag2)
				{
					SingletonMonoBehaviour<MessageManager>.instance.Send("unlockElfin");
					PnlNavigationBtnOption.UnlockNew(PnlNavigationBtnOption.MenuType.Elfin);
				}
				if (flag3)
				{
					SingletonMonoBehaviour<MessageManager>.instance.Send("unlockLoading");
				}
				if (flag4)
				{
					SingletonMonoBehaviour<MessageManager>.instance.Send("unlockWelcome");
				}
				Singleton<AchievementManager>.instance.Do();
				if (SingletonMonoBehaviour<MessageManager>.instance.messages.Count((IData m) => m["type"].GetResult<string>() == "achievement") > 0)
				{
					Singleton<StatisticsManager>.instance.OnChallengeAchievementEnd();
				}
			}
			return true;
		}

		public bool OnStateReward(string rarity, string nextRarity)
		{
			if (string.IsNullOrEmpty(rarity))
			{
				if (items.Count((IData item) => item["isSkin"].GetResult<bool>() && item["isUnlock"].GetResult<bool>() && item["free"].GetResult<bool>() && !item["hide"].GetResult<bool>()) <= 3)
				{
					IData data = items.Find((IData item) => item["index"].GetResult<int>() == 4 && item["type"].GetResult<string>() == "character" && item["isUnlock"].GetResult<bool>() && item["free"].GetResult<bool>() && !item["hide"].GetResult<bool>());
					if (data != null)
					{
						AddSkin("c");
					}
					else
					{
						RandomUtils.RandomEvent(new float[2]
						{
							0.5f,
							0.5f
						}, delegate
						{
							AddItem("character", 4, 1);
						}, delegate
						{
							AddSkin("c");
						});
					}
					return true;
				}
				return false;
			}
			int num = Singleton<ConfigManager>.instance.GetJson("character", false).Count((JToken t) => (string)t["rarity"] == rarity && (bool)t["free"] && !(bool)t["hide"]);
			num += Singleton<ConfigManager>.instance.GetJson("elfin", false).Count((JToken t) => (string)t["rarity"] == rarity);
			bool flag = items.Count((IData item) => item["isSkin"].GetResult<bool>() && item["isUnlock"].GetResult<bool>() && item["rarity"].GetResult<string>() == rarity && item["free"].GetResult<bool>() && !item["hide"].GetResult<bool>()) == num;
			num = Singleton<ConfigManager>.instance.GetJson("character", false).Count((JToken t) => (string)t["rarity"] == nextRarity && (bool)t["free"] && !(bool)t["hide"]);
			num += Singleton<ConfigManager>.instance.GetJson("elfin", false).Count((JToken t) => (string)t["rarity"] == nextRarity && (bool)t["free"] && !(bool)t["hide"]);
			bool flag2 = items.Count((IData item) => item["isSkin"].GetResult<bool>() && item["isUnlock"].GetResult<bool>() && item["rarity"].GetResult<string>() == nextRarity && item["free"].GetResult<bool>() && !item["hide"].GetResult<bool>()) == num;
			bool flag3 = items.Count((IData item) => item["isSkin"].GetResult<bool>() && item["isUnlock"].GetResult<bool>() && item["rarity"].GetResult<string>() == nextRarity && item["free"].GetResult<bool>() && !item["hide"].GetResult<bool>()) >= 2 && rarity != "a";
			bool flag4 = items.Count((IData item) => !item["isSkin"].GetResult<bool>() && item["isUnlock"].GetResult<bool>() && item["free"].GetResult<bool>() && !item["hide"].GetResult<bool>()) == totalWelcomeCount;
			if (flag && flag3)
			{
				return false;
			}
			bool flag5 = items.Count((IData item) => !item["isSkin"].GetResult<bool>() && item["isUnlock"].GetResult<bool>() && item["free"].GetResult<bool>() && !item["hide"].GetResult<bool>()) == totalLoadingCount;
			if (!flag && !flag2 && !flag5)
			{
				RandomUtils.RandomEvent(new float[3]
				{
					0.5f,
					0.25f,
					0.25f
				}, delegate
				{
					AddSkin(rarity);
				}, delegate
				{
					AddSkin(nextRarity);
				}, AddLoading);
				return true;
			}
			if (flag && !flag2 && !flag5)
			{
				RandomUtils.RandomEvent(new float[2]
				{
					0.5f,
					0.5f
				}, delegate
				{
					AddSkin(nextRarity);
				}, AddLoading);
				return true;
			}
			if (!flag && flag2 && !flag5)
			{
				RandomUtils.RandomEvent(new float[2]
				{
					0.6667f,
					0.3333f
				}, delegate
				{
					AddSkin(rarity);
				}, AddLoading);
				return true;
			}
			if (!flag && !flag2)
			{
				RandomUtils.RandomEvent(new float[2]
				{
					0.6667f,
					0.3333f
				}, delegate
				{
					AddSkin(rarity);
				}, delegate
				{
					AddSkin(nextRarity);
				});
				return true;
			}
			if (!flag)
			{
				AddSkin(rarity);
				return true;
			}
			if (!flag2)
			{
				AddSkin(nextRarity);
				return true;
			}
			if (!flag5)
			{
				AddLoading();
				return true;
			}
			if (!flag4)
			{
				AddWelcome();
				return true;
			}
			return false;
		}

		private void AddSkin(string rarity)
		{
			string type = "elfin";
			RandomUtils.RandomEvent(new float[2]
			{
				0.5f,
				0.5f
			}, delegate
			{
				type = ((items.Count((IData item) => item["isUnlock"].GetResult<bool>() && item["type"].GetResult<string>() == "elfin" && item["free"].GetResult<bool>() && !item["hide"].GetResult<bool>()) != totalElfinCount) ? "elfin" : "character");
			}, delegate
			{
				type = ((items.Count((IData item) => item["isUnlock"].GetResult<bool>() && item["type"].GetResult<string>() == "character" && item["free"].GetResult<bool>() && !item["hide"].GetResult<bool>()) != totalCharacterCount) ? "character" : "elfin");
			});
			if (items.Count((IData item) => item["isUnlock"].GetResult<bool>() && item["type"].GetResult<string>() == type && item["rarity"].GetResult<string>() == rarity && item["free"].GetResult<bool>() && !item["hide"].GetResult<bool>()) == Singleton<ConfigManager>.instance.GetJson(type, false).Count((JToken c) => (string)c["rarity"] == rarity && (bool)c["free"] && !(bool)c["hide"]))
			{
				type = ((!(type == "character")) ? "character" : "elfin");
			}
			JArray json = Singleton<ConfigManager>.instance.GetJson(type, false);
			List<int> list = new List<int>();
			for (int i = 0; i < json.Count; i++)
			{
				IData data = items.Find((IData t) => t["type"].GetResult<string>() == type && t["index"].GetResult<int>() == i && t["free"].GetResult<bool>() && !t["hide"].GetResult<bool>());
				if ((string)json[i]["rarity"] == rarity && (bool)json[i]["free"] && !(bool)json[i]["hide"] && (data == null || !data["isUnlock"].GetResult<bool>()))
				{
					list.Add(i);
				}
			}
			int index = list.Random();
			AddItem(type, index, 1);
		}

		private void AddLoading()
		{
			JArray json = Singleton<ConfigManager>.instance.GetJson("loading", false);
			List<int> list = new List<int>();
			int i;
			for (i = 0; i < json.Count; i++)
			{
				IData data = items.Find((IData t) => t["type"].GetResult<string>() == "loading" && t["index"].GetResult<int>() == i);
				if ((data == null || !data["isUnlock"].GetResult<bool>()) && (bool)json[i]["free"] && !(bool)json[i]["hide"])
				{
					list.Add(i);
				}
			}
			int index = list.Random();
			AddItem("loading", index, 1);
		}

		private void AddWelcome()
		{
			JArray json = Singleton<ConfigManager>.instance.GetJson("welcome", false);
			List<int> list = new List<int>();
			int i;
			for (i = 0; i < json.Count; i++)
			{
				IData data = items.Find((IData t) => t["type"].GetResult<string>() == "welcome" && t["index"].GetResult<int>() == i);
				if ((data == null || !data["isUnlock"].GetResult<bool>()) && (bool)json[i]["free"] && !(bool)json[i]["hide"])
				{
					list.Add(i);
				}
			}
			if (list.Count > 0)
			{
				int index = list.Random();
				AddItem("welcome", index, 1);
			}
		}

		public void AddItem(string type, int index, int count)
		{
			IData data = items.Find((IData t) => t["type"].GetResult<string>() == type && t["index"].GetResult<int>() == index);
			if (data != null)
			{
				bool result = data["isUnlock"].GetResult<bool>();
				int num = data["count"].GetResult<int>() + count;
				int result2 = data["chipAmount"].GetResult<int>();
				num = ((num <= result2) ? num : result2);
				data["count"].SetResult(num);
				if (!result && data["isUnlock"].GetResult<bool>())
				{
					data["lockNew"].SetResult(true);
				}
			}
			else
			{
				data = new Data("Item");
				data["type"].SetResult(type);
				data["index"].SetResult(index);
				data["new"].SetResult(true);
				int result3 = data["chipAmount"].GetResult<int>();
				count = ((count <= result3) ? count : result3);
				data["count"].SetResult(count);
				int num2 = 0;
				for (int i = 0; i < items.Count; i++)
				{
					IData data2 = items[i];
					string result4 = data2["type"].GetResult<string>();
					if (result4 == type)
					{
						num2 = ((data2["index"].GetResult<int>() <= index) ? (i + 1) : i);
					}
					else if (result4 == "character")
					{
						num2 = i + 1;
					}
					else if (type == "character")
					{
						if (num2 > i)
						{
							num2 = i;
						}
					}
					else if (result4 == "elfin")
					{
						num2 = i + 1;
					}
					else if (type == "elfin")
					{
						if (num2 > i)
						{
							num2 = i;
						}
					}
					else if (result4 == "loading")
					{
						num2 = i + 1;
					}
					else if (type == "loading")
					{
						if (num2 > i)
						{
							num2 = i;
						}
					}
					else if (result4 == "welcome")
					{
						num2 = i + 1;
					}
					else if (type == "welcome" && num2 > i)
					{
						num2 = i;
					}
				}
				items.Insert(num2, data);
			}
			for (int j = 0; j < count; j++)
			{
				Data data3 = new Data();
				data3["type"].SetResult(type);
				data3["index"].SetResult(index);
				if (m_RewardItems != null)
				{
					m_RewardItems.Add(data3);
				}
			}
			if (type == "loading" && data["isUnlock"].GetResult<bool>())
			{
				AddItemToUsedList(type, data["index"].GetResult<int>());
			}
			if (type == "welcome" && data["isUnlock"].GetResult<bool>())
			{
				AddItemToUsedList(type, data["index"].GetResult<int>());
			}
		}

		public void RemoveItem(string type, int index)
		{
			items.RemoveAll((IData t) => t["type"].GetResult<string>() == type && t["index"].GetResult<int>() == index);
		}

		public void AddExtraItem(string type, int index, int count, bool awardTip = true)
		{
			if (m_RewardItems != null)
			{
				m_RewardItems.Clear();
			}
			IData data = items.Find((IData t) => t["type"].GetResult<string>() == type && t["index"].GetResult<int>() == index);
			if (data != null && data["isUnlock"].GetResult<bool>())
			{
				return;
			}
			if (awardTip)
			{
				Singleton<DataManager>.instance["Account"]["ShowPnlItemAward"].SetResult(true);
			}
			data = (data ?? new Data("Item"));
			data["type"].SetResult(type);
			data["index"].SetResult(index);
			data["new"].SetResult(awardTip);
			data["lockNew"].SetResult(awardTip);
			int result = data["chipAmount"].GetResult<int>();
			int result2 = data["count"].GetResult<int>();
			if (result2 < result)
			{
				count = ((count + result2 <= result) ? count : (result - result2));
			}
			addCount = count;
			data["count"].SetResult(result2 + count);
			int num = 0;
			for (int i = 0; i < items.Count; i++)
			{
				IData data2 = items[i];
				string result3 = data2["type"].GetResult<string>();
				if (result3 == type)
				{
					num = ((data2["index"].GetResult<int>() <= index) ? (i + 1) : i);
				}
				else if (result3 == "character")
				{
					num = i + 1;
				}
				else if (type == "character")
				{
					if (num > i)
					{
						num = i;
					}
				}
				else if (result3 == "elfin")
				{
					num = i + 1;
				}
				else if (type == "elfin")
				{
					if (num > i)
					{
						num = i;
					}
				}
				else if (result3 == "loading")
				{
					num = i + 1;
				}
				else if (type == "loading")
				{
					if (num > i)
					{
						num = i;
					}
				}
				else if (result3 == "welcome")
				{
					num = i + 1;
				}
				else if (type == "welcome" && num > i)
				{
					num = i;
				}
			}
			items.Insert(num, data);
			if (awardTip)
			{
				Data data3 = new Data();
				data3["type"].SetResult(type);
				data3["index"].SetResult(index);
				data3["count"].SetResult(data["count"].GetResult<int>());
				if (m_RewardItems == null)
				{
					m_RewardItems = Singleton<DataManager>.instance["Account"]["RewardItems"].GetResult<List<IData>>();
					m_RewardItems.Clear();
				}
				m_RewardItems.Add(data3);
				Singleton<DataManager>.instance["Account"]["RewardItems"].SetResult(m_RewardItems);
				if (data["isUnlock"].GetResult<bool>())
				{
					object obj;
					switch (data["type"].GetResult<string>())
					{
					case "character":
						obj = "unlockRole";
						break;
					case "elfin":
						obj = "unlockElfin";
						break;
					case "loading":
						obj = "unlockLoading";
						break;
					default:
						obj = "unlockWelcome";
						break;
					}
					string type2 = (string)obj;
					SingletonMonoBehaviour<MessageManager>.instance.Send(type2);
				}
				Singleton<AchievementManager>.instance.Do();
				if (SingletonMonoBehaviour<MessageManager>.instance.messages != null && SingletonMonoBehaviour<MessageManager>.instance.messages.Count((IData m) => m["type"].GetResult<string>() == "achievement") > 0)
				{
					Singleton<StatisticsManager>.instance.OnChallengeAchievementEnd();
				}
			}
			if (type == "loading" && data["isUnlock"].GetResult<bool>())
			{
				AddItemToUsedList(type, data["index"].GetResult<int>());
			}
			if (data["type"].GetResult<string>() == "welcome" && data["index"].GetResult<int>() >= 3)
			{
				AddItemToUsedList(type, data["index"].GetResult<int>());
			}
		}

		public void AddItemToUsedList(string type, int index)
		{
			if (type == "loading")
			{
				if (!Singleton<DataManager>.instance["Account"]["UseLoadingIndex"].GetResult<List<int>>().Contains(index))
				{
					Singleton<DataManager>.instance["Account"]["UseLoadingIndex"].GetResult<List<int>>().Add(index);
				}
			}
			else if (type == "welcome" && !Singleton<DataManager>.instance["Account"]["UseWelcomeIndex"].GetResult<List<int>>().Contains(index))
			{
				Singleton<DataManager>.instance["Account"]["UseWelcomeIndex"].GetResult<List<int>>().Add(index);
			}
		}

		public bool GetItemIsUsed(string type)
		{
			List<string> result = Singleton<DataManager>.instance["Account"]["UsedWelcomeIndex"].GetResult<List<string>>();
			bool result2 = false;
			if (type == "loading")
			{
				result = Singleton<DataManager>.instance["Account"]["UsedLoadingIndex"].GetResult<List<string>>();
			}
			else if (type == "welcome")
			{
				result = Singleton<DataManager>.instance["Account"]["UsedWelcomeIndex"].GetResult<List<string>>();
			}
			for (int i = 0; i < result.Count; i++)
			{
				if (result.Contains(items[Singleton<DataManager>.instance["Account"]["SelectedItemIndex"].GetResult<int>()]["index"].GetResult<int>().ToString()))
				{
					result2 = true;
				}
			}
			return result2;
		}

		public void CheckAndAddWelcome(int index, int count, bool showTips)
		{
			IData data = items.Find((IData t) => t["type"].GetResult<string>() == "welcome" && t["index"].GetResult<int>() == index);
			if (data == null)
			{
				AddItem("welcome", index, count);
				Singleton<DataManager>.instance.Save();
			}
		}

		public void CleanWelcomeIndexListForNew()
		{
			Singleton<DataManager>.instance["Account"]["UseWelcomeIndex"].GetResult<List<int>>().Clear();
		}

		public bool ChristmasItemLogic(string itemType)
		{
			DateTime now = DateTime.Now;
			int month = now.Month;
			int day = now.Day;
			int year = now.Year;
			if (month != 12 || (day != 24 && day != 25))
			{
				return false;
			}
			IData data = items.Find((IData t) => t["type"].GetResult<string>() == "welcome" && t["index"].GetResult<int>() == 3);
			if (itemType.Equals("character"))
			{
				List<string> result = Singleton<DataManager>.instance["Account"]["GetSantaRinTimeList"].GetResult<List<string>>();
				if (result != null && !result.Contains(year.ToString()))
				{
					if (!Singleton<StageBattleComponent>.instance.isNew)
					{
						result.Add(year.ToString());
						Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].SetResult(13);
					}
					Singleton<DataManager>.instance["Account"]["HasGetSantaRin"].SetResult(false);
					Singleton<DataManager>.instance.Save();
				}
			}
			if (itemType.Equals("welcome"))
			{
				List<string> result2 = Singleton<DataManager>.instance["Account"]["GetChristmasWelcomeTimeList"].GetResult<List<string>>();
				if (result2 != null && !result2.Contains(year.ToString()))
				{
					result2.Add(year.ToString());
					CheckAndAddWelcome(3, 5, false);
					return true;
				}
				if (data == null)
				{
					CheckAndAddWelcome(3, 5, false);
					return true;
				}
			}
			return false;
		}

		public bool IsChirstmas()
		{
			DateTime now = DateTime.Now;
			int month = now.Month;
			int day = now.Day;
			if (month == 12 && (day == 24 || day == 25))
			{
				return true;
			}
			return false;
		}

		public bool IsMay()
		{
			DateTime now = DateTime.Now;
			int year = now.Year;
			int month = now.Month;
			int day = now.Day;
			if (year == 2020 && ((month == 4 && day == 30) || (month == 5 && day <= 7)))
			{
				CheckAndAddWelcome(4, 5, false);
				if (!Singleton<DataManager>.instance["Account"]["IsShowMay"].GetResult<bool>())
				{
					Singleton<DataManager>.instance["Account"]["IsShowMay"].SetResult(true);
					Singleton<DataManager>.instance.Save();
					return true;
				}
				return false;
			}
			return false;
		}

		public bool TimeIsMay()
		{
			DateTime now = DateTime.Now;
			int year = now.Year;
			int month = now.Month;
			int day = now.Day;
			if (year == 2020 && ((month == 4 && day == 30) || (month == 5 && day <= 7)))
			{
				return true;
			}
			return false;
		}

		public bool TimeIsNanahira()
		{
			DateTime now = DateTime.Now;
			int year = now.Year;
			int month = now.Month;
			int day = now.Day;
			if (year == 2020 && ((month == 7 && day == 31) || (month == 8 && day <= 7)))
			{
				return true;
			}
			return false;
		}

		public bool IsNanahira()
		{
			DateTime now = DateTime.Now;
			int year = now.Year;
			int month = now.Month;
			int day = now.Day;
			if (year == 2020 && ((month == 7 && day == 31) || (month == 8 && day <= 7)))
			{
				CheckAndAddWelcome(5, 5, false);
				if (!Singleton<DataManager>.instance["Account"]["IsShowNanahira"].GetResult<bool>())
				{
					Singleton<DataManager>.instance["Account"]["IsShowNanahira"].SetResult(true);
					Singleton<DataManager>.instance.Save();
					return true;
				}
				return false;
			}
			return false;
		}

		public bool IsFestivalCarnival()
		{
			DateTime now = DateTime.Now;
			int month = now.Month;
			int day = now.Day;
			if (month == 2 && day >= 5 && day <= 18)
			{
				CheckAndAddWelcome(5, 5, false);
				List<string> result = Singleton<DataManager>.instance["GameConfig"]["FirsetOpenEvent"].GetResult<List<string>>();
				if (!result.Contains("IsFestivalCarnival"))
				{
					result.Add("IsFestivalCarnival");
					Singleton<DataManager>.instance.Save();
					return true;
				}
				return false;
			}
			return false;
		}
	}
}
