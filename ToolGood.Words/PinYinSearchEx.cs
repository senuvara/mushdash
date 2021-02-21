using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToolGood.Words.internals;

namespace ToolGood.Words
{
	public class PinYinSearchEx
	{
		private class FirstPinYinNode : IDisposable
		{
			public Dictionary<ushort, PinYinNode> PinYinNodes = new Dictionary<ushort, PinYinNode>();

			public ChineseNode AddNode(ushort pinYin, ushort chinese, int index)
			{
				PinYinNode value;
				if (!PinYinNodes.TryGetValue(pinYin, out value))
				{
					value = new PinYinNode();
					PinYinNodes.Add(pinYin, value);
				}
				return value.AddNode(chinese, index);
			}

			public void Dispose()
			{
				if (PinYinNodes != null)
				{
					PinYinNodes.Clear();
					PinYinNodes = null;
				}
			}
		}

		private class PinYinNode : IDisposable
		{
			public Dictionary<ushort, ChineseNode> ChineseNodes = new Dictionary<ushort, ChineseNode>();

			public ChineseNode AddNode(ushort chinese, int index)
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

			public void Dispose()
			{
				if (ChineseNodes != null)
				{
					ChineseNodes.Clear();
					ChineseNodes = null;
				}
			}
		}

		private class ChineseNode : IDisposable
		{
			public Dictionary<byte, FirstPinYinNode> FirstPinYinNodes = new Dictionary<byte, FirstPinYinNode>();

			public int RangeStart;

			public int RangeEnd;

			public int Index;

			public bool IsEnd => FirstPinYinNodes.Count == 0;

			public ChineseNode AddNode(byte firstPinYin, ushort pinYin, ushort chinese, int index)
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

			public Dictionary<ushort, ChineseNode> GetNodes()
			{
				Dictionary<ushort, ChineseNode> dictionary = new Dictionary<ushort, ChineseNode>();
				foreach (KeyValuePair<byte, FirstPinYinNode> firstPinYinNode in FirstPinYinNodes)
				{
					foreach (KeyValuePair<ushort, PinYinNode> pinYinNode in firstPinYinNode.Value.PinYinNodes)
					{
						foreach (KeyValuePair<ushort, ChineseNode> chineseNode in pinYinNode.Value.ChineseNodes)
						{
							dictionary.Add(chineseNode.Key, chineseNode.Value);
						}
					}
				}
				return dictionary;
			}

			public void Dispose()
			{
				if (FirstPinYinNodes != null)
				{
					FirstPinYinNodes.Clear();
					FirstPinYinNodes = null;
				}
			}
		}

		private class TextLine
		{
			public List<TextLine> Lines = new List<TextLine>();

			public byte FristPinYin;

			public ushort PinYin;

			public ushort Chinese;

			public TextLine TopLine;

			public bool IsError;

			public static TextLine Error
			{
				get
				{
					TextLine textLine = new TextLine();
					textLine.IsError = true;
					return textLine;
				}
			}

			public TextLine()
			{
				TopLine = this;
			}

			public TextLine(byte fristPinYin, ushort pinYin, ushort chinese)
			{
				FristPinYin = fristPinYin;
				PinYin = pinYin;
				Chinese = chinese;
				TopLine = this;
			}

			public TextLine(byte fristPinYin, ushort pinYin, ushort chinese, TextLine line)
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

			public void FilterError()
			{
				if (TopLine.Lines.Count == 0)
				{
					return;
				}
				foreach (TextLine line in TopLine.Lines)
				{
					line.FilterError();
				}
				int num = 0;
				foreach (TextLine line2 in TopLine.Lines)
				{
					if (line2.IsError)
					{
						num++;
					}
				}
				if (num == TopLine.Lines.Count)
				{
					IsError = true;
				}
			}
		}

		private class MiniSearchResult
		{
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

			public MiniSearchResult(string keyword, int start, int end)
			{
				Keyword = keyword;
				End = end;
				Start = start;
			}
		}

		private class MiniSearch : BaseSearch
		{
			public List<MiniSearchResult> FindAll(string text)
			{
				TrieNode trieNode = null;
				List<MiniSearchResult> list = new List<MiniSearchResult>();
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
						foreach (string result in node.Results)
						{
							list.Add(new MiniSearchResult(result, i + 1 - result.Length, i));
						}
					}
					trieNode = node;
				}
				return list;
			}
		}

		private class WordNode
		{
			public List<WordNode> Nodes = new List<WordNode>();

			private Dictionary<string, WordNode> dict = new Dictionary<string, WordNode>();

			public string Text;

			public int Count;

			public int Index;

			public int fpyIndex;

			public int pyIndex;

			public int cIndex;

			public void Add(string fpy, string py, string chinese)
			{
				WordNode value;
				if (!dict.TryGetValue(fpy, out value))
				{
					value = new WordNode();
					dict.Add(fpy, value);
					Nodes.Add(value);
					value.Text = fpy;
				}
				value.Count++;
				WordNode value2;
				if (!value.dict.TryGetValue(py, out value2))
				{
					value2 = new WordNode();
					value.dict.Add(py, value2);
					value.Nodes.Add(value2);
					value2.Text = py;
				}
				value2.Count++;
				WordNode value3;
				if (!value2.dict.TryGetValue(chinese, out value3))
				{
					value3 = new WordNode();
					value2.dict.Add(chinese, value3);
					value2.Nodes.Add(value3);
					value3.Text = chinese;
				}
				value3.Count++;
			}

			public void SetIndex()
			{
				int num = 1;
				foreach (WordNode node in Nodes)
				{
					node.Index = num++;
				}
				fpyIndex = num;
				num = 1;
				foreach (WordNode node2 in Nodes)
				{
					foreach (WordNode node3 in node2.Nodes)
					{
						node3.Index = num++;
					}
				}
				pyIndex = num;
				num = 1;
				foreach (WordNode node4 in Nodes)
				{
					foreach (WordNode node5 in node4.Nodes)
					{
						foreach (WordNode node6 in node5.Nodes)
						{
							node6.Index = num++;
						}
					}
				}
				cIndex = num;
			}

			public void SetSort2()
			{
				if (Nodes.Count == 0)
				{
					return;
				}
				foreach (WordNode node in Nodes)
				{
					node.SetSort2();
				}
				Nodes = Nodes.OrderBy((WordNode q) => q.Text).ToList();
			}
		}

		private const char wordsSpace = '\b';

		private const char pinYinSpace = '\t';

		private PinYinSearchType _type;

		private int[] _ids;

		private string[] _keywords;

		private int[] _indexs;

		private ushort[] _toWord1;

		private Dictionary<string, ushort> _toWord2;

		private byte[] _toFirstPinYin;

		private ushort[] _wordToPinYin;

		private byte[] _pinYinToFirstPinYin;

		public ushort[] _pinYinStart;

		public ushort[] _firstPinYinStart;

		private ushort[] _word;

		private int[] _start;

		private int[] _end;

		private BitArray _isEnd;

		private int[] _fpyIndexBase;

		private byte[] _fpy;

		private int[] _pyIndexBase;

		private ushort[] _py;

		private int[] _wordIndexBase;

		private int[] _wordCheck;

		private static MiniSearch _pinyinSplit;

		private List<ushort> _word_firstPinYin;

		public PinYinSearchEx(PinYinSearchType type = PinYinSearchType.PinYin)
		{
			_type = type;
		}

		public void SetKeywords(List<string> keywords)
		{
			setKeywords(keywords);
			_ids = new int[0];
		}

		public void SetKeywords(List<string> keywords, List<int> ids)
		{
			if (keywords.Count != ids.Count)
			{
				throw new ArgumentException("keywords and ids inconsistent number.");
			}
			setKeywords(keywords);
			_ids = ids.ToArray();
		}

		public List<string> SearchTexts(string text, bool keywordSort = false)
		{
			List<int> list = searchIndexs(text);
			if (list.Count == 0)
			{
				return new List<string>();
			}
			List<int> list2 = new List<int>();
			foreach (int item in list)
			{
				for (int i = _start[item]; i <= _end[item]; i++)
				{
					list2.Add(_indexs[i]);
				}
			}
			list2 = list2.Distinct().ToList();
			if (!keywordSort)
			{
				list2 = list2.OrderBy((int q) => q).ToList();
			}
			List<string> list3 = new List<string>();
			foreach (int item2 in list2)
			{
				list3.Add(_keywords[item2]);
			}
			return list3;
		}

		public List<int> SearchIds(string text, bool keywordSort = false)
		{
			List<int> list = searchIndexs(text);
			if (list.Count == 0)
			{
				return new List<int>();
			}
			List<int> list2 = new List<int>();
			foreach (int item in list)
			{
				for (int i = _start[item]; i <= _end[item]; i++)
				{
					list2.Add(_indexs[i]);
				}
			}
			list2 = list2.Distinct().ToList();
			if (!keywordSort)
			{
				list2 = list2.OrderBy((int q) => q).ToList();
			}
			List<int> list3 = new List<int>();
			foreach (int item2 in list2)
			{
				list3.Add(_indexs[item2]);
			}
			return list3;
		}

		public List<PinYinSearchResult> SearchTextWithIds(string text, bool keywordSort = false)
		{
			List<int> list = searchIndexs(text);
			if (list.Count == 0)
			{
				return new List<PinYinSearchResult>();
			}
			List<int> list2 = new List<int>();
			foreach (int item in list)
			{
				for (int i = _start[item]; i <= _end[item]; i++)
				{
					list2.Add(_indexs[i]);
				}
			}
			list2 = list2.Distinct().ToList();
			if (!keywordSort)
			{
				list2 = list2.OrderBy((int q) => q).ToList();
			}
			List<PinYinSearchResult> list3 = new List<PinYinSearchResult>();
			foreach (int item2 in list2)
			{
				list3.Add(new PinYinSearchResult(_keywords[item2], _ids[item2]));
			}
			return list3;
		}

		public List<string> PickTexts(string text, bool keywordSort = false, int pickLength = 2)
		{
			List<int> list = searchIndexs(text);
			if (list.Count == 0)
			{
				return new List<string>();
			}
			List<int> list2 = new List<int>();
			foreach (int item in list)
			{
				for (int i = _start[item]; i <= _end[item]; i++)
				{
					list2.Add(_indexs[i]);
				}
			}
			list2 = list2.Distinct().ToList();
			if (!keywordSort)
			{
				list2 = list2.OrderBy((int q) => q).ToList();
			}
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			List<string> list5 = new List<string>();
			int length = text.Length;
			int num = length + pickLength;
			foreach (int item2 in list2)
			{
				string text2 = _keywords[item2];
				if (text2.Length == length)
				{
					list3.Add(text2);
				}
				else if (text2.Length <= num)
				{
					list4.Add(text2);
				}
				else
				{
					list5.Add(text2);
				}
			}
			list3.AddRange(list4);
			list3.AddRange(list5);
			return list3;
		}

		public List<PinYinSearchResult> PickTextWithIds(string text, bool keywordSort = false, int pickLength = 2)
		{
			List<int> list = searchIndexs(text);
			if (list.Count == 0)
			{
				return new List<PinYinSearchResult>();
			}
			List<int> list2 = new List<int>();
			foreach (int item in list)
			{
				for (int i = _start[item]; i <= _end[item]; i++)
				{
					list2.Add(_indexs[i]);
				}
			}
			list2 = list2.Distinct().ToList();
			if (!keywordSort)
			{
				list2 = list2.OrderBy((int q) => q).ToList();
			}
			List<PinYinSearchResult> list3 = new List<PinYinSearchResult>();
			List<PinYinSearchResult> list4 = new List<PinYinSearchResult>();
			List<PinYinSearchResult> list5 = new List<PinYinSearchResult>();
			int length = text.Length;
			int num = length + pickLength;
			foreach (int item2 in list2)
			{
				string text2 = _keywords[item2];
				if (text2.Length == length)
				{
					list3.Add(new PinYinSearchResult(text2, _ids[item2]));
				}
				else if (text2.Length <= num)
				{
					list4.Add(new PinYinSearchResult(text2, _ids[item2]));
				}
				else
				{
					list5.Add(new PinYinSearchResult(text2, _ids[item2]));
				}
			}
			list3.AddRange(list4);
			list3.AddRange(list5);
			return list3;
		}

		public void SaveFile(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				throw new ArgumentNullException("filePath");
			}
			filePath = Path.GetFullPath(filePath);
			string directoryName = Path.GetDirectoryName(filePath);
			Directory.CreateDirectory(directoryName);
			if (!File.Exists(filePath))
			{
				File.Create(filePath).Close();
			}
			saveFile(filePath);
		}

		public void LoadFile(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				throw new ArgumentNullException("filePath");
			}
			filePath = Path.GetFullPath(filePath);
			if (!File.Exists(filePath))
			{
				throw new ArgumentException(filePath + "文体不存在");
			}
			loadFile(filePath);
		}

		private void loadFile(string filePath)
		{
			FileStream fileStream = File.OpenRead(filePath);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			_type = (PinYinSearchType)binaryReader.ReadInt32();
			_ids = readIntArray(binaryReader);
			_keywords = readStringArray(binaryReader);
			_indexs = readIntArray(binaryReader);
			_toWord1 = readWordArray(binaryReader);
			_toWord2 = readDictionary(binaryReader);
			_toFirstPinYin = readByteArray(binaryReader);
			_wordToPinYin = readWordArray(binaryReader);
			_pinYinToFirstPinYin = readByteArray(binaryReader);
			_pinYinStart = readWordArray(binaryReader);
			_firstPinYinStart = readWordArray(binaryReader);
			_word = readWordArray(binaryReader);
			_start = readIntArray(binaryReader);
			_end = readIntArray(binaryReader);
			_isEnd = readBitArray(binaryReader);
			_fpyIndexBase = readIntArray(binaryReader);
			_fpy = readByteArray(binaryReader);
			_pyIndexBase = readIntArray(binaryReader);
			_py = readWordArray(binaryReader);
			_wordIndexBase = readIntArray(binaryReader);
			_wordCheck = readIntArray(binaryReader);
			binaryReader.Close();
			fileStream.Close();
		}

		private int[] readIntArray(BinaryReader br)
		{
			int num = br.ReadInt32();
			int[] array = new int[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = br.ReadInt32();
			}
			return array;
		}

		private string[] readStringArray(BinaryReader br)
		{
			int num = br.ReadInt32();
			string[] array = new string[num];
			for (int i = 0; i < num; i++)
			{
				int count = br.ReadInt32();
				byte[] bytes = br.ReadBytes(count);
				array[i] = Encoding.UTF8.GetString(bytes);
			}
			return array;
		}

		private ushort[] readWordArray(BinaryReader br)
		{
			int num = br.ReadInt32();
			ushort[] array = new ushort[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = br.ReadUInt16();
			}
			return array;
		}

		private byte[] readByteArray(BinaryReader br)
		{
			int count = br.ReadInt32();
			return br.ReadBytes(count);
		}

		private Dictionary<string, ushort> readDictionary(BinaryReader br)
		{
			Dictionary<string, ushort> dictionary = new Dictionary<string, ushort>();
			int num = br.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				int count = br.ReadInt32();
				byte[] bytes = br.ReadBytes(count);
				string @string = Encoding.UTF8.GetString(bytes);
				ushort value = br.ReadUInt16();
				dictionary.Add(@string, value);
			}
			return dictionary;
		}

		private BitArray readBitArray(BinaryReader br)
		{
			int count = br.ReadInt32();
			byte[] bytes = br.ReadBytes(count);
			return new BitArray(bytes);
		}

		private void saveFile(string filePath)
		{
			FileStream fileStream = File.OpenWrite(filePath);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			binaryWriter.Write((int)_type);
			write(binaryWriter, _ids);
			write(binaryWriter, _keywords);
			write(binaryWriter, _indexs);
			write(binaryWriter, _toWord1);
			write(binaryWriter, _toWord2);
			write(binaryWriter, _toFirstPinYin);
			write(binaryWriter, _wordToPinYin);
			write(binaryWriter, _pinYinToFirstPinYin);
			write(binaryWriter, _pinYinStart);
			write(binaryWriter, _firstPinYinStart);
			write(binaryWriter, _word);
			write(binaryWriter, _start);
			write(binaryWriter, _end);
			write(binaryWriter, _isEnd);
			write(binaryWriter, _fpyIndexBase);
			write(binaryWriter, _fpy);
			write(binaryWriter, _pyIndexBase);
			write(binaryWriter, _py);
			write(binaryWriter, _wordIndexBase);
			write(binaryWriter, _wordCheck);
			binaryWriter.Close();
			fileStream.Close();
		}

		private void write(BinaryWriter bw, int[] w)
		{
			if (w == null || w.Length == 0)
			{
				bw.Write(0);
				return;
			}
			bw.Write(w.Length);
			foreach (int value in w)
			{
				bw.Write(value);
			}
		}

		private void write(BinaryWriter bw, string[] texts)
		{
			if (texts == null || texts.Length == 0)
			{
				bw.Write(0);
				return;
			}
			bw.Write(texts.Length);
			foreach (string s in texts)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(s);
				bw.Write(bytes.Length);
				bw.Write(bytes);
			}
		}

		private void write(BinaryWriter bw, ushort[] w)
		{
			if (w == null || w.Length == 0)
			{
				bw.Write(0);
				return;
			}
			bw.Write(w.Length);
			foreach (ushort value in w)
			{
				bw.Write(value);
			}
		}

		private void write(BinaryWriter bw, byte[] w)
		{
			if (w == null || w.Length == 0)
			{
				bw.Write(0);
				return;
			}
			bw.Write(w.Length);
			bw.Write(w);
		}

		private void write(BinaryWriter bw, Dictionary<string, ushort> w)
		{
			if (w == null || w.Count == 0)
			{
				bw.Write(0);
				return;
			}
			bw.Write(w.Count);
			foreach (KeyValuePair<string, ushort> item in w)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(item.Key);
				bw.Write(bytes.Length);
				bw.Write(bytes);
				bw.Write(item.Value);
			}
		}

		private void write(BinaryWriter bw, BitArray array)
		{
			if (array == null || array.Length == 0)
			{
				bw.Write(0);
				return;
			}
			byte[] array2 = new byte[(int)Math.Ceiling((double)array.Length / 8.0)];
			array.CopyTo(array2, 0);
			bw.Write(array2.Length);
			bw.Write(array2);
		}

		private List<int> searchIndexs(string text)
		{
			TextLine root;
			if (trySplitSearchText(text, out root))
			{
				List<int> list = new List<int>();
				List<int> list2 = new List<int>();
				list2.Add(0);
				List<int> baseList = list2;
				searchIndexs(root, baseList, list);
				return list.Distinct().ToList();
			}
			return new List<int>();
		}

		private void searchIndexs(TextLine line, List<int> baseList, List<int> results)
		{
			TextLine topLine = line.TopLine;
			if (topLine.Lines.Count == 0)
			{
				results.AddRange(baseList);
				return;
			}
			foreach (TextLine line2 in topLine.Lines)
			{
				if (line2.IsError)
				{
					continue;
				}
				List<int> list = new List<int>();
				for (int i = 0; i < baseList.Count; i++)
				{
					int num = baseList[i];
					if (!_isEnd[num])
					{
						matchFirstPinYin(line2, _fpyIndexBase[num], list);
					}
				}
				if (list.Count > 0)
				{
					searchIndexs(line2, list, results);
				}
			}
		}

		private void matchFirstPinYin(TextLine line, int baseIndex, List<int> nextBaseList)
		{
			int num = baseIndex + line.FristPinYin;
			byte b = _fpy[num];
			if (b == line.FristPinYin)
			{
				int baseIndex2 = _pyIndexBase[num];
				matchPinYin(line, baseIndex2, nextBaseList);
			}
		}

		private void matchPinYin(TextLine line, int baseIndex, List<int> nextBaseList)
		{
			if (line.PinYin == 0)
			{
				for (int i = _firstPinYinStart[line.FristPinYin]; i < _firstPinYinStart[line.FristPinYin + 1]; i++)
				{
					int num = baseIndex + i;
					ushort num2 = _py[num];
					if (num2 != (ushort)i)
					{
						continue;
					}
					int num3 = _wordIndexBase[num];
					for (int j = _pinYinStart[i]; j < _pinYinStart[i + 1]; j++)
					{
						int num4 = num3 + j;
						int num5 = _wordCheck[num4];
						if (num5 > 0 && _word[num5] == j)
						{
							nextBaseList.Add(num5);
						}
					}
				}
			}
			else
			{
				int num6 = baseIndex + line.PinYin;
				ushort num7 = _py[num6];
				if (num7 == line.PinYin)
				{
					int baseIndex2 = _wordIndexBase[num6];
					matchChinese(line, baseIndex2, nextBaseList);
				}
			}
		}

		private void matchChinese(TextLine line, int baseIndex, List<int> nextBaseList)
		{
			if (line.Chinese == 0)
			{
				for (int i = _pinYinStart[line.PinYin]; i < _pinYinStart[line.PinYin + 1]; i++)
				{
					int num = baseIndex + i;
					int num2 = _wordCheck[num];
					if (num2 != 0 && _word[num2] == i)
					{
						nextBaseList.Add(num2);
					}
				}
			}
			else
			{
				int num3 = baseIndex + line.Chinese;
				int num4 = _wordCheck[num3];
				if (num4 != 0 && _word[num4] == line.Chinese)
				{
					nextBaseList.Add(num4);
				}
			}
		}

		private MiniSearch getPinYinSplit()
		{
			if (_pinyinSplit == null)
			{
				_pinyinSplit = new MiniSearch();
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
			List<MiniSearchResult> list = getPinYinSplit().FindAll(text);
			root = new TextLine();
			List<TextLine> list2 = new List<TextLine>();
			list2.Add(root);
			List<TextLine> list3 = list2;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || c == ' ')
				{
					byte b = _toFirstPinYin[c];
					TextLine textLine = (b != 0) ? new TextLine(b, 0, 0) : TextLine.Error;
					list3[i].Add(textLine);
					list3.Add(textLine);
					continue;
				}
				ushort num = _toWord1[c];
				switch (num)
				{
				case 0:
					return false;
				case ushort.MaxValue:
				{
					List<string> allPinYin = PinYinDict.GetAllPinYin(c);
					for (int j = 0; j < allPinYin.Count; j++)
					{
						num = _toWord2[allPinYin[j].ToUpper() + '\t' + c];
						if (j == 0)
						{
							TextLine textLine = new TextLine(getFirstPinYin(num), getPinYin(num), num);
							list3[i].Add(textLine);
							list3.Add(textLine);
						}
						else
						{
							TextLine textLine = new TextLine(getFirstPinYin(num), getPinYin(num), num, list3[i + 1]);
							list3[i].Add(textLine);
						}
					}
					break;
				}
				default:
				{
					TextLine textLine = new TextLine(getFirstPinYin(num), getPinYin(num), getWord(num));
					list3[i].Add(textLine);
					list3.Add(textLine);
					break;
				}
				}
			}
			foreach (MiniSearchResult item in list)
			{
				ushort value;
				if (_toWord2.TryGetValue(item.Keyword, out value))
				{
					TextLine line = new TextLine(_pinYinToFirstPinYin[value], value, 0, list3[item.End + 1]);
					list3[item.Start].Add(line);
				}
			}
			root.FilterError();
			return true;
		}

		private void setKeywords(List<string> keywords)
		{
			initDictionary(keywords);
			List<string> keySort = SplitKeywords(keywords);
			ChineseNode chineseNode = buildNode(keySort, keywords);
			buildToArray(chineseNode);
			chineseNode.Dispose();
			chineseNode = null;
			_keywords = keywords.ToArray();
		}

		private void buildToArray(ChineseNode root)
		{
			buildNodeInfo(root);
			buildCheckArray(root);
		}

		private void buildNodeInfo(ChineseNode root)
		{
			List<ushort> list = new List<ushort>();
			list.Add(0);
			List<ushort> list2 = list;
			List<int> list3 = new List<int>();
			list3.Add(0);
			List<int> list4 = list3;
			list3 = new List<int>();
			list3.Add(_indexs.Length);
			List<int> list5 = list3;
			List<bool> list6 = new List<bool>();
			list6.Add(false);
			List<bool> list7 = list6;
			int index = 1;
			buildNodeInfo(root, list2, list4, list5, list7, ref index);
			list2.Add(0);
			list4.Add(0);
			list5.Add(0);
			list7.Add(true);
			_word = list2.ToArray();
			_start = list4.ToArray();
			_end = list5.ToArray();
			_isEnd = new BitArray(list7.ToArray());
		}

		private void buildNodeInfo(ChineseNode node, List<ushort> word, List<int> start, List<int> end, List<bool> isEnd, ref int index)
		{
			Dictionary<ushort, ChineseNode> nodes = node.GetNodes();
			foreach (KeyValuePair<ushort, ChineseNode> item in nodes)
			{
				word.Add(item.Key);
				isEnd.Add(item.Value.IsEnd);
				start.Add(item.Value.RangeStart);
				end.Add(item.Value.RangeEnd);
				item.Value.Index = index++;
			}
			foreach (KeyValuePair<ushort, ChineseNode> item2 in nodes)
			{
				buildNodeInfo(item2.Value, word, start, end, isEnd, ref index);
			}
		}

		private void buildCheckArray(ChineseNode root)
		{
			_fpyIndexBase = new int[_word.Length];
			List<byte> list = new List<byte>();
			list.Add(0);
			List<byte> list2 = list;
			List<int> list3 = new List<int>();
			list3.Add(0);
			List<int> list4 = list3;
			List<ushort> list5 = new List<ushort>();
			list5.Add(0);
			List<ushort> list6 = list5;
			list3 = new List<int>();
			list3.Add(0);
			List<int> list7 = list3;
			list3 = new List<int>();
			list3.Add(0);
			List<int> list8 = list3;
			buildCheckArray(root, list2, list4, list6, list7, list8);
			for (int i = 0; i < _wordToPinYin.Length; i++)
			{
				list2.Add(0);
				list4.Add(0);
				list6.Add(0);
				list7.Add(0);
				list8.Add(0);
			}
			_fpy = list2.ToArray();
			_pyIndexBase = list4.ToArray();
			_py = list6.ToArray();
			_wordIndexBase = list7.ToArray();
			_wordCheck = list8.ToArray();
		}

		private void buildCheckArray(ChineseNode root, List<byte> fpy, List<int> pyIndexBase, List<ushort> py, List<int> wordIndexBase, List<int> wordCheck)
		{
			if (root.IsEnd)
			{
				return;
			}
			byte b = byte.MaxValue;
			byte b2 = 0;
			foreach (KeyValuePair<byte, FirstPinYinNode> firstPinYinNode in root.FirstPinYinNodes)
			{
				if (firstPinYinNode.Key < b)
				{
					b = firstPinYinNode.Key;
				}
				if (firstPinYinNode.Key > b2)
				{
					b2 = firstPinYinNode.Key;
				}
			}
			int count = pyIndexBase.Count;
			int num = count - b;
			bool flag = true;
			while (flag)
			{
				flag = false;
				for (int i = 1; i < b; i++)
				{
					if (num + i >= 0)
					{
						if (fpy.Count <= num + i)
						{
							break;
						}
						if (fpy[num + i] == (byte)i)
						{
							flag = true;
							num++;
							break;
						}
					}
				}
			}
			for (int j = count; j <= num + b2; j++)
			{
				fpy.Add(0);
				pyIndexBase.Add(0);
			}
			_fpyIndexBase[root.Index] = num;
			foreach (KeyValuePair<byte, FirstPinYinNode> firstPinYinNode2 in root.FirstPinYinNodes)
			{
				fpy[num + firstPinYinNode2.Key] = firstPinYinNode2.Key;
			}
			List<byte> list = root.FirstPinYinNodes.Keys.OrderByDescending((byte q) => q).ToList();
			foreach (byte item in list)
			{
				FirstPinYinNode root2 = root.FirstPinYinNodes[item];
				buildCheckArray(root2, fpy, pyIndexBase, py, wordIndexBase, wordCheck, num + item);
			}
		}

		private void buildCheckArray(FirstPinYinNode root, List<byte> fpy, List<int> pyIndexBase, List<ushort> py, List<int> wordIndexBase, List<int> wordCheck, int baseIndex)
		{
			ushort num = ushort.MaxValue;
			ushort num2 = 0;
			foreach (KeyValuePair<ushort, PinYinNode> pinYinNode in root.PinYinNodes)
			{
				if (pinYinNode.Key < num)
				{
					num = pinYinNode.Key;
				}
				if (pinYinNode.Key > num2)
				{
					num2 = pinYinNode.Key;
				}
			}
			int count = wordIndexBase.Count;
			int num3 = count - num;
			bool flag = true;
			ushort num4 = _pinYinStart[_pinYinToFirstPinYin[num]];
			while (flag)
			{
				flag = false;
				for (int i = num4; i < num; i++)
				{
					if (num3 + i >= 0)
					{
						if (py.Count <= num3 + i)
						{
							break;
						}
						if (py[num3 + i] == (ushort)i)
						{
							flag = true;
							num3++;
							break;
						}
					}
				}
			}
			for (int j = count; j <= num3 + num2; j++)
			{
				py.Add(0);
				wordIndexBase.Add(0);
			}
			pyIndexBase[baseIndex] = num3;
			foreach (KeyValuePair<ushort, PinYinNode> pinYinNode2 in root.PinYinNodes)
			{
				py[num3 + pinYinNode2.Key] = pinYinNode2.Key;
			}
			List<ushort> list = root.PinYinNodes.Keys.OrderByDescending((ushort q) => q).ToList();
			foreach (ushort item in list)
			{
				PinYinNode root2 = root.PinYinNodes[item];
				buildCheckArray(root2, fpy, pyIndexBase, py, wordIndexBase, wordCheck, num3 + item);
			}
		}

		private void buildCheckArray(PinYinNode root, List<byte> fpy, List<int> pyIndexBase, List<ushort> py, List<int> wordIndexBase, List<int> wordCheck, int baseIndex)
		{
			ushort num = ushort.MaxValue;
			ushort num2 = 0;
			foreach (KeyValuePair<ushort, ChineseNode> chineseNode in root.ChineseNodes)
			{
				if (chineseNode.Key < num)
				{
					num = chineseNode.Key;
				}
				if (chineseNode.Key > num2)
				{
					num2 = chineseNode.Key;
				}
			}
			int count = wordCheck.Count;
			int num3 = count - num;
			bool flag = true;
			ushort num4 = _pinYinStart[_wordToPinYin[num]];
			while (flag)
			{
				flag = false;
				for (int i = num4; i < num; i++)
				{
					if (num3 + i >= 0)
					{
						if (wordCheck.Count <= num3 + i)
						{
							break;
						}
						int num5 = wordCheck[num3 + i];
						if (num5 != 0 && _word[num5] == (ushort)i)
						{
							flag = true;
							num3++;
							break;
						}
					}
				}
			}
			for (int j = count; j <= num3 + num2; j++)
			{
				wordCheck.Add(0);
			}
			wordIndexBase[baseIndex] = num3;
			foreach (KeyValuePair<ushort, ChineseNode> chineseNode2 in root.ChineseNodes)
			{
				wordCheck[num3 + chineseNode2.Key] = chineseNode2.Value.Index;
			}
			List<ushort> list = root.ChineseNodes.Keys.OrderByDescending((ushort q) => q).ToList();
			foreach (ushort item in list)
			{
				ChineseNode root2 = root.ChineseNodes[item];
				buildCheckArray(root2, fpy, pyIndexBase, py, wordIndexBase, wordCheck);
			}
		}

		private ChineseNode buildNode(List<string> keySort, List<string> keywords)
		{
			ChineseNode chineseNode = new ChineseNode();
			List<int> list = new List<int>();
			for (int i = 0; i < keySort.Count; i++)
			{
				string text = keySort[i];
				string[] array = text.Split('\b');
				ChineseNode chineseNode2 = chineseNode;
				for (int j = 0; j < array.Length - 1; j++)
				{
					ushort word = getWord(array[j]);
					chineseNode2 = chineseNode2.AddNode(getFirstPinYin(word), getPinYin(word), word, i);
				}
				list.Add(int.Parse(array[array.Length - 1]));
			}
			_indexs = list.ToArray();
			return chineseNode;
		}

		private ushort getWord(string text)
		{
			string[] array = text.Split('\t');
			ushort num = _toWord1[array[2][0]];
			if (num == ushort.MaxValue)
			{
				string key = array[1] + '\t' + array[2];
				return _toWord2[key];
			}
			return num;
		}

		private ushort getWord(ushort word)
		{
			if (_word_firstPinYin == null)
			{
				_word_firstPinYin = new List<ushort>();
				char c = 'A';
				while (c <= 'Z')
				{
					_word_firstPinYin.Add(_toWord1[c++]);
				}
			}
			if (_word_firstPinYin.Contains(word))
			{
				return 0;
			}
			return word;
		}

		private ushort getPinYin(ushort word)
		{
			if (_word_firstPinYin == null)
			{
				_word_firstPinYin = new List<ushort>();
				char c = 'A';
				while (c <= 'Z')
				{
					_word_firstPinYin.Add(_toWord1[c++]);
				}
			}
			if (_word_firstPinYin.Contains(word))
			{
				return 0;
			}
			return _wordToPinYin[word];
		}

		private byte getFirstPinYin(ushort word)
		{
			return _pinYinToFirstPinYin[_wordToPinYin[word]];
		}

		private void initDictionary(List<string> keywords)
		{
			List<char> list = new List<char>();
			WordNode wordNode = new WordNode();
			for (int i = 0; i < keywords.Count; i++)
			{
				string text = keywords[i].ToUpper();
				string[] pinYinList = PinYinDict.GetPinYinList(text);
				for (int j = 0; j < text.Length; j++)
				{
					char c = text[j];
					if ((c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9') || c == ' ')
					{
						wordNode.Add(c.ToString(), c.ToString(), c.ToString());
					}
					else if (c >= '一' && c <= '龥')
					{
						List<string> allPinYin = PinYinDict.GetAllPinYin(c);
						if (allPinYin.Count > 1)
						{
							list.Add(c);
						}
						foreach (string item in allPinYin)
						{
							wordNode.Add(item[0].ToString(), item.ToUpper(), c.ToString());
						}
						wordNode.Add(pinYinList[j][0].ToString(), pinYinList[j].ToUpper(), c.ToString());
					}
					else
					{
						wordNode.Add(" ", " ", c.ToString());
					}
				}
			}
			wordNode.SetSort2();
			wordNode.SetIndex();
			_toWord1 = new ushort[65536];
			_toWord2 = new Dictionary<string, ushort>();
			_toFirstPinYin = new byte[128];
			_wordToPinYin = new ushort[wordNode.cIndex + 1];
			_pinYinToFirstPinYin = new byte[wordNode.pyIndex + 1];
			_pinYinStart = new ushort[wordNode.pyIndex + 1];
			_pinYinStart[1] = 1;
			_firstPinYinStart = new ushort[wordNode.fpyIndex + 1];
			_firstPinYinStart[1] = 1;
			foreach (WordNode node in wordNode.Nodes)
			{
				_toFirstPinYin[node.Text[0]] = (byte)node.Index;
				foreach (WordNode node2 in node.Nodes)
				{
					_toWord2[node2.Text] = (ushort)node2.Index;
					_pinYinToFirstPinYin[node2.Index] = (byte)node.Index;
					_firstPinYinStart[node.Index + 1] = (ushort)(node2.Index + 1);
					foreach (WordNode node3 in node2.Nodes)
					{
						_wordToPinYin[node3.Index] = (ushort)node2.Index;
						char c2 = node3.Text[0];
						if (list.Contains(c2))
						{
							_toWord1[c2] = ushort.MaxValue;
							_toWord2[node2.Text + '\t' + c2] = (ushort)node3.Index;
						}
						else
						{
							_toWord1[c2] = (ushort)node3.Index;
						}
						_pinYinStart[node2.Index + 1] = (ushort)(node3.Index + 1);
					}
				}
			}
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
