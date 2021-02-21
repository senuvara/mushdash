using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameCore.Managers
{
	public class MessageManager : SingletonMonoBehaviour<MessageManager>
	{
		public const string task = "task";

		public const string achievement = "achievement";

		public const string level = "level";

		public const string exp = "exp";

		public const string item = "item";

		public const string unlockStage = "unlockStage";

		public const string stageAchievement = "stage_achievement";

		public const string unlockLevel = "unlockLevel";

		public const string unlockRole = "unlockRole";

		public const string unlockElfin = "unlockElfin";

		public const string unlockLoading = "unlockLoading";

		public const string unlockWelcome = "unlockWelcome";

		public const string rank = "rank";

		[HideInInspector]
		public static int maxExp = 100;

		public List<IData> messages
		{
			get;
			private set;
		}

		public bool available
		{
			get
			{
				List<IData> list = messages.Where(delegate(IData m)
				{
					string result = m["type"].GetResult<string>();
					int result2;
					switch (result)
					{
					default:
						result2 = ((result == "rank") ? 1 : 0);
						break;
					case "task":
					case "achievement":
					case "stage_achievement":
					case "unlockLevel":
						result2 = 1;
						break;
					}
					return (byte)result2 != 0;
				});
				return list.Count > 0;
			}
		}

		public void Init()
		{
			Transform root = base.transform.root;
			Object.DontDestroyOnLoad((!root) ? base.gameObject : root.gameObject);
			messages = Singleton<DataManager>.instance["Account"]["Messages"].GetResult<List<IData>>();
		}

		public void Send(string type, params object[] msg)
		{
			if (messages == null)
			{
				messages = Singleton<DataManager>.instance["Account"]["Messages"].GetResult<List<IData>>();
			}
			if (type == "exp")
			{
				int num = (int)msg[0];
				num = Mathf.RoundToInt(num);
				int result = Singleton<DataManager>.instance["Account"]["CurExp"].GetResult<int>();
				int maxAddExp = maxExp - result;
				bool flag = messages.Exists((IData m) => m["count"].GetResult<int>() == maxAddExp);
				IData data = messages.Find(delegate(IData m)
				{
					string result3 = m["type"].GetResult<string>();
					int result4 = m["count"].GetResult<int>();
					return result3 == "exp" && result4 != maxAddExp && result4 < maxExp;
				});
				if (data == null)
				{
					data = new Data();
					data["type"].SetResult(type);
					data["count"].SetResult(0);
					messages.Add(data);
				}
				int result2 = data["count"].GetResult<int>();
				if (flag)
				{
					maxAddExp = maxExp;
				}
				if (result2 + num > maxAddExp)
				{
					num = result2 + num - maxAddExp;
					data["count"].SetResult(maxAddExp);
					Send("exp", num);
				}
				else
				{
					data["count"].SetResult(result2 + num);
				}
			}
			else if (type == "level" || type == "item")
			{
				int num2 = (int)msg[0];
				IData data2 = messages.Find((IData m) => m["type"].GetResult<string>() == type);
				if (data2 == null)
				{
					data2 = new Data();
					data2["type"].SetResult(type);
					data2["count"].SetResult(0);
					messages.Add(data2);
				}
				data2["count"].SetResult(data2["count"].GetResult<int>() + num2);
			}
			else if (type == "achievement" || type == "task" || type == "stage_achievement")
			{
				string value = (string)msg[0];
				Data data3 = new Data();
				data3["type"].SetResult(type);
				data3["uid"].SetResult(value);
				messages.Add(data3);
			}
			else if (type == "unlockStage" || type == "unlockLevel" || type == "unlockRole" || type == "unlockElfin" || type == "unlockLoading" || type == "unlockWelcome" || type == "rank")
			{
				Data data4 = new Data();
				data4["type"].SetResult(type);
				messages.Add(data4);
			}
		}

		public void Receive(string type, params object[] msg)
		{
			if (type == "exp" || type == "level")
			{
				IData data = messages.Find((IData m) => m["type"].GetResult<string>() == type);
				int result = data["count"].GetResult<int>();
				messages.Remove(data);
				if (type == "exp")
				{
					int result2 = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
					int result3 = Singleton<DataManager>.instance["Account"]["Exp"].GetResult<int>();
					Singleton<DataManager>.instance["Account"]["Exp"].SetResult(result3 + result);
					int result4 = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
					if (result4 != result2)
					{
						Send("level", result4 - result2);
					}
				}
				else if (type == "level")
				{
					Receive("item");
					Receive("unlockStage");
				}
			}
			else if (type == "item")
			{
				Singleton<ItemManager>.instance.Reward();
			}
			else if (type == "unlockStage")
			{
				Singleton<StageManager>.instance.UnlockStage();
			}
			else if (type == "unlockLevel" || type == "unlockRole" || type == "unlockElfin" || type == "unlockLoading" || type == "unlockWelcome" || type == "rank")
			{
				messages.RemoveAll((IData m) => m["type"].GetResult<string>() == type);
			}
			else if (type == "task" || type == "achievement" || type == "stage_achievement")
			{
				string uid = (string)msg[0];
				messages.RemoveAll((IData m) => m["type"].GetResult<string>() == type && m["uid"].GetResult<string>() == uid);
				int num = (!(type == "task") && !(type == "achievement")) ? 20 : Singleton<ConfigManager>.instance.GetConfigIntValue(type, "uid", "reward", uid);
				Send("exp", num);
			}
		}
	}
}
