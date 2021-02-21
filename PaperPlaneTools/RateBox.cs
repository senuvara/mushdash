using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace PaperPlaneTools
{
	public class RateBox
	{
		private static string statisticsPath = "com.paperplanetools.ratebox.RateBoxStatistics.xml";

		private static RateBox instance;

		public static RateBox Instance
		{
			get
			{
				if (instance == null)
				{
					string path = Application.persistentDataPath + "/" + statisticsPath;
					RateBoxStatistics rateBoxStatistics = null;
					if (File.Exists(path))
					{
						try
						{
							XmlSerializer xmlSerializer = new XmlSerializer(typeof(RateBoxStatistics));
							FileStream fileStream = new FileStream(path, FileMode.Open);
							rateBoxStatistics = (xmlSerializer.Deserialize(fileStream) as RateBoxStatistics);
							fileStream.Close();
						}
						catch (Exception ex)
						{
							rateBoxStatistics = null;
							Debug.Log(ex.Message);
						}
					}
					if (rateBoxStatistics == null)
					{
						rateBoxStatistics = new RateBoxStatistics();
					}
					instance = new RateBox(rateBoxStatistics);
				}
				return instance;
			}
		}

		public bool DebugMode
		{
			get;
			set;
		}

		public string RateUrl
		{
			get;
			set;
		}

		public RateBoxConditions Conditions
		{
			get;
			set;
		}

		public RateBoxSettings Settings
		{
			get;
			set;
		}

		public RateBoxStatistics Statistics
		{
			get;
			private set;
		}

		public RateBoxTextSettings DefaultTextSettings
		{
			get;
			set;
		}

		public IAlertPlatformAdapter AlertAdapter
		{
			get;
			set;
		}

		private RateBox(RateBoxStatistics stat)
		{
			Statistics = stat;
			DebugMode = false;
		}

		public static string GetStoreUrl(string iTunesAppId, string googlePlayMarketAppBundleId)
		{
			return string.Empty;
		}

		public void Init(string rateUrl, RateBoxConditions conditions = null, RateBoxTextSettings textSettings = null, RateBoxSettings settings = null)
		{
			int num = Time();
			if (Statistics.ApplicationVersion == null || Statistics.ApplicationVersion != Application.version)
			{
				Statistics = new RateBoxStatistics();
				Statistics.ApplicationVersion = Application.version;
			}
			if (Statistics.AppInstallAt <= 0)
			{
				Statistics.AppInstallAt = num;
			}
			Statistics.AppLaunchAt = num;
			Statistics.SessionsCount++;
			Conditions = ((conditions != null) ? conditions : new RateBoxConditions());
			Settings = ((settings != null) ? settings : new RateBoxSettings());
			DefaultTextSettings = textSettings;
			RateUrl = rateUrl;
			SaveStatistics();
		}

		public void Show(string title, string message, string rateButtonTitle, string postponeButtonTilte, string rejectButtonTitle = null)
		{
			if (CheckConditionsAreMet())
			{
				Statistics.DialogShownAt = Time();
				SaveStatistics();
				ForceShow(title, message, rateButtonTitle, postponeButtonTilte, rejectButtonTitle);
			}
		}

		public void Show()
		{
			if (DefaultTextSettings == null)
			{
				DebugLog("Can't show a dialog because dialog strings are not configured. Please drag PaperPlaneTools/Resources/RateBox/RateBoxPrefab to your scene or set DefaultTextSettings manually.");
			}
			else
			{
				Show(DefaultTextSettings.Title, DefaultTextSettings.Message, DefaultTextSettings.RateButtonTitle, DefaultTextSettings.PostponeButtonTitle, DefaultTextSettings.RejectButtonTitle);
			}
		}

		public void ForceShow(string title, string message, string rateButtonTitle, string postponeButtonTilte, string rejectButtonTitle = null)
		{
			Alert alert = new Alert(title, message).SetPositiveButton(rateButtonTitle, delegate
			{
				GoToRateUrl();
			}).SetNeutralButton(postponeButtonTilte).AddOptions(new AlertIOSOptions
			{
				PreferableButton = Alert.ButtonType.Positive
			});
			if (rejectButtonTitle != null)
			{
				alert.SetNegativeButton(rejectButtonTitle, delegate
				{
					Statistics.DialogIsRejected = true;
					SaveStatistics();
				});
			}
			alert.SetAdapter(AlertAdapter);
			alert.Show();
		}

		public void ForceShow()
		{
			if (DefaultTextSettings == null)
			{
				DebugLog("Can't show a dialog because dialog strings are not configured. Please drag PaperPlaneTools/Resources/RateBox/RateBoxPrefab to your scene or set DefaultTextSettings manually.");
			}
			else
			{
				ForceShow(DefaultTextSettings.Title, DefaultTextSettings.Message, DefaultTextSettings.RateButtonTitle, DefaultTextSettings.PostponeButtonTitle, DefaultTextSettings.RejectButtonTitle);
			}
		}

		public void IncrementCustomCounter(int value = 1)
		{
			Statistics.CustomEventCount += value;
			SaveStatistics();
		}

		public bool CheckConditionsAreMet()
		{
			int num = Time();
			if (Conditions == null)
			{
				DebugLog("Conditions are NOT met because Init function is never called.");
				return false;
			}
			if (Statistics.DialogIsRejected)
			{
				DebugLog("Conditions are NOT met because dialog was rejected; DialogIsRejected == true, this flag will be cleared after new application version is detected.");
				return false;
			}
			if (Statistics.DialogIsRated)
			{
				DebugLog("Conditions are NOT met because user has already rate the app; DialogIsRated == true, this flag will be cleared after new application version is detected.");
				return false;
			}
			if (Statistics.SessionsCount < Conditions.MinSessionCount)
			{
				DebugLog($"Conditions.MinSessionCount is NOT met; {Statistics.SessionsCount} < {Conditions.MinSessionCount}. Session counter increases everytime Init function is called.");
				return false;
			}
			if (Statistics.CustomEventCount < Conditions.MinCustomEventsCount)
			{
				DebugLog($"Conditions.MinCustomEventsCount is NOT met; {Statistics.CustomEventCount} < {Conditions.MinCustomEventsCount}. Counter increases everytime IncrementCustomCounter function is called. ");
				return false;
			}
			if (Statistics.AppInstallAt + Conditions.DelayAfterInstallInSeconds > num)
			{
				DebugLog(string.Format("Conditions.DelayAfterInstallInSeconds is NOT met. Need to wait {3} more seconds. Install time = {0}, Conditions.DelayAfterInstallInSeconds = {1}, now = {2}", Statistics.AppInstallAt, Conditions.DelayAfterInstallInSeconds, num, Statistics.AppInstallAt + Conditions.DelayAfterInstallInSeconds - num));
				return false;
			}
			if (Statistics.AppLaunchAt + Conditions.DelayAfterLaunchInSeconds > num)
			{
				DebugLog(string.Format("Conditions.DelayAfterLaunchInSeconds is not met. Need to wait {3} more seconds. Launch time = {0}, Conditions.DelayAfterLaunchInSeconds = {1}, now = {2}", Statistics.AppLaunchAt, Conditions.DelayAfterLaunchInSeconds, num, Statistics.AppLaunchAt + Conditions.DelayAfterLaunchInSeconds - num));
				return false;
			}
			if (Statistics.DialogShownAt + Conditions.PostponeCooldownInSeconds > num)
			{
				DebugLog(string.Format("Conditions.PostponeCooldownInSeconds is NOT met. Need to wait {3} more seconds. Show time = {0}, Conditions.PostponeCooldownInSeconds = {1}, now = {2}", Statistics.DialogShownAt, Conditions.PostponeCooldownInSeconds, num, Statistics.DialogShownAt + Conditions.PostponeCooldownInSeconds - num));
				return false;
			}
			if (Conditions.RequireInternetConnection && Application.internetReachability == NetworkReachability.NotReachable)
			{
				DebugLog($"Conditions.RequireInternetConnection is NOT met.");
				return false;
			}
			DebugLog("All conditions are met.");
			return true;
		}

		public void ClearStatistics()
		{
			Statistics = new RateBoxStatistics();
			SaveStatistics();
		}

		public bool SaveStatistics()
		{
			try
			{
				string path = Application.persistentDataPath + "/" + statisticsPath;
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(RateBoxStatistics));
				FileStream fileStream = new FileStream(path, FileMode.Create);
				xmlSerializer.Serialize(fileStream, Statistics);
				fileStream.Close();
			}
			catch (Exception ex)
			{
				Debug.Log(ex.Message);
				return false;
			}
			return true;
		}

		private int Time()
		{
			DateTime d = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);
			return (int)Math.Floor((DateTime.UtcNow - d).TotalSeconds);
		}

		private void GoToRateUrl()
		{
			Statistics.DialogIsRated = true;
			SaveStatistics();
			Application.OpenURL(RateUrl);
		}

		private void DebugLog(string str)
		{
			if (DebugMode)
			{
				Debug.Log("RateBox: " + str);
			}
		}
	}
}
