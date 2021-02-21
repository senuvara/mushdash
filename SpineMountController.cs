using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using GameLogic;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpineMountController : MonoBehaviour
{
	private string[] sklNames;

	private GameObject[] dymanicObjects;

	[SerializeField]
	private SkeletonMountData[] mountData;

	private static bool FindBones(Bone elem, string _name)
	{
		if (elem.Data.Name == _name)
		{
			return true;
		}
		return false;
	}

	private void Start()
	{
		InitSkeletonName();
		dymanicObjects = new GameObject[mountData.Length];
		int i = 0;
		for (int num = mountData.Length; i < num; i++)
		{
			SkeletonMountData skeletonMountData = mountData[i];
			Type type = SpineActionController.TYPE_POLL[skeletonMountData.moduleId];
			if ((type != typeof(DebugInfoController) || GameGlobal.IS_NODE_DEBUG) && !(skeletonMountData.instance == null))
			{
				GameObject gameObject = Singleton<PoolManager>.instance.FastInstantiate(skeletonMountData.instance, 1);
				BoneFollower boneFollower = gameObject.GetComponent<BoneFollower>();
				if (boneFollower == null)
				{
					boneFollower = gameObject.AddComponent<BoneFollower>();
				}
				boneFollower.SkeletonRenderer = base.gameObject.GetComponent<SkeletonAnimation>();
				boneFollower.boneName = sklNames[skeletonMountData.actionId];
				SpineActionController.InitTypePoll();
				gameObject.AddComponent(type);
				dymanicObjects[i] = gameObject;
			}
		}
	}

	public void AddData()
	{
		SkeletonMountData item = default(SkeletonMountData);
		List<SkeletonMountData> list = (mountData == null || mountData.Length <= 0) ? new List<SkeletonMountData>() : mountData.ToList();
		list.Add(item);
		mountData = list.ToArray();
	}

	public void DelData(int idx)
	{
		if (mountData != null && mountData.Length > idx)
		{
			List<SkeletonMountData> list = mountData.ToList();
			list.RemoveAt(idx);
			mountData = list.ToArray();
		}
	}

	public void SetData(int idx, SkeletonMountData d)
	{
		if (mountData != null && mountData.Length > idx)
		{
			mountData[idx] = d;
		}
	}

	public int DataCount()
	{
		if (mountData == null)
		{
			return 0;
		}
		return mountData.Length;
	}

	public SkeletonMountData GetData(int idx)
	{
		return mountData[idx];
	}

	public void DestoryDynamicObjects()
	{
		if (dymanicObjects == null || dymanicObjects.Length == 0)
		{
			return;
		}
		for (int i = 0; i < dymanicObjects.Length; i++)
		{
			GameObject gameObject = dymanicObjects[i];
			if (!(gameObject == null))
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}
	}

	public GameObject[] GetMountObjects()
	{
		return dymanicObjects;
	}

	public GameObject GetMountObjectByIdx(int index)
	{
		if (dymanicObjects == null)
		{
			return null;
		}
		if (index >= dymanicObjects.Length)
		{
			return null;
		}
		return dymanicObjects[index];
	}

	public void OnControllerStart()
	{
		if (dymanicObjects == null || dymanicObjects.Length == 0)
		{
			return;
		}
		for (int i = 0; i < dymanicObjects.Length; i++)
		{
			GameObject gameObject = dymanicObjects[i];
			if (!(gameObject == null))
			{
				BaseSpineObjectController component = gameObject.GetComponent<BaseSpineObjectController>();
				if (component != null)
				{
					component.OnControllerStart();
				}
			}
		}
	}

	private void InitSkeletonName()
	{
		ExposedList<Bone> bones = base.gameObject.GetComponent<SkeletonAnimation>().skeleton.Bones;
		sklNames = new string[bones.Count];
		int i = 0;
		for (int count = bones.Count; i < count; i++)
		{
			sklNames[i] = bones.Items[i].Data.Name;
		}
	}
}
