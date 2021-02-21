using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.PeroTools.Nice.Variables
{
	public class DataJsonConverter : CustomCreationConverter<IData>
	{
		public override bool CanWrite => true;

		public override bool CanRead => true;

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			Data data = value as Data;
			Dictionary<string, IVariable> dictionary = data?.fields;
			if (data != null && !string.IsNullOrEmpty(data.uid))
			{
				Assets.Scripts.PeroTools.Nice.Values.String @string = new Assets.Scripts.PeroTools.Nice.Values.String();
				@string.result = data.uid;
				Assets.Scripts.PeroTools.Nice.Values.String value2 = @string;
				Constance constance = (Constance)(dictionary["_dataObjectUid"] = new Constance(value2));
			}
			serializer.Serialize(writer, dictionary);
		}

		public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
		{
			Dictionary<string, IVariable> dictionary = serializer.Deserialize<Dictionary<string, IVariable>>(reader);
			string text = string.Empty;
			if (dictionary.ContainsKey("_dataObjectUid"))
			{
				text = dictionary["_dataObjectUid"].GetResult<string>();
				dictionary.Remove("_dataObjectUid");
			}
			Data data = (!string.IsNullOrEmpty(text)) ? new Data(text) : new Data();
			if (!string.IsNullOrEmpty(text))
			{
				Dictionary<string, IVariable> fields = Singleton<DataManager>.instance.datas[text].fields;
				foreach (KeyValuePair<string, IVariable> item in fields)
				{
					if (dictionary.ContainsKey(item.Key) && !(item.Value is Constance))
					{
						dictionary.Remove(item.Key);
					}
				}
			}
			data.fields = dictionary;
			return data;
		}

		public override IData Create(Type objectType)
		{
			return new Data();
		}
	}
}
