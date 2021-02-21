using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Assets.Scripts.GameCore
{
	public class DataUpgrader : Singleton<DataUpgrader>
	{
		public bool Upgrade(string inData, out string outData)
		{
			if (inData.StartsWith("{"))
			{
				outData = inData;
				return false;
			}
			byte[] bytes = JsonUtils.Deserialize<byte[]>(inData);
			Dictionary<string, IVariable> field = SerializationUtility.DeserializeValue<Dictionary<string, IVariable>>(bytes, DataFormat.Binary);
			outData = SingletonDataObject.FieldToJson(field);
			return true;
		}

		public void LocalUpgrade()
		{
			Dictionary<string, IData> datas = Singleton<DataManager>.instance.datas;
			bool flag = true;
			foreach (KeyValuePair<string, IData> item in datas)
			{
				SingletonDataObject exists = item.Value as SingletonDataObject;
				if (!exists)
				{
					continue;
				}
				string @string = Singleton<ConfigManager>.instance.GetString(item.Key);
				if (!string.IsNullOrEmpty(@string))
				{
					string outData;
					if (!Upgrade(@string, out outData))
					{
						flag = false;
						break;
					}
					Singleton<ConfigManager>.instance.SaveString(item.Key, outData);
				}
			}
			if (flag)
			{
				Singleton<DataManager>.instance.Load();
			}
		}
	}
}
