using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Platforms;
using Assets.Scripts.PeroTools.Platforms.Steam;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Datas
{
	public class DataManager : Singleton<DataManager>
	{
		private const int m_Version = 8;

		private ISync m_PlatformISync;

		private const char splitChar = '/';

		public Dictionary<string, IData> datas
		{
			get;
			private set;
		}

		public SingletonDataObject this[string uid]
		{
			get
			{
				if (!datas.ContainsKey(uid))
				{
					Debug.Log($"Data with uid {uid} not found!");
					return null;
				}
				SingletonDataObject singletonDataObject = datas[uid] as SingletonDataObject;
				if (!singletonDataObject)
				{
					Debug.Log($"Data with uid {uid} isn't a singleton");
					return null;
				}
				return singletonDataObject;
			}
			set
			{
				if (datas.ContainsKey(uid))
				{
					datas[uid] = value;
				}
				else
				{
					Debug.Log($"Data with uid {uid} not found!");
				}
			}
		}

		private void VersionCheckUp()
		{
			switch (Singleton<ConfigManager>.instance.GetObject<int>("DatasVersion"))
			{
			case 8:
				break;
			case 0:
				Singleton<ConfigManager>.instance.SaveObject("DatasVersion", 8);
				break;
			default:
			{
				Reset();
				DataManager instance = Singleton<DataManager>.instance;
				break;
			}
			}
		}

		public void Init()
		{
			m_PlatformISync = new SteamSync();
			Load();
			VersionCheckUp();
		}

		private void OnApplicationQuit()
		{
			ResetLocalData();
		}

		private void ResetLocalData()
		{
			foreach (KeyValuePair<string, IData> data in datas)
			{
				SingletonDataObject singletonDataObject = data.Value as SingletonDataObject;
				if ((bool)singletonDataObject)
				{
					singletonDataObject.Reset();
				}
			}
		}

		public void Load()
		{
			datas = (datas ?? new Dictionary<string, IData>());
			DataObject[] array = Singleton<AssetBundleManager>.instance.LoadAllAssetFromAssetBundle<DataObject>("globalconfigs");
			array.For(delegate(DataObject data)
			{
				datas[data.name] = data;
			});
			if (m_PlatformISync != null)
			{
				array.For(delegate(DataObject data)
				{
					SingletonDataObject singletonDataObject2 = data as SingletonDataObject;
					if ((bool)singletonDataObject2)
					{
						singletonDataObject2.Reload();
					}
				});
				m_PlatformISync.LoadLocal();
				return;
			}
			array.For(delegate(DataObject data)
			{
				SingletonDataObject singletonDataObject = data as SingletonDataObject;
				if ((bool)singletonDataObject)
				{
					singletonDataObject.Load();
				}
			});
		}

		public void Save()
		{
			Singleton<DataManager>.instance["Account"]["LastSaveTime"].SetResult(DateTime.Now.ToString(CultureInfo.CurrentCulture));
			if (m_PlatformISync != null)
			{
				m_PlatformISync.SaveLocal();
				return;
			}
			foreach (KeyValuePair<string, IData> data in datas)
			{
				SingletonDataObject singletonDataObject = data.Value as SingletonDataObject;
				if ((bool)singletonDataObject)
				{
					singletonDataObject.Save();
				}
			}
			Singleton<ConfigManager>.instance.Save();
		}

		public new void Reset()
		{
			Singleton<ConfigManager>.instance.ClearAllPrefs();
			Singleton<ConfigManager>.instance.SaveObject("DatasVersion", 8);
			ResetLocalData();
			base.Reset();
		}

		public string ToJson()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			foreach (KeyValuePair<string, IData> data in datas)
			{
				SingletonDataObject singletonDataObject = data.Value as SingletonDataObject;
				if ((bool)singletonDataObject && singletonDataObject.isSync)
				{
					dictionary.Add(data.Key, singletonDataObject.ToJson());
				}
			}
			return JsonUtils.Serialize(dictionary);
		}

		[Obsolete]
		public byte[] ToBytes()
		{
			Dictionary<string, byte[]> dictionary = new Dictionary<string, byte[]>();
			if (datas != null)
			{
				foreach (KeyValuePair<string, IData> data in datas)
				{
					SingletonDataObject singletonDataObject = data.Value as SingletonDataObject;
					if ((bool)singletonDataObject && singletonDataObject.isSync)
					{
						dictionary.Add(data.Key, singletonDataObject.ToBytes());
					}
				}
			}
			return SerializationUtility.SerializeValue(dictionary, DataFormat.Binary);
		}

		public void LoadFromJson(string json)
		{
			Dictionary<string, object> dictionary = JsonUtils.Deserialize<Dictionary<string, object>>(json);
			if (dictionary == null)
			{
				return;
			}
			foreach (KeyValuePair<string, object> item in dictionary)
			{
				SingletonDataObject singletonDataObject = datas[item.Key] as SingletonDataObject;
				if (singletonDataObject != null)
				{
					singletonDataObject.LoadFromJson(item.Value.ToString());
				}
			}
		}

		[Obsolete]
		public void LoadFromBytes(byte[] bytes)
		{
			if (bytes == null || bytes.Length <= 0)
			{
				return;
			}
			if (datas == null)
			{
				Debug.LogError("Unable to load bytes to datamanager with field 'datas' empty.");
				return;
			}
			Dictionary<string, byte[]> dictionary = SerializationUtility.DeserializeValue<Dictionary<string, byte[]>>(bytes, DataFormat.Binary);
			if (dictionary == null)
			{
				return;
			}
			foreach (KeyValuePair<string, byte[]> item in dictionary)
			{
				SingletonDataObject singletonDataObject = datas[item.Key] as SingletonDataObject;
				if (singletonDataObject != null)
				{
					singletonDataObject.LoadFromBytes(item.Value);
				}
			}
		}

		public IVariable GetVariable(string uid)
		{
			string[] array = uid.Split('/');
			string uid2 = array[0];
			string uid3 = array[1];
			return this[uid2][uid3];
		}

		public void SetVariable(string uid, IVariable variable)
		{
			string[] array = uid.Split('/');
			string uid2 = array[0];
			string uid3 = array[1];
			this[uid2][uid3] = variable;
		}

		public List<string> GetUids()
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, IData> data in datas)
			{
				if (data.Value is SingletonDataObject)
				{
					string name = data.Key;
					Dictionary<string, IVariable> fields = data.Value.fields;
					list.AddRange(fields.Select((KeyValuePair<string, IVariable> field) => $"{name}/{field.Key}"));
				}
			}
			return list;
		}
	}
}
