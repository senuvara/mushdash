using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Components;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.PreWarm;
using Assets.Scripts.PeroTools.UI;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
	public class PnlDlc : MonoBehaviour, IPreWarm
	{
		public FancyScrollView fancyScrollView;

		public Text txtTitle;

		public Text txtPrice;

		public Text txtMusicAuthor;

		public Text txtDiscount;

		public Text txtPurchased;

		public GameObject tip;

		public BtnIAP btnIAP;

		public GameObject discount;

		private Coroutine m_Coroutine;

		private Coroutine m_NextCoroutine;

		private CoroutineManager m_CoroutineManager;

		private AudioManager m_AudioManager;

		private string m_CurMusicName;

		private EventManager.EventTrigger m_EventTrigger;

		private readonly Dictionary<string, string[]> m_MusicPrices = new Dictionary<string, string[]>
		{
			{
				"70%",
				new string[11]
				{
					"¥18.00",
					"$2.99",
					"3,49 €",
					"¥360",
					"HK$ 23.00",
					"£2.99",
					"NT$ 90",
					"3,49€",
					"HK$23.00",
					"NT$90",
					"₩3,900"
				}
			},
			{
				"50%",
				new string[11]
				{
					"¥12.00",
					"$1.99",
					"2,29 €",
					"¥240",
					"HK$ 15.00",
					"£1.99",
					"NT$ 60",
					"2,29€",
					"HK$15.00",
					"NT$60",
					"₩2,500"
				}
			},
			{
				"30%",
				new string[11]
				{
					"¥8.00",
					"$0.99",
					"1,99 €",
					"¥150",
					"HK$ 8.00",
					"£0.99",
					"NT$ 33",
					"1,99€",
					"HK$8.00",
					"NT$33",
					"₩1,200"
				}
			},
			{
				"25%",
				new string[11]
				{
					"¥6.00",
					"$0.99",
					"1,09 €",
					"¥120",
					"HK$ 8.00",
					"£0.99",
					"NT$ 33",
					"1,09€",
					"HK$8.00",
					"NT$33",
					"₩1,200"
				}
			},
			{
				"5%",
				new string[11]
				{
					"¥1.00",
					"$0.99",
					"0,49 €",
					"¥120",
					"HK$ 8.00",
					"£0.49",
					"NT$ 60",
					"0,49€",
					"HK$8.00",
					"NT$33",
					"₩500"
				}
			},
			{
				"0%",
				new string[11]
				{
					"¥0.00",
					"$0.00",
					"0,00 €",
					"¥0",
					"HK$ 0.00",
					"£0.00",
					"NT$ 0",
					"0,00€",
					"HK$0.00",
					"NT$0",
					"₩0"
				}
			}
		};

		private readonly Dictionary<string, string[]> m_UnlockPrices = new Dictionary<string, string[]>
		{
			{
				"90%",
				new string[11]
				{
					"¥168.00",
					"$25.99",
					"28,99 €",
					"¥3100",
					"HK$ 203.00",
					"£24.99",
					"NT$ 790",
					"28,99€",
					"HK$203.00",
					"NT$790",
					"₩32,000"
				}
			},
			{
				"80%",
				new string[11]
				{
					"¥153.00",
					"$22.99",
					"24,99 €",
					"¥2800",
					"HK$ 178.00",
					"£21.99",
					"NT$ 690",
					"24,99€",
					"HK$178.00",
					"NT$690",
					"₩29,000"
				}
			},
			{
				"70%",
				new string[11]
				{
					"¥138.00",
					"$20.99",
					"22,99 €",
					"¥2500",
					"HK$ 163.00",
					"£20.99",
					"NT$ 630",
					"22,99€",
					"HK$163.00",
					"NT$630",
					"₩26,000"
				}
			},
			{
				"65%",
				new string[11]
				{
					"¥123.00",
					"$18.99",
					"20,99 €",
					"¥2300",
					"HK$ 148.00",
					"£18.99",
					"NT$ 570",
					"20,99€",
					"HK$148.00",
					"NT$570",
					"₩23,000"
				}
			},
			{
				"60%",
				new string[11]
				{
					"¥118.00",
					"$17.99",
					"19,99 €",
					"¥2200",
					"HK$ 138.00",
					"£17.99",
					"NT$ 540",
					"19,99€",
					"HK$138.00",
					"NT$540",
					"₩22,000"
				}
			},
			{
				"56%",
				new string[11]
				{
					"¥108.00",
					"$15.99",
					"17,99 €",
					"¥1900",
					"HK$ 123.00",
					"£15.99",
					"NT$ 490",
					"17,99€",
					"HK$123.00",
					"NT$490",
					"₩20,000"
				}
			},
			{
				"50%",
				new string[11]
				{
					"¥98.00",
					"$14.99",
					"16,99 €",
					"¥1800",
					"HK$ 118.00",
					"£14.99",
					"NT$ 450",
					"16,99€",
					"HK$118.00",
					"NT$450",
					"₩19,000"
				}
			},
			{
				"40%",
				new string[11]
				{
					"¥78.00",
					"$11.99",
					"12,99 €",
					"¥1480",
					"HK$ 88.00",
					"£11.99",
					"NT$ 390",
					"12,99€",
					"HK$88.00",
					"NT$390",
					"₩15,000"
				}
			},
			{
				"30%",
				new string[11]
				{
					"¥60.00",
					"$8.99",
					"9,99 €",
					"¥1100",
					"HK$ 68.00",
					"£8.99",
					"NT$ 290",
					"9,99€",
					"HK$68.00",
					"NT$290",
					"₩11,000"
				}
			},
			{
				"25%",
				new string[11]
				{
					"¥50.00",
					"$7.99",
					"8,99 €",
					"¥980",
					"HK$ 58.00",
					"£7.99",
					"NT$ 270",
					"8,99€",
					"HK$58.00",
					"NT$270",
					"₩9,900"
				}
			},
			{
				"20%",
				new string[11]
				{
					"¥40.00",
					"$5.99",
					"6,99 €",
					"¥730",
					"HK$ 48.00",
					"£5.99",
					"NT$ 190",
					"6,99€",
					"HK$48.00",
					"NT$190",
					"₩7,500"
				}
			},
			{
				"3%",
				new string[11]
				{
					"¥6.00",
					"$0.99",
					"1,09 €",
					"¥120",
					"HK$ 8.00",
					"£0.99",
					"NT$ 33",
					"1,09€",
					"HK$8.00",
					"NT$33",
					"₩1,200"
				}
			},
			{
				"1.5%",
				new string[11]
				{
					"¥3.00",
					"$0.99",
					"0,99 €",
					"¥120",
					"HK$ 8.00",
					"£0.79",
					"NT$ 33",
					"0,99€",
					"HK$8.00",
					"NT$33",
					"₩900"
				}
			}
		};

		public void PreWarm(int slice)
		{
			if (slice != 0)
			{
				return;
			}
			m_CoroutineManager = SingletonMonoBehaviour<CoroutineManager>.instance;
			m_AudioManager = Singleton<AudioManager>.instance;
			fancyScrollView.onFinalItemIndexChange += delegate(int i)
			{
				if (m_Coroutine != null)
				{
					m_CoroutineManager.StopCoroutine(m_Coroutine);
				}
				int index = i + 1;
				string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", index, "jsonName");
				if (!string.IsNullOrEmpty(configStringValue))
				{
					JArray json = Singleton<ConfigManager>.instance.GetJson(configStringValue, false);
					List<string> list = new List<string>();
					for (int j = 0; j < json.Count; j++)
					{
						list.Add((string)json[j]["demo"]);
					}
					if (configStringValue == "ALBUM26")
					{
						list.Add("best_one_demo");
					}
					PlayAlbum(list, Random.Range(0, list.Count - 1));
				}
				else
				{
					if (m_NextCoroutine != null)
					{
						m_CoroutineManager.StopCoroutine(m_NextCoroutine);
					}
					Singleton<AudioManager>.instance.PlayBGM("MD OP");
				}
			};
			fancyScrollView.onItemIndexChange += RefreshItem;
			btnIAP.onUnlockRestoreSucceed.AddListener(RefreshItem);
			m_EventTrigger = Singleton<EventManager>.instance.RegEvent("UI/OnRemotePurchasedRefresh");
			m_EventTrigger.trigger += OnTrigger;
			Singleton<EventManager>.instance.RegEvent("UI/SwitchToDLC").trigger += UnlockAskToDlc;
		}

		private void OnDestroy()
		{
			m_EventTrigger.trigger -= OnTrigger;
			Singleton<EventManager>.instance.RegEvent("UI/SwitchToDLC").trigger -= UnlockAskToDlc;
		}

		private void OnTrigger(object sender, object reciever, params object[] args)
		{
			RefreshItem();
		}

		public void RefreshToIndex(string packageName)
		{
			JArray json = Singleton<ConfigManager>.instance.GetJson("albums", false);
			int count = json.Count;
			for (int i = 0; i < count; i++)
			{
				if (packageName == (string)json[i]["uid"])
				{
					fancyScrollView.startIndex.SetResult(i - 1);
					fancyScrollView.Rebuild();
				}
			}
		}

		private void UnlockAskToDlc(object sender, object reciever, params object[] args)
		{
			if ((bool)this)
			{
				Application.OpenURL("steam://advertise/1055810");
			}
		}

		public void RefreshItem()
		{
			RefreshItem(fancyScrollView.selectItemIndex);
		}

		private void RefreshItem(int i)
		{
			int index = i + 1;
			string uid = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", index, "uid");
			uid = ((!(uid == "unlockall_0")) ? uid : BtnIAP.GetUnlockId());
			tip.SetActive(uid.StartsWith("unlockall_"));
			txtMusicAuthor.gameObject.SetActive(uid.StartsWith("music_package_"));
			txtPrice.text = Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "purchaseInavaiable");
			Product product = BtnIAP.products.Find((Product p) => p.id == uid);
			if (product == null)
			{
				return;
			}
			txtTitle.text = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", index, "title");
			txtPrice.text = ((!(product.price >= 0f)) ? txtPrice.text : product.localizedPrice);
			bool configBoolValue = Singleton<ConfigManager>.instance.GetConfigBoolValue("albums", index, "free");
			if (configBoolValue)
			{
				int result = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
				string key = string.Empty;
				if (uid == "music_package_21")
				{
					key = ((result < 15) ? "getFreeLimited15" : "getFree");
				}
				if (uid == "music_package_6")
				{
					key = ((result < 30) ? "getFreeLimited30" : "getFree");
				}
				txtPrice.text = Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, key);
			}
			txtPurchased.text = Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, (!configBoolValue) ? "purchased" : "freePurchased");
			discount.SetActive(false);
			if (!configBoolValue)
			{
				SetDiscountByPrice(product);
				SetUnlockDiscountByPrice(product);
			}
			if (Singleton<DataManager>.instance["IAP"][product.id].GetResult<bool>() || BtnIAP.IsUnlockAll())
			{
				discount.SetActive(false);
			}
		}

		private void SetDiscountByPrice(Product product)
		{
			if (!product.id.StartsWith("music_package_"))
			{
				return;
			}
			discount.SetActive(false);
			foreach (KeyValuePair<string, string[]> musicPrice in m_MusicPrices)
			{
				if (musicPrice.Value.Contains(product.localizedPrice))
				{
					txtDiscount.text = musicPrice.Key;
					discount.SetActive(true);
					break;
				}
			}
		}

		private void SetUnlockDiscountByPrice(Product product)
		{
			if (!product.id.StartsWith("unlockall_"))
			{
				return;
			}
			discount.SetActive(false);
			foreach (KeyValuePair<string, string[]> unlockPrice in m_UnlockPrices)
			{
				if (unlockPrice.Value.Contains(product.localizedPrice))
				{
					txtDiscount.text = unlockPrice.Key;
					discount.SetActive(true);
					break;
				}
			}
		}

		private void PlayAlbum(List<string> musics, int index)
		{
			AudioClip audioClip = Singleton<AssetBundleManager>.instance.LoadFromName<AudioClip>(musics[index]);
			m_AudioManager.StopBGM();
			m_AudioManager.PlayBGM(audioClip);
			if (m_NextCoroutine != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_NextCoroutine);
			}
			m_NextCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				if (m_AudioManager.bgm.clip != audioClip)
				{
					m_AudioManager.PlayBGM(audioClip);
				}
			}, 0);
			m_Coroutine = m_CoroutineManager.Delay(delegate
			{
				int index2 = (index != musics.Count - 1) ? (++index) : 0;
				PlayAlbum(musics, index2);
			}, MathUtils.Floor(audioClip.length, 1));
		}

		private void Awake()
		{
			Singleton<EventManager>.instance.RegEvent("UI/OnDlcMusicSelected").trigger -= OnDlcMusicSelected;
			Singleton<EventManager>.instance.RegEvent("UI/OnDlcMusicSelected").trigger += OnDlcMusicSelected;
		}

		private void OnDlcMusicSelected(object sender, object reciever, params object[] args)
		{
			if ((bool)m_CoroutineManager && m_Coroutine != null)
			{
				m_CoroutineManager.StopCoroutine(m_Coroutine);
			}
		}

		private void OnEnable()
		{
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				RefreshItem(fancyScrollView.selectItemIndex);
			}, 0.1f);
		}

		private void OnDisable()
		{
			if ((bool)m_CoroutineManager && m_Coroutine != null)
			{
				m_CoroutineManager.StopCoroutine(m_Coroutine);
			}
		}

		private void Update()
		{
			if (!(Singleton<AudioManager>.instance.bgm.clip != null) || !(m_CurMusicName != Singleton<AudioManager>.instance.bgm.clip.name))
			{
				return;
			}
			m_CurMusicName = Singleton<AudioManager>.instance.bgm.clip.name;
			Singleton<EventManager>.instance.Invoke("UI/OnDlcMusicChanged");
			int count = Singleton<ConfigManager>.instance["albums"].Count;
			for (int i = 1; i < count; i++)
			{
				string text = $"ALBUM{i + 1}";
				JArray json = Singleton<ConfigManager>.instance.GetJson(text, false);
				if (json != null)
				{
					for (int j = 0; j < json.Count; j++)
					{
						JToken jToken = json[j];
						if ((string)jToken["demo"] == m_CurMusicName)
						{
							txtMusicAuthor.text = string.Format("{0} - {1}", Singleton<ConfigManager>.instance.GetConfigStringValue(text, j, "name"), Singleton<ConfigManager>.instance.GetConfigStringValue(text, j, "author"));
							return;
						}
					}
				}
				else
				{
					txtMusicAuthor.text = null;
				}
			}
			if (m_CurMusicName == "best_one_demo")
			{
				txtMusicAuthor.text = Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "dlcjkBuroSelect");
			}
			if (m_CurMusicName == "you_make_my_life_1up_selected_demo")
			{
				txtMusicAuthor.text = Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "nanahiraSelect");
			}
			if (m_CurMusicName == "char_4_yume_bgm")
			{
				txtMusicAuthor.text = Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "dlcGCSelect");
			}
			if (m_CurMusicName == "preparara_selected_demo")
			{
				txtMusicAuthor.text = Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "dlcSquareLakeSelect");
			}
			if (m_CurMusicName == "char_5_neko_bgm")
			{
				txtMusicAuthor.text = Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "dlcCytusIISelect");
			}
		}

		public void OpenFeedBackWindows()
		{
		}
	}
}
