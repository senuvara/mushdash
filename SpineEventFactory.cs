using GameLogic;
using Spine;
using System;
using System.Reflection;
using UnityEngine;

public class SpineEventFactory : MonoBehaviour
{
	private int idx;

	private static Type[] TYPE_POLL;

	private DoNothing[] eventObjects;

	private static string GAME_SPACE = "GameLogic.";

	private static Assembly assembly = Assembly.Load("Assembly-CSharp");

	private void Start()
	{
		Init();
	}

	public void Init()
	{
		if (TYPE_POLL == null || TYPE_POLL.Length == 0)
		{
			TYPE_POLL = new Type[EditorData.Instance.SpineEventName.Length];
			for (int i = 0; i < EditorData.Instance.SpineEventName.Length; i++)
			{
				string str = EditorData.Instance.SpineEventName[i];
				TYPE_POLL[i] = assembly.GetType(GAME_SPACE + str);
			}
		}
		if (eventObjects != null && eventObjects.Length > 0)
		{
			return;
		}
		eventObjects = new DoNothing[EditorData.Instance.SpineEventName.Length];
		for (int j = 0; j < TYPE_POLL.Length; j++)
		{
			Type type = TYPE_POLL[j];
			if (type != null)
			{
				DoNothing doNothing = (DoNothing)type.Assembly.CreateInstance(type.ToString());
				if (doNothing != null)
				{
					doNothing.SetIdx(idx);
					doNothing.SetGameObject(base.gameObject);
					doNothing.Init();
					eventObjects[j] = doNothing;
				}
			}
		}
	}

	public void SetIdx(int idx)
	{
		this.idx = idx;
		Init();
		for (int i = 0; i < eventObjects.Length; i++)
		{
			DoNothing doNothing = eventObjects[i];
			if (doNothing != null)
			{
				doNothing.SetIdx(idx);
				doNothing.Init();
			}
		}
	}

	public int DataCount()
	{
		if (eventObjects == null)
		{
			return 0;
		}
		return eventObjects.Length;
	}

	public Spine.AnimationState.TrackEntryDelegate GetFunc(int idx)
	{
		if (eventObjects == null || idx >= eventObjects.Length)
		{
			return null;
		}
		return eventObjects[idx].Do;
	}

	public static Spine.AnimationState.TrackEntryDelegate GetFunction(GameObject obj, int idx)
	{
		if (obj == null)
		{
			return null;
		}
		SpineEventFactory component = obj.GetComponent<SpineEventFactory>();
		if (component == null)
		{
			return null;
		}
		return component.GetFunc(idx);
	}
}
