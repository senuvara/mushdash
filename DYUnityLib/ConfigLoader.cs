using LitJson;
using System;
using System.Collections;
using UnityEngine;

namespace DYUnityLib
{
	public class ConfigLoader
	{
		private static string DEFAULT_PATH = "config/";

		private TextAsset Load(string fileName, string path = null)
		{
			string text = path;
			if (text == null)
			{
				text = DEFAULT_PATH;
			}
			try
			{
				TextAsset textAsset = (TextAsset)Resources.Load(text + fileName);
				if (textAsset == null)
				{
					Debug.Log("DYULConfigLoader no such file " + text + fileName);
					return textAsset;
				}
				return textAsset;
			}
			catch (InvalidCastException)
			{
				return null;
			}
		}

		public JsonData LoadPrefs(string fileName)
		{
			string @string = PlayerPrefs.GetString(fileName);
			if (@string == null)
			{
				return null;
			}
			return JsonMapper.ToObject(@string);
		}

		public string LoadPrefs(string fileName, bool _string)
		{
			return PlayerPrefs.GetString(fileName);
		}

		public void SavePrefs(string fileName, JsonData data)
		{
			string value = JsonMapper.ToJson(data);
			PlayerPrefs.SetString(fileName, value);
		}

		public void SavePrefs(string fileName, string _string)
		{
			PlayerPrefs.SetString(fileName, _string);
		}

		public void ClearPrefs()
		{
			PlayerPrefs.DeleteAll();
		}

		public JsonData LoadAsJsonData(string fileName, string path = null)
		{
			TextAsset textAsset = Load(fileName, path);
			if (textAsset == null)
			{
				return null;
			}
			JsonReader reader = new JsonReader(textAsset.text);
			return JsonMapper.ToObject(reader);
		}

		public JsonData JsonFromExcelParse(JsonData config, int sheet = 0)
		{
			JsonData jsonData = config["Workbook"]["Worksheet"];
			if (jsonData == null || jsonData.Count <= 0 || jsonData.Count < sheet)
			{
				Debug.Log("Config has no sheet");
				return null;
			}
			IList list = jsonData[sheet]["Table"]["Row"];
			if (list == null || list.Count <= 1)
			{
				Debug.Log("Config sheet " + sheet + " has no data.");
				return null;
			}
			JsonData jsonData2 = new JsonData();
			IList list2 = ((JsonData)list[0])["Cell"];
			int count = list2.Count;
			for (int i = 1; i < list.Count; i++)
			{
				JsonData jsonData3 = (JsonData)list[i];
				if (jsonData3 == null || jsonData3.IsString || !jsonData3.Keys.Contains("Cell"))
				{
					Debug.Log("Config at row " + i + " has fucking no data.");
					continue;
				}
				IList list3 = jsonData3["Cell"];
				if (list3 == null)
				{
					continue;
				}
				JsonData jsonData4 = new JsonData();
				for (int j = 0; j < count; j++)
				{
					if (j < list3.Count)
					{
						string prop_name = (string)((JsonData)list2[j])["Data"];
						JsonData jsonData5 = (JsonData)list3[j];
						if (jsonData5 != null && !jsonData5.IsString && jsonData5.Keys.Contains("Data"))
						{
							JsonData jsonData7 = jsonData4[prop_name] = jsonData5["Data"];
						}
					}
				}
				jsonData2.Add(jsonData4);
			}
			return jsonData2;
		}

		public JsonData JsonFromExcelParse(JsonData config, int _BeginRow, int sheet = 0)
		{
			JsonData jsonData = config["Workbook"]["Worksheet"];
			if (jsonData == null || jsonData.Count <= 0 || jsonData.Count < sheet)
			{
				Debug.Log("Config has no sheet");
				return null;
			}
			IList list = jsonData["Table"]["Row"];
			if (list == null || list.Count <= 1)
			{
				Debug.Log("Config sheet " + sheet + " has no data.");
				return null;
			}
			JsonData jsonData2 = new JsonData();
			IList list2 = ((JsonData)list[0])["Cell"];
			int count = list2.Count;
			for (int i = _BeginRow - 1; i < list.Count; i++)
			{
				JsonData jsonData3 = (JsonData)list[i];
				if (jsonData3 == null || jsonData3.IsString || !jsonData3.Keys.Contains("Cell"))
				{
					Debug.Log("Config at row " + i + " has fucking no data.");
					continue;
				}
				IList list3 = jsonData3["Cell"];
				if (list3 == null)
				{
					continue;
				}
				JsonData jsonData4 = new JsonData();
				for (int j = 0; j < count; j++)
				{
					if (j < list3.Count)
					{
						string prop_name = (string)((JsonData)list2[j])["Data"];
						JsonData jsonData5 = (JsonData)list3[j];
						if (jsonData5 != null && !jsonData5.IsString && jsonData5.Keys.Contains("Data"))
						{
							JsonData jsonData7 = jsonData4[prop_name] = jsonData5["Data"];
						}
					}
				}
				jsonData2.Add(jsonData4);
			}
			return jsonData2;
		}
	}
}
