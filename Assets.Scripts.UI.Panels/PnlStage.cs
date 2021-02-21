using Assets.Scripts.Common.XDSDK;
using Assets.Scripts.GameCore;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Components;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Events;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Platforms.Steam;
using Assets.Scripts.PeroTools.PreWarm;
using Assets.Scripts.PeroTools.UI;
using Assets.Scripts.UI.Controls;
using Assets.Scripts.UI.Specials;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
	public class PnlStage : VisBasePropertyModifier, IPreWarm
	{
		private delegate JToken GetData();

		private delegate int PointerAppend(int x, int cnt);

		public struct musicInfo
		{
			public string uid;

			public string album;

			public string albumUid;

			public int albumMusicIndex;

			public int level;

			public bool isUnlock;

			public int albumJsonIndex;

			public int getDifficulty;
		}

		public class albumInfo
		{
			public string uid;

			public string name;

			public List<string> list;

			public List<string> nameList;

			public bool isWeekFree;

			public bool isNew;
		}

		public struct defaultInfo
		{
			public string uid;

			public int level;
		}

		internal delegate void OnMusicCellSelected(float i);

		public struct musicInfoFromAlbum
		{
			public string uid;

			public string albumName;

			public string albumUid;

			public int level;

			public int difficulty1;

			public int difficulty2;

			public int difficulty3;
		}

		[Title("Album", null, TitleAlignments.Left, true, true)]
		[Space(15f)]
		[Required]
		public GameObject pnlAlbum;

		[Required]
		public FancyScrollView albumFancyScrollView;

		[Required]
		public FancyScrollView albumFancyScrollViewNs;

		[Required]
		public Transform albumFancyScrollViewContent;

		[Required]
		public FancyScrollView albumDifficultyFancyScrollView;

		[Required]
		public AlbumDifficultyCell[] allAlbumDifficultyCells;

		[Required]
		public AlbumCell albumCellPrefabs;

		[Required]
		public AlbumCell newAlbumCellPrefabs;

		[Required]
		public Button btnCloseAlbumBgs;

		[Required]
		public Button btnCloseAlbumDiffBgs;

		[Required]
		public Button btnCloseAlbumSelect;

		[Required]
		public Button btnCloseAlbumDiffSelect;

		[Required]
		public Button btnOwn;

		[Required]
		public Text titleOwn;

		[Required]
		public GameObject imgFavoriteMusic;

		[Title("Music", null, TitleAlignments.Left, true, true)]
		[Space(15f)]
		[Required]
		public FancyScrollView musicFancyScrollView;

		[Required]
		public Text musicNameTitle;

		[Required]
		public Text artistNameTitle;

		[Title("Anim", null, TitleAlignments.Left, true, true)]
		[Space(15f)]
		[Required]
		public GameObject musicRoot;

		[Required]
		public string animNameAlbumIn;

		[Required]
		public string animNameAlbumOut;

		[Title("Special(特殊逻辑使用)", null, TitleAlignments.Left, true, true)]
		public Button btnAhcievement;

		[Required]
		public Button btnEnter;

		[Required]
		public Button btnBackInPnlMenu;

		[Required]
		public Button btnPlay;

		[Required]
		public ShowText showText;

		[Required]
		public GameObject pnlPreparation;

		[Required]
		public Button btnBackToStage;

		public float demoMusicFadeInTime = 0.25f;

		public GameObject difficult1Lock;

		public GameObject difficult2Lock;

		[Required]
		public Text difficulty1;

		[Required]
		public Text difficulty2;

		[Required]
		public Text difficulty3;

		[Required]
		public GameObject difficulty1Easy;

		[Required]
		public GameObject difficulty2Hard;

		[Required]
		public GameObject difficulty3Empty;

		[Required]
		public GameObject difficulty3Lock;

		[Required]
		public GameObject difficulty3Master;

		[Required]
		public GameObject emptyEasy;

		[Required]
		public GameObject emptyMaster;

		[Required]
		public Toggle tglLike;

		private Image m_tglLikeImg;

		[Required]
		public GameObject achievementObj;

		[Required]
		public Image imgLikeOn;

		[Required]
		public Image imgLikeHide;

		[Required]
		public GameObject stageAchievementPercent;

		[Required]
		public GameObject nsNoCollectionTip;

		[Required]
		public GameObject noMusicFromHide;

		[Required]
		public GameObject noMusicFromDiff;

		[Required]
		public GameObject bgAlbumLock;

		[Required]
		public GameObject bgAlbumFree;

		[Required]
		public GameObject txtNotPurchase;

		[Required]
		public GameObject txtFreeSong;

		[Required]
		public GameObject txtFreeSongNoPurchase;

		[Required]
		public GameObject txtBudgetIsBurning15;

		[Required]
		public GameObject txtBudgetIsBurning30;

		[Required]
		public GameObject txtBurnV1NotPurchase;

		[Required]
		public GameObject txtBurnV2NotPurchase;

		[Required]
		public Image bgFreeDiscount;

		[Required]
		public Toggle tglDifficultyEasy;

		[Required]
		public Toggle tglDifficultyHard;

		[Required]
		public Toggle tglDifficultyMaster;

		[Required]
		public Button btnMenuBackToStage;

		[Required]
		public GameObject[] pnlPreparationObjs;

		public Scrollbar musicFsvShow;

		public Scrollbar musicFsvPlay;

		public Font specialFont;

		public Font simpleFont;

		private IVariable m_VarCurSelectedAlbumUid;

		private IVariable m_VarCurSelectedMusicIndex;

		private IVariable m_VarCurSelectedMusicIndexInAllList;

		private IVariable m_VarCurSelectedAblumTagIndex;

		private IVariable m_VarCurSelectedAlbumTagScrollIndex;

		private IVariable m_VarCurSelectedAlbumDifficulty;

		private IVariable m_VarCurSelectedMusicUidFromInfoList;

		private string m_SelectedMusicUidFromInfoList;

		private Animator m_MusicRootAnimator;

		private List<string> m_Collection;

		private List<string> m_Hide;

		private Coroutine m_Nume;

		private bool m_CanPlayMusic = true;

		private int m_CurrentHighlighAlbumIndex;

		private int m_CurrentHighlighAlbumDifficultyIndex;

		private CoroutineManager m_CoroutineManager;

		private Effect m_ParTglLikes;

		private Effect m_ParTglDisLikes;

		private Effect m_ParTglHide;

		private Dictionary<string, GetData>[] m_AlbumDatas;

		public string selectedAlbumTagIndex;

		private bool m_IsClickRandom;

		private int randomIndex;

		private bool m_IsClickHide;

		[SerializeField]
		private List<GameObject> m_AlbumFSVCells = new List<GameObject>();

		private bool m_IsLock;

		private int m_CurAlbumTag;

		private int m_CurDifficutyTag;

		private int m_AllDifficulty;

		private int m_T1Difficulty = 1;

		private int m_T2Difficulty = 2;

		private int m_T3Difficulty = 4;

		private int m_T4Difficulty = 8;

		private int m_T5Difficulty = 16;

		private int[] m_AllAlbumDifficultys = new int[6]
		{
			0,
			1,
			2,
			4,
			8,
			16
		};

		private Dictionary<int, string> m_AllDifficultyTextDic = new Dictionary<int, string>
		{
			{
				0,
				"全部"
			},
			{
				1,
				"1 ～ 3"
			},
			{
				2,
				"4 ～ 6"
			},
			{
				3,
				"7 ～ 8"
			},
			{
				4,
				"9 ～ 10"
			},
			{
				5,
				"11"
			}
		};

		private List<albumInfo> m_AllAlbumTagData = new List<albumInfo>
		{
			new albumInfo
			{
				uid = "collections",
				name = "收藏",
				list = new List<string>(),
				nameList = new List<string>(),
				isWeekFree = false,
				isNew = false
			},
			new albumInfo
			{
				uid = "tag-1",
				name = "全部",
				list = new List<string>(),
				nameList = new List<string>(),
				isWeekFree = false,
				isNew = false
			},
			new albumInfo
			{
				uid = "tag-2",
				name = "基础包",
				list = new List<string>(),
				nameList = new List<string>(),
				isWeekFree = false,
				isNew = false
			},
			new albumInfo
			{
				uid = "tag-3",
				name = "放弃治疗",
				list = new List<string>(),
				nameList = new List<string>(),
				isWeekFree = false,
				isNew = false
			},
			new albumInfo
			{
				uid = "tag-4",
				name = "肥宅快乐包",
				list = new List<string>(),
				nameList = new List<string>(),
				isWeekFree = false
			},
			new albumInfo
			{
				uid = "tag-5",
				name = "可爱即正义",
				list = new List<string>(),
				nameList = new List<string>(),
				isWeekFree = false,
				isNew = false
			},
			new albumInfo
			{
				uid = "tag-6",
				name = "暮 色 电 台",
				list = new List<string>(),
				nameList = new List<string>(),
				isWeekFree = false,
				isNew = false
			},
			new albumInfo
			{
				uid = "tag-7",
				name = "联动",
				list = new List<string>(),
				nameList = new List<string>(),
				isWeekFree = false,
				isNew = false
			},
			new albumInfo
			{
				uid = "hide",
				name = "隐藏",
				list = new List<string>(),
				nameList = new List<string>(),
				isWeekFree = false,
				isNew = false
			}
		};

		private List<string> m_MusicRangeList = new List<string>();

		private List<int> m_MusicRangeLevelList = new List<int>();

		private List<musicInfo> m_MusicRangeInfoList = new List<musicInfo>();

		private List<musicInfo> m_RandomMusicRangeInfoList = new List<musicInfo>();

		private string m_BudgeBruningV1;

		private string m_BudgeBruningV2;

		public static List<string> m_MusicNameList = new List<string>();

		private List<string> m_AllAlbumUid = new List<string>();

		private List<string> m_AllAlbumName = new List<string>();

		private List<string> m_AllBaseAlbumUid = new List<string>();

		private List<string> m_AllGiveUpAlbumUid = new List<string>();

		private List<string> m_AllGiveUpAlbumUid_Re = new List<string>();

		private List<string> m_AllGiveUpAlbumName_Re = new List<string>();

		private List<string> m_AllGiveUpAlbumName = new List<string>();

		private List<string> m_AllHappyAlbumUid = new List<string>();

		private List<string> m_AllHappyAlbumUid_Re = new List<string>();

		private List<string> m_AllHappyAlbumName_Re = new List<string>();

		private List<string> m_AllHappyAlbumName = new List<string>();

		private List<string> m_AllCuteAlbumUid = new List<string>();

		private List<string> m_AllCuteAlbumUid_Re = new List<string>();

		private List<string> m_AllCuteAlbumName_Re = new List<string>();

		private List<string> m_AllCuteAlbumName = new List<string>();

		private List<string> m_AllBurningAlbumUid = new List<string>();

		private List<string> m_AllBurningAlbumUid_Re = new List<string>();

		private List<string> m_AllBurningAlbumName_Re = new List<string>();

		private List<string> m_AllBurningAlbumName = new List<string>();

		private List<string> m_AllOtherAlbumUid = new List<string>();

		private List<string> m_AllOtherAlbumUid_Re = new List<string>();

		private List<string> m_AllOtherAlbumName_Re = new List<string>();

		private List<string> m_AllOtherAlbumName = new List<string>();

		private List<string> m_AllMuseRadioAlbumUid = new List<string>();

		private List<string> m_AllMuseRadioAlbumUid_Re = new List<string>();

		private List<string> m_AllMuseRadioAlbumName_Re = new List<string>();

		private List<string> m_AllMuseRadioAlbumName = new List<string>();

		internal static OnMusicCellSelected m_MusicCellSelected;

		private bool m_IsRandom;

		private musicInfo m_RandomMusicInfo;

		[SerializeField]
		private GameObject m_AlbumTitleObj;

		[SerializeField]
		private Text m_AlbumTitleTxt;

		private StageLikeToggle m_TglLikeScript;

		[SerializeField]
		private Text m_DifficultyTagTxt;

		private Dictionary<string, musicInfoFromAlbum[]> m_AllJson = new Dictionary<string, musicInfoFromAlbum[]>();

		public List<string> newAlbumUids = new List<string>();

		private bool m_IsInitedAlbumFSV;

		private bool m_IsInitedNewAlbum;

		private List<int> m_CurWeekFreeIndexs = new List<int>();

		private List<albumInfo> m_NewAlbumTagData = new List<albumInfo>();

		private List<int> m_AddDataIndexs = new List<int>();

		private bool m_IsHideToNoMusic;

		private bool m_IsDiffToNoMusic;

		private int[] m_AllStageDiff = new int[3];

		private int m_CurDiffIndex;

		private bool isCollection => selectedAlbumTagIndex == "collections";

		private bool isHide => selectedAlbumTagIndex == "hide";

		public int selectedAlbumIndex => GetAlbumIndexByUid(m_VarCurSelectedAlbumUid.GetResult<string>());

		public int selectedAlbumIndex_New => GetAlbumTagListIndex(selectedAlbumTagIndex);

		public int selectedAlbumDifficulty => m_VarCurSelectedAlbumDifficulty.GetResult<int>();

		public int selectedAlbumDifficultyIndex => GetAlbumDifficultListIndex(m_VarCurSelectedAlbumDifficulty.GetResult<int>());

		public string selectedAlbumUid => m_VarCurSelectedAlbumUid.GetResult<string>();

		public int selectedMusicIndex => m_VarCurSelectedMusicIndex.GetResult<int>();

		public int selectedMusicIndexInAllList => m_VarCurSelectedMusicIndexInAllList.GetResult<int>();

		public string selectedMusicUid
		{
			get
			{
				if (isCollection)
				{
					if (selectedMusicIndexInAllList < m_MusicRangeInfoList.Count)
					{
						musicInfo musicInfo = m_MusicRangeInfoList[selectedMusicIndexInAllList];
						return musicInfo.uid;
					}
					return string.Empty;
				}
				if (m_IsRandom && m_IsClickRandom && m_RandomMusicRangeInfoList.Count > 0)
				{
					if (randomIndex >= m_RandomMusicRangeInfoList.Count)
					{
						randomIndex = 0;
					}
					musicInfo musicInfo2 = m_RandomMusicRangeInfoList[randomIndex];
					return musicInfo2.uid;
				}
				return m_SelectedMusicUidFromInfoList;
			}
		}

		public int GetAlbumIndexByUid(string uid)
		{
			for (int i = 0; i < m_AlbumDatas.Length; i++)
			{
				if (GetAlbumUid(i) == uid)
				{
					return i;
				}
			}
			return -1;
		}

		public string GetAlbumUid(int albumIndex)
		{
			if (m_AlbumDatas[albumIndex] == null)
			{
				Debug.Log("Null");
			}
			return (string)m_AlbumDatas[albumIndex]["uid"]();
		}

		public string GetAlbumTitle(int albumIndex)
		{
			return (string)m_AlbumDatas[albumIndex]["title"]();
		}

		public bool GetAlbumNeedPurchase(int albumIndex)
		{
			return (bool)m_AlbumDatas[albumIndex]["needPurchase"]();
		}

		public float GetAlbumPrice(int albumIndex)
		{
			return (float)m_AlbumDatas[albumIndex]["price"]();
		}

		public string GetSelectedMusicAlbumJsonName()
		{
			string selectedMusicUid = this.selectedMusicUid;
			if (string.IsNullOrEmpty(selectedMusicUid))
			{
				return string.Empty;
			}
			if (isCollection)
			{
				return $"ALBUM{int.Parse(selectedMusicUid.Split('-')[0]) + 1}";
			}
			return GetAlbumJsonName(selectedAlbumIndex);
		}

		public string GetAlbumJsonName(int albumIndex)
		{
			if (albumIndex == -1)
			{
				albumIndex = 0;
			}
			return (string)m_AlbumDatas[albumIndex]["jsonName"]();
		}

		public string GetAlbumPrefabsName(int albumIndex)
		{
			if (albumIndex == -1)
			{
				albumIndex = 0;
			}
			return (string)m_AlbumDatas[albumIndex]["prefabsName"]();
		}

		public string GetMusicUid(int musidIndex)
		{
			if (isCollection)
			{
				List<string> result = Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>();
				if (selectedMusicIndexInAllList < result.Count)
				{
					return result[selectedMusicIndexInAllList];
				}
				return string.Empty;
			}
			return Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>();
		}

		public void PlayCurrentSelectedMusicIfNeed()
		{
			if (m_MusicRangeInfoList.Count != 0)
			{
				musicInfo musicInfo = m_MusicRangeInfoList[selectedMusicIndexInAllList];
				if (musicInfo.uid == "?")
				{
					return;
				}
			}
			if (musicRoot.activeSelf)
			{
				if (IsSelectedMusicUnlock())
				{
					PlayMusic();
				}
				else
				{
					Singleton<AudioManager>.instance.StopBGM();
				}
			}
		}

		public override void SetProperty(float propertyValue)
		{
			OnFancyScrollViewCellUpdate cell = musicFancyScrollView.GetCell(m_VarCurSelectedMusicIndexInAllList.GetResult<int>());
			if (cell != null)
			{
				if (musicFancyScrollView.state == FancyScrollView.State.Static)
				{
					VisPropertyHelper.SetGameObjectProperty(cell.transform.GetChild(0).gameObject, GameObjectProperty.UniformScale, propertyValue);
				}
				else
				{
					cell.transform.GetChild(0).localScale = Vector3.one;
				}
			}
		}

		public void SwitchToAlbum(int albumIndex)
		{
			bool flag = Singleton<WeekFreeManager>.instance.freeAlbumUids != null;
			bool flag2 = false;
			if (flag)
			{
				List<int> list = new List<int>(Singleton<WeekFreeManager>.instance.freeAlbumIndexs);
				if (albumIndex > list.Count + 1)
				{
					albumIndex -= list.Count;
					list.Sort((int a, int b) => b - a);
					for (int i = 0; i < list.Count; i++)
					{
						if (m_AlbumDatas.Length - list[i] <= albumIndex)
						{
							albumIndex++;
						}
					}
				}
				else if (albumIndex >= 2 && albumIndex <= list.Count + 1)
				{
					int num = list[albumIndex - 2];
					albumIndex = m_AlbumDatas.Length - num;
					if (m_VarCurSelectedAlbumUid.GetResult<string>() != GetAlbumUid(albumIndex))
					{
						int switchSongIndex = 0;
						for (int j = 0; j < Singleton<WeekFreeManager>.instance.freeSongUids.Length; j++)
						{
							string str = Singleton<WeekFreeManager>.instance.freeSongUids[j];
							if (str.BeginBefore('-') == num.ToString())
							{
								switchSongIndex = int.Parse(str.LastAfter('-'));
								break;
							}
						}
						flag2 = true;
						m_VarCurSelectedMusicIndex.SetResult(switchSongIndex);
						SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
						{
							musicFancyScrollView.ScrollToDataIndex(switchSongIndex, 0f);
						}, 1);
					}
				}
			}
			if (albumIndex >= 0 && albumIndex < m_AlbumDatas.Length && albumIndex != selectedAlbumIndex)
			{
				m_VarCurSelectedAlbumUid.result = GetAlbumUid(albumIndex);
				PlayAlbumSwitchAnim(true);
				if (!flag || albumIndex == 1)
				{
					albumFancyScrollViewNs.ScrollTo(albumIndex, albumFancyScrollViewNs.switchDuration);
				}
				if (!flag2)
				{
					m_VarCurSelectedMusicIndex.SetResult(0);
				}
				RefreshMusicFSV();
			}
		}

		private void SetTitleOwnText(int albumIndex)
		{
			titleOwn.text = GetAlbumTitle(albumIndex);
		}

		private void SetTitleOwnTextNew(string albumName)
		{
			titleOwn.text = albumName;
		}

		public void SetMouseWheelBinding(FancyScrollView fsv, bool enable)
		{
			InputKeyBinding[] components = fsv.btnPrevious.GetComponents<InputKeyBinding>();
			InputKeyBinding[] array = components;
			foreach (InputKeyBinding inputKeyBinding in array)
			{
				if (inputKeyBinding.axisName == "MouseAxis3")
				{
					inputKeyBinding.enabled = enable;
				}
			}
			components = fsv.btnNext.GetComponents<InputKeyBinding>();
			InputKeyBinding[] array2 = components;
			foreach (InputKeyBinding inputKeyBinding2 in array2)
			{
				if (inputKeyBinding2.axisName == "MouseAxis3")
				{
					inputKeyBinding2.enabled = enable;
				}
			}
		}

		public void SetHalloweenDifficulty()
		{
			if (DateTime.Now.Month == 11 && DateTime.Now.Day == 1)
			{
				Singleton<SpecialSongManager>.instance.SetHideBMSInfo("8-3");
			}
		}

		public void OnAlbumActive()
		{
			albumDifficultyFancyScrollView.ScrollToDataIndex(selectedAlbumDifficultyIndex, 0f);
			albumFancyScrollView.ScrollToDataIndex(selectedAlbumIndex_New, 0f);
		}

		public void PreWarm(int slice)
		{
			switch (slice)
			{
			case 0:
			{
				InitAlbumDatas();
				albumFancyScrollView.onFinalItemIndexChange += ChangeFinalAlbum;
				albumDifficultyFancyScrollView.onFinalItemIndexChange += ChangeFinalAlbumDifficulty;
				if (albumFancyScrollViewNs != null)
				{
					albumFancyScrollViewNs.onFinalItemIndexChange += ChangeFinalAblumNs;
				}
				m_MusicRootAnimator = musicRoot.GetComponent<Animator>();
				pnlAlbum.GetOrAddComponent<EnableDisableHooker>().onEnable += OnPnlAlbumEnable;
				musicRoot.GetOrAddComponent<EnableDisableHooker>().onEnable += OnPnlMusicEnable;
				musicFancyScrollView.onFinalItemIndexChange += ChangeFinalMusic;
				musicFancyScrollView.onItemIndexChange += ChangeMusic;
				musicFancyScrollView.OnStateChange += DisableBtnPlayWhenDynamic;
				btnPlay.onClick.AddListener(OnClickBtnPlay);
				btnEnter.onClick.AddListener(delegate
				{
					m_Nume = SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(EnableAndPlayMusic1SecLater());
				});
				tglLike.onValueChanged.AddListener(OnClickBtnLike);
				m_tglLikeImg = tglLike.GetComponent<Image>();
				m_TglLikeScript = tglLike.GetComponent<StageLikeToggle>();
				Singleton<EventManager>.instance.UnregEvent("UI/OnClickHide");
				Singleton<EventManager>.instance.UnregEvent("UI/IsInvokeHide");
				Singleton<EventManager>.instance.UnregEvent("UI/IsInvokeHideCancel");
				Singleton<EventManager>.instance.RegEvent("UI/OnClickHide").trigger += OnClickHide;
				Singleton<EventManager>.instance.RegEvent("UI/IsInvokeHide").trigger += OnHideMusic;
				Singleton<EventManager>.instance.RegEvent("UI/IsInvokeHideCancel").trigger += OnRemoveHideMusic;
				btnCloseAlbumBgs.onClick.AddListener(OnClickCloseAlbumBtn);
				btnCloseAlbumDiffBgs.onClick.AddListener(OnClickCloseAlbumDifficultyBtn);
				btnBackInPnlMenu.onClick.AddListener(ReplayMusicWhenFormPnlMenuToPnlStage);
				for (int i = 0; i < allAlbumDifficultyCells.Length; i++)
				{
					int index = i;
					allAlbumDifficultyCells[i].GetComponent<Button>().onClick.AddListener(delegate
					{
						m_CurrentHighlighAlbumDifficultyIndex = index;
						OnClickCloseAlbumDifficultyBtn();
					});
				}
				musicFsvPlay.onValueChanged.AddListener(OnMusicFSVPlayValueChange);
				break;
			}
			case 1:
			{
				m_Collection = Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>();
				m_Hide = Singleton<DataManager>.instance["Account"]["Hides"].GetResult<List<string>>();
				EffectManager instance = Singleton<EffectManager>.instance;
				string uid = "ParTglLike";
				Transform transform = tglLike.transform;
				m_ParTglLikes = instance.Preload(uid, 5, -1, transform);
				EffectManager instance2 = Singleton<EffectManager>.instance;
				uid = "ParTglDisLike";
				transform = tglLike.transform;
				m_ParTglDisLikes = instance2.Preload(uid, 5, -1, transform);
				m_VarCurSelectedMusicIndex = Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"];
				m_VarCurSelectedMusicIndexInAllList = Singleton<DataManager>.instance["Account"]["SelectedMusicIndexInAllList"];
				m_VarCurSelectedAlbumUid = Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"];
				m_VarCurSelectedAblumTagIndex = Singleton<DataManager>.instance["Account"]["SelectedAlbumTagUid"];
				selectedAlbumTagIndex = m_VarCurSelectedAblumTagIndex.GetResult<string>();
				m_VarCurSelectedAlbumTagScrollIndex = Singleton<DataManager>.instance["Account"]["SelectedAlbumTagIndex"];
				m_VarCurSelectedAlbumDifficulty = Singleton<DataManager>.instance["Account"]["SelectedAlbumDifficultyInt"];
				m_VarCurSelectedMusicUidFromInfoList = Singleton<DataManager>.instance["Account"]["SelectedMusicUidFromInfoList"];
				m_SelectedMusicUidFromInfoList = m_VarCurSelectedMusicUidFromInfoList.GetResult<string>();
				RangeStageList();
				albumDifficultyFancyScrollView.startIndex.result = selectedAlbumDifficultyIndex;
				break;
			}
			case 2:
				m_CoroutineManager = SingletonMonoBehaviour<CoroutineManager>.instance;
				break;
			}
		}

		private void OnMusicFSVPlayValueChange(float value)
		{
			musicFsvShow.value = value;
		}

		private new void OnDestroy()
		{
			Singleton<EventManager>.instance.RegEvent("UI/OnClickHide").trigger -= OnClickHide;
			Singleton<EventManager>.instance.RegEvent("UI/IsInvokeHide").trigger -= OnHideMusic;
			Singleton<EventManager>.instance.RegEvent("UI/IsInvokeHideCancel").trigger -= OnRemoveHideMusic;
			SpecialSongManager.onTriggerHideBmsEvent = null;
		}

		private void InitAlbumDatas()
		{
			string albumsJsonName = "albums";
			int count = Singleton<ConfigManager>.instance[albumsJsonName].Count;
			int num = 0;
			PointerAppend pointerAppend = delegate(int i, int cnt)
			{
				int num2 = (i + 1) % cnt;
				if (num2 == 1)
				{
					num2++;
				}
				return num2;
			};
			m_AlbumDatas = new Dictionary<string, GetData>[count];
			for (int j = 1; j < m_AlbumDatas.Length; j++)
			{
				int index = num;
				string jsonName = Singleton<ConfigManager>.instance.GetConfigStringValue(albumsJsonName, index, "jsonName");
				if (!string.IsNullOrEmpty(jsonName))
				{
					m_AlbumDatas[j] = new Dictionary<string, GetData>
					{
						{
							"uid",
							() => Singleton<ConfigManager>.instance.GetConfigStringValue(albumsJsonName, index, "uid")
						},
						{
							"title",
							() => Singleton<ConfigManager>.instance.GetConfigStringValue(albumsJsonName, index, "title")
						},
						{
							"prefabsName",
							() => Singleton<ConfigManager>.instance.GetConfigStringValue(albumsJsonName, index, "prefabsName")
						},
						{
							"price",
							() => Singleton<ConfigManager>.instance.GetConfigStringValue(albumsJsonName, index, "price")
						},
						{
							"jsonName",
							() => jsonName
						},
						{
							"needPurchase",
							() => Singleton<ConfigManager>.instance.GetConfigStringValue(albumsJsonName, index, "needPurchase")
						}
					};
				}
				num = pointerAppend(num, count);
			}
			m_AlbumDatas[0] = new Dictionary<string, GetData>
			{
				{
					"uid",
					() => "collections"
				},
				{
					"title",
					() => Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "collections")
				},
				{
					"needPurchase",
					() => false
				},
				{
					"jsonName",
					() => string.Empty
				},
				{
					"prefabsName",
					() => "AlbumCollection"
				}
			};
		}

		public override void Start()
		{
			base.Start();
			if (!Singleton<XDSDKManager>.instance.isOvearSea)
			{
			}
			Singleton<SpecialSongManager>.instance.Init();
			tglDifficultyEasy.onValueChanged.AddListener(HideBMSLogicListener);
			tglDifficultyHard.onValueChanged.AddListener(HideBMSLogicListener);
			tglDifficultyMaster.onValueChanged.AddListener(HideBMSLogicListener);
			tglDifficultyEasy.onValueChanged.AddListener(NacoLogicListener);
			tglDifficultyHard.onValueChanged.AddListener(NacoLogicListener);
			tglDifficultyMaster.onValueChanged.AddListener(NacoLogicListener);
			btnBackToStage.onClick.AddListener(delegate
			{
				Singleton<SpecialSongManager>.instance.ResetHideBmsCondition();
			});
			btnMenuBackToStage.onClick.AddListener(delegate
			{
				Singleton<SpecialSongManager>.instance.ResetHideBmsCondition();
			});
			if (SpecialSongManager.onTriggerHideBmsEvent == null)
			{
				SpecialSongManager.onTriggerHideBmsEvent = delegate
				{
					RefPnlPreparation();
				};
			}
			btnBackToStage.onClick.AddListener(delegate
			{
				Singleton<SpecialSongManager>.instance.SetSpecialSong();
			});
			SetAprilFoolsDayDifficulty();
			RefreshAlbumFSV();
			RefreshMusicFSV();
		}

		private void OnEnable()
		{
			if (Singleton<DataManager>.instance["IAP"]["unlockall_0"].GetResult<bool>())
			{
				Singleton<ItemManager>.instance.AddExtraItem("loading", 14, 5);
			}
			Singleton<ItemManager>.instance.CheckAndAddWelcome(0, 5, false);
			Singleton<ItemManager>.instance.CheckAndAddWelcome(1, 5, false);
			Singleton<ItemManager>.instance.CheckAndAddWelcome(2, 5, false);
			Singleton<DataManager>.instance.Save();
			InitAlbumTagCell();
			SetWeekFreeAlbum();
		}

		public void CheckFirstOpenHideVersion()
		{
			List<string> result = Singleton<DataManager>.instance["GameConfig"]["FirstOpenEvent"].GetResult<List<string>>();
			if (!result.Contains("IsFirstOpenHideVersion"))
			{
				Singleton<EventManager>.instance.Invoke("UI/OnCollectionsTip");
				result.Add("IsFirstOpenHideVersion");
				Singleton<DataManager>.instance.Save();
			}
		}

		private void OnPnlAlbumEnable(GameObject obj)
		{
			RefreshAlbumFSV();
		}

		private void OnPnlMusicEnable(GameObject obj)
		{
			RefreshMusicFSV();
		}

		private void OnDisable()
		{
			m_CanPlayMusic = true;
			if (m_CoroutineManager != null && m_Nume != null)
			{
				m_CoroutineManager.StopCoroutine(m_Nume);
				m_Nume = null;
			}
		}

		private void OnClickBtnPlay()
		{
			if (m_IsRandom && m_RandomMusicRangeInfoList.Count == 0)
			{
				return;
			}
			string hideBMSInfo = Singleton<DataManager>.instance["Account"]["SelectedDifficulty"].GetResult<int>().ToString();
			if (m_IsRandom)
			{
				m_IsClickRandom = true;
				if (m_RandomMusicRangeInfoList.Count > 0)
				{
					randomIndex = UnityEngine.Random.Range(0, m_RandomMusicRangeInfoList.Count);
					m_RandomMusicInfo = m_RandomMusicRangeInfoList[randomIndex];
					IVariable data = Singleton<DataManager>.instance["Account"]["SelectedMusicUidFromInfoList"];
					musicInfo musicInfo = m_RandomMusicRangeInfoList[randomIndex];
					data.SetResult(musicInfo.uid);
					RandomMusicStage();
					PlayMusic();
				}
			}
			SetHideBMSInfo(hideBMSInfo);
			Singleton<SpecialSongManager>.instance.SetSpecialSong(selectedMusicUid);
			SetHalloweenDifficulty();
			SetAprilFoolsDayDifficulty();
			SingletonMonoBehaviour<SteamManager>.instance.DLCVertify();
			if (GetSelectedMusicAlbumJsonName() == "ALBUM7" && Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>() < 30)
			{
				Singleton<EventManager>.instance.Invoke("UI/PnlBudgetIsBurning");
				return;
			}
			if (GetSelectedMusicAlbumJsonName() == "ALBUM22" && Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>() < 15)
			{
				Singleton<EventManager>.instance.Invoke("UI/PnlBudgetIsBurningNanoCore");
				return;
			}
			bool flag = false;
			flag = true;
			string result = Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"].GetResult<string>();
			string albumUid = "music_package_" + result;
			if (result.StartsWith("music_package"))
			{
				albumUid = result;
			}
			RefreshBg(albumUid);
			bool flag2 = IsSelectedMusicPurchased();
			bool flag3 = Singleton<WeekFreeManager>.instance.freeSongUids.Contains(selectedMusicUid);
			if (IsCanPreparationOut() || flag3)
			{
				pnlPreparation.gameObject.SetActive(true);
				btnBackToStage.gameObject.SetActive(true);
				txtFreeSong.SetActive(false);
				GetComponent<Animator>().Play("StageToPreparation");
			}
			else if (flag)
			{
				if (!Singleton<DataManager>.instance["IAP"]["unlockall_0"].GetResult<bool>() && GetSelectedMusicAlbumJsonName() != "ALBUM1")
				{
					SingletonMonoBehaviour<SteamManager>.instance.DLCVertify();
					string result2 = Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"].GetResult<string>();
					string albumUid2 = "music_package_" + result2;
					if (result2.StartsWith("music_package"))
					{
						albumUid2 = result2;
					}
					RefreshBg(albumUid2);
					Singleton<EventManager>.instance.Invoke("UI/PnlNoPunchesAsk");
				}
				else
				{
					showText.Show(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "songLock"));
				}
			}
			else if (flag2)
			{
				showText.Show(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "songLock"));
			}
			else
			{
				Singleton<EventManager>.instance.Invoke("UI/OnUnlockAsk");
			}
		}

		public bool IsCanPreparationOut()
		{
			if (m_IsRandom && m_RandomMusicRangeInfoList.Count == 0)
			{
				return false;
			}
			bool flag = isCollection && selectedMusicIndexInAllList >= Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>().Count;
			bool flag2 = IsSelectedMusicPurchased();
			if (GetSelectedMusicAlbumJsonName() == "ALBUM7" && Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>() < 30)
			{
				return false;
			}
			if (GetSelectedMusicAlbumJsonName() == "ALBUM22" && Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>() < 15)
			{
				return false;
			}
			return IsSelectedMusicUnlock() && !flag && flag2;
		}

		private void OnClickCloseAlbumBtn()
		{
			AlbumCell component = albumFancyScrollView.GetCell(m_CurrentHighlighAlbumIndex).GetComponent<AlbumCell>();
			if (component == null)
			{
				return;
			}
			if (!component.isLock())
			{
				btnCloseAlbumSelect.onClick.Invoke();
				int result = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
				GetCurAlbumListStage(result, m_AllAlbumTagData[component.GetDataIndex()].list, selectedAlbumDifficulty, m_AllAlbumTagData[component.GetDataIndex()].nameList, m_AllAlbumTagData[component.GetDataIndex()].uid);
				if (component.GetDataIndex() == 2)
				{
					ChangeDefaultAlbum(m_MusicRangeInfoList, result, m_BudgeBruningV1, 30, isHide);
					ChangeDefaultAlbum(m_MusicRangeInfoList, result, m_BudgeBruningV2, 15, isHide);
				}
				if (m_MusicRangeInfoList.Count > 0)
				{
					IVariable varCurSelectedMusicIndex = m_VarCurSelectedMusicIndex;
					musicInfo musicInfo = m_MusicRangeInfoList[0];
					varCurSelectedMusicIndex.result = musicInfo.albumMusicIndex;
				}
				else
				{
					m_VarCurSelectedMusicIndex.result = 0;
				}
				albumFancyScrollView.ScrollToDataIndex(component.GetDataIndex(), 0f);
				m_VarCurSelectedAblumTagIndex.SetResult(m_AllAlbumTagData[component.GetDataIndex()].uid);
				selectedAlbumTagIndex = m_AllAlbumTagData[component.GetDataIndex()].uid;
				m_VarCurSelectedAlbumTagScrollIndex.SetResult(component.GetDataIndex());
				Singleton<DataManager>.instance.Save();
				PlayAlbumSwitchAnim(true);
				RefreshMusicFSV();
				if (!m_AllAlbumTagData[component.GetDataIndex()].isWeekFree)
				{
					return;
				}
				int num = Singleton<WeekFreeManager>.instance.freeSongUids.Length;
				for (int i = 0; i < num; i++)
				{
					string a = Singleton<WeekFreeManager>.instance.freeSongUids[i];
					int count = m_MusicRangeInfoList.Count;
					for (int j = 0; j < count; j++)
					{
						musicInfo musicInfo2 = m_MusicRangeInfoList[j];
						if (a == musicInfo2.uid)
						{
							musicFancyScrollView.ScrollToDataIndex(j, 0f);
							break;
						}
					}
				}
			}
			else if (component.uid == "collections")
			{
				Singleton<EventManager>.instance.Invoke("UI/OnCollectionsTip");
			}
			else if (component.uid == "hide")
			{
				Singleton<EventManager>.instance.Invoke("UI/OnCollectionsTip");
			}
			else
			{
				showText.Show(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "dlcInavaiable"));
			}
		}

		private void SwitchToAlbum1()
		{
			btnCloseAlbumSelect.onClick.Invoke();
			int result = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
			GetCurAlbumListStage(result, m_AllAlbumTagData[2].list, selectedAlbumDifficulty, m_AllAlbumTagData[2].nameList, m_AllAlbumTagData[2].uid);
			ChangeDefaultAlbum(m_MusicRangeInfoList, result, m_BudgeBruningV1, 30, isHide);
			ChangeDefaultAlbum(m_MusicRangeInfoList, result, m_BudgeBruningV2, 15, isHide);
			m_VarCurSelectedAlbumUid.result = "music_package_0";
			IVariable varCurSelectedMusicIndex = m_VarCurSelectedMusicIndex;
			musicInfo musicInfo = m_MusicRangeInfoList[0];
			varCurSelectedMusicIndex.result = musicInfo.albumMusicIndex;
			albumFancyScrollView.ScrollToDataIndex(2, 0f);
			m_VarCurSelectedAblumTagIndex.SetResult(m_AllAlbumTagData[2].uid);
			selectedAlbumTagIndex = m_AllAlbumTagData[2].uid;
			m_VarCurSelectedAlbumTagScrollIndex.SetResult(2);
			Singleton<DataManager>.instance.Save();
			PlayAlbumSwitchAnim(true);
			RefreshMusicFSV();
			if (!m_AllAlbumTagData[2].isWeekFree)
			{
				return;
			}
			int num = Singleton<WeekFreeManager>.instance.freeSongUids.Length;
			for (int i = 0; i < num; i++)
			{
				string a = Singleton<WeekFreeManager>.instance.freeSongUids[i];
				int count = m_MusicRangeInfoList.Count;
				for (int j = 0; j < count; j++)
				{
					musicInfo musicInfo2 = m_MusicRangeInfoList[j];
					if (a == musicInfo2.uid)
					{
						musicFancyScrollView.ScrollToDataIndex(j, 0f);
						break;
					}
				}
			}
		}

		private void OnClickCloseAlbumDifficultyBtn()
		{
			AlbumDifficultyCell component = albumDifficultyFancyScrollView.GetCell(m_CurrentHighlighAlbumDifficultyIndex).GetComponent<AlbumDifficultyCell>();
			if (!(component == null))
			{
				btnCloseAlbumDiffSelect.onClick.Invoke();
				int result = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
				GetCurAlbumListStage(result, m_AllAlbumTagData[selectedAlbumIndex_New].list, m_AllAlbumDifficultys[component.GetDataIndex()], m_AllAlbumTagData[selectedAlbumIndex_New].nameList, m_AllAlbumTagData[selectedAlbumIndex_New].uid);
				if (selectedAlbumIndex_New == 2)
				{
					ChangeDefaultAlbum(m_MusicRangeInfoList, result, m_BudgeBruningV1, 30, isHide);
					ChangeDefaultAlbum(m_MusicRangeInfoList, result, m_BudgeBruningV2, 15, isHide);
				}
				m_VarCurSelectedAlbumDifficulty.result = m_AllAlbumDifficultys[component.GetDataIndex()];
				albumDifficultyFancyScrollView.ScrollToDataIndex(component.GetDataIndex(), 0f);
				Singleton<DataManager>.instance.Save();
				PlayAlbumSwitchAnim(true);
				RefreshMusicFSV();
			}
		}

		private void OnClickBtnLike(bool like)
		{
			if (!m_IsClickHide && m_VarCurSelectedAblumTagIndex.GetResult<string>() != "hide")
			{
				imgLikeHide.gameObject.SetActive(false);
				if (like)
				{
					m_TglLikeScript.ResetHoldImg();
					if (!imgLikeOn.gameObject.activeInHierarchy)
					{
						imgLikeOn.gameObject.SetActive(true);
					}
					if (!m_Collection.Contains(selectedMusicUid))
					{
						m_ParTglLikes.CreateInstance();
						if (selectedMusicUid != string.Empty && !selectedMusicUid.StartsWith("A"))
						{
							m_Collection.Add(selectedMusicUid);
							showText.Show(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "collectionsSuccess"));
						}
						Singleton<AudioManager>.instance.PlayOneShot("sfx_count_finish", Singleton<DataManager>.instance["GameConfig"]["SfxVolume"].GetResult<float>());
						Singleton<DataManager>.instance.Save();
					}
				}
				else
				{
					if (!m_Collection.Contains(selectedMusicUid))
					{
						return;
					}
					musicInfo musicInfo = m_MusicRangeInfoList[selectedMusicIndexInAllList];
					if (!(musicInfo.uid != "?"))
					{
						return;
					}
					m_Collection.Remove(selectedMusicUid);
					Singleton<AudioManager>.instance.PlayOneShot("sfx_common_back", Singleton<DataManager>.instance["GameConfig"]["SfxVolume"].GetResult<float>());
					m_ParTglDisLikes.CreateInstance();
					if (isCollection)
					{
						if (m_Collection.Count == 0)
						{
							SwitchToAlbum1();
						}
						else
						{
							m_VarCurSelectedMusicIndexInAllList.SetResult((selectedMusicIndexInAllList > 1) ? (selectedMusicIndexInAllList - 1) : 0);
							PlayAlbumSwitchAnim(true);
							int result = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
							GetCurAlbumListStage(result, m_AllAlbumTagData[selectedAlbumIndex_New].list, selectedAlbumDifficulty, m_AllAlbumTagData[selectedAlbumIndex_New].nameList, m_AllAlbumTagData[selectedAlbumIndex_New].uid);
							RefreshMusicFSV();
						}
					}
					Singleton<DataManager>.instance.Save();
				}
			}
			else if (!like && m_Hide.Contains(selectedMusicUid))
			{
				Singleton<EventManager>.instance.Invoke("UI/OnAskHideCancel");
				tglLike.isOn = true;
			}
		}

		public void OnClickHide(object sender, object reciever, params object[] args)
		{
			m_IsClickHide = true;
		}

		public void OnCancelHideAsk()
		{
			m_IsClickHide = false;
		}

		public void GetALBUM1Index(string uid)
		{
			int count = m_MusicRangeInfoList.Count;
			for (int i = 0; i < count; i++)
			{
				musicInfo musicInfo = m_MusicRangeInfoList[i];
				if (uid == musicInfo.uid)
				{
					musicFancyScrollView.ScrollToDataIndex(i, 0.5f);
				}
			}
		}

		private void OnHideMusic(object sender, object reciever, params object[] args)
		{
			tglLike.isOn = false;
			musicInfo item = m_MusicRangeInfoList[selectedMusicIndexInAllList];
			if (item.uid != string.Empty && !item.uid.StartsWith("A"))
			{
				if (m_MusicRangeInfoList.Contains(item))
				{
					m_MusicRangeInfoList.Remove(item);
				}
				if (m_MusicRangeList.Contains(item.uid))
				{
					m_MusicRangeList.Remove(item.uid);
				}
				if (m_RandomMusicRangeInfoList.Contains(item))
				{
					m_RandomMusicRangeInfoList.Remove(item);
				}
				m_MusicRangeLevelList.RemoveAt(selectedMusicIndexInAllList);
				m_Hide.Add(item.uid);
				Singleton<HideManager>.instance.RefreshHideSongs();
				showText.Show(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "hideSuccess"));
				if (m_Collection.Contains(item.uid))
				{
					m_Collection.Remove(item.uid);
					if (m_Collection.Count == 0 && isCollection)
					{
						SwitchToAlbum1();
					}
				}
				Singleton<DataManager>.instance.Save();
				PlayAlbumSwitchAnim(true);
				if (m_MusicRangeInfoList.Count == 0)
				{
					goto IL_019d;
				}
				if (m_MusicRangeInfoList.Count == 1)
				{
					musicInfo musicInfo = m_MusicRangeInfoList[0];
					if (musicInfo.uid == "?")
					{
						goto IL_019d;
					}
				}
				goto IL_01b0;
			}
			goto IL_01b6;
			IL_01b6:
			m_IsClickHide = false;
			return;
			IL_01b0:
			RefreshMusicFSV();
			goto IL_01b6;
			IL_019d:
			m_AlbumTitleObj.SetActive(false);
			m_IsHideToNoMusic = true;
			goto IL_01b0;
		}

		private void OnRemoveHideMusic(object sender, object reciever, params object[] args)
		{
			musicInfo item = m_MusicRangeInfoList[selectedMusicIndexInAllList];
			if (item.uid != string.Empty && !item.uid.StartsWith("A"))
			{
				if (m_Hide.Contains(item.uid))
				{
					m_Hide.Remove(item.uid);
					Singleton<HideManager>.instance.RefreshHideSongs();
				}
				if (m_MusicRangeInfoList.Contains(item))
				{
					m_MusicRangeInfoList.Remove(item);
				}
				if (m_MusicRangeList.Contains(item.uid))
				{
					m_MusicRangeList.Remove(item.uid);
				}
				m_MusicRangeLevelList.RemoveAt(selectedMusicIndexInAllList);
				showText.Show(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "hideCancel"));
				if (m_Hide.Count == 0)
				{
					SwitchToAlbum1();
				}
				PlayAlbumSwitchAnim(true);
				RefreshMusicFSV();
				Singleton<DataManager>.instance.Save();
			}
		}

		public void SetHideBMSInfo(string str)
		{
			if (selectedMusicUid == "29-5" && !SpecialSongManager.isOnOuroboros)
			{
				Singleton<SpecialSongManager>.instance.hideBmsInfo += str;
				if (Singleton<SpecialSongManager>.instance.hideBmsInfo.Contains("321232123"))
				{
					Singleton<SpecialSongManager>.instance.SetHideBMSInfo(selectedMusicUid);
				}
			}
			else
			{
				Singleton<SpecialSongManager>.instance.hideBmsInfo = string.Empty;
			}
		}

		private void RefreshAlbumFSV()
		{
			if (!BtnIAP.IsUnlockAll())
			{
				string result = m_VarCurSelectedAlbumUid.GetResult<string>();
				if (result != "music_package_0" && result != "collections" && !Singleton<WeekFreeManager>.instance.freeAlbumUids.Contains(result) && !Singleton<DataManager>.instance["IAP"][m_VarCurSelectedAlbumUid.GetResult<string>()].GetResult<bool>())
				{
					m_VarCurSelectedAlbumUid.SetResult("music_package_0");
					m_VarCurSelectedMusicIndex.SetResult(0);
				}
			}
			GameObject[] array = new GameObject[m_AllAlbumTagData.Count];
			string[] freeAlbumUids = Singleton<WeekFreeManager>.instance.freeAlbumUids;
			for (int i = 0; i < m_AllAlbumTagData.Count; i++)
			{
				AlbumCell component = (array[i] = albumFancyScrollViewContent.GetChild(i).gameObject).GetComponent<AlbumCell>();
				int index = i;
				string uid = m_AllAlbumTagData[i].uid;
				component.SetUid(uid);
				if (uid != "collections")
				{
					bool albumNeedPurchase = GetAlbumNeedPurchase(i);
					if (uid.StartsWith("tag-new"))
					{
						bool @lock = albumNeedPurchase && !Singleton<DataManager>.instance["IAP"][m_AllAlbumTagData[i].nameList[0]].GetResult<bool>() && !BtnIAP.IsUnlockAll();
						if (freeAlbumUids.Contains(uid))
						{
							@lock = false;
						}
						component.SetLock(@lock);
					}
					component.SetWeekFree(m_AllAlbumTagData[i].isWeekFree);
				}
				else
				{
					component.SetLock(false);
					component.SetWeekFree(false);
				}
				component.btn.onClick.RemoveAllListeners();
				component.btn.onClick.AddListener(delegate
				{
					m_CurrentHighlighAlbumIndex = index;
					OnClickCloseAlbumBtn();
				});
			}
			m_AlbumFSVCells.Clear();
			m_AlbumFSVCells.AddRange(array);
			SetHalloweenDifficulty();
		}

		private void RefreshAlbumDifficultyFSV()
		{
		}

		private void RefreshMusicFSV()
		{
			bool flag = m_VarCurSelectedAblumTagIndex.GetResult<string>() == "hide";
			imgLikeHide.gameObject.SetActive(flag);
			m_TglLikeScript.m_IsInHideTag = flag;
			if (flag)
			{
				imgLikeHide.gameObject.SetActive(true);
				tglLike.isOn = flag;
			}
			else
			{
				imgLikeOn.gameObject.SetActive(isCollection);
				if (isCollection)
				{
					imgLikeHide.gameObject.SetActive(false);
					tglLike.isOn = true;
				}
			}
			string result = Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"].GetResult<string>();
			string text2;
			if (Singleton<WeekFreeManager>.instance.freeAlbumUids.Contains(result))
			{
				string text = Singleton<WeekFreeManager>.instance.discounts[result];
				if (!bgAlbumFree.activeInHierarchy)
				{
					bgAlbumFree.SetActive(true);
					bgAlbumFree.GetComponent<Animator>().Play("BgAlbumIn", 0);
				}
				if (bgAlbumLock.activeInHierarchy)
				{
					bgAlbumLock.SetActive(false);
				}
			}
			else
			{
				if (bgAlbumFree.activeInHierarchy)
				{
					bgAlbumFree.SetActive(false);
				}
				text2 = "music_package_" + result;
				if (result.StartsWith("music_package"))
				{
					text2 = result;
				}
				if (!CheckAlbumBuy(text2))
				{
					switch (text2)
					{
					case "music_package_0":
					case "music_package_6":
					case "music_package_21":
						break;
					default:
						goto IL_01c8;
					}
				}
				if (bgAlbumLock.activeInHierarchy)
				{
					bgAlbumLock.SetActive(false);
				}
			}
			goto IL_0214;
			IL_0214:
			imgFavoriteMusic.SetActive(isCollection);
			string albumJsonName = GetAlbumJsonName(selectedAlbumIndex);
			List<string> list = new List<string>();
			List<string> result2 = Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>();
			for (int i = 0; i < result2.Count; i++)
			{
				string text3 = result2[i];
				try
				{
					if (int.Parse(text3.BeginBefore('-')) <= GameInit.maxAlbumIndex)
					{
						list.Add(text3);
					}
				}
				catch (Exception)
				{
				}
			}
			int num = (!string.IsNullOrEmpty(albumJsonName)) ? Singleton<ConfigManager>.instance.GetJson(albumJsonName, false).Count : list.Count;
			string albumPrefabsName = GetAlbumPrefabsName(selectedAlbumIndex);
			GameObject gameObject = Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>("AlbumDiscoNew");
			musicFancyScrollView.startIndex.SetResult(selectedMusicIndexInAllList);
			musicFancyScrollView.cellCount.result = m_MusicRangeList.Count;
			gameObject.GetComponent<MusicStageCell>().m_MusicList = m_MusicRangeList.ToArray();
			gameObject.GetComponent<MusicStageCell>().m_MusicLevelList = m_MusicRangeLevelList.ToArray();
			musicFancyScrollView.Rebuild(gameObject);
			SetTitleOwnTextNew(Singleton<AlbumTagName>.instance.GetAlbumTagLocaliztion(m_VarCurSelectedAblumTagIndex.GetResult<string>()));
			if (m_MusicRangeInfoList.Count != 0)
			{
				if (m_MusicRangeInfoList.Count == 1)
				{
					musicInfo musicInfo = m_MusicRangeInfoList[0];
					if (musicInfo.uid == "?")
					{
						goto IL_03cf;
					}
				}
				m_TglLikeScript.m_IsNoMusic = false;
				return;
			}
			goto IL_03cf;
			IL_01c8:
			if (!bgAlbumLock.activeInHierarchy && text2 != "music_package_0" && text2 != "music_package_6" && text2 != "music_package_21")
			{
				bgAlbumLock.SetActive(true);
			}
			goto IL_0214;
			IL_03cf:
			SetPnlNoMusicEnable(true);
		}

		private void SetBgLockAction()
		{
			if (bgAlbumLock == null)
			{
				return;
			}
			string result = Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"].GetResult<string>();
			string selectedMusicAlbumJsonName = GetSelectedMusicAlbumJsonName();
			bool flag = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>() >= 15;
			bool flag2 = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>() >= 30;
			bool flag3;
			bool active;
			bool active2;
			if (selectedMusicAlbumJsonName == "ALBUM22" && result != "collections")
			{
				flag3 = (!flag && !BtnIAP.IsUnlockAll());
				active = flag3;
				active2 = false;
			}
			else if (selectedMusicAlbumJsonName == "ALBUM7" && result != "collections")
			{
				flag3 = (!flag2 && !BtnIAP.IsUnlockAll());
				active = flag3;
				active2 = false;
			}
			else if (result == "collections" || result == "music_package_0")
			{
				flag3 = false;
				active = false;
				active2 = false;
			}
			else
			{
				bool result2 = Singleton<DataManager>.instance["IAP"]["unlockall_0"].GetResult<bool>();
				flag3 = !result2;
				active2 = !result2;
				active = false;
			}
			if (Singleton<WeekFreeManager>.instance.freeAlbumUids != null && Singleton<WeekFreeManager>.instance.freeAlbumUids.Contains(result))
			{
				flag3 = false;
				active2 = false;
				active = false;
				bgAlbumFree.SetActive(true);
				bgAlbumFree.GetComponent<Animator>().Play("BgAlbumIn", 0);
			}
			else
			{
				bgAlbumFree.SetActive(false);
			}
			if (flag3)
			{
				bgAlbumLock.SetActive(true);
				bgAlbumLock.GetComponent<Animator>().Play("BgAlbumIn", 0);
				m_IsLock = false;
			}
			else if (!m_IsLock)
			{
				m_IsLock = true;
				if (bgAlbumLock.activeSelf)
				{
					bgAlbumLock.GetComponent<Animator>().Play("BgAlbumOut", 0);
				}
			}
			else
			{
				bgAlbumLock.SetActive(false);
			}
			txtNotPurchase.SetActive(active2);
			if (!flag2 && selectedMusicAlbumJsonName == "ALBUM7")
			{
				txtBudgetIsBurning15.SetActive(false);
				txtBudgetIsBurning30.SetActive(active);
			}
			else if (!flag && selectedMusicAlbumJsonName == "ALBUM22")
			{
				txtBudgetIsBurning15.SetActive(active);
				txtBudgetIsBurning30.SetActive(false);
			}
			else
			{
				txtBudgetIsBurning15.SetActive(false);
				txtBudgetIsBurning30.SetActive(false);
			}
		}

		private void PlayMusic()
		{
			if (!m_CanPlayMusic)
			{
				Singleton<AudioManager>.instance.StopBGM();
				return;
			}
			string selectedMusicAlbumJsonName = GetSelectedMusicAlbumJsonName();
			if (!string.IsNullOrEmpty(selectedMusicAlbumJsonName))
			{
				string text = isCollection ? Singleton<ConfigManager>.instance.GetConfigStringValue(selectedMusicAlbumJsonName, "uid", "demo", selectedMusicUid) : ((!(selectedMusicAlbumJsonName != "ALBUM1")) ? Singleton<ConfigManager>.instance.GetConfigStringValue(selectedMusicAlbumJsonName, "uid", "demo", selectedMusicUid) : Singleton<ConfigManager>.instance.GetConfigStringValue(selectedMusicAlbumJsonName, selectedMusicIndex, "demo"));
				if (selectedMusicUid.Contains("21-2") && SpecialSongManager.isOnNanoCoreAudio)
				{
					text += "2";
				}
				Singleton<AudioManager>.instance.PlayBGM(text, demoMusicFadeInTime);
			}
		}

		private void PlayRandomMusic()
		{
			int result = Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>();
			string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("character", result, "bgm");
			Singleton<AudioManager>.instance.PlayBGM(configStringValue, demoMusicFadeInTime);
		}

		private void PlayAlbumSwitchAnim(bool isFadeIn)
		{
			if (m_MusicRootAnimator != null)
			{
				if (isFadeIn)
				{
					m_MusicRootAnimator.Play(animNameAlbumIn);
				}
				else
				{
					m_MusicRootAnimator.Play(animNameAlbumOut);
				}
			}
		}

		private IEnumerator EnableAndPlayMusic1SecLater()
		{
			m_CanPlayMusic = false;
			yield return new WaitForSeconds(1f);
			m_CanPlayMusic = true;
			if (m_MusicRangeInfoList.Count != 0)
			{
				PlayCurrentSelectedMusicIfNeed();
			}
		}

		private bool IsSelectedMusicPurchased()
		{
			if (!IsSelectedAlbumsAvailiable())
			{
				return true;
			}
			string result = Singleton<DataManager>.instance["Account"]["SelectedMusicUid"].GetResult<string>();
			string a = $"music_package_{result.BeginBefore('-')}";
			if (a == "music_package_6" && Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>() >= 30)
			{
				return true;
			}
			if (a == "music_package_21" && Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>() >= 15)
			{
				return true;
			}
			if (a == "music_package_0")
			{
				return true;
			}
			return BtnIAP.IsUnlockAll();
		}

		private bool IsSelectedMusicUnlock()
		{
			if (isCollection)
			{
				return selectedMusicIndexInAllList < Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>().Count;
			}
			return Singleton<DataManager>.instance["Account"]["IsSelectedMusicUnlock"].GetResult<bool>();
		}

		private void DisableBtnPlayWhenDynamic(FancyScrollView.State state)
		{
			bool interactable = (state == FancyScrollView.State.Static) ? true : false;
			btnPlay.interactable = interactable;
		}

		private void ChangeFinalAlbum(int i)
		{
			AlbumCell component = albumFancyScrollView.GetCell(i).GetComponent<AlbumCell>();
			if (component != null)
			{
				m_CurrentHighlighAlbumIndex = i;
			}
		}

		private void ChangeFinalAlbumDifficulty(int i)
		{
			AlbumDifficultyCell component = albumDifficultyFancyScrollView.GetCell(i).GetComponent<AlbumDifficultyCell>();
			if (component != null)
			{
				m_CurrentHighlighAlbumDifficultyIndex = i;
			}
		}

		private void ChangeFinalMusic(int i)
		{
			PlayCurrentSelectedMusicIfNeed();
		}

		private bool IsSelectedAlbumsAvailiable()
		{
			string selectedMusicAlbumJsonName = GetSelectedMusicAlbumJsonName();
			return Singleton<ConfigManager>.instance[selectedMusicAlbumJsonName] != null;
		}

		private void ChangeMusic(int i)
		{
			if (m_MusicRangeInfoList.Count == 0)
			{
				return;
			}
			musicInfo musicInfo = m_MusicRangeInfoList[i];
			int albumMusicIndex = musicInfo.albumMusicIndex;
			Singleton<DataManager>.instance["Account"]["SelectedMusicIndexInAllList"].SetResult(i);
			musicInfo musicInfo2 = m_MusicRangeInfoList[i];
			string album = musicInfo2.album;
			musicInfo musicInfo3 = m_MusicRangeInfoList[i];
			int num = musicInfo3.getDifficulty;
			if (album != "?")
			{
				IVariable data = Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"];
				musicInfo musicInfo4 = m_MusicRangeInfoList[i];
				data.SetResult(musicInfo4.albumJsonIndex);
				IVariable data2 = Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"];
				musicInfo musicInfo5 = m_MusicRangeInfoList[i];
				data2.SetResult($"music_package_{musicInfo5.albumUid}");
				IVariable data3 = Singleton<DataManager>.instance["Account"]["SelectedMusicUidFromInfoList"];
				musicInfo musicInfo6 = m_MusicRangeInfoList[i];
				data3.SetResult(musicInfo6.uid);
				musicInfo musicInfo7 = m_MusicRangeInfoList[i];
				m_SelectedMusicUidFromInfoList = musicInfo7.uid;
			}
			string text = "?????";
			string text2 = "???";
			string text3 = "0";
			string text4 = "0";
			string text5 = "0";
			string text6 = string.Empty;
			List<string> result = Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>();
			if (album != "?")
			{
				m_TglLikeScript.m_IsInRandom = false;
				m_IsRandom = false;
				m_IsClickRandom = false;
				musicInfo musicInfo8 = m_MusicRangeInfoList[i];
				RefreshBg("music_package_" + musicInfo8.albumUid);
				if (!isCollection)
				{
					if (album == "ALBUM1")
					{
						musicInfo musicInfo9 = m_MusicRangeInfoList[i];
						text6 = musicInfo9.uid;
						text = Singleton<ConfigManager>.instance.GetConfigStringValue(album, "uid", "name", text6);
						text2 = Singleton<ConfigManager>.instance.GetConfigStringValue(album, "uid", "author", text6);
						text3 = Singleton<ConfigManager>.instance.GetConfigStringValue(album, "uid", "difficulty1", text6);
						text4 = Singleton<ConfigManager>.instance.GetConfigStringValue(album, "uid", "difficulty2", text6);
						text5 = Singleton<ConfigManager>.instance.GetConfigStringValue(album, "uid", "difficulty3", text6);
					}
					else
					{
						text = Singleton<ConfigManager>.instance.GetConfigStringValue(album, albumMusicIndex, "name");
						text2 = Singleton<ConfigManager>.instance.GetConfigStringValue(album, albumMusicIndex, "author");
						text3 = Singleton<ConfigManager>.instance.GetConfigStringValue(album, albumMusicIndex, "difficulty1");
						text4 = Singleton<ConfigManager>.instance.GetConfigStringValue(album, albumMusicIndex, "difficulty2");
						text5 = Singleton<ConfigManager>.instance.GetConfigStringValue(album, albumMusicIndex, "difficulty3");
						musicInfo musicInfo10 = m_MusicRangeInfoList[i];
						text6 = musicInfo10.uid;
					}
				}
				else if (i < result.Count)
				{
					musicInfo musicInfo11 = m_MusicRangeInfoList[i];
					text6 = musicInfo11.uid;
					if (IsSelectedAlbumsAvailiable())
					{
						text = Singleton<ConfigManager>.instance.GetConfigStringValue(album, "uid", "name", text6);
						text2 = Singleton<ConfigManager>.instance.GetConfigStringValue(album, "uid", "author", text6);
						text3 = Singleton<ConfigManager>.instance.GetConfigStringValue(album, "uid", "difficulty1", text6);
						text4 = Singleton<ConfigManager>.instance.GetConfigStringValue(album, "uid", "difficulty2", text6);
						text5 = Singleton<ConfigManager>.instance.GetConfigStringValue(album, "uid", "difficulty3", text6);
					}
				}
				m_AlbumTitleObj.SetActive(true);
				m_AlbumTitleTxt.text = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", "jsonName", "title", album);
				SetLikeAchiEmpty(false);
				imgLikeOn.gameObject.SetActive(isCollection || m_Collection.Contains(selectedMusicUid));
			}
			else
			{
				m_IsRandom = true;
				PlayRandomMusic();
				m_AlbumTitleObj.SetActive(false);
				bgAlbumLock.SetActive(false);
				bgAlbumFree.SetActive(false);
				txtFreeSong.SetActive(false);
				txtFreeSongNoPurchase.SetActive(false);
				txtBurnV1NotPurchase.SetActive(false);
				txtBurnV2NotPurchase.SetActive(false);
				text = Singleton<AlbumTagName>.instance.GetRandomNameLocaliztion();
				text2 = string.Empty;
				text3 = "?";
				text4 = "?";
				text5 = "?";
				SetLikeAchiEmpty(true);
			}
			musicNameTitle.text = text;
			artistNameTitle.text = text2;
			difficulty1.text = text3;
			difficulty2.text = text4;
			difficulty3.text = text5;
			if (album != "?")
			{
				bool flag = IsSelectedMusicPurchased();
				txtFreeSong.SetActive(Singleton<WeekFreeManager>.instance.freeSongUids.Contains(text6));
				txtFreeSongNoPurchase.SetActive(!Singleton<WeekFreeManager>.instance.freeSongUids.Contains(text6) && !flag);
				txtBurnV1NotPurchase.SetActive(selectedAlbumUid == "music_package_6" && !flag);
				txtBurnV2NotPurchase.SetActive(selectedAlbumUid == "music_package_21" && !flag);
			}
			emptyEasy.SetActive(difficulty1.text == "0" && IsSelectedMusicUnlock());
			emptyMaster.SetActive(difficulty3.text == "0");
			bool result2 = Singleton<DataManager>.instance["Account"]["IsSelectedUnlockMaster"].GetResult<bool>();
			difficulty3.transform.parent.gameObject.SetActive(result2);
			bool flag2 = difficulty3.text != "0";
			difficulty3Empty.SetActive(!flag2);
			difficulty3Lock.SetActive(!m_IsRandom && flag2 && !result2);
			difficulty1Easy.SetActive(difficulty1.text != "0");
			difficulty2Hard.SetActive(difficulty2.text != "0");
			difficulty3Master.SetActive(flag2 && !difficulty3Lock.activeSelf);
			bool flag3 = IsSelectedMusicUnlock();
			if (!isCollection)
			{
				tglLike.interactable = flag3;
			}
			bool flag4 = result.Contains(text6);
			bool flag5 = false;
			if (!flag4)
			{
				flag5 = m_Hide.Contains(text6);
			}
			if (!flag5 && !flag4)
			{
				tglLike.isOn = false;
			}
			if (flag4)
			{
				imgLikeHide.gameObject.SetActive(false);
				tglLike.isOn = true;
			}
			if (album == "?")
			{
				tglLike.interactable = false;
				m_TglLikeScript.m_IsInRandom = true;
			}
			else if (flag5)
			{
				imgLikeHide.gameObject.SetActive(true);
			}
			if (flag3)
			{
				KnowMusic();
			}
			else
			{
				UnknowMusic();
			}
			if (selectedAlbumDifficulty != 0)
			{
				if (num == 3 && !result2)
				{
					num = 2;
				}
				Singleton<DataManager>.instance["Account"]["SelectedDifficulty"].SetResult(num);
			}
		}

		private void RefreshBg(string albumUid)
		{
			if (Singleton<WeekFreeManager>.instance.freeAlbumUids.Contains(albumUid) && !CheckAlbumBuy(albumUid))
			{
				if (!bgAlbumFree.activeInHierarchy)
				{
					bgAlbumFree.SetActive(true);
					bgAlbumFree.GetComponent<Animator>().Play("BgAlbumIn", 0);
					if (bgAlbumLock.activeInHierarchy)
					{
						bgAlbumLock.SetActive(false);
					}
				}
				return;
			}
			if (bgAlbumFree.activeInHierarchy)
			{
				bgAlbumFree.SetActive(false);
			}
			if (!CheckAlbumBuy(albumUid))
			{
				switch (albumUid)
				{
				case "music_package_0":
				case "music_package_6":
				case "music_package_21":
					break;
				default:
					if (!bgAlbumLock.activeInHierarchy && albumUid != "music_package_0" && albumUid != "music_package_6" && albumUid != "music_package_21")
					{
						bgAlbumLock.SetActive(true);
					}
					return;
				}
			}
			if (bgAlbumLock.activeInHierarchy)
			{
				bgAlbumLock.SetActive(false);
			}
		}

		private void UnknowMusic()
		{
			musicNameTitle.text = "?????";
			artistNameTitle.text = "???";
			difficulty1.gameObject.SetActive(false);
			difficulty2.gameObject.SetActive(false);
			difficulty3.gameObject.SetActive(false);
			btnAhcievement.interactable = false;
			difficult1Lock.SetActive(true);
			difficult2Lock.SetActive(true);
			difficulty3Lock.SetActive(true);
			difficulty3Empty.SetActive(false);
			m_TglLikeScript.isUnKnow = true;
		}

		private void KnowMusic()
		{
			difficulty1.gameObject.SetActive(true);
			difficulty2.gameObject.SetActive(true);
			difficulty3.gameObject.SetActive(true);
			btnAhcievement.interactable = true;
			difficult1Lock.SetActive(false);
			difficult2Lock.SetActive(false);
			m_TglLikeScript.isUnKnow = false;
		}

		private void ReplayMusicWhenFormPnlMenuToPnlStage()
		{
			PlayMusic();
		}

		private void ChangeFinalAblumNs(int i)
		{
			if (i == 0)
			{
				List<string> result = Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>();
				SetPnlNoCollectionEnable(result.Count == 0);
			}
			else
			{
				SetPnlNoCollectionEnable(false);
			}
			SwitchToAlbum(i);
		}

		private void SetPnlNoCollectionEnable(bool enable)
		{
			nsNoCollectionTip.SetActive(enable);
			tglLike.GetComponent<InputKeyBinding>().enabled = !enable;
			tglLike.graphic.gameObject.SetActive(!enable);
			tglLike.enabled = !enable;
			btnPlay.gameObject.SetActive(!enable);
			musicFancyScrollView.btnPrevious.gameObject.SetActive(!enable);
			musicFancyScrollView.btnNext.gameObject.SetActive(!enable);
			musicFancyScrollView.processBar.gameObject.SetActive(!enable);
			musicFancyScrollView.gameObject.SetActive(!enable);
			if (enable)
			{
				UnknowMusic();
				stageAchievementPercent.SetActive(false);
				m_TglLikeScript.m_IsNoMusic = enable;
				if (enable)
				{
					PlayRandomMusic();
				}
				else
				{
					PlayMusic();
				}
			}
		}

		private void SetPnlNoMusicEnable(bool enable)
		{
			if (m_IsHideToNoMusic)
			{
				noMusicFromHide.SetActive(enable);
				noMusicFromDiff.SetActive(!enable);
				if (bgAlbumLock.activeInHierarchy)
				{
					bgAlbumLock.SetActive(false);
				}
				albumInfo albumInfo = m_AllAlbumTagData[selectedAlbumIndex_New];
				if (!AlbumHadBuy(albumInfo.nameList, albumInfo.uid))
				{
					bgAlbumLock.SetActive(true);
				}
				else if (bgAlbumLock.activeInHierarchy)
				{
					bgAlbumLock.SetActive(false);
				}
			}
			else if (m_IsDiffToNoMusic)
			{
				noMusicFromDiff.SetActive(enable);
				noMusicFromHide.SetActive(!enable);
				albumInfo albumInfo2 = m_AllAlbumTagData[selectedAlbumIndex_New];
				if (!AlbumHadBuy(albumInfo2.nameList, albumInfo2.uid))
				{
					bgAlbumLock.SetActive(true);
				}
				else if (bgAlbumLock.activeInHierarchy)
				{
					bgAlbumLock.SetActive(false);
				}
			}
			else
			{
				noMusicFromHide.SetActive(enable);
				noMusicFromDiff.SetActive(enable);
			}
			SetLikeAchiEmpty(enable);
			tglLike.GetComponent<InputKeyBinding>().enabled = !enable;
			tglLike.interactable = !enable;
			btnPlay.gameObject.SetActive(!enable);
			musicFancyScrollView.btnPrevious.gameObject.SetActive(!enable);
			musicFancyScrollView.btnNext.gameObject.SetActive(!enable);
			musicFancyScrollView.processBar.gameObject.SetActive(!enable);
			musicFancyScrollView.gameObject.SetActive(!enable);
			if (!enable)
			{
				return;
			}
			UnknowMusic();
			stageAchievementPercent.SetActive(false);
			m_TglLikeScript.m_IsNoMusic = enable;
			if (enable)
			{
				if (base.gameObject.activeInHierarchy)
				{
					PlayRandomMusic();
				}
			}
			else
			{
				PlayMusic();
			}
		}

		private new void Update()
		{
			base.Update();
			Singleton<SpecialSongManager>.instance.Update();
		}

		private void SetAprilFoolsDayDifficulty()
		{
			if (DateTime.Now.Month == 4 && DateTime.Now.Day == 1)
			{
				Singleton<SpecialSongManager>.instance.SetHideBMSInfo("4-5");
			}
		}

		private void NacoLogicListener(bool isOn)
		{
			if (SpecialSongManager.isOnNanoCoreAudio || !selectedMusicUid.Contains("21-2"))
			{
				return;
			}
			if (isOn)
			{
				if (!Singleton<SpecialSongManager>.instance.isInitNanoCoreCount)
				{
					Singleton<SpecialSongManager>.instance.isInitNanoCoreCount = true;
					return;
				}
				Singleton<SpecialSongManager>.instance.trggleNanoCoreCount++;
			}
			else
			{
				if (Singleton<SpecialSongManager>.instance.trggleNanoCoreCount == 0)
				{
					return;
				}
				Singleton<SpecialSongManager>.instance.trggleNanoCoreCount = 1;
				Singleton<SpecialSongManager>.instance.isInitNanoCoreCount = false;
			}
			if (Singleton<SpecialSongManager>.instance.trggleNanoCoreCount >= 10)
			{
				Singleton<SpecialSongManager>.instance.SetHideBMSInfo(selectedMusicUid);
				PlayMusic();
			}
		}

		private void HideBMSLogicListener(bool isOn)
		{
			if (Singleton<SpecialSongManager>.instance.IsHideBMS(selectedMusicUid))
			{
				return;
			}
			if (isOn)
			{
				if (!Singleton<SpecialSongManager>.instance.isHideBmsCount)
				{
					Singleton<SpecialSongManager>.instance.isHideBmsCount = true;
					return;
				}
				Singleton<SpecialSongManager>.instance.toggleHideBmsCount++;
			}
			else
			{
				if (Singleton<SpecialSongManager>.instance.toggleHideBmsCount == 0)
				{
					return;
				}
				Singleton<SpecialSongManager>.instance.toggleHideBmsCount = 1;
				Singleton<SpecialSongManager>.instance.isHideBmsCount = false;
			}
			if (Singleton<SpecialSongManager>.instance.toggleHideBmsCount >= 10)
			{
				Singleton<SpecialSongManager>.instance.SetHideBMSInfo(selectedMusicUid);
			}
		}

		private void RefPnlPreparation()
		{
			for (int i = 0; i < pnlPreparationObjs.Length; i++)
			{
				pnlPreparationObjs[i].SetActive(false);
				pnlPreparationObjs[i].SetActive(true);
			}
		}

		private void OnMusicCellChange(float i)
		{
			if (m_MusicCellSelected != null)
			{
				m_MusicCellSelected(i);
			}
		}

		private int GetAlbumTagListIndex(string uid)
		{
			for (int i = 0; i < m_AllAlbumTagData.Count; i++)
			{
				if (m_AllAlbumTagData[i].uid == uid)
				{
					return i;
				}
			}
			return 1;
		}

		private int GetAlbumDifficultListIndex(int difficulty)
		{
			for (int i = 0; i < m_AllAlbumDifficultys.Length; i++)
			{
				if (m_AllAlbumDifficultys[i] == difficulty)
				{
					return i;
				}
			}
			return 0;
		}

		private string GetAlbumTagListName(string uid)
		{
			for (int i = 0; i < m_AllAlbumTagData.Count; i++)
			{
				if (m_AllAlbumTagData[i].uid == uid)
				{
					return m_AllAlbumTagData[i].name;
				}
			}
			return m_AllAlbumTagData[0].name;
		}

		private void RangeStageList()
		{
			musicFancyScrollView.onUpdatePosition += OnMusicCellChange;
			JArray json = Singleton<ConfigManager>.instance.GetJson("albums", false);
			for (int i = 1; i < m_AlbumDatas.Length; i++)
			{
				string text = (string)json[(i != 1) ? i : 0]["title"];
				string text2 = (string)m_AlbumDatas[i]["jsonName"]();
				string item = (string)m_AlbumDatas[i]["uid"]();
				if (text == "Just as Planned")
				{
					continue;
				}
				if (text.StartsWith("Give Up"))
				{
					m_AllGiveUpAlbumUid.Add(text2);
					m_AllGiveUpAlbumName_Re.Add(item);
					m_AllGiveUpAlbumUid_Re.Add(text2);
					m_AllGiveUpAlbumName.Add(item);
				}
				else if (text.StartsWith("Happy"))
				{
					m_AllHappyAlbumUid.Add(text2);
					m_AllHappyAlbumName_Re.Add(item);
					m_AllHappyAlbumUid_Re.Add(text2);
					m_AllHappyAlbumName.Add(item);
				}
				else if (text.StartsWith("Cute"))
				{
					m_AllCuteAlbumUid.Add(text2);
					m_AllCuteAlbumName_Re.Add(item);
					m_AllCuteAlbumUid_Re.Add(text2);
					m_AllCuteAlbumName.Add(item);
				}
				else if (text == "Default Music")
				{
					m_AllBaseAlbumUid.Add(text2);
				}
				else if (text.StartsWith("Budget Is Burning"))
				{
					m_AllBurningAlbumUid.Add(text2);
					m_AllBurningAlbumName_Re.Add(item);
					m_AllBurningAlbumUid_Re.Add(text2);
					m_AllBurningAlbumName.Add(item);
					if (text.EndsWith("1"))
					{
						m_BudgeBruningV1 = text2;
					}
					if (text.EndsWith("Nano Core"))
					{
						m_BudgeBruningV2 = text2;
					}
				}
				else if (text == "MUSE RADIO")
				{
					m_AllMuseRadioAlbumUid.Add(text2);
					m_AllMuseRadioAlbumName_Re.Add(item);
					m_AllMuseRadioAlbumUid_Re.Add(text2);
					m_AllMuseRadioAlbumName.Add(item);
				}
				else
				{
					m_AllOtherAlbumUid.Add(text2);
					m_AllOtherAlbumName_Re.Add(item);
					m_AllOtherAlbumUid_Re.Add(text2);
					m_AllOtherAlbumName.Add(item);
				}
			}
			List<string> allBurningAlbumUid_Re = m_AllBurningAlbumUid_Re;
			allBurningAlbumUid_Re.Reverse();
			List<string> allBurningAlbumName_Re = m_AllBurningAlbumName_Re;
			allBurningAlbumName_Re.Reverse();
			List<string> allCuteAlbumUid_Re = m_AllCuteAlbumUid_Re;
			allCuteAlbumUid_Re.Reverse();
			List<string> allCuteAlbumName_Re = m_AllCuteAlbumName_Re;
			allCuteAlbumName_Re.Reverse();
			List<string> allHappyAlbumUid_Re = m_AllHappyAlbumUid_Re;
			allHappyAlbumUid_Re.Reverse();
			List<string> allHappyAlbumName_Re = m_AllHappyAlbumName_Re;
			allHappyAlbumName_Re.Reverse();
			List<string> allGiveUpAlbumUid_Re = m_AllGiveUpAlbumUid_Re;
			allGiveUpAlbumUid_Re.Reverse();
			List<string> allGiveUpAlbumName_Re = m_AllGiveUpAlbumName_Re;
			allGiveUpAlbumName_Re.Reverse();
			List<string> allOtherAlbumUid_Re = m_AllOtherAlbumUid_Re;
			allOtherAlbumUid_Re.Reverse();
			List<string> allOtherAlbumName_Re = m_AllOtherAlbumName_Re;
			allOtherAlbumName_Re.Reverse();
			List<string> allMuseRadioAlbumUid_Re = m_AllMuseRadioAlbumUid_Re;
			allMuseRadioAlbumUid_Re.Reverse();
			List<string> allMuseRadioAlbumName_Re = m_AllMuseRadioAlbumName_Re;
			allMuseRadioAlbumName_Re.Reverse();
			m_AllAlbumUid.Add(m_AllBaseAlbumUid[0]);
			m_AllAlbumName.Add("music_package_0");
			for (int j = 0; j < allBurningAlbumUid_Re.Count; j++)
			{
				m_AllAlbumUid.Add(allBurningAlbumUid_Re[j]);
				m_AllAlbumName.Add(allBurningAlbumName_Re[j]);
			}
			for (int k = 0; k < allCuteAlbumUid_Re.Count; k++)
			{
				m_AllAlbumUid.Add(allCuteAlbumUid_Re[k]);
				m_AllAlbumName.Add(allCuteAlbumName_Re[k]);
			}
			for (int l = 0; l < allHappyAlbumUid_Re.Count; l++)
			{
				m_AllAlbumUid.Add(allHappyAlbumUid_Re[l]);
				m_AllAlbumName.Add(allHappyAlbumName_Re[l]);
			}
			for (int m = 0; m < allGiveUpAlbumUid_Re.Count; m++)
			{
				m_AllAlbumUid.Add(allGiveUpAlbumUid_Re[m]);
				m_AllAlbumName.Add(allGiveUpAlbumName_Re[m]);
			}
			for (int n = 0; n < allMuseRadioAlbumUid_Re.Count; n++)
			{
				m_AllAlbumUid.Add(allMuseRadioAlbumUid_Re[n]);
				m_AllAlbumName.Add(allMuseRadioAlbumName_Re[n]);
			}
			for (int num = 0; num < allOtherAlbumUid_Re.Count; num++)
			{
				m_AllAlbumUid.Add(allOtherAlbumUid_Re[num]);
				m_AllAlbumName.Add(allOtherAlbumName_Re[num]);
			}
			m_AllAlbumTagData[0].list = m_Collection;
			m_AllAlbumTagData[1].list = m_AllAlbumUid;
			m_AllAlbumTagData[1].nameList = m_AllAlbumName;
			m_AllAlbumTagData[2].list = m_AllBaseAlbumUid;
			m_AllAlbumTagData[2].nameList.Add("music_package_0");
			m_AllAlbumTagData[3].list = m_AllGiveUpAlbumUid;
			m_AllAlbumTagData[3].nameList = m_AllGiveUpAlbumName;
			m_AllAlbumTagData[4].list = m_AllHappyAlbumUid;
			m_AllAlbumTagData[4].nameList = m_AllHappyAlbumName;
			m_AllAlbumTagData[5].list = m_AllCuteAlbumUid;
			m_AllAlbumTagData[5].nameList = m_AllCuteAlbumName;
			m_AllAlbumTagData[6].list = m_AllMuseRadioAlbumUid;
			m_AllAlbumTagData[6].nameList = m_AllMuseRadioAlbumName;
			m_AllAlbumTagData[7].list = m_AllOtherAlbumUid;
			m_AllAlbumTagData[7].nameList = m_AllOtherAlbumName;
			m_AllAlbumTagData[8].list = m_Hide;
		}

		private void InitMusicInfo(string albumName, string albumUid)
		{
			List<musicInfoFromAlbum> list = new List<musicInfoFromAlbum>();
			JArray json = Singleton<ConfigManager>.instance.GetJson(albumName, false);
			int count = json.Count;
			musicInfoFromAlbum musicInfoFromAlbum = default(musicInfoFromAlbum);
			musicInfoFromAlbum.uid = string.Empty;
			musicInfoFromAlbum.albumName = string.Empty;
			musicInfoFromAlbum.albumUid = string.Empty;
			musicInfoFromAlbum.level = 0;
			musicInfoFromAlbum.difficulty1 = 0;
			musicInfoFromAlbum.difficulty2 = 0;
			musicInfoFromAlbum.difficulty3 = 0;
			musicInfoFromAlbum item = musicInfoFromAlbum;
			for (int i = 0; i < count; i++)
			{
				JToken jToken = json[i];
				item.uid = (string)jToken["uid"];
				item.albumName = albumName;
				item.albumUid = albumUid;
				item.level = (int)jToken["unlockLevel"];
				item.difficulty1 = int.Parse((string)jToken["difficulty1"]);
				item.difficulty2 = int.Parse((string)jToken["difficulty2"]);
				item.difficulty3 = int.Parse((string)jToken["difficulty3"]);
				list.Add(item);
			}
			m_AllJson.Add(albumName, list.ToArray());
		}

		private void SetWeekFreeAlbum()
		{
			if (!m_IsInitedNewAlbum)
			{
				InitNewAlbum();
				m_IsInitedNewAlbum = true;
			}
			if (Singleton<WeekFreeManager>.instance.freeAlbumUids != null)
			{
				m_CurWeekFreeIndexs.Clear();
				m_NewAlbumTagData.Clear();
				m_AddDataIndexs.Clear();
				int num = Singleton<WeekFreeManager>.instance.freeAlbumUids.Length;
				int count = newAlbumUids.Count;
				for (int i = 0; i < num; i++)
				{
					string item = Singleton<WeekFreeManager>.instance.freeAlbumUids[i];
					int num2 = -1;
					if (m_AllGiveUpAlbumName.Contains(item))
					{
						num2 = FindAlbumTagIndex("tag-3");
					}
					else if (m_AllHappyAlbumName.Contains(item))
					{
						num2 = FindAlbumTagIndex("tag-4");
					}
					else if (m_AllCuteAlbumName.Contains(item))
					{
						num2 = FindAlbumTagIndex("tag-5");
					}
					else if (m_AllMuseRadioAlbumName.Contains(item))
					{
						num2 = FindAlbumTagIndex("tag-6");
					}
					else if (m_AllOtherAlbumName.Contains(item))
					{
						num2 = FindAlbumTagIndex("tag-7");
					}
					if (num2 != -1 && !m_CurWeekFreeIndexs.Contains(num2))
					{
						m_CurWeekFreeIndexs.Add(num2);
					}
				}
				if (m_CurWeekFreeIndexs.Count > 0)
				{
					for (int j = 0; j < m_AllAlbumTagData.Count; j++)
					{
						if (j >= 3 + count)
						{
							continue;
						}
						if (j > 1 && m_AllAlbumTagData[j].nameList.Count > 0)
						{
							for (int k = 0; k < num; k++)
							{
								string item2 = Singleton<WeekFreeManager>.instance.freeAlbumUids[k];
								if (m_AllAlbumTagData[j].nameList.Contains(item2))
								{
									m_AllAlbumTagData[j].isWeekFree = true;
								}
							}
						}
						m_NewAlbumTagData.Add(m_AllAlbumTagData[j]);
						m_AddDataIndexs.Add(j);
					}
					int count2 = m_CurWeekFreeIndexs.Count;
					for (int l = 0; l < count2; l++)
					{
						int num3 = m_CurWeekFreeIndexs[l];
						if (num3 >= m_AllAlbumTagData.Count)
						{
							continue;
						}
						albumInfo albumInfo = m_AllAlbumTagData[num3];
						m_AllAlbumTagData[num3].isWeekFree = true;
						m_NewAlbumTagData.Add(m_AllAlbumTagData[num3]);
						m_AddDataIndexs.Add(num3);
						m_AlbumFSVCells[num3].transform.SetSiblingIndex(3 + count + l);
						for (int m = 0; m < num; m++)
						{
							string text = Singleton<WeekFreeManager>.instance.freeAlbumUids[m];
							if (CheckAlbumBuy(text))
							{
								continue;
							}
							string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", "uid", "jsonName", text);
							if (m_AllAlbumTagData[num3].list.Count > 1 && m_AllAlbumTagData[num3].nameList.Count > 1 && m_AllAlbumTagData[num3].nameList.Contains(text) && m_AllAlbumTagData[num3].list.Contains(configStringValue))
							{
								m_AllAlbumTagData[num3].list.Remove(configStringValue);
								m_AllAlbumTagData[num3].list.Insert(0, configStringValue);
								m_AllAlbumTagData[num3].nameList.Remove(text);
								m_AllAlbumTagData[num3].nameList.Insert(0, text);
							}
							if (m_AllAlbumTagData[1].list.Count > 1 && m_AllAlbumTagData[1].nameList.Count > 1)
							{
								if (m_AllAlbumTagData[1].list.Contains(configStringValue))
								{
									m_AllAlbumTagData[1].list.Remove(configStringValue);
									m_AllAlbumTagData[1].list.Insert(0, configStringValue);
								}
								if (m_AllAlbumTagData[1].nameList.Contains(text))
								{
									m_AllAlbumTagData[1].nameList.Remove(text);
									m_AllAlbumTagData[1].nameList.Insert(0, text);
								}
							}
						}
					}
					for (int n = 0; n < m_AllAlbumTagData.Count; n++)
					{
						if (!m_AddDataIndexs.Contains(n))
						{
							m_NewAlbumTagData.Add(m_AllAlbumTagData[n]);
						}
					}
					m_AllAlbumTagData.Clear();
					for (int num4 = 0; num4 < m_NewAlbumTagData.Count; num4++)
					{
						m_AllAlbumTagData.Add(m_NewAlbumTagData[num4]);
					}
				}
			}
			albumFancyScrollView.startIndex.result = selectedAlbumIndex_New;
			albumFancyScrollView.Rebuild();
			int result = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
			GetCurAlbumListStage(result, m_AllAlbumTagData[selectedAlbumIndex_New].list, selectedAlbumDifficulty, m_AllAlbumTagData[selectedAlbumIndex_New].nameList, m_AllAlbumTagData[selectedAlbumIndex_New].uid);
			if (selectedAlbumIndex_New == 2)
			{
				ChangeDefaultAlbum(m_MusicRangeInfoList, result, m_BudgeBruningV1, 30, isHide);
				ChangeDefaultAlbum(m_MusicRangeInfoList, result, m_BudgeBruningV2, 15, isHide);
			}
		}

		private void InitNewAlbum()
		{
			int count = newAlbumUids.Count;
			for (int i = 0; i < count; i++)
			{
				albumInfo albumInfo = new albumInfo();
				albumInfo.uid = string.Empty;
				albumInfo.name = string.Empty;
				albumInfo.list = new List<string>();
				albumInfo.nameList = new List<string>();
				albumInfo.isWeekFree = false;
				albumInfo.isNew = false;
				albumInfo albumInfo2 = albumInfo;
				string text = newAlbumUids[i];
				string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", "uid", "jsonName", text);
				albumInfo2.uid = "tag-new" + i;
				albumInfo2.name = Singleton<ConfigManager>.instance.GetConfigStringValue("albums", "uid", "title", text);
				albumInfo2.list.Add(configStringValue);
				albumInfo2.nameList.Add(text);
				albumInfo2.isWeekFree = Singleton<WeekFreeManager>.instance.freeAlbumUids.Contains(text);
				albumInfo2.isNew = true;
				m_AllAlbumTagData.Insert(3 + i, albumInfo2);
			}
		}

		private void InitAlbumTagCell()
		{
			m_AlbumFSVCells.Clear();
			for (int i = 0; i < albumFancyScrollViewContent.childCount; i++)
			{
				m_AlbumFSVCells.Add(albumFancyScrollViewContent.GetChild(i).gameObject);
			}
		}

		private int FindAlbumTagIndex(string albumUid)
		{
			for (int i = 0; i < m_AllAlbumTagData.Count; i++)
			{
				if (m_AllAlbumTagData[i].uid == albumUid)
				{
					return i;
				}
			}
			return 0;
		}

		private void GetCurAlbumListStage(int level, List<string> albumList, int curDifficutyTag, List<string> albumNameList, string uid)
		{
			m_CurDifficutyTag = curDifficutyTag;
			m_MusicRangeList.Clear();
			m_MusicRangeInfoList.Clear();
			m_MusicRangeLevelList.Clear();
			m_RandomMusicRangeInfoList.Clear();
			m_IsHideToNoMusic = true;
			m_IsDiffToNoMusic = true;
			int count = albumList.Count;
			for (int i = 0; i < count; i++)
			{
				string text = albumList[i];
				if (uid != "collections" && uid != "hide")
				{
					string albumUid = albumNameList[i];
					JArray json = Singleton<ConfigManager>.instance.GetJson(text, false);
					if (json == null)
					{
						continue;
					}
					if (uid != "collections" && uid != "tag-2")
					{
						if (CheckAlbumBuy(albumUid) || !AlbumHadBuy(albumNameList, uid) || CheckAlbumIsWeekFree(albumUid))
						{
							for (int j = 0; j < json.Count; j++)
							{
								GetMusicInfo(j, json[j], text, level);
							}
						}
					}
					else
					{
						for (int k = 0; k < json.Count; k++)
						{
							GetMusicInfo(k, json[k], text, level);
						}
					}
				}
				else
				{
					if (text.StartsWith("A"))
					{
						continue;
					}
					int num = int.Parse(text.BeginBefore('-')) + 1;
					int num2 = int.Parse(text.LastAfter('-'));
					JArray json2 = Singleton<ConfigManager>.instance.GetJson("ALBUM" + num, false);
					int count2 = json2.Count;
					for (int l = 0; l < count2; l++)
					{
						JToken jToken = json2[l];
						if (text == (string)jToken["uid"])
						{
							GetMusicInfo(l, jToken, "ALBUM" + num, level, uid == "hide");
						}
					}
				}
			}
			if (uid == "tag-1" || AlbumHadBuy(albumNameList, uid) || CheckWeekFreeAlbumList(albumNameList))
			{
				m_MusicRangeList.Add("?");
				m_MusicRangeLevelList.Add(0);
				m_MusicRangeInfoList.Add(new musicInfo
				{
					uid = "?",
					album = "?",
					albumUid = "?",
					albumMusicIndex = 0,
					level = 0,
					isUnlock = true,
					albumJsonIndex = 0,
					getDifficulty = 1
				});
			}
			if (m_MusicRangeInfoList.Count == 1)
			{
				musicInfo musicInfo = m_MusicRangeInfoList[0];
				if (musicInfo.uid == "?")
				{
					SetPnlNoMusicEnable(true);
					m_AlbumTitleObj.SetActive(false);
				}
				else
				{
					SetPnlNoMusicEnable(false);
				}
			}
			else if (m_MusicRangeInfoList.Count == 0)
			{
				SetPnlNoMusicEnable(true);
				m_AlbumTitleObj.SetActive(false);
			}
			else
			{
				SetPnlNoMusicEnable(false);
			}
			SetDifficultyTagTxt(curDifficutyTag);
		}

		public bool CheckWeekFreeAlbumList(List<string> albumList)
		{
			int count = albumList.Count;
			for (int i = 0; i < count; i++)
			{
				if (CheckAlbumIsWeekFree(albumList[i]))
				{
					return true;
				}
			}
			return false;
		}

		public void OnOutRefreshMusicList()
		{
			int num = (int)m_VarCurSelectedAlbumTagScrollIndex.result;
			albumInfo albumInfo = m_AllAlbumTagData[num];
			int result = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
			GetCurAlbumListStage(result, albumInfo.list, selectedAlbumDifficulty, albumInfo.nameList, albumInfo.uid);
			if (num == 2)
			{
				ChangeDefaultAlbum(m_MusicRangeInfoList, result, m_BudgeBruningV1, 30, isHide);
				ChangeDefaultAlbum(m_MusicRangeInfoList, result, m_BudgeBruningV2, 15, isHide);
			}
			RefreshAlbumFSV();
			txtBurnV1NotPurchase.SetActive(selectedAlbumUid == "music_package_6" && !IsSelectedMusicPurchased());
			txtBurnV2NotPurchase.SetActive(selectedAlbumUid == "music_package_21" && !IsSelectedMusicPurchased());
		}

		private void GetMusicInfo(int jsonIndex, JToken albumJson, string album, int level, bool isHide = false)
		{
			string text = (string)albumJson["uid"];
			if (CheckIsSpecialMusic(text) || (Singleton<HideManager>.instance.hideSongUids.Contains(text) && !isHide))
			{
				return;
			}
			m_IsHideToNoMusic = false;
			string item = (string)albumJson["name"];
			int num = (int)albumJson["unlockLevel"];
			musicInfo musicInfo = default(musicInfo);
			musicInfo.uid = string.Empty;
			musicInfo.album = string.Empty;
			musicInfo.albumUid = string.Empty;
			musicInfo.albumMusicIndex = -1;
			musicInfo.level = 0;
			musicInfo.isUnlock = false;
			musicInfo.albumJsonIndex = 0;
			musicInfo.getDifficulty = 1;
			musicInfo item2 = musicInfo;
			if (m_CurDifficutyTag != m_AllDifficulty)
			{
				if (!GetCurDifficulty(albumJson))
				{
					return;
				}
				m_IsDiffToNoMusic = false;
				if (!m_MusicRangeList.Contains(text))
				{
					m_MusicRangeList.Add(text);
					m_MusicNameList.Add(item);
					m_MusicRangeLevelList.Add(num);
					item2.uid = text;
					item2.album = album;
					item2.albumUid = item2.uid.BeginBefore('-');
					string s = item2.uid.LastAfter('-');
					item2.albumMusicIndex = int.Parse(s);
					item2.level = num;
					item2.isUnlock = CheckAlbumBuy("music_package_" + item2.albumUid);
					item2.albumJsonIndex = jsonIndex;
					item2.getDifficulty = m_CurDiffIndex;
					if ((item2.isUnlock || CheckMusicIsWeekFree(item2.uid)) && level >= item2.level)
					{
						m_RandomMusicRangeInfoList.Add(item2);
					}
					m_MusicRangeInfoList.Add(item2);
				}
			}
			else
			{
				m_IsDiffToNoMusic = false;
				m_MusicRangeList.Add(text);
				m_MusicNameList.Add(item);
				m_MusicRangeLevelList.Add(num);
				item2.uid = text;
				item2.album = album;
				item2.albumUid = item2.uid.BeginBefore('-');
				string s2 = item2.uid.LastAfter('-');
				item2.albumMusicIndex = int.Parse(s2);
				item2.level = num;
				item2.isUnlock = CheckAlbumBuy("music_package_" + item2.albumUid);
				item2.albumJsonIndex = jsonIndex;
				if ((item2.isUnlock || CheckMusicIsWeekFree(item2.uid)) && level >= item2.level)
				{
					m_RandomMusicRangeInfoList.Add(item2);
				}
				m_MusicRangeInfoList.Add(item2);
			}
		}

		private bool CheckIsSpecialMusic(string uid)
		{
			if (uid == "33-12")
			{
				return true;
			}
			return false;
		}

		private bool GetCurDifficulty(JToken stageJson)
		{
			int num = ((string)stageJson["difficulty1"] != "?") ? int.Parse((string)stageJson["difficulty1"]) : 0;
			m_AllStageDiff[0] = num;
			int num2 = ((string)stageJson["difficulty2"] != "?") ? int.Parse((string)stageJson["difficulty2"]) : 0;
			m_AllStageDiff[1] = num2;
			int num3 = ((string)stageJson["difficulty3"] != "?") ? int.Parse((string)stageJson["difficulty3"]) : 0;
			m_AllStageDiff[2] = num3;
			if (m_CurDifficutyTag == m_T1Difficulty)
			{
				for (int i = 0; i < m_AllStageDiff.Length; i++)
				{
					if (m_AllStageDiff[i] <= 3)
					{
						m_CurDiffIndex = i + 1;
						return true;
					}
				}
				return false;
			}
			if (m_CurDifficutyTag == m_T2Difficulty)
			{
				for (int j = 0; j < m_AllStageDiff.Length; j++)
				{
					if (m_AllStageDiff[j] >= 4 && m_AllStageDiff[j] <= 6)
					{
						m_CurDiffIndex = j + 1;
						return true;
					}
				}
				return false;
			}
			if (m_CurDifficutyTag == m_T3Difficulty)
			{
				for (int k = 0; k < m_AllStageDiff.Length; k++)
				{
					if (m_AllStageDiff[k] >= 7 && m_AllStageDiff[k] <= 8)
					{
						m_CurDiffIndex = k + 1;
						return true;
					}
				}
				return false;
			}
			if (m_CurDifficutyTag == m_T4Difficulty)
			{
				for (int l = 0; l < m_AllStageDiff.Length; l++)
				{
					if (m_AllStageDiff[l] >= 9 && m_AllStageDiff[l] <= 10)
					{
						m_CurDiffIndex = l + 1;
						return true;
					}
				}
				return false;
			}
			if (m_CurDifficutyTag == m_T5Difficulty)
			{
				for (int m = 0; m < m_AllStageDiff.Length; m++)
				{
					if (m_AllStageDiff[m] == 11)
					{
						m_CurDiffIndex = m + 1;
						return true;
					}
				}
				return false;
			}
			return false;
		}

		private bool CheckAlbumBuy(string albumUid)
		{
			if (albumUid == "?" || albumUid.EndsWith("?"))
			{
				return true;
			}
			if (albumUid == "music_package_0")
			{
				return true;
			}
			return Singleton<DataManager>.instance["IAP"][albumUid].GetResult<bool>() || BtnIAP.IsUnlockAll();
		}

		private bool CheckAlbumIsWeekFree(string albumUid)
		{
			int num = Singleton<WeekFreeManager>.instance.freeAlbumUids.Length;
			for (int i = 0; i < num; i++)
			{
				if (albumUid == Singleton<WeekFreeManager>.instance.freeAlbumUids[i])
				{
					return true;
				}
			}
			return false;
		}

		private bool CheckMusicIsWeekFree(string musicUid)
		{
			int num = Singleton<WeekFreeManager>.instance.freeSongUids.Length;
			for (int i = 0; i < num; i++)
			{
				if (musicUid == Singleton<WeekFreeManager>.instance.freeSongUids[i])
				{
					return true;
				}
			}
			return false;
		}

		private bool AlbumHadBuy(List<string> list, string uid)
		{
			if (uid == "tag-2")
			{
				return true;
			}
			bool result = false;
			int count = list.Count;
			for (int i = 0; i < count; i++)
			{
				if (CheckAlbumBuy(list[i]))
				{
					result = true;
				}
			}
			return result;
		}

		private bool CheckStageIsBan(string stageUid)
		{
			List<string> result = Singleton<DataManager>.instance["Account"]["BanStageList"].GetResult<List<string>>();
			return result.Contains(stageUid);
		}

		private void ChangeDefaultAlbum(List<musicInfo> defaultAlbumInfo, int accountLeve, string insertInfo, int level, bool isHide = false)
		{
			int count = defaultAlbumInfo.Count;
			int num = 0;
			while (true)
			{
				if (num < count)
				{
					musicInfo musicInfo = defaultAlbumInfo[num];
					if (musicInfo.level >= level)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			musicInfo musicInfo2 = default(musicInfo);
			musicInfo2.album = string.Empty;
			musicInfo2.albumMusicIndex = -1;
			musicInfo2.albumUid = string.Empty;
			musicInfo2.level = 0;
			musicInfo2.uid = string.Empty;
			musicInfo2.isUnlock = false;
			musicInfo2.albumJsonIndex = 0;
			musicInfo2.getDifficulty = 1;
			musicInfo item = musicInfo2;
			JArray json = Singleton<ConfigManager>.instance.GetJson(insertInfo, false);
			int count2 = json.Count;
			for (int i = 0; i < count2; i++)
			{
				item.uid = (string)json[i]["uid"];
				if (Singleton<HideManager>.instance.hideSongUids.Contains(item.uid) && !isHide)
				{
					continue;
				}
				if (m_CurDifficutyTag != m_AllDifficulty)
				{
					if (GetCurDifficulty(json[i]))
					{
						item.album = insertInfo;
						item.albumUid = item.uid.BeginBefore('-');
						string s = item.uid.LastAfter('-');
						item.albumMusicIndex = int.Parse(s);
						item.level = level;
						item.isUnlock = CheckAlbumBuy("music_package_" + item.albumUid);
						item.albumJsonIndex = i;
						item.getDifficulty = m_CurDiffIndex;
						if ((item.isUnlock || CheckMusicIsWeekFree(item.uid)) && accountLeve >= level)
						{
							m_RandomMusicRangeInfoList.Add(item);
						}
						defaultAlbumInfo.Insert(num, item);
						m_MusicRangeList.Insert(num, (string)json[i]["uid"]);
						m_MusicRangeLevelList.Insert(num, level);
					}
				}
				else
				{
					item.album = insertInfo;
					item.albumUid = item.uid.BeginBefore('-');
					string s2 = item.uid.LastAfter('-');
					item.albumMusicIndex = int.Parse(s2);
					item.level = level;
					item.isUnlock = CheckAlbumBuy("music_package_" + item.albumUid);
					item.albumJsonIndex = i;
					if ((item.isUnlock || CheckMusicIsWeekFree(item.uid)) && accountLeve >= level)
					{
						m_RandomMusicRangeInfoList.Add(item);
					}
					defaultAlbumInfo.Insert(num, item);
					m_MusicRangeList.Insert(num, (string)json[i]["uid"]);
					m_MusicRangeLevelList.Insert(num, level);
				}
			}
		}

		private void RandomMusicStage()
		{
			Singleton<DataManager>.instance["Account"]["SelectedAlbumUid"].SetResult($"music_package_{m_RandomMusicInfo.albumUid}");
			Singleton<DataManager>.instance["Account"]["SelectedMusicIndex"].SetResult(m_RandomMusicInfo.albumJsonIndex);
		}

		private void SetDifficultyTagTxt(int difficulty)
		{
			if (difficulty == 0)
			{
				m_DifficultyTagTxt.text = Singleton<AlbumTagName>.instance.GetAlbumDifficultyLocaliztion();
			}
			else
			{
				m_DifficultyTagTxt.text = m_AllDifficultyTextDic[m_AllAlbumDifficultys.IndexOf(difficulty)];
			}
		}

		private void SetLikeAchiEmpty(bool enable)
		{
			achievementObj.SetActive(enable);
			btnAhcievement.gameObject.SetActive(!enable);
			m_tglLikeImg.sprite = ((!enable) ? Singleton<AssetBundleManager>.instance.LoadFromName<Sprite>("BtnLikeOff") : Singleton<AssetBundleManager>.instance.LoadFromName<Sprite>("BtnLikeEmpty"));
		}
	}
}
