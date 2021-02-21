using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.UI.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
	public class PnlIdInput : MonoBehaviour
	{
		public InputField inputField;

		public Button btnComfirm;

		public Button btnVerifying;

		public Button btnCancel;

		private void Start()
		{
			btnComfirm.onClick.AddListener(delegate
			{
				btnVerifying.gameObject.SetActive(true);
				string playerName = inputField.text;
				if (playerName == Singleton<DataManager>.instance["Account"]["PlayerName"].GetResult<string>())
				{
					btnVerifying.gameObject.SetActive(false);
					btnCancel.onClick.Invoke();
				}
				else if (string.IsNullOrEmpty(playerName))
				{
					ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "invaildName"));
					btnVerifying.gameObject.SetActive(false);
				}
				else if (playerName.Length > 10)
				{
					ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "tooLongName"));
					btnVerifying.gameObject.SetActive(false);
				}
				else
				{
					Singleton<ServerManager>.instance.SetPlayerName(playerName, delegate(int code)
					{
						btnVerifying.gameObject.SetActive(false);
						string text = string.Empty;
						switch (code)
						{
						case 10005:
							text = "invaildName";
							break;
						case 10009:
							text = "repeatName";
							break;
						case 0:
							text = "changeNameSuccess";
							btnCancel.onClick.Invoke();
							Singleton<DataManager>.instance["Account"]["PlayerName"].SetResult(playerName);
							Singleton<DataManager>.instance.Save();
							break;
						}
						if (!string.IsNullOrEmpty(text))
						{
							ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, text));
						}
					}, delegate
					{
						btnVerifying.gameObject.SetActive(false);
					});
				}
			});
		}
	}
}
