using System;
using UnityEngine;

namespace PaperPlaneTools
{
	[Serializable]
	public class RateBoxStatistics
	{
		[SerializeField]
		private int sessionsCount;

		[SerializeField]
		private int customEventCount;

		[SerializeField]
		private int appInstallAt;

		[SerializeField]
		private int appLaunchAt;

		[SerializeField]
		private int dialogShownAt;

		[SerializeField]
		private bool dialogIsRejected;

		[SerializeField]
		private bool dialogIsRated;

		[SerializeField]
		private string applicationVersion;

		public int SessionsCount
		{
			get
			{
				return sessionsCount;
			}
			set
			{
				sessionsCount = value;
			}
		}

		public int CustomEventCount
		{
			get
			{
				return customEventCount;
			}
			set
			{
				customEventCount = value;
			}
		}

		public int AppInstallAt
		{
			get
			{
				return appInstallAt;
			}
			set
			{
				appInstallAt = value;
			}
		}

		public int AppLaunchAt
		{
			get
			{
				return appLaunchAt;
			}
			set
			{
				appLaunchAt = value;
			}
		}

		public int DialogShownAt
		{
			get
			{
				return dialogShownAt;
			}
			set
			{
				dialogShownAt = value;
			}
		}

		public bool DialogIsRejected
		{
			get
			{
				return dialogIsRejected;
			}
			set
			{
				dialogIsRejected = value;
			}
		}

		public bool DialogIsRated
		{
			get
			{
				return dialogIsRated;
			}
			set
			{
				dialogIsRated = value;
			}
		}

		public string ApplicationVersion
		{
			get
			{
				return applicationVersion;
			}
			set
			{
				applicationVersion = value;
			}
		}

		public RateBoxStatistics()
		{
			SessionsCount = 0;
			CustomEventCount = 0;
			AppInstallAt = 0;
			AppLaunchAt = 0;
			DialogShownAt = 0;
			DialogIsRejected = false;
			DialogIsRated = false;
			ApplicationVersion = null;
		}
	}
}
