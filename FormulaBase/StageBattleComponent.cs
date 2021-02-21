using Assets.Scripts.Common;
using Assets.Scripts.Common.XDSDK;
using Assets.Scripts.GameCore.GameObjectLogics.GameObjectManager;
using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.UI.Panels;
using DG.Tweening;
using DYUnityLib;
using GameLogic;
using Newtonsoft.Json.Linq;
using PeroTools.Commons;
using RuntimeAudioClipLoader;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace FormulaBase
{
	public class StageBattleComponent : Singleton<StageBattleComponent>
	{
		private Hashtable m_PrefabCatchObj;

		private List<MusicData> m_MusicTickData;

		private Dictionary<int, List<TimeNodeOrder>> m_TimeNodeOrders;

		public int curPunchLpsIdx = -1;

		public int curJumpLpsIdx = -1;

		public int musicStartTime;

		public int lastTnoIndex = -1;

		public decimal lastTnoTime;

		public bool isInGame;

		public bool isDead;

		public string evaluate;

		public int evaluateNum;

		public float leastHpRate = 1f;

		public float failTime;

		public bool unlockNextLevel;

		public bool isSucceed;

		public int rank;

		public int resizeAdd = 500;

		private List<ParticleSystem> m_ParticleSystems;

		private List<string> m_PreloadAudios;

		public bool isSceneChangeType;

		public readonly Dictionary<string, int> sceneInfo = new Dictionary<string, int>
		{
			{
				"1O",
				1
			},
			{
				"1P",
				2
			},
			{
				"1Q",
				3
			},
			{
				"1R",
				4
			},
			{
				"1S",
				5
			},
			{
				"1T",
				6
			},
			{
				"1U",
				7
			}
		};

		public List<string> curSceneChangeType;

		private int m_Combo;

		public bool isNew;

		private Coroutine m_DelayStartCoroutine;

		private readonly TimeNodeOrder[] m_TmpTnos = new TimeNodeOrder[100];

		public bool isPause
		{
			get;
			private set;
		}

		public bool isTutorial
		{
			get;
			private set;
		}

		public float timeFromMusicStart => (float)(realTimeTick - musicStartTime) / 1000f;

		public decimal timeFromMusicStartDecimal => (decimal)(realTimeTick - musicStartTime) / 1000m;

		public int realTimeTick => Mathf.RoundToInt(GameGlobal.stopwatch.ElapsedMilliseconds);

		public TimeNodeOrder curTimeNodeOrder
		{
			get
			{
				float tickTime = GameGlobal.gTouch.tickTime;
				List<TimeNodeOrder> timeNodeByTick = GetTimeNodeByTick(tickTime);
				if (timeNodeByTick == null || timeNodeByTick.Count == 0)
				{
					return null;
				}
				return timeNodeByTick.Find((TimeNodeOrder t) => !t.isAir);
			}
		}

		public TimeNodeOrder curAirTimeNodeOrder
		{
			get
			{
				float tickTime = GameGlobal.gTouch.tickTime;
				List<TimeNodeOrder> timeNodeByTick = GetTimeNodeByTick(tickTime);
				if (timeNodeByTick == null || timeNodeByTick.Count == 0)
				{
					return null;
				}
				return timeNodeByTick.Find((TimeNodeOrder t) => t.isAir);
			}
		}

		public float offset
		{
			get;
			private set;
		}

		public StageBattleComponent()
		{
			string @string = Singleton<ConfigManager>.instance.GetString("IsAutoPlay");
			bool.TryParse(@string, out Singleton<BattleProperty>.instance.isAutoPlay);
		}

		public static void ReleaseReferences()
		{
			Singleton<StageBattleComponent>.instance.m_PrefabCatchObj = null;
			Singleton<StageBattleComponent>.instance.m_MusicTickData = null;
			Singleton<StageBattleComponent>.instance.m_TimeNodeOrders = null;
			Singleton<StageBattleComponent>.instance.m_ParticleSystems = null;
		}

		public void InitByName(string name)
		{
			int musicIndexByName = GetMusicIndexByName(name);
			int albumIndexByName = GetAlbumIndexByName(name);
			InitById(musicIndexByName);
			Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].SetResult(musicIndexByName);
			Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"].SetResult(Singleton<ConfigManager>.instance.GetConfigStringValue("albums", albumIndexByName, "uid"));
		}

		public void InitById(int idx)
		{
			Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].SetResult(idx);
			isTutorial = (idx == -1 && !bool.Parse(SingletonScriptableObject<ConstanceManager>.instance["IsEditorMode"]));
		}

		public void InitByUid(string uid)
		{
			int num = int.Parse(uid.LastAfter('-').BeginBefore('_'));
			int index = int.Parse(uid.BeginBefore('-'));
			InitById(num);
			Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].SetResult(num);
			Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"].SetResult(Singleton<ConfigManager>.instance.GetConfigStringValue("albums", index, "uid"));
		}

		public void FindAllParticles()
		{
			m_ParticleSystems = GameUtils.FindObjectsOfType<ParticleSystem>();
		}

		public int GetMusicIndexByName(string name)
		{
			int index = 0;
			IndexByName(name, delegate(int i, int j)
			{
				index = j;
			});
			return index;
		}

		public int GetAlbumIndexByName(string name)
		{
			int index = 0;
			IndexByName(name, delegate(int i, int j)
			{
				index = i;
			});
			return index;
		}

		public void IndexByName(string name, Action<int, int> callback)
		{
			for (int i = 1; i < int.MaxValue; i++)
			{
				string jsonName = $"ALBUM{i}";
				JArray jArray = Singleton<ConfigManager>.instance[jsonName];
				if (jArray == null)
				{
					break;
				}
				for (int j = 0; j < jArray.Count; j++)
				{
					JToken jToken = jArray[j];
					if ((string)jToken["name"] == name)
					{
						callback(i - 1, j);
					}
				}
			}
		}

		public int GetMusicIndex()
		{
			return Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].GetResult<int>();
		}

		public void SetDifficulty(int value)
		{
			Singleton<DataManager>.instance["Account"]["SelectedDifficulty"].SetResult(value);
		}

		public string GetDiffcultStr()
		{
			switch (GetDiffcult())
			{
			case 1u:
				return "easy";
			case 2u:
				return "hard";
			case 3u:
				return "master";
			default:
				return string.Empty;
			}
		}

		public string GetAlbumName()
		{
			return Singleton<DataManager>.instance["Account"]["SelectedAlbumName"].GetResult<string>();
		}

		public uint GetDiffcult()
		{
			return (uint)((GetMusicIndex() == -1) ? 1 : Singleton<DataManager>.instance["Account"]["SelectedDifficulty"].GetResult<int>());
		}

		public bool IsAllCombo()
		{
			return Singleton<TaskStageTarget>.instance.GetComboMax() >= Singleton<TaskStageTarget>.instance.GetTotalNum();
		}

		public bool IsFool()
		{
			return DateTime.Now.Month == 4 && DateTime.Now.Day == 1 && !Singleton<DataManager>.instance["Account"]["IsNew"].GetResult<bool>();
		}

		public string GetNoteJsonName()
		{
			string result = Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>();
			if (string.IsNullOrEmpty(result))
			{
				if (IsFool())
				{
					return "tutorial_map2";
				}
				return "tutorial_v2_map1";
			}
			if (DateTime.Now.Month == 4 && DateTime.Now.Day == 1 && result == "4-5" && GetDiffcult() == 2)
			{
				return "goodtek_map4";
			}
			if (DateTime.Now.Month == 11 && DateTime.Now.Day == 1 && result == "8-3")
			{
				return "sweet_witch_girl_map4";
			}
			if (result == "22-1" && GetDiffcult() >= 3 && SpecialSongManager.isOnFREEDOMDIVE)
			{
				return "freedom_dive_map4";
			}
			if (result == "0-45" && GetDiffcult() >= 3 && SpecialSongManager.isOnMopemope)
			{
				return "mopemope_map4";
			}
			if (result == "20-2" && GetDiffcult() >= 3 && SpecialSongManager.isOnINFiNiTEENERZY)
			{
				return "infinite_enerzy_overdoze_map4";
			}
			if (result == "28-1" && GetDiffcult() >= 3 && SpecialSongManager.isOnGinevra)
			{
				return "ginevra_map4";
			}
			if (result == "8-4" && GetDiffcult() >= 3 && SpecialSongManager.isOnTrippersFeeling)
			{
				return "trippers_feeling_map4";
			}
			if (result == "0-11" && GetDiffcult() >= 3 && SpecialSongManager.isOnLightsOfMuse)
			{
				return "lights_of_muse_map4";
			}
			if (result == "5-3" && GetDiffcult() >= 3 && SpecialSongManager.isOnXING)
			{
				return "xing_map4";
			}
			if (result == "6-4" && GetDiffcult() >= 3 && SpecialSongManager.isOnStargazer)
			{
				return "stargazer_map4";
			}
			if (result == "29-1" && GetDiffcult() >= 3 && SpecialSongManager.isOnFujinRumble)
			{
				return "fujin_rumble_map4";
			}
			if (result == "29-3" && GetDiffcult() >= 3 && SpecialSongManager.isOnHGMakaizou)
			{
				return "hg_makaizou_polyvinyl_shounen_map4";
			}
			if (result == "29-5" && GetDiffcult() >= 3 && SpecialSongManager.isOnOuroboros)
			{
				return "ouroboros_twin_stroke_of_the_end_map4";
			}
			if (result == "31-5" && GetDiffcult() >= 3 && SpecialSongManager.isOnSquareLake)
			{
				return "square_lake_map4";
			}
			if (result == "33-2" && GetDiffcult() >= 3 && SpecialSongManager.isOnHappinessBreeze)
			{
				return "happiness_breeze_map4";
			}
			if (result == "33-3" && GetDiffcult() >= 3 && SpecialSongManager.isOnChromeVOX)
			{
				return "chrome_vox_map4";
			}
			if (result == "34-1" && GetDiffcult() >= 3 && SpecialSongManager.isOnBattleNo1)
			{
				return "battle_no1_map4";
			}
			if (result == "34-2" && GetDiffcult() >= 3 && SpecialSongManager.isOnCthugha)
			{
				return "cthugha_map4";
			}
			if (result == "34-3" && GetDiffcult() >= 3 && SpecialSongManager.isOnTwinkleMagic)
			{
				return "twinkle_magic_map4";
			}
			if (result == "34-4" && GetDiffcult() >= 3 && SpecialSongManager.isOnCometCoaster)
			{
				return "comet_coaster_map4";
			}
			if (result == "34-5" && GetDiffcult() >= 3 && SpecialSongManager.isOnXodus)
			{
				return "xodus_map4";
			}
			int configIndex = Singleton<ConfigManager>.instance.GetConfigIndex(GetAlbumName(), "uid", result);
			if ((bool)UITest.instance)
			{
				return (string)UITest.instance.bms.info["NAME"];
			}
			return string.Format("{0}{1}", Singleton<ConfigManager>.instance.GetJson(GetAlbumName(), false)[configIndex]["noteJson"], GetDiffcult());
		}

		public int GetHideBMSDifficulty()
		{
			if ((SpecialSongManager.isOnFREEDOMDIVE && Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "freedom_dive_map4") || Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "goodtek_map4" || Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "sweet_witch_girl_map4" || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "mopemope_map4" && SpecialSongManager.isOnMopemope) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "infinite_enerzy_overdoze_map4" && SpecialSongManager.isOnINFiNiTEENERZY) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "ginevra_map4" && SpecialSongManager.isOnGinevra) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "trippers_feeling_map4" && SpecialSongManager.isOnTrippersFeeling) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "lights_of_muse_map4" && SpecialSongManager.isOnLightsOfMuse) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "xing_map4" && SpecialSongManager.isOnXING) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "stargazer_map4" && SpecialSongManager.isOnStargazer) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "fujin_rumble_map4" && SpecialSongManager.isOnFujinRumble) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "hg_makaizou_polyvinyl_shounen_map4" && SpecialSongManager.isOnHGMakaizou) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "ouroboros_twin_stroke_of_the_end_map4" && SpecialSongManager.isOnOuroboros) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "square_lake_map4" && SpecialSongManager.isOnSquareLake) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "happiness_breeze_map4" && SpecialSongManager.isOnHappinessBreeze) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "chrome_vox_map4" && SpecialSongManager.isOnChromeVOX) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "battle_no1_map4" && SpecialSongManager.isOnBattleNo1) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "cthugha_map4" && SpecialSongManager.isOnCthugha) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "twinkle_magic_map4" && SpecialSongManager.isOnTwinkleMagic) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "comet_coaster_map4" && SpecialSongManager.isOnCometCoaster) || (Singleton<StageBattleComponent>.instance.GetNoteJsonName() == "xodus_map4" && SpecialSongManager.isOnXodus))
			{
				return 4;
			}
			return Singleton<DataManager>.instance["Account"]["SelectedDifficulty"].GetResult<int>();
		}

		public string GetMusic()
		{
			int musicIndex = GetMusicIndex();
			if (musicIndex == -1)
			{
				return "heart_pounding_flight_music";
			}
			if ((bool)UITest.instance)
			{
				return Path.GetFileNameWithoutExtension((string)UITest.instance.bms.info["WAV10"]);
			}
			return Singleton<ConfigManager>.instance.GetConfigStringValue(GetAlbumName(), "uid", "music", Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>());
		}

		public string GetSceneName()
		{
			int musicIndex = GetMusicIndex();
			if (musicIndex == -1)
			{
				return "scene_05";
			}
			if ((bool)UITest.instance)
			{
				return (string)UITest.instance.bms.info["GENRE"];
			}
			return Singleton<ConfigManager>.instance.GetConfigStringValue(GetAlbumName(), "uid", "scene", Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>());
		}

		public int GetCombo()
		{
			return m_Combo;
		}

		public List<MusicData> GetMusicData()
		{
			return m_MusicTickData;
		}

		public MusicData GetMusicDataByIdx(int idx)
		{
			List<MusicData> musicData = GetMusicData();
			if (musicData == null || idx < 0 || idx >= musicData.Count)
			{
				return default(MusicData);
			}
			return m_MusicTickData[idx];
		}

		public void SetMusicData(MusicData md)
		{
			m_MusicTickData[md.objId] = md;
		}

		public decimal GetEndTimePlus()
		{
			return 0.5m;
		}

		public bool IsAutoPlay()
		{
			return Singleton<BattleProperty>.instance.isAutoPlay;
		}

		public void SetMusicIndex(int id)
		{
			Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].SetResult(id);
		}

		public void SetCombo(int combo, bool addCount = true)
		{
			if (combo == 0 && addCount)
			{
				Singleton<TaskStageTarget>.instance.AddComboMiss(1);
			}
			if (m_Combo >= Singleton<BattleProperty>.instance.missComboMax || combo != 0)
			{
				m_Combo = combo;
				Singleton<TaskStageTarget>.instance.AddComboMax(combo);
				if (combo >= 10 && combo % 10 == 0)
				{
					Singleton<EventManager>.instance.Invoke("Battle/OnCombo10", combo);
				}
				Singleton<EventManager>.instance.Invoke("Battle/OnComboChanged", combo);
			}
		}

		public void SetAutoPlay(bool val)
		{
			if (isInGame)
			{
				Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false);
				Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, true);
				SingletonMonoBehaviour<GirlManager>.instance.SetJumpingAction(false);
				SingletonMonoBehaviour<GirlManager>.instance.SetTone(false);
			}
			Singleton<ConfigManager>.instance.SaveString("IsAutoPlay", val.ToString());
			Singleton<BattleProperty>.instance.isAutoPlay = val;
			Singleton<EventManager>.instance.Invoke("Battle/OnSetAutoPlay");
			Singleton<DataManager>.instance.Save();
			Debug.Log("Set auto play : " + val);
		}

		public float GetTimeProgress()
		{
			if (!Singleton<AudioManager>.instance.bgm)
			{
				return 0f;
			}
			if (Singleton<AudioManager>.instance.bgm.isPlaying && !Singleton<AudioManager>.instance.bgm.mute)
			{
				return timeFromMusicStart / Singleton<AudioManager>.instance.bgm.clip.length;
			}
			return 0f;
		}

		public void OnLoadComplete()
		{
			if (GameSceneMainController.isEditorMode)
			{
				Singleton<AudioManager>.instance.bgm.clip = null;
			}
			else
			{
				int musicIndex = GetMusicIndex();
				if (musicIndex != -1)
				{
					PlayMusic(Singleton<ConfigManager>.instance.GetConfigStringValue(GetAlbumName(), "uid", "music", Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>()));
				}
			}
			if (isNew)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					Singleton<DataManager>.instance["Account"]["IsNew"].SetResult(false);
					Singleton<DataManager>.instance.Save();
				}, 2f);
			}
			Screen.autorotateToLandscapeLeft = false;
			Screen.autorotateToLandscapeRight = false;
		}

		public void OnInterrupt()
		{
			if (!isPause && isInGame && !isDead)
			{
				Singleton<EventManager>.instance.Invoke("UI/OnShowPnlPause");
				Singleton<EventManager>.instance.Invoke("Game/Pause");
			}
		}

		public void PlayMusic(string music)
		{
			AudioClip audioClip = null;
			string result = Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>();
			if (GameSceneMainController.isEditorMode && MusicConfigReader.Instance.bms != null)
			{
				char c = '/';
				c = '\\';
				music = (string)MusicConfigReader.Instance.bms.info["WAV10"];
				string text = $"{Application.streamingAssetsPath}{c}music{c}{music}";
				if (!FileUtils.Exists(text))
				{
					text = $"{Application.streamingAssetsPath}{c}map{c}{music}";
				}
				if (FileUtils.Exists(text))
				{
					audioClip = Manager.Load(text, false, false, false);
					if ((bool)audioClip)
					{
						Singleton<AudioManager>.instance.PlayBGM(audioClip);
					}
				}
			}
			if (!audioClip)
			{
				if (!string.IsNullOrEmpty(result) && result.Contains("21-2") && SpecialSongManager.isOnNanoCoreAudio)
				{
					music += "2";
				}
				Singleton<AudioManager>.instance.PlayBGM(music);
			}
		}

		public void Enter(int id)
		{
			SetMusicIndex(id);
			Singleton<SceneManager>.instance.LoadSceneViaLoadingScene("GameMain", OnLoadComplete);
			Singleton<InputManager>.instance.RewiredJoystickControllerMapSetEnable(false, "UI");
			Singleton<InputManager>.instance.SwitchHandleProposal(Singleton<InputManager>.instance.handleProposal);
		}

		public void Pause(bool pauseCorountine = true)
		{
			if (!isInGame || isPause)
			{
				return;
			}
			Screen.sleepTimeout = -2;
			isPause = true;
			Singleton<InputManager>.instance.isPauseMap = true;
			if (pauseCorountine)
			{
				Singleton<AudioManager>.instance.bgm.mute = true;
				SingletonMonoBehaviour<CoroutineManager>.instance.isCoroutineActive = false;
				Singleton<AudioManager>.instance.PauseSfx();
				DOTweenUtils.Pause();
			}
			if ((bool)SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs)
			{
				SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs.mute = true;
			}
			PnlBattle.instance.Pause();
			Singleton<EffectManager>.instance.Pause();
			SpineActionController.starTweeners.For(delegate(Tweener t)
			{
				t.Pause();
			});
			if (FeverEffectManager.instance != null)
			{
				FeverEffectManager.instance.gameObject.Enable(false, false, typeof(Animator), typeof(Animation), typeof(SkeletonAnimation));
			}
			GameObject gameObject = Boss.Instance.GetGameObject();
			if ((bool)gameObject)
			{
				gameObject.Enable(false, false, typeof(Animation), typeof(SkeletonAnimation));
			}
			m_ParticleSystems.For(delegate(ParticleSystem p)
			{
				p.Pause();
			});
			GameUtils.FindObjectsOfType<ParticleSystem>(AttackEffectManager.instance.transform, false).For(delegate(ParticleSystem p)
			{
				p.Pause();
			});
			GameUtils.FindObjectsOfType<ParticleSystem>(SingletonMonoBehaviour<GirlManager>.instance.girlGhost.transform, false).For(delegate(ParticleSystem p)
			{
				p.Pause();
			});
			if (FeverEffectManager.instance != null)
			{
				GameUtils.FindObjectsOfType<ParticleSystem>(FeverEffectManager.instance.transform, false).For(delegate(ParticleSystem p)
				{
					p.Pause();
				});
			}
			AttackEffectManager.instance.gameObject.Enable(false, false, typeof(Animator), typeof(Animation), typeof(SkeletonAnimation));
			SingletonMonoBehaviour<GirlManager>.instance.gameObject.Enable(false, false, typeof(Animator), typeof(Animation), typeof(SkeletonAnimation));
			GirlActionController.instance.Pause();
			Boss.Instance.Pause();
			GameGlobal.gGameMusicScene.spineActionCtrls.For(delegate(SpineActionController s)
			{
				if ((bool)s)
				{
					s.Pause();
				}
			});
			if ((bool)SingletonMonoBehaviour<GirlManager>.instance.girlGhost)
			{
				SingletonMonoBehaviour<GirlManager>.instance.girlGhost.Enable(false, false, typeof(Animator), typeof(Animation), typeof(SkeletonAnimation));
			}
			GameMusicScene.instance.scene.Enable(false, true, typeof(Animator), typeof(Animation), typeof(SkeletonAnimation));
			SingletonMonoBehaviour<SceneObjectController>.instance.gameObject.Enable(false, false, typeof(Animator), typeof(Animation), typeof(SkeletonAnimation));
			Singleton<AudioManager>.instance.bgm.Pause();
			FixUpdateTimer.PauseTimer();
			GameGlobal.stopwatch.Stop();
			SingletonMonoBehaviour<GameSceneMainController>.instance.Delay(0.1f, GcControl.Collect);
			Singleton<InputManager>.instance.RewiredJoystickControllerMapSetEnable(true, "UI");
		}

		public void Resume()
		{
			if (!isPause)
			{
				return;
			}
			Screen.sleepTimeout = -1;
			isPause = false;
			Singleton<InputManager>.instance.isPauseMap = false;
			SingletonMonoBehaviour<CoroutineManager>.instance.isCoroutineActive = true;
			PnlBattle.instance.Resume();
			Singleton<EffectManager>.instance.Resume();
			Singleton<AudioManager>.instance.ResumeSfx();
			SpineActionController.starTweeners.For(delegate(Tweener t)
			{
				t.Play();
			});
			if (FeverEffectManager.instance != null)
			{
				FeverEffectManager.instance.gameObject.Enable(true, false, typeof(Animator), typeof(Animation), typeof(SkeletonAnimation));
			}
			GameObject gameObject = Boss.Instance.GetGameObject();
			if ((bool)gameObject)
			{
				gameObject.Enable(true, false, typeof(Animation), typeof(SkeletonAnimation));
			}
			m_ParticleSystems.For(delegate(ParticleSystem p)
			{
				p.Play();
			});
			GameUtils.FindObjectsOfType<ParticleSystem>(AttackEffectManager.instance.transform, false).For(delegate(ParticleSystem p)
			{
				p.Play();
			});
			GameUtils.FindObjectsOfType<ParticleSystem>(SingletonMonoBehaviour<GirlManager>.instance.girlGhost.transform, false).For(delegate(ParticleSystem p)
			{
				p.Play();
			});
			if (FeverEffectManager.instance != null)
			{
				GameUtils.FindObjectsOfType<ParticleSystem>(FeverEffectManager.instance.transform, false).For(delegate(ParticleSystem p)
				{
					p.Play();
				});
			}
			AttackEffectManager.instance.gameObject.Enable(true, false, typeof(Animator), typeof(Animation), typeof(SkeletonAnimation));
			SingletonMonoBehaviour<GirlManager>.instance.gameObject.Enable(true, false, typeof(Animator), typeof(Animation), typeof(SkeletonAnimation));
			GirlActionController.instance.Resume();
			Boss.Instance.Resume();
			GameGlobal.gGameMusicScene.spineActionCtrls.For(delegate(SpineActionController s)
			{
				if ((bool)s)
				{
					s.Resume();
				}
			});
			if ((bool)SingletonMonoBehaviour<GirlManager>.instance.girlGhost)
			{
				SingletonMonoBehaviour<GirlManager>.instance.girlGhost.Enable(true, false, typeof(Animator), typeof(Animation), typeof(SkeletonAnimation));
			}
			GameMusicScene.instance.scene.Enable(true, true, typeof(Animator), typeof(Animation), typeof(SkeletonAnimation));
			SingletonMonoBehaviour<SceneObjectController>.instance.gameObject.Enable(true, false, typeof(Animator), typeof(Animation), typeof(SkeletonAnimation));
			if ((bool)SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs)
			{
				SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs.mute = false;
			}
			FixUpdateTimer.ResumeTimer();
			GameGlobal.stopwatch.Start();
			MultHitEnemyController.Resume();
			if ((bool)Singleton<AudioManager>.instance.bgm.clip)
			{
				int num = Mathf.RoundToInt((timeFromMusicStart + MusicConfigReader.Instance.floatDelay - offset) / Singleton<AudioManager>.instance.bgm.clip.length * (float)Singleton<AudioManager>.instance.bgm.clip.samples);
				if (num >= 0 && num < Singleton<AudioManager>.instance.bgm.clip.samples)
				{
					Singleton<AudioManager>.instance.bgm.mute = false;
					Singleton<AudioManager>.instance.bgm.Play();
					SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
					{
						FixedOffset();
					}, 0.5f);
				}
			}
			Singleton<InputManager>.instance.RewiredJoystickControllerMapSetEnable(false, "UI");
		}

		public void FixedOffset(Action callback = null)
		{
			if (!(Singleton<AudioManager>.instance.bgm.clip == null))
			{
				int num = Mathf.RoundToInt((timeFromMusicStart + MusicConfigReader.Instance.floatDelay - offset) / Singleton<AudioManager>.instance.bgm.clip.length * (float)Singleton<AudioManager>.instance.bgm.clip.samples);
				if (num >= 0 && num < Singleton<AudioManager>.instance.bgm.clip.samples)
				{
					callback?.Invoke();
					Singleton<AudioManager>.instance.bgm.timeSamples = num;
				}
			}
		}

		public GameObject AddObj(string filename)
		{
			if (m_PrefabCatchObj == null)
			{
				Debug.Log("Stage prefabCatchObj not init.");
				return null;
			}
			GameObject gameObject = m_PrefabCatchObj[filename] as GameObject;
			if (gameObject == null)
			{
				gameObject = Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(filename);
				if (gameObject == null)
				{
					return null;
				}
				m_PrefabCatchObj[filename] = gameObject;
			}
			return UnityEngine.Object.Instantiate(gameObject);
		}

		public void AddObjAsync(string filename, Action<GameObject> callback)
		{
			if (m_PrefabCatchObj == null)
			{
				Debug.Log("Stage prefabCatchObj not init.");
				return;
			}
			GameObject x = m_PrefabCatchObj[filename] as GameObject;
			if (!(x == null))
			{
				return;
			}
			Singleton<AssetBundleManager>.instance.LoadFromNameAsyn(filename, delegate(GameObject obj)
			{
				if (!(obj == null))
				{
					m_PrefabCatchObj[filename] = obj;
					UnityEngine.Object.Instantiate(obj);
					callback(obj);
				}
			});
		}

		public void End()
		{
			if (!Singleton<XDSDKManager>.instance.isOvearSea)
			{
			}
			isSucceed = true;
			Screen.sleepTimeout = -2;
			bool isHalloweenMap = GetNoteJsonName() == "sweet_witch_girl_map4";
			bool flag = GetHideBMSDifficulty() == 4;
			Screen.autorotateToLandscapeLeft = true;
			Screen.autorotateToLandscapeRight = true;
			Singleton<InputManager>.instance.RewiredJoystickControllerMapSetEnable(true, "UI");
			Singleton<InputManager>.instance.RewiredJoystickControllerMapSetEnable(false, Singleton<InputManager>.instance.handleProposal);
			SingletonMonoBehaviour<CoroutineManager>.instance.isCoroutineActive = true;
			if (isDead || isTutorial)
			{
				return;
			}
			if (Singleton<BattleProperty>.instance.isNekoCharacter && Singleton<BattleProperty>.instance.isNekoSkillTrigger)
			{
				Singleton<EventManager>.instance.Invoke("Battle/OnVictory");
				return;
			}
			if (Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>() != 2 && !isHalloweenMap)
			{
				Singleton<StatisticsManager>.instance.OnBattleEnd();
			}
			Singleton<EventManager>.instance.Invoke("Battle/OnVictory");
			if (Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>() == 2)
			{
				return;
			}
			KeyValuePair<string, int> stageEvaluate = Singleton<TaskStageTarget>.instance.GetStageEvaluate();
			evaluate = stageEvaluate.Key;
			evaluateNum = stageEvaluate.Value;
			SingletonDataObject singletonDataObject = Singleton<DataManager>.instance["Exp"];
			IVariable data = singletonDataObject["Exp"];
			singletonDataObject["Judge"].SetResult(evaluate);
			singletonDataObject["Skill"].SetResult(Singleton<BattleProperty>.instance.expRate - 1f);
			SingletonMonoBehaviour<MessageManager>.instance.Send("exp", data.GetResult<int>());
			bool flag2 = Halloween(() => isHalloweenMap, 2, 3);
			if (Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>() == "30-0" || Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>() == "30-1" || Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>() == "30-2")
			{
				Singleton<ItemManager>.instance.AddExtraItem("loading", 36, 5);
			}
			if (Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>() == "0-50" || Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>() == "0-51")
			{
				Singleton<ItemManager>.instance.AddExtraItem("loading", 39, 5);
			}
			Halloween(() => Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>() == "0-41", 3, 2);
			NanoCoreLogic();
			if (!flag)
			{
				Singleton<StageAchievementManager>.instance.Do();
			}
			Singleton<TaskManager>.instance.Do(true);
			Singleton<AchievementManager>.instance.Do(true);
			Singleton<StatisticsManager>.instance.OnChallengeAchievementEnd();
			if (unlockNextLevel)
			{
				SingletonMonoBehaviour<MessageManager>.instance.Send("unlockLevel");
			}
			Singleton<DataManager>.instance.Save();
			if (evaluateNum >= 2 && Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>() == "0-32")
			{
				Singleton<DataManager>.instance["Account"]["IsShowMusynx"].SetResult(true);
			}
			if (Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>() == "0-36")
			{
				Singleton<ItemManager>.instance.AddExtraItem("loading", 20, 5);
			}
			if (Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>() == "0-44")
			{
				Singleton<ItemManager>.instance.AddExtraItem("loading", 31, 5);
			}
			if (SingletonMonoBehaviour<MessageManager>.instance.available)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					Singleton<EventManager>.instance.Invoke("Battle/OnReward");
				}, 1.7f);
			}
		}

		private bool Halloween(Func<bool> isRun, int getCount, int targetCount)
		{
			if (isRun())
			{
				int num = Singleton<ItemManager>.instance.items.Find((IData it) => it["type"].GetResult<string>() == "loading" && it["index"].GetResult<int>() == 26)?["count"].GetResult<int>() ?? 0;
				if (num == 0 || num == targetCount)
				{
					Singleton<ItemManager>.instance.AddExtraItem("loading", 26, getCount);
				}
				return true;
			}
			return false;
		}

		private void NanoCoreLogic()
		{
			if (!(GetAlbumName() != "ALBUM22") && (evaluate == "a" || evaluate == "s" || evaluate == "ss" || evaluate == "sss") && !Singleton<DataManager>.instance["Account"]["IsShowNanoCore"].GetResult<bool>())
			{
				Singleton<DataManager>.instance["Account"]["IsShowNanoCore"].SetResult(true);
			}
		}

		public void Dead()
		{
			isDead = true;
			failTime = Singleton<AudioManager>.instance.bgm.time;
			SetCombo(0, false);
			Singleton<TaskManager>.instance.Do(false);
			Singleton<AchievementManager>.instance.Do(false);
			if (!SingletonMonoBehaviour<GirlManager>.instance.IsJumpingAction())
			{
				SingletonMonoBehaviour<GirlManager>.instance.DestroyGirlSpineAnimation();
				Singleton<EventManager>.instance.Invoke("Battle/OnFail");
			}
			else
			{
				SkeletonAnimation skAnimation = GirlActionController.instance.spineActionCtrl.skAnimation;
				float num = skAnimation.state.GetCurrent(0).TrackTime;
				Animator animator = GirlActionController.instance.animator;
				bool flag = animator.GetCurrentAnimatorStateInfo(0).IsName("char_jump");
				bool flag2 = Singleton<BattleEnemyManager>.instance.isGroundPressing || Singleton<BattleEnemyManager>.instance.isAirPressing;
				string animationName = flag2 ? "AirPressHurt" : ((!flag) ? "AirHitHurt" : "JumpHurt");
				if (flag2)
				{
					num = 0f;
				}
				skAnimation.state.SetAnimation(0, animationName, false);
				skAnimation.state.GetCurrent(0).TrackTime = num;
				SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					SingletonMonoBehaviour<GirlManager>.instance.DestroyGirlSpineAnimation();
					SingletonMonoBehaviour<GirlManager>.instance.SetJumpingAction(false);
					SingletonMonoBehaviour<GirlManager>.instance.SetTone(false);
					Singleton<EventManager>.instance.Invoke("Battle/OnFail");
				}, num);
			}
			Singleton<StatisticsManager>.instance.OnBattleEnd(true);
			if (Singleton<DataManager>.instance["Account"]["SelectedMusicLevel"].GetResult<int>() >= 9)
			{
				Singleton<ItemManager>.instance.AddExtraItem("loading", 32, 5);
			}
			Singleton<DataManager>.instance["Account"]["IsNekoSkillAvailable"].SetResult(true);
			Singleton<DataManager>.instance.Save();
		}

		public void GameStart(object sender, uint triggerId, params object[] args)
		{
			int musicIndex = GetMusicIndex();
			InitById(musicIndex);
			FeverManager.Instance.InitGameKernel();
			Singleton<StatisticsManager>.instance.OnBattleStart();
			GameGlobal.stopwatch.Reset();
			Screen.sleepTimeout = -1;
			PnlBattle.instance.OnGameStart();
			offset = (float)Singleton<DataManager>.instance["GameConfig"]["Offset"].GetResult<int>() * 0.001f;
			leastHpRate = 1f;
			m_DelayStartCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				Singleton<AudioManager>.instance.bgm.Play();
				Singleton<AudioManager>.instance.bgm.timeSamples = 0;
				Singleton<AudioManager>.instance.bgm.mute = false;
				SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					FixedOffset();
					GameAudioVisualization.isPlay = true;
				}, 0.5f);
			}, -MusicConfigReader.Instance.delay + (decimal)offset);
			GameGlobal.gGameMusic.Run();
			GameGlobal.gGameMusicScene.Run();
			GameGlobal.stopwatch.Start();
			musicStartTime = 0;
			isInGame = true;
			GcControl.Disable();
		}

		public void ResetAll()
		{
			m_Combo = 0;
			MultHitEnemyController.hitCount = 0;
			MultHitEnemyController.isBanEmptyAction = false;
			MultHitEnemyController.isMulHitEnding = false;
			isDead = false;
			isSucceed = false;
			rank = -1;
			unlockNextLevel = false;
			lastTnoTime = 0m;
			lastTnoIndex = 0;
			curJumpLpsIdx = -1;
			curPunchLpsIdx = -1;
			FeverManager.Instance.ResetFever();
			FixUpdateTimer.ResumeTimer();
			Singleton<BattleEnemyManager>.instance.Init();
			BattleRoleAttributeComponent.instance.Revive(true);
			InitData();
		}

		public void ReEnter()
		{
			Exit("GameMain", delegate
			{
				OnLoadComplete();
				InitById(GetMusicIndex());
				isInGame = false;
			}, false);
		}

		public void EditorReEnter()
		{
			int idx = UITest.instance.index;
			int diff = UITest.instance.difficulty;
			Exit("GameMain", delegate
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
				{
					OnLoadComplete();
					UITest.instance.index = idx;
					UITest.instance.difficulty = diff;
					InitByName((string)UITest.instance.bms.info["TITLE"]);
					UITest.instance.GameStart();
				}, () => UITest.instance);
			}, false);
		}

		public void Exit(string sceneName = "UISystem", Action calllback = null, bool withBack = true)
		{
			Singleton<InputManager>.instance.SwitchButtonProposal(Singleton<InputManager>.instance.keyBoardProposal);
			if (sceneName != "GameMain")
			{
				sceneName = "UISystem_PC";
			}
			if (m_DelayStartCoroutine != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_DelayStartCoroutine);
			}
			if ((sceneName == "UISystem" || sceneName == "UISystem_PC") && !isTutorial)
			{
				Singleton<TaskManager>.instance.Do(Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>() != 2 && isSucceed, true);
			}
			Resume();
			DOTweenUtils.Kill();
			isInGame = false;
			Singleton<AudioManager>.instance.bgm.mute = false;
			Singleton<AudioManager>.instance.bgm.loop = true;
			Debug.Log("Stage Exit.");
			if (withBack)
			{
				Singleton<EventManager>.instance.Invoke("UI/DisableInputKey");
			}
			Action action = delegate
			{
				Singleton<AudioManager>.instance.bgm.Stop();
				if ((bool)SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs)
				{
					SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs.mute = false;
				}
				if (SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs != null)
				{
					Singleton<PoolManager>.instance.FastDestroy(SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs.gameObject);
				}
				SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs = null;
				GameGlobal.gGameMusic.Stop();
				GameGlobal.gGameMusicScene.Stop();
				GTrigger.ClearEvent();
				GameGlobal.gTouch.ClearCustomEvent();
				Singleton<TaskStageTarget>.instance.Reset();
				Singleton<AudioManager>.instance.Unload(m_PreloadAudios);
				FeverManager.Instance.ResetFever();
				Singleton<AudioManager>.instance.bgm.loop = true;
				Singleton<SceneManager>.instance.LoadSceneViaLoadingScene(sceneName, delegate
				{
					if (calllback != null)
					{
						calllback();
					}
					if (withBack)
					{
						if (isTutorial)
						{
							if (isNew)
							{
								SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
								{
									Singleton<EventManager>.instance.Invoke("UI/EnableInputKey");
									Singleton<EventManager>.instance.Invoke("UI/OnPnlTutorialTips");
								}, 0.1f);
								SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
								{
									isNew = false;
								}, 3f);
							}
							else
							{
								Singleton<UIManager>.instance["PnlMenu"].SetActive(true);
								Singleton<UIManager>.instance["PnlMenu"].transform.Find("TagsGroupLocalization/TagsGroupPC/TglOption_Pc").GetComponent<Toggle>().isOn = true;
								Singleton<UIManager>.instance["PnlOption"].SetActive(true);
								Singleton<EventManager>.instance.Invoke("UI/EnableInputKey");
							}
						}
						else
						{
							Singleton<UIManager>.instance["PnlHome"].SetActive(false);
							Singleton<UIManager>.instance["PnlStage"].SetActive(true);
							Debug.Log("load scene call back");
							Singleton<EventManager>.instance.Invoke("UI/EnableInputKey");
						}
					}
					SingletonMonoBehaviour<MessageManager>.instance.messages.RemoveAll((IData m) => m["type"].GetResult<string>() == "rank");
				});
			};
			Boss.ReleaseReferences();
			ReleaseReferences();
			GirlManager.ReleaseReferences();
			AttackEffectManager.ReleaseReferences();
			GirlActionController.ReleaseReferences();
			GameMusicScene.ReleaseReferences();
			SpineActionController.ReleaseReferences();
			BattleEnemyManager.ReleaseReferences();
			MusicConfigReader.ReleaseReferences();
			GcControl.Enable();
			action();
		}

		public void EditorExit()
		{
			Exit("GameMain", null, false);
			Screen.autorotateToLandscapeLeft = false;
			Screen.autorotateToLandscapeRight = false;
		}

		public void TutorialSetting()
		{
			int originMusicIndex = Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].GetResult<int>();
			int originRoleIndex = Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>();
			int originElfinIndex = Singleton<DataManager>.instance["Account"]["SelectedElfinIndex"].GetResult<int>();
			Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].SetResult(-1);
			Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].SetResult(0);
			Singleton<DataManager>.instance["Account"]["SelectedElfinIndex"].SetResult(-1);
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].SetResult(originMusicIndex);
					Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].SetResult(originRoleIndex);
					Singleton<DataManager>.instance["Account"]["SelectedElfinIndex"].SetResult(originElfinIndex);
				}, 1f);
			}, () => isInGame);
		}

		public static void UnlockAll()
		{
			int count = Singleton<ConfigManager>.instance["ALBUM1"].Count;
			for (int i = 0; i < count; i++)
			{
				string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("ALBUM1", i, "uid");
				Singleton<StageManager>.instance.UnlockStage(configStringValue, false);
			}
			Singleton<DataManager>.instance["Account"]["IsUnlockAllMaster"].SetResult(true);
			Singleton<DataManager>.instance["Account"]["Items"].GetResult<List<IData>>().Clear();
			int count2 = Singleton<ConfigManager>.instance["character"].Count;
			int count3 = Singleton<ConfigManager>.instance["elfin"].Count;
			int count4 = Singleton<ConfigManager>.instance["loading"].Count;
			int count5 = Singleton<ConfigManager>.instance["welcome"].Count;
			for (int j = 0; j < count2; j++)
			{
				int configIntValue = Singleton<ConfigManager>.instance.GetConfigIntValue("character", j, "chipAmount");
				if (!Singleton<ConfigManager>.instance.GetConfigBoolValue("character", j, "hide"))
				{
					Singleton<ItemManager>.instance.AddItem("character", j, configIntValue);
				}
			}
			for (int k = 0; k < count3; k++)
			{
				int configIntValue2 = Singleton<ConfigManager>.instance.GetConfigIntValue("elfin", k, "chipAmount");
				if (!Singleton<ConfigManager>.instance.GetConfigBoolValue("elfin", k, "hide"))
				{
					Singleton<ItemManager>.instance.AddItem("elfin", k, configIntValue2);
				}
			}
			for (int l = 0; l < count4; l++)
			{
				int configIntValue3 = Singleton<ConfigManager>.instance.GetConfigIntValue("loading", l, "chipAmount");
				if (!Singleton<ConfigManager>.instance.GetConfigBoolValue("loading", l, "hide"))
				{
					Singleton<ItemManager>.instance.AddItem("loading", l, configIntValue3);
				}
			}
			for (int m = 0; m < count5; m++)
			{
				int configIntValue4 = Singleton<ConfigManager>.instance.GetConfigIntValue("welcome", m, "chipAmount");
				if (!Singleton<ConfigManager>.instance.GetConfigBoolValue("loading", m, "hide"))
				{
					Singleton<ItemManager>.instance.AddExtraItem("welcome", m, configIntValue4, false);
				}
			}
			int result = Singleton<DataManager>.instance["Account"]["Exp"].GetResult<int>();
			if (result < 9900)
			{
				Singleton<DataManager>.instance["Account"]["Exp"].SetResult(9900);
			}
			Singleton<DataManager>.instance.Save();
			Singleton<ServerManager>.instance.Synchronize();
		}

		public void ResetUserData()
		{
			Singleton<ItemManager>.instance.Reset();
			Singleton<StageManager>.instance.Reset();
			Singleton<TaskManager>.instance.Reset();
			Singleton<AchievementManager>.instance.Reset();
			Singleton<ItemManager>.instance.Reset();
			Singleton<DataManager>.instance.Reset();
			SingletonMonoBehaviour<MessageManager>.instance.Init();
			TaskManager instance = Singleton<TaskManager>.instance;
		}

		public void InitGame()
		{
			Singleton<BattleProperty>.instance.Reset();
			if (GameSceneMainController.isEditorMode)
			{
				SetAutoPlay(Singleton<ConfigManager>.instance.GetObject<bool>("IsAutoPlay"));
			}
			Singleton<SkillManager>.instance.Apply();
			m_PrefabCatchObj = new Hashtable();
			GameGlobal.gGameMissPlay = new GameMissPlay();
			GameGlobal.gGameTouchPlay = new GameTouchPlay();
			GameGlobal.gGameMusic = new GameMusic();
			GameGlobal.gGameMusicScene = new GameMusicScene();
			FixUpdateTimer timer = new FixUpdateTimer();
			FixUpdateTimer stepTimer = new FixUpdateTimer();
			FixUpdateTimer sceneObjTimer = new FixUpdateTimer();
			FixUpdateTimer timer2 = new FixUpdateTimer();
			GameGlobal.gGameMusic.SetTimer(timer);
			GameGlobal.gGameMusic.SetStepTimer(stepTimer);
			GameGlobal.gGameMusic.SetSceneObjTimer(sceneObjTimer);
			GameGlobal.gGameMusicScene.SetTimer(timer2);
			GameGlobal.gTouch = new TouchScript();
			GameGlobal.gTouch.OnStart();
			Load();
			BattleRoleAttributeComponent.instance.Init();
		}

		private void Load()
		{
			int musicIndex = GetMusicIndex();
			string text;
			string text2;
			string text3;
			int difficulty;
			if (GameSceneMainController.isEditorMode)
			{
				text = GetNoteJsonName();
				iBMSCManager.BMS bMS = Singleton<iBMSCManager>.instance.Load(text);
				text2 = (string)bMS.info["WAV10"];
				text3 = (string)bMS.info["GENRE"];
				difficulty = int.Parse((string)bMS.info["RANK"]);
				MusicConfigReader.Instance.bms = bMS;
				GameGlobal.gGameMusic.LoadMusicDataByFileName();
			}
			else
			{
				GameGlobal.gGameMusic.LoadMusicDataByFileName();
				text = MusicConfigReader.Instance.stageInfo.mapName;
				text2 = MusicConfigReader.Instance.stageInfo.music;
				text3 = MusicConfigReader.Instance.stageInfo.scene;
				difficulty = MusicConfigReader.Instance.stageInfo.difficulty;
			}
			SceneChangeController.curScene = int.Parse(text3.Replace("scene_0", string.Empty));
			SetDifficulty(difficulty);
			Debug.Log("Load stage (" + musicIndex + ") " + text3 + " ===== " + text + " ===== " + text2);
			PlayMusic(text2);
			Singleton<AudioManager>.instance.bgm.mute = true;
			Singleton<AudioManager>.instance.bgm.loop = false;
			InitTimeNodeOrder();
			GameGlobal.gGameMusicScene.LoadScene(musicIndex, text3);
		}

		public Dictionary<int, List<TimeNodeOrder>> CreateTimeNodeOrder(List<MusicData> musicDatas)
		{
			Dictionary<int, List<TimeNodeOrder>> dictionary = new Dictionary<int, List<TimeNodeOrder>>();
			for (int i = 0; i < musicDatas.Count; i++)
			{
				MusicData md = musicDatas[i];
				if (md.noteData.type == 0)
				{
					continue;
				}
				decimal d = 0.001m;
				int num = (int)(md.tick / d);
				if (md.noteData.type == 2)
				{
					md.noteData.right_perfect_range += Singleton<BattleProperty>.instance.blockRP;
					md.noteData.right_great_range += Singleton<BattleProperty>.instance.blockRG;
				}
				else if (md.noteData.addCombo)
				{
					md.noteData.left_perfect_range += Singleton<BattleProperty>.instance.hitRangePerfectAdded;
					md.noteData.right_perfect_range += Singleton<BattleProperty>.instance.hitRangePerfectAdded;
				}
				int num2 = (int)((md.noteData.left_great_range + md.noteData.left_perfect_range) / d);
				int num3 = (int)((md.noteData.right_great_range + md.noteData.right_perfect_range) / d);
				if (!md.isLongPressing)
				{
					if (md.isLongPressEnd)
					{
						MusicData musicData = musicDatas.Find((MusicData m) => m.configData.time == md.longPressPTick && m.isAir == md.isAir && m.configData.length > 0m);
						num3 = ((musicData.noteData.right_great_range + musicData.noteData.right_perfect_range > musicData.configData.length) ? ((int)((musicData.noteData.right_great_range + musicData.noteData.right_perfect_range - musicData.configData.length) / d)) : 0);
					}
					decimal left_perfect_range = md.noteData.left_perfect_range;
					decimal right_perfect_range = md.noteData.right_perfect_range;
					for (int j = 0; j <= num3; j++)
					{
						TimeNodeOrder timeNodeOrder = new TimeNodeOrder();
						timeNodeOrder.idx = md.objId;
						timeNodeOrder.isLongPressEnd = md.isLongPressEnd;
						timeNodeOrder.isLongPressStart = md.isLongPressStart;
						timeNodeOrder.isLongPressing = false;
						timeNodeOrder.enableJump = (md.noteData.pathway == 0);
						timeNodeOrder.result = 4;
						timeNodeOrder.isPerfectNode = ((!md.isLongPressEnd) ? (j == 0) : (j == num3 || j == 0));
						timeNodeOrder.isAir = md.isAir;
						timeNodeOrder.isLast = (j == num3);
						timeNodeOrder.isFirst = false;
						timeNodeOrder.isMulStart = md.isMul;
						timeNodeOrder.isMuling = false;
						timeNodeOrder.md = md;
						timeNodeOrder.isRight = (j != 0);
						TimeNodeOrder timeNodeOrder2 = timeNodeOrder;
						decimal d2 = (decimal)j * d;
						if (d2 > right_perfect_range)
						{
							timeNodeOrder2.result = 3;
						}
						int idx = num + j;
						AddTimeNodeOrder(idx, timeNodeOrder2, dictionary);
					}
					bool enableJump = md.noteData.pathway == 0;
					for (int k = -num2; k < 0; k++)
					{
						TimeNodeOrder timeNodeOrder = new TimeNodeOrder();
						timeNodeOrder.idx = md.objId;
						timeNodeOrder.enableJump = enableJump;
						timeNodeOrder.result = 4;
						timeNodeOrder.isLongPressEnd = md.isLongPressEnd;
						timeNodeOrder.isLongPressStart = md.isLongPressStart;
						timeNodeOrder.isLongPressing = false;
						timeNodeOrder.isPerfectNode = false;
						timeNodeOrder.isAir = md.isAir;
						timeNodeOrder.isLast = false;
						timeNodeOrder.isFirst = (k == -num2);
						timeNodeOrder.isMulStart = md.isMul;
						timeNodeOrder.isMuling = false;
						timeNodeOrder.md = md;
						timeNodeOrder.isRight = false;
						TimeNodeOrder timeNodeOrder3 = timeNodeOrder;
						decimal d3 = (decimal)k * d;
						if (-d3 > left_perfect_range)
						{
							timeNodeOrder3.result = 3;
						}
						int idx2 = num + k;
						AddTimeNodeOrder(idx2, timeNodeOrder3, dictionary);
					}
					if (md.isMul)
					{
						int num4 = (int)(md.configData.length / d);
						for (int n = -num2; n < 0; n++)
						{
							TimeNodeOrder timeNodeOrder = new TimeNodeOrder();
							timeNodeOrder.idx = md.objId;
							timeNodeOrder.isLongPressEnd = false;
							timeNodeOrder.isLongPressStart = false;
							timeNodeOrder.isLongPressing = false;
							timeNodeOrder.enableJump = enableJump;
							timeNodeOrder.result = 4;
							timeNodeOrder.isPerfectNode = false;
							timeNodeOrder.isAir = md.isAir;
							timeNodeOrder.isLast = false;
							timeNodeOrder.isFirst = false;
							timeNodeOrder.isMulStart = false;
							timeNodeOrder.isMuling = true;
							timeNodeOrder.md = md;
							timeNodeOrder.isRight = false;
							TimeNodeOrder tno = timeNodeOrder;
							int idx3 = num + n;
							AddTimeNodeOrder(idx3, tno, dictionary);
						}
						for (int num5 = 0; num5 <= num4; num5++)
						{
							TimeNodeOrder timeNodeOrder = new TimeNodeOrder();
							timeNodeOrder.idx = md.objId;
							timeNodeOrder.isLongPressEnd = false;
							timeNodeOrder.isLongPressStart = false;
							timeNodeOrder.isLongPressing = false;
							timeNodeOrder.enableJump = enableJump;
							timeNodeOrder.result = 4;
							timeNodeOrder.isPerfectNode = false;
							timeNodeOrder.isAir = md.isAir;
							timeNodeOrder.isLast = (num5 == num4);
							timeNodeOrder.isFirst = false;
							timeNodeOrder.isMulStart = false;
							timeNodeOrder.isMuling = true;
							timeNodeOrder.md = md;
							timeNodeOrder.isRight = true;
							TimeNodeOrder tno2 = timeNodeOrder;
							int idx4 = num + num5;
							AddTimeNodeOrder(idx4, tno2, dictionary);
						}
					}
					continue;
				}
				num3 = Mathf.CeilToInt((float)(0.05m / d));
				num2 = num3;
				bool enableJump2 = md.noteData.pathway == 0;
				for (int num6 = 0; num6 <= num3; num6++)
				{
					TimeNodeOrder timeNodeOrder = new TimeNodeOrder();
					timeNodeOrder.idx = md.objId;
					timeNodeOrder.enableJump = enableJump2;
					timeNodeOrder.result = 3;
					timeNodeOrder.isPerfectNode = (num6 == 0);
					timeNodeOrder.isAir = md.isAir;
					timeNodeOrder.isLongPressEnd = false;
					timeNodeOrder.isLongPressStart = false;
					timeNodeOrder.isLongPressing = true;
					timeNodeOrder.isLast = (num6 == num3);
					timeNodeOrder.isFirst = false;
					timeNodeOrder.isMulStart = false;
					timeNodeOrder.isMuling = false;
					timeNodeOrder.md = md;
					timeNodeOrder.isRight = true;
					TimeNodeOrder tno3 = timeNodeOrder;
					int num7 = num + num6;
					if (num7 >= md.endIndex)
					{
						break;
					}
					AddLongPressingTimeNodeOrder(num7, tno3, dictionary);
				}
				for (int num8 = -num2; num8 < 0; num8++)
				{
					TimeNodeOrder timeNodeOrder = new TimeNodeOrder();
					timeNodeOrder.idx = md.objId;
					timeNodeOrder.enableJump = enableJump2;
					timeNodeOrder.result = 3;
					timeNodeOrder.isPerfectNode = false;
					timeNodeOrder.isAir = md.isAir;
					timeNodeOrder.isLongPressEnd = false;
					timeNodeOrder.isLongPressStart = false;
					timeNodeOrder.isLongPressing = true;
					timeNodeOrder.isLast = false;
					timeNodeOrder.isFirst = false;
					timeNodeOrder.isMulStart = false;
					timeNodeOrder.isMuling = false;
					timeNodeOrder.md = md;
					timeNodeOrder.isRight = false;
					TimeNodeOrder tno4 = timeNodeOrder;
					int idx5 = num + num8;
					AddLongPressingTimeNodeOrder(idx5, tno4, dictionary);
				}
			}
			foreach (List<TimeNodeOrder> value in dictionary.Values)
			{
				if (value == null || value.Count <= 1)
				{
					continue;
				}
				value.Sort(delegate(TimeNodeOrder l, TimeNodeOrder r)
				{
					if ((l.isAir && r.isAir) || (!l.isAir && !r.isAir))
					{
						return (!(l.md.tick - r.md.tick < 0m)) ? 1 : (-1);
					}
					return (!l.isAir) ? 1 : (-1);
				});
			}
			return dictionary;
		}

		private void InitTimeNodeOrder()
		{
			m_TimeNodeOrders = CreateTimeNodeOrder(m_MusicTickData);
		}

		private void AddTimeNodeOrder(int idx, TimeNodeOrder tno, Dictionary<int, List<TimeNodeOrder>> tons)
		{
			if (tno.md.tick > lastTnoTime && (tno.md.noteData.addCombo || tno.md.noteData.type == 2 || tno.md.noteData.type == 7 || tno.md.noteData.type == 6))
			{
				lastTnoTime = tno.md.tick;
				if (tno.md.noteData.type == 8)
				{
					lastTnoTime += tno.md.configData.length;
				}
				lastTnoIndex = tno.idx;
			}
			if (!tons.ContainsKey(idx))
			{
				tons[idx] = new List<TimeNodeOrder>
				{
					tno
				};
				return;
			}
			List<TimeNodeOrder> list = tons[idx];
			if (tno.isLongPressStart && list.Exists((TimeNodeOrder o) => o.isLongPressEnd))
			{
				int index = list.FindIndex((TimeNodeOrder o) => o.isLongPressEnd);
				list.Insert(index, tno);
			}
			else
			{
				list.Add(tno);
			}
		}

		private void AddLongPressingTimeNodeOrder(int idx, TimeNodeOrder tno, Dictionary<int, List<TimeNodeOrder>> tons)
		{
			if (!tons.ContainsKey(idx))
			{
				tons[idx] = new List<TimeNodeOrder>
				{
					tno
				};
				return;
			}
			List<TimeNodeOrder> list = tons[idx];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].isAir == tno.isAir)
				{
					return;
				}
			}
			list.Add(tno);
		}

		private void InitData()
		{
			m_PreloadAudios = new List<string>
			{
				"char_common_empty_atk",
				"char_common_empty_jump"
			};
			LoadMusicData();
			List<NoteConfigData> data = NodeConfigReader.Instance.GetData();
			for (int i = 0; i < data.Count; i++)
			{
				NoteConfigData noteConfigData = data[i];
				string key_audio = noteConfigData.key_audio;
				if (!(key_audio == "0") && !m_PreloadAudios.Contains(key_audio))
				{
					m_PreloadAudios.Add(key_audio);
				}
			}
			for (int j = 0; j < 16; j++)
			{
				m_PreloadAudios.Add($"hitsound_{j:D3}");
			}
			Singleton<AudioManager>.instance.Preload(m_PreloadAudios, true);
			string[] assetNames = new string[2]
			{
				"sfx_press",
				"char_common_fever"
			};
			Singleton<AudioManager>.instance.Preload(assetNames, false);
			MusicConfigReader.Instance.ClearData();
			NodeConfigReader.Instance.ClearData();
			if (Singleton<XDSDKManager>.instance.isOvearSea)
			{
			}
		}

		private void LoadMusicData()
		{
			isSceneChangeType = false;
			NodeConfigReader.Instance.Init();
			string noteJsonName = GetNoteJsonName();
			m_MusicTickData = ((!GameSceneMainController.isEditorMode) ? MusicConfigReader.Instance.GetMusicDataFromStageInfo(noteJsonName) : (from m in MusicConfigReader.Instance.GetData(noteJsonName).ToArray()
				select (MusicData)m));
			MusicConfigReader.Instance.LoadJKSkill(m_MusicTickData);
			List<string> longPressPrefabName = new List<string>();
			curSceneChangeType = new List<string>();
			int num = 0;
			List<MusicData> list = new List<MusicData>();
			for (int i = 0; i < m_MusicTickData.Count; i++)
			{
				MusicData musicData = m_MusicTickData[i];
				string ibmsId = musicData.noteData.ibms_id;
				string a = null;
				if (string.IsNullOrEmpty(ibmsId))
				{
					continue;
				}
				foreach (KeyValuePair<string, int> item in sceneInfo)
				{
					if (item.Key.Contains(ibmsId))
					{
						if (a != ibmsId)
						{
							list.Add(m_MusicTickData[i]);
							a = ibmsId;
						}
						if (string.IsNullOrEmpty(curSceneChangeType.Find((string p) => p == ibmsId)))
						{
							curSceneChangeType.Add(ibmsId);
						}
						if (!isSceneChangeType)
						{
							isSceneChangeType = true;
						}
						num = item.Value;
					}
					if (!isSceneChangeType)
					{
						continue;
					}
					MusicData musicData2 = m_MusicTickData[i];
					if (musicData2.noteData.prefab_name[1] == '0')
					{
						continue;
					}
					MusicData musicData3 = m_MusicTickData[i];
					if (musicData3.noteData.prefab_name[1] != 'o')
					{
						MusicData musicData4 = m_MusicTickData[i];
						if (musicData4.noteData.prefab_name[1] != 'm')
						{
							MusicData musicData5 = m_MusicTickData[i];
							musicData5.noteData.prefab_name = musicData5.noteData.prefab_name.Remove(1, 1);
							musicData5.noteData.prefab_name = musicData5.noteData.prefab_name.Insert(1, num.ToString());
							musicData5.configData.note_uid = musicData5.configData.note_uid.Remove(1, 1);
							musicData5.configData.note_uid = musicData5.configData.note_uid.Insert(1, num.ToString());
							SetMusicData(musicData5);
						}
					}
				}
			}
			m_MusicTickData.For(delegate(MusicData md)
			{
				if (md.isLongPressType)
				{
					string prefab_name = md.noteData.prefab_name;
					if (!longPressPrefabName.Contains(prefab_name))
					{
						longPressPrefabName.Add(prefab_name);
					}
				}
			});
			foreach (string item2 in longPressPrefabName)
			{
				Singleton<AssetBundleManager>.instance.LoadFromNameAsyn(item2, delegate(GameObject go)
				{
					SpineActionController component = go.GetComponent<SpineActionController>();
					GameObject rendererPreb = component.rendererPreb;
					go = UnityEngine.Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(rendererPreb.name));
					go.transform.position = new Vector3(9999f, 9999f, 9999f);
					go.SetActive(false);
				});
			}
			if (!isSceneChangeType)
			{
				return;
			}
			for (int j = 0; j < m_MusicTickData.Count; j++)
			{
				MusicData musicData6 = m_MusicTickData[j];
				if (string.IsNullOrEmpty(musicData6.noteData.ibms_id))
				{
					continue;
				}
				MusicData musicData7 = m_MusicTickData[j];
				musicData7.noteData.sceneChangeNames = new List<string>();
				MusicData musicData8 = m_MusicTickData[j];
				if (musicData8.noteData.prefab_name[1] == '0')
				{
					continue;
				}
				MusicData musicData9 = m_MusicTickData[j];
				if (musicData9.noteData.prefab_name[1] == 'o')
				{
					continue;
				}
				MusicData musicData10 = m_MusicTickData[j];
				if (musicData10.noteData.prefab_name[1] == 'm')
				{
					continue;
				}
				foreach (MusicData item3 in list)
				{
					MusicData musicData11 = m_MusicTickData[j];
					decimal d = musicData11.configData.time - item3.configData.time;
					if (d > 0m && d < 2m)
					{
						musicData7.noteData.sceneChangeNames.Add(item3.noteData.ibms_id);
						SetMusicData(musicData7);
					}
				}
			}
		}

		public List<TimeNodeOrder> GetTimeNodeByTick(float tick)
		{
			int key = (int)(tick * 1000f);
			if (!m_TimeNodeOrders.ContainsKey(key))
			{
				return null;
			}
			return m_TimeNodeOrders[key];
		}

		public List<TimeNodeOrder> GetTimeNodeByTick(decimal tick)
		{
			int key = (int)(tick * 1000m);
			if (!m_TimeNodeOrders.ContainsKey(key))
			{
				return null;
			}
			return m_TimeNodeOrders[key];
		}

		public TimeNodeOrder[] GetAllTimeNodeByTick(decimal tick, ref int count)
		{
			int num = Mathf.RoundToInt((float)tick / 0.001f);
			int num2 = num - 5;
			int num3 = num + 5;
			for (int i = num2; i < num3; i++)
			{
				if (!m_TimeNodeOrders.ContainsKey(i))
				{
					continue;
				}
				List<TimeNodeOrder> list = m_TimeNodeOrders[i];
				if (list != null)
				{
					for (int j = 0; j < list.Count; j++)
					{
						m_TmpTnos[count++] = list[j];
					}
				}
			}
			return m_TmpTnos;
		}

		public MusicData GetCurMusicData(bool isAir = false)
		{
			TimeNodeOrder timeNodeOrder = (!isAir) ? curTimeNodeOrder : curAirTimeNodeOrder;
			if (timeNodeOrder == null)
			{
				return default(MusicData);
			}
			short idx = timeNodeOrder.idx;
			return GetMusicDataByIdx(idx);
		}

		public void OnFocusChangePauseGame()
		{
			Debug.Log("Pause Game");
			if (isInGame)
			{
				if (!Singleton<UIManager>.instance["PnlFailText"].activeInHierarchy && !Singleton<UIManager>.instance["PnlFail"].activeInHierarchy)
				{
					GameObject gameObject = GameObject.Find("Pnl321");
					bool flag = (bool)gameObject && gameObject.activeSelf;
					if (!isPause || flag)
					{
						Singleton<EventManager>.instance.Invoke("UI/OnShowPnlPause");
						Singleton<EventManager>.instance.Invoke("Battle/OnPause");
						Pause();
					}
				}
				return;
			}
			GameObject gameObject2 = GameObject.Find("Tutorial");
			if ((bool)gameObject2 && gameObject2.transform.childCount > 0)
			{
				Debug.Log("Pause Tutorial");
				AnimatorStateInfo currentAnimatorStateInfo = gameObject2.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
				float num = currentAnimatorStateInfo.normalizedTime * currentAnimatorStateInfo.length;
				if (13.5f - num > 0f)
				{
					SingletonMonoBehaviour<CoroutineManager>.instance.Delay(OnFocusChangePauseGame, 13.5f - num);
				}
			}
			GameObject gameObject3 = GameObject.Find("PnlReadyGo");
			if ((bool)gameObject3)
			{
				Debug.Log("Pause Ready");
				AnimatorStateInfo currentAnimatorStateInfo2 = gameObject3.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);
				if (currentAnimatorStateInfo2.normalizedTime < 1f)
				{
					SingletonMonoBehaviour<CoroutineManager>.instance.Delay(OnFocusChangePauseGame, (1f - currentAnimatorStateInfo2.normalizedTime + 0.1f) * currentAnimatorStateInfo2.length);
				}
			}
		}
	}
}
