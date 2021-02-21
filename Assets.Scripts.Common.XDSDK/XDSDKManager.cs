using Assets.Scripts.GameCore;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.UI;
using LeanCloud;
using Newtonsoft.Json.Linq;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using xdsdk;

namespace Assets.Scripts.Common.XDSDK
{
	public class XDSDKManager : Singleton<XDSDKManager>
	{
		public class XDSDKHandler : XDCallback
		{
			public static bool isInited;

			public static event Action onInitSucceed;

			public static event Action<string> onInitFailed;

			public static event Action<string> onLoginSucceed;

			public static event Action<string> onLoginFailed;

			public static event Action onLogout;

			public static event Action onLoginCancell;

			public static event Action onPayCompleted;

			public static event Action<string> onPayFailed;

			public static event Action onPayCanceled;

			public static event Action onRealNameSucceed;

			public static event Action onRealNameFailed;

			public override void OnInitSucceed()
			{
				isInited = true;
				Singleton<XDSDKManager>.instance.isInited = true;
				XDSDKHandler.onInitSucceed();
			}

			public override void OnInitFailed(string msg)
			{
				Singleton<XDSDKManager>.instance.isInited = false;
				XDSDKHandler.onInitFailed(msg);
			}

			public override void OnLoginSucceed(string token)
			{
				XDSDKHandler.onLoginSucceed(token);
			}

			public override void OnLoginFailed(string msg)
			{
				XDSDKHandler.onLoginFailed(msg);
			}

			public override void OnLoginCanceled()
			{
				XDSDKHandler.onLoginCancell();
			}

			public override void OnGuestBindSucceed(string token)
			{
			}

			public override void OnLogoutSucceed()
			{
				XDSDKHandler.onLogout();
			}

			public override void OnPayCompleted()
			{
				XDSDKHandler.onPayCompleted();
			}

			public override void OnPayFailed(string msg)
			{
				XDSDKHandler.onPayFailed(msg);
			}

			public override void OnPayCanceled()
			{
				XDSDKHandler.onPayCanceled();
			}

			public override void OnExitConfirm()
			{
			}

			public override void OnExitCancel()
			{
			}

			public override void OnWXShareSucceed()
			{
			}

			public override void OnRealNameSucceed()
			{
				XDSDKHandler.onRealNameSucceed();
			}

			public override void OnRealNameFailed(string msg)
			{
				XDSDKHandler.onRealNameFailed();
			}

			static XDSDKHandler()
			{
				XDSDKHandler.onInitSucceed = delegate
				{
				};
				XDSDKHandler.onInitFailed = delegate
				{
				};
				XDSDKHandler.onLoginSucceed = delegate
				{
				};
				XDSDKHandler.onLoginFailed = delegate
				{
				};
				XDSDKHandler.onLogout = delegate
				{
				};
				XDSDKHandler.onLoginCancell = delegate
				{
				};
				XDSDKHandler.onPayCompleted = delegate
				{
				};
				XDSDKHandler.onPayFailed = delegate
				{
				};
				XDSDKHandler.onPayCanceled = delegate
				{
				};
				XDSDKHandler.onRealNameSucceed = delegate
				{
				};
				XDSDKHandler.onRealNameFailed = delegate
				{
				};
			}
		}

		private static AndroidJavaObject m_AndroidJavaObject;

		private bool m_Overseas;

		private bool m_IsAndroid;

		private bool m_IsIOS;

		private bool m_IsTapTap;

		private bool m_IsSync;

		private GameObject m_XDSDKOSContainer;

		public bool showTip;

		public bool isInited;

		private Coroutine m_Coroutine;

		public bool isLogined
		{
			get;
			private set;
		}

		public bool regionCheck
		{
			get;
			private set;
		}

		public string accessToken
		{
			get;
			private set;
		}

		public bool isOvearSea => m_Overseas;

		public event Action onSyncDone = delegate
		{
		};

		private void Init()
		{
			regionCheck = false;
			regionCheck = true;
			m_Overseas = false;
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				if (m_Overseas)
				{
					XDSDKHandler.isInited = true;
				}
				InitSDK();
			}, () => regionCheck);
			accessToken = string.Empty;
		}

		public void SetOversea(bool isOversea)
		{
			m_Overseas = isOversea;
		}

		public void InitSDK()
		{
			if (isInited)
			{
				return;
			}
			isInited = true;
			string[] loginEntries = new string[3]
			{
				"TAPTAP_LOGIN",
				"WX_LOGIN",
				"QQ_LOGIN"
			};
			GameObject gameObject = new GameObject("XDSDKHandler");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			if (!m_Overseas)
			{
				gameObject.AddComponent<XDSDKListener>();
				xdsdk.XDSDK.SetCallback(new XDSDKHandler());
				xdsdk.XDSDK.SetLoginEntries(loginEntries);
				XDSDKHandler.onLoginCancell += delegate
				{
					Singleton<EventManager>.instance.Invoke("UI/OnLoginEnd");
				};
				XDSDKHandler.onLoginFailed += delegate
				{
					Singleton<EventManager>.instance.Invoke("UI/OnLoginEnd");
				};
				XDSDKHandler.onLoginSucceed += delegate
				{
					Singleton<EventManager>.instance.Invoke("UI/OnLoginEnd");
				};
				string appid = (!m_IsIOS) ? "57kt9skpujwokgg" : "bom6by2myfsc4kg";
				if (false)
				{
				}
				string channel = "Others";
				xdsdk.XDSDK.InitSDK(appid, 0, channel, Application.version, true);
				return;
			}
			string text = "161513";
			string text2 = "5a018829d087d55d5d28c5fe373cb0ea";
			SystemLanguage systemLanguage = Application.systemLanguage;
			string text3 = "en";
			switch (systemLanguage)
			{
			case SystemLanguage.Chinese:
				text3 = "cn";
				break;
			case SystemLanguage.ChineseTraditional:
				text3 = "tw";
				break;
			case SystemLanguage.ChineseSimplified:
				text3 = "cn";
				break;
			case SystemLanguage.English:
				text3 = "en";
				break;
			case SystemLanguage.French:
				text3 = "fr";
				break;
			case SystemLanguage.German:
				text3 = "de";
				break;
			case SystemLanguage.Japanese:
				text3 = "jp";
				break;
			case SystemLanguage.Korean:
				text3 = "kr";
				break;
			case SystemLanguage.Portuguese:
				text3 = "pt";
				break;
			case SystemLanguage.Russian:
				text3 = "ru";
				break;
			case SystemLanguage.Spanish:
				text3 = "es";
				break;
			case SystemLanguage.Thai:
				text3 = "th";
				break;
			case SystemLanguage.Turkish:
				text3 = "tr";
				break;
			case SystemLanguage.Vietnamese:
				text3 = "vi";
				break;
			}
			gameObject.AddComponent<Assets.Scripts.Common.XDSDK.XDSDKHandler>();
			XDSDKHandler.isInited = true;
			if (m_IsAndroid)
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				m_AndroidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				m_AndroidJavaObject.Call("InitSDK", text, text2, "android_md", text3);
			}
		}

		public bool IsXDLoggedIn()
		{
			return xdsdk.XDSDK.IsLoggedIn();
		}

		public bool IsLoggedIn()
		{
			return !string.IsNullOrEmpty(Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>());
		}

		public void IsRealName(Action<bool, bool> callback)
		{
			UnityWebRequest webRequest = null;
			string url = "https://api.xd.com/v1/user";
			Dictionary<string, object> datas = new Dictionary<string, object>
			{
				{
					"access_token",
					GetAccessToken()
				}
			};
			webRequest = WebUtils.SendToUrl(url, "GET", datas, delegate(DownloadHandler handler)
			{
				JObject jObject = JsonUtils.Deserialize<JObject>(handler.text);
				if (webRequest.responseCode == 200)
				{
					bool arg = jObject["authoriz_state"].Value<int>() > 0;
					bool arg2 = jObject["fcm"].Value<int>() == 1;
					callback(arg, arg2);
				}
				else
				{
					callback(false, false);
				}
			});
		}

		public void XDLogin()
		{
			InitSDK();
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				if (!IsXDLoggedIn())
				{
					if (Singleton<DataManager>.instance["Account"]["TipTapAccount"].GetResult<bool>() && m_IsTapTap)
					{
						Singleton<EventManager>.instance.Invoke("UI/OnTipTapAccount");
					}
					else
					{
						Singleton<EventManager>.instance.Invoke("UI/OnLoginStart");
						xdsdk.XDSDK.Login();
					}
				}
			}, () => XDSDKHandler.isInited);
		}

		public void WelcomeLogin(bool isSync = true)
		{
			InitSDK();
			if (m_Coroutine != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_Coroutine);
			}
			m_Coroutine = SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				if (!IsLoggedIn() || (!m_Overseas && !xdsdk.XDSDK.IsLoggedIn()))
				{
					if (m_Overseas)
					{
						if (m_IsIOS)
						{
						}
						if (m_IsAndroid)
						{
							m_AndroidJavaObject.Call("SignIn");
						}
					}
					else
					{
						m_IsSync = isSync;
						XDSDKHandler.onLoginSucceed -= OnLoginSuccess;
						XDSDKHandler.onLoginSucceed += OnLoginSuccess;
						xdsdk.XDSDK.Login();
					}
				}
			}, () => XDSDKHandler.isInited);
		}

		public void Login(bool isSync = true)
		{
			InitSDK();
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				if (!IsLoggedIn())
				{
					if (m_Overseas)
					{
						if (m_IsIOS)
						{
						}
						if (m_IsAndroid)
						{
							m_AndroidJavaObject.Call("SignIn");
						}
						Singleton<EventManager>.instance.Invoke("UI/PnlOverseaLoginShow");
					}
					else
					{
						m_IsSync = isSync;
						XDSDKHandler.onLoginSucceed -= OnLoginSuccess;
						XDSDKHandler.onLoginSucceed += OnLoginSuccess;
						XDLogin();
					}
				}
			}, () => XDSDKHandler.isInited);
		}

		private void OnLoginSuccess(string _accessToken)
		{
			if (IsLoggedIn())
			{
				return;
			}
			accessToken = _accessToken;
			WebUtils.SendToUrl($"https://api.xd.com/v1/user?access_token={accessToken}", "GET", null, delegate(DownloadHandler handler)
			{
				JObject jObject = JsonUtils.Deserialize<JObject>(handler.text);
				string id = (string)jObject["id"];
				if (!string.IsNullOrEmpty(id))
				{
					Singleton<ServerManager>.instance.SendToUrl("user/xd-login", "POST", new Dictionary<string, object>
					{
						{
							"access_token",
							accessToken
						}
					}, delegate(JObject jObj)
					{
						OnLoginFinished(jObj, id, m_IsSync);
					});
				}
				else
				{
					Logout();
				}
			}, delegate
			{
				Logout();
			}, null, 15);
		}

		public void Logout()
		{
			if (!IsLoggedIn())
			{
				return;
			}
			Singleton<DataManager>.instance["GameConfig"]["Auth"].SetResult(string.Empty);
			Singleton<DataManager>.instance["GameConfig"]["pero_uid"].SetResult(string.Empty);
			Singleton<DataManager>.instance["Account"]["UserID"].SetResult("0");
			Singleton<DataManager>.instance["Account"]["userUid"].SetResult(string.Empty);
			InitSDK();
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				if (m_Overseas)
				{
					if (m_IsIOS)
					{
					}
					if (m_IsAndroid)
					{
						m_AndroidJavaObject.Call("SignOut");
					}
				}
				else
				{
					xdsdk.XDSDK.Logout();
				}
				if (m_IsTapTap)
				{
					Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"].SetResult("music_package_0");
					BtnIAP.products.For(delegate(Product p)
					{
						if (m_IsTapTap)
						{
							Singleton<DataManager>.instance["IAP"][p.id].SetResult(false);
							for (int i = 0; i <= BtnIAP.planMaxCount; i++)
							{
								if (i != 6)
								{
									Singleton<DataManager>.instance["IAP"][$"unlockall_{i}"].SetResult(false);
								}
							}
						}
					});
					Singleton<DataManager>.instance.Save();
				}
				Singleton<DataManager>.instance.Save();
				if (!m_IsTapTap)
				{
					Singleton<ServerManager>.instance.Synchronize();
				}
				Singleton<EventManager>.instance.Invoke("Net/OnLogout");
			}, () => XDSDKHandler.isInited);
		}

		public void Pay(Dictionary<string, string> info)
		{
			xdsdk.XDSDK.Pay(info);
		}

		public void OpenRealName(Action failCallback, Action succeedCallback)
		{
			Action failAction = null;
			Action succeedAction = null;
			failAction = delegate
			{
				failCallback();
				XDSDKHandler.onRealNameSucceed -= succeedAction;
				XDSDKHandler.onRealNameFailed -= failAction;
			};
			succeedAction = delegate
			{
				succeedCallback();
				XDSDKHandler.onRealNameSucceed -= succeedAction;
				XDSDKHandler.onRealNameFailed -= failAction;
			};
			XDSDKHandler.onRealNameFailed -= failAction;
			XDSDKHandler.onRealNameFailed += failAction;
			XDSDKHandler.onRealNameSucceed -= succeedAction;
			XDSDKHandler.onRealNameSucceed += succeedAction;
			xdsdk.XDSDK.OpenRealName();
		}

		public string GetAccessToken()
		{
			string accessToken = xdsdk.XDSDK.GetAccessToken();
			if (string.IsNullOrEmpty(accessToken))
			{
				accessToken = this.accessToken;
			}
			return accessToken;
		}

		public void UserFeedback()
		{
			xdsdk.XDSDK.UserFeedback();
		}

		public void OnOSLoginSuccess(string sid)
		{
			string url = "http://p.txwy.tw/passport/auth";
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("sid", sid);
			dictionary = dictionary;
			WebUtils.SendToUrl(url, "GET", dictionary, delegate(DownloadHandler handler)
			{
				JObject jObject = JsonUtils.Deserialize<JObject>(handler.text);
				string userId = (string)jObject["uid"];
				Singleton<ServerManager>.instance.SendToUrl("user/txwy-login", "POST", new Dictionary<string, object>
				{
					{
						"sid",
						sid
					}
				}, delegate(JObject jObj)
				{
					OnLoginFinished(jObj, userId);
				}, delegate
				{
					Logout();
				});
			}, delegate
			{
				Logout();
			});
		}

		public void OnGoogleLoginFail()
		{
			Singleton<EventManager>.instance.Invoke("Net/OnGoogleLoginFail");
		}

		private void OnLoginFinished(JObject jObj, string userId, bool isSync = true)
		{
			string value = (string)jObj["auth"];
			JToken jToken = jObj["profile"];
			bool flag = (bool)jObj["fresh"];
			string nickName = string.Empty;
			string empty = string.Empty;
			if (jToken != null)
			{
				if (m_Overseas)
				{
					if (flag)
					{
						Singleton<EventManager>.instance.Invoke("UI/OnPlayerNameRequestNoCancel");
					}
					else
					{
						nickName = (string)jToken["nickname"];
						Singleton<DataManager>.instance["Account"]["PlayerName"].SetResult(nickName);
					}
				}
				else
				{
					nickName = (string)jToken["nickname"];
					Singleton<DataManager>.instance["Account"]["PlayerName"].SetResult(nickName);
				}
				empty = (string)jToken["user_id"];
				Singleton<DataManager>.instance["Account"]["userUid"].SetResult(empty);
			}
			JToken jToken2 = jObj["uid"];
			if (jToken2 != null)
			{
				string value2 = jToken2.Value<string>();
				Singleton<DataManager>.instance["GameConfig"]["pero_uid"].SetResult(value2);
			}
			Singleton<DataManager>.instance["GameConfig"]["Auth"].SetResult(value);
			Singleton<DataManager>.instance["Account"]["UserID"].SetResult(userId);
			Singleton<DataManager>.instance.Save();
			Singleton<EventManager>.instance.Invoke("Net/OnLoginSucceed");
			if (isSync)
			{
				Singleton<ServerManager>.instance.Synchronize(delegate
				{
					if (!string.IsNullOrEmpty(nickName))
					{
						Singleton<DataManager>.instance["Account"]["UserID"].SetResult(userId);
						Singleton<DataManager>.instance["Account"]["PlayerName"].SetResult(nickName);
					}
					Singleton<DataManager>.instance.Save();
				});
			}
			isLogined = true;
		}

		public void Synchronize(Action callback = null)
		{
			string result = Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>();
			if (string.IsNullOrEmpty(result))
			{
				Debug.Log("Old Sync");
				OldSynchronize();
			}
			else
			{
				NewSynchronize(callback);
			}
		}

		private void NewSynchronize(Action callback = null)
		{
			Singleton<EventManager>.instance.Invoke("Net/OnConnecting");
			string auth = Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>();
			Singleton<ServerManager>.instance.SendToUrl("musedash/v2/save", headers: new Dictionary<string, string>
			{
				{
					"Authorization",
					auth
				}
			}, method: "GET", datas: null, callback: delegate(JObject jObj)
			{
				if ((int)jObj["code"] == 90001)
				{
					Singleton<EventManager>.instance.Invoke("Net/OnConnectFail");
				}
				else
				{
					Dictionary<string, IData> datas = Singleton<DataManager>.instance.datas;
					JToken jsonDatas = jObj["save"];
					bool flag = string.IsNullOrEmpty(jsonDatas["Account"].ToString());
					if (flag || callback == null)
					{
						if (flag)
						{
							Singleton<DataManager>.instance["Account"]["LoadingFirstLogin"].SetResult(true);
						}
						OnSaveSelectCallback(true, datas, auth, jsonDatas, callback);
					}
					else
					{
						foreach (KeyValuePair<string, IData> item in datas)
						{
							SingletonDataObject singletonDataObject = item.Value as SingletonDataObject;
							if ((bool)singletonDataObject && singletonDataObject.isSync)
							{
								string inData = jsonDatas[item.Key].Value<string>();
								string outData;
								Singleton<DataUpgrader>.instance.Upgrade(inData, out outData);
								jsonDatas[item.Key] = outData;
							}
						}
						Dictionary<string, IVariable> dictionary = SingletonDataObject.JsonToField((string)jsonDatas["Account"]);
						int result = dictionary["Exp"].GetResult<int>();
						int result2 = Singleton<DataManager>.instance["Account"]["Exp"].GetResult<int>();
						Singleton<EventManager>.instance.Invoke("Net/OnConnectSucceed");
						string n = "PnlCloud";
						SingletonMonoBehaviour<SteamGoogleLogin>.instance.CheckIsGoogleLogin();
						Singleton<EventManager>.instance.Invoke("Net/OnRemoteOverride");
						CloudDataChangeHandler component = Singleton<UIManager>.instance[n].GetComponent<CloudDataChangeHandler>();
						string result3 = dictionary["LastSaveTime"].GetResult<string>();
						component.Init(result, result3, result2, delegate(bool isLocal)
						{
							OnSaveSelectCallback(isLocal, datas, auth, jsonDatas, callback);
							Singleton<ServerManager>.instance.CheckBan();
						});
					}
				}
			}, faillCallback: delegate
			{
				Singleton<EventManager>.instance.Invoke("Net/OnConnectFail");
			});
		}

		private void OnSaveSelectCallback(bool isLocal, Dictionary<string, IData> datas, string auth, JToken jsonDatas, Action callback = null)
		{
			if (isLocal)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				foreach (KeyValuePair<string, IData> data in datas)
				{
					SingletonDataObject singletonDataObject = data.Value as SingletonDataObject;
					if ((bool)singletonDataObject && singletonDataObject.isSync)
					{
						dictionary.Add(data.Key, singletonDataObject.ToJson());
					}
				}
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				dictionary2.Add("save", dictionary);
				Dictionary<string, object> dictionary3 = dictionary2;
				auth = ((!string.IsNullOrEmpty(auth)) ? auth : string.Empty);
				Singleton<ServerManager>.instance.SendToUrl("musedash/v2/save", "PUT", dictionary3, headers: new Dictionary<string, string>
				{
					{
						"Authorization",
						auth
					}
				}, callback: delegate
				{
					Singleton<EventManager>.instance.Invoke("Net/OnConnectSucceed");
					if (callback != null)
					{
						callback();
					}
					this.onSyncDone();
				}, faillCallback: delegate
				{
					Singleton<EventManager>.instance.Invoke("Net/OnConnectFail");
				});
				return;
			}
			foreach (KeyValuePair<string, IData> data2 in datas)
			{
				SingletonDataObject singletonDataObject2 = data2.Value as SingletonDataObject;
				if ((bool)singletonDataObject2 && singletonDataObject2.isSync)
				{
					singletonDataObject2.Reset();
					singletonDataObject2.Reload();
					singletonDataObject2.LoadFromJson((string)jsonDatas[data2.Key]);
				}
			}
			RefreshDatas();
		}

		private void OldSynchronize()
		{
			string userID = Singleton<DataManager>.instance["Account"]["UserID"].GetResult<string>();
			Singleton<EventManager>.instance.Invoke("Net/OnConnecting");
			Debug.Log("==============Snync Start!==============");
			Debug.Log($"=============='{userID}' All Data is Syncing...==============");
			bool isOverride = false;
			bool syncAccountDone = false;
			bool isRegistered = true;
			int index = 0;
			int totalCount = 0;
			bool isFail = false;
			Coroutine coroutine = SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				Singleton<EventManager>.instance.Invoke("Net/OnConnectFail");
			}, () => isFail);
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				if (index != totalCount)
				{
					isFail = true;
				}
				else
				{
					SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(coroutine);
				}
			}, 30f);
			Dictionary<string, IData> datas = new Dictionary<string, IData>(Singleton<DataManager>.instance.datas);
			SingletonDataObject account = Singleton<DataManager>.instance["Account"];
			AVQuery<AVObject> aVQuery = new AVQuery<AVObject>("Account").WhereEqualTo("userID", userID).Limit(1);
			Action callback = delegate
			{
				syncAccountDone = true;
				Debug.Log("=============='Account' Synced!==============");
			};
			aVQuery.FindAsync().ContinueWith(delegate(Task<IEnumerable<AVObject>> task)
			{
				if (task.IsFaulted || task.IsCanceled)
				{
					isFail = true;
				}
				else if (task.Result.Any())
				{
					IEnumerable<AVObject> result2 = task.Result;
					AVObject aVObject = result2.First();
					byte[] bytes2 = JsonUtils.Deserialize<byte[]>(aVObject["data"].ToString());
					Dictionary<string, IVariable> dictionary = SerializationUtility.DeserializeValue<Dictionary<string, IVariable>>(bytes2, DataFormat.Binary);
					if (dictionary["Exp"].GetResult<int>() > account["Exp"].GetResult<int>())
					{
						isOverride = true;
					}
					if (isOverride)
					{
						datas.Remove("Account");
						account.LoadFromBytes(bytes2);
					}
					callback();
				}
				else
				{
					isRegistered = false;
					syncAccountDone = true;
				}
			});
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				if (!isFail)
				{
					string name = default(string);
					SingletonDataObject singletonDataObject = default(SingletonDataObject);
					byte[] bytes = default(byte[]);
					AVObject avObject = default(AVObject);
					Action callback2 = default(Action);
					foreach (KeyValuePair<string, IData> item in datas)
					{
						name = item.Key;
						singletonDataObject = (item.Value as SingletonDataObject);
						if ((bool)singletonDataObject && singletonDataObject.isSync)
						{
							totalCount++;
							bytes = singletonDataObject.ToBytes();
							avObject = null;
							if (!isRegistered)
							{
								avObject = new AVObject(name);
								avObject["userID"] = userID;
								avObject["data"] = JsonUtils.Serialize(bytes);
								avObject.SaveAsync().ContinueWith(delegate(Task t)
								{
									if (t.IsCanceled || t.IsFaulted)
									{
										isFail = true;
									}
									else
									{
										Debug.Log($"=============='{name}' Created!==============");
										if (++index == totalCount && !isFail)
										{
											Singleton<EventManager>.instance.Invoke("Net/OnConnectSucceed");
											Debug.Log($"=============='{userID}' All Data is Created!==============");
										}
									}
								});
							}
							else
							{
								AVQuery<AVObject> aVQuery2 = new AVQuery<AVObject>(name).WhereEqualTo("userID", userID).Limit(1);
								aVQuery2.FindAsync().ContinueWith(delegate(Task<IEnumerable<AVObject>> task)
								{
									if (task.IsCanceled || task.IsFaulted)
									{
										isFail = true;
									}
									else
									{
										IEnumerable<AVObject> result = task.Result;
										avObject = result.First();
									}
								});
								callback2 = delegate
								{
									Debug.Log($"=============='{name}' Synced!==============");
									if (++index == totalCount && !isFail)
									{
										RefreshDatas();
										Singleton<EventManager>.instance.Invoke("Net/OnConnectSucceed");
										if (isOverride)
										{
											Singleton<EventManager>.instance.Invoke("Net/OnRemoteOverride");
										}
										Debug.Log($"=============='{userID}' All Data is Synced!==============");
									}
								};
								SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
								{
									if (!isFail)
									{
										if (isOverride)
										{
											singletonDataObject.LoadFromBytes(JsonUtils.Deserialize<byte[]>(avObject["data"].ToString()));
											callback2();
										}
										else
										{
											avObject["data"] = JsonUtils.Serialize(bytes);
											avObject.SaveAsync().ContinueWith(delegate(Task t)
											{
												if (t.IsCanceled || t.IsFaulted)
												{
													isFail = true;
												}
												else
												{
													callback2();
												}
											});
										}
									}
								}, () => avObject != null || isFail);
							}
						}
					}
				}
			}, () => syncAccountDone || isFail);
		}

		public void RefreshDatas()
		{
			Singleton<DataManager>.instance.Save();
			Singleton<ItemManager>.instance.Reset();
			Singleton<StageManager>.instance.Reset();
			Singleton<TaskManager>.instance.Reset();
			Singleton<AchievementManager>.instance.Reset();
			SingletonMonoBehaviour<MessageManager>.instance.Init();
			TaskManager instance = Singleton<TaskManager>.instance;
		}
	}
}
