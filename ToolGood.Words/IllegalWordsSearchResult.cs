namespace ToolGood.Words
{
	public class IllegalWordsSearchResult
	{
		public bool Success
		{
			get;
			private set;
		}

		public int Start
		{
			get;
			private set;
		}

		public int End
		{
			get;
			private set;
		}

		public string SrcString
		{
			get;
			private set;
		}

		public string Keyword
		{
			get;
			private set;
		}

		public static IllegalWordsSearchResult Empty => new IllegalWordsSearchResult();

		internal IllegalWordsSearchResult(string keyword, int start, int end, string srcText)
		{
			Keyword = keyword;
			Success = true;
			End = end;
			Start = start;
			SrcString = srcText.Substring(Start, end - Start + 1);
		}

		private IllegalWordsSearchResult()
		{
			Success = false;
			Start = 0;
			End = 0;
			SrcString = null;
			Keyword = null;
		}

		public override string ToString()
		{
			return Start + "|" + SrcString;
		}
	}
}
