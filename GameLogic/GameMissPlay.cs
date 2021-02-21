using Assets.Scripts.GameCore.GameObjectLogics.GameObjectManager;
using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using FormulaBase;
using UnityEngine;

namespace GameLogic
{
	public class GameMissPlay
	{
		private decimal m_MissHardTime = -1m;

		public void Init()
		{
			m_MissHardTime = -1m;
		}

		public void SetMissHardTime(decimal t)
		{
			m_MissHardTime = t;
			if (t == 0m)
			{
				SingletonMonoBehaviour<GirlManager>.instance.StopBeAttckedEffect();
			}
		}

		public decimal GetMissHardTime()
		{
			return m_MissHardTime;
		}

		public bool MissCube(int idx, decimal currentTick)
		{
			MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
			if (musicDataByIdx.noteData.type == 2 && Singleton<BattleProperty>.instance.isGcCharacter)
			{
				return !(m_MissHardTime > 0m);
			}
			if (BattleRoleAttributeComponent.instance.IsDead() || idx == 0 || Singleton<BattleProperty>.instance.isAutoPlay)
			{
				return false;
			}
			if (idx >= GameGlobal.gGameMusicScene.objCtrls.Length || !GameGlobal.gGameMusicScene.objCtrls[idx])
			{
				Debug.Log("MissCube Already null with " + idx + " Time:" + currentTick);
				return false;
			}
			BaseSpineObjectController baseSpineObjectController = GameGlobal.gGameMusicScene.objCtrls[idx];
			if (SkillMiss(idx))
			{
				return false;
			}
			if (!baseSpineObjectController.ControllerMissCheck(idx, currentTick))
			{
				return false;
			}
			baseSpineObjectController.OnControllerMiss(idx);
			if (GameGlobal.IS_DEBUG)
			{
				Debug.Log("Miss at " + idx);
			}
			return false;
		}

		public bool SkillMiss(int idx)
		{
			if (Singleton<BattleProperty>.instance.missToGreat > 0)
			{
				MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
				bool flag = musicDataByIdx.doubleIdx > -1 && musicDataByIdx.doubleIdx != 9999;
				byte playResult = Singleton<BattleEnemyManager>.instance.GetPlayResult(idx);
				byte b = (byte)((!flag) ? 4 : Singleton<BattleEnemyManager>.instance.GetPlayResult(musicDataByIdx.doubleIdx));
				if (((!flag) ? (playResult == 0) : (playResult == 0 || b == 0)) && !musicDataByIdx.isLongPressType && !musicDataByIdx.isMul && musicDataByIdx.noteData.type != 2)
				{
					if (flag)
					{
						if (playResult == 0)
						{
							GameObject gameObject = AttackEffectManager.instance.carrotRobotSkillEffect.CreateInstance();
							Transform transform = gameObject.transform;
							Vector3 position = gameObject.transform.position;
							float x = position.x;
							float y = (!musicDataByIdx.isAir) ? (-0.85f) : 1.3f;
							Vector3 position2 = gameObject.transform.position;
							transform.position = new Vector3(x, y, position2.z);
						}
						if (b == 0)
						{
							GameObject gameObject2 = AttackEffectManager.instance.carrotRobotSkillEffect.CreateInstance();
							Transform transform2 = gameObject2.transform;
							Vector3 position3 = gameObject2.transform.position;
							float x2 = position3.x;
							float y2 = musicDataByIdx.isAir ? (-0.85f) : 1.3f;
							Vector3 position4 = gameObject2.transform.position;
							transform2.position = new Vector3(x2, y2, position4.z);
						}
					}
					else
					{
						GameObject gameObject3 = AttackEffectManager.instance.carrotRobotSkillEffect.CreateInstance();
						Transform transform3 = gameObject3.transform;
						Vector3 position5 = gameObject3.transform.position;
						float x3 = position5.x;
						float y3 = (!musicDataByIdx.isAir) ? (-0.85f) : 1.3f;
						Vector3 position6 = gameObject3.transform.position;
						transform3.position = new Vector3(x3, y3, position6.z);
					}
					Singleton<BattleProperty>.instance.missToGreat--;
					AttacksController.Instance.showAttackAnim = false;
					GameGlobal.gGameTouchPlay.TouchResult(idx, 3, 1u, null, true);
					if (flag)
					{
						GameGlobal.gGameTouchPlay.TouchResult(musicDataByIdx.doubleIdx, 3, 1u, null, true);
					}
					AttacksController.Instance.showAttackAnim = true;
					Singleton<EventManager>.instance.Invoke("Battle/OnMiss2Great");
					if (flag && b == 0 && playResult == 0)
					{
						Singleton<EventManager>.instance.Invoke("Battle/OnRobotDoubleHit");
					}
					else
					{
						Singleton<EventManager>.instance.Invoke((!((b != 0) ? musicDataByIdx.isAir : (!musicDataByIdx.isAir))) ? "Battle/OnRobotDownHit" : "Battle/OnRobotUpHit");
					}
					return true;
				}
			}
			return false;
		}
	}
}
