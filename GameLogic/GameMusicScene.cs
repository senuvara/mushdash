using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using DYUnityLib;
using FormulaBase;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic
{
	public class GameMusicScene
	{
		private struct Node
		{
			public Transform current;

			public int currentId;

			public int countOfThisLine;
		}

		public FixUpdateTimer stepTimer;

		public GameObject[] preloads;

		public NodeInitController[] nodeInitCtrls;

		public SpineActionController[] spineActionCtrls;

		public BaseSpineObjectController[] objCtrls;

		public Animator[] animators;

		public SkeletonAnimation[] animations;

		public SpineMountController[] spineMountCtrls;

		public List<GameObject>[] preloads1;

		public List<NodeInitController>[] nodeInitCtrls1;

		public List<SpineActionController>[] spineActionCtrls1;

		public List<BaseSpineObjectController>[] objCtrls1;

		public List<Animator>[] animators1;

		public List<SkeletonAnimation>[] animations1;

		public List<SpineMountController>[] spineMountCtrls1;

		public List<bool>[] showEffects;

		public bool firstIn = true;

		public List<GameObject> scenes;

		public string curSceneName;

		private static GameObject sceneChangeObj;

		public int passIdx;

		public int prePassIdx;

		public static GameMusicScene instance => GameGlobal.gGameMusicScene;

		public GameObject scene
		{
			get;
			private set;
		}

		public Material catchMaterial
		{
			get;
			private set;
		}

		public Sprite catchGroundSprite
		{
			get;
			private set;
		}

		public Sprite catchAirSprite
		{
			get;
			private set;
		}

		public Color groundCatchCircleColor
		{
			get;
			private set;
		}

		public Color groundCatchCircle2Color
		{
			get;
			private set;
		}

		public Color airCatchCircleColor
		{
			get;
			private set;
		}

		public Color airCatchCircle2Color
		{
			get;
			private set;
		}

		public GameObject SecneObject => SingletonMonoBehaviour<SceneObjectController>.instance.gameObject;

		public static void ReleaseReferences()
		{
			instance.preloads = null;
			instance.nodeInitCtrls = null;
			instance.spineActionCtrls = null;
			instance.objCtrls = null;
			instance.animators = null;
			instance.animations = null;
			instance.spineMountCtrls = null;
			instance.scene = null;
		}

		public void SetTimer(FixUpdateTimer timer)
		{
			stepTimer = timer;
		}

		public void Run()
		{
			if (stepTimer == null)
			{
				Debug.Log("Run scene with a null timer.");
				return;
			}
			stepTimer.Run();
			if (SingletonMonoBehaviour<SceneObjectController>.instance != null)
			{
				SingletonMonoBehaviour<SceneObjectController>.instance.Run();
			}
		}

		public void Stop()
		{
			if (stepTimer == null)
			{
				Debug.Log("Stop scene with a null timer.");
			}
			else
			{
				stepTimer.Cancel();
			}
		}

		public void Reset()
		{
			decimal total = 240m;
			passIdx = 0;
			InitTimer(total);
		}

		public void LoadScene(int idx, string sceneName)
		{
			sceneChangeObj = new GameObject();
			sceneName = SceneFestival(sceneName);
			if (Singleton<StageBattleComponent>.instance.isSceneChangeType)
			{
				List<string> curSceneChangeType = Singleton<StageBattleComponent>.instance.curSceneChangeType;
				scenes = new List<GameObject>();
				curSceneName = sceneName;
				InitSceneObject(sceneName, true);
				for (int i = 0; i < curSceneChangeType.Count; i++)
				{
					string text = SceneFestival("scene_0" + Singleton<StageBattleComponent>.instance.sceneInfo[curSceneChangeType[i]]);
					if (text != curSceneName)
					{
						InitSceneObject(text);
					}
				}
			}
			else
			{
				InitSceneObject(sceneName);
			}
			PreLoad();
			InitEventTrigger();
			Reset();
			Debug.Log("Load scene " + sceneName);
			if (scene.name == "scene_07")
			{
				Singleton<BattleProperty>.instance.isGCScene = true;
			}
		}

		public void Replace(int idx, int curScene)
		{
			preloads[idx] = preloads1[idx][curScene];
			nodeInitCtrls[idx] = nodeInitCtrls1[idx][curScene];
			spineActionCtrls[idx] = spineActionCtrls1[idx][curScene];
			objCtrls[idx] = objCtrls1[idx][curScene];
			if (animations.Length > idx && animations1.Length > idx && animations1[idx] != null)
			{
				animations[idx] = animations1[idx][curScene];
			}
			if (animators.Length > idx && animators1.Length > idx && animators1[idx] != null)
			{
				animators[idx] = animators1[idx][curScene];
			}
			if (spineMountCtrls.Length > idx && spineMountCtrls1.Length > idx && spineMountCtrls1[idx] != null)
			{
				spineMountCtrls[idx] = spineMountCtrls1[idx][curScene];
			}
		}

		private string SceneFestival(string sceneFestivalName)
		{
			if (sceneFestivalName == "scene_05" && ((DateTime.Now.Month == 12 && DateTime.Now.Day == 24) || (DateTime.Now.Month == 12 && DateTime.Now.Day == 25)))
			{
				sceneFestivalName = "scene_05_christmas";
			}
			return sceneFestivalName;
		}

		public void SceneEventTrigger(object sender, uint triggerId, params object[] args)
		{
			decimal ts = (decimal)args[0];
			List<SceneEvent> list = MusicConfigReader.Instance.sceneEvents.FindAll((SceneEvent s) => s.time == ts);
			for (int i = 0; i < list.Count; i++)
			{
				SceneEvent sceneEvent = list[i];
				if (!string.IsNullOrEmpty(sceneEvent.uid))
				{
					if (sceneEvent.uid == "SceneEvent/OnBPMChanged")
					{
						MusicConfigReader.Instance.bpm = float.Parse(sceneEvent.value.ToString());
					}
					Singleton<EventManager>.instance.Invoke(sceneEvent.uid);
				}
			}
		}

		public void AddObjTrigger(object sender, uint triggerId, params object[] args)
		{
			decimal genTick = (decimal)args[0];
			int musicIndexByGenTick = GameGlobal.gGameMusic.GetMusicIndexByGenTick(genTick);
			if (musicIndexByGenTick >= 0)
			{
				Singleton<BattleEnemyManager>.instance.CreateBattleEnemy(musicIndexByGenTick);
			}
		}

		public void OnObjRun(int idx)
		{
			if (!(SingletonMonoBehaviour<SceneObjectController>.instance == null))
			{
				string nodeUidByIdx = Singleton<BattleEnemyManager>.instance.GetNodeUidByIdx(idx);
				if (nodeUidByIdx != null)
				{
					SingletonMonoBehaviour<SceneObjectController>.instance.DoObjectComeoutEvent(nodeUidByIdx);
				}
			}
		}

		public void OnObjBeAttacked(int idx)
		{
			if (!(SingletonMonoBehaviour<SceneObjectController>.instance == null))
			{
				string nodeUidByIdx = Singleton<BattleEnemyManager>.instance.GetNodeUidByIdx(idx);
				if (nodeUidByIdx != null)
				{
					SingletonMonoBehaviour<SceneObjectController>.instance.DoObjectOnAttackedEvent(nodeUidByIdx);
				}
			}
		}

		public void OnObjBeMissed(int idx)
		{
			if (!(SingletonMonoBehaviour<SceneObjectController>.instance == null))
			{
				string nodeUidByIdx = Singleton<BattleEnemyManager>.instance.GetNodeUidByIdx(idx);
				if (nodeUidByIdx != null)
				{
					SingletonMonoBehaviour<SceneObjectController>.instance.DoObjectOnMissedEvent(nodeUidByIdx);
				}
			}
		}

		public GameObject GetPreLoadGameObject(int idx)
		{
			if (idx >= preloads.Length)
			{
				return null;
			}
			return preloads[idx];
		}

		public List<GameObject> GetPreLoadTmpGameObject(int idx)
		{
			if (idx >= preloads1.Length)
			{
				return null;
			}
			return preloads1[idx];
		}

		public void ChangeAnimationSpeed(float value)
		{
			if (SingletonMonoBehaviour<SceneObjectController>.instance == null)
			{
				return;
			}
			Animator[] componentsInChildren = SingletonMonoBehaviour<SceneObjectController>.instance.GetComponentsInChildren<Animator>();
			if (componentsInChildren != null)
			{
				Animator[] array = componentsInChildren;
				foreach (Animator animator in array)
				{
					float num2 = animator.speed = Mathf.Max(0f, animator.speed + value);
				}
			}
		}

		public void SetAnimationSpeed(float value)
		{
			if (SingletonMonoBehaviour<SceneObjectController>.instance == null)
			{
				return;
			}
			Animator[] componentsInChildren = SingletonMonoBehaviour<SceneObjectController>.instance.GetComponentsInChildren<Animator>();
			if (componentsInChildren != null)
			{
				Animator[] array = componentsInChildren;
				foreach (Animator animator in array)
				{
					animator.speed = value;
				}
			}
		}

		public GameObject PreLoad(int idx)
		{
			MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
			if (Singleton<StageBattleComponent>.instance.isSceneChangeType && musicDataByIdx.noteData.sceneChangeNames != null)
			{
				Dictionary<string, int> sceneInfo = Singleton<StageBattleComponent>.instance.sceneInfo;
				GameObject gameObject = UnityEngine.Object.Instantiate(sceneChangeObj, Vector3.zero, Quaternion.identity, SingletonMonoBehaviour<SceneObjectController>.instance.transform);
				gameObject.name = musicDataByIdx.noteData.prefab_name + idx;
				foreach (string sceneChangeName in musicDataByIdx.noteData.sceneChangeNames)
				{
					int curScene = sceneInfo[sceneChangeName];
					if (int.Parse(musicDataByIdx.noteData.prefab_name[1].ToString()) != sceneInfo[sceneChangeName])
					{
						string text = musicDataByIdx.noteData.prefab_name.Remove(1, 1);
						text = text.Insert(1, curScene.ToString());
						PreloadGameObject(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(text), musicDataByIdx, curScene).transform.parent = gameObject.transform;
					}
				}
				int curScene2 = int.Parse(musicDataByIdx.noteData.prefab_name[1].ToString());
				PreloadGameObject(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(musicDataByIdx.noteData.prefab_name), musicDataByIdx, curScene2).transform.parent = gameObject.transform;
				return gameObject;
			}
			return PreloadGameObject(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(musicDataByIdx.noteData.prefab_name), musicDataByIdx, -1);
		}

		private GameObject PreloadTmpGameObject(GameObject go, MusicData md, int curScene)
		{
			if (curScene == -1 || md.isLongPressType || md.isMul)
			{
				return go;
			}
			short objId = md.objId;
			if (preloads1.Length <= objId)
			{
				Array.Resize(ref preloads1, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
			}
			if (preloads1[objId] == null)
			{
				preloads1[objId] = new List<GameObject>
				{
					null,
					null,
					null,
					null,
					null,
					null,
					null
				};
			}
			preloads1[objId][curScene] = go;
			NodeInitController orAddComponent = go.GetOrAddComponent<NodeInitController>();
			if ((bool)orAddComponent)
			{
				if (nodeInitCtrls1.Length <= objId)
				{
					Array.Resize(ref nodeInitCtrls1, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
				}
				if (nodeInitCtrls1[objId] == null)
				{
					nodeInitCtrls1[objId] = new List<NodeInitController>
					{
						null,
						null,
						null,
						null,
						null,
						null,
						null
					};
				}
				nodeInitCtrls1[objId][curScene] = orAddComponent;
			}
			SpineActionController orAddComponent2 = go.GetOrAddComponent<SpineActionController>();
			if ((bool)orAddComponent2)
			{
				if (spineActionCtrls1.Length <= objId)
				{
					Array.Resize(ref spineActionCtrls1, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
				}
				if (spineActionCtrls1[objId] == null)
				{
					spineActionCtrls1[objId] = new List<SpineActionController>
					{
						null,
						null,
						null,
						null,
						null,
						null,
						null
					};
				}
				spineActionCtrls1[objId][curScene] = orAddComponent2;
			}
			BaseSpineObjectController objController = orAddComponent2.objController;
			if ((bool)objController)
			{
				if (objCtrls1.Length <= objId)
				{
					Array.Resize(ref objCtrls1, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
				}
				if (objCtrls1[objId] == null)
				{
					objCtrls1[objId] = new List<BaseSpineObjectController>
					{
						null,
						null,
						null,
						null,
						null,
						null,
						null
					};
				}
				objCtrls1[objId][curScene] = objController;
			}
			Animator component = go.GetComponent<Animator>();
			if ((bool)component)
			{
				if (animators1.Length <= objId)
				{
					Array.Resize(ref animators1, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
				}
				if (animators1[objId] == null)
				{
					animators1[objId] = new List<Animator>
					{
						null,
						null,
						null,
						null,
						null,
						null,
						null
					};
				}
				animators1[objId][curScene] = component;
			}
			SkeletonAnimation component2 = go.GetComponent<SkeletonAnimation>();
			if ((bool)component2)
			{
				if (animations1.Length <= objId)
				{
					Array.Resize(ref animations1, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
				}
				if (animations1[objId] == null)
				{
					animations1[objId] = new List<SkeletonAnimation>
					{
						null,
						null,
						null,
						null,
						null,
						null,
						null
					};
				}
				animations1[objId][curScene] = component2;
			}
			SpineMountController component3 = go.GetComponent<SpineMountController>();
			if ((bool)component3)
			{
				if (spineMountCtrls1.Length <= objId)
				{
					Array.Resize(ref spineMountCtrls1, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
				}
				if (spineMountCtrls1[objId] == null)
				{
					spineMountCtrls1[objId] = new List<SpineMountController>
					{
						null,
						null,
						null,
						null,
						null,
						null,
						null
					};
				}
				spineMountCtrls1[objId][curScene] = component3;
			}
			return go;
		}

		private GameObject PreloadGameObject(GameObject preObj, MusicData md, int curScene)
		{
			if (preObj == null)
			{
				Debug.Log("Enemy " + md.objId + " preload has no resource : " + md.noteData.prefab_name);
				return null;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(preObj, Vector3.right * 1000f, Quaternion.identity, SingletonMonoBehaviour<SceneObjectController>.instance.transform);
			short objId = md.objId;
			Transform transform = gameObject.transform.Find("Catch");
			if ((bool)transform)
			{
				GameObject gameObject2 = transform.gameObject;
				gameObject2.SetActive(GameGlobal.gGameMusic.catchNotes.Contains(objId));
				gameObject2.transform.Find("CatchCircle2").gameObject.SetActive(GameGlobal.gGameMusic.catchHeadNotes.Contains(objId));
			}
			gameObject.SetActive(false);
			gameObject.transform.position = preObj.transform.position;
			gameObject.name += md.objId;
			if (preloads.Length <= objId)
			{
				Array.Resize(ref preloads, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
			}
			preloads[objId] = gameObject;
			NodeInitController orAddComponent = gameObject.GetOrAddComponent<NodeInitController>();
			if ((bool)orAddComponent)
			{
				orAddComponent.InitWithoutRun(objId);
				if (nodeInitCtrls.Length <= objId)
				{
					Array.Resize(ref nodeInitCtrls, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
				}
				nodeInitCtrls[objId] = orAddComponent;
			}
			SpineActionController orAddComponent2 = gameObject.GetOrAddComponent<SpineActionController>();
			if ((bool)orAddComponent2)
			{
				orAddComponent2.Init(objId);
				if (spineActionCtrls.Length <= objId)
				{
					Array.Resize(ref spineActionCtrls, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
				}
				spineActionCtrls[objId] = orAddComponent2;
			}
			BaseSpineObjectController objController = orAddComponent2.objController;
			if ((bool)objController)
			{
				if (!orAddComponent2)
				{
					objController.Init();
				}
				if (objCtrls.Length <= objId)
				{
					Array.Resize(ref objCtrls, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
				}
				objCtrls[objId] = objController;
			}
			Animator component = gameObject.GetComponent<Animator>();
			if ((bool)component)
			{
				if (animators.Length <= objId)
				{
					Array.Resize(ref animators, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
				}
				animators[objId] = component;
			}
			SkeletonAnimation component2 = gameObject.GetComponent<SkeletonAnimation>();
			if ((bool)component2)
			{
				if (animations.Length <= objId)
				{
					Array.Resize(ref animations, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
				}
				animations[objId] = component2;
			}
			SpineMountController component3 = gameObject.GetComponent<SpineMountController>();
			if ((bool)component3)
			{
				if (spineMountCtrls.Length <= objId)
				{
					Array.Resize(ref spineMountCtrls, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
				}
				spineMountCtrls[objId] = component3;
			}
			if (curScene >= 0 && !md.isLongPressType && !md.isMul)
			{
				NoteConfigData noteConfigData = NodeConfigReader.Instance.GetData().Find((NoteConfigData n) => n.prefab_name == preObj.name);
				if (showEffects.Length <= objId)
				{
					Array.Resize(ref showEffects, objId + Singleton<StageBattleComponent>.instance.resizeAdd);
				}
				if (showEffects[objId] == null)
				{
					showEffects[objId] = new List<bool>
					{
						false,
						false,
						false,
						false,
						false,
						false,
						false
					};
				}
				showEffects[objId][curScene] = noteConfigData.isShowPlayEffect;
			}
			return PreloadTmpGameObject(gameObject, md, curScene);
		}

		private void PreLoad()
		{
			Debug.Log("Preload enemy.");
			firstIn = true;
			preloads = new GameObject[0];
			nodeInitCtrls = new NodeInitController[0];
			spineActionCtrls = new SpineActionController[0];
			objCtrls = new BaseSpineObjectController[0];
			animators = new Animator[0];
			animations = new SkeletonAnimation[0];
			spineMountCtrls = new SpineMountController[0];
			preloads1 = new List<GameObject>[0];
			nodeInitCtrls1 = new List<NodeInitController>[0];
			spineActionCtrls1 = new List<SpineActionController>[0];
			objCtrls1 = new List<BaseSpineObjectController>[0];
			animators1 = new List<Animator>[0];
			animations1 = new List<SkeletonAnimation>[0];
			spineMountCtrls1 = new List<SpineMountController>[0];
			showEffects = new List<bool>[0];
			Boss.Instance.SetBoss();
			List<MusicData> musicData = Singleton<StageBattleComponent>.instance.GetMusicData();
			string arg = Singleton<StageBattleComponent>.instance.GetSceneName().Last().ToString();
			GameObject gameObject = Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>($"0{arg}1001_road_nor_1");
			Transform transform = gameObject.transform.Find("Catch");
			groundCatchCircleColor = transform.transform.Find("CatchCircle").GetComponent<SpriteRenderer>().color;
			groundCatchCircle2Color = transform.transform.Find("CatchCircle2").GetComponent<SpriteRenderer>().color;
			catchMaterial = transform.transform.Find("CatchCircle").GetComponent<SpriteRenderer>().sharedMaterial;
			catchGroundSprite = transform.transform.Find("CatchCircle").GetComponent<SpriteRenderer>().sprite;
			GameObject gameObject2 = Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>($"0{arg}1004_air_nor_1");
			transform = gameObject2.transform.Find("Catch");
			airCatchCircleColor = transform.transform.Find("CatchCircle").GetComponent<SpriteRenderer>().color;
			airCatchCircle2Color = transform.transform.Find("CatchCircle2").GetComponent<SpriteRenderer>().color;
			catchAirSprite = transform.transform.Find("CatchCircle").GetComponent<SpriteRenderer>().sprite;
			for (int i = 0; i < musicData.Count; i++)
			{
				PreLoad(i);
			}
			Singleton<BattleEnemyManager>.instance.PreloadCreateBattleEnemy();
		}

		private void InitSceneObject(string name, bool isActive = false)
		{
			scene = Singleton<StageBattleComponent>.instance.AddObj(name);
			if (scene == null)
			{
				Debug.Log("Scene " + name + " has no prefabs.");
				return;
			}
			scene.name = scene.name.Replace("(Clone)", string.Empty);
			scene.transform.SetParent(SingletonMonoBehaviour<SceneObjectController>.instance.transform);
			if (Singleton<StageBattleComponent>.instance.isSceneChangeType)
			{
				scene.SetActive(isActive);
				scenes.Add(scene);
			}
		}

		private void InitTimer(decimal total)
		{
			if (stepTimer == null)
			{
				Debug.Log("Load Scene with null timer, before LoadMusicDataByFileName, call method SetTimer.");
				return;
			}
			stepTimer.ClearTickEvent();
			stepTimer.Init(total);
			List<MusicData> musicData = Singleton<StageBattleComponent>.instance.GetMusicData();
			if (musicData == null || musicData.Count <= 0)
			{
				Debug.Log("Load Scene with null music data.");
				return;
			}
			for (int i = 0; i < musicData.Count; i++)
			{
				MusicData musicData2 = musicData[i];
				stepTimer.AddTickEvent(musicData2.tick - musicData2.dt, 3u);
			}
			List<SceneEvent> sceneEvents = MusicConfigReader.Instance.sceneEvents;
			List<decimal> list = new List<decimal>();
			foreach (SceneEvent item in sceneEvents)
			{
				if (!list.Contains(item.time))
				{
					list.Add(item.time);
				}
			}
			for (int j = 0; j < list.Count; j++)
			{
				stepTimer.AddTickEvent(list[j], 5u);
			}
		}

		private void InitEventTrigger()
		{
			GTrigger.UnRegEvent(3u);
			EventTrigger eventTrigger = GTrigger.RegEvent(3u);
			eventTrigger.Trigger += AddObjTrigger;
			GTrigger.UnRegEvent(5u);
			EventTrigger eventTrigger2 = GTrigger.RegEvent(5u);
			eventTrigger2.Trigger += SceneEventTrigger;
		}
	}
}
