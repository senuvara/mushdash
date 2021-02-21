using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using System;
using UnityEngine;

public class SpecialSongManager : Singleton<SpecialSongManager>
{
	private struct InputPosData
	{
		public Vector3 mouseStartPos;

		public Vector3 mouseEndPos;
	}

	private const int width_size = 18;

	private const int height_size = 10;

	private Vector2[] m_PosPoints;

	private string[] m_OperationInfos;

	private string[] m_OperationInfosTouchTwo;

	public bool isMusicFREEDOMDIVE;

	public static bool isOnFREEDOMDIVE;

	private static string m_HideDifficultyFREEDOMDIVE;

	private static string m_DesignerNameFREEDOMDIVE;

	private string[] m_CircleData = new string[8]
	{
		"1234123412341",
		"4321432143214",
		"2341234123412",
		"3214321432143",
		"3412341234133",
		"2143214321432",
		"4123412341234",
		"1432143214321"
	};

	public bool isMusicXodus;

	public static bool isOnXodus;

	private static string m_XodusDifficulty;

	private static string m_DesignerNameXodus;

	private InputPosData m_CurPosData;

	private InputPosData m_NextPosData;

	private bool m_IsRecordPosData;

	private static string[] m_HalloweenDifficulty;

	private static string m_GOODTEKDifficulty;

	public int trggleNanoCoreCount;

	public bool isInitNanoCoreCount;

	public static bool isOnNanoCoreAudio;

	public static string[] m_NanoAuthorName;

	public int toggleHideBmsCount;

	public bool isHideBmsCount;

	public string hideBmsInfo;

	public static bool isOnMopemope;

	private static string m_MopemopeDifficulty;

	private static string m_DesignerNameMopeMope;

	public static bool isOnINFiNiTEENERZY;

	private static string m_INFiNiTEENERZYDifficulty;

	public static bool isOnGinevra;

	private static string m_DesignerNameGinevra;

	public static bool isOnTrippersFeeling;

	private static string m_TrippersFeelingDifficulty;

	private static string m_DesignerNameTrippersFeeling;

	public static bool isOnLightsOfMuse;

	private static string m_LightsOfMuseDifficulty;

	public static bool isOnXING;

	private static string m_XINGDifficulty;

	public static bool isOnStargazer;

	private static string m_StargazerDifficulty;

	private static string m_DesignerNameStargazer;

	public static bool isOnFujinRumble;

	private static string m_FujinRumbleDifficulty;

	private static string m_DesignerNameFujinRumble;

	public static bool isOnHGMakaizou;

	private static string m_HGMakaizouDifficulty;

	private static string m_DesignerNameHGMakaizou;

	public static bool isOnOuroboros;

	public static string ouroborosDifficulty;

	public static string designerNameOuroboros;

	public static bool isOnSquareLake;

	private static string m_SquareLakeDifficulty;

	private static string m_DesignerNameSquareLake;

	public static bool isOnHappinessBreeze;

	private static string m_HappinessBreezeDifficulty;

	private static string m_DesignerNameHappinessBreeze;

	public static bool isOnChromeVOX;

	private static string m_ChromeVOXDifficulty;

	public static bool isOnBattleNo1;

	private static string m_BattleNo1Difficulty;

	private static string m_DesignerNameBattleNo1;

	public static bool isOnCthugha;

	private static string m_CthughaDifficulty;

	private static string m_DesignerNameCthugha;

	public static bool isOnTwinkleMagic;

	private static string m_TwinkleMagicDifficulty;

	private static string m_DesignerNameTwinkleMagic;

	public static bool isOnCometCoaster;

	private static string m_CometCoasterDifficulty;

	private static string m_DesignerNameCometCoaster;

	public static Action onTriggerHideBmsEvent;

	private string SetFDOperationInfo(char str, string inputInfos)
	{
		if (inputInfos.Length > 24)
		{
			return string.Empty;
		}
		if (inputInfos.Length == 0)
		{
			inputInfos += str;
		}
		if (inputInfos.Length > 0 && inputInfos[inputInfos.Length - 1] != str)
		{
			inputInfos += str;
		}
		return inputInfos;
	}

	public void Init()
	{
		int num = 180;
		m_PosPoints = new Vector2[num];
		m_OperationInfos = new string[num];
		ResetUserInputInfo();
		int num2 = Screen.width / 18;
		int num3 = Screen.height / 10;
		int num4 = 0;
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 18; j++)
			{
				m_PosPoints[num4].x = num2 * (j + 1);
				m_PosPoints[num4].y = num3 * (i + 1);
				num4++;
			}
		}
		InitHideBMSInfo();
	}

	public void Update()
	{
		if (isMusicFREEDOMDIVE && !isOnFREEDOMDIVE)
		{
			UpdateFDInput();
		}
		if (isMusicXodus && !isOnXodus)
		{
			UpdateXInput();
		}
	}

	public bool IsHideBMS(string selectedMusicUid)
	{
		if ((isOnMopemope || selectedMusicUid != "0-45") && (isOnINFiNiTEENERZY || selectedMusicUid != "20-2") && (isOnGinevra || selectedMusicUid != "28-1") && (isOnTrippersFeeling || selectedMusicUid != "8-4") && (isOnLightsOfMuse || selectedMusicUid != "0-11") && (isOnXING || selectedMusicUid != "5-3") && (isOnStargazer || selectedMusicUid != "6-4") && (isOnFujinRumble || selectedMusicUid != "29-1") && (isOnHGMakaizou || selectedMusicUid != "29-3") && (isOnSquareLake || selectedMusicUid != "31-5") && (isOnHappinessBreeze || selectedMusicUid != "33-2") && (isOnChromeVOX || selectedMusicUid != "33-3") && (isOnBattleNo1 || selectedMusicUid != "34-1") && (isOnCthugha || selectedMusicUid != "34-2") && (isOnCometCoaster || selectedMusicUid != "34-4"))
		{
			return true;
		}
		return false;
	}

	public void SetHideBMSInfo(string selectedMusicUid)
	{
		int num = 0;
		string empty = string.Empty;
		if (selectedMusicUid == "21-2")
		{
			empty = "ALBUM22";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			isOnNanoCoreAudio = true;
			if (m_NanoAuthorName == null)
			{
				m_NanoAuthorName = new string[5];
				m_NanoAuthorName[0] = (string)Singleton<ConfigManager>.instance.GetJson(empty + "_ChineseS", false)[num]["name"];
				m_NanoAuthorName[1] = (string)Singleton<ConfigManager>.instance.GetJson(empty + "_ChineseT", false)[num]["name"];
				m_NanoAuthorName[2] = (string)Singleton<ConfigManager>.instance.GetJson(empty + "_English", false)[num]["name"];
				m_NanoAuthorName[3] = (string)Singleton<ConfigManager>.instance.GetJson(empty + "_Japanese", false)[num]["name"];
				m_NanoAuthorName[4] = (string)Singleton<ConfigManager>.instance.GetJson(empty + "_Korean", false)[num]["name"];
			}
			Singleton<ConfigManager>.instance.GetJson(empty + "_ChineseS", false)[num]["name"] = "Irreplaceable feat.AKINO with bless4";
			Singleton<ConfigManager>.instance.GetJson(empty + "_ChineseT", false)[num]["name"] = "Irreplaceable feat.AKINO with bless4";
			Singleton<ConfigManager>.instance.GetJson(empty + "_English", false)[num]["name"] = "Irreplaceable feat.AKINO with bless4";
			Singleton<ConfigManager>.instance.GetJson(empty + "_Japanese", false)[num]["name"] = "Irreplaceable feat.AKINO with bless4";
			Singleton<ConfigManager>.instance.GetJson(empty + "_Korean", false)[num]["name"] = "Irreplaceable feat.AKINO with bless4";
			Singleton<EventManager>.instance.Invoke("UI/OnChangeNanoCoreMusicShow");
		}
		if (selectedMusicUid == "4-5")
		{
			empty = "ALBUM5";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_GOODTEKDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty2"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty2"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
		}
		if (selectedMusicUid == "8-3")
		{
			empty = "ALBUM9";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			if (m_HalloweenDifficulty == null)
			{
				m_HalloweenDifficulty = new string[num];
				m_HalloweenDifficulty[0] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty1"];
				m_HalloweenDifficulty[1] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty2"];
				m_HalloweenDifficulty[2] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			}
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty1"] = "0";
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty2"] = Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = "0";
		}
		if (selectedMusicUid == "22-1" && !isOnFREEDOMDIVE)
		{
			isOnFREEDOMDIVE = true;
			empty = "ALBUM23";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_HideDifficultyFREEDOMDIVE = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			m_DesignerNameFREEDOMDIVE = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<EventManager>.instance.Invoke("UI/OnFreedomDiveShow");
		}
		if (selectedMusicUid == "0-45" && !isOnMopemope)
		{
			isOnMopemope = true;
			empty = "ALBUM1";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_DesignerNameMopeMope = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			m_MopemopeDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<EventManager>.instance.Invoke("UI/OnMopemopeShow");
		}
		if (selectedMusicUid == "20-2" && !isOnINFiNiTEENERZY)
		{
			isOnINFiNiTEENERZY = true;
			empty = "ALBUM21";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_INFiNiTEENERZYDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<EventManager>.instance.Invoke("UI/OnINFiNiTEENERZYShow");
		}
		if (selectedMusicUid == "28-1" && !isOnGinevra)
		{
			isOnGinevra = true;
			empty = "ALBUM29";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_DesignerNameGinevra = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<EventManager>.instance.Invoke("UI/OnGinevraShow");
		}
		if (selectedMusicUid == "8-4" && !isOnTrippersFeeling)
		{
			isOnTrippersFeeling = true;
			empty = "ALBUM9";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_DesignerNameTrippersFeeling = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			m_TrippersFeelingDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<EventManager>.instance.Invoke("UI/OnTrippersFeelingShow");
		}
		if (selectedMusicUid == "0-11" && !isOnLightsOfMuse)
		{
			isOnLightsOfMuse = true;
			empty = "ALBUM1";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_LightsOfMuseDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<EventManager>.instance.Invoke("UI/OnLightsOfMuseShow");
		}
		if (selectedMusicUid == "5-3" && !isOnXING)
		{
			isOnXING = true;
			empty = "ALBUM6";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_XINGDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<EventManager>.instance.Invoke("UI/OnXINGShow");
		}
		if (selectedMusicUid == "6-4" && !isOnStargazer)
		{
			isOnStargazer = true;
			empty = "ALBUM7";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_StargazerDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			m_DesignerNameStargazer = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<EventManager>.instance.Invoke("UI/OnStargazerShow");
		}
		if (selectedMusicUid == "29-1" && !isOnFujinRumble)
		{
			isOnFujinRumble = true;
			empty = "ALBUM30";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_FujinRumbleDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			m_DesignerNameFujinRumble = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<EventManager>.instance.Invoke("UI/OnFujinRumbleShow");
		}
		if (selectedMusicUid == "29-3" && !isOnHGMakaizou)
		{
			isOnHGMakaizou = true;
			empty = "ALBUM30";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_HGMakaizouDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			m_DesignerNameHGMakaizou = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<EventManager>.instance.Invoke("UI/OnHGMakaizouShow");
		}
		if (selectedMusicUid == "29-5" && !isOnOuroboros)
		{
			isOnOuroboros = true;
			empty = "ALBUM30";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			ouroborosDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			designerNameOuroboros = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<EventManager>.instance.Invoke("UI/OnOuroborosShow");
		}
		if (selectedMusicUid == "31-5" && !isOnSquareLake)
		{
			isOnSquareLake = true;
			empty = "ALBUM32";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_SquareLakeDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			m_DesignerNameSquareLake = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<EventManager>.instance.Invoke("UI/OnSquareLakeShow");
		}
		if (selectedMusicUid == "33-2" && !isOnHappinessBreeze)
		{
			isOnHappinessBreeze = true;
			empty = "ALBUM34";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_HappinessBreezeDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			m_DesignerNameHappinessBreeze = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<EventManager>.instance.Invoke("UI/OnHappinessBreezeShow");
		}
		if (selectedMusicUid == "33-3" && !isOnChromeVOX)
		{
			isOnChromeVOX = true;
			empty = "ALBUM34";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_ChromeVOXDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<EventManager>.instance.Invoke("UI/OnChromeVOXShow");
		}
		if (selectedMusicUid == "34-1" && !isOnBattleNo1)
		{
			isOnBattleNo1 = true;
			empty = "ALBUM35";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_BattleNo1Difficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			m_DesignerNameBattleNo1 = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<EventManager>.instance.Invoke("UI/OnBattleNo1Show");
		}
		if (selectedMusicUid == "34-2" && !isOnCthugha)
		{
			isOnCthugha = true;
			empty = "ALBUM35";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_CthughaDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			m_DesignerNameCthugha = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<EventManager>.instance.Invoke("UI/OnCthughaShow");
		}
		if (selectedMusicUid == "34-3" && !isOnTwinkleMagic)
		{
			isOnTwinkleMagic = true;
			empty = "ALBUM35";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_TwinkleMagicDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			m_DesignerNameTwinkleMagic = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<EventManager>.instance.Invoke("UI/OnTwinkleMagicShow");
		}
		if (selectedMusicUid == "34-4" && !isOnCometCoaster)
		{
			isOnCometCoaster = true;
			empty = "ALBUM35";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_CometCoasterDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			m_DesignerNameCometCoaster = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<EventManager>.instance.Invoke("UI/OnCometCoasterShow");
		}
		if (selectedMusicUid == "34-5" && !isOnXodus)
		{
			isOnXodus = true;
			empty = "ALBUM35";
			num = Singleton<ConfigManager>.instance.GetConfigIndex(empty, "uid", selectedMusicUid);
			m_XodusDifficulty = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"];
			m_DesignerNameXodus = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty3"] = (string)Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["difficulty4"];
			Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner3"] = Singleton<ConfigManager>.instance.GetJson(empty, false)[num]["levelDesigner4"];
			Singleton<EventManager>.instance.Invoke("UI/OnXodusShow");
		}
		if (onTriggerHideBmsEvent != null)
		{
			onTriggerHideBmsEvent();
		}
	}

	public void ResetHideBmsCondition()
	{
		toggleHideBmsCount = 0;
		trggleNanoCoreCount = 0;
		hideBmsInfo = string.Empty;
	}

	public void SetSpecialSong(string selectedMusicUid = null)
	{
		if (selectedMusicUid == "22-1")
		{
			Input.multiTouchEnabled = true;
			isMusicFREEDOMDIVE = true;
		}
		else if (selectedMusicUid == "34-5")
		{
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				Input.multiTouchEnabled = true;
				isMusicXodus = true;
			}, 0.3f);
		}
		else
		{
			isMusicFREEDOMDIVE = false;
			isMusicXodus = false;
		}
	}

	private void InitHideBMSInfo()
	{
		if (m_NanoAuthorName != null)
		{
			Singleton<ConfigManager>.instance.GetJson("ALBUM22_ChineseS", false)[2]["name"] = m_NanoAuthorName[0];
			Singleton<ConfigManager>.instance.GetJson("ALBUM22_ChineseT", false)[2]["name"] = m_NanoAuthorName[1];
			Singleton<ConfigManager>.instance.GetJson("ALBUM22_English", false)[2]["name"] = m_NanoAuthorName[2];
			Singleton<ConfigManager>.instance.GetJson("ALBUM22_Japanese", false)[2]["name"] = m_NanoAuthorName[3];
			Singleton<ConfigManager>.instance.GetJson("ALBUM22_Korean", false)[2]["name"] = m_NanoAuthorName[4];
		}
		if (m_GOODTEKDifficulty != null)
		{
			Singleton<ConfigManager>.instance.GetJson("ALBUM5", false)[5]["difficulty2"] = m_GOODTEKDifficulty;
		}
		if (m_HalloweenDifficulty != null)
		{
			Singleton<ConfigManager>.instance.GetJson("ALBUM9", false)[3]["difficulty1"] = m_HalloweenDifficulty[0];
			Singleton<ConfigManager>.instance.GetJson("ALBUM9", false)[3]["difficulty2"] = m_HalloweenDifficulty[1];
			Singleton<ConfigManager>.instance.GetJson("ALBUM9", false)[3]["difficulty3"] = m_HalloweenDifficulty[2];
		}
		if (m_HideDifficultyFREEDOMDIVE != null)
		{
			Singleton<ConfigManager>.instance.GetJson("ALBUM23", false)[1]["difficulty3"] = m_HideDifficultyFREEDOMDIVE;
			if (m_DesignerNameFREEDOMDIVE != null)
			{
				Singleton<ConfigManager>.instance.GetJson("ALBUM23", false)[1]["levelDesigner3"] = m_DesignerNameFREEDOMDIVE;
			}
		}
		if (m_MopemopeDifficulty != null)
		{
			int configIndex = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM1", "uid", "0-45");
			Singleton<ConfigManager>.instance.GetJson("ALBUM1", false)[configIndex]["difficulty3"] = m_MopemopeDifficulty;
			if (m_DesignerNameMopeMope != null)
			{
				Singleton<ConfigManager>.instance.GetJson("ALBUM1", false)[configIndex]["levelDesigner3"] = m_DesignerNameMopeMope;
			}
		}
		if (m_INFiNiTEENERZYDifficulty != null)
		{
			int configIndex2 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM21", "uid", "20-2");
			Singleton<ConfigManager>.instance.GetJson("ALBUM21", false)[configIndex2]["difficulty3"] = m_INFiNiTEENERZYDifficulty;
		}
		if (m_DesignerNameGinevra != null)
		{
			int configIndex3 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM29", "uid", "28-1");
			Singleton<ConfigManager>.instance.GetJson("ALBUM29", false)[configIndex3]["levelDesigner3"] = m_DesignerNameGinevra;
		}
		if (m_DesignerNameTrippersFeeling != null)
		{
			int configIndex4 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM9", "uid", "8-4");
			Singleton<ConfigManager>.instance.GetJson("ALBUM9", false)[configIndex4]["difficulty3"] = m_TrippersFeelingDifficulty;
			Singleton<ConfigManager>.instance.GetJson("ALBUM9", false)[configIndex4]["levelDesigner3"] = m_DesignerNameTrippersFeeling;
		}
		if (m_LightsOfMuseDifficulty != null)
		{
			int configIndex5 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM1", "uid", "0-11");
			Singleton<ConfigManager>.instance.GetJson("ALBUM1", false)[configIndex5]["difficulty3"] = m_LightsOfMuseDifficulty;
		}
		if (m_XINGDifficulty != null)
		{
			int configIndex6 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM6", "uid", "5-3");
			Singleton<ConfigManager>.instance.GetJson("ALBUM6", false)[configIndex6]["difficulty3"] = m_XINGDifficulty;
		}
		if (m_StargazerDifficulty != null)
		{
			int configIndex7 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM7", "uid", "6-4");
			Singleton<ConfigManager>.instance.GetJson("ALBUM7", false)[configIndex7]["difficulty3"] = m_StargazerDifficulty;
			Singleton<ConfigManager>.instance.GetJson("ALBUM7", false)[configIndex7]["levelDesigner3"] = m_DesignerNameStargazer;
		}
		if (m_FujinRumbleDifficulty != null)
		{
			int configIndex8 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM30", "uid", "29-1");
			Singleton<ConfigManager>.instance.GetJson("ALBUM30", false)[configIndex8]["difficulty3"] = m_FujinRumbleDifficulty;
			Singleton<ConfigManager>.instance.GetJson("ALBUM30", false)[configIndex8]["levelDesigner3"] = m_DesignerNameFujinRumble;
		}
		if (m_HGMakaizouDifficulty != null)
		{
			int configIndex9 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM30", "uid", "29-3");
			Singleton<ConfigManager>.instance.GetJson("ALBUM30", false)[configIndex9]["difficulty3"] = m_HGMakaizouDifficulty;
			Singleton<ConfigManager>.instance.GetJson("ALBUM30", false)[configIndex9]["levelDesigner3"] = m_DesignerNameHGMakaizou;
		}
		if (ouroborosDifficulty != null)
		{
			int configIndex10 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM30", "uid", "29-5");
			Singleton<ConfigManager>.instance.GetJson("ALBUM30", false)[configIndex10]["difficulty3"] = ouroborosDifficulty;
			Singleton<ConfigManager>.instance.GetJson("ALBUM30", false)[configIndex10]["levelDesigner3"] = designerNameOuroboros;
		}
		if (m_SquareLakeDifficulty != null)
		{
			int configIndex11 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM32", "uid", "31-5");
			Singleton<ConfigManager>.instance.GetJson("ALBUM32", false)[configIndex11]["difficulty3"] = m_SquareLakeDifficulty;
			Singleton<ConfigManager>.instance.GetJson("ALBUM32", false)[configIndex11]["levelDesigner3"] = m_DesignerNameSquareLake;
		}
		if (m_HappinessBreezeDifficulty != null)
		{
			int configIndex12 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM34", "uid", "33-2");
			Singleton<ConfigManager>.instance.GetJson("ALBUM34", false)[configIndex12]["difficulty3"] = m_HappinessBreezeDifficulty;
			Singleton<ConfigManager>.instance.GetJson("ALBUM34", false)[configIndex12]["levelDesigner3"] = m_DesignerNameHappinessBreeze;
		}
		if (m_ChromeVOXDifficulty != null)
		{
			int configIndex13 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM34", "uid", "33-3");
			Singleton<ConfigManager>.instance.GetJson("ALBUM34", false)[configIndex13]["difficulty3"] = m_ChromeVOXDifficulty;
		}
		if (m_BattleNo1Difficulty != null)
		{
			int configIndex14 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM35", "uid", "34-1");
			Singleton<ConfigManager>.instance.GetJson("ALBUM35", false)[configIndex14]["difficulty3"] = m_BattleNo1Difficulty;
			Singleton<ConfigManager>.instance.GetJson("ALBUM35", false)[configIndex14]["levelDesigner3"] = m_DesignerNameBattleNo1;
		}
		if (m_CthughaDifficulty != null)
		{
			int configIndex15 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM35", "uid", "34-2");
			Singleton<ConfigManager>.instance.GetJson("ALBUM35", false)[configIndex15]["difficulty3"] = m_CthughaDifficulty;
			Singleton<ConfigManager>.instance.GetJson("ALBUM35", false)[configIndex15]["levelDesigner3"] = m_DesignerNameCthugha;
		}
		if (m_TwinkleMagicDifficulty != null)
		{
			int configIndex16 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM35", "uid", "34-3");
			Singleton<ConfigManager>.instance.GetJson("ALBUM35", false)[configIndex16]["difficulty3"] = m_TwinkleMagicDifficulty;
			Singleton<ConfigManager>.instance.GetJson("ALBUM35", false)[configIndex16]["levelDesigner3"] = m_DesignerNameTwinkleMagic;
		}
		if (m_CometCoasterDifficulty != null)
		{
			int configIndex17 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM35", "uid", "34-4");
			Singleton<ConfigManager>.instance.GetJson("ALBUM35", false)[configIndex17]["difficulty3"] = m_CometCoasterDifficulty;
			Singleton<ConfigManager>.instance.GetJson("ALBUM35", false)[configIndex17]["levelDesigner3"] = m_DesignerNameCometCoaster;
		}
		if (m_XodusDifficulty != null)
		{
			int configIndex18 = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM35", "uid", "34-5");
			Singleton<ConfigManager>.instance.GetJson("ALBUM35", false)[configIndex18]["difficulty3"] = m_XodusDifficulty;
			Singleton<ConfigManager>.instance.GetJson("ALBUM35", false)[configIndex18]["levelDesigner3"] = m_DesignerNameXodus;
		}
		m_CurPosData = default(InputPosData);
		m_NextPosData = default(InputPosData);
		m_IsRecordPosData = false;
		hideBmsInfo = string.Empty;
		toggleHideBmsCount = 0;
		trggleNanoCoreCount = 0;
		isInitNanoCoreCount = false;
		isMusicFREEDOMDIVE = false;
		isMusicXodus = false;
		isOnNanoCoreAudio = false;
		isOnMopemope = false;
		isOnINFiNiTEENERZY = false;
		isOnGinevra = false;
		isOnTrippersFeeling = false;
		isOnLightsOfMuse = false;
		isOnXING = false;
		isOnStargazer = false;
		isOnFujinRumble = false;
		isOnHGMakaizou = false;
		isOnOuroboros = false;
		isOnSquareLake = false;
		isOnHappinessBreeze = false;
		isOnChromeVOX = false;
		isOnXodus = false;
		isOnBattleNo1 = false;
		isOnCthugha = false;
		isOnTwinkleMagic = false;
		isOnCometCoaster = false;
	}

	private void SetHideBMSInfo(Vector2 end, bool isTouchTwo = false)
	{
		for (int i = 0; i < 180; i++)
		{
			if (isTouchTwo)
			{
				m_OperationInfosTouchTwo[i] = SetFDOperationInfo(SetPosTag(i, end), m_OperationInfosTouchTwo[i]);
			}
			else
			{
				m_OperationInfos[i] = SetFDOperationInfo(SetPosTag(i, end), m_OperationInfos[i]);
			}
		}
	}

	private void UpdateFDInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			SetHideBMSInfo(Input.mousePosition);
		}
		if (Input.GetMouseButton(0))
		{
			for (int i = 0; i < m_PosPoints.Length; i++)
			{
				SetHideBMSInfo(Input.mousePosition);
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			ResetUserInputInfo();
		}
		else
		{
			HideBMSLogic();
		}
	}

	private void UpdateXInput()
	{
		if (Input.GetMouseButtonDown(0))
		{
			if (m_IsRecordPosData)
			{
				m_NextPosData.mouseStartPos = Input.mousePosition;
				m_IsRecordPosData = false;
			}
			else
			{
				m_CurPosData.mouseStartPos = Input.mousePosition;
				m_IsRecordPosData = true;
			}
		}
		if (!Input.GetMouseButtonUp(0))
		{
			return;
		}
		if (m_IsRecordPosData)
		{
			m_CurPosData.mouseEndPos = Input.mousePosition;
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				m_CurPosData = default(InputPosData);
			}, 2f);
		}
		else
		{
			m_NextPosData.mouseEndPos = Input.mousePosition;
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				m_NextPosData = default(InputPosData);
			}, 2f);
		}
		HideBMSLogic();
	}

	private void HideBMSLogic()
	{
		if (IsCircleData() && isMusicFREEDOMDIVE)
		{
			SetHideBMSInfo("22-1");
		}
		if (IsIntersection() && isMusicXodus)
		{
			SetHideBMSInfo("34-5");
		}
	}

	private char SetPosTag(int index, Vector2 end)
	{
		if (Mathf.Abs(m_PosPoints[index].x - end.x) > Mathf.Abs(m_PosPoints[index].y - end.y))
		{
			if (m_PosPoints[index].x > end.x)
			{
				return '4';
			}
			return '2';
		}
		if (m_PosPoints[index].y > end.y)
		{
			return '3';
		}
		return '1';
	}

	private bool IsCircleData()
	{
		for (int i = 0; i < 180; i++)
		{
			for (int j = 0; j < m_CircleData.Length; j++)
			{
				if (m_OperationInfos[i].Contains(m_CircleData[j]))
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool IsIntersection()
	{
		Vector3 vector = Vector3.Cross(m_NextPosData.mouseEndPos - m_NextPosData.mouseStartPos, m_CurPosData.mouseStartPos - m_NextPosData.mouseStartPos);
		float num = Mathf.Sign(vector.z);
		Vector3 vector2 = Vector3.Cross(m_NextPosData.mouseEndPos - m_NextPosData.mouseStartPos, m_CurPosData.mouseEndPos - m_NextPosData.mouseStartPos);
		float num2 = Mathf.Sign(vector2.z);
		if (num == num2)
		{
			return false;
		}
		Vector3 vector3 = Vector3.Cross(m_CurPosData.mouseEndPos - m_CurPosData.mouseStartPos, m_NextPosData.mouseStartPos - m_CurPosData.mouseStartPos);
		float num3 = Mathf.Sign(vector3.z);
		Vector3 vector4 = Vector3.Cross(m_CurPosData.mouseEndPos - m_CurPosData.mouseStartPos, m_NextPosData.mouseEndPos - m_CurPosData.mouseStartPos);
		float num4 = Mathf.Sign(vector4.z);
		if (num3 == num4)
		{
			return false;
		}
		return true;
	}

	private void ResetUserInputInfo()
	{
		for (int i = 0; i < 180; i++)
		{
			m_OperationInfos[i] = string.Empty;
		}
	}
}
