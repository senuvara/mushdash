using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using SA.Common.Pattern;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common
{
	public class IOSSynchronize : Assets.Scripts.PeroTools.Commons.Singleton<IOSSynchronize>
	{
		public void Synchronize()
		{
			Debug.Log("[iCloud] Save Local Datas to Cloud");
			Dictionary<string, IData> datas = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance.datas;
			foreach (KeyValuePair<string, IData> item in datas)
			{
				SingletonDataObject singletonDataObject = item.Value as SingletonDataObject;
				if ((bool)singletonDataObject && singletonDataObject.isSync)
				{
					SA.Common.Pattern.Singleton<iCloudManager>.Instance.SetString(singletonDataObject.name, JsonUtils.Serialize(singletonDataObject.ToBytes()));
				}
			}
		}
	}
}
