using Assets.Scripts.Common.XDSDK;
using Assets.Scripts.Graphics;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.GeneralLocalization;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Platforms.Steam;
using Assets.Scripts.PeroTools.UI;
using Discord;
using FormulaBase;
using PeroTools.Commons;
using Rewired;
using SA.Common.Pattern;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.GameCore.Managers
{
	public class MainManager : SingletonMonoBehaviour<MainManager>
	{
		[HideInInspector]
		public DateTime dateTime;

		public Discord.Discord discord;

		public bool isFirstOpen
		{
			get;
			private set;
		}

		public event Action<DateTime> onTimeChanged;

		private void Init()
		{
			if (base.gameObject.GetComponent<AudioListener>() == null)
			{
				base.gameObject.AddComponent<AudioListener>();
			}
			base.transform.SetParent(SingletonMonoBehaviour<UnityGameManager>.instance.persistenceRoot);
			CloudSaveUpdate();
			DataManager instance = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance;
			Assets.Scripts.PeroTools.Commons.Singleton<DataUpgrader>.instance.LocalUpgrade();
			isFirstOpen = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["IsNew"].GetResult<bool>();
			Assets.Scripts.PeroTools.Commons.Singleton<UIManager>.instance.Init();
			PanelManage panelManage = new PanelManage();
			Time.fixedDeltaTime = 0.01f;
			Application.targetFrameRate = -1;
			InitRegion();
			InitLanguage();
			InitPlatforms();
			InitDevices();
			InitChannel();
			InitXDSDK();
			InitGraphic();
			InitTimer();
			InitStageInfo();
			InitController();
			InitReview();
			InitWeekFree();
			LoadingInit();
			Assets.Scripts.PeroTools.Commons.Singleton<Assets.Scripts.PeroTools.Managers.SceneManager>.instance.onSceneChanged += OnSceneChange;
			SteamManager instance2 = SingletonMonoBehaviour<SteamManager>.instance;
			instance2.onOverlayActivated = (SteamManager.OverlayActivated_delegate)Delegate.Combine(instance2.onOverlayActivated, new SteamManager.OverlayActivated_delegate(Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.OnFocusChangePauseGame));
			SA.Common.Pattern.Singleton<DiscordManager>.Instance.InitDiscord();
		}

		private void InitWeekFree()
		{
			WeekFreeManager instance = Assets.Scripts.PeroTools.Commons.Singleton<WeekFreeManager>.instance;
		}

		private void InitReview()
		{
			SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption("Review", "Default");
		}

		private void InitXDSDK()
		{
			XDSDKManager instance = Assets.Scripts.PeroTools.Commons.Singleton<XDSDKManager>.instance;
		}

		private void InitStageInfo()
		{
			NoteDataMananger instance = SingletonScriptableObject<NoteDataMananger>.instance;
		}

		public void InitController(bool firstTime = true)
		{
			if (firstTime)
			{
				UnityEngine.Object.Instantiate(Resources.Load("InputManager"));
				ReInput.ControllerDisconnectedEvent += delegate
				{
					InitController(false);
					Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.OnFocusChangePauseGame();
				};
				ReInput.ControllerConnectedEvent += delegate
				{
					InitController(false);
					Assets.Scripts.PeroTools.Commons.Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance.OnHandleConnet(Assets.Scripts.PeroTools.Commons.Singleton<Assets.Scripts.PeroTools.Managers.SceneManager>.instance.curScene.name, Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isSucceed, Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isDead, Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isPause, Assets.Scripts.PeroTools.Commons.Singleton<StageBattleComponent>.instance.isTutorial);
				};
			}
			Assets.Scripts.PeroTools.Managers.InputManager instance = Assets.Scripts.PeroTools.Commons.Singleton<Assets.Scripts.PeroTools.Managers.InputManager>.instance;
			string config = string.Empty;
			IControlable controller = ControllerFactory.GetController(out config, Application.platform);
			instance.SetController(controller, config);
			if (ControllerUtils.IsPS4Controller())
			{
				SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption("Controller", "PS4");
				instance.currentControllerName = "PS4";
				instance.RewiredJoystickControllerMapSetEnable(true, "UI");
			}
			else if (ControllerUtils.IsNSController())
			{
				SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption("Controller", "Ns");
				instance.currentControllerName = "Ns";
				instance.RewiredJoystickControllerMapSetEnable(true, "UI");
			}
			else if (ControllerUtils.IsCustomController())
			{
				SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption("Controller", "Custom Controller");
				instance.currentControllerName = "XBox";
				instance.RewiredJoystickControllerMapSetEnable(true, "UI");
			}
			else if (ControllerUtils.IsXBoxController())
			{
				SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption("Controller", "XBox");
				instance.currentControllerName = "XBox";
				instance.RewiredJoystickControllerMapSetEnable(true, "UI");
			}
			else if (ControllerUtils.IsKeyboardController())
			{
				SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption("Controller", "Keyboard");
				instance.currentControllerName = "Keyboard";
			}
		}

		public void LoadingInit()
		{
			StartCoroutine(WaitItemLoading());
		}

		public IEnumerator WaitItemLoading()
		{
			yield return new WaitForSeconds(0.1f);
			InitLoading();
		}

		public void InitLoading(string key = "LoadingNull")
		{
			if (Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"][key].GetResult<bool>())
			{
				return;
			}
			Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"][key].SetResult(true);
			int i;
			for (i = 0; i < Assets.Scripts.PeroTools.Commons.Singleton<ItemManager>.instance.items.Count; i++)
			{
				IData data = Assets.Scripts.PeroTools.Commons.Singleton<ItemManager>.instance.items.Find((IData t) => t["type"].GetResult<string>() == "loading" && t["isUnlock"].GetResult<bool>() && t["index"].GetResult<int>() == i);
				if (data != null)
				{
					Assets.Scripts.PeroTools.Commons.Singleton<ItemManager>.instance.AddItemToUsedList("loading", i);
				}
			}
			Assets.Scripts.PeroTools.Commons.Singleton<ItemManager>.instance.CheckAndAddWelcome(0, 5, false);
			Assets.Scripts.PeroTools.Commons.Singleton<ItemManager>.instance.CheckAndAddWelcome(1, 5, false);
			Assets.Scripts.PeroTools.Commons.Singleton<ItemManager>.instance.CheckAndAddWelcome(2, 5, false);
			for (int j = 0; j < 6; j++)
			{
				if ((Assets.Scripts.PeroTools.Commons.Singleton<ItemManager>.instance.IsChirstmas() || j != 3) && (Assets.Scripts.PeroTools.Commons.Singleton<ItemManager>.instance.TimeIsMay() || j != 4) && (Assets.Scripts.PeroTools.Commons.Singleton<ItemManager>.instance.TimeIsNanahira() || j != 5))
				{
					Assets.Scripts.PeroTools.Commons.Singleton<ItemManager>.instance.AddItemToUsedList("welcome", j);
				}
			}
			Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance.Save();
		}

		private void InitChannel()
		{
		}

		private void InitPlatforms()
		{
		}

		private void InitDevices()
		{
		}

		private void InitLanguage()
		{
			Scheme scheme = SingletonScriptableObject<LocalizationSettings>.instance.GetScheme("Language");
			if (scheme == null)
			{
				Debug.LogWarning("There is no scheme 'Language' defined at the settings");
				return;
			}
			string empty = string.Empty;
			if (isFirstOpen)
			{
				switch (Application.systemLanguage)
				{
				case SystemLanguage.Chinese:
				case SystemLanguage.ChineseSimplified:
					empty = "ChineseS";
					break;
				case SystemLanguage.ChineseTraditional:
					empty = "ChineseT";
					break;
				case SystemLanguage.English:
					empty = "English";
					break;
				case SystemLanguage.Japanese:
					empty = "Japanese";
					break;
				case SystemLanguage.Korean:
					empty = "Korean";
					break;
				default:
					empty = ((scheme.OptionCount() <= 0) ? null : scheme.GetOptionName(0));
					break;
				}
			}
			else
			{
				empty = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Language"].GetResult<string>();
				if (string.IsNullOrEmpty(empty))
				{
					empty = ((scheme.OptionCount() <= 0) ? null : scheme.GetOptionName(0));
				}
			}
			if (string.IsNullOrEmpty(empty))
			{
				Debug.LogError("[Localization] 语言初始化失败。");
				return;
			}
			SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption(scheme, empty);
			if (isFirstOpen)
			{
				Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["Language"].SetResult(empty);
				Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance.Save();
			}
			Debug.LogFormat("[Localization] 初始化语言 : {0}", empty);
		}

		private void InitRegion()
		{
			Scheme scheme = SingletonScriptableObject<LocalizationSettings>.instance.GetScheme("Region");
			if (scheme == null)
			{
				Debug.LogWarning("There is no scheme 'Region' defined at the settings");
			}
		}

		private void InitGraphic()
		{
			GraphicSettings.Init();
			if (isFirstOpen)
			{
				Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["GameConfig"]["QualityLevel"].SetResult(Mathf.Clamp(GraphicSettings.GetRecommandQualityCode(), 1, 2));
				Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["GameConfig"]["EffectLevel"].SetResult(GraphicSettings.GetRecommandEffectCode());
				Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance.Save();
			}
			GraphicSettings.SetFps((Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["GameConfig"]["Fps"].GetResult<int>() != 0) ? Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["GameConfig"]["Fps"].GetResult<int>() : 60, Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["GameConfig"]["Fps"].GetResult<int>() == 0);
		}

		private void InitTimer()
		{
			float time = Time.realtimeSinceStartup;
			Assets.Scripts.PeroTools.Commons.Singleton<ServerManager>.instance.GetTime(delegate(DateTime dt)
			{
				dateTime = dt;
			});
			int tickSecond = 0;
			SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("RefreshTime", delegate(float tick)
			{
				int num = (int)tick;
				if (num != tickSecond)
				{
					tickSecond = num;
					dateTime = dateTime.AddSeconds(Time.realtimeSinceStartup - time);
					time = Time.realtimeSinceStartup;
					if (this.onTimeChanged != null)
					{
						this.onTimeChanged(dateTime);
					}
				}
			}, UnityGameManager.LoopType.Update);
		}

		public void ScreenAdapte()
		{
			if (!Assets.Scripts.PeroTools.Commons.Singleton<UIManager>.instance.rectTransform)
			{
				return;
			}
			CanvasScaler component = Assets.Scripts.PeroTools.Commons.Singleton<UIManager>.instance.rectTransform.GetComponent<CanvasScaler>();
			Vector2 referenceResolution = component.referenceResolution;
			float num = referenceResolution.x / component.referencePixelsPerUnit;
			Vector2 referenceResolution2 = component.referenceResolution;
			float num2 = referenceResolution2.y / component.referencePixelsPerUnit;
			float num3 = 1f * (float)GraphicSettings.curScreenWidth / (float)GraphicSettings.curScreenHeight;
			if (num3 < num / num2)
			{
				Camera camera = Camera.main ?? UnityEngine.Object.FindObjectOfType<Camera>();
				float num4 = camera.orthographicSize * 2f * num3;
				if (num4 < num)
				{
					float num5 = num / num3;
					camera.orthographicSize = num5 * camera.orthographicSize / num2;
				}
			}
			float num6 = 1f * (float)GraphicSettings.curScreenWidth / (float)GraphicSettings.curScreenHeight;
			float num7 = 1f * (float)Screen.width / (float)Screen.height;
			Dictionary<string, float> dictionary = new Dictionary<string, float>();
			dictionary.Add("4:3", 1.77777779f);
			dictionary.Add("Others", -1f);
			Dictionary<string, float> dictionary2 = dictionary;
			foreach (KeyValuePair<string, float> item in dictionary2)
			{
				if (num7 == 1.5f || (double)num7 + 0.01 == 1.5)
				{
					SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption("Resolution", item.Key);
					break;
				}
				if ((double)num6 + 0.01 <= (double)item.Value)
				{
					SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption("Resolution", item.Key);
					break;
				}
				if (item.Value <= 0f)
				{
					SingletonScriptableObject<LocalizationSettings>.instance.ActiveOption("Resolution", item.Key);
				}
			}
		}

		private void OnSceneChange(Scene arg0, Scene scene)
		{
			ScreenAdapte();
			AnimatorsInit();
		}

		private void CloudSaveUpdate()
		{
		}

		private void AnimatorsInit()
		{
			List<SkeletonAnimator> array = from a in GameUtils.FindObjectsOfType<SkeletonAnimator>()
				where !a.gameObject.activeInHierarchy
				select a;
			List<GameObject> gameObjects = new List<GameObject>();
			gameObjects.AddRange(from a in array
				select a.gameObject into g
				where !g.activeSelf
				select g);
			array.For(delegate(SkeletonAnimator a)
			{
				Transform[] componentsInParent = a.GetComponentsInParent<Transform>(true);
				Transform[] array2 = componentsInParent;
				foreach (Transform transform in array2)
				{
					if (!transform.gameObject.activeSelf && !gameObjects.Contains(transform.gameObject))
					{
						gameObjects.Add(transform.gameObject);
					}
				}
			});
			gameObjects.For(delegate(GameObject g)
			{
				g.SetActive(true);
				g.SetVisible(false);
			});
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				gameObjects.For(delegate(GameObject g)
				{
					if ((bool)g)
					{
						SkeletonAnimator component = g.GetComponent<SkeletonAnimator>();
						if ((bool)component)
						{
							component.Initialize(false);
							Animator component2 = g.GetComponent<Animator>();
							component2.SetTime(0f);
						}
						g.SetVisible(true);
						g.SetActive(false);
					}
				});
			}, 2);
		}

		private void OnApplicationPause(bool pauseStatus)
		{
			if (dateTime == DateTime.MaxValue)
			{
				Assets.Scripts.PeroTools.Commons.Singleton<ServerManager>.instance.GetTime(delegate(DateTime dt)
				{
					dateTime = dt;
				});
			}
			if (pauseStatus)
			{
				GcControl.Enable();
			}
			else
			{
				GcControl.Disable();
			}
		}

		private void OnApplicationQuit()
		{
			GcControl.Enable();
		}
	}
}
