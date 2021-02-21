using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.UI.Controls;
using FormulaBase;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
	public class PnlTerminal : MonoBehaviour
	{
		public Button btnVerifying;

		private Coroutine m_Coroutine;

		public void Exchange(string code)
		{
			btnVerifying.gameObject.SetActive(true);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("ActivationCode", code);
			Dictionary<string, object> datas = dictionary;
			Singleton<ServerManager>.instance.SendToCloud("use_function_activation_code", datas, delegate(Task<string> t)
			{
				btnVerifying.gameObject.SetActive(false);
				string result = t.Result;
				if (result.Contains("success_"))
				{
					string a = result.Replace("success_", string.Empty);
					if (a == "unlock_base")
					{
						StageBattleComponent.UnlockAll();
						ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "unlockAll"));
					}
				}
				else
				{
					switch (result)
					{
					case "invalid":
						ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "invaildCode"));
						break;
					case "used":
						ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "codeUsed"));
						break;
					case "too fast":
						btnVerifying.gameObject.SetActive(true);
						if (m_Coroutine != null)
						{
							SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_Coroutine);
						}
						m_Coroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
						{
							btnVerifying.gameObject.SetActive(false);
						}, 5f);
						break;
					case "unlocked":
						ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "unlockAllready"));
						break;
					}
				}
			}, delegate
			{
				ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "invaildCode"));
				btnVerifying.gameObject.SetActive(false);
			}, 10f);
		}
	}
}
