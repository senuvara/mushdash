using System.Collections.Generic;

namespace ToolGood.Words.internals
{
	public abstract class BaseSearch
	{
		protected TrieNode _root = new TrieNode();

		protected TrieNode[] _first = new TrieNode[65536];

		public virtual void SetKeywords(ICollection<string> _keywords)
		{
			TrieNode[] array = new TrieNode[65536];
			TrieNode trieNode = new TrieNode();
			foreach (string _keyword in _keywords)
			{
				if (!string.IsNullOrEmpty(_keyword))
				{
					TrieNode trieNode2 = _first[_keyword[0]];
					if (trieNode2 == null)
					{
						trieNode2 = trieNode.Add(_keyword[0]);
						array[_keyword[0]] = trieNode2;
					}
					for (int i = 1; i < _keyword.Length; i++)
					{
						trieNode2 = trieNode2.Add(_keyword[i]);
					}
					trieNode2.SetResults(_keyword);
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
				item.Key.Merge(item.Value, dictionary);
			}
			_root = trieNode;
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
	}
}
