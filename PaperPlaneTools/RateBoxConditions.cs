namespace PaperPlaneTools
{
	public class RateBoxConditions
	{
		public bool RequireInternetConnection
		{
			get;
			set;
		}

		public int MinSessionCount
		{
			get;
			set;
		}

		public int MinCustomEventsCount
		{
			get;
			set;
		}

		public int DelayAfterInstallInSeconds
		{
			get;
			set;
		}

		public int DelayAfterLaunchInSeconds
		{
			get;
			set;
		}

		public int PostponeCooldownInSeconds
		{
			get;
			set;
		}

		public RateBoxConditions()
		{
			MinSessionCount = 0;
			MinCustomEventsCount = 0;
			DelayAfterInstallInSeconds = 0;
			DelayAfterLaunchInSeconds = 0;
			PostponeCooldownInSeconds = 79200;
			RequireInternetConnection = true;
		}
	}
}
