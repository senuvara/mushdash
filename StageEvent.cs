using System;

[Serializable]
public struct StageEvent
{
	public bool flodAction;

	public bool flodTime;

	public StageActionEvent[] actionEvents;

	public StageTimeEvent[] timeEvents;
}
