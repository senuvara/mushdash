using System;

[Serializable]
public struct StageTimeEvent
{
	public bool flod;

	public float time;

	public StageTimeEventItem[] eventItems;
}
