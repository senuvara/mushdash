using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.GeneralLocalization;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Specials
{
	public class SwitchLanguages : MonoBehaviour
	{
		[Required]
		public Button btnSwitch;

		[Required]
		public Text langText;

		private List<string> m_Options;

		private IVariable m_SaveVal;

		private int m_SelectedInd;

		private void Awake()
		{
			m_Options = ListPool<string>.Get();
			SingletonScriptableObject<LocalizationSettings>.instance.GetScheme("Language").GetAllOptionsName(m_Options);
			m_SaveVal = Singleton<DataManager>.instance.GetVariable("Account/Language");
			string result = m_SaveVal.GetResult<string>();
			m_SelectedInd = m_Options.IndexOf(result);
			m_SelectedInd = Mathf.Clamp(m_SelectedInd, 0, m_Options.Count - 1);
			btnSwitch.onClick.AddListener(OnClick);
		}

		private void OnDestroy()
		{
			ListPool<string>.Release(m_Options);
		}

		private void OnClick()
		{
			m_SelectedInd = ++m_SelectedInd % m_Options.Count;
			SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption("Language", m_Options[m_SelectedInd]);
			m_SaveVal.SetResult(m_Options[m_SelectedInd]);
			Singleton<DataManager>.instance.Save();
		}
	}
}
