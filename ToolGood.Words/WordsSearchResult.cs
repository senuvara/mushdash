namespace ToolGood.Words
{
	public class WordsSearchResult
	{
		private int _hash = -1;

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

		public string Keyword
		{
			get;
			private set;
		}

		public int Index
		{
			get;
			private set;
		}

		public static WordsSearchResult Empty => new WordsSearchResult();

		internal WordsSearchResult(string keyword, int start, int end, int index)
		{
			Keyword = keyword;
			Success = true;
			End = end;
			Start = start;
			Index = index;
		}

		private WordsSearchResult()
		{
			Success = false;
			Start = 0;
			End = 0;
			Index = -1;
			Keyword = null;
		}

		public override int GetHashCode()
		{
			if (_hash == -1)
			{
				int num = Start << 5;
				num += End - Start;
				_hash = num << 1 + (Success ? 1 : 0);
			}
			return _hash;
		}

		public override string ToString()
		{
			return Start + "|" + Keyword;
		}
	}
}
