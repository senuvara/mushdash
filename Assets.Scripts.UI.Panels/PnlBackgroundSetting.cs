using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Components;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.PreWarm;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
	public class PnlBackgroundSetting : MonoBehaviour, IPreWarm
	{
		public Image[] images;

		public Slider slider;

		public Text text;

		public FancyScrollView fancyScrollView;

		private List<IData> m_Brightnesses;

		private readonly List<string> m_SceneNames = new List<string>
		{
			"scene_01",
			"scene_02",
			"scene_03",
			"scene_04",
			"scene_05",
			"scene_06",
			"scene_07"
		};

		private void OnSliderValueChanged(float value)
		{
			text.text = value.ToString("P0");
			int num = (fancyScrollView.selectItemIndex != -1) ? fancyScrollView.selectItemIndex : 0;
			string sceneName = m_SceneNames[num];
			images[num].DOFade(1f - value, 0f);
			IData data = m_Brightnesses.Find((IData b) => b["Uid"].GetResult<string>() == sceneName);
			data["Brightness"].SetResult(value);
		}

		private void OnSliderValueChanged(float value, bool reset)
		{
			if (reset)
			{
				slider.value = value;
			}
			OnSliderValueChanged(value);
		}

		private void OnSceneIndexChanged(int index)
		{
			string sceneName = m_SceneNames[index];
			IData data = m_Brightnesses.Find((IData b) => b["Uid"].GetResult<string>() == sceneName);
			if (data == null)
			{
				data = new Data();
				m_Brightnesses.Add(data);
				data["Uid"].SetResult(sceneName);
				data["Brightness"].SetResult(1f);
			}
			OnSliderValueChanged(data["Brightness"].GetResult<float>(), true);
		}

		private void OnEnable()
		{
			int index = (fancyScrollView.selectItemIndex != -1) ? fancyScrollView.selectItemIndex : 0;
			OnSceneIndexChanged(index);
		}

		private void OnDisable()
		{
			Singleton<DataManager>.instance["GameConfig"]["Brightness"].SetResult(slider.value);
			Singleton<DataManager>.instance.Save();
		}

		public void PreWarm(int slice)
		{
			if (slice == 0)
			{
				slider.onValueChanged.AddListener(OnSliderValueChanged);
				m_Brightnesses = Singleton<DataManager>.instance["GameConfig"]["Brightnesses"].GetResult<List<IData>>();
				fancyScrollView.onFinalItemIndexChange += OnSceneIndexChanged;
			}
		}
	}
}
