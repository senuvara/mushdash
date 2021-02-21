using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.UI.Controls;
using FormulaBase;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
	public class PnlRank : MonoBehaviour
	{
		public Transform parent;

		public GameObject rankCell;

		public GameObject loading;

		public GameObject loadingTipSwitch;

		public GameObject noNet;

		public GameObject scrollView;

		public GameObject noRank;

		public GameObject repairing;

		public GameObject tooFast;

		public Button refresh;

		public GameObject serverBusyTip;

		public GameObject logoutTip;

		[Header("Server")]
		public GameObject server;

		public Text txtServerRank;

		public Text txtServerName;

		public Text txtServerAcc;

		public Text txtServerScore;

		private readonly Dictionary<string, JToken> m_Ranks = new Dictionary<string, JToken>();

		private readonly Dictionary<string, JToken> m_SelfRank = new Dictionary<string, JToken>();

		public void Refresh(bool force = false)
		{
			if (!base.gameObject.activeInHierarchy)
			{
				return;
			}
			server.SetActive(false);
			noNet.SetActive(Application.internetReachability == NetworkReachability.NotReachable);
			scrollView.SetActive(false);
			noRank.SetActive(false);
			if ((bool)repairing)
			{
				repairing.SetActive(false);
			}
			if ((bool)tooFast)
			{
				tooFast.SetActive(false);
			}
			if ((bool)serverBusyTip)
			{
				serverBusyTip.SetActive(false);
			}
			if ((bool)logoutTip)
			{
				logoutTip.SetActive(false);
			}
			string result = Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>();
			int hideBMSDifficulty = Singleton<StageBattleComponent>.instance.GetHideBMSDifficulty();
			int num = int.Parse(result.Substring(0, 1)) * 100 + int.Parse(result.Substring(2, result.Length - 2));
			loading.SetActive(true);
			string uid = $"{result}_{hideBMSDifficulty}";
			if (m_Ranks.ContainsKey(uid) && !force)
			{
				loading.SetActive(false);
				UIRefresh(uid);
				return;
			}
			Singleton<ServerManager>.instance.GetRanks(result, hideBMSDifficulty, delegate(JToken token, JToken selfRank, int code)
			{
				if ((bool)this)
				{
					server.SetActive(false);
					noNet.SetActive(Application.internetReachability == NetworkReachability.NotReachable);
					scrollView.SetActive(false);
					noRank.SetActive(false);
					if ((bool)repairing)
					{
						repairing.SetActive(false);
					}
					if ((bool)tooFast)
					{
						tooFast.SetActive(false);
					}
					m_SelfRank[uid] = selfRank;
					m_Ranks[uid] = token;
					if (code == 300)
					{
						m_SelfRank.Remove(uid);
						m_Ranks.Remove(uid);
					}
					loading.SetActive(false);
					UIRefresh(uid);
					if (code == 300)
					{
						noRank.SetActive(false);
					}
					if ((bool)repairing)
					{
						repairing.SetActive(code == 300);
					}
					if ((bool)tooFast)
					{
						tooFast.SetActive(code == 429);
					}
				}
			}, delegate
			{
				if ((bool)this)
				{
					noNet.SetActive(true);
					loading.SetActive(false);
				}
			});
		}

		private void NsRefreshFail(NexResultDescription.ErrorType errorType, string uid)
		{
			if ((bool)refresh)
			{
				refresh.interactable = true;
			}
			string result = Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>();
			int result2 = Singleton<DataManager>.instance["Account"]["SelectedDifficulty"].GetResult<int>();
			string a = $"{result}_{result2}";
			if ((bool)this && !(a != uid))
			{
				loading.SetActive(false);
				scrollView.SetActive(false);
				if (errorType == NexResultDescription.ErrorType.NoNet && (bool)noNet)
				{
					noNet.SetActive(true);
				}
				else if (errorType == NexResultDescription.ErrorType.Busy && (bool)serverBusyTip)
				{
					serverBusyTip.SetActive(true);
				}
				else if (errorType == NexResultDescription.ErrorType.Logout && (bool)logoutTip)
				{
					logoutTip.SetActive(true);
				}
			}
		}

		private void NsRefreshSuccess(JToken token, JObject selfRank, string uid)
		{
			m_SelfRank[uid] = selfRank;
			m_Ranks[uid] = token;
			string result = Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>();
			int result2 = Singleton<DataManager>.instance["Account"]["SelectedDifficulty"].GetResult<int>();
			string a = $"{result}_{result2}";
			if ((bool)this && !(a != uid))
			{
				if ((bool)refresh)
				{
					refresh.interactable = true;
				}
				loading.SetActive(false);
				UIRefresh(uid);
			}
		}

		private void UIRefresh(string uid)
		{
			JToken jToken = (!m_SelfRank.ContainsKey(uid)) ? null : m_SelfRank[uid];
			if (jToken != null)
			{
				JToken jToken2 = jToken["detail"];
				JToken jToken3 = jToken["order"];
				if (!jToken3.IsNullOrEmpty())
				{
					server.SetActive(true);
					int num = (int)jToken3;
					txtServerName.text = Singleton<DataManager>.instance["Account"]["PlayerName"].GetResult<string>();
					txtServerRank.text = ((num >= 0 && num <= 998) ? (num + 1).ToString("00") : "999+");
					if (!jToken2.IsNullOrEmpty())
					{
						txtServerAcc.text = ((float)jToken2["acc"] / 100f).ToString("p2");
						txtServerScore.text = jToken2["score"].ToString();
					}
				}
				else
				{
					server.SetActive(false);
				}
			}
			else
			{
				server.SetActive(false);
			}
			JToken jToken4 = (!m_Ranks.ContainsKey(uid)) ? null : m_Ranks[uid];
			int num2 = 0;
			for (int i = 0; i < parent.childCount; i++)
			{
				Object.Destroy(parent.GetChild(i).gameObject);
			}
			if (jToken4 != null)
			{
				num2 = jToken4.Count();
				for (int j = 0; j < num2; j++)
				{
					int number = j + 1;
					JToken jToken5 = jToken4[j];
					string nickName = (string)jToken5["user"]["nickname"];
					if (!jToken5["play"].IsNullOrEmpty())
					{
						int score = (int)jToken5["play"]["score"];
						float acc = (float)jToken5["play"]["acc"] / 100f;
						GameObject gameObject = Object.Instantiate(rankCell, parent);
						RankCell component = gameObject.GetComponent<RankCell>();
						component.SetValue(number, nickName, score, acc);
					}
				}
			}
			noRank.SetActive(num2 == 0 && Application.internetReachability != NetworkReachability.NotReachable);
			scrollView.SetActive(num2 != 0 && Application.internetReachability != NetworkReachability.NotReachable);
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				noRank.SetActive(false);
				scrollView.SetActive(false);
			}
		}
	}
}
