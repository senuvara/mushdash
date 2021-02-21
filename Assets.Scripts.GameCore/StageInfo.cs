using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using GameLogic;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Assets.Scripts.GameCore
{
	public class StageInfo : SerializedScriptableObject
	{
		[ReadOnly]
		public List<MusicData> musicDatas;

		[ReadOnly]
		public decimal delay;

		[ReadOnly]
		public string mapName;

		[ReadOnly]
		public string music;

		[ReadOnly]
		public string scene;

		[ReadOnly]
		public int difficulty;

		[ReadOnly]
		public string md5;

		[ReadOnly]
		public float bpm;

		[ReadOnly]
		public List<SceneEvent> sceneEvents;

		[Button]
		public void Refresh()
		{
			Refresh(mapName);
		}

		public void Refresh(string m)
		{
			MusicConfigReader.Instance.bms = Singleton<iBMSCManager>.instance.Load(m);
			musicDatas = from o in MusicConfigReader.Instance.GetData(mapName).ToArray()
				select (MusicData)o;
			delay = MusicConfigReader.Instance.delay;
			mapName = (string)MusicConfigReader.Instance.bms.info["NAME"];
			music = ((string)MusicConfigReader.Instance.bms.info["WAV10"]).BeginBefore('.');
			scene = (string)MusicConfigReader.Instance.bms.info["GENRE"];
			difficulty = int.Parse((string)MusicConfigReader.Instance.bms.info["RANK"]);
			bpm = MusicConfigReader.Instance.bms.GetBpm();
			md5 = MusicConfigReader.Instance.bms.md5;
			sceneEvents = MusicConfigReader.Instance.sceneEvents;
		}
	}
}
