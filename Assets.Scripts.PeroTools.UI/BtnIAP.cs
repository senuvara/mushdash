using Assets.Scripts.Common.XDSDK;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.PreWarm;
using Assets.Scripts.UI.Controls;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.UI
{
	public class BtnIAP : SerializedMonoBehaviour, IPreWarm
	{
		public IAPType type;

		[ShowIf("type", IAPType.Purchase, true)]
		public string id;

		[ShowIf("type", IAPType.Purchase, true)]
		public List<string> prewarmId;

		public OnPurchaseRestoreCompleted onPurchaseRestoreCompleted;

		public OnPurchaseRestoreFailed onPurchaseRestoreFailed;

		public OnRestoreCompleted onRestoreCompleted;

		public OnHasPurchased onHasPurchased;

		public UnityEvent onUnlockRestoreSucceed;

		public GameObject mask;

		private bool m_NoResult;

		private int m_RestoreCount;

		private bool m_IsRestoring;

		private static bool m_IsRealName;

		public static List<Product> products;

		private static List<string> m_Skus;

		private static Dictionary<string, GooglePurchase> m_States = new Dictionary<string, GooglePurchase>();

		private static List<string> m_Ids = new List<string>();

		private bool m_IsReactable;

		private bool m_IsResoreOne;

		private bool m_IsGetFree;

		private bool m_HasShowText;

		private bool m_RestoreEnterTransaction = true;

		private static bool m_IsApple;

		private static bool m_IsAppleInited;

		private static bool m_IsGoogle;

		private static bool m_IsGoogleInited;

		private static bool m_IsGoogleStoreLoaded;

		private static bool m_IsTapTap;

		private static bool m_IsEditor;

		private static List<string> m_AlbumIds = new List<string>();

		private static List<string> m_UnlockAllIDs = new List<string>();

		public static int planMaxCount => 9;

		public static bool isAvaiable
		{
			get
			{
				if (m_IsApple)
				{
					return SingletonMonoBehaviour<IAPManager>.instance.isPurchaseAvaible;
				}
				return true;
			}
		}

		private void InitStoreKit()
		{
			if (m_IsApple && !m_IsAppleInited)
			{
				m_IsAppleInited = true;
				Action<Product[]> callback = null;
				callback = delegate(Product[] ps)
				{
					if (!this)
					{
						SingletonMonoBehaviour<IAPManager>.instance.onInitSucceed -= callback;
					}
					else
					{
						Product p2 = default(Product);
						for (int m = 0; m < ps.Length; m++)
						{
							p2 = ps[m];
							int num = products.FindIndex((Product pd) => pd.id == p2.id);
							if (num != -1)
							{
								products[num] = p2;
							}
						}
						RefreshRemotePurchasedList();
					}
				};
				SingletonMonoBehaviour<IAPManager>.instance.onInitSucceed += callback;
				SingletonMonoBehaviour<IAPManager>.instance.InitIAP(m_Ids.ToArray());
				Action<string[]> callback2 = null;
				callback2 = delegate(string[] ps)
				{
					if (!this)
					{
						SingletonMonoBehaviour<IAPManager>.instance.onReceiptCompleted -= callback2;
					}
					else
					{
						Debug.Log($"[BtnIAP]: products active {JsonUtils.Serialize(ps)}");
						foreach (string uid in ps)
						{
							Singleton<DataManager>.instance["IAP"][uid].SetResult(true);
						}
						Singleton<DataManager>.instance.Save();
					}
				};
				SingletonMonoBehaviour<IAPManager>.instance.onReceiptCompleted += callback2;
			}
			if (m_IsGoogle && !m_IsGoogleInited)
			{
				m_IsGoogleInited = true;
				Debug.Log("Google Play iab init start!");
				Action initSucceeded = null;
				Action<string> initFailed = null;
				Action<List<GooglePurchase>, List<GoogleSkuInfo>> getProductions = null;
				initSucceeded = delegate
				{
					GoogleIABManager.billingSupportedEvent -= initSucceeded;
					GoogleIABManager.billingNotSupportedEvent -= initFailed;
					m_IsGoogleStoreLoaded = true;
					Debug.Log("Google Play iab init succeeded!");
					m_Skus = new List<string>();
					int count = Singleton<ConfigManager>.instance["albums"].Count;
					for (int i = 1; i < count; i++)
					{
						string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", i, "uid");
						if (configStringValue.StartsWith("music_package_") && !Singleton<ConfigManager>.instance.GetConfigBoolValue("albums", i, "free"))
						{
							m_Skus.Add(configStringValue);
						}
					}
					for (int j = 0; j <= planMaxCount; j++)
					{
						if (j != 6)
						{
							m_Skus.Add($"unlockall_{j}");
						}
					}
					GoogleIABManager.queryInventorySucceededEvent += getProductions;
					GoogleIAB.queryInventory(m_Skus.ToArray());
					RefreshRemotePurchasedList();
				};
				initFailed = delegate(string e)
				{
					GoogleIABManager.billingSupportedEvent -= initSucceeded;
					GoogleIABManager.billingNotSupportedEvent -= initFailed;
					Debug.Log($"Google Play iab init fail: {e}");
					m_IsGoogleInited = false;
				};
				getProductions = delegate(List<GooglePurchase> list, List<GoogleSkuInfo> infos)
				{
					GoogleIABManager.queryInventorySucceededEvent -= getProductions;
					Debug.Log($"Google Play iab get productions succeeded, count: {infos.Count}");
					infos.For(delegate(GoogleSkuInfo info)
					{
						Product product = products.Find((Product p) => p.id == info.productId);
						if (product != null)
						{
							product.title = info.title;
							product.isAvailable = true;
							product.localizedPrice = info.price;
							product.price = 1f;
						}
						GooglePurchase googlePurchase = list.Find((GooglePurchase l) => l.productId == info.productId);
						if (googlePurchase != null)
						{
							m_States[googlePurchase.productId] = googlePurchase;
						}
					});
				};
				GoogleIABManager.billingSupportedEvent += initSucceeded;
				GoogleIABManager.billingNotSupportedEvent += initFailed;
				GoogleIAB.init("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgolJv2qTJiaN+KJ/+/MohBd2Dh7JZgsGB7uWBwBZv4O4E74CLbqcNef110uA08YLvhH8ThEro6izCFjrYgOtra6dooUqaz6XVlR3jne6kz1imAFvo3PB58Q3Rc/QITqWjTrveKxVqn8hVPTLSiPEUbt8/mrsmGguPiQlDaVXlltihxORwk6YvKfu05+TYZfTHfBQoiqBVUntsHJpJCJ/BIEctXghugTTjN7CrtROmilOtifKfDqePnWm2MIFQrsecmJ14DYA4HOlfRjTY+I0oW/8+BePH/HZawXCLfJJTVtMgDSteKYq2qDO79SazJTZhqfBmb7xNFqmgILF5gQKJwIDAQAB");
			}
			if (m_IsTapTap)
			{
				RefreshRemotePurchasedList();
			}
		}

		private void RequirePrice(Action callback = null)
		{
			if (m_IsApple)
			{
				Action<Product[]> initSucceed = null;
				initSucceed = delegate(Product[] ps)
				{
					if (!this)
					{
						SingletonMonoBehaviour<IAPManager>.instance.onInitSucceed -= initSucceed;
					}
					else
					{
						Product p2 = default(Product);
						for (int k = 0; k < ps.Length; k++)
						{
							p2 = ps[k];
							int num2 = products.FindIndex((Product pd) => pd.id == p2.id);
							if (num2 != -1)
							{
								products[num2] = p2;
							}
						}
						if (callback != null)
						{
							callback();
						}
					}
				};
				SingletonMonoBehaviour<IAPManager>.instance.onInitSucceed += initSucceed;
				SingletonMonoBehaviour<IAPManager>.instance.InitIAP(m_Ids.ToArray());
			}
			if (m_IsGoogle)
			{
				Action<List<GooglePurchase>, List<GoogleSkuInfo>> getProductions = null;
				getProductions = delegate(List<GooglePurchase> list, List<GoogleSkuInfo> infos)
				{
					GoogleIABManager.queryInventorySucceededEvent -= getProductions;
					Debug.Log($"Google Play iab get productions succeeded, count: {infos.Count}");
					infos.For(delegate(GoogleSkuInfo info)
					{
						Product product2 = products.Find((Product p) => p.id == info.productId);
						if (product2 != null)
						{
							product2.title = info.title;
							product2.isAvailable = true;
							product2.localizedPrice = info.price;
							product2.price = 1f;
						}
						GooglePurchase googlePurchase = list.Find((GooglePurchase l) => l.productId == info.productId);
						if (googlePurchase != null)
						{
							m_States[googlePurchase.productId] = googlePurchase;
						}
					});
				};
				GoogleIABManager.queryInventorySucceededEvent += getProductions;
				GoogleIAB.queryInventory(m_Skus.ToArray());
			}
			if (!m_IsTapTap)
			{
				return;
			}
			string url = $"{Singleton<ServerManager>.instance.GetHost()}purchase/products";
			WebUtils.SendToUrl(url, "GET", null, delegate(DownloadHandler handler)
			{
				JObject jObject = JsonUtils.Deserialize<JObject>(handler.text);
				JToken jToken = jObject["code"];
				switch ((!jToken.IsNullOrEmpty()) ? ((int)jToken) : 0)
				{
				case 0:
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>();
					JToken jToken2 = jObject["items"];
					for (int i = 0; i < jToken2.Count(); i++)
					{
						JToken jToken3 = jToken2[i];
						dictionary.Add((string)jToken3["product_id"], (int)jToken3["price"]);
					}
					for (int j = 0; j < products.Count; j++)
					{
						Product product = products[j];
						if (dictionary.ContainsKey(product.id))
						{
							int num = dictionary[product.id];
							product.isAvailable = true;
							product.price = (float)num / 100f;
							product.localizedPrice = $"¥{product.price:f2}";
						}
					}
					if (callback != null)
					{
						callback();
					}
					break;
				}
				case 90001:
					Singleton<XDSDKManager>.instance.Logout();
					Singleton<EventManager>.instance.Invoke("UI/OnLoginAgainRequest");
					break;
				}
			});
		}

		public void PreWarm(int slice)
		{
			if (slice != 0)
			{
				return;
			}
			if (products == null)
			{
				products = new List<Product>();
				int count = Singleton<ConfigManager>.instance["albums"].Count;
				for (int j = 1; j < count; j++)
				{
					Product product = new Product();
					product.id = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", j, "uid");
					product.title = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", j, "title");
					product.localizedPrice = Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "purchaseInavaiable");
					product.price = -1f;
					Product product2 = product;
					if (m_IsEditor)
					{
						product2.localizedPrice = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", j, "price");
					}
					products.Add(product2);
				}
				for (int k = 1; k <= planMaxCount; k++)
				{
					if (k != 6)
					{
						Product product = new Product();
						product.id = $"unlockall_{k}";
						product.price = -1f;
						Product item = product;
						products.Add(item);
					}
				}
			}
			if (!prewarmId.Contains(id))
			{
				prewarmId.Add(id);
			}
			RefreshLocalPurchasedList();
			if (m_IsApple)
			{
				prewarmId.For(delegate(string i)
				{
					if (!m_Ids.Contains(i))
					{
						m_Ids.Add(i);
					}
				});
				int count2 = Singleton<ConfigManager>.instance["albums"].Count;
				for (int l = 0; l < count2; l++)
				{
					string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", l, "uid");
					if (!m_Ids.Contains(configStringValue))
					{
						m_Ids.Add(configStringValue);
					}
				}
				for (int m = 0; m <= planMaxCount; m++)
				{
					if (m == 6)
					{
						if (m_Ids.Contains("unlockall_6"))
						{
							m_Ids.Remove("unlockall_6");
						}
						continue;
					}
					string item2 = $"unlockall_{m}";
					if (!m_Ids.Contains(item2))
					{
						m_Ids.Add(item2);
					}
				}
				InitStoreKit();
				Action<TransactionResult> puchased2 = null;
				Action restored = null;
				puchased2 = delegate(TransactionResult result)
				{
					if (!this)
					{
						SingletonMonoBehaviour<IAPManager>.instance.onTransactionCompleted -= puchased2;
					}
					else
					{
						OnTransactionComplete(result);
					}
				};
				restored = delegate
				{
					m_IsReactable = false;
					if (!m_RestoreEnterTransaction)
					{
						onPurchaseRestoreCompleted.Invoke(null);
						m_RestoreEnterTransaction = true;
					}
					if (!this)
					{
						SingletonMonoBehaviour<IAPManager>.instance.onRestoreCompleted -= restored;
					}
				};
				SingletonMonoBehaviour<IAPManager>.instance.onTransactionCompleted += puchased2;
				if (type == IAPType.Restore)
				{
					SingletonMonoBehaviour<IAPManager>.instance.onRestoreCompleted += restored;
				}
				else
				{
					XDSDKManager instance = Singleton<XDSDKManager>.instance;
					XDSDKManager.XDSDKHandler.onPayCompleted += delegate
					{
						OnTransactionComplete(new TransactionResult
						{
							productId = id,
							state = TransactionState.Purchased
						});
					};
					XDSDKManager.XDSDKHandler.onPayFailed += delegate
					{
						OnTransactionComplete(new TransactionResult
						{
							productId = id,
							state = TransactionState.Failed
						});
					};
					XDSDKManager.XDSDKHandler.onPayCanceled += delegate
					{
						OnTransactionComplete(new TransactionResult
						{
							productId = id,
							state = TransactionState.Failed
						});
					};
				}
			}
			if (m_IsGoogle)
			{
				InitStoreKit();
				Action<GooglePurchase> puchased = null;
				puchased = delegate(GooglePurchase p)
				{
					if (!this)
					{
						GoogleIABManager.purchaseSucceededEvent -= puchased;
					}
					else
					{
						m_States[p.productId] = p;
						OnTransactionComplete(new TransactionResult
						{
							productId = p.productId,
							state = TransactionState.Purchased
						});
					}
				};
				GoogleIABManager.purchaseSucceededEvent += puchased;
				if (type == IAPType.Purchase)
				{
					Action<string, int> failed = null;
					failed = delegate
					{
						if (!this)
						{
							GoogleIABManager.purchaseFailedEvent -= failed;
						}
						else
						{
							OnTransactionComplete(new TransactionResult
							{
								productId = id,
								state = TransactionState.Failed
							});
						}
					};
					GoogleIABManager.purchaseFailedEvent += failed;
				}
			}
			if (m_IsTapTap)
			{
				if (!Singleton<XDSDKManager>.instance.IsLoggedIn())
				{
					List<string> list = new List<string>();
					JArray json = Singleton<ConfigManager>.instance.GetJson("albums", false);
					for (int n = 0; n < json.Count; n++)
					{
						JToken jToken = json[n];
						bool flag = (bool)jToken["free"];
						string text = (string)jToken["uid"];
						if (!flag && text != "unlockall_0")
						{
							list.Add(text);
						}
					}
					for (int num = 0; num <= planMaxCount; num++)
					{
						if (num != 6)
						{
							list.Add($"unlockall_{num}");
						}
					}
					for (int num2 = 0; num2 < list.Count; num2++)
					{
						string uid = list[num2];
						Singleton<DataManager>.instance["IAP"][uid].SetResult(false);
					}
					Singleton<DataManager>.instance.Save();
				}
				InitStoreKit();
				if (products.Exists((Product p) => !p.isAvailable))
				{
					RequirePrice();
				}
				XDSDKManager instance2 = Singleton<XDSDKManager>.instance;
				if (type == IAPType.Purchase)
				{
					XDSDKManager.XDSDKHandler.onPayCompleted += delegate
					{
						OnTransactionComplete(new TransactionResult
						{
							productId = id,
							state = TransactionState.Purchased
						});
					};
					XDSDKManager.XDSDKHandler.onPayFailed += delegate
					{
						OnTransactionComplete(new TransactionResult
						{
							productId = id,
							state = TransactionState.Failed
						});
					};
					XDSDKManager.XDSDKHandler.onPayCanceled += delegate
					{
						OnTransactionComplete(new TransactionResult
						{
							productId = id,
							state = TransactionState.Failed
						});
					};
				}
			}
			base.gameObject.GetComponent<Button>().onClick.AddListener(delegate
			{
				m_IsReactable = true;
				m_IsResoreOne = false;
				m_IsGetFree = false;
				m_HasShowText = true;
				if (type == IAPType.Purchase && (Singleton<DataManager>.instance["IAP"][id].GetResult<bool>() || IsUnlockAll()))
				{
					onHasPurchased.Invoke();
				}
				else
				{
					if (m_IsApple)
					{
					}
					if (m_IsGoogle)
					{
						if (FreePackageLogic())
						{
							return;
						}
						RequirePrice();
						GoogleLogic();
					}
					if (m_IsTapTap)
					{
						TapTapLogic0();
					}
					if (m_IsEditor && !FreePackageLogic())
					{
						EditorLogic();
					}
				}
			});
			if (m_UnlockAllIDs.Count == 0)
			{
				for (int num3 = 0; num3 <= planMaxCount; num3++)
				{
					m_UnlockAllIDs.Add("unlockall_" + num3);
				}
			}
		}

		private void TapTapLogic0(Action callback = null)
		{
			if (Singleton<XDSDKManager>.instance.IsLoggedIn() && Singleton<XDSDKManager>.instance.IsXDLoggedIn())
			{
				if (callback == null)
				{
					TapTapLogic1();
				}
				else
				{
					callback();
				}
				return;
			}
			Singleton<XDSDKManager>.instance.Logout();
			OnTransactionComplete(new TransactionResult
			{
				productId = id,
				state = TransactionState.Failed
			});
			mask.SetActive(false);
			Singleton<XDSDKManager>.instance.Login();
		}

		private void TapTapLogic1()
		{
			Singleton<XDSDKManager>.instance.onSyncDone -= TapTapLogic1;
			if (FreePackageLogic())
			{
				return;
			}
			if (type == IAPType.Purchase)
			{
				RefreshRemotePurchasedList(delegate
				{
					if (IsUnlockAll())
					{
						onPurchaseRestoreCompleted.Invoke(id);
					}
					else
					{
						RequirePrice(RefreshUidForPurchase);
					}
				});
				return;
			}
			Singleton<ServerManager>.instance.SendToUrl("purchase/check_xd_purchase", headers: new Dictionary<string, string>
			{
				{
					"Authorization",
					Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>()
				}
			}, method: "GET", datas: null, callback: delegate(JObject jObj)
			{
				if ((int)jObj["code"] != 0)
				{
					onPurchaseRestoreFailed.Invoke(id, null);
				}
				else
				{
					JToken jToken = jObj["data"];
					foreach (JToken item in (IEnumerable<JToken>)jToken)
					{
						string text = (string)item["product_id"];
						if (!Singleton<DataManager>.instance["IAP"][text].GetResult<bool>())
						{
							m_IsResoreOne = true;
						}
						if (text.StartsWith("unlockall_"))
						{
							Singleton<ItemManager>.instance.AddExtraItem("loading", 14, 5);
						}
						if (text == "music_package_25" || text.StartsWith("unlockall_"))
						{
							Singleton<ItemManager>.instance.AddExtraItem("character", 14, 8);
							Singleton<ItemManager>.instance.AddExtraItem("loading", 29, 5);
						}
						if (text == "music_package_29" || text.StartsWith("unlockall_"))
						{
							Singleton<ItemManager>.instance.AddExtraItem("character", 15, 8);
							Singleton<ItemManager>.instance.AddExtraItem("loading", 34, 5);
						}
						if (text == "music_package_32" || text.StartsWith("unlockall_"))
						{
							Singleton<ItemManager>.instance.CheckAndAddWelcome(6, 5, false);
						}
						if (text == "music_package_33" || text.StartsWith("unlockall_"))
						{
							Singleton<ItemManager>.instance.AddExtraItem("character", 16, 8);
							Singleton<ItemManager>.instance.CheckAndAddWelcome(8, 5, false);
						}
						Singleton<DataManager>.instance["IAP"][text].SetResult(true);
					}
					onPurchaseRestoreCompleted.Invoke(null);
					if (m_IsResoreOne)
					{
						onRestoreCompleted.Invoke();
					}
					Singleton<DataManager>.instance.Save();
				}
			});
			m_IsReactable = false;
		}

		private void RefreshUidForPurchase()
		{
			Singleton<ServerManager>.instance.SendToUrl("user/auth", headers: new Dictionary<string, string>
			{
				{
					"Authorization",
					Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>()
				}
			}, method: "GET", datas: null, callback: delegate(JObject jObj)
			{
				JToken jToken = jObj["code"];
				if (jToken != null)
				{
					if (jToken.Value<int>() != 0)
					{
						Singleton<XDSDKManager>.instance.Logout();
					}
					else
					{
						JToken jToken2 = jObj["uid"];
						if (jToken2 != null)
						{
							string value = jToken2.Value<string>();
							Singleton<DataManager>.instance["GameConfig"]["pero_uid"].SetResult(value);
							Singleton<DataManager>.instance.Save();
							TapTapLogic2();
						}
						else
						{
							Singleton<XDSDKManager>.instance.Logout();
						}
					}
				}
				else
				{
					Singleton<XDSDKManager>.instance.Logout();
				}
			});
		}

		private void TapTapLogic2()
		{
			id = GetId();
			Product product = products.Find((Product p) => p.id == id);
			string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", "uid", "title", (!id.StartsWith("unlockall_")) ? id : "unlockall_0");
			Dictionary<string, string> info = new Dictionary<string, string>
			{
				{
					"Product_Name",
					configStringValue
				},
				{
					"Product_Id",
					id
				},
				{
					"Product_Price",
					Mathf.RoundToInt(product.price * 100f).ToString()
				},
				{
					"Sid",
					"666"
				},
				{
					"Role_Id",
					"666"
				},
				{
					"OrderId",
					"666"
				},
				{
					"EXT",
					"_" + Singleton<DataManager>.instance["GameConfig"]["pero_uid"].GetResult<string>()
				}
			};
			if (product.price >= 0f)
			{
				Singleton<ServerManager>.instance.SendToUrl("purchase/check_xd_purchase", headers: new Dictionary<string, string>
				{
					{
						"Authorization",
						Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>()
					}
				}, method: "GET", datas: null, callback: delegate(JObject jObj)
				{
					if ((int)jObj["code"] != 0)
					{
						onPurchaseRestoreFailed.Invoke(id, null);
					}
					else
					{
						JToken jToken = jObj["data"];
						bool flag = false;
						foreach (JToken item in (IEnumerable<JToken>)jToken)
						{
							string text = (string)item["product_id"];
							if (text == id)
							{
								if (text.StartsWith("unlockall_"))
								{
									Singleton<ItemManager>.instance.AddExtraItem("loading", 14, 5);
								}
								if (text == "music_package_25" || text.StartsWith("unlockall_"))
								{
									Singleton<ItemManager>.instance.AddExtraItem("character", 14, 8);
									Singleton<ItemManager>.instance.AddExtraItem("loading", 29, 5);
								}
								if (text == "music_package_29" || text.StartsWith("unlockall_"))
								{
									Singleton<ItemManager>.instance.AddExtraItem("character", 15, 8);
									Singleton<ItemManager>.instance.AddExtraItem("loading", 34, 5);
								}
								if (text == "music_package_32" || text.StartsWith("unlockall_"))
								{
									Singleton<ItemManager>.instance.CheckAndAddWelcome(6, 5, false);
								}
								if (text == "music_package_33" || text.StartsWith("unlockall_"))
								{
									Singleton<ItemManager>.instance.AddExtraItem("character", 16, 8);
									Singleton<ItemManager>.instance.CheckAndAddWelcome(8, 5, false);
								}
								Singleton<DataManager>.instance["IAP"][id].SetResult(true);
								Singleton<DataManager>.instance.Save();
								onPurchaseRestoreCompleted.Invoke(id);
								onRestoreCompleted.Invoke();
								flag = true;
							}
						}
						if (!flag)
						{
							if (product.price == 0f)
							{
								PurchaseFree(product.id);
							}
							else
							{
								Singleton<XDSDKManager>.instance.Pay(info);
							}
						}
					}
				});
			}
			else
			{
				onPurchaseRestoreFailed.Invoke(string.Empty, null);
				RequirePrice();
			}
		}

		private void PurchaseFree(string productId)
		{
			Singleton<ServerManager>.instance.SendToUrl("purchase/zero_price_product", "POST", new Dictionary<string, object>
			{
				{
					"product_id",
					productId
				}
			}, headers: new Dictionary<string, string>
			{
				{
					"Authorization",
					Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>()
				}
			}, callback: delegate(JObject jObj)
			{
				if ((int)jObj["code"] != 0)
				{
					OnTransactionComplete(new TransactionResult
					{
						productId = productId,
						state = TransactionState.Failed
					});
				}
				else
				{
					Singleton<DataManager>.instance["IAP"][productId].SetResult(true);
					Singleton<DataManager>.instance.Save();
					OnTransactionComplete(new TransactionResult
					{
						productId = productId,
						state = TransactionState.Purchased
					});
				}
			});
		}

		private void GoogleLogic()
		{
			if (m_IsGoogleStoreLoaded)
			{
				if (type == IAPType.Purchase)
				{
					if (m_States.ContainsKey(id) && m_States[id].purchaseState == GooglePurchase.GooglePurchaseState.Purchased)
					{
						OnTransactionComplete(new TransactionResult
						{
							productId = id,
							state = TransactionState.Restored
						});
						return;
					}
					Debug.Log("====refresh remote purchased start");
					RefreshRemotePurchasedList(delegate
					{
						Debug.Log("====refresh remote purchased end");
						if (IsUnlockAll())
						{
							onPurchaseRestoreCompleted.Invoke(id);
						}
						else
						{
							Debug.Log("===purchase===" + GetId());
							GoogleIAB.purchaseProduct(GetId());
						}
					});
				}
				else
				{
					if (type != IAPType.Restore)
					{
						return;
					}
					List<string> list2 = new List<string>();
					int count = Singleton<ConfigManager>.instance["albums"].Count;
					for (int i = 1; i < count; i++)
					{
						string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", i, "uid");
						if (configStringValue.StartsWith("music_package_") && !Singleton<ConfigManager>.instance.GetConfigBoolValue("albums", i, "free"))
						{
							list2.Add(configStringValue);
						}
					}
					for (int j = 0; j <= planMaxCount; j++)
					{
						if (j != 6)
						{
							list2.Add($"unlockall_{j}");
						}
					}
					Action<List<GooglePurchase>, List<GoogleSkuInfo>> getProductions = null;
					getProductions = delegate(List<GooglePurchase> list, List<GoogleSkuInfo> infos)
					{
						GoogleIABManager.queryInventorySucceededEvent -= getProductions;
						infos.For(delegate(GoogleSkuInfo info)
						{
							Product product = products.Find((Product p) => p.id == info.productId);
							if (product != null)
							{
								product.title = info.title;
								product.isAvailable = true;
								product.localizedPrice = info.price;
							}
							GooglePurchase googlePurchase = list.Find((GooglePurchase l) => l.productId == info.productId);
							if (googlePurchase != null)
							{
								m_States[googlePurchase.productId] = googlePurchase;
							}
						});
						products.For(delegate(Product p)
						{
							if ((p.id.StartsWith("unlockall_") || !Singleton<ConfigManager>.instance.GetConfigBoolValue("albums", "uid", "free", p.id)) && m_States.ContainsKey(p.id))
							{
								OnTransactionComplete(new TransactionResult
								{
									productId = p.id,
									state = TransactionState.Restored
								});
							}
						});
						m_IsReactable = false;
					};
					GoogleIABManager.queryInventorySucceededEvent += getProductions;
					GoogleIAB.queryInventory(list2.ToArray());
				}
			}
			else
			{
				onPurchaseRestoreFailed.Invoke(id, null);
				InitStoreKit();
			}
		}

		private void AppleLogic(bool isOverSea)
		{
			if (SingletonMonoBehaviour<IAPManager>.instance.isInited)
			{
				if (type == IAPType.Purchase)
				{
					bool flag = false;
					if (!IsUnlockAllPackage())
					{
						flag = Singleton<ConfigManager>.instance.GetConfigBoolValue("albums", "uid", "free", id);
					}
					if (flag)
					{
						return;
					}
					RefreshRemotePurchasedList(delegate
					{
						if (IsUnlockAll())
						{
							onPurchaseRestoreCompleted.Invoke(id);
						}
						else if (isOverSea)
						{
							SingletonMonoBehaviour<IAPManager>.instance.Purchase(GetId());
						}
						else
						{
							Singleton<XDSDKManager>.instance.XDLogin();
							SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
							{
								if (m_IsRealName)
								{
									SingletonMonoBehaviour<IAPManager>.instance.Purchase(GetId());
								}
							}, () => Singleton<XDSDKManager>.instance.IsXDLoggedIn());
						}
					});
				}
				else
				{
					if (type != IAPType.Restore)
					{
						return;
					}
					m_RestoreEnterTransaction = false;
					m_RestoreCount = 0;
					Action restore = null;
					restore = delegate
					{
						SingletonMonoBehaviour<IAPManager>.instance.onRestoreCompleted -= restore;
						SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
						{
							RefreshLocalPurchasedList();
							onUnlockRestoreSucceed.Invoke();
						}, () => m_RestoreCount == 0);
					};
					SingletonMonoBehaviour<IAPManager>.instance.onRestoreCompleted += restore;
					SingletonMonoBehaviour<IAPManager>.instance.RestoreAll();
				}
			}
			else
			{
				onPurchaseRestoreFailed.Invoke(id, null);
				InitStoreKit();
			}
		}

		private void EditorLogic()
		{
			if (type == IAPType.Purchase)
			{
				OnTransactionComplete(new TransactionResult
				{
					productId = id,
					state = TransactionState.Purchased
				});
				onRestoreCompleted.Invoke();
			}
			else
			{
				if (type != IAPType.Restore)
				{
					return;
				}
				products.For(delegate(Product p)
				{
					if (p.id.StartsWith("unlockall_") || !Singleton<ConfigManager>.instance.GetConfigBoolValue("albums", "uid", "free", p.id))
					{
						OnTransactionComplete(new TransactionResult
						{
							productId = p.id,
							state = TransactionState.Purchased
						});
					}
				});
				m_IsReactable = false;
			}
		}

		private bool FreePackageLogic()
		{
			int result = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
			if (type == IAPType.Purchase)
			{
				if (IsUnlockAllPackage())
				{
					return false;
				}
				if (Singleton<ConfigManager>.instance.GetConfigBoolValue("albums", "uid", "free", id))
				{
					if (result < 30 && id == "music_package_6")
					{
						ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "noGetFree30"));
						onPurchaseRestoreCompleted.Invoke(id);
						return true;
					}
					if (result < 15 && id == "music_package_21")
					{
						ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "noGetFree15"));
						onPurchaseRestoreCompleted.Invoke(id);
						return true;
					}
					if (Application.internetReachability == NetworkReachability.NotReachable)
					{
						onPurchaseRestoreFailed.Invoke(id, null);
					}
					else
					{
						Singleton<DataManager>.instance["IAP"][id].SetResult(true);
						bool result2 = Singleton<DataManager>.instance["Account"][id].GetResult<bool>();
						onPurchaseRestoreCompleted.Invoke(id);
						if (result2)
						{
							onRestoreCompleted.Invoke();
							return true;
						}
						Singleton<DataManager>.instance["Account"][id].SetResult(true);
						ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, GetFreeSucceeded()));
					}
					Singleton<DataManager>.instance.Save();
					return true;
				}
				return false;
			}
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				onPurchaseRestoreFailed.Invoke(id, null);
			}
			else
			{
				JArray json = Singleton<ConfigManager>.instance.GetJson("albums", false);
				for (int i = 0; i < json.Count; i++)
				{
					JToken jToken = json[i];
					string uid = (string)jToken["uid"];
					if ((bool)jToken["free"] && !Singleton<DataManager>.instance["IAP"][uid].GetResult<bool>() && result > 30)
					{
						Singleton<DataManager>.instance["IAP"][uid].SetResult(true);
						if (Singleton<DataManager>.instance["Account"][uid].GetResult<bool>())
						{
							m_IsResoreOne = true;
							continue;
						}
						Singleton<DataManager>.instance["Account"][uid].SetResult(true);
						m_IsGetFree = true;
					}
				}
			}
			Singleton<DataManager>.instance.Save();
			return false;
		}

		private static void RefreshLocalPurchasedList()
		{
			m_AlbumIds.Clear();
			JArray json = Singleton<ConfigManager>.instance.GetJson("albums", false);
			for (int i = 0; i < json.Count; i++)
			{
				JToken jToken = json[i];
				bool flag = (bool)jToken["free"];
				string text = (string)jToken["uid"];
				if (!flag && Singleton<DataManager>.instance["IAP"][text].GetResult<bool>() && !m_AlbumIds.Contains(text) && text.StartsWith("music_package_"))
				{
					m_AlbumIds.Add(text);
				}
			}
		}

		private void RefreshRemotePurchasedList(Action callback = null)
		{
			if (m_IsApple && callback != null)
			{
				m_NoResult = true;
				m_RestoreCount = 0;
				m_IsRestoring = true;
				Action restore = null;
				restore = delegate
				{
					SingletonMonoBehaviour<IAPManager>.instance.onRestoreCompleted -= restore;
					SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
					{
						m_IsRestoring = false;
						m_NoResult = false;
						m_IsReactable = true;
						RefreshLocalPurchasedList();
						onUnlockRestoreSucceed.Invoke();
						callback();
					}, () => m_RestoreCount == 0, 15f, delegate
					{
						onPurchaseRestoreFailed.Invoke(id, null);
					});
				};
				SingletonMonoBehaviour<IAPManager>.instance.onRestoreCompleted += restore;
				SingletonMonoBehaviour<IAPManager>.instance.RestoreAll();
			}
			if (m_IsGoogle)
			{
				int count = Singleton<ConfigManager>.instance["albums"].Count;
				List<string> list2 = new List<string>();
				for (int i = 1; i < count; i++)
				{
					string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", i, "uid");
					if (configStringValue.StartsWith("music_package_") && !Singleton<ConfigManager>.instance.GetConfigBoolValue("albums", i, "free"))
					{
						list2.Add(configStringValue);
					}
				}
				for (int j = 0; j <= planMaxCount; j++)
				{
					if (j != 6)
					{
						list2.Add($"unlockall_{j}");
					}
				}
				Action<List<GooglePurchase>, List<GoogleSkuInfo>> getProductions = null;
				getProductions = delegate(List<GooglePurchase> list, List<GoogleSkuInfo> infos)
				{
					Debug.Log("====get production===");
					m_AlbumIds.Clear();
					GoogleIABManager.queryInventorySucceededEvent -= getProductions;
					infos.For(delegate(GoogleSkuInfo info)
					{
						GooglePurchase googlePurchase = list.Find((GooglePurchase l) => l.productId == info.productId);
						if (googlePurchase != null)
						{
							string productId = googlePurchase.productId;
							Singleton<DataManager>.instance["IAP"][productId].SetResult(true);
							if (productId.StartsWith("music_package_") && !m_AlbumIds.Contains(productId))
							{
								m_AlbumIds.Add(productId);
							}
						}
					});
					RefreshLocalPurchasedList();
					onUnlockRestoreSucceed.Invoke();
					if (callback != null)
					{
						callback();
					}
				};
				GoogleIABManager.queryInventorySucceededEvent += getProductions;
				Debug.Log("=====google require purchase info=====" + JsonUtils.Serialize(list2));
				GoogleIAB.queryInventory(list2.ToArray());
			}
			if (!m_IsTapTap)
			{
				return;
			}
			List<string> list3 = new List<string>();
			JArray json = Singleton<ConfigManager>.instance.GetJson("albums", false);
			for (int k = 0; k < json.Count; k++)
			{
				JToken jToken = json[k];
				bool flag = (bool)jToken["free"];
				string text = (string)jToken["uid"];
				if (!flag && text != "unlockall_0")
				{
					list3.Add(text);
				}
			}
			for (int m = 0; m <= planMaxCount; m++)
			{
				if (m != 6)
				{
					list3.Add($"unlockall_{m}");
				}
			}
			if (Singleton<XDSDKManager>.instance.IsLoggedIn())
			{
				m_AlbumIds.Clear();
				Singleton<ServerManager>.instance.SendToUrl("purchase/check_xd_purchase", headers: new Dictionary<string, string>
				{
					{
						"Authorization",
						Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>()
					}
				}, method: "GET", datas: null, callback: delegate(JObject jObj)
				{
					if ((int)jObj["code"] != 0)
					{
						onPurchaseRestoreFailed.Invoke(id, null);
					}
					else
					{
						JToken jToken2 = jObj["data"];
						foreach (JToken item in (IEnumerable<JToken>)jToken2)
						{
							string text2 = (string)item["product_id"];
							Singleton<DataManager>.instance["IAP"][text2].SetResult(true);
							if (!m_AlbumIds.Contains(text2) && text2.StartsWith("music_package_"))
							{
								m_AlbumIds.Add(text2);
							}
						}
						RefreshLocalPurchasedList();
						onUnlockRestoreSucceed.Invoke();
						if (callback != null)
						{
							callback();
						}
					}
				});
			}
			else
			{
				for (int n = 0; n < list3.Count; n++)
				{
					string uid = list3[n];
					Singleton<DataManager>.instance["IAP"][uid].SetResult(false);
				}
				Singleton<DataManager>.instance.Save();
			}
		}

		public static int GetIAPCount()
		{
			return m_AlbumIds.Count;
		}

		public bool IsUnlockAllPackage()
		{
			return id.StartsWith("unlockall_");
		}

		public string GetId()
		{
			if (IsUnlockAllPackage())
			{
				id = GetUnlockId();
				return id;
			}
			return id;
		}

		public static string GetUnlockId()
		{
			RefreshLocalPurchasedList();
			int num = planMaxCount - 1;
			int num2 = (m_AlbumIds.Count <= num) ? m_AlbumIds.Count : num;
			if (num2 >= 6)
			{
				num2++;
			}
			return $"unlockall_{num2}";
		}

		public static bool IsUnlockAll()
		{
			if (m_UnlockAllIDs.Count == 0)
			{
				for (int i = 0; i <= planMaxCount; i++)
				{
					m_UnlockAllIDs.Add("unlockall_" + i);
				}
			}
			for (int j = 0; j <= planMaxCount; j++)
			{
				if (j != 6 && Singleton<DataManager>.instance["IAP"][m_UnlockAllIDs[j]].GetResult<bool>())
				{
					return true;
				}
			}
			return false;
		}

		public void SetId(string i)
		{
			id = i;
			if (IsUnlockAllPackage())
			{
				id = GetId();
			}
		}

		private void OnTransactionComplete(TransactionResult result)
		{
			if (!m_IsReactable && !m_IsRestoring)
			{
				return;
			}
			if (type == IAPType.Purchase && !IsUnlockAllPackage())
			{
				if (!IsUnlockAllPackage())
				{
					m_IsReactable = false;
				}
				else if (result.productId.StartsWith("unlockall_"))
				{
					m_IsReactable = false;
				}
			}
			if (type == IAPType.Restore)
			{
				m_RestoreEnterTransaction = true;
			}
			string productId = result.productId;
			switch (result.state)
			{
			case TransactionState.Purchased:
			case TransactionState.Restored:
				if ((m_IsApple || m_IsGoogle) && m_IsApple)
				{
					m_RestoreCount++;
				}
				SecondaryVerify(result, delegate(bool isSuccueed)
				{
					if (isSuccueed)
					{
						if (productId.StartsWith("music_package_") && !productId.Contains(productId))
						{
							m_AlbumIds.Add(productId);
						}
						if (productId.StartsWith("unlockall_"))
						{
							Singleton<ItemManager>.instance.AddExtraItem("loading", 14, 5);
						}
						if (productId == "music_package_25" || productId.StartsWith("unlockall_"))
						{
							Singleton<ItemManager>.instance.AddExtraItem("character", 14, 8);
							Singleton<ItemManager>.instance.AddExtraItem("loading", 29, 5);
						}
						if (productId == "music_package_29" || productId.StartsWith("unlockall_"))
						{
							Singleton<ItemManager>.instance.AddExtraItem("character", 15, 8);
							Singleton<ItemManager>.instance.AddExtraItem("loading", 34, 5);
						}
						if (productId == "music_package_32" || productId.StartsWith("unlockall_"))
						{
							Singleton<ItemManager>.instance.CheckAndAddWelcome(6, 5, false);
						}
						if (productId == "music_package_33" || productId.StartsWith("unlockall_"))
						{
							Singleton<ItemManager>.instance.AddExtraItem("character", 16, 8);
							Singleton<ItemManager>.instance.CheckAndAddWelcome(8, 5, false);
						}
						if (!Singleton<DataManager>.instance["IAP"][productId].GetResult<bool>())
						{
							m_IsResoreOne = true;
						}
						if (m_IsResoreOne)
						{
							if (!m_IsGetFree)
							{
								if (!m_IsReactable && type == IAPType.Restore && m_HasShowText)
								{
									m_HasShowText = false;
									if (!m_NoResult)
									{
										onRestoreCompleted.Invoke();
									}
								}
							}
							else if (m_HasShowText)
							{
								m_HasShowText = false;
								string key = string.Empty;
								if (Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>() > 15)
								{
									key = "getFreeAndRestoreSucceeded15";
								}
								if (Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>() > 30)
								{
									key = "getFreeAndRestoreSucceeded30";
								}
								ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, key));
							}
						}
						else if (m_IsGetFree && m_HasShowText)
						{
							m_HasShowText = false;
							ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, GetFreeSucceeded()));
						}
						Singleton<DataManager>.instance["IAP"][productId].SetResult(true);
						Singleton<DataManager>.instance.Save();
						if (!m_NoResult)
						{
							onPurchaseRestoreCompleted.Invoke(productId);
						}
					}
					else
					{
						onPurchaseRestoreFailed.Invoke(productId, result);
					}
					Singleton<EventManager>.instance.Invoke("UI/OnRemotePurchasedRefresh");
					if (m_IsApple)
					{
						m_RestoreCount--;
					}
				});
				break;
			case TransactionState.Deferred:
			case TransactionState.Failed:
				onPurchaseRestoreFailed.Invoke(productId, result);
				break;
			}
		}

		private void SecondaryVerify(TransactionResult transactionResult, Action<bool> callback)
		{
			if (m_IsApple)
			{
				Dictionary<string, object> dictionary = new Dictionary<string, object>();
				dictionary.Add("receipt_data", transactionResult.receipt);
				Dictionary<string, object> datas = dictionary;
				Singleton<ServerManager>.instance.SendToUrl("purchase/receipts", "POST", datas, delegate(JObject jObject)
				{
					JToken jToken2 = jObject["status"];
					if (jToken2.IsNullOrEmpty())
					{
						callback(false);
					}
					else
					{
						int num = (int)jToken2;
						callback(num == 0);
					}
				});
			}
			if (m_IsGoogle)
			{
				if (m_States.ContainsKey(transactionResult.productId))
				{
					GooglePurchase state = m_States[transactionResult.productId];
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					dictionary.Add("signedData", state.originalJson);
					dictionary.Add("signature", state.signature);
					Dictionary<string, object> datas2 = dictionary;
					Singleton<ServerManager>.instance.SendToCloud("google_play_verify_signature", datas2, delegate(Task<string> t)
					{
						string a = Singleton<ServerManager>.instance.Dncrypt(t.Result);
						if (a == "purchased")
						{
							if (transactionResult.state == TransactionState.Restored && type == IAPType.Purchase)
							{
								onRestoreCompleted.Invoke();
							}
							callback(true);
						}
						else
						{
							onPurchaseRestoreCompleted.Invoke(state.productId);
						}
					}, delegate
					{
						callback(false);
					}, 10f);
				}
				else
				{
					callback(false);
				}
			}
			if (m_IsTapTap)
			{
				string pid = transactionResult.productId;
				int count = 0;
				Action sendCallback = null;
				sendCallback = delegate
				{
					if (count > 4)
					{
						callback(false);
					}
					count++;
					Singleton<ServerManager>.instance.SendToUrl("purchase/check_xd_purchase", headers: new Dictionary<string, string>
					{
						{
							"Authorization",
							Singleton<DataManager>.instance["GameConfig"]["Auth"].GetResult<string>()
						}
					}, method: "GET", datas: null, callback: delegate(JObject jObj)
					{
						if ((int)jObj["code"] != 0)
						{
							callback(false);
						}
						else
						{
							JToken jToken = jObj["data"];
							foreach (JToken item in (IEnumerable<JToken>)jToken)
							{
								if ((string)item["product_id"] == pid)
								{
									callback(true);
									return;
								}
							}
							SingletonMonoBehaviour<CoroutineManager>.instance.Delay(sendCallback, 5f);
						}
					});
				};
				sendCallback();
			}
			if (m_IsEditor)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					callback(true);
				}, 1f);
			}
		}

		private string GetFreeSucceeded()
		{
			if (id == "music_package_21")
			{
				return "getFreeSucceeded15";
			}
			if (id == "music_package_6")
			{
				return "getFreeSucceeded30";
			}
			return string.Empty;
		}
	}
}
