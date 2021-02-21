namespace ToolGood.Words
{
	public class PinYinSearchResult
	{
		public string Keyword
		{
			get;
			private set;
		}

		public int Id
		{
			get;
			private set;
		}

		public PinYinSearchResult(string keyword, int id)
		{
			Keyword = keyword;
			Id = id;
		}
	}
}
