using Assets.Scripts.Common.XDSDK;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Actions;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using UnityEngine;

namespace Assets.Scripts.UI.NetworkDetect
{
	public class PnlNetDetector : MonoBehaviour
	{
		private static float m_CurTime;

		public static bool m_Detect;

		public static bool m_DetectLogin;

		private static bool m_Once;

		private float m_DetectTime = 120f;

		private string m_GameMain = "GameMain";

		private bool m_IsPoping;

		private Popup m_Popup;

		private Popup m_PopupLogin;

		private string m_CurSceneName;

		private void Start()
		{
		}

		private void NetworkDetect()
		{
			Singleton<XDSDKManager>.instance.IsRealName(delegate(bool isRealName, bool isLimit)
			{
				m_Detect = true;
				Singleton<DataManager>.instance["GameConfig"]["NoForceNetwork"].SetResult(false);
				if (isRealName && !isLimit)
				{
					m_Detect = false;
					Singleton<DataManager>.instance["GameConfig"]["NoForceNetwork"].SetResult(true);
					Singleton<DataManager>.instance.Save();
				}
			});
		}

		private void Update()
		{
			if (!m_Detect && !m_DetectLogin)
			{
				return;
			}
			if (m_IsPoping && Application.internetReachability != 0)
			{
				if (m_Detect)
				{
					m_Popup.OnShutButtonClick();
				}
				if (m_DetectLogin)
				{
					m_PopupLogin.OnShutButtonClick();
				}
				m_IsPoping = false;
				m_CurTime = 0f;
			}
			if (m_CurTime >= m_DetectTime && m_Detect)
			{
				m_CurTime = 0f;
				if (m_CurSceneName != m_GameMain)
				{
					Detect();
				}
			}
			if (m_CurSceneName != m_GameMain && m_DetectLogin)
			{
				DetectLogin();
			}
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				m_CurTime += Time.deltaTime;
			}
			else
			{
				m_CurTime = 0f;
			}
		}

		public void Detect()
		{
			if (m_Detect && !Singleton<DataManager>.instance["GameConfig"]["NoForceNetwork"].GetResult<bool>() && Application.internetReachability == NetworkReachability.NotReachable && !m_IsPoping)
			{
				Singleton<EventManager>.instance.Invoke("UI/OnNetworkLost");
				m_IsPoping = true;
			}
		}

		public void DetectLogin()
		{
			if (m_DetectLogin && (!Singleton<XDSDKManager>.instance.IsLoggedIn() || !Singleton<XDSDKManager>.instance.IsXDLoggedIn()) && Application.internetReachability == NetworkReachability.NotReachable && !m_IsPoping)
			{
				Singleton<EventManager>.instance.Invoke("UI/OnNetworkLostLogin");
				m_IsPoping = true;
			}
		}
	}
}
