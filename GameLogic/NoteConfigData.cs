using System.Collections.Generic;

namespace GameLogic
{
	public struct NoteConfigData
	{
		public static uint[] NODE_TYPE_IS_ADD_COMBO = new uint[5]
		{
			1u,
			4u,
			3u,
			5u,
			8u
		};

		public static uint[] NODE_TYPE_IS_MISS_COMBO = new uint[5]
		{
			1u,
			2u,
			3u,
			5u,
			8u
		};

		public static uint[] NODE_TYPE_IS_SHOW_PLAY_EFFECT = new uint[8]
		{
			1u,
			4u,
			3u,
			5u,
			8u,
			9u,
			10u,
			11u
		};

		public string id;

		public string ibms_id;

		public string uid;

		public string scene;

		public string des;

		public string prefab_name;

		public uint type;

		public string effect;

		public string key_audio;

		public string boss_action;

		public List<string> sceneChangeNames;

		public decimal left_perfect_range;

		public decimal left_great_range;

		public decimal right_perfect_range;

		public decimal right_great_range;

		public int damage;

		public int pathway;

		public int speed;

		public int score;

		public int fever;

		public bool missCombo;

		public bool addCombo;

		public bool jumpNote;

		public bool isShowPlayEffect;
	}
}
