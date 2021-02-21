namespace Assets.Scripts.PeroTools.UI
{
	public class RateOurGame
	{
		private static bool m_IsInit;

		public static string GetMuseDashStoreUrl(bool review = true)
		{
			string text = string.Format("https://www.taptap.com/app/60809{0}", (!review) ? string.Empty : "/review");
			string text2 = "com.prpr.musedash";
			string text3 = "1361473095";
			return $"https://store.steampowered.com/app/774171/Muse_Dash/";
		}

		public static void Rate(string rateUrl, string title, string message, string rateButton, string postponeButton, string rejectButton = null, bool force = false)
		{
		}
	}
}
