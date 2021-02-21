using System;
using UnityEngine;

[Serializable]
public struct StageActionEvent
{
	public int nodeIndex;

	public GameObject sceneObject;

	public int bornActionIndex;

	public int hittedActionIndex;

	public int missActionIndex;
}
