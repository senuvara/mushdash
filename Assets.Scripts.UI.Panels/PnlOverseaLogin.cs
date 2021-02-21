using Assets.Scripts.Common.XDSDK;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Actions;
using Assets.Scripts.PeroTools.Nice.Events;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ZenFulcrum.EmbeddedBrowser;

namespace Assets.Scripts.UI.Panels
{
	public class PnlOverseaLogin : UISelectManage
	{
		public Button btnFaceBook;

		public Button btnGoogle;

		public Browser browser;

		public GameObject goPlatform;

		private string m_Copy;

		private List<Button> m_Buttons = new List<Button>();

		[SerializeField]
		private GameObject m_GoogleLoginWeb;

		public override void OnInit()
		{
			m_Buttons = GameUtils.FindObjectsOfType<Button>(base.transform);
			btnFaceBook.onClick.AddListener(delegate
			{
				browser.GetComponent<RawImage>().enabled = false;
				goPlatform.SetActive(true);
				browser.LoadURL("https://prpr-muse-dash.avosapps.us/oauth/facebook/login", true);
			});
			btnGoogle.onClick.AddListener(delegate
			{
				SingletonMonoBehaviour<SteamGoogleLogin>.instance.OnWebInit();
				SingletonMonoBehaviour<SteamGoogleLogin>.instance.OpenWeb();
			});
			browser.onLoad += delegate(JSONNode node)
			{
				browser.GetComponent<RawImage>().enabled = true;
				if (!node.IsNull)
				{
					JSONNode jSONNode = node["status"];
					JSONNode jSONNode2 = node["url"];
					if (!jSONNode.IsNull && !jSONNode2.IsNull && (double)jSONNode.Value == 200.0 && ((string)jSONNode2.Value).StartsWith("https://prpr-muse-dash.avosapps.us/oauth"))
					{
						m_Copy = GUIUtility.systemCopyBuffer;
						GUIUtility.systemCopyBuffer = null;
						browser.SendFrameCommand(BrowserNative.FrameCommand.SelectAll);
						browser.SendFrameCommand(BrowserNative.FrameCommand.Copy);
						string info = string.Empty;
						SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
						{
							string sid = JsonUtils.Deserialize<JObject>(info)["sid"].ToString();
							GUIUtility.systemCopyBuffer = m_Copy;
							Singleton<XDSDKManager>.instance.OnOSLoginSuccess(sid);
							goPlatform.SetActive(false);
							GetComponent<OnCustomEvent>().GetPlayables<Popup>()[0].OnShutButtonClick();
						}, () => !string.IsNullOrEmpty(info = GUIUtility.systemCopyBuffer));
					}
				}
			};
		}

		public override List<GameObject> SetSelectableObj()
		{
			List<GameObject> list = new List<GameObject>();
			m_Buttons.For(delegate(Button b)
			{
				list.Add(b.gameObject);
			});
			list.Add(browser.gameObject);
			return list;
		}

		public override void OnDisablePnl()
		{
			EventSystem.current.SetSelectedGameObject(deactivatePanel.transform.Find("Toggles/Account").gameObject);
		}
	}
}
