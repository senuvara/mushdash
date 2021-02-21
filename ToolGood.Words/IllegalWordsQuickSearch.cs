using System;
using System.Collections.Generic;
using System.Text;
using ToolGood.Words.internals;

namespace ToolGood.Words
{
	public class IllegalWordsQuickSearch : IllegalWordsSearch
	{
		public IllegalWordsQuickSearch(int jumpLength = 1)
			: base(jumpLength)
		{
		}

		public override bool ContainsAny(string text)
		{
			bool r = false;
			search(text, delegate(string keyword, char ch, int end)
			{
				r = !isInEnglishOrInNumber(keyword, ch, end, text);
				return r;
			});
			if (r)
			{
				return true;
			}
			string searchText = WordsHelper.ToSenseIllegalWords(text);
			search(searchText, delegate(string keyword, char ch, int end)
			{
				r = !isInEnglishOrInNumber(keyword, ch, end, searchText);
				return r;
			});
			if (r)
			{
				return true;
			}
			searchText = WordsHelper.RemoveNontext(searchText);
			search(searchText, delegate(string keyword, char ch, int end)
			{
				r = !isInEnglishOrInNumber(keyword, ch, end, searchText);
				return r;
			});
			return r;
		}

		public override IllegalWordsSearchResult FindFirst(string text)
		{
			IllegalWordsSearchResult result = null;
			search(text, delegate(string keyword, char ch, int end)
			{
				int start2 = end + 1 - keyword.Length;
				result = GetIllegalResult(keyword, ch, start2, end, text, text);
				return result != null;
			});
			if (result != null)
			{
				return result;
			}
			string searchText = WordsHelper.ToSenseIllegalWords(text);
			search(searchText, delegate(string keyword, char ch, int end)
			{
				int start = end + 1 - keyword.Length;
				result = GetIllegalResult(keyword, ch, start, end, text, searchText);
				return result != null;
			});
			if (result != null)
			{
				return result;
			}
			searchText = WordsHelper.RemoveNontext(searchText);
			search(searchText, delegate(string keyword, char ch, int end)
			{
				int num = end;
				for (int i = 0; i < keyword.Length; i++)
				{
					for (char c = searchText[num--]; c == '\u0001'; c = searchText[num--])
					{
					}
				}
				num++;
				result = GetIllegalResult(keyword, ch, num, end, text, searchText);
				return result != null;
			});
			if (result != null)
			{
				return result;
			}
			return IllegalWordsSearchResult.Empty;
		}

		public override List<IllegalWordsSearchResult> FindAll(string text)
		{
			List<IllegalWordsSearchResult> newlist = new List<IllegalWordsSearchResult>();
			string searchText = WordsHelper.ToSenseIllegalWords(text);
			searchAll(searchText, delegate(string keyword, char ch, int end)
			{
				int start = end + 1 - keyword.Length;
				IllegalWordsSearchResult illegalResult2 = GetIllegalResult(keyword, ch, start, end, text, searchText);
				if (illegalResult2 != null)
				{
					newlist.Add(illegalResult2);
				}
			});
			searchText = removeChecks(searchText, newlist);
			searchText = WordsHelper.RemoveNontext(searchText);
			searchAll(searchText, delegate(string keyword, char ch, int end)
			{
				int num = end;
				for (int i = 0; i < keyword.Length; i++)
				{
					for (char c = searchText[num--]; c == '\u0001'; c = searchText[num--])
					{
					}
				}
				num++;
				IllegalWordsSearchResult illegalResult = GetIllegalResult(keyword, ch, num, end, text, searchText);
				if (illegalResult != null)
				{
					newlist.Add(illegalResult);
				}
			});
			return newlist;
		}

		private IllegalWordsSearchResult GetIllegalResult(string keyword, char ch, int start, int end, string srcText, string searchText)
		{
			if (end < searchText.Length - 1 && ch < '\u007f')
			{
				char c = searchText[end + 1];
				if (c < '\u007f')
				{
					int num = "00000000000000000000000000000000000000000000000011111111110000000aaaaaaaaaaaaaaaaaaaaaaaaaa000000aaaaaaaaaaaaaaaaaaaaaaaaaa00000"[c] + "00000000000000000000000000000000000000000000000011111111110000000aaaaaaaaaaaaaaaaaaaaaaaaaa000000aaaaaaaaaaaaaaaaaaaaaaaaaa00000"[ch];
					if (num == 98 || num == 194)
					{
						return null;
					}
				}
			}
			if (start > 0)
			{
				char c2 = searchText[start - 1];
				if (c2 < '\u007f')
				{
					char c3 = keyword[0];
					if (c3 < '\u007f')
					{
						int num2 = "00000000000000000000000000000000000000000000000011111111110000000aaaaaaaaaaaaaaaaaaaaaaaaaa000000aaaaaaaaaaaaaaaaaaaaaaaaaa00000"[c2] + "00000000000000000000000000000000000000000000000011111111110000000aaaaaaaaaaaaaaaaaaaaaaaaaa000000aaaaaaaaaaaaaaaaaaaaaaaaaa00000"[c3];
						if (num2 == 98 || num2 == 194)
						{
							return null;
						}
					}
				}
			}
			return new IllegalWordsSearchResult(keyword, start, end, srcText);
		}

		private void search(string text, Func<string, char, int, bool> func)
		{
			TrieNode trieNode = null;
			int num = 0;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '\u0001')
				{
					num++;
					if (num > _jumpLength)
					{
						num = 0;
						trieNode = null;
					}
					continue;
				}
				TrieNode node;
				if (trieNode == null)
				{
					node = _first[c];
				}
				else if (!trieNode.TryGetValue(c, out node))
				{
					node = _first[c];
				}
				if (node != null && node.End)
				{
					foreach (string result in node.Results)
					{
						if (func(result, c, i))
						{
							return;
						}
					}
				}
				trieNode = node;
			}
		}

		private void searchAll(string text, Action<string, char, int> action)
		{
			TrieNode trieNode = null;
			int num = 0;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (c == '\u0001')
				{
					num++;
					if (num > _jumpLength)
					{
						num = 0;
						trieNode = null;
					}
					continue;
				}
				num = 0;
				if (c == '\0')
				{
					trieNode = null;
					continue;
				}
				TrieNode node;
				if (trieNode == null)
				{
					node = _first[c];
				}
				else if (!trieNode.TryGetValue(c, out node))
				{
					node = _first[c];
				}
				if (node != null && node.End)
				{
					foreach (string result in node.Results)
					{
						action(result, c, i);
					}
				}
				trieNode = node;
			}
		}

		private string removeChecks(string text, List<IllegalWordsSearchResult> results)
		{
			StringBuilder stringBuilder = new StringBuilder(text);
			foreach (IllegalWordsSearchResult result in results)
			{
				for (int i = result.Start; i <= result.End; i++)
				{
					stringBuilder[i] = '\0';
				}
			}
			return stringBuilder.ToString();
		}
	}
}
