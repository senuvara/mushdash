using System;
using System.Collections.Generic;

namespace PaperPlaneTools
{
	public class AlertManager
	{
		private IAlertPlatformAdapter currentAdapter;

		private Alert currentAlert;

		private List<Alert> queue = new List<Alert>();

		private static AlertManager instance;

		public static AlertManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new AlertManager();
				}
				return instance;
			}
		}

		public Func<IAlertPlatformAdapter> AlertFactory
		{
			get;
			set;
		}

		private AlertManager()
		{
		}

		public void Show(Alert alert)
		{
			queue.Add(alert);
			ShowNext();
		}

		public void Dismiss(Alert alert)
		{
			if (currentAlert == alert)
			{
				currentAdapter.Dismiss();
				return;
			}
			int num = queue.IndexOf(alert);
			if (num >= 0)
			{
				queue.RemoveAt(num);
				if (alert.OnDismiss != null)
				{
					alert.OnDismiss();
				}
			}
		}

		public void HandleEvent(string eventName, string value)
		{
			if (currentAlert != null)
			{
				currentAdapter.HandleEvent(eventName, value);
			}
		}

		private IAlertPlatformAdapter CreateAdapter()
		{
			return (AlertFactory == null) ? null : AlertFactory();
		}

		private void OnDismiss()
		{
			currentAdapter = null;
			currentAlert = null;
			ShowNext();
		}

		private void ShowNext()
		{
			if (currentAdapter == null && queue.Count > 0)
			{
				currentAlert = queue[0];
				queue.RemoveAt(0);
				if (currentAlert != null)
				{
					currentAdapter = ((currentAlert.Adapter == null) ? CreateAdapter() : currentAlert.Adapter);
					currentAdapter.SetOnDismiss(OnDismiss);
					currentAdapter.Show(currentAlert);
				}
			}
		}
	}
}
