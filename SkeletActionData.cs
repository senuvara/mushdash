using System;

[Serializable]
public struct SkeletActionData
{
	public bool collapsed;

	public bool isEndLoop;

	public bool isRandomSequence;

	public bool isSelfProtect;

	public string name;

	public int protectLevel;

	public int spineActionKeyIndex;

	public string[] actionIdx;

	public int[] actionEventIdx;
}
