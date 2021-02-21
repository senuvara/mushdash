using Assets.Scripts.PeroTools.Managers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Commons
{
	public static class JsonUtils
	{
		public static object UnityClone(this object obj)
		{
			return JsonUtility.FromJson(JsonUtility.ToJson(obj), obj.GetType());
		}

		public static object Clone(this object obj)
		{
			return Deserialize(Serialize(obj), obj.GetType());
		}

		public static List<string> Paths(JToken jToken, params JTokenType[] types)
		{
			List<JToken> tokens = new List<JToken>();
			Action<JToken> callFunc = null;
			callFunc = delegate(JToken token)
			{
				if (token.Any())
				{
					foreach (JToken item in token.Children())
					{
						callFunc(item);
						if (types != null && types.Length != 0)
						{
							if (types.Contains(item.Type))
							{
								tokens.Add(item);
							}
						}
						else if (item.Type == JTokenType.Property)
						{
							tokens.Add(item);
						}
					}
				}
			};
			callFunc(jToken);
			return tokens.Select(delegate(JToken t)
			{
				string text = t.Path.Replace("[0]", string.Empty);
				text = text.Remove(0, 1);
				return text.Replace('.', '/');
			}).ToList();
		}

		public static int ArrayDepth(JToken jToken, string path)
		{
			int num = 1;
			string[] array = path.Split('/');
			int num2 = 0;
			while (jToken != null)
			{
				if (jToken.Type == JTokenType.Array)
				{
					jToken = jToken.First();
					num++;
				}
				else if (jToken.Type == JTokenType.Object)
				{
					string key = array[num2++];
					jToken = jToken.Children().ToList().Find((JToken c) => c.Path.Contains(key));
				}
				else
				{
					jToken = ((jToken.Type != JTokenType.Property) ? null : JProperty.Load(jToken.CreateReader()).Value);
				}
			}
			return num;
		}

		public static bool IsNullOrEmpty(this JToken token)
		{
			return token == null || (token.Type == JTokenType.Array && !token.HasValues) || (token.Type == JTokenType.Object && !token.HasValues) || (token.Type == JTokenType.String && token.ToString() == string.Empty) || token.Type == JTokenType.Null;
		}

		public static JToken GetValue(JToken jToken, string path, int[] idxs)
		{
			string[] array = path.Split('/');
			int num = 0;
			int num2 = 0;
			bool flag = false;
			string a = array[0];
			if (a == "Json Count - 1")
			{
				return jToken.Count() - 1;
			}
			if (a == "Json Count")
			{
				return jToken.Count();
			}
			while (!flag)
			{
				if (jToken.Type == JTokenType.Array)
				{
					int num3 = idxs[num2++];
					jToken = jToken[num3];
				}
				else if (jToken.Type == JTokenType.Object)
				{
					string key = array[num++];
					jToken = jToken.Children().ToList().Find((JToken c) => c.Path.Contains(key));
				}
				else if (jToken.Type == JTokenType.Property)
				{
					jToken = JProperty.Load(jToken.CreateReader()).Value;
				}
				else
				{
					flag = true;
				}
			}
			return jToken;
		}

		public static List<string> Keys(this JToken jToken)
		{
			return jToken.Select((JToken t) => JProperty.Load(t.CreateReader()).Name).ToList();
		}

		public static string Name(this JToken jToken)
		{
			return (jToken.Type == JTokenType.Property) ? JProperty.Load(jToken.CreateReader()).Name : string.Empty;
		}

		public static List<string> DiffProperty(object newValue, object oldValue)
		{
			Dictionary<string, object> newDic = ToDictionary(newValue);
			if (oldValue == null)
			{
				return newDic.Keys.ToList();
			}
			Dictionary<string, object> array = ToDictionary(oldValue);
			return (from kvp in array
				let key = kvp.Key
				where newDic.ContainsKey(key)
				where kvp.Value != newDic[key]
				select key).ToList();
		}

		public static Dictionary<string, object> ToDictionary(object obj)
		{
			return Deserialize<Dictionary<string, object>>(Serialize(obj));
		}

		public static JArray ToArray(string json)
		{
			return JArray.Parse(json);
		}

		public static JObject ToObject(string json)
		{
			return JObject.Parse(json);
		}

		public static T DeserializeByJson<T>(string jsonName, JsonSerializerSettings setting = null)
		{
			string json = Singleton<ConfigManager>.instance[jsonName].ToString();
			return Deserialize<T>(json);
		}

		public static T Deserialize<T>(string json, JsonSerializerSettings setting = null)
		{
			return (T)Deserialize(json, typeof(T), setting);
		}

		public static object Deserialize(string json, Type type, JsonSerializerSettings setting = null)
		{
			return JsonConvert.DeserializeObject(json, type, setting);
		}

		public static object Deserialize(string json)
		{
			return JsonConvert.DeserializeObject(json);
		}

		public static string Serialize<T>(T target, JsonSerializerSettings setting = null)
		{
			return JsonConvert.SerializeObject(target, setting);
		}
	}
}
