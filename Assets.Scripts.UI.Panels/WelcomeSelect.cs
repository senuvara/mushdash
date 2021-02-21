using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.PreWarm;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
	public class WelcomeSelect : MonoBehaviour, IPreWarm
	{
		public GameObject welcome;

		public CanvasScaler uiCanvas;

		private int m_WelcomeIndex;

		private void Start()
		{
			welcome.SetActive(true);
			if (m_WelcomeIndex == 1 || m_WelcomeIndex == 8)
			{
				uiCanvas.matchWidthOrHeight = 0f;
			}
		}

		public void PreWarm(int slice)
		{
			if (slice == 1)
			{
				bool flag = Singleton<ItemManager>.instance.ChristmasItemLogic("welcome");
				bool flag2 = Singleton<ItemManager>.instance.IsMay();
				bool flag3 = Singleton<ItemManager>.instance.IsNanahira();
				bool flag4 = Singleton<ItemManager>.instance.IsFestivalCarnival();
				List<int> result = Singleton<DataManager>.instance["Account"]["UseWelcomeIndex"].GetResult<List<int>>();
				if (result.Count == 0)
				{
					result.Add(2);
				}
				m_WelcomeIndex = result.Random();
				if (flag)
				{
					m_WelcomeIndex = 3;
				}
				if (flag2)
				{
					m_WelcomeIndex = 4;
				}
				if (flag3)
				{
					m_WelcomeIndex = 5;
				}
				if (flag4)
				{
					m_WelcomeIndex = 7;
				}
				if (!Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>($"Welcome_0{m_WelcomeIndex + 1}"))
				{
					m_WelcomeIndex = 0;
				}
				Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>($"Welcome_0{m_WelcomeIndex + 1}"), welcome.transform);
			}
		}
	}
}
