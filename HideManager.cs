using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using System.Collections.Generic;

public class HideManager : Singleton<HideManager>
{
	public string[] hideSongUids
	{
		get;
		private set;
	}

	private void Init()
	{
		hideSongUids = Singleton<DataManager>.instance["Account"]["Hides"].GetResult<List<string>>().ToArray();
	}

	public void RefreshHideSongs()
	{
		hideSongUids = Singleton<DataManager>.instance["Account"]["Hides"].GetResult<List<string>>().ToArray();
	}
}
