using System;

namespace PaperPlaneTools
{
	public interface IAlertPlatformAdapter
	{
		void SetOnDismiss(Action action);

		void Show(Alert alert);

		void Dismiss();

		void HandleEvent(string name, string value);
	}
}
