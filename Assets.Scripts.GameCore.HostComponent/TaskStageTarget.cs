using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using CodeStage.AntiCheat.ObscuredTypes;
using FormulaBase;
using GameLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameCore.HostComponent
{
	public class TaskStageTarget : Singleton<TaskStageTarget>
	{
		private int m_EnergyCount;

		private int m_MusicCount;

		private int m_HitCount;

		private int m_LongPressCount;

		private int m_MaxCombo;

		private ObscuredInt m_Score;

		private int m_Block;

		private int m_Blood;

		private int m_LongPressHitCount;

		private int m_HitEnemy;

		private int m_BossNearHit;

		private int m_Miss;

		private int m_MissCombo;

		private int m_Recover;

		private int m_MaxDamage;

		private int m_Damage;

		private int m_HideNote;

		private int m_MissResult;

		private int m_CoolResult;

		private int m_GreatResult;

		private int m_PerfectResult;

		private int m_JumpOverResult;

		private int m_FeverResult;

		public bool IsAll(string[] ibmsId, byte result)
		{
			List<MusicData> musicData = Singleton<StageBattleComponent>.instance.GetMusicData();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < musicData.Count; i++)
			{
				MusicData musicData2 = musicData[i];
				if (ibmsId.Contains(musicData2.noteData.ibms_id) && !musicData2.isLongPressing)
				{
					num++;
					if (Singleton<BattleEnemyManager>.instance.GetPlayResult(musicData2.objId) == result)
					{
						num2++;
					}
				}
			}
			return num2 == num;
		}

		public bool IsFullCombo()
		{
			return GetComboMiss() == 0;
		}

		public float GetAccuracy()
		{
			float trueAccuracy = GetTrueAccuracy();
			float num = 0.0001f;
			float num2 = (float)Mathf.RoundToInt(trueAccuracy / num) * num;
			if (trueAccuracy < num2 && (num2 == 0.6f || num2 == 0.7f || num2 == 0.8f || num2 == 0.9f || num2 == 1f))
			{
				num2 -= num;
			}
			return num2;
		}

		public float GetTrueAccuracy()
		{
			List<MusicData> musicData = Singleton<StageBattleComponent>.instance.GetMusicData();
			if (musicData == null)
			{
				return 0f;
			}
			int num = musicData.Count((MusicData m) => m.noteData.type == 6 || m.noteData.type == 7);
			int num2 = musicData.Count((MusicData m) => m.noteData.addCombo && !m.isLongPressing);
			int num3 = musicData.Count((MusicData m) => m.noteData.type == 2);
			float num4 = (float)(num2 + num3) + (float)num / 2f;
			if (Singleton<StageBattleComponent>.instance.isTutorial && !Singleton<StageBattleComponent>.instance.IsFool())
			{
				num4 = 34f;
			}
			int num5 = GetNoteItemCount() + GetEnergyItemCount();
			float num6 = (float)GetHitCountByResult(4u) + (float)GetHitCountByResult(3u) / 2f;
			int block = GetBlock();
			float num7 = num6 + (float)block + (float)num5 / 2f;
			float num8 = num7 / num4;
			if (Singleton<BattleProperty>.instance.accFunc != null)
			{
				return Singleton<BattleProperty>.instance.accFunc(num8);
			}
			return num8;
		}

		public KeyValuePair<string, int> GetStageEvaluate()
		{
			float accuracy = GetAccuracy();
			bool flag = IsFullCombo();
			if (accuracy <= 0.5999f)
			{
				if (flag)
				{
					return new KeyValuePair<string, int>("a", 3);
				}
				return new KeyValuePair<string, int>("d", 0);
			}
			if (accuracy <= 0.6999f)
			{
				if (flag)
				{
					return new KeyValuePair<string, int>("a", 3);
				}
				return new KeyValuePair<string, int>("c", 1);
			}
			if (accuracy <= 0.7999f)
			{
				if (flag)
				{
					return new KeyValuePair<string, int>("a", 3);
				}
				return new KeyValuePair<string, int>("b", 2);
			}
			if (accuracy <= 0.8999f)
			{
				return new KeyValuePair<string, int>("a", 3);
			}
			if (accuracy <= 0.9499f)
			{
				return new KeyValuePair<string, int>("s", 4);
			}
			if (accuracy < 1f)
			{
				return new KeyValuePair<string, int>("ss", 5);
			}
			return new KeyValuePair<string, int>("sss", 6);
		}

		public void SetPlayResult(int idx, uint result, bool isMulEnd = false)
		{
			if (result == 0)
			{
				return;
			}
			MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
			byte playResult = Singleton<BattleEnemyManager>.instance.GetPlayResult(idx);
			if (musicDataByIdx.isLongPressing || !musicDataByIdx.noteData.addCombo)
			{
				return;
			}
			int num = 0;
			if (musicDataByIdx.isLongPressEnd || musicDataByIdx.isLongPressStart)
			{
				AddCount(result, 1);
				if (result > 1)
				{
					num = 1;
				}
			}
			else if (playResult == 0 || isMulEnd || musicDataByIdx.doubleIdx > 0)
			{
				if (musicDataByIdx.doubleIdx > 0 && musicDataByIdx.doubleIdx != 9999)
				{
					byte playResult2 = Singleton<BattleEnemyManager>.instance.GetPlayResult(musicDataByIdx.doubleIdx);
					if ((playResult2 == 4 && result == 3) || (result == 4 && playResult2 == 3) || (result == playResult2 && playResult2 == 3))
					{
						AddCount(3u, 2);
					}
					else if (result == playResult2 && playResult2 == 4)
					{
						AddCount(4u, 2);
					}
					else if (result == 1 && playResult2 == 0)
					{
						AddCount(1u, 2);
					}
				}
				else
				{
					AddCount(result, 1);
				}
				if (result > 1)
				{
					num = 1;
				}
			}
			if (num <= 0)
			{
				return;
			}
			m_HitCount += num;
			uint type = musicDataByIdx.noteData.type;
			string boss_action = musicDataByIdx.noteData.boss_action;
			if (type == 4)
			{
				AddHideNodeCount(num);
			}
			switch (type)
			{
			default:
				return;
			case 8u:
				if (string.IsNullOrEmpty(boss_action) || !(boss_action != "0"))
				{
					return;
				}
				break;
			case 5u:
				break;
			}
			AddBossNearHit(num);
			MultHitEnemyController.ShakeBossCamera();
		}

		private void AddCount(uint result, int value)
		{
			if (!Singleton<BattleProperty>.instance.isAutoPlay || !Singleton<StageBattleComponent>.instance.isTutorial)
			{
				switch (result)
				{
				case 1u:
					m_MissResult += value;
					break;
				case 2u:
					m_CoolResult += value;
					break;
				case 3u:
					m_GreatResult += value;
					break;
				case 4u:
					m_PerfectResult += value;
					break;
				case 5u:
					m_JumpOverResult += value;
					break;
				case 6u:
					m_FeverResult += value;
					break;
				}
			}
		}

		public int GetHitCount()
		{
			return m_HitCount;
		}

		public int GetHitCountByResult(uint result)
		{
			switch (result)
			{
			case 1u:
				return m_MissResult;
			case 2u:
				return m_CoolResult;
			case 3u:
				return m_GreatResult;
			case 4u:
				return m_PerfectResult;
			case 5u:
				return m_JumpOverResult;
			case 6u:
				return m_FeverResult;
			default:
				return 0;
			}
		}

		public float GetHitPercent(uint result)
		{
			int hitCountByResult = GetHitCountByResult(result);
			return (float)hitCountByResult / (float)GetTotalNum();
		}

		public void AddEnergyItemCount(int value)
		{
			m_EnergyCount += value;
		}

		public int GetEnergyItemCount()
		{
			return m_EnergyCount;
		}

		public void AddNoteItemCount(int value)
		{
			m_MusicCount += value;
		}

		public int GetNoteItemCount()
		{
			return m_MusicCount;
		}

		public void AddComboMax(int value)
		{
			if (value > m_MaxCombo)
			{
				m_MaxCombo = value;
			}
		}

		public int GetComboMax()
		{
			return m_MaxCombo;
		}

		public void AddDamageMax(int value)
		{
			int maxDamage = m_MaxDamage;
			if (value >= maxDamage)
			{
				m_MaxDamage = value;
			}
		}

		public void AddScore(int value, int id, string noteType, bool isAir, float time = -1f)
		{
			if (!string.IsNullOrEmpty(noteType) && !Singleton<StageBattleComponent>.instance.IsAutoPlay())
			{
				time = ((!(time < 0f)) ? time : GameGlobal.gTouch.tickTime);
				Singleton<StatisticsManager>.instance.OnGetScore(id, noteType, Mathf.RoundToInt(time * 1000f), value, (!isAir) ? "bottom" : "top");
			}
			AddDamageMax(value);
			m_Score = (int)m_Score + value;
			m_Damage = value;
			Singleton<EventManager>.instance.Invoke("Battle/OnScoreChanged", GetScore());
		}

		public void AddBlock(int value)
		{
			m_Block += value;
		}

		public int GetBlock()
		{
			return m_Block;
		}

		public void AddBlood(int value)
		{
			m_Blood += value;
		}

		public int GetBlood()
		{
			return m_Blood;
		}

		public void AddHitEnemy(int value)
		{
			m_HitEnemy += value;
		}

		public int GetHitEnemy()
		{
			return m_HitEnemy;
		}

		public void AddLongPressHit(int value)
		{
			m_LongPressHitCount += value;
		}

		public int GetLongPressHit()
		{
			return m_LongPressHitCount;
		}

		public void AddBossNearHit(int value)
		{
			m_BossNearHit += value;
		}

		public int GetBossNearHit()
		{
			return m_BossNearHit;
		}

		public void AddMiss(int value)
		{
			m_Miss += value;
		}

		public int GetMiss()
		{
			return m_Miss;
		}

		public void AddComboMiss(int value)
		{
			m_MissCombo += value;
		}

		public int GetComboMiss()
		{
			return m_MissCombo;
		}

		public int GetAddScore()
		{
			return m_Damage;
		}

		public int GetScore()
		{
			return m_Score;
		}

		public void SetRecover(int value)
		{
			m_Recover = value;
		}

		public int GetRecover()
		{
			return m_Recover;
		}

		public void AddLongPressFinishCount(int value)
		{
			m_LongPressCount += value;
		}

		public int GetLongPressFinishCount()
		{
			return m_LongPressCount;
		}

		public void AddHideNodeCount(int value)
		{
			m_HideNote += value;
		}

		public int GetHideNoteHitCount()
		{
			return m_HideNote;
		}

		public int GetTotalNum(uint noteType = uint.MaxValue)
		{
			List<MusicData> musicData = Singleton<StageBattleComponent>.instance.GetMusicData();
			if (musicData == null)
			{
				return 1;
			}
			return (noteType != uint.MaxValue) ? musicData.Count((MusicData m) => m.noteData.type == noteType) : musicData.Count((MusicData m) => m.noteData.addCombo && !m.isLongPressing);
		}
	}
}
