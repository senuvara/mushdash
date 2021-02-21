using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using System.Collections.Generic;
using UnityEngine;

public class AdminData : MonoBehaviour
{
	private const string PATH = "AdminData";

	private static GameObject dataObject;

	private static AdminData instance;

	[SerializeField]
	public string defaultStageName;

	[SerializeField]
	public int defualtDifficulty;

	public int defaultClothUid;

	public int defaultElfinUid;

	public List<int> defaultSkills;

	public int defaultPetSkill;

	public static AdminData Instance
	{
		get
		{
			if (instance == null)
			{
				dataObject = Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>("AdminData");
				instance = dataObject.GetComponent<AdminData>();
			}
			return instance;
		}
	}

	public static GameObject GetDataObject()
	{
		return dataObject;
	}

	public void AfterSave()
	{
		instance = null;
	}
}
