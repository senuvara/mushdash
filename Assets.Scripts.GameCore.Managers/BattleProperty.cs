using Assets.Scripts.PeroTools.Commons;
using System;

namespace Assets.Scripts.GameCore.Managers
{
	public class BattleProperty : Singleton<BattleProperty>
	{
		public int maxHp = 300;

		public decimal missHardTime = 1.2m;

		public bool isBloodMissHardTime;

		public int greatToPerfect;

		public int missToGreat;

		public bool isAutoPlay;

		public float heartNoteRate = 1f;

		public float musicNoteRate = 1f;

		public float hideNoteRate = 1f;

		public float blockNoteRate = 1f;

		public float blockDamageRate = 1f;

		public float expRate = 1f;

		public bool hasGodChance;

		public int godTimeCount;

		public float godTime = 15f;

		public bool isInGod;

		public float comboRate = 0.5f;

		public int missComboMax;

		public int hpSkillCount;

		public int hpRevive = 100;

		public float reviveRate = 1f;

		public decimal skillMissHardTime = 2.0m;

		public float maxFever = 120f;

		public decimal hpChangedPerTime;

		public bool isHpChangable;

		public float scoreExtraRate = 1f;

		public float feverTime = 5f;

		public int hurtReduce;

		public int reviveDeadline = -1;

		public decimal reviveDuration = 15m;

		public bool isReviveInvoked;

		public decimal reviveValue = 0.1m;

		public float bossAttackScoreRate = 1f;

		public bool isFeverGod;

		public float feverScoreRate = 1f;

		public float perfectScoreExtra = 1f;

		public int perfectHpRevive;

		public int musicNoteAddHp;

		public decimal hitRangePerfectAdded;

		public int blockNoHurtRange = 150;

		public decimal hitRangeAdded;

		public decimal blockRP;

		public decimal blockRG;

		public Func<float, float> accFunc;

		public bool isCatchAvailable;

		public float catchGapThreshold = 0.3f;

		public bool isGroundCatching;

		public bool isAirCatching;

		public bool isGcCharacter;

		public bool isGCScene;

		public bool isNekoCharacter;

		public bool isNekoSkillTrigger;
	}
}
