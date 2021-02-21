using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ToolGood.Words
{
	public class StringSearchEx
	{
		private class TrieNode
		{
			public char Char;

			internal bool End;

			internal List<int> Results;

			internal Dictionary<char, TrieNode> m_values;

			internal Dictionary<char, TrieNode> merge_values;

			private uint minflag = uint.MaxValue;

			private uint maxflag;

			internal int Next;

			private int Count;

			public TrieNode()
			{
				m_values = new Dictionary<char, TrieNode>();
				merge_values = new Dictionary<char, TrieNode>();
				Results = new List<int>();
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
				value.Char = c;
				m_values[c] = value;
				Count++;
				return value;
			}

			public void SetResults(int text)
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
					foreach (int result in node.Results)
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
						if (!merge_values.ContainsKey(value2.Key))
						{
							merge_values[value2.Key] = value2.Value;
							Count++;
						}
					}
				}
				TrieNode value;
				if (links.TryGetValue(node, out value))
				{
					Merge(value, links);
				}
			}

			public int GetMaxLength()
			{
				int num = m_values.Count + merge_values.Count;
				num *= 5;
				foreach (KeyValuePair<char, TrieNode> value in m_values)
				{
					num += value.Value.GetMaxLength();
				}
				return num;
			}

			public int Rank(TrieNode[] has)
			{
				bool[] seats = new bool[has.Length];
				int maxCount = 1;
				int start = 1;
				has[0] = this;
				Rank(ref maxCount, ref start, seats, has);
				return maxCount;
			}

			private void Rank(ref int maxCount, ref int start, bool[] seats, TrieNode[] has)
			{
				if (maxflag == 0)
				{
					return;
				}
				List<char> list = m_values.Select((KeyValuePair<char, TrieNode> q) => q.Key).ToList();
				list.AddRange(merge_values.Select((KeyValuePair<char, TrieNode> q) => q.Key).ToList());
				while (has[start] != null)
				{
					start++;
				}
				for (int i = start; i < has.Length; i++)
				{
					if (has[i] != null)
					{
						continue;
					}
					int num = i - (int)minflag;
					if (num < 0 || seats[num])
					{
						continue;
					}
					bool flag = true;
					foreach (char item in list)
					{
						if (has[i - minflag + (int)item] != null)
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						SetSeats(num, ref maxCount, seats, has);
						break;
					}
				}
				IOrderedEnumerable<KeyValuePair<char, TrieNode>> orderedEnumerable = from q in m_values
					orderby q.Value.Count descending, q.Value.maxflag - q.Value.minflag descending
					select q;
				foreach (KeyValuePair<char, TrieNode> item2 in orderedEnumerable)
				{
					item2.Value.Rank(ref maxCount, ref start, seats, has);
				}
			}

			private void SetSeats(int next, ref int maxCount, bool[] seats, TrieNode[] has)
			{
				Next = next;
				seats[next] = true;
				foreach (KeyValuePair<char, TrieNode> merge_value in merge_values)
				{
					int num = next + merge_value.Key;
					has[num] = merge_value.Value;
				}
				foreach (KeyValuePair<char, TrieNode> value in m_values)
				{
					int num2 = next + value.Key;
					has[num2] = value.Value;
				}
				int num3 = next + (int)maxflag;
				if (maxCount <= num3)
				{
					maxCount = num3;
				}
			}
		}

		private string[] _keywords;

		private int[][] _guides;

		private int[] _key;

		private int[] _next;

		private int[] _check;

		private int[] _dict;

		public List<string> FindAll(string text)
		{
			List<string> list = new List<string>();
			int num = 0;
			foreach (char c in text)
			{
				char c2 = (char)_dict[c];
				if (c2 == '\0')
				{
					num = 0;
					continue;
				}
				int num2 = _next[num] + c2;
				bool flag = _key[num2] == c2;
				if (!flag && num != 0)
				{
					num = 0;
					num2 = _next[0] + c2;
					flag = (_key[num2] == c2);
				}
				if (!flag)
				{
					continue;
				}
				int num3 = _check[num2];
				if (num3 > 0)
				{
					int[] array = _guides[num3];
					foreach (int num4 in array)
					{
						list.Add(_keywords[num4]);
					}
				}
				num = num2;
			}
			return list;
		}

		public string FindFirst(string text)
		{
			int num = 0;
			foreach (char c in text)
			{
				char c2 = (char)_dict[c];
				if (c2 == '\0')
				{
					num = 0;
					continue;
				}
				int num2 = _next[num] + c2;
				if (_key[num2] == c2)
				{
					int num3 = _check[num2];
					if (num3 > 0)
					{
						return _keywords[_guides[num3][0]];
					}
					num = num2;
					continue;
				}
				num = 0;
				num2 = _next[num] + c2;
				if (_key[num2] == c2)
				{
					int num4 = _check[num2];
					if (num4 > 0)
					{
						return _keywords[_guides[num4][0]];
					}
					num = num2;
				}
			}
			return null;
		}

		public bool ContainsAny(string text)
		{
			int num = 0;
			foreach (char c in text)
			{
				char c2 = (char)_dict[c];
				if (c2 == '\0')
				{
					num = 0;
					continue;
				}
				int num2 = _next[num] + c2;
				if (_key[num2] == c2)
				{
					if (_check[num2] > 0)
					{
						return true;
					}
					num = num2;
					continue;
				}
				num = 0;
				num2 = _next[num] + c2;
				if (_key[num2] == c2)
				{
					if (_check[num2] > 0)
					{
						return true;
					}
					num = num2;
				}
			}
			return false;
		}

		public string Replace(string text, char replaceChar = '*')
		{
			StringBuilder stringBuilder = new StringBuilder(text);
			int num = 0;
			for (int i = 0; i < text.Length; i++)
			{
				char c = (char)_dict[text[i]];
				if (c == '\0')
				{
					num = 0;
					continue;
				}
				int num2 = _next[num] + c;
				bool flag = _key[num2] == c;
				if (!flag && num != 0)
				{
					num = 0;
					num2 = _next[num] + c;
					flag = (_key[num2] == c);
				}
				if (!flag)
				{
					continue;
				}
				int num3 = _check[num2];
				if (num3 > 0)
				{
					int length = _keywords[_guides[num3][0]].Length;
					int num4 = i + 1 - length;
					for (int j = num4; j <= i; j++)
					{
						stringBuilder[j] = replaceChar;
					}
				}
				num = num2;
			}
			return stringBuilder.ToString();
		}

		public void Save(string fileName)
		{
			FileStream fileStream = File.Open(fileName, FileMode.Create);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			binaryWriter.Write(_keywords.Length);
			string[] keywords = _keywords;
			foreach (string value in keywords)
			{
				binaryWriter.Write(value);
			}
			List<int> list = new List<int>();
			list.Add(_guides.Length);
			int[][] guides = _guides;
			foreach (int[] array in guides)
			{
				list.Add(array.Length);
				int[] array2 = array;
				foreach (int item in array2)
				{
					list.Add(item);
				}
			}
			byte[] array3 = IntArrToByteArr(list.ToArray());
			binaryWriter.Write(array3.Length);
			binaryWriter.Write(array3);
			array3 = IntArrToByteArr(_key);
			binaryWriter.Write(array3.Length);
			binaryWriter.Write(array3);
			array3 = IntArrToByteArr(_next);
			binaryWriter.Write(array3.Length);
			binaryWriter.Write(array3);
			array3 = IntArrToByteArr(_check);
			binaryWriter.Write(array3.Length);
			binaryWriter.Write(array3);
			array3 = IntArrToByteArr(_dict);
			binaryWriter.Write(array3.Length);
			binaryWriter.Write(array3);
			binaryWriter.Close();
			fileStream.Close();
		}

		private byte[] IntArrToByteArr(int[] intArr)
		{
			int num = 4 * intArr.Length;
			byte[] array = new byte[num];
			IntPtr intPtr = Marshal.AllocHGlobal(num);
			Marshal.Copy(intArr, 0, intPtr, intArr.Length);
			Marshal.Copy(intPtr, array, 0, array.Length);
			Marshal.FreeHGlobal(intPtr);
			return array;
		}

		public void Load(string filePath)
		{
			FileStream fileStream = File.OpenRead(filePath);
			BinaryReader binaryReader = new BinaryReader(fileStream);
			int num = binaryReader.ReadInt32();
			_keywords = new string[num];
			for (int i = 0; i < num; i++)
			{
				_keywords[i] = binaryReader.ReadString();
			}
			num = binaryReader.ReadInt32();
			byte[] buffer = binaryReader.ReadBytes(num);
			using (MemoryStream input = new MemoryStream(buffer))
			{
				BinaryReader binaryReader2 = new BinaryReader(input);
				int num2 = binaryReader2.ReadInt32();
				_guides = new int[num2][];
				for (int j = 0; j < num2; j++)
				{
					int num3 = binaryReader2.ReadInt32();
					_guides[j] = new int[num3];
					for (int k = 0; k < num3; k++)
					{
						_guides[j][k] = binaryReader2.ReadInt32();
					}
				}
			}
			num = binaryReader.ReadInt32();
			_key = ByteArrToIntArr(binaryReader.ReadBytes(num));
			num = binaryReader.ReadInt32();
			_next = ByteArrToIntArr(binaryReader.ReadBytes(num));
			num = binaryReader.ReadInt32();
			_check = ByteArrToIntArr(binaryReader.ReadBytes(num));
			num = binaryReader.ReadInt32();
			_dict = ByteArrToIntArr(binaryReader.ReadBytes(num));
			binaryReader.Close();
			fileStream.Close();
		}

		private int[] ByteArrToIntArr(byte[] btArr)
		{
			int num = btArr.Length / 4;
			int[] array = new int[num];
			IntPtr intPtr = Marshal.AllocHGlobal(btArr.Length);
			Marshal.Copy(btArr, 0, intPtr, btArr.Length);
			Marshal.Copy(intPtr, array, 0, array.Length);
			Marshal.FreeHGlobal(intPtr);
			return array;
		}

		public void SetKeywords(ICollection<string> keywords)
		{
			_keywords = keywords.ToArray();
			int length = CreateDict(keywords);
			TrieNode trieNode = new TrieNode();
			for (int i = 0; i < _keywords.Length; i++)
			{
				string text = _keywords[i];
				TrieNode trieNode2 = trieNode;
				for (int j = 0; j < text.Length; j++)
				{
					trieNode2 = trieNode2.Add((char)_dict[text[j]]);
				}
				trieNode2.SetResults(i);
			}
			Dictionary<TrieNode, TrieNode> dictionary = new Dictionary<TrieNode, TrieNode>();
			foreach (KeyValuePair<char, TrieNode> value in trieNode.m_values)
			{
				TryLinks(value.Value, null, dictionary, trieNode);
			}
			foreach (KeyValuePair<TrieNode, TrieNode> item in dictionary)
			{
				item.Key.Merge(item.Value, dictionary);
			}
			build(trieNode, length);
		}

		private void build(TrieNode root, int length)
		{
			TrieNode[] array = new TrieNode[root.GetMaxLength()];
			length = root.Rank(array) + length + 1;
			_key = new int[length];
			_next = new int[length];
			_check = new int[length];
			List<int[]> list = new List<int[]>();
			list.Add(new int[1]);
			for (int i = 0; i < length; i++)
			{
				TrieNode trieNode = array[i];
				if (trieNode != null)
				{
					_key[i] = trieNode.Char;
					_next[i] = trieNode.Next;
					if (trieNode.End)
					{
						_check[i] = list.Count;
						list.Add(trieNode.Results.ToArray());
					}
				}
			}
			_guides = list.ToArray();
		}

		private void TryLinks(TrieNode node, TrieNode node2, Dictionary<TrieNode, TrieNode> links, TrieNode root)
		{
			foreach (KeyValuePair<char, TrieNode> value in node.m_values)
			{
				TrieNode node3 = null;
				if (node2 == null)
				{
					if (root.TryGetValue(value.Key, out node3))
					{
						links[value.Value] = node3;
					}
				}
				else if (node2.TryGetValue(value.Key, out node3))
				{
					links[value.Value] = node3;
				}
				TryLinks(value.Value, node3, links, root);
			}
		}

		private int CreateDict(ICollection<string> keywords)
		{
			Dictionary<char, int> dictionary = new Dictionary<char, int>();
			foreach (string keyword in keywords)
			{
				for (int i = 0; i < keyword.Length; i++)
				{
					char key = keyword[i];
					if (dictionary.ContainsKey(key))
					{
						if (i > 0)
						{
							dictionary[key] += 2;
						}
					}
					else
					{
						dictionary[key] = ((i <= 0) ? 1 : 2);
					}
				}
			}
			List<char> list = (from q in dictionary
				orderby q.Value descending
				select q.Key).ToList();
			List<char> list2 = new List<char>();
			bool flag = false;
			foreach (char item in list)
			{
				if (flag)
				{
					list2.Add(item);
				}
				else
				{
					list2.Insert(0, item);
				}
				flag = !flag;
			}
			_dict = new int[65536];
			for (int j = 0; j < list2.Count; j++)
			{
				_dict[list2[j]] = j + 1;
			}
			return dictionary.Count;
		}
	}
}
