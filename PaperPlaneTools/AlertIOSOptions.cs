namespace PaperPlaneTools
{
	public class AlertIOSOptions
	{
		public static AlertIOSButton.Type DefaultPositiveButton = AlertIOSButton.Type.Default;

		public static AlertIOSButton.Type DefaultNegativeButton = AlertIOSButton.Type.Default;

		public static AlertIOSButton.Type DefaultNeutralButton = AlertIOSButton.Type.Default;

		public static Alert.ButtonType DefaultPreferableButton = Alert.ButtonType.Positive;

		public static Alert.ButtonType[] DefaultButtonsAddOrder = new Alert.ButtonType[3]
		{
			Alert.ButtonType.Positive,
			Alert.ButtonType.Neutral,
			Alert.ButtonType.Negative
		};

		public AlertIOSButton.Type PositiveButton
		{
			get;
			set;
		}

		public AlertIOSButton.Type NegativeButton
		{
			get;
			set;
		}

		public AlertIOSButton.Type NeutralButton
		{
			get;
			set;
		}

		public Alert.ButtonType PreferableButton
		{
			get;
			set;
		}

		public Alert.ButtonType[] ButtonsAddOrder
		{
			get;
			set;
		}

		public AlertIOSOptions()
		{
			PositiveButton = DefaultPositiveButton;
			NegativeButton = DefaultNegativeButton;
			NeutralButton = DefaultNeutralButton;
			PreferableButton = DefaultPreferableButton;
			ButtonsAddOrder = DefaultButtonsAddOrder;
		}
	}
}
