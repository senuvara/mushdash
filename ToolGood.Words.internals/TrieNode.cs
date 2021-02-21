using System.Collections.Generic;

namespace ToolGood.Words.internals
{
	public class TrieNode
	{
		internal Dictionary<char, TrieNode> m_values;

		private uint minflag = uint.MaxValue;

		private uint maxflag;

		public bool End
		{
			get;
			set;
		}

		public List<string> Results
		{
			get;
			private set;
		}

		public TrieNode()
		{
			m_values = new Dictionary<char, TrieNode>();
			Results = new List<string>();
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

		public ICollection<TrieNode> Transitions()
		{
			return m_values.Values;
		}

		public TrieNode Add(char c)
		{
			TrieNode value;
			if (m_values.TryGetValue(c, out value))
			{
				return value;
			}
			if (minflag > c)
			{
				minflag = c;
			}
			if (maxflag < c)
			{
				maxflag = c;
			}
			value = new TrieNode();
			m_values[c] = value;
			return value;
		}

		public void SetResults(string text)
		{
			if (!End)
			{
				End = true;
			}
			if (!Results.Contains(text))
			{
				Results.Add(text);
			}
		}

		public void Merge(TrieNode node, Dictionary<TrieNode, TrieNode> links)
		{
			if (node.End)
			{
				if (!End)
				{
					End = true;
				}
				foreach (string result in node.Results)
				{
					if (!Results.Contains(result))
					{
						Results.Add(result);
					}
				}
			}
			foreach (KeyValuePair<char, TrieNode> value2 in node.m_values)
			{
				if (!m_values.ContainsKey(value2.Key))
				{
					if (minflag > value2.Key)
					{
						minflag = value2.Key;
					}
					if (maxflag < value2.Key)
					{
						maxflag = value2.Key;
					}
					m_values[value2.Key] = value2.Value;
				}
			}
			TrieNode value;
			if (links.TryGetValue(node, out value))
			{
				Merge(value, links);
			}
		}

		public TrieNode[] ToArray()
		{
			TrieNode[] array = new TrieNode[65536];
			foreach (KeyValuePair<char, TrieNode> value in m_values)
			{
				array[value.Key] = value.Value;
			}
			return array;
		}
	}
}
