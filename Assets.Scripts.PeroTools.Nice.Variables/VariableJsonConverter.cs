using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Values;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.PeroTools.Nice.Variables
{
	public class VariableJsonConverter : CustomCreationConverter<IVariable>
	{
		public override bool CanWrite => true;

		public override bool CanRead => true;

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			IVariable variable = value as IVariable;
			if (variable != null)
			{
				serializer.Serialize(writer, variable.result);
			}
		}

		public override object ReadJson(JsonReader reader, Type type, object existingValue, JsonSerializer serializer)
		{
			object obj = serializer.Deserialize(reader);
			if (obj != null)
			{
				Constance constance;
				if (reader.TokenType == JsonToken.Integer)
				{
					Integer integer = new Integer();
					integer.result = Convert.ToInt32(obj);
					constance = new Constance(integer);
				}
				else if (reader.TokenType == JsonToken.Boolean)
				{
					Boolen boolen = new Boolen();
					boolen.result = obj;
					constance = new Constance(boolen);
				}
				else if (reader.TokenType == JsonToken.Float)
				{
					if (obj is double)
					{
						obj = Convert.ToSingle(obj);
					}
					Float @float = new Float();
					@float.result = obj;
					constance = new Constance(@float);
				}
				else if (reader.TokenType == JsonToken.String)
				{
					Assets.Scripts.PeroTools.Nice.Values.String @string = new Assets.Scripts.PeroTools.Nice.Values.String();
					@string.result = obj;
					constance = new Constance(@string);
				}
				else
				{
					if (reader.TokenType == JsonToken.EndArray)
					{
						JArray jArray = obj as JArray;
						if (jArray != null)
						{
							List<object> list = new List<object>();
							for (int i = 0; i < jArray.Count; i++)
							{
								JToken jToken = jArray[i];
								object obj2 = null;
								obj2 = ((jToken.Type != JTokenType.Object) ? ((jToken.Type != JTokenType.Integer) ? ((jToken.Type != JTokenType.Boolean) ? ((jToken.Type != JTokenType.Float) ? ((jToken.Type != JTokenType.String) ? jToken.ToObject<object>(serializer) : jToken.ToObject<string>(serializer)) : ((object)jToken.ToObject<float>(serializer))) : ((object)jToken.ToObject<bool>(serializer))) : ((object)jToken.ToObject<int>(serializer))) : jToken.ToObject<IData>(serializer));
								list.Add(obj2);
							}
							obj = list;
						}
					}
					constance = new Constance();
					constance.SetResult(obj);
				}
				return constance;
			}
			return null;
		}

		public override IVariable Create(Type type)
		{
			return new Constance();
		}
	}
}
