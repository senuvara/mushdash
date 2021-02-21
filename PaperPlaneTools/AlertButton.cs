using System;

namespace PaperPlaneTools
{
	public class AlertButton
	{
		public string Title
		{
			get;
			private set;
		}

		public Action Handler
		{
			get;
			private set;
		}

		public AlertButton(string title, Action handler)
		{
			Title = title;
			Handler = handler;
		}
	}
}
