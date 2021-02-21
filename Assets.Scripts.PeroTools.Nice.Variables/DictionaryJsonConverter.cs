using Assets.Scripts.PeroTools.Nice.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.PeroTools.Nice.Variables
{
	public class DictionaryJsonConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Dictionary<string, IVariable> dictionary = value as Dictionary<string, IVariable>;
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			if (dictionary != null)
			{
				foreach (KeyValuePair<string, IVariable> item in dictionary)
				{
					Constance constance = item.Value as Constance;
					if (constance != null)
					{
						dictionary2.Add(item.Key, item.Value);
					}
				}
			}
			value = dictionary2;
			serializer.Serialize(writer, value);
		}

		public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
		{
			return serializer.Deserialize(reader, type);
		}

		public override bool CanConvert(Type type)
		{
			return type == typeof(Dictionary<string, IVariable>);
		}
	}
}
