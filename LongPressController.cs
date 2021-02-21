using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using FormulaBase;
using GameLogic;
using System.Collections.Generic;

public class LongPressController : BaseEnemyObjectController
{
	public static int psidx = -1;

	public bool isActive = true;

	public bool isHurt;

	public bool isOnAir;

	public static int airFingerId = -1;

	public static int groundFingerId = -1;

	public static int index = -1;

	public static int airIndex = -1;

	private MusicData m_PMusicData;

	public static int GetIdx(bool isAir)
	{
		int num = (!isAir) ? index : airIndex;
		if (num == -1)
		{
			if (!isAir && Singleton<StageBattleComponent>.instance.curPunchLpsIdx != -1)
			{
				num = Singleton<StageBattleComponent>.instance.curPunchLpsIdx;
			}
			if (isAir && Singleton<StageBattleComponent>.instance.curJumpLpsIdx != -1)
			{
				num = Singleton<StageBattleComponent>.instance.curJumpLpsIdx;
			}
		}
		else
		{
			if (!isAir && Singleton<StageBattleComponent>.instance.curPunchLpsIdx > num)
			{
				num = Singleton<StageBattleComponent>.instance.curPunchLpsIdx;
			}
			if (isAir && Singleton<StageBattleComponent>.instance.curJumpLpsIdx > num)
			{
				num = Singleton<StageBattleComponent>.instance.curJumpLpsIdx;
			}
		}
		return num;
	}

	public static SpineActionController GetSac(bool isAir)
	{
		int idx = GetIdx(isAir);
		return GameMusicScene.instance.spineActionCtrls[idx];
	}

	public override void Init()
	{
		base.Init();
		if (psidx < 0)
		{
			psidx = idx;
		}
		m_MusicData = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
		isOnAir = m_MusicData.isAir;
		if (m_MusicData.isLongPressStart)
		{
			m_PMusicData = m_MusicData;
			return;
		}
		m_PMusicData = Singleton<StageBattleComponent>.instance.GetMusicData().Find((MusicData m) => m.configData.time == m_MusicData.longPressPTick && m.isAir == m_MusicData.isAir && m.configData.length > 0m);
	}

	public override bool ControllerMissCheck(int i, decimal currentTick)
	{
		return true;
	}

	public void OnControllerHit(TimeNodeOrder tno)
	{
		index = m_PMusicData.objId;
		airIndex = m_PMusicData.objId;
		bool isAir = tno.md.isAir;
		if (tno.isLongPressing && Singleton<BattleEnemyManager>.instance.GetPlayResult(m_PMusicData.objId) < 2)
		{
			LongPressHurt();
			Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, tno.isAir);
			return;
		}
		bool flag = false;
		if (!isAir)
		{
			flag = GameGlobal.gGameTouchPlay.isPunchStay;
			if (GameGlobal.gGameTouchPlay.isPunchEnded || GameGlobal.gGameTouchPlay.isPunchBegan)
			{
				flag = false;
			}
			if (tno.isLongPressStart)
			{
				flag = (GameGlobal.gGameTouchPlay.isPunchBegan && !GameGlobal.gGameTouchPlay.hasReactGround);
			}
			else if (tno.isLongPressing)
			{
				flag = (flag || GameGlobal.gGameTouchPlay.isPunchBegan || GameGlobal.gGameTouchPlay.isPunchStay);
			}
			else if (tno.isLongPressEnd)
			{
				flag = ((!tno.isPerfectNode && !tno.isLast) ? GameGlobal.gGameTouchPlay.isPunchEnded : (GameGlobal.gGameTouchPlay.isPunchBegan || GameGlobal.gGameTouchPlay.isPunchStay || GameGlobal.gGameTouchPlay.isPunchEnded));
			}
		}
		else
		{
			flag = GameGlobal.gGameTouchPlay.isJumpStay;
			if (GameGlobal.gGameTouchPlay.isJumpEnded || GameGlobal.gGameTouchPlay.isJumpBegan)
			{
				flag = false;
			}
			if (tno.isLongPressStart)
			{
				flag = (GameGlobal.gGameTouchPlay.isJumpBegan && !GameGlobal.gGameTouchPlay.hasReactAir);
			}
			else if (tno.isLongPressing)
			{
				flag = (flag || GameGlobal.gGameTouchPlay.isJumpBegan || GameGlobal.gGameTouchPlay.isJumpStay);
			}
			else if (tno.isLongPressEnd)
			{
				flag = ((!tno.isPerfectNode && !tno.isLast) ? GameGlobal.gGameTouchPlay.isJumpEnded : (GameGlobal.gGameTouchPlay.isJumpBegan || GameGlobal.gGameTouchPlay.isJumpStay || GameGlobal.gGameTouchPlay.isJumpEnded));
			}
		}
		if (flag)
		{
			if (isActive)
			{
				Singleton<BattleEnemyManager>.instance.AddHp(tno.idx, -1);
				if (tno.isLongPressStart)
				{
					if (tno.isAir)
					{
						airFingerId = ((GameGlobal.gGameTouchPlay.beganAirIndexs.Count <= 0) ? (-1) : GameGlobal.gGameTouchPlay.beganAirIndexs.Last());
					}
					else
					{
						groundFingerId = ((GameGlobal.gGameTouchPlay.beganGroundIndexs.Count <= 0) ? (-1) : GameGlobal.gGameTouchPlay.beganGroundIndexs.Last());
					}
					byte playResult = Singleton<BattleEnemyManager>.instance.GetPlayResult(m_PMusicData.objId);
					if (playResult == 0)
					{
						Singleton<BattleEnemyManager>.instance.SetLongPressEffect(true, tno.isAir);
					}
					Singleton<StatisticsManager>.instance.OnNoteResult(1);
					short objId = m_PMusicData.objId;
					if (objId != -1)
					{
						SpineActionController spineActionController = GameMusicScene.instance.spineActionCtrls[objId];
						if (!spineActionController.IsLongPressAlpha() && playResult == 0)
						{
							GameGlobal.gGameTouchPlay.TouchResult(tno.idx, tno.result, 1u, tno);
							if (!GameGlobal.gGameTouchPlay.hasReactGround && !tno.isAir)
							{
								Singleton<BattleEnemyManager>.instance.isGroundPressingHead = true;
								GameGlobal.gGameTouchPlay.hasReactGround = true;
							}
							if (!GameGlobal.gGameTouchPlay.hasReactAir && tno.isAir)
							{
								Singleton<BattleEnemyManager>.instance.isAirPressingHead = true;
								GameGlobal.gGameTouchPlay.hasReactAir = true;
							}
							GameGlobal.gGameTouchPlay.MoveTouchPhaser();
						}
					}
					GameGlobal.gTouch.RefreshCatch();
				}
				if (tno.isLongPressing)
				{
					bool flag2 = true;
					if (tno.isAir)
					{
						if (airFingerId != -1 && !GameGlobal.gGameTouchPlay.beganAirIndexs.Contains(airFingerId))
						{
							flag2 = false;
						}
					}
					else if (groundFingerId != -1 && !GameGlobal.gGameTouchPlay.beganGroundIndexs.Contains(groundFingerId))
					{
						flag2 = false;
					}
					if (flag2)
					{
						SpineActionController spineActionController2 = GameMusicScene.instance.spineActionCtrls[m_PMusicData.objId];
						if (!spineActionController2.IsLongPressAlpha())
						{
							GameGlobal.gGameTouchPlay.TouchResult(tno.idx, tno.result, 1u, tno);
						}
					}
					else
					{
						LongPressHurt();
						Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, tno.isAir);
					}
				}
				if (tno.isLongPressEnd && Singleton<BattleEnemyManager>.instance.GetPlayResult(m_PMusicData.objId) > 2)
				{
					Singleton<StatisticsManager>.instance.OnNoteResult(1);
					TouchEndResult(tno);
				}
			}
		}
		else if (m_MusicData.isLongPressing)
		{
			LongPressHurt();
			Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, tno.isAir);
		}
		if (tno.isLast && tno.isLongPressStart && Singleton<BattleEnemyManager>.instance.GetPlayResult(tno.idx) < 2)
		{
			Singleton<StatisticsManager>.instance.OnNoteResult(0);
			LongPressHurt();
			Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, tno.isAir);
		}
		if (tno.isLongPressEnd && tno.isLast)
		{
			if (SpineActionController.CurrentAnimationName(-1).Contains("Press") && !Singleton<BattleEnemyManager>.instance.isGroundPressing && !Singleton<BattleEnemyManager>.instance.isAirPressing)
			{
				if (tno.isAir && SingletonMonoBehaviour<GirlManager>.instance.IsJumpingAction())
				{
					SpineActionController.Play("char_uppress_end", -1);
				}
				else
				{
					SpineActionController.Play("char_run", -1);
				}
			}
			byte playResult2 = Singleton<BattleEnemyManager>.instance.GetPlayResult(tno.idx);
			if (playResult2 < 2)
			{
				if (m_PMusicData.objId > 0 && Singleton<BattleEnemyManager>.instance.GetPlayResult(m_PMusicData.objId) > 1)
				{
					Singleton<StatisticsManager>.instance.OnNoteResult(0);
				}
				else
				{
					Singleton<StatisticsManager>.instance.OnNoteResult(0);
				}
				LongPressHurt();
				if (m_PMusicData.objId > 0 && Singleton<BattleEnemyManager>.instance.GetPlayResult(m_PMusicData.objId) <= 1)
				{
					Singleton<StageBattleComponent>.instance.SetCombo(0);
				}
				Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, tno.isAir);
			}
			if (tno.isAir)
			{
				Singleton<BattleEnemyManager>.instance.isAirPressingHead = false;
			}
			else
			{
				Singleton<BattleEnemyManager>.instance.isGroundPressingHead = false;
			}
		}
		tno.isFucked = true;
	}

	public void TouchEndResult(TimeNodeOrder tno)
	{
		if (!isActive)
		{
			return;
		}
		int idx = (!tno.isAir) ? index : airIndex;
		if (Singleton<BattleEnemyManager>.instance.GetPlayResult(idx) < 2)
		{
			return;
		}
		if (tno.isRight)
		{
			MusicData musicData = Singleton<StageBattleComponent>.instance.GetMusicData().Find((MusicData m) => m.configData.time == tno.md.longPressPTick && m.isAir == tno.md.isAir && m.configData.length > 0m);
			tno.result = Singleton<BattleEnemyManager>.instance.GetPlayResult(musicData.objId);
		}
		if (tno.isPerfectNode && !tno.isLast)
		{
			tno.result = 4;
		}
		if (Singleton<BattleEnemyManager>.instance.GetPlayResult(tno.idx) <= 0)
		{
			GameGlobal.gGameTouchPlay.TouchResult(tno.idx, tno.result, 1u, tno);
			Singleton<TaskStageTarget>.instance.AddLongPressFinishCount(1);
			Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, tno.isAir, true);
			isActive = false;
		}
	}

	public override bool OnControllerMiss(int i)
	{
		return true;
	}

	public void LongPressHurt()
	{
		if (!base.gameObject.activeSelf)
		{
			return;
		}
		bool isAir = m_MusicData.isAir;
		if (!isActive)
		{
			return;
		}
		isActive = false;
		Singleton<StageBattleComponent>.instance.SetCombo(0);
		SpineActionController spineActionController = GameMusicScene.instance.spineActionCtrls[m_PMusicData.objId];
		spineActionController.SetAlpha(0.391f);
		Singleton<BattleEnemyManager>.instance.SetPlayResult(m_MusicData.objId, 1);
		List<MusicData> musicData = Singleton<StageBattleComponent>.instance.GetMusicData();
		List<MusicData> list = musicData.Where((MusicData m) => m.longPressPTick == m_PMusicData.configData.time && m.isAir == isAir && m.noteData.type == 3);
		for (int i = 0; i < list.Count; i++)
		{
			MusicData musicData2 = list[i];
			short objId = musicData2.objId;
			if (objId >= GameGlobal.gGameMusicScene.objCtrls.Length)
			{
				continue;
			}
			BaseSpineObjectController baseSpineObjectController = GameGlobal.gGameMusicScene.objCtrls[objId];
			if ((bool)baseSpineObjectController)
			{
				LongPressController longPressController = baseSpineObjectController as LongPressController;
				if ((bool)longPressController)
				{
					Singleton<BattleEnemyManager>.instance.SetPlayResult(objId, 1);
					longPressController.isActive = false;
				}
			}
		}
		if (!isHurt && !spineActionController.IsLongPressDestroy() && (!Singleton<BattleEnemyManager>.instance.isGroundPressing || !Singleton<BattleEnemyManager>.instance.isAirPressing))
		{
			bool flag = SingletonMonoBehaviour<GirlManager>.instance.IsAir();
			if ((!isAir || flag) && (isAir || !flag))
			{
				if (!FeverManager.Instance.IsGod())
				{
					SingletonMonoBehaviour<GirlManager>.instance.UnLockActionProtect();
					GameGlobal.gGameMusicScene.OnObjBeMissed(m_PMusicData.objId);
					if (!isAir && Singleton<BattleEnemyManager>.instance.isGroundPressing)
					{
						SpineActionController.Play("char_run", -1);
					}
				}
				isHurt = true;
				List<MusicData> musicData3 = Singleton<StageBattleComponent>.instance.GetMusicData();
				List<MusicData> list2 = musicData3.Where((MusicData m) => m.longPressPTick == m_PMusicData.configData.time && m.isAir == isAir && m.noteData.type == 3);
				foreach (MusicData item in list2)
				{
					short objId2 = item.objId;
					if (objId2 >= GameGlobal.gGameMusicScene.objCtrls.Length)
					{
						continue;
					}
					BaseSpineObjectController baseSpineObjectController2 = GameGlobal.gGameMusicScene.objCtrls[objId2];
					if ((bool)baseSpineObjectController2)
					{
						LongPressController longPressController2 = baseSpineObjectController2 as LongPressController;
						if ((bool)longPressController2)
						{
							longPressController2.isHurt = true;
						}
					}
				}
			}
		}
		if (!Singleton<BattleEnemyManager>.instance.isGroundPressing && !Singleton<BattleEnemyManager>.instance.isAirPressing)
		{
			Singleton<BattleProperty>.instance.isHpChangable = true;
		}
	}
}
