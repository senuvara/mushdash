using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.GeneralLocalization;
using Newtonsoft.Json.Linq;
using PeroTools.Saves;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Managers
{
	public class ConfigManager : Singleton<ConfigManager>
	{
		private readonly Dictionary<string, JArray> m_Dictionary = new Dictionary<string, JArray>();

		private readonly Dictionary<string, TextAsset> m_TextAssets = new Dictionary<string, TextAsset>();

		private PrefsHandler m_PrefsHandler;

		public JArray this[string jsonName] => GetJson(jsonName, true);

		private void Init()
		{
			InitPrefsHandler();
		}

		private void InitPrefsHandler()
		{
			m_PrefsHandler = new DefaultPrefsHandler();
		}

		public JArray GetJson(string name, bool localization)
		{
			TextAsset textAsset = null;
			if (Application.isPlaying)
			{
				if (localization)
				{
					string activeOption = SingletonScriptableObject<LocalizationSettings>.instance.GetActiveOption("Language");
					if (!string.IsNullOrEmpty(activeOption))
					{
						string text = $"{name}_{activeOption}";
						if (SingletonScriptableObject<AssetBundleConfigManager>.instance.Contains(text))
						{
							name = text;
						}
					}
				}
				if (m_Dictionary.ContainsKey(name))
				{
					return m_Dictionary[name];
				}
			}
			else if (m_TextAssets.ContainsKey(name))
			{
				textAsset = m_TextAssets[name];
			}
			string text2 = string.Empty;
			if (!textAsset)
			{
				textAsset = Singleton<AssetBundleManager>.instance.LoadFromName<TextAsset>($"{name}.json");
				if (textAsset == null)
				{
					Debug.Log($"Can not load json name {name}");
				}
			}
			if ((bool)textAsset)
			{
				if (!m_TextAssets.ContainsKey(name))
				{
					m_TextAssets.Add(name, textAsset);
				}
				text2 = textAsset.text;
			}
			if (!string.IsNullOrEmpty(text2))
			{
				JArray jArray = JsonUtils.ToArray(text2);
				if (!m_Dictionary.ContainsKey(name))
				{
					m_Dictionary.Add(name, jArray);
				}
				return jArray;
			}
			Debug.Log(name + " json not found");
			return null;
		}

		public void Add(string key, string data)
		{
			JArray value = JsonUtils.ToArray(data);
			if (m_Dictionary.ContainsKey(key))
			{
				m_Dictionary[key] = value;
			}
			else
			{
				m_Dictionary.Add(key, value);
			}
		}

		public void Clear()
		{
			m_Dictionary.Clear();
		}

		public void Remove(string key)
		{
			if (m_Dictionary.ContainsKey(key))
			{
				m_Dictionary.Remove(key);
			}
		}

		public JToken Convert(string path)
		{
			string fileName = Path.GetFileName(path);
			if (string.IsNullOrEmpty(fileName))
			{
				Debug.Log(path + " is null.");
				return null;
			}
			return GetJson(fileName, false);
		}

		public int GetConfigIndex(string fileName, string cmpKey, object cmpValue)
		{
			JArray json = GetJson(fileName, false);
			for (int i = 0; i < json.Count; i++)
			{
				if (json[i][cmpKey].ToString() == cmpValue.ToString())
				{
					return i;
				}
			}
			return -1;
		}

		public int GetConfigIntValue(string fileName, int index, string key)
		{
			return int.Parse(GetConfigStringValue(fileName, index, key));
		}

		public int GetConfigIntValue(string fileName, string cmpKey, string targetKey, object cmpValue)
		{
			return int.Parse(GetConfigStringValue(fileName, cmpKey, targetKey, cmpValue));
		}

		public float GetConfigFloatValue(string fileName, int index, string key)
		{
			return float.Parse(GetConfigStringValue(fileName, index, key));
		}

		public float GetConfigFloatValue(string fileName, string cmpKey, string targetKey, object cmpValue)
		{
			return float.Parse(GetConfigStringValue(fileName, cmpKey, targetKey, cmpValue));
		}

		public bool GetConfigBoolValue(string fileName, int index, string key)
		{
			return bool.Parse(GetConfigStringValue(fileName, index, key));
		}

		public bool GetConfigBoolValue(string fileName, string cmpKey, string targetKey, object cmpValue)
		{
			return bool.Parse(GetConfigStringValue(fileName, cmpKey, targetKey, cmpValue));
		}

		public string GetConfigStringValue(string fileName, int index, string key)
		{
			return GetConfigToken(fileName, index, key)?.ToString();
		}

		public string GetConfigStringValue(string fileName, string cmpKey, string targetKey, object cmpValue)
		{
			return GetConfigToken(fileName, cmpKey, targetKey, cmpValue)?.ToString();
		}

		public JToken GetConfigToken(string fileName, int index, string key)
		{
			JArray jArray = this[fileName];
			if (index < 0 || index >= jArray.Count)
			{
				return null;
			}
			jArray = ((!jArray[index].Keys().Contains(key)) ? GetJson(fileName, false) : jArray);
			return jArray[index][key];
		}

		public JToken GetConfigToken(string fileName, string cmpKey, string targetKey, object cmpValue)
		{
			if (cmpValue == null)
			{
				return null;
			}
			JArray jArray = this[fileName];
			JArray json = GetJson(fileName, false);
			for (int i = 0; i < json.Count; i++)
			{
				JToken jToken = json[i];
				JToken jToken2 = jToken[cmpKey];
				string a = jToken2.ToString();
				if (a == cmpValue.ToString())
				{
					JToken jToken3 = jArray[i];
					if (jToken3.Keys().Contains(targetKey))
					{
						return jToken3[targetKey];
					}
					return json[i][targetKey];
				}
			}
			return null;
		}

		public T GetObject<T>(string fileName)
		{
			string @string = GetString(fileName);
			if (string.IsNullOrEmpty(@string) || @string == "null")
			{
				T val = default(T);
				if (val != null)
				{
					SaveString(fileName, val.ToString());
				}
				return val;
			}
			if (typeof(T) == typeof(bool))
			{
				return (T)(object)bool.Parse(@string);
			}
			return JsonUtils.Deserialize<T>(@string);
		}

		public object GetObject(string fileName, Type type)
		{
			return JsonUtils.Deserialize(GetString(fileName), type);
		}

		public string GetString(string fileName)
		{
			return m_PrefsHandler.GetString(fileName);
		}

		public void SaveObject(string fileName, object obj)
		{
			SaveString(fileName, JsonUtils.Serialize(obj));
		}

		public void SaveString(string fileName, string s)
		{
			m_PrefsHandler.SaveString(fileName, s);
		}

		public void Save()
		{
			m_PrefsHandler.Save();
		}

		public void Delete(string fileName)
		{
			m_PrefsHandler.Delete(fileName);
		}

		public void ClearAllPrefs()
		{
			m_PrefsHandler.ClearAllPrefs();
		}
	}
}
