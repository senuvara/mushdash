using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Others;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameCore.Managers
{
	public class WeekFreeManager : Singleton<WeekFreeManager>
	{
		private bool m_OldVersion;

		public string[] freeSongUids
		{
			get;
			private set;
		}

		public Dictionary<string, string> discounts
		{
			get;
			private set;
		}

		public string[] freeAlbumUids
		{
			get
			{
				if (freeSongUids == null)
				{
					return null;
				}
				List<string> list = new List<string>();
				for (int i = 0; i < freeSongUids.Length; i++)
				{
					string item = $"music_package_{freeSongUids[i].BeginBefore('-')}";
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
				return list.ToArray();
			}
		}

		public int[] freeAlbumIndexs
		{
			get
			{
				if (freeSongUids == null)
				{
					return null;
				}
				List<int> list = new List<int>();
				for (int i = 0; i < freeSongUids.Length; i++)
				{
					string str = freeSongUids[i];
					int item = int.Parse(str.BeginBefore('-'));
					if (!list.Contains(item))
					{
						list.Add(item);
					}
				}
				return list.ToArray();
			}
		}

		private void Init()
		{
			freeSongUids = new string[0];
			Singleton<Assets.Scripts.PeroTools.Managers.SceneManager>.instance.onSceneLoad += OnSceneLoad;
			OnSceneRefresh("GameMain");
		}

		private void OnSceneLoad(Scene scene, LoadSceneMode mode)
		{
			OnSceneRefresh(scene.name);
		}

		private void OnSceneRefresh(string sceneName)
		{
			if (sceneName.StartsWith("GameMain"))
			{
				Refresh();
			}
			else
			{
				if (!sceneName.StartsWith("UISystem") || this.freeSongUids == null || this.freeSongUids.Length <= 0)
				{
					return;
				}
				string text = string.Empty;
				string[] freeSongUids = this.freeSongUids;
				foreach (string str in freeSongUids)
				{
					text += str;
				}
				string text2 = freeAlbumUids.First();
				if (Singleton<DataManager>.instance["IAP"]["unlockall_0"].GetResult<bool>() || Singleton<Assets.Scripts.PeroTools.Managers.SceneManager>.instance.sceneName.StartsWith("GameMain"))
				{
					return;
				}
				string result = Singleton<DataManager>.instance["GameConfig"]["WeekFreeSongUdid"].GetResult<string>();
				if (!(result != text))
				{
					return;
				}
				if (Singleton<DataManager>.instance["Account"]["Exp"].GetResult<int>() > 0)
				{
					Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"].SetResult(text2);
					string str2 = this.freeSongUids.First();
					for (int j = 0; j < this.freeSongUids.Length; j++)
					{
						string text3 = this.freeSongUids[j];
						if (text3.BeginBefore('-') == text2.LastAfter('_'))
						{
							str2 = text3;
							break;
						}
					}
					Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].SetResult(int.Parse(str2.LastAfter('-')));
				}
				Singleton<DataManager>.instance["GameConfig"]["WeekFreeSongUdid"].SetResult(text);
			}
		}

		private void Refresh()
		{
			ServerManager instance = Singleton<ServerManager>.instance;
			string url = "musedash/v3/weekly";
			Dictionary<string, object> datas = new Dictionary<string, object>
			{
				{
					"accesskey_id",
					50257764
				},
				{
					"nonce",
					ServerManager.RandomString(16)
				},
				{
					"timestamp",
					(int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds
				},
				{
					"version",
					"v2"
				}
			};
			Action<JObject> callback = delegate(JObject data)
			{
				JToken jToken = data["items"];
				int num = jToken.Count();
				freeSongUids = new string[num];
				discounts = new Dictionary<string, string>();
				for (int i = 0; i < num; i++)
				{
					JToken jToken2 = jToken[i];
					string text = (string)jToken2["music_uid"];
					float num2 = (float)jToken2["discount"];
					string key = (string)jToken2["product"];
					if (!discounts.ContainsKey(key))
					{
						discounts.Add(key, num2.ToString("P").Replace(".00 ", string.Empty));
					}
					freeSongUids[i] = text;
				}
				int num3 = int.Parse(Singleton<ConfigManager>.instance.GetConfigStringValue("albums", 2, "uid").LastAfter('_'));
				List<string> list = new List<string>();
				for (int j = 0; j < freeSongUids.Length; j++)
				{
					string item = freeSongUids[j];
					if (int.Parse(freeSongUids[j].BeginBefore('-')) > num3)
					{
						list.Add(item);
					}
				}
				m_OldVersion = (list.Count > 0);
				for (int k = 0; k < list.Count; k++)
				{
					freeSongUids = freeSongUids.Remove(list[k]);
				}
				OnSceneRefresh("UISystem");
			};
			string appkey = "u¡ar~\u008c\u00a0\\pqq\u009f\u007ft\u0091\u0095\u0096£a¢\u0097\u008f~ym[^\u0091[\u0099_vp\u008e\u0092^\u009ey\u0084Zs|}x\\t\u009dtmb\u0083\u008dm\u009d\u0099\u008c\\\u009a[t\u0097\u0098y\u0093".Caesar(-42);
			instance.SendToUrl(url, "GET", datas, callback, null, null, 0, false, false, appkey);
		}
	}
}
