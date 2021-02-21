using UnityEngine;

namespace PaperPlaneTools
{
	public class RateBoxPrefabScript : MonoBehaviour
	{
		[Header("DISPLAY CONDITIONS:")]
		[Tooltip("Minimum number of sessions before prompting a dialog.\nThe counter increases every time the game starts.")]
		public int minSessionCount;

		[Tooltip("Minimum number of a custom events before prompting a dialog.\nThe custom counter increases every time RateBox.IncrementCustomCounter function is called.")]
		public int minCustomEventsCount;

		[Tooltip("Number of hours to wait before prompting a dialog after the application was first time started.\n\nFor example, a value of 2.5 means that the user won't see a rate prompt at least first 2 and a half hours after the installation.")]
		public float delayAfterInstallInHours;

		[Tooltip("Number of hours to wait before prompting a dialog after the application was started. \n\nFor example, a value of 0.2 means that the user won't see a rate prompt at least first 12 minutes (0.2 * 60) after the applications starts.")]
		public float delayAfterLaunchInHours;

		[Tooltip("Number of hours to wait before prompting a dialog after the former one was displayed.\n\nFor example, a value of 12.5 means that the user won't see a rate prompt at least first 12 and a half hours after dismissing the former one.")]
		public float postponeCooldownInHours = 22f;

		[Tooltip("Check internet connection before prompting a dialog. This makes sense because user won't be able to rate the app without an internet connection.")]
		public bool requireInternetConnection = true;

		[Header("TEXT:")]
		[Tooltip("Title of the dialog")]
		public string title = "Like the game?";

		[Tooltip("Message of the dialog")]
		[Multiline]
		public string message = "Take a moment to rate us!";

		[Tooltip("Title of the rate-button")]
		public string rateButton = "Rate";

		[Tooltip("Title of the later-button")]
		public string postponeButton = "Later";

		[Tooltip("Title of the rate button.\nIf empty string, the button won't be displayed.")]
		public string rejectButton = string.Empty;

		[Header("RATE URLS:")]
		[Tooltip("Apple AppStore app id.\nThe url will be https://itunes.apple.com/app/id{iTunesAppId}")]
		public string appStoreAppId = string.Empty;

		[Tooltip("Your app bundle id for Google Playe Market.\nThe url will be https://play.google.com/store/apps/details?id={playMarketAppBundleId}")]
		public string playMarketAppBundleId = string.Empty;

		[Header("Settings:")]
		[Tooltip("Use new iOS SKStoreReviewController if available")]
		public bool useIOSReview;

		[Header("Custom window:")]
		[Tooltip("Show custom UI window instead of native alerts")]
		public GameObject customUIWindow;

		[Header("DEBUG (Unity Editor Only):")]
		[Tooltip("Clear statistics on start.\nSometimes you don't want RateBox to store statistics permanently, for instance, if you reject the prompt once, you will never see it again.\nThis behavior is not always desired when debugging, so by enabling 'Clear On Start' you can start from the blank state every time you launch the app in the Unity Editor.")]
		public bool clearOnStart;

		[Tooltip("Turn on debug to report conditions check log.\nThis will help to understand why a dialog doesn't appear after calling Show method.")]
		public bool logDebugMessages = true;

		private void Start()
		{
			string storeUrl = RateBox.GetStoreUrl(appStoreAppId, playMarketAppBundleId);
			RateBox.Instance.DebugMode = false;
			string text = rejectButton.Trim();
			RateBox.Instance.Init(storeUrl, new RateBoxConditions
			{
				MinSessionCount = minSessionCount,
				MinCustomEventsCount = minCustomEventsCount,
				DelayAfterInstallInSeconds = Mathf.CeilToInt(delayAfterInstallInHours * 3600f),
				DelayAfterLaunchInSeconds = Mathf.CeilToInt(delayAfterLaunchInHours * 3600f),
				PostponeCooldownInSeconds = Mathf.CeilToInt(postponeCooldownInHours * 3600f),
				RequireInternetConnection = requireInternetConnection
			}, new RateBoxTextSettings
			{
				Title = title.Trim(),
				Message = message.Trim(),
				RateButtonTitle = rateButton.Trim(),
				PostponeButtonTitle = postponeButton.Trim(),
				RejectButtonTitle = ((text.Length <= 0) ? null : text.Trim())
			}, new RateBoxSettings
			{
				UseIOSReview = useIOSReview
			});
			IAlertPlatformAdapter alertAdapter = null;
			if (customUIWindow != null)
			{
				customUIWindow.SetActive(false);
				alertAdapter = customUIWindow.GetComponent<IAlertPlatformAdapter>();
			}
			RateBox.Instance.AlertAdapter = alertAdapter;
		}
	}
}
