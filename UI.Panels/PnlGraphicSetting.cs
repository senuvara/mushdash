using Assets.Scripts.Graphics;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels
{
	public class PnlGraphicSetting : MonoBehaviour
	{
		[Required]
		public ToggleGroup quality;

		[Required]
		public ToggleGroup effect;

		public GameObject hightImgSuggest;

		public GameObject lowImgSuggest;

		[Required]
		public ToggleGroup advanced;

		private IVariable m_QualityData;

		private IVariable m_EffectData;

		private IVariable m_Advanced;

		private void Awake()
		{
			InitVariable();
			if ((bool)quality)
			{
				InitGroup(quality, m_QualityData, GraphicSettings.GetRecommandQualityCode());
			}
			InitGroup(effect, m_EffectData, GraphicSettings.GetRecommandEffectCode());
			InitGroup(advanced, m_Advanced);
		}

		private void InitVariable()
		{
			m_QualityData = Singleton<DataManager>.instance["GameConfig"]["QualityLevel"];
			m_EffectData = Singleton<DataManager>.instance["GameConfig"]["EffectLevel"];
			m_Advanced = Singleton<DataManager>.instance["Account"]["IsAdvancedJudge"];
		}

		private void InitGroup(ToggleGroup group, IVariable var, int recommand)
		{
			Toggle[] componentsInChildren = group.GetComponentsInChildren<Toggle>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].group = group;
			}
			componentsInChildren[var.GetResult<int>()].isOn = true;
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				Toggle toggle = componentsInChildren[j];
				int index = j;
				toggle.onValueChanged.AddListener(delegate(bool val)
				{
					if (val)
					{
						var.SetResult(index);
					}
				});
			}
			if (recommand == 0)
			{
				lowImgSuggest.SetActive(true);
			}
			else
			{
				hightImgSuggest.SetActive(true);
			}
		}

		private void InitGroup(ToggleGroup group, IVariable var)
		{
			Toggle[] componentsInChildren = group.GetComponentsInChildren<Toggle>();
			if (var.GetResult<bool>())
			{
				componentsInChildren[0].isOn = true;
			}
			else
			{
				componentsInChildren[1].isOn = true;
			}
			componentsInChildren[0].onValueChanged.AddListener(delegate(bool val)
			{
				if (val)
				{
					var.SetResult(val);
				}
			});
			componentsInChildren[1].onValueChanged.AddListener(delegate(bool val)
			{
				if (val)
				{
					var.SetResult(!val);
				}
			});
		}

		private void OnDisable()
		{
			Singleton<DataManager>.instance.Save();
		}
	}
}
