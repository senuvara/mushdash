using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EditorData : MonoBehaviour
{
	private static string PATH = "EditorData";

	private static GameObject dataObject;

	private static EditorData instance;

	[SerializeField]
	public string[] SpineActionKeys;

	public string[] SpineActionDes;

	public string[] SpineEventName;

	public string[] SpineEventDes;

	public string[] SpineModeName;

	public string[] SpineModeDes;

	public StageEvent[] StageEvents;

	public static EditorData Instance
	{
		get
		{
			if (instance == null)
			{
				dataObject = Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(PATH);
				instance = dataObject.GetComponent<EditorData>();
			}
			return instance;
		}
	}

	public static GameObject GetDataObject()
	{
		return dataObject;
	}

	public void AddStringListItem(ref string item, ref string[] strList)
	{
		List<string> list = (strList == null || strList.Length <= 0) ? new List<string>() : strList.ToList();
		list.Add(item);
		strList = list.ToArray();
	}

	public void DelStringListItem(int idx, ref string[] strList)
	{
		if (strList != null && strList.Length > idx)
		{
			List<string> list = strList.ToList();
			list.RemoveAt(idx);
			strList = list.ToArray();
		}
	}

	public void AddIntListItem(int item, ref int[] intList)
	{
		List<int> list = (intList == null || intList.Length <= 0) ? new List<int>() : intList.ToList();
		list.Add(item);
		intList = list.ToArray();
	}

	public void DelIntListItem(int idx, ref int[] intList)
	{
		if (intList != null && intList.Length > idx)
		{
			List<int> list = intList.ToList();
			list.RemoveAt(idx);
			intList = list.ToArray();
		}
	}

	public void AddGameObjectListItem(ref GameObject item, ref GameObject[] objList)
	{
		List<GameObject> list = (objList == null || objList.Length <= 0) ? new List<GameObject>() : objList.ToList();
		list.Add(item);
		objList = list.ToArray();
	}

	public void DelGameObjectListItem(int idx, ref GameObject[] objList)
	{
		if (objList != null && objList.Length > idx)
		{
			List<GameObject> list = objList.ToList();
			list.RemoveAt(idx);
			objList = list.ToArray();
		}
	}

	public void AfterSave()
	{
		instance = null;
	}

	public StageEvent GetStageEventDataById(int idx)
	{
		if (StageEvents.Length <= idx)
		{
			for (int i = StageEvents.Length; i < idx + 2; i++)
			{
				StageEvent item = default(StageEvent);
				AddStageEvent(ref item);
			}
		}
		return StageEvents[idx];
	}

	public void AddStageEvent(ref StageEvent item)
	{
		List<StageEvent> list = (StageEvents == null || StageEvents.Length <= 0) ? new List<StageEvent>() : StageEvents.ToList();
		list.Add(item);
		StageEvents = list.ToArray();
	}
}
