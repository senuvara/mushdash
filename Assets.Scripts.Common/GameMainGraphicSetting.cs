using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.PreWarm;
using UnityEngine;

namespace Assets.Scripts.Common
{
	public class GameMainGraphicSetting : MonoBehaviour, IPreWarm
	{
		[HideInInspector]
		public bool IsAdvanced;

		private void Awake()
		{
			IsAdvanced = Singleton<DataManager>.instance["Account"]["IsAdvancedJudge"].GetResult<bool>();
		}

		public void PreWarm(int slice)
		{
			if (slice != 0)
			{
			}
		}
	}
}
