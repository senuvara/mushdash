using System;

namespace PaperPlaneTools
{
	public class AlertIOSButton
	{
		public enum Type
		{
			Default,
			Cancel,
			Destructive
		}

		public Type WhichButton
		{
			get;
			private set;
		}

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

		public bool IsPreferable
		{
			get;
			private set;
		}

		public AlertIOSButton(Type whichButton, string title, Action handler, bool isPreferable)
		{
			WhichButton = whichButton;
			Title = title;
			Handler = handler;
			IsPreferable = isPreferable;
		}
	}
}
