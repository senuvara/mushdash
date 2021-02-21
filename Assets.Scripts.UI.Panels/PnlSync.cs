using Assets.Scripts.Common.XDSDK;
using Assets.Scripts.GameCore;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using SA.Common.Pattern;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
	public class PnlSync : MonoBehaviour
	{
		[Space(10f)]
		public GameObject txtLocal;

		public GameObject txtCloud;

		[Space(10f)]
		public Button btnLocal;

		public Button btnCloud;

		public Button btnYes;

		public Button btnNo;

		[Space(10f)]
		public Text strLevelCloud;

		public Text strExpCloud;

		public Text strDateCloud;

		[Space(10f)]
		public Text strLevelLocal;

		public Text strExpLocal;

		[Space(10f)]
		public Text confirmLevelCloud;

		public Text confirmExpCloud;

		public Text confirmDateCloud;

		[Space(10f)]
		public Text confirmLevelLocal;

		public Text confirmExpLocal;

		private bool m_IsLocal;

		private int m_TotleKey;

		private int m_DoneKey;

		private bool m_IsHanding;

		private void Awake()
		{
			btnLocal.onClick.AddListener(delegate
			{
				m_IsLocal = true;
				txtCloud.SetActive(false);
				txtLocal.SetActive(true);
			});
			btnCloud.onClick.AddListener(delegate
			{
				m_IsLocal = false;
				txtCloud.SetActive(true);
				txtLocal.SetActive(false);
			});
			btnYes.onClick.AddListener(delegate
			{
				if (!m_IsLocal)
				{
					m_TotleKey = 0;
					m_DoneKey = 0;
					foreach (KeyValuePair<string, IData> data in Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance.datas)
					{
						SingletonDataObject singletonDataObject = data.Value as SingletonDataObject;
						if ((bool)singletonDataObject && singletonDataObject.isSync)
						{
							m_TotleKey++;
						}
					}
					SingletonDataObject singletonData = default(SingletonDataObject);
					foreach (KeyValuePair<string, IData> data2 in Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance.datas)
					{
						singletonData = (data2.Value as SingletonDataObject);
						if ((bool)singletonData && singletonData.isSync)
						{
							SA.Common.Pattern.Singleton<iCloudManager>.Instance.RequestDataForKey(singletonData.name, delegate(iCloudData cloudData)
							{
								m_DoneKey++;
								string outData;
								Assets.Scripts.PeroTools.Commons.Singleton<DataUpgrader>.instance.Upgrade(cloudData.StringValue, out outData);
								Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.SaveString(singletonData.name, outData);
								Debug.LogFormat("[Cloud Data] Override Cloud Data {0} to Local", singletonData.name);
								if (m_DoneKey >= m_TotleKey)
								{
									Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance.Load();
									Assets.Scripts.PeroTools.Commons.Singleton<SceneManager>.instance.LoadSceneViaLoadingScene("UISystem_PC");
								}
							});
						}
					}
				}
				m_IsHanding = false;
			});
			iCloudManager.OnStoreDidChangeExternally += OnCloudAccountChange;
		}

		private void OnDestroy()
		{
			iCloudManager.OnStoreDidChangeExternally -= OnCloudAccountChange;
		}

		private void GetInfoFromAccountStringData(string str, out int exp, out string lastSaveData)
		{
			exp = 0;
			lastSaveData = string.Empty;
			if (!string.IsNullOrEmpty(str))
			{
				byte[] bytes = JsonUtils.Deserialize<byte[]>(str);
				SingletonDataObject singletonDataObject = new SingletonDataObject();
				singletonDataObject.LoadFromBytes(bytes);
				exp = singletonDataObject["Exp"].GetResult<int>();
				lastSaveData = singletonDataObject["LastSaveTime"].GetResult<string>();
			}
		}

		private void OnCloudAccountChange(List<iCloudData> iCloudDatas)
		{
			Debug.Log("[Cloud Data] Received New Cloud data");
			string accountStr;
			SA.Common.Pattern.Singleton<iCloudManager>.Instance.RequestDataForKey("Account", delegate(iCloudData data)
			{
				if (Assets.Scripts.PeroTools.Commons.Singleton<XDSDKManager>.instance.isOvearSea)
				{
					if (!m_IsHanding && string.IsNullOrEmpty(data.StringValue))
					{
						DataObject[] array = Assets.Scripts.PeroTools.Commons.Singleton<AssetBundleManager>.instance.LoadAllAssetFromAssetBundle<DataObject>("globalconfigs");
						array.For(delegate(DataObject d)
						{
							SingletonDataObject exists = d as SingletonDataObject;
							if ((bool)exists)
							{
								string @string = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetString(d.name);
								SA.Common.Pattern.Singleton<iCloudManager>.Instance.SetString(d.name, @string);
							}
						});
					}
					else
					{
						accountStr = data.StringValue;
						byte[] bytes = JsonUtils.Deserialize<byte[]>(accountStr);
						SingletonDataObject singletonDataObject = new SingletonDataObject();
						singletonDataObject.LoadFromBytes(bytes);
						int exp;
						string lastSaveData;
						GetInfoFromAccountStringData(data.StringValue, out exp, out lastSaveData);
						int num = CacularLevel(exp);
						int num2 = CacularCurExp(exp);
						int result = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["CurExp"].GetResult<int>();
						int result2 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
						Debug.LogFormat("[Cloud] Last Save Time {0},cloud level {1},cloud exp {2},level {3},exp {4}", lastSaveData, num, num2, result2, result);
						if (result2 != num || result != num2 || m_IsHanding)
						{
							strLevelCloud.text = num.ToString();
							strExpCloud.text = num2.ToString();
							strDateCloud.text = lastSaveData;
							strLevelLocal.text = result2.ToString();
							strExpLocal.text = result.ToString();
							confirmLevelCloud.text = num.ToString();
							confirmExpCloud.text = num2.ToString();
							confirmDateCloud.text = lastSaveData;
							confirmLevelLocal.text = result2.ToString();
							confirmExpLocal.text = result.ToString();
							if (!m_IsHanding)
							{
								Assets.Scripts.PeroTools.Commons.Singleton<EventManager>.instance.Invoke("UI/OnCloudUpdate");
								m_IsHanding = true;
							}
						}
					}
				}
			});
		}

		private int CacularLevel(int exp)
		{
			return exp / 100 + 1;
		}

		private int CacularCurExp(int exp)
		{
			int num = CacularLevel(exp);
			return exp - (num - 1) * 100;
		}
	}
}
