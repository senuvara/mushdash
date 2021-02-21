using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using FormulaBase;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Boss
{
	private static Boss instance;

	private GameObject bossObject;

	private Dictionary<string, GameObject> bossObjects;

	private string bossName;

	public SpineActionController spineActionController
	{
		get;
		private set;
	}

	public Animator animator
	{
		get;
		private set;
	}

	public GameObject go
	{
		get
		{
			if (Singleton<StageBattleComponent>.instance.isSceneChangeType)
			{
				return bossObjects[bossName];
			}
			return bossObject;
		}
	}

	public static Boss Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new Boss();
			}
			return instance;
		}
	}

	public Boss()
	{
		bossObject = null;
		bossObjects = null;
	}

	public static void ReleaseReferences()
	{
		instance.bossObject = null;
		instance.spineActionController = null;
		instance.animator = null;
		instance.bossObjects = null;
	}

	public GameObject GetGameObject()
	{
		if (Singleton<StageBattleComponent>.instance.isSceneChangeType)
		{
			return bossObjects[bossName];
		}
		return bossObject;
	}

	public string GetBossName()
	{
		return bossName;
	}

	public void SceneBossChange(int idx)
	{
		string b = BossFestival("0" + idx + "01_boss");
		if (bossName == b)
		{
			return;
		}
		bossName = b;
		string curActionKey = spineActionController.curActionKey;
		float num = 0f;
		bossObjects[bossName].SetActive(bossObject.activeSelf);
		spineActionController = bossObjects[bossName].GetComponent<SpineActionController>();
		spineActionController.GetCurrentAnimationName();
		Animator component = bossObjects[bossName].GetComponent<Animator>();
		if ((bool)component)
		{
			animator = component;
		}
		bossObject.SetActive(false);
		bossObject = bossObjects[bossName];
		if (curActionKey == "multi_atk_hurt_end" || curActionKey == "multi_atk_48_end")
		{
			if (curActionKey == "multi_atk_48_end")
			{
				num = 2f;
			}
			string actionKey = curActionKey;
			int idx2 = -2;
			float time = num;
			SpineActionController.PlaySkeletonAnim(actionKey, idx2, false, true, 0f, time);
		}
		else
		{
			SpineActionController.Play(curActionKey, -2, num);
		}
	}

	public void Pause()
	{
		if ((bool)animator)
		{
			animator.enabled = false;
		}
		if ((bool)spineActionController)
		{
			spineActionController.Pause();
		}
	}

	public void Resume()
	{
		if ((bool)animator)
		{
			animator.enabled = true;
		}
		if ((bool)spineActionController)
		{
			spineActionController.Resume();
		}
	}

	public void SetBoss()
	{
		if ((bool)bossObject)
		{
			return;
		}
		string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("boss", "scene_name", "boss_name", Singleton<StageBattleComponent>.instance.GetSceneName());
		configStringValue = (bossName = BossFestival(configStringValue));
		if (Singleton<StageBattleComponent>.instance.isSceneChangeType)
		{
			List<string> curSceneChangeType = Singleton<StageBattleComponent>.instance.curSceneChangeType;
			bossObjects = new Dictionary<string, GameObject>();
			for (int i = 0; i < curSceneChangeType.Count; i++)
			{
				string text = BossFestival("0" + Singleton<StageBattleComponent>.instance.sceneInfo[curSceneChangeType[i]] + "01_boss");
				if (text != bossName)
				{
					InitBossObject(text);
				}
			}
			InitBossObject(configStringValue);
		}
		else
		{
			InitBossObject(configStringValue);
		}
	}

	public void Play(string key)
	{
		bossObject.SetActive(true);
		SpineActionController.Play(key, -2, 0f, false);
		switch (key)
		{
		case "boss_far_atk_2":
			if (bossName == "0101_boss")
			{
				Singleton<EventManager>.instance.Invoke("Battle/OnBoss01Shot");
			}
			else if (bossName == "0201_boss")
			{
				Singleton<EventManager>.instance.Invoke("Battle/OnBoss02Shot01");
			}
			else if (bossName == "0301_boss")
			{
				Singleton<EventManager>.instance.Invoke("Battle/OnBoss03Shot");
			}
			else if (bossName == "0601_boss")
			{
				Singleton<EventManager>.instance.Invoke("Battle/OnBoss06Shot");
			}
			break;
		case "boss_far_atk_1_L":
		case "boss_far_atk_1_R":
			if (bossName == "0501_boss" || bossName == "0501_boss_christmas")
			{
				Singleton<EventManager>.instance.Invoke("Battle/OnBoss05Shot");
			}
			else if (bossName == "0201_boss")
			{
				Singleton<EventManager>.instance.Invoke("Battle/OnBoss02Shot02");
			}
			break;
		}
	}

	private string BossFestival(string bossFestivalName)
	{
		if (bossFestivalName == "0501_boss" && ((DateTime.Now.Month == 12 && DateTime.Now.Day == 24) || (DateTime.Now.Month == 12 && DateTime.Now.Day == 25)))
		{
			bossFestivalName = "0501_boss_christmas";
		}
		return bossFestivalName;
	}

	private void InitBossObject(string name)
	{
		bossObject = UnityEngine.Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(name));
		bossObject.name = name;
		if (bossObject == null)
		{
			Debug.Log("No such boss " + bossName);
			return;
		}
		Debug.Log("Load boss : " + bossName);
		bossObject.transform.SetParent(SingletonMonoBehaviour<SceneObjectController>.instance.transform, false);
		SpineActionController component = bossObject.GetComponent<SpineActionController>();
		component.Init(-1);
		spineActionController = component;
		Animator component2 = bossObject.GetComponent<Animator>();
		if ((bool)component2)
		{
			animator = component2;
		}
		bossObject.SetActive(false);
		if (Singleton<StageBattleComponent>.instance.isSceneChangeType)
		{
			bossObjects.Add(name, bossObject);
		}
	}
}
