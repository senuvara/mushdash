using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Common
{
	public class CloudDataChangeHandler : MonoBehaviour
	{
		[Space(10f)]
		public GameObject txtLocal;

		public GameObject txtCloud;

		[Space(10f)]
		public Button btnLocal;

		public Button btnCloud;

		public Button btnYes;

		public Button btnNo;

		[Space(10f)]
		public Text strLevelCloud;

		public Text strExpCloud;

		public Text strDateCloud;

		[Space(10f)]
		public Text strLevelLocal;

		public Text strExpLocal;

		[Space(10f)]
		public Text confirmLevelCloud;

		public Text confirmExpCloud;

		public Text confirmDateCloud;

		[Space(10f)]
		public Text confirmLevelLocal;

		public Text confirmExpLocal;

		private Action<bool> m_AndroidSyncCallback;

		public bool isLocal;

		private int m_TotleKey;

		private int m_DoneKey;

		private bool m_IsHanding;

		public static bool hasHandleCloudDataInit
		{
			get
			{
				return PlayerPrefs.GetInt("hasHandleCloudDataInit") != 0;
			}
			set
			{
				PlayerPrefs.SetInt("hasHandleCloudDataInit", value ? 1 : 0);
			}
		}

		private int CacularLevel(int exp)
		{
			int num = exp / 100 + 1;
			return (num <= 999) ? num : 999;
		}

		private int CacularCurExp(int exp)
		{
			return exp % 100;
		}

		public void Init(int cloudExp, string cloudSaveTime, int localExp, Action<bool> callback)
		{
			strExpCloud.text = CacularCurExp(cloudExp).ToString();
			confirmExpCloud.text = CacularCurExp(cloudExp).ToString();
			strLevelCloud.text = CacularLevel(cloudExp).ToString();
			confirmLevelCloud.text = CacularLevel(cloudExp).ToString();
			strDateCloud.text = cloudSaveTime;
			confirmDateCloud.text = cloudSaveTime;
			strExpLocal.text = CacularCurExp(localExp).ToString();
			confirmExpLocal.text = CacularCurExp(localExp).ToString();
			strLevelLocal.text = CacularLevel(localExp).ToString();
			confirmLevelLocal.text = CacularLevel(localExp).ToString();
			m_AndroidSyncCallback = callback;
			btnYes.onClick.AddListener(delegate
			{
				m_AndroidSyncCallback(isLocal);
			});
			btnLocal.onClick.AddListener(delegate
			{
				isLocal = true;
				txtCloud.SetActive(false);
				txtLocal.SetActive(true);
			});
			btnCloud.onClick.AddListener(delegate
			{
				isLocal = false;
				txtCloud.SetActive(true);
				txtLocal.SetActive(false);
			});
		}

		private void OnApplicationFocus(bool isFocus)
		{
			if (!isFocus && (base.transform.GetChild(0).gameObject.activeSelf || base.transform.GetChild(1).gameObject.activeSelf))
			{
				Debug.LogError("VortexBoy --- Logout!");
				m_AndroidSyncCallback(false);
			}
		}
	}
}
