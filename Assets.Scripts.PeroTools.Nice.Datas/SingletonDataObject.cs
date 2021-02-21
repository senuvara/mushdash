using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Datas
{
	[CreateAssetMenu(fileName = "NewSingletonData", menuName = "Nice/Data/Singleton Data", order = 0)]
	[HideMonoScript]
	public class SingletonDataObject : DataObject
	{
		[SerializeField]
		private bool m_IsSaved;

		private Dictionary<string, IVariable> m_OriginField;

		public bool isSync;

		public void Load()
		{
			Reload();
			if (m_IsSaved)
			{
				string @string = Singleton<ConfigManager>.instance.GetString(base.name);
				if (!string.IsNullOrEmpty(@string))
				{
					LoadFromJson(@string);
				}
			}
		}

		public void Reload()
		{
			m_OriginField = (m_OriginField ?? m_Fields.ToDictionary((KeyValuePair<string, IVariable> f) => f.Key, (KeyValuePair<string, IVariable> f) => (IVariable)SerializationUtility.CreateCopy(f.Value)));
			foreach (KeyValuePair<string, IVariable> field in m_Fields)
			{
				Formula.SetFormulaData(field.Value, this);
			}
		}

		public void LoadFromJson(string json)
		{
			Dictionary<string, IVariable> dictionary = JsonToField(json);
			if (dictionary == null)
			{
				return;
			}
			foreach (KeyValuePair<string, IVariable> item in dictionary)
			{
				m_Fields[item.Key] = item.Value;
			}
		}

		[Obsolete]
		public void LoadFromBytes(byte[] bytes)
		{
			Dictionary<string, IVariable> dictionary = SerializationUtility.DeserializeValue<Dictionary<string, IVariable>>(bytes, DataFormat.Binary);
			if (dictionary == null)
			{
				return;
			}
			foreach (KeyValuePair<string, IVariable> item in dictionary)
			{
				m_Fields[item.Key] = item.Value;
			}
		}

		public Dictionary<string, IVariable> ToConstance()
		{
			return Enumerable.Where(m_Fields, (KeyValuePair<string, IVariable> field) => field.Value is Constance).ToDictionary((KeyValuePair<string, IVariable> field) => field.Key, (KeyValuePair<string, IVariable> field) => field.Value);
		}

		public string ToJson()
		{
			return FieldToJson(ToConstance());
		}

		[Obsolete]
		public byte[] ToBytes()
		{
			return SerializationUtility.SerializeValue(ToConstance(), DataFormat.Binary);
		}

		public void Save()
		{
			if (m_IsSaved)
			{
				Singleton<ConfigManager>.instance.SaveString(base.name, ToJson());
			}
		}

		public void Reset()
		{
			m_Fields = m_OriginField;
		}

		public static Dictionary<string, IVariable> JsonToField(string json)
		{
			Dictionary<string, IVariable> result = new Dictionary<string, IVariable>();
			try
			{
				result = JsonUtils.Deserialize<Dictionary<string, IVariable>>(json, new JsonSerializerSettings
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
					NullValueHandling = NullValueHandling.Ignore,
					Converters = new List<JsonConverter>
					{
						new VariableJsonConverter(),
						new DataJsonConverter()
					}
				});
				return result;
			}
			catch (Exception message)
			{
				Debug.Log(message);
				return result;
			}
		}

		public static string FieldToJson(Dictionary<string, IVariable> field)
		{
			return JsonUtils.Serialize(field, new JsonSerializerSettings
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				NullValueHandling = NullValueHandling.Ignore,
				Converters = new List<JsonConverter>
				{
					new VariableJsonConverter(),
					new DataJsonConverter(),
					new DictionaryJsonConverter()
				}
			});
		}
	}
}
