using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using FormulaBase;
using GameLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameCore.HostComponent
{
	public class BattleEnemyManager : Singleton<BattleEnemyManager>
	{
		private int m_CurrentGenIdx = -1;

		private int[] m_Hp;

		private byte[] m_Evaluates;

		private bool[] m_LeftRight;

		public bool isAirPressing;

		public bool isGroundPressing;

		public bool isAirPressingHead;

		public bool isGroundPressingHead;

		public bool isCharacterGroundHitting;

		public bool isCharacterAirHitting;

		public bool isGhostHitting;

		public bool isGhostOnAir;

		public bool isMulHitting;

		public float damageRatio;

		public void Init()
		{
			m_CurrentGenIdx = -1;
			isCharacterGroundHitting = false;
			isCharacterAirHitting = false;
			isAirPressing = false;
			isGroundPressing = false;
			isGhostHitting = false;
			isGhostOnAir = false;
			isMulHitting = false;
			isAirPressingHead = false;
			isGroundPressingHead = false;
		}

		public static void ReleaseReferences()
		{
			Singleton<BattleEnemyManager>.instance.m_Hp = null;
			Singleton<BattleEnemyManager>.instance.m_Evaluates = null;
			Singleton<BattleEnemyManager>.instance.m_LeftRight = null;
		}

		public byte GetPlayResult(int idx)
		{
			if (idx < 0 || idx >= m_Evaluates.Length)
			{
				return 0;
			}
			return m_Evaluates[idx];
		}

		public bool IsPlayLeft(int idx)
		{
			if (idx < 0 || idx >= m_LeftRight.Length)
			{
				return false;
			}
			return m_LeftRight[idx];
		}

		public bool IsDead(int idx)
		{
			return m_Hp[idx] <= 0;
		}

		public int GetCurrentGenIdx()
		{
			return m_CurrentGenIdx;
		}

		public int GetDamageValueByIndex(int idx)
		{
			if (damageRatio == 0f)
			{
				object obj;
				switch (Singleton<StageBattleComponent>.instance.GetDiffcult())
				{
				case 1u:
					obj = "easyDamageRatio";
					break;
				case 2u:
					obj = "normalDamageRatio";
					break;
				default:
					obj = "hardDamageRatio";
					break;
				}
				string key = (string)obj;
				damageRatio = float.Parse(SingletonScriptableObject<ConstanceManager>.instance[key]);
			}
			MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
			return Mathf.RoundToInt((float)musicDataByIdx.noteData.damage * damageRatio);
		}

		public string GetNodeAudioByIdx(int idx)
		{
			MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
			return musicDataByIdx.noteData.key_audio;
		}

		public string GetNodeUidByIdx(int idx)
		{
			MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
			return musicDataByIdx.noteData.uid;
		}

		public string GetHitEffectByIdx(int idx)
		{
			MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
			if (!musicDataByIdx.noteData.isShowPlayEffect)
			{
				return null;
			}
			return musicDataByIdx.noteData.effect;
		}

		public void SetPlayResult(int idx, byte result, bool isMulStart = false, bool isMulEnd = false, bool isLeft = false)
		{
			if (!isMulStart)
			{
				Singleton<TaskStageTarget>.instance.SetPlayResult(idx, result, isMulEnd);
			}
			m_Evaluates[idx] = result;
			m_LeftRight[idx] = isLeft;
			AddHp(idx, -1);
		}

		public void SetLongPressEffect(bool isTo, bool isAir = false, bool isForce = false)
		{
			if ((isTo && ((isAir && Singleton<StageBattleComponent>.instance.curJumpLpsIdx < 0) || (!isAir && Singleton<StageBattleComponent>.instance.curPunchLpsIdx < 0))) || (isTo && LongPressController.GetSac(isAir).IsLongPressAlpha()))
			{
				return;
			}
			if (isForce)
			{
				SpineActionController.PlayLongPressEffect(isTo, isAir);
			}
			else if (isAir)
			{
				if (isAirPressing != isTo)
				{
					SpineActionController.PlayLongPressEffect(isTo, true);
				}
			}
			else if (isGroundPressing != isTo)
			{
				SpineActionController.PlayLongPressEffect(isTo);
			}
		}

		public void AddHp(int idx, int value)
		{
			m_Hp[idx] += value;
		}

		public void CreateBattleEnemy(int idx)
		{
			MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
			if (!musicDataByIdx.isLongPressing && !musicDataByIdx.isLongPressEnd)
			{
				GameGlobal.gGameMusicScene.passIdx++;
			}
			CreateObj(idx, !musicDataByIdx.isLongPressing);
			m_CurrentGenIdx = idx;
		}

		public void PreloadCreateBattleEnemy()
		{
			Init();
			List<MusicData> musicData = Singleton<StageBattleComponent>.instance.GetMusicData();
			m_Hp = new int[musicData.Count];
			m_Evaluates = new byte[musicData.Count];
			m_LeftRight = new bool[musicData.Count];
			for (int i = 0; i < musicData.Count; i++)
			{
				MusicData musicData2 = musicData[i];
				short objId = musicData2.objId;
				string nodeUidByIdx = GetNodeUidByIdx(objId);
				if (!string.IsNullOrEmpty(nodeUidByIdx))
				{
					m_Hp[i] = 1;
					m_Evaluates[i] = 0;
					m_LeftRight[i] = false;
				}
			}
		}

		private string GetObjPath(int idx)
		{
			string nodeAnimation = GameGlobal.gGameMusic.GetNodeAnimation(idx);
			if (nodeAnimation == Boss.Instance.GetBossName())
			{
				return nodeAnimation;
			}
			return nodeAnimation;
		}

		private void CreateObj(int idx, bool isVisible = true)
		{
			string objPath = GetObjPath(idx);
			if (objPath == Boss.Instance.GetBossName())
			{
				GameObject gameObject = Boss.Instance.GetGameObject();
				ObjInit(gameObject, idx, isVisible);
				return;
			}
			GameObject preLoadGameObject = GameGlobal.gGameMusicScene.GetPreLoadGameObject(idx);
			if (preLoadGameObject == null)
			{
				GameObject gameObject2 = GameGlobal.gGameMusicScene.PreLoad(idx);
				if (gameObject2 == null)
				{
					if (!string.IsNullOrEmpty(objPath))
					{
						gameObject2 = Singleton<StageBattleComponent>.instance.AddObj(objPath);
					}
					ObjInit(gameObject2, idx, isVisible);
				}
				else
				{
					ObjInit(gameObject2, idx, isVisible);
				}
			}
			else
			{
				ObjInit(preLoadGameObject, idx, isVisible);
			}
		}

		private void ObjInit(GameObject obj, int idx, bool isVisible)
		{
			if (!(obj == null) && idx < GameGlobal.gGameMusicScene.nodeInitCtrls.Length)
			{
				NodeInitController nodeInitController = GameGlobal.gGameMusicScene.nodeInitCtrls[idx];
				if ((bool)nodeInitController)
				{
					obj.SetActive(isVisible);
					nodeInitController.Run();
				}
			}
		}
	}
}
