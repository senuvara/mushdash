using DYUnityLib;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameLogic
{
	public class GameGlobal
	{
		public static bool IS_DEBUG = false;

		public static bool IS_NODE_DEBUG = false;

		public static bool IS_UNLOCK_ALL_STAGE = false;

		public static bool ENABLE_LOCAL_SAVE = false;

		public const int LIMITE_INT = 999999;

		public const float TIME_SCALE = 1f;

		public const decimal DEFAULT_MUSIC_LEN = 240m;

		public const decimal DEFAULT_END_CD = 0.5m;

		public const decimal CONTINUE_ATTACK_IVR = 0.5m;

		public const decimal COMEOUT_TIME_MAX = 3m;

		public const int TOUCH_PHASE_COUNT = 5;

		public const decimal LONG_PRESS_FREQUENCY = 0.1m;

		public static int tickLoop = 1;

		public static int onClickNotReachableNumber = 0;

		public static bool isCytusHideBMS = false;

		public const int COMBO_INTERVAL = 10;

		public const uint FINISH_LEVEL_1 = 1u;

		public const uint FINISH_LEVEL_2 = 2u;

		public const uint FINISH_LEVEL_3 = 3u;

		public const int DIFF_LEVEL_NORMAL = 1;

		public const int DIFF_LEVEL_HARD = 2;

		public const int DIFF_LEVEL_SUPER = 3;

		public const int FIX_FATAL_MULT = 2;

		public const uint FIX_FEVER_ADD = 1u;

		public const string LOADING_SCENE_NAME = "LoadingScene";

		public const string PREFABS_PATH = "Prefabs/";

		public static string[] STAGE_EVLUATE_MAP = new string[5]
		{
			"D",
			"C",
			"B",
			"A",
			"S"
		};

		public const int TEAM_PLACE_ATK = 0;

		public const int TEAM_PLACE_DEF = 1;

		public const int TEAM_PLACE_BUF = 2;

		public const uint ITEM_TYPE_NONE = 0u;

		public const uint ITEM_TYPE_ITEM = 1u;

		public const uint ITEM_TYPE_EQUIP = 2u;

		public const uint EQUIPMENT_PART_WEAPON = 0u;

		public const uint EQUIPMENT_PART_CLOTH = 1u;

		public const uint EQUIPMENT_PART_OTHER = 9u;

		public const uint JUDGE_LEVEL_LIMITE = 5u;

		public static Hashtable JUDGE_MAP = new Hashtable();

		public const uint DEFAULT_BAG_LIMITE = 20u;

		public const int TMPKEY = 99999;

		public const int SKILL_CARRY_LIMITE = 5;

		public const int ROLE_CARRY_LIMITE = 5;

		public static decimal DELAY_FOR_MUSIC = 0.0m;

		public static decimal DELAY_FOR_GAMESTART = 0.0m;

		public static decimal DELAY_FOR_ANDRIOD = 0.0m;

		public const uint PLAY_RESULT_LOCK_LEVEL_LONG_PRESS = 1u;

		public const uint PLAY_RESULT_LOCK_LEVEL_BUFF = 2u;

		public const uint PRESS_STATE_NONE = 0u;

		public const uint PRESS_STATE_PUMCH = 1u;

		public const uint PRESS_STATE_JUMP = 2u;

		public const float REDUCE_ENERGY_TIME = 0.2f;

		public const float FEVER_LAST_TIME = 5f;

		public const int LONG_PRESS_NODE_TYPE = 12;

		public const decimal MISS_NO_CHECK_TICK = -5m;

		public const int SIGN_KEY_MIN_LEN = 2;

		public const int RESOURCE_TYPE_GOLD = 1;

		public const int RESOURCE_TYPE_DIAMOND = 2;

		public const int RESOURCE_TYPE_CUP = 3;

		public const uint NODE_TYPE_NONE = 0u;

		public const uint NODE_TYPE_MONSTER = 1u;

		public const uint NODE_TYPE_BLOCK = 2u;

		public const uint NODE_TYPE_PRESS = 3u;

		public const uint NODE_TYPE_HIDE = 4u;

		public const uint NODE_TYPE_BOSS = 5u;

		public const uint NODE_TYPE_HP = 6u;

		public const uint NODE_TYPE_MUSIC = 7u;

		public const uint NODE_TYPE_MUL = 8u;

		public const uint NODE_TYPE_SCENECHANGE = 9u;

		public const uint NODE_TYPE_AUTO_ON = 10u;

		public const uint NODE_TYPE_AUTO_OFF = 11u;

		public static List<uint> NODE_TYPES_NO_MISS = new List<uint>
		{
			6u,
			7u,
			4u,
			9u,
			10u,
			10u,
			11u
		};

		public const string LANGUAGE_VERSION = "chs";

		public const string SOUND_TYPE_HURT = "hurt";

		public const string SOUND_TYPE_FEVER = "fever";

		public const string SOUND_TYPE_MAIN_BOARD_TOUCH = "char_touch";

		public const string SOUND_TYPE_ENTER_STAGE = "enter_stage";

		public const string SOUND_TYPE_STAGE_START = "stage_start";

		public const string SOUND_TYPE_LAST_NODE = "final_note";

		public const string SOUND_TYPE_DEAD = "dead";

		public const string SOUND_TYPE_ON_EQUIP = "equip";

		public const string SOUND_TYPE_ON_EXP = "exp";

		public const string SOUND_TYPE_ON_CHEST = "capsule";

		public const string SOUND_TYPE_ON_CHEST_OPEN = "capsule_open";

		public const string SOUND_TYPE_ON_TEN_COMBO = "combo";

		public const string SOUND_TYPE_UI = "ui_touch";

		public const string SOUND_TYPE_UI_BGM = "ui_bgm";

		public const string SOUND_TYPE_UI_ATTACK_MISS = "attack_miss";

		public const string SOUND_TYPE_UI_JUMP_MISS = "jump_miss";

		public const int MUSIC_SINGLE_PRESS = 0;

		public const int MUSIC_END_PRESS = 1;

		public const int MUSIC_TOUCH_EVENT = 2;

		public const int SCENE_ADD_OBJ = 3;

		public const int MUSIC_STEP_EVENT = 4;

		public const int SCENE_EVENT = 5;

		public const int DYUL_EVENT_TOUCH_BEGAN = 6;

		public const int DYUL_EVENT_TOUCH_MOVED = 7;

		public const int DYUL_EVENT_TOUCH_ENDED = 8;

		public static ConfigLoader gConfigLoader = new ConfigLoader();

		public static TouchScript gTouch = null;

		public static GameMissPlay gGameMissPlay = null;

		public static GameTouchPlay gGameTouchPlay = null;

		public static GameMusic gGameMusic = null;

		public static GameMusicScene gGameMusicScene = null;

		public static Stopwatch stopwatch = new Stopwatch();

		public const float JUMP_WHOLE_TIME = 0.2f;

		public const bool BZAAAAAAA = true;
	}
}
