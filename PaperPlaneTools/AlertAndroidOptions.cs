namespace PaperPlaneTools
{
	public class AlertAndroidOptions
	{
		public static bool DefaultCancelable = true;

		public bool Cancelable
		{
			get;
			set;
		}

		public AlertAndroidOptions()
		{
			Cancelable = DefaultCancelable;
		}
	}
}
