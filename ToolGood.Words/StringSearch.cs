using System.Collections.Generic;
using System.Text;
using ToolGood.Words.internals;

namespace ToolGood.Words
{
	public class StringSearch : BaseSearch
	{
		public string FindFirst(string text)
		{
			TrieNode trieNode = null;
			foreach (char c in text)
			{
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
					return node.Results[0];
				}
				trieNode = node;
			}
			return null;
		}

		public List<string> FindAll(string text)
		{
			TrieNode trieNode = null;
			List<string> list = new List<string>();
			foreach (char c in text)
			{
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
						list.Add(result);
					}
				}
				trieNode = node;
			}
			return list;
		}

		public bool ContainsAny(string text)
		{
			TrieNode trieNode = null;
			foreach (char c in text)
			{
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
					return true;
				}
				trieNode = node;
			}
			return false;
		}

		public string Replace(string text, char replaceChar = '*')
		{
			StringBuilder stringBuilder = new StringBuilder(text);
			TrieNode trieNode = null;
			for (int i = 0; i < text.Length; i++)
			{
				TrieNode node;
				if (trieNode == null)
				{
					node = _first[text[i]];
				}
				else if (!trieNode.TryGetValue(text[i], out node))
				{
					node = _first[text[i]];
				}
				if (node != null && node.End)
				{
					int length = node.Results[0].Length;
					int num = i + 1 - length;
					for (int j = num; j <= i; j++)
					{
						stringBuilder[j] = replaceChar;
					}
				}
				trieNode = node;
			}
			return stringBuilder.ToString();
		}
	}
}
