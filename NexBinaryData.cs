using System;
using System.Collections.Generic;
using UnityEngine;

public class NexBinaryData
{
	public static List<byte> GetBinaryData(string characterUid, string elfinUid, int hp, int combo, string evaluate, int miss)
	{
		List<byte> list = new List<byte>();
		string text = characterUid + "," + elfinUid + "," + hp + "," + combo + "," + evaluate + "," + miss + ",";
		string text2 = text;
		foreach (char value in text2)
		{
			list.Add(Convert.ToByte(value));
		}
		if (list.Count > 100)
		{
			Debug.LogErrorFormat("Overflow Binary Data [{0}]", list.Count);
		}
		return list;
	}

	public static List<string> GetObject(List<byte> binaryData)
	{
		List<string> list = new List<string>();
		string text = string.Empty;
		foreach (byte binaryDatum in binaryData)
		{
			if (Convert.ToChar(binaryDatum) == ',')
			{
				list.Add(text);
				text = string.Empty;
			}
			else
			{
				text += Convert.ToChar(binaryDatum);
			}
		}
		return list;
	}
}
