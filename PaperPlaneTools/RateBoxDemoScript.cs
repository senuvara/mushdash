using System;
using UnityEngine;
using UnityEngine.UI;

namespace PaperPlaneTools
{
	public class RateBoxDemoScript : MonoBehaviour
	{
		public Text infoText;

		private void Update()
		{
			infoText.text = GetDebugText();
		}

		public void OnButtonShow()
		{
			RateBox.Instance.Show();
		}

		public void OnButtonForceShow()
		{
			RateBox.Instance.ForceShow();
		}

		private string GetDebugText()
		{
			DateTime d = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Utc);
			int num = (int)Math.Floor((DateTime.UtcNow - d).TotalSeconds);
			RateBoxStatistics statistics = RateBox.Instance.Statistics;
			RateBoxConditions conditions = RateBox.Instance.Conditions;
			if (conditions == null)
			{
				return "RateBox.Instance.Conditions was not called";
			}
			string empty = string.Empty;
			empty = empty + "App version: " + Application.version + "\n";
			int num2 = statistics.AppInstallAt + conditions.DelayAfterInstallInSeconds - num;
			empty = empty + "Install cooldown: " + ((num2 <= 0) ? "<color=green>OK</color>" : $"<color=red>wait {num2} sec.</color>") + "\n";
			num2 = statistics.AppLaunchAt + conditions.DelayAfterLaunchInSeconds - num;
			empty = empty + "Launch cooldown: " + ((num2 <= 0) ? "<color=green>OK</color>" : $"<color=red>wait {num2} sec.</color>") + "\n";
			num2 = statistics.DialogShownAt + conditions.PostponeCooldownInSeconds - num;
			empty = empty + "Demonstartion cooldown: " + ((num2 <= 0) ? "<color=green>OK</color>" : $"<color=red>wait {num2} sec.</color>") + "\n";
			empty = empty + "Internet connection: " + ((Application.internetReachability != 0) ? "<color=green>OK</color>" : ((!conditions.RequireInternetConnection) ? "<color=green>OK (no Internet)</color>" : "<color=red>Failed (no Internet)</color>")) + "\n";
			empty = empty + "Dialog rejected: " + ((!statistics.DialogIsRejected) ? "<color=green>OK (not rejected)</color>" : "<color=red>Failed (rejected)</color>") + "\n";
			empty = empty + "Rated: " + ((!statistics.DialogIsRated) ? "<color=green>OK (not rated)</color>" : "<color=red>Failed (already rated)</color>") + "\n";
			num2 = conditions.MinSessionCount - statistics.SessionsCount;
			empty = empty + "Sessions: " + ((num2 <= 0) ? "<color=green>OK</color>" : $"<color=red>wait {num2} more sesssions</color>") + "\n";
			num2 = conditions.MinCustomEventsCount - statistics.CustomEventCount;
			empty = empty + "Custom events: " + ((num2 <= 0) ? "<color=green>OK</color>" : $"<color=red>wait {num2} more events</color>") + "\n";
			return empty + "Url: " + RateBox.Instance.RateUrl + "\n";
		}
	}
}
