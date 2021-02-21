using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Assets.Scripts.GameCore.Managers
{
	public class StageManager : Singleton<StageManager>
	{
		private List<IData> m_UnlockStages;

		private List<int> m_UnlockAlbums;

		private void Init()
		{
			m_UnlockStages = Singleton<DataManager>.instance["Account"]["UnlockStages"].GetResult<List<IData>>();
			m_UnlockAlbums = Singleton<DataManager>.instance["Account"]["UnlockAlbums"].GetResult<List<int>>();
			if (m_UnlockStages.Count == 0)
			{
				UnlockStage(false);
			}
			DataUpdate();
		}

		private void DataUpdate()
		{
			int result = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
			JArray json = Singleton<ConfigManager>.instance.GetJson("ALBUM1", false);
			for (int i = 0; i < json.Count; i++)
			{
				JToken jToken = json[i];
				string uid = (string)jToken["uid"];
				if (result >= (int)jToken["unlockLevel"] && !m_UnlockStages.Exists((IData s) => s["uid"].GetResult<string>() == uid) && !m_UnlockStages.Exists((IData s) => s.Exists("index") && s["index"].GetResult<int>() == i))
				{
					Data data = new Data();
					data["uid"].SetResult(uid);
					data["new"].SetResult(false);
					m_UnlockStages.Add(data);
				}
			}
			Singleton<DataManager>.instance.Save();
		}

		public void UnlockStage(bool isNew = true)
		{
			JArray jArray = Singleton<ConfigManager>.instance["ALBUM1"];
			int result = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
			for (int i = 0; i < jArray.Count; i++)
			{
				string uid = Singleton<ConfigManager>.instance.GetConfigStringValue("ALBUM1", i, "uid");
				if (!m_UnlockStages.Exists((IData d) => d["uid"].GetResult<string>() == uid))
				{
					int configIntValue = Singleton<ConfigManager>.instance.GetConfigIntValue("ALBUM1", i, "unlockLevel");
					if (configIntValue <= result)
					{
						UnlockStage(uid, isNew);
					}
				}
			}
		}

		public void UnlockStage(string uid, bool isNew = true)
		{
			if (!m_UnlockStages.Exists((IData d) => d["uid"].GetResult<string>() == uid))
			{
				Data data = new Data();
				data["uid"].SetResult(uid);
				data["new"].SetResult(isNew);
				m_UnlockStages.Add(data);
			}
		}

		public void UnlockAlbum(int albumIndex)
		{
			if (!m_UnlockAlbums.Contains(albumIndex))
			{
				m_UnlockAlbums.Add(albumIndex);
			}
		}
	}
}
