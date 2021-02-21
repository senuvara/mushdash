using System;
using System.Collections.Generic;
using System.Linq;

namespace ToolGood.Words
{
	public class PinYinSearch
	{
		private class FirstPinYinNode
		{
			private Dictionary<string, PinYinNode> PinYinNodes = new Dictionary<string, PinYinNode>();

			public ChineseNode AddNode(string pinYin, char chinese, int index)
			{
				PinYinNode value;
				if (!PinYinNodes.TryGetValue(pinYin, out value))
				{
					value = new PinYinNode();
					PinYinNodes.Add(pinYin, value);
				}
				return value.AddNode(chinese, index);
			}

			public bool MatchingNode(char firstPinYin, string pinYin, char chinese, List<ChineseNode> nodes)
			{
				if (string.IsNullOrEmpty(pinYin))
				{
					foreach (KeyValuePair<string, PinYinNode> pinYinNode in PinYinNodes)
					{
						pinYinNode.Value.ToAddNodes(nodes);
					}
				}
				else
				{
					if (firstPinYin.ToString() == pinYin && chinese == '\0')
					{
						foreach (KeyValuePair<string, PinYinNode> pinYinNode2 in PinYinNodes)
						{
							pinYinNode2.Value.ToAddNodes(nodes);
						}
						return true;
					}
					PinYinNode value;
					if (!PinYinNodes.TryGetValue(pinYin, out value))
					{
						return false;
					}
					value.MatchingNode(firstPinYin, pinYin, chinese, nodes);
				}
				return true;
			}
		}

		private class PinYinNode
		{
			private Dictionary<char, ChineseNode> ChineseNodes = new Dictionary<char, ChineseNode>();

			public void ToAddNodes(List<ChineseNode> nodes)
			{
				foreach (KeyValuePair<char, ChineseNode> chineseNode in ChineseNodes)
				{
					nodes.Add(chineseNode.Value);
				}
			}

			public ChineseNode AddNode(char chinese, int index)
			{
				ChineseNode value;
				if (!ChineseNodes.TryGetValue(chinese, out value))
				{
					value = new ChineseNode();
					ChineseNodes.Add(chinese, value);
					value.RangeStart = index;
				}
				value.RangeEnd = index;
				return value;
			}

			public bool MatchingNode(char firstPinYin, string pinYin, char chinese, List<ChineseNode> nodes)
			{
				if (chinese == '\0')
				{
					foreach (KeyValuePair<char, ChineseNode> chineseNode in ChineseNodes)
					{
						nodes.Add(chineseNode.Value);
					}
				}
				else
				{
					ChineseNode value;
					if (!ChineseNodes.TryGetValue(chinese, out value))
					{
						return false;
					}
					nodes.Add(value);
				}
				return true;
			}
		}

		private class ChineseNode
		{
			private Dictionary<char, FirstPinYinNode> FirstPinYinNodes = new Dictionary<char, FirstPinYinNode>();

			public int RangeStart;

			public int RangeEnd;

			public ChineseNode AddNode(char firstPinYin, string pinYin, char chinese, int index)
			{
				RangeEnd = index;
				FirstPinYinNode value;
				if (!FirstPinYinNodes.TryGetValue(firstPinYin, out value))
				{
					value = new FirstPinYinNode();
					FirstPinYinNodes.Add(firstPinYin, value);
				}
				return value.AddNode(pinYin, chinese, index);
			}

			public bool MatchingNode(char firstPinYin, string pinYin, char chinese, List<ChineseNode> nodes)
			{
				FirstPinYinNode value;
				if (!FirstPinYinNodes.TryGetValue(firstPinYin, out value))
				{
					return false;
				}
				value.MatchingNode(firstPinYin, pinYin, chinese, nodes);
				return true;
			}
		}

		private class TextLine
		{
			public List<TextLine> Lines = new List<TextLine>();

			public char FristPinYin;

			public string PinYin;

			public char Chinese;

			public TextLine TopLine;

			public TextLine()
			{
				TopLine = this;
			}

			public TextLine(char fristPinYin, string pinYin, char chinese)
			{
				FristPinYin = fristPinYin;
				PinYin = pinYin;
				Chinese = chinese;
				TopLine = this;
			}

			public TextLine(char fristPinYin, string pinYin, char chinese, TextLine line)
			{
				FristPinYin = fristPinYin;
				PinYin = pinYin;
				Chinese = chinese;
				TopLine = line;
			}

			public void Add(TextLine line)
			{
				Lines.Add(line);
			}
		}

		private const char wordsSpace = '\b';

		private const char pinYinSpace = '\t';

		private PinYinSearchType _type;

		private int[] _ids;

		private string[] _keywords;

		private int[] _indexs;

		private ChineseNode _root;

		private static WordsSearch _pinyinSplit;

		public PinYinSearch(PinYinSearchType type = PinYinSearchType.PinYin)
		{
			_type = type;
		}

		public void SetKeywords(List<string> keywords)
		{
			List<string> keySorts = SplitKeywords(keywords);
			buildKeywords(keySorts, keywords);
		}

		public void SetKeywords(List<string> keywords, List<int> ids)
		{
			if (keywords.Count != ids.Count)
			{
				throw new ArgumentException("keywords and ids inconsistent number.");
			}
			List<string> keySorts = SplitKeywords(keywords);
			buildKeywords(keySorts, keywords);
			_ids = ids.ToArray();
		}

		public List<string> SearchTexts(string text, bool keywordSort = false)
		{
			TextLine root;
			trySplitSearchText(text, out root);
			List<ChineseNode> list = matching(root);
			List<string> list2 = new List<string>();
			if (keywordSort)
			{
				foreach (ChineseNode item in list)
				{
					for (int i = item.RangeStart; i <= item.RangeEnd; i++)
					{
						list2.Add(_keywords[_indexs[i]]);
					}
				}
				return list2;
			}
			List<int> list3 = new List<int>();
			foreach (ChineseNode item2 in list)
			{
				for (int j = item2.RangeStart; j <= item2.RangeEnd; j++)
				{
					list3.Add(_indexs[j]);
				}
			}
			list3 = list3.OrderBy((int q) => q).Distinct().ToList();
			foreach (int item3 in list3)
			{
				list2.Add(_keywords[item3]);
			}
			return list2;
		}

		public List<int> SearchIds(string text, bool keywordSort = false)
		{
			TextLine root;
			trySplitSearchText(text, out root);
			List<ChineseNode> list = matching(root);
			List<int> list2 = new List<int>();
			if (keywordSort)
			{
				foreach (ChineseNode item in list)
				{
					for (int i = item.RangeStart; i <= item.RangeEnd; i++)
					{
						list2.Add(_ids[_indexs[i]]);
					}
				}
				return list2;
			}
			List<int> list3 = new List<int>();
			foreach (ChineseNode item2 in list)
			{
				for (int j = item2.RangeStart; j <= item2.RangeEnd; j++)
				{
					list3.Add(_indexs[j]);
				}
			}
			list3 = list3.OrderBy((int q) => q).Distinct().ToList();
			foreach (int item3 in list3)
			{
				list2.Add(_ids[item3]);
			}
			return list2;
		}

		public List<PinYinSearchResult> SearchTextWithIds(string text, bool keywordSort = false)
		{
			TextLine root;
			trySplitSearchText(text, out root);
			List<ChineseNode> list = matching(root);
			List<PinYinSearchResult> list2 = new List<PinYinSearchResult>();
			if (keywordSort)
			{
				foreach (ChineseNode item in list)
				{
					for (int i = item.RangeStart; i <= item.RangeEnd; i++)
					{
						int num = _indexs[i];
						list2.Add(new PinYinSearchResult(_keywords[num], _ids[num]));
					}
				}
				return list2;
			}
			List<int> list3 = new List<int>();
			foreach (ChineseNode item2 in list)
			{
				for (int j = item2.RangeStart; j <= item2.RangeEnd; j++)
				{
					list3.Add(_indexs[j]);
				}
			}
			list3 = list3.OrderBy((int q) => q).Distinct().ToList();
			foreach (int item3 in list3)
			{
				list2.Add(new PinYinSearchResult(_keywords[item3], _ids[item3]));
			}
			return list2;
		}

		public List<string> PickTexts(string text, bool keywordSort = false, int pickLength = 2)
		{
			TextLine root;
			trySplitSearchText(text, out root);
			List<ChineseNode> list = matching(root);
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			int num = text.Length + pickLength;
			if (keywordSort)
			{
				foreach (ChineseNode item in list)
				{
					for (int i = item.RangeStart; i <= item.RangeEnd; i++)
					{
						string text2 = _keywords[i];
						if (text2.Length <= num)
						{
							list2.Add(text2);
						}
						else
						{
							list3.Add(text2);
						}
					}
				}
			}
			else
			{
				List<int> list4 = new List<int>();
				foreach (ChineseNode item2 in list)
				{
					for (int j = item2.RangeStart; j <= item2.RangeEnd; j++)
					{
						list4.Add(_indexs[j]);
					}
				}
				list4 = list4.OrderBy((int q) => q).Distinct().ToList();
				foreach (int item3 in list4)
				{
					string text3 = _keywords[item3];
					if (text3.Length <= num)
					{
						list2.Add(text3);
					}
					else
					{
						list3.Add(text3);
					}
				}
			}
			list2.AddRange(list3);
			return list2;
		}

		public List<PinYinSearchResult> PickTextWithIds(string text, bool keywordSort = false, int pickLength = 2)
		{
			TextLine root;
			trySplitSearchText(text, out root);
			List<ChineseNode> list = matching(root);
			List<PinYinSearchResult> list2 = new List<PinYinSearchResult>();
			List<PinYinSearchResult> list3 = new List<PinYinSearchResult>();
			int num = text.Length + pickLength;
			if (keywordSort)
			{
				foreach (ChineseNode item in list)
				{
					for (int i = item.RangeStart; i <= item.RangeEnd; i++)
					{
						int num2 = _indexs[i];
						string text2 = _keywords[num2];
						if (text2.Length <= num)
						{
							list2.Add(new PinYinSearchResult(text2, _ids[num2]));
						}
						else
						{
							list3.Add(new PinYinSearchResult(text2, _ids[num2]));
						}
					}
				}
			}
			else
			{
				List<int> list4 = new List<int>();
				foreach (ChineseNode item2 in list)
				{
					for (int j = item2.RangeStart; j <= item2.RangeEnd; j++)
					{
						list4.Add(_indexs[j]);
					}
				}
				list4 = list4.OrderBy((int q) => q).Distinct().ToList();
				foreach (int item3 in list4)
				{
					string text3 = _keywords[item3];
					if (text3.Length <= num)
					{
						list2.Add(new PinYinSearchResult(text3, _ids[item3]));
					}
					else
					{
						list3.Add(new PinYinSearchResult(text3, _ids[item3]));
					}
				}
			}
			list2.AddRange(list3);
			return list2;
		}

		private List<ChineseNode> matching(TextLine line)
		{
			List<ChineseNode> list = new List<ChineseNode>();
			matching(line, new List<ChineseNode>
			{
				_root
			}, list);
			return list;
		}

		private void matching(TextLine line, List<ChineseNode> nodes, List<ChineseNode> results)
		{
			TextLine topLine = line.TopLine;
			if (topLine.Lines.Count == 0)
			{
				foreach (ChineseNode node in nodes)
				{
					results.Add(node);
				}
				return;
			}
			foreach (TextLine line2 in topLine.Lines)
			{
				List<ChineseNode> nodes2 = new List<ChineseNode>();
				foreach (ChineseNode node2 in nodes)
				{
					node2.MatchingNode(line2.FristPinYin, line2.PinYin, line2.Chinese, nodes2);
				}
				if (nodes.Count > 0)
				{
					matching(line2, nodes2, results);
				}
			}
		}

		private WordsSearch getPinYinSplit()
		{
			if (_pinyinSplit == null)
			{
				_pinyinSplit = new WordsSearch();
				List<string> list = new List<string>();
				foreach (string item in PinYinDict.pyName)
				{
					string text = item.ToUpper();
					if (text.Length >= 2)
					{
						list.Add(text);
					}
				}
				_pinyinSplit.SetKeywords(list);
			}
			return _pinyinSplit;
		}

		private bool trySplitSearchText(string text, out TextLine root)
		{
			text = text.ToUpper();
			List<WordsSearchResult> list = getPinYinSplit().FindAll(text);
			root = new TextLine();
			List<TextLine> list2 = new List<TextLine>();
			list2.Add(root);
			List<TextLine> list3 = list2;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (_type == PinYinSearchType.PinYin && c >= '一' && c <= '龥')
				{
					List<string> allPinYin = PinYinDict.GetAllPinYin(c);
					if (allPinYin.Count == 0)
					{
						allPinYin.Add(" ");
					}
					for (int j = 0; j < allPinYin.Count; j++)
					{
						string text2 = allPinYin[j].ToUpper();
						TextLine textLine;
						if (j == 0)
						{
							textLine = new TextLine(text2[0], text2, c);
							list3.Add(textLine);
						}
						else
						{
							textLine = new TextLine(text2[0], text2, c, list3[i + 1]);
						}
						list3[i].Add(textLine);
					}
				}
				else
				{
					TextLine textLine2;
					if (c < '一' || c > '龥')
					{
						textLine2 = ((c < '0' && c > '9') ? ((c < 'A' && c > 'Z') ? new TextLine(' ', " ", c) : new TextLine(c, null, '\0')) : new TextLine(c, null, '\0'));
					}
					else
					{
						string pinYinFast = PinYinDict.GetPinYinFast(c);
						textLine2 = new TextLine(pinYinFast[0], pinYinFast, c);
					}
					list3[i].Add(textLine2);
					list3.Add(textLine2);
				}
			}
			foreach (WordsSearchResult item in list)
			{
				TextLine line = new TextLine(item.Keyword[0], item.Keyword, '\0', list3[item.End + 1]);
				list3[item.Start].Add(line);
			}
			return true;
		}

		private void buildKeywords(List<string> keySorts, List<string> keywords)
		{
			_root = new ChineseNode();
			List<int> list = new List<int>();
			for (int i = 0; i < keySorts.Count; i++)
			{
				string[] array = keySorts[i].Split('\b');
				ChineseNode chineseNode = _root;
				for (int j = 0; j < array.Length - 1; j++)
				{
					string[] array2 = array[j].Split('\t');
					chineseNode = chineseNode.AddNode(array2[0][0], array2[1], array2[2][0], i);
				}
				list.Add(int.Parse(array[array.Length - 1]));
			}
			_keywords = keywords.ToArray();
			_indexs = list.ToArray();
		}

		private List<string> SplitKeywords(List<string> keywords)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < keywords.Count; i++)
			{
				string text = keywords[i];
				splitKeywords(text.ToUpper(), i, list);
			}
			return list.OrderBy((string q) => q).ToList();
		}

		private void splitKeywords(string keyword, int baseIndex, List<string> results)
		{
			List<List<string>> list = new List<List<string>>();
			if (_type == PinYinSearchType.PinYin)
			{
				string[] pinYinList = PinYinDict.GetPinYinList(keyword);
				for (int i = 0; i < keyword.Length; i++)
				{
					List<string> list2 = new List<string>();
					char c = keyword[i];
					if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z'))
					{
						list2.Add(c.ToString());
					}
					else if (c >= '一' && c <= '龥')
					{
						string text = pinYinList[i];
						if (text == null)
						{
							text = " ";
						}
						list2.Add(text.ToUpper());
					}
					else
					{
						list2.Add(" ");
					}
					list.Add(list2);
				}
			}
			else
			{
				for (int j = 0; j < keyword.Length; j++)
				{
					List<string> list3 = new List<string>();
					char c2 = keyword[j];
					if ((c2 >= '0' && c2 <= '9') || (c2 >= 'A' && c2 <= 'Z'))
					{
						list3.Add(c2.ToString());
					}
					else if (c2 >= '一' && c2 <= '龥')
					{
						List<string> allPinYin = PinYinDict.GetAllPinYin(c2);
						if (allPinYin.Count == 0)
						{
							allPinYin.Add(" ");
						}
						foreach (string item in allPinYin)
						{
							list3.Add(item.ToUpper());
						}
					}
					else
					{
						list3.Add(" ");
					}
					list.Add(list3);
				}
			}
			foreach (string item2 in list[0])
			{
				string py = item2[0].ToString() + '\t' + item2 + '\t' + keyword[0];
				splitKeywords(py, 1, list, keyword, baseIndex, results);
			}
		}

		private void splitKeywords(string py, int index, List<List<string>> ks, string keyword, int baseIndex, List<string> results)
		{
			if (ks.Count == index)
			{
				results.Add(py + '\b' + baseIndex.ToString());
				return;
			}
			List<string> list = ks[index];
			foreach (string item in list)
			{
				string text = py;
				py = text + '\b' + item[0].ToString() + '\t' + item + '\t' + keyword[index].ToString();
				splitKeywords(py, index + 1, ks, keyword, baseIndex, results);
			}
		}
	}
}
