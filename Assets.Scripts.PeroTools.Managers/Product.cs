namespace Assets.Scripts.PeroTools.Managers
{
	public class Product
	{
		public bool isAvailable;

		public string id;

		public string title;

		public float price;

		public string currencySymbol;

		private string _localizedPrice;

		public string localizedPrice
		{
			get
			{
				if (string.IsNullOrEmpty(_localizedPrice))
				{
					return string.Format("{0}{1}", currencySymbol, price.ToString("f2"));
				}
				return _localizedPrice;
			}
			set
			{
				_localizedPrice = value;
			}
		}
	}
}
