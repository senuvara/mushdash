using Assets.Scripts.Common;
using Assets.Scripts.Common.XDSDK;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using LeanCloud;
using LeanCloud.Storage.Internal;
using Newtonsoft.Json.Linq;
using ProtoBuf;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.PeroTools.Managers
{
	public class ServerManager : Singleton<ServerManager>
	{
		private bool m_IsAvailable;

		private static System.Random random = new System.Random();

		public bool useProduction
		{
			get;
			private set;
		}

		public bool useTest
		{
			get;
			private set;
		}

		private void Init()
		{
			useProduction = SingletonScriptableObject<ConstanceManager>.instance.GetBool("useProduction");
			AVClient.UseProduction = useProduction;
			useTest = SingletonScriptableObject<ConstanceManager>.instance.GetBool("useTest");
			m_IsAvailable = true;
			if (m_IsAvailable)
			{
				AVClient.Configuration configuration = default(AVClient.Configuration);
				configuration.ApplicationId = Singleton<DataManager>.instance["GameConfig"]["ApplicationID"].GetResult<string>();
				configuration.ApplicationKey = Singleton<DataManager>.instance["GameConfig"]["ApplicationKey"].GetResult<string>();
				AVClient.Initialize(configuration);
				Dispatcher.Instance.GameObject = SingletonMonoBehaviour<CoroutineManager>.instance.gameObject;
				SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(Dispatcher.Instance.DispatcherCoroutine);
			}
			CheckBan();
		}

		public void FindAll(string className, Action<IEnumerable<AVObject>> callback)
		{
			AVQuery<AVObject> aVQuery = new AVQuery<AVObject>(className);
			aVQuery.FindAsync().ContinueWith(delegate(Task<IEnumerable<AVObject>> t)
			{
				if (t.IsCompleted)
				{
					callback(t.Result);
				}
			});
		}

		public void Find(string className, Dictionary<string, object> targets, int count, Action<IEnumerable<AVObject>> callback)
		{
			AVQuery<AVObject> aVQuery = new AVQuery<AVObject>(className);
			if (count != -1)
			{
				aVQuery = aVQuery.Limit(count);
			}
			foreach (KeyValuePair<string, object> target in targets)
			{
				aVQuery = aVQuery.WhereEqualTo(target.Key, target.Value);
			}
			aVQuery.FindAsync().ContinueWith(delegate(Task<IEnumerable<AVObject>> t)
			{
				if (t.IsCompleted)
				{
					callback(t.Result);
				}
			});
		}

		public void SaveStrings(IList<string> strs, Action callback = null)
		{
			List<Task> tasks = new List<Task>();
			strs.For(delegate(string s)
			{
				tasks.Add(SaveString(s));
			});
			SpawnTasks(tasks, callback);
		}

		public void DeleteStrings(IList<string> strs, Action callback = null)
		{
			List<Task> tasks = new List<Task>();
			strs.For(delegate(string s)
			{
				tasks.Add(DeleteString(s));
			});
			SpawnTasks(tasks, callback);
		}

		public Task SaveString(string str, Action callback = null)
		{
			Debug.Log($"============== Save String {str} ==============");
			AVObject aVObject = new AVObject("Strings");
			aVObject["data"] = str;
			return aVObject.SaveAsync().ContinueWith(delegate
			{
				if (callback != null)
				{
					callback();
				}
				Debug.Log($"============== Save String {str} Done! ==============");
			});
		}

		public Task DeleteString(string str, Action<bool> callback = null)
		{
			Debug.Log($"============== Delete String {str} ==============");
			AVQuery<AVObject> query = new AVQuery<AVObject>("Strings").WhereEqualTo("data", str).Limit(1);
			Task<int> task = Task.FromResult(0);
			task.ContinueWith(delegate
			{
				query.FindAsync().ContinueWith(delegate(Task<IEnumerable<AVObject>> t)
				{
					if (!t.Result.Any())
					{
						Debug.Log($"============== String {str} not found! ==============");
						if (callback != null)
						{
							callback(false);
						}
					}
					else
					{
						task.ContinueWith(delegate
						{
							t.Result.First().DeleteAsync().ContinueWith(delegate
							{
								Debug.Log($"============== Delete String {str} Done! ==============");
								if (callback != null)
								{
									callback(true);
								}
							});
						});
					}
				});
			});
			return task;
		}

		public static string MD5Compute(string strPwd = "")
		{
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] bytes = Encoding.Default.GetBytes(strPwd);
			byte[] array = mD.ComputeHash(bytes);
			mD.Clear();
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				text += array[i].ToString("X").PadLeft(2, '0');
			}
			return text;
		}

		public static string RandomString(int length = 32)
		{
			IEnumerable<string> array = Enumerable.Repeat("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", length);
			return new string(LinqUtils.Select(array, (string s) => s[random.Next(s.Length)]).ToArray());
		}

		private void SpawnTasks(Action callback, params Task[] tasks)
		{
			SpawnTasks(tasks, callback);
		}

		private void SpawnTasks(IList<Task> tasks, Action callback = null)
		{
			Task.FromResult(0);
			Task task = Task.WhenAll(tasks);
			task.ContinueWith(delegate
			{
				if (callback != null)
				{
					callback();
				}
			});
		}

		public string Encrypt(string input)
		{
			return input;
		}

		public string Dncrypt(string input)
		{
			return input;
		}

		public Task<T> SendToCloud<T>(string function, Dictionary<string, object> datas = null, Action<Task<T>> callback = null, Action<AggregateException> faillCallback = null, float failTime = -1f)
		{
			Debug.Log($"==============Send to cloud: {function}==============, with data: \n{JsonUtils.Serialize(LinqUtils.ToList(datas))}");
			Dictionary<string, object> parameters = datas?.ToDictionary((Func<KeyValuePair<string, object>, string>)((KeyValuePair<string, object> data) => data.Key), (Func<KeyValuePair<string, object>, object>)((KeyValuePair<string, object> data) => Encrypt(data.Value.ToString())));
			Task<T> task = AVCloud.CallFunctionAsync<T>(function, parameters);
			Coroutine coroutine = SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				if (task.Exception == null)
				{
					Debug.Log($"==============Succuessfully recieve from cloud: {function}==============, with data: \n{JsonUtils.Serialize(task.Result)}");
					if (callback != null)
					{
						callback(task);
					}
				}
				else
				{
					Debug.Log($"==============Exception recieve from cloud: {function}==============, with data: \n{JsonUtils.Serialize(task.Exception)}");
					if (faillCallback != null)
					{
						faillCallback(task.Exception);
					}
				}
			}, () => task.IsCompleted);
			if (failTime > 0f)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					if (!task.IsCompleted)
					{
						if (coroutine != null)
						{
							SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(coroutine);
						}
						if (faillCallback != null)
						{
							faillCallback(task.Exception);
						}
					}
				}, failTime);
			}
			return task;
		}

		public void GetTime(Action<DateTime> callback)
		{
			callback(DateTime.Now);
		}

		public string GetHost(bool isLocal = false)
		{
			string result = (!useTest) ? ((!useProduction) ? "https://stg-prpr-muse-dash.peropero.net/" : "https://prpr-muse-dash.peropero.net/") : ((!useProduction) ? "https://stg-dev-muse-dash.peropero.net/" : "https://dev-muse-dash.peropero.net/");
			if (isLocal)
			{
				result = "http://192.168.1.109:3000/";
			}
			return result;
		}

		public void SendToUrl(string url, string method = "GET", Dictionary<string, object> datas = null, Action<JObject> callback = null, Action<string> faillCallback = null, Dictionary<string, string> headers = null, int failTime = 0, bool isLocal = false, bool isAutoSend = true, string appkey = null)
		{
			string host = GetHost(isLocal);
			url = $"{host}{url}";
			UnityWebRequest webRequest = WebUtils.SendToUrl(url, method, datas, delegate(DownloadHandler handler)
			{
				JObject jObject = JsonUtils.Deserialize<JObject>(handler.text);
				JToken jToken = jObject["code"];
				int num = (!jToken.IsNullOrEmpty()) ? ((int)jToken) : 0;
				if (num != 0 && num == 90001)
				{
					Singleton<XDSDKManager>.instance.Logout();
					Singleton<EventManager>.instance.Invoke("UI/OnLoginAgainRequest");
				}
				if (callback != null)
				{
					callback(jObject);
				}
			}, faillCallback, headers, failTime, isAutoSend);
			if (!isAutoSend)
			{
				SignatureDispose(webRequest, appkey);
			}
		}

		public void Synchronize(Action callback = null)
		{
			string result = Singleton<DataManager>.instance["Account"]["UserID"].GetResult<string>();
			if (!(result == "0"))
			{
				Singleton<XDSDKManager>.instance.Synchronize(callback);
			}
		}

		public void BanPlayer(bool isBan)
		{
			if (isBan)
			{
				BanTip(Singleton<SceneManager>.instance.scene, LoadSceneMode.Single);
			}
			else
			{
				Singleton<DataManager>.instance["Account"]["IsBanShown"].SetResult(false);
			}
		}

		public void CheckBan()
		{
			IsLeaderboardBan(BanPlayer);
		}

		private void SignatureDispose(UnityWebRequest webRequest, string appkey)
		{
			string text = $"{webRequest.url}&appkey={appkey}";
			string arg = MD5Compute(text.Remove(0, text.LastIndexOf("?") + 1));
			webRequest.url += $"&signature={arg}";
			webRequest.SendWebRequest();
		}

		private void BanTip(Scene scene, LoadSceneMode mode)
		{
			Singleton<SceneManager>.instance.onSceneLoad -= BanTip;
			if (scene.name.StartsWith("UISystem"))
			{
				if (!Singleton<DataManager>.instance["Account"]["IsBanShown"].GetResult<bool>())
				{
					Singleton<DataManager>.instance["Account"]["IsBanShown"].SetResult(true);
					Singleton<EventManager>.instance.Invoke("UI/YouCheat");
				}
			}
			else
			{
				Singleton<SceneManager>.instance.onSceneLoad += BanTip;
			}
		}

		private void IsLeaderboardBan(Action<bool> callback)
		{
			string result = Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>();
			if (string.IsNullOrEmpty(result))
			{
				return;
			}
			string empty = string.Empty;
			empty = "musedash/v1/pcleaderboard/is-ban";
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("devices_id", GetDeviceID());
			Dictionary<string, object> dictionary2 = dictionary;
			CSteamID steamID = SteamUser.GetSteamID();
			dictionary2.Add("steam_id", steamID.m_SteamID);
			string url = empty;
			dictionary = dictionary2;
			SendToUrl(url, "GET", dictionary, delegate(JObject token)
			{
				if (!token["ban"].IsNullOrEmpty())
				{
					callback(token["ban"].Value<bool>());
				}
			}, null, new Dictionary<string, string>
			{
				{
					"Authorization",
					result
				}
			});
		}

		private Dictionary<string, object> PrepareUploadScore(string musicUid, int musicDifficulty, string characterUid, string elfinUid, int hp, int score, float acc, int combo, string evaluate, int miss, JArray beats, string bmsVersion)
		{
			List<Beat> list = new List<Beat>();
			for (int i = 0; i < beats.Count; i++)
			{
				JToken jToken = beats[i];
				Beat beat = new Beat();
				beat.note_type = (string)jToken["note_type"];
				beat.offset = (int)jToken["offset"];
				beat.score = (int)jToken["score"];
				beat.side = ((!((string)jToken["side"] == "top")) ? Side.Bottom : Side.Top);
				Beat item = beat;
				list.Add(item);
			}
			UploadHighScore uploadHighScore = new UploadHighScore();
			uploadHighScore.bms_version = bmsVersion;
			uploadHighScore.music_uid = musicUid;
			uploadHighScore.music_difficulty = musicDifficulty;
			uploadHighScore.character_uid = characterUid;
			uploadHighScore.elfin_uid = elfinUid;
			uploadHighScore.combo = combo;
			uploadHighScore.hp = hp;
			uploadHighScore.score = score;
			uploadHighScore.acc = acc;
			uploadHighScore.miss = miss;
			uploadHighScore.judge = evaluate;
			uploadHighScore.beats = list;
			uploadHighScore.devices_id = GetDeviceID();
			uploadHighScore.steam_id = string.Empty;
			UploadHighScore uploadHighScore2 = uploadHighScore;
			uploadHighScore2.steam_id = SteamUser.GetSteamID().m_SteamID.ToString();
			byte[] array;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Serializer.Serialize(memoryStream, uploadHighScore2);
				memoryStream.Position = 0L;
				int num = (int)memoryStream.Length;
				array = new byte[num];
				memoryStream.Read(array, 0, num);
			}
			int[] array2 = new int[3]
			{
				35,
				9010,
				590557986
			};
			int num2 = 0;
			int num3;
			for (num3 = 0; num3 < array.Length; num3++)
			{
				if (num2 == 2 && num3 + 3 >= array.Length)
				{
					num2--;
				}
				if (num2 == 1 && num3 + 1 >= array.Length)
				{
					num2--;
				}
				byte[] bytes = BitConverter.GetBytes(array2[num2]);
				List<byte> list2 = bytes.Where((byte o) => o != 0);
				for (int j = 0; j < list2.Count; j++)
				{
					byte b = list2[j];
					array[num3 + j] = Convert.ToByte(array[num3 + j] ^ b);
				}
				num3 += list2.Count - 1;
				num2++;
				if (num2 == 3)
				{
					num2 = 0;
				}
			}
			string text = Convert.ToBase64String(array);
			string s = string.Format("{0}{1}", text, "YLWfwcMjEqSmCysWMynnhBvRKLwCBhIMSHsxrUPMBcbosuzacalmownLoQREMBMPhTTRdRCRrNlCTjsPpYQaUhNfZUOKSrEELbeCjqmcxlGselGWhAwsUgQCNQuINpLP");
			byte[] inArray = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(s));
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("payload", text);
			dictionary.Add("signature", Convert.ToBase64String(inArray));
			return dictionary;
		}

		public void UploadScore(string musicUid, int musicDifficulty, string characterUid, string elfinUid, int hp, int score, float acc, int combo, string evaluate, int miss, JArray beats, string bmsVersion, Action<int> callback)
		{
			string result = Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>();
			if (string.IsNullOrEmpty(result))
			{
				return;
			}
			string text = "musedash/v2/leaderboard/high-score";
			text = "musedash/v2/pcleaderboard/high-score";
			string url = text;
			string method = "POST";
			Dictionary<string, object> datas = PrepareUploadScore(musicUid, musicDifficulty, characterUid, elfinUid, hp, score, acc, combo, evaluate, miss, beats, bmsVersion);
			Action<JObject> callback2 = delegate(JObject jObj)
			{
				JToken value = jObj["order"];
				if (!string.IsNullOrEmpty((string)value))
				{
					callback((int)value);
				}
			};
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{
					"Authorization",
					result
				}
			};
			SendToUrl(url, method, datas, callback2, null, headers);
		}

		public string GetDeviceID()
		{
			string text = string.Empty;
			string empty = string.Empty;
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			NetworkInterface[] array = allNetworkInterfaces;
			foreach (NetworkInterface networkInterface in array)
			{
				Debug.Log(networkInterface.Description);
				if (networkInterface.Description == "en0")
				{
					text = networkInterface.GetPhysicalAddress().ToString();
					break;
				}
				text = networkInterface.GetPhysicalAddress().ToString();
				if (text != string.Empty)
				{
					break;
				}
			}
			UTF8Encoding uTF8Encoding = new UTF8Encoding();
			byte[] bytes = uTF8Encoding.GetBytes(text);
			byte[] array2 = MD5.Create().ComputeHash(bytes);
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < array2.Length; j++)
			{
				stringBuilder.Append(array2[j].ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		public void UploadScoreForShow(string musicUid, int musicDifficulty, string characterUid, string elfinUid, int hp, int score, float acc, int combo, string evaluate, int miss, JArray beats, string bmsVersion, string auth, Action<int> callback)
		{
			string text = "musedash/v2/exhileaderboard/high-score";
			string url = text;
			string method = "POST";
			Dictionary<string, object> datas = PrepareUploadScore(musicUid, musicDifficulty, characterUid, elfinUid, hp, score, acc, combo, evaluate, miss, beats, bmsVersion);
			Action<JObject> callback2 = delegate(JObject jObj)
			{
				JToken value = jObj["order"];
				if (!string.IsNullOrEmpty((string)value))
				{
					callback((int)value);
				}
			};
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{
					"Authorization",
					auth
				}
			};
			SendToUrl(url, method, datas, callback2, null, headers);
		}

		public void GetRanksForShow(string musicUid, int difficulty, Action<JToken, JToken, int> callback, Action<string> failCallback)
		{
			string result = Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>();
			result = ((!string.IsNullOrEmpty(result)) ? result : string.Empty);
			string text = "musedash/v1/exhileaderboard/top";
			string url = text;
			string method = "GET";
			Dictionary<string, object> datas = new Dictionary<string, object>
			{
				{
					"music_uid",
					musicUid
				},
				{
					"music_difficulty",
					difficulty
				},
				{
					"limit",
					9
				},
				{
					"offset",
					0
				}
			};
			Action<JObject> callback2 = delegate(JObject jObj)
			{
				JToken jToken = jObj["result"];
				JArray jArray = new JArray();
				JArray jArray2 = new JArray();
				int num = 0;
				for (int i = 0; i < jToken.Count(); i++)
				{
					JToken jToken2 = jToken[i];
					int num2 = (int)jToken2["play"]["score"];
					if (num2 != num)
					{
						if (jArray2.Count > 1)
						{
							List<int> list = new List<int>();
							for (int j = 0; j < jArray2.Count; j++)
							{
								list.Add(j);
							}
							for (int k = 0; k < jArray2.Count; k++)
							{
								for (int l = 0; l < jArray.Count; l++)
								{
									string a = (string)jArray[l]["user"]["nickname"];
									if (a == (string)jArray2[k]["user"]["nickname"])
									{
										jArray.RemoveAt(l);
										break;
									}
								}
							}
							for (int m = 0; m < jArray2.Count; m++)
							{
								int num3 = list.Random();
								list.Remove(num3);
								jArray.Add(jArray2[num3]);
							}
						}
						jArray2.Clear();
						jArray.Add(jToken2);
					}
					jArray2.Add(jToken2);
					num = num2;
				}
				jToken = jArray;
				JToken jToken3 = jObj["rank"];
				int arg = (int)jObj["code"];
				callback(jToken, (jToken3 != null && !jToken3.IsNullOrEmpty()) ? jToken3 : ((JToken)(-1)), arg);
			};
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{
					"Authorization",
					result
				}
			};
			SendToUrl(url, method, datas, callback2, failCallback, headers);
		}

		public void GetRanks(string musicUid, int difficulty, Action<JToken, JToken, int> callback, Action<string> failCallback)
		{
			string result = Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>();
			result = ((!string.IsNullOrEmpty(result)) ? result : string.Empty);
			string text = "musedash/v1/leaderboard/top";
			text = "musedash/v1/pcleaderboard/top";
			string url = text;
			string method = "GET";
			Dictionary<string, object> datas = new Dictionary<string, object>
			{
				{
					"music_uid",
					musicUid
				},
				{
					"music_difficulty",
					difficulty
				},
				{
					"limit",
					99
				},
				{
					"offset",
					0
				}
			};
			Action<JObject> callback2 = delegate(JObject jObj)
			{
				JToken arg = jObj["result"];
				JToken jToken = jObj["rank"];
				int arg2 = (int)jObj["code"];
				callback(arg, (jToken != null && !jToken.IsNullOrEmpty()) ? jToken : ((JToken)(-1)), arg2);
			};
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{
					"Authorization",
					result
				}
			};
			SendToUrl(url, method, datas, callback2, failCallback, headers);
		}

		public void SetPlayerName(string name, Action<int> callback, Action<string> failCallback)
		{
			string result = Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>();
			if (string.IsNullOrEmpty(result))
			{
				SendToUrl("exhibition-user/login", "POST", new Dictionary<string, object>
				{
					{
						"nickname",
						name
					}
				}, delegate(JObject jObj)
				{
					int num = (int)jObj["code"];
					if (num == 0)
					{
						Singleton<DataManager>.instance["GameConfig"]["Auth"].SetResult((string)jObj["auth"]);
					}
					callback(num);
				}, failCallback);
				return;
			}
			string url = "user/profile";
			string method = "POST";
			Dictionary<string, object> datas = new Dictionary<string, object>
			{
				{
					"nickname",
					name
				}
			};
			Action<JObject> callback2 = delegate(JObject jObj)
			{
				int obj = (int)jObj["code"];
				callback(obj);
			};
			Dictionary<string, string> headers = new Dictionary<string, string>
			{
				{
					"Authorization",
					result
				}
			};
			SendToUrl(url, method, datas, callback2, failCallback, headers);
		}
	}
}
