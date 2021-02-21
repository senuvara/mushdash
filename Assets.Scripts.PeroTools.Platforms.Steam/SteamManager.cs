using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Platforms.Steam
{
	[DisallowMultipleComponent]
	public class SteamManager : SingletonMonoBehaviour<SteamManager>
	{
		public delegate void OverlayActivated_delegate();

		private static bool m_EverInitialized;

		private bool m_Initialized;

		private SteamAPIWarningMessageHook_t m_SteamAPIWarningMessageHook;

		private Callback<GetAuthSessionTicketResponse_t> m_GetAuthSessionTicketResponse;

		private Callback<GameOverlayActivated_t> m_OnSteamOverlayActivated;

		private byte[] m_Ticket;

		private uint m_TicketCount;

		public OverlayActivated_delegate onOverlayActivated;

		private CallResult<LeaderboardFindResult_t> m_FindOrCreateLeaderboardUploadCallResult;

		private CallResult<LeaderboardScoreUploaded_t> m_UploadLeaderboardScoreCallResult;

		private Action<LeaderboardScoreUploaded_t> m_OnLeaderboardScoreUploaded = delegate
		{
		};

		private int m_Score;

		private float m_Accuracy;

		private CallResult<LeaderboardFindResult_t> m_FindOrCreateLeaderboardDownloadCallResult;

		private CallResult<LeaderboardScoresDownloaded_t> m_DownloadLeaderboardEntriesCallResult;

		private Action<LeaderboardScoresDownloaded_t> m_OnLeaderBoardScoresDownloaded = delegate
		{
		};

		public static bool Initialized => SingletonMonoBehaviour<SteamManager>.instance.m_Initialized;

		private static void SteamAPIDebugTextHook(int nSeverity, StringBuilder pchDebugText)
		{
			Debug.LogWarning(pchDebugText);
		}

		private void Init()
		{
			if (m_EverInitialized)
			{
				throw new Exception("Tried to Initialize the SteamAPI twice in one session!");
			}
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			if (!Packsize.Test())
			{
				Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.", this);
			}
			if (!DllCheck.Test())
			{
				Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.", this);
			}
			m_Initialized = SteamAPI.Init();
			if (!m_Initialized)
			{
				Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.", this);
				return;
			}
			m_EverInitialized = true;
			m_GetAuthSessionTicketResponse = Callback<GetAuthSessionTicketResponse_t>.Create(OnGetAuthSessionTicketResponse);
			DLCVertify();
			m_FindOrCreateLeaderboardUploadCallResult = CallResult<LeaderboardFindResult_t>.Create(OnFindOrCreateLeaderboardUpload);
			m_UploadLeaderboardScoreCallResult = CallResult<LeaderboardScoreUploaded_t>.Create(OnUploadLeaderboardScore);
			m_FindOrCreateLeaderboardDownloadCallResult = CallResult<LeaderboardFindResult_t>.Create(OnFindOrCreateLeaderboardDownload);
			m_DownloadLeaderboardEntriesCallResult = CallResult<LeaderboardScoresDownloaded_t>.Create(OnDownloadLeaderboardEntries);
			m_OnSteamOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnSteamOverlayActivated);
			SingletonMonoBehaviour<UnityGameManager>.instance.onApplicationFocusChange += FocusChange;
		}

		private void OnEnable()
		{
			if (m_Initialized && m_SteamAPIWarningMessageHook == null)
			{
				m_SteamAPIWarningMessageHook = SteamAPIDebugTextHook;
				SteamClient.SetWarningMessageHook(m_SteamAPIWarningMessageHook);
			}
		}

		private void OnDestroy()
		{
			if (m_Initialized)
			{
				SteamAPI.Shutdown();
			}
		}

		private void Update()
		{
			if (m_Initialized)
			{
				SteamAPI.RunCallbacks();
			}
		}

		public void DLCVertify()
		{
			if (SteamApps.BIsDlcInstalled(new AppId_t(1055810u)))
			{
				Singleton<DataManager>.instance["IAP"]["unlockall_0"].SetResult(true);
			}
		}

		private void OnGetAuthSessionTicketResponse(GetAuthSessionTicketResponse_t pCallback)
		{
			Debug.Log(string.Concat("[SteamManager]: [", 163, " - GetAuthSessionTicketResponse] - ", pCallback.m_hAuthTicket, " -- ", pCallback.m_eResult));
			byte[] array = new byte[m_TicketCount];
			for (int i = 0; i < m_TicketCount; i++)
			{
				array[i] = m_Ticket[i];
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("key", "BFEC643D79E480800796411D435CFF7D");
			dictionary.Add("appid", "774171");
			dictionary.Add("ticket", array);
			Dictionary<string, object> datas = dictionary;
			WebUtils.SendToUrl("https://api.steampowered.com/ISteamUserAuth/AuthenticateUserTicket/v1/", "GET", datas, delegate
			{
			});
		}

		public static bool IsOversea()
		{
			string iPCountry = SteamUtils.GetIPCountry();
			Debug.Log("[SteamManager]: Current Country IP: " + iPCountry);
			return iPCountry != "CN";
		}

		public void UploadScore(string musicID, int score, float accuracy, Action<LeaderboardScoreUploaded_t> callback)
		{
			m_Score = score;
			m_Accuracy = accuracy;
			SteamAPICall_t hAPICall = SteamUserStats.FindOrCreateLeaderboard(musicID, ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric);
			m_FindOrCreateLeaderboardUploadCallResult.Set(hAPICall);
			m_OnLeaderboardScoreUploaded = callback;
		}

		private void OnFindOrCreateLeaderboardUpload(LeaderboardFindResult_t callBack, bool IOFailure)
		{
			if (callBack.m_bLeaderboardFound == 1)
			{
				SteamAPICall_t hAPICall = SteamUserStats.UploadLeaderboardScore(pScoreDetails: new int[1]
				{
					(int)(m_Accuracy * 100f)
				}, hSteamLeaderboard: callBack.m_hSteamLeaderboard, eLeaderboardUploadScoreMethod: ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodForceUpdate, nScore: m_Score, cScoreDetailsCount: 1);
				m_UploadLeaderboardScoreCallResult.Set(hAPICall);
			}
			else
			{
				Debug.LogError("FindOrCreateLeaderboard Failure!!!(失败)");
			}
		}

		private void OnUploadLeaderboardScore(LeaderboardScoreUploaded_t callBack, bool IOFailure)
		{
			m_OnLeaderboardScoreUploaded(callBack);
		}

		public void GetLeaderboardData(string musicID, Action<LeaderboardScoresDownloaded_t> callback)
		{
			m_DownloadLeaderboardEntriesCallResult.Cancel();
			m_FindOrCreateLeaderboardDownloadCallResult.Cancel();
			SteamAPICall_t hAPICall = SteamUserStats.FindOrCreateLeaderboard(musicID, ELeaderboardSortMethod.k_ELeaderboardSortMethodDescending, ELeaderboardDisplayType.k_ELeaderboardDisplayTypeNumeric);
			m_FindOrCreateLeaderboardDownloadCallResult.Set(hAPICall);
			m_OnLeaderBoardScoresDownloaded = callback;
		}

		private void OnFindOrCreateLeaderboardDownload(LeaderboardFindResult_t callBack, bool IOFailure)
		{
			if (callBack.m_bLeaderboardFound == 1)
			{
				SteamAPICall_t hAPICall = SteamUserStats.DownloadLeaderboardEntries(callBack.m_hSteamLeaderboard, ELeaderboardDataRequest.k_ELeaderboardDataRequestGlobal, 1, 100);
				m_DownloadLeaderboardEntriesCallResult.Set(hAPICall);
			}
			else
			{
				Debug.LogError("FindOrCreateLeaderboard Failure!!!");
			}
		}

		private void OnDownloadLeaderboardEntries(LeaderboardScoresDownloaded_t callBack, bool IOFailure)
		{
			m_OnLeaderBoardScoresDownloaded(callBack);
		}

		private void FocusChange(bool hasFocus)
		{
			if (!hasFocus && onOverlayActivated != null)
			{
				onOverlayActivated();
			}
		}

		private void OnSteamOverlayActivated(GameOverlayActivated_t callBack)
		{
			Debug.Log("Steam Overlay " + callBack.m_bActive);
			if (callBack.m_bActive == 1 && onOverlayActivated != null)
			{
				onOverlayActivated();
			}
		}
	}
}
