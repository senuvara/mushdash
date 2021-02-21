using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolGood.Words
{
	public class WordsSearch
	{
		private class TrieNode
		{
			internal Dictionary<char, TrieNode> m_values;

			private uint minflag = uint.MaxValue;

			private uint maxflag;

			public bool End
			{
				get;
				set;
			}

			public List<KeyValuePair<string, int>> Results
			{
				get;
				set;
			}

			public TrieNode()
			{
				m_values = new Dictionary<char, TrieNode>();
				Results = new List<KeyValuePair<string, int>>();
			}

			public bool TryGetValue(char c, out TrieNode node)
			{
				if (minflag <= c && maxflag >= c)
				{
					return m_values.TryGetValue(c, out node);
				}
				node = null;
				return false;
			}

			public TrieNode Add(char c)
			{
				if (minflag > c)
				{
					minflag = c;
				}
				if (maxflag < c)
				{
					maxflag = c;
				}
				TrieNode value;
				if (m_values.TryGetValue(c, out value))
				{
					return value;
				}
				value = new TrieNode();
				m_values[c] = value;
				return value;
			}

			public void SetResults(string text, int index)
			{
				if (!End)
				{
					End = true;
				}
				Results.Add(new KeyValuePair<string, int>(text, index));
			}

			public void Merge(TrieNode node)
			{
				if (node.End)
				{
					if (!End)
					{
						End = true;
					}
					foreach (KeyValuePair<string, int> result in node.Results)
					{
						Results.Add(result);
					}
				}
				foreach (KeyValuePair<char, TrieNode> value in node.m_values)
				{
					if (!m_values.ContainsKey(value.Key))
					{
						if (minflag > value.Key)
						{
							minflag = value.Key;
						}
						if (maxflag < value.Key)
						{
							maxflag = value.Key;
						}
						m_values[value.Key] = value.Value;
					}
				}
			}
		}

		private TrieNode[] _first = new TrieNode[65536];

		public void SetKeywords(ICollection<string> keywords)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			int num = 0;
			foreach (string keyword in keywords)
			{
				dictionary[keyword] = num++;
			}
			SetKeywords(dictionary);
		}

		public void SetKeywords(ICollection<string> keywords, ICollection<int> indexs)
		{
			if (keywords.Count != indexs.Count)
			{
				throw new Exception("数量不一样");
			}
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			long num = 0L;
			int[] array = indexs.ToArray();
			foreach (string keyword in keywords)
			{
				dictionary[keyword] = array[num++];
			}
			SetKeywords(dictionary);
		}

		public void SetKeywords(IDictionary<string, int> keywords)
		{
			TrieNode[] array = new TrieNode[65536];
			TrieNode trieNode = new TrieNode();
			foreach (KeyValuePair<string, int> keyword in keywords)
			{
				string key = keyword.Key;
				if (!string.IsNullOrEmpty(key))
				{
					TrieNode trieNode2 = array[key[0]];
					if (trieNode2 == null)
					{
						trieNode2 = trieNode.Add(key[0]);
						array[key[0]] = trieNode2;
					}
					for (int i = 1; i < key.Length; i++)
					{
						trieNode2 = trieNode2.Add(key[i]);
					}
					trieNode2.SetResults(key, keyword.Value);
				}
			}
			_first = array;
			Dictionary<TrieNode, TrieNode> dictionary = new Dictionary<TrieNode, TrieNode>();
			foreach (KeyValuePair<char, TrieNode> value in trieNode.m_values)
			{
				TryLinks(value.Value, null, dictionary);
			}
			foreach (KeyValuePair<TrieNode, TrieNode> item in dictionary)
			{
				item.Key.Merge(item.Value);
			}
		}

		private void TryLinks(TrieNode node, TrieNode node2, Dictionary<TrieNode, TrieNode> links)
		{
			foreach (KeyValuePair<char, TrieNode> value in node.m_values)
			{
				TrieNode node3 = null;
				if (node2 == null)
				{
					node3 = _first[value.Key];
					if (node3 != null)
					{
						links[value.Value] = node3;
					}
				}
				else if (node2.TryGetValue(value.Key, out node3))
				{
					links[value.Value] = node3;
				}
				TryLinks(value.Value, node3, links);
			}
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

		public WordsSearchResult FindFirst(string text)
		{
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
					KeyValuePair<string, int> keyValuePair = node.Results[0];
					return new WordsSearchResult(keyValuePair.Key, i + 1 - keyValuePair.Key.Length, i, keyValuePair.Value);
				}
				trieNode = node;
			}
			return WordsSearchResult.Empty;
		}

		public List<WordsSearchResult> FindAll(string text)
		{
			TrieNode trieNode = null;
			List<WordsSearchResult> list = new List<WordsSearchResult>();
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
					foreach (KeyValuePair<string, int> result in node.Results)
					{
						list.Add(new WordsSearchResult(result.Key, i + 1 - result.Key.Length, i, result.Value));
					}
				}
				trieNode = node;
			}
			return list;
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
					int length = node.Results[0].Key.Length;
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
