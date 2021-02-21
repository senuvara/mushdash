using Assets.Scripts.GameCore.GameObjectLogics.GameObjectManager;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using CodeStage.AntiCheat.ObscuredTypes;
using FormulaBase;
using GameLogic;
using UnityEngine;

namespace Assets.Scripts.GameCore.HostComponent
{
	public class BattleRoleAttributeComponent
	{
		private static BattleRoleAttributeComponent m_Instance;

		public ObscuredInt hp;

		public int hurt;

		public int state;

		public int early;

		public int late;

		private Coroutine m_Coroutine1;

		private Coroutine m_Coroutine2;

		private bool m_IsShowLateEarly;

		private Coroutine m_HurtEffectCoroutine;

		private Coroutine m_ZombiaCoroutine;

		public static BattleRoleAttributeComponent instance => m_Instance ?? (m_Instance = new BattleRoleAttributeComponent());

		public void Init()
		{
			SingletonMonoBehaviour<GirlManager>.instance.Reset();
			AttackEffectManager.instance.Reset();
			hp = GetHpMax();
			hurt = 0;
			early = 0;
			late = 0;
			m_IsShowLateEarly = Singleton<DataManager>.instance["Account"]["IsAdvancedJudge"].GetResult<bool>();
			Revive();
		}

		private void Destroy()
		{
			SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_Coroutine1);
			SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_Coroutine2);
		}

		public void AddHp(int value)
		{
			int hpMax = GetHpMax();
			int num = ((int)hp + value <= hpMax || value <= 0) ? value : (hpMax - (int)hp);
			ObscuredInt value2 = hp;
			hp = (int)hp + num;
			if (((int)hp <= 0 || (int)value2 == 0) && Singleton<BattleProperty>.instance.isNekoCharacter && Singleton<DataManager>.instance["Account"]["IsNekoSkillAvailable"].GetResult<bool>() && num != GetHpMax())
			{
				hp = 0;
				Singleton<EventManager>.instance.Invoke("Battle/OnHpRateChanged");
				Singleton<EventManager>.instance.Invoke("Battle/OnHpAdd", HpRate());
				if (!Singleton<BattleProperty>.instance.isNekoSkillTrigger)
				{
					Singleton<EventManager>.instance.Invoke("Battle/OnNekoSkillTrigger");
					AttackEffectManager.instance.nekoSkillEffect.SetActive(true);
					GirlActionController.instance.animator.Play("char_neko_invincible", 1, 0f);
					Singleton<BattleProperty>.instance.isNekoSkillTrigger = true;
				}
				return;
			}
			if ((int)hp <= 0)
			{
				hp = 0;
				if (Singleton<BattleProperty>.instance.isInGod)
				{
					hp = 1;
				}
			}
			Singleton<EventManager>.instance.Invoke("Battle/OnHpRateChanged");
			if (value > 0)
			{
				Singleton<TaskStageTarget>.instance.SetRecover(value);
				Singleton<EventManager>.instance.Invoke("Battle/OnHpAdd", HpRate());
			}
			else if (value < 0)
			{
				Singleton<EventManager>.instance.Invoke("Battle/OnHpDeduct", HpRate());
			}
			float num2 = HpRate();
			if (num2 < Singleton<StageBattleComponent>.instance.leastHpRate)
			{
				Singleton<StageBattleComponent>.instance.leastHpRate = num2;
			}
			if ((int)hp > 0 || Singleton<BattleProperty>.instance.isInGod)
			{
				return;
			}
			if (Singleton<BattleProperty>.instance.godTimeCount > 0)
			{
				hp = 1;
				Singleton<EventManager>.instance.Invoke("Battle/OnHpRateChanged");
				Singleton<BattleProperty>.instance.isInGod = true;
				MissHardEffect(Singleton<BattleProperty>.instance.godTime);
				AttackEffectManager.instance.zombiaSkill.SetActive(true);
				AttackEffectManager.instance.zombiaSkillEffect.SetActive(true);
				Singleton<BattleProperty>.instance.godTimeCount = 0;
				m_Coroutine1 = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					if ((bool)AttackEffectManager.instance && (bool)AttackEffectManager.instance.zombiaSkill)
					{
						AttackEffectManager.instance.zombiaSkill.SetActive(false);
					}
					Singleton<BattleProperty>.instance.isInGod = false;
					m_Coroutine2 = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
					{
						if ((bool)AttackEffectManager.instance && (bool)AttackEffectManager.instance.zombiaSkill && (bool)AttackEffectManager.instance.zombiaSkillEffect)
						{
							AttackEffectManager.instance.zombiaSkillEffect.SetActive(false);
						}
					}, 0.5f);
				}, Singleton<BattleProperty>.instance.godTime);
			}
			else
			{
				Dead();
			}
		}

		public void MissHardEffect(float duration)
		{
			if (SingletonMonoBehaviour<GirlManager>.instance.hurtCoroutine != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(SingletonMonoBehaviour<GirlManager>.instance.hurtCoroutine);
			}
			if (duration == Singleton<BattleProperty>.instance.godTime)
			{
				if (m_ZombiaCoroutine != null)
				{
					SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_ZombiaCoroutine);
				}
				GirlActionController.instance.animator.Play("char_zombie_invincible", 1, 0f);
			}
			else
			{
				if (m_HurtEffectCoroutine != null)
				{
					SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_HurtEffectCoroutine);
				}
				GirlActionController.instance.animator.Play("char_invincible", 1, 0f);
			}
			if (duration == Singleton<BattleProperty>.instance.godTime)
			{
				m_ZombiaCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					if ((bool)GirlActionController.instance && (bool)GirlActionController.instance.animator)
					{
						GirlActionController.instance.animator.Play("char_zombie_invincible_end", 1, 0f);
					}
				}, duration - 3f);
				return;
			}
			m_HurtEffectCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				if ((bool)GirlActionController.instance && (bool)GirlActionController.instance.animator)
				{
					GirlActionController.instance.animator.Play("empty", 1, 0f);
				}
			}, duration);
		}

		public float HpRate()
		{
			return 1f * (float)(int)hp / (float)GetHpMax();
		}

		public int GetHp()
		{
			return hp;
		}

		public int GetHpMax()
		{
			return Singleton<BattleProperty>.instance.maxHp;
		}

		public void Hurt(int hurtValue, bool isAir)
		{
			if (!Singleton<StageBattleComponent>.instance.isDead)
			{
				GameGlobal.gGameMissPlay.SetMissHardTime(Singleton<BattleProperty>.instance.missHardTime);
				Singleton<TaskStageTarget>.instance.AddMiss(1);
				if (!Singleton<StageBattleComponent>.instance.isDead)
				{
					Singleton<EventManager>.instance.Invoke("Battle/OnCharacterHurt");
				}
				hurt = hurtValue;
				AddHp(hurtValue);
				if (!Singleton<BattleProperty>.instance.isInGod)
				{
					Singleton<EventManager>.instance.Invoke((!isAir) ? "Battle/OnNoteMiss" : "Battle/OnNoteMissAir", hurtValue);
				}
				GameGlobal.gGameTouchPlay.DisMissHardTime();
			}
		}

		public int GetHurtValue()
		{
			return hurt;
		}

		public bool IsDead()
		{
			return Singleton<StageBattleComponent>.instance.isDead;
		}

		public void Dead()
		{
			Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false);
			Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, true);
			Singleton<StageBattleComponent>.instance.Dead();
		}

		public void Revive(bool ifFirstLoad = false)
		{
			Singleton<StageBattleComponent>.instance.Resume();
			AddHp(GetHpMax() - (int)hp);
		}

		private void CalCritical(MusicData md, int result)
		{
			if (!md.isLongPressing && (!md.isMul || Singleton<BattleEnemyManager>.instance.GetPlayResult(md.objId) <= 1))
			{
				float num = 0f;
				if (!FeverManager.Instance.IsOnFeverState())
				{
					float num2 = (result != 3) ? 1f : 0.5f;
					num = Mathf.RoundToInt(num2 * (float)md.noteData.fever);
				}
				if (num > 0f)
				{
					FeverManager.Instance.AddFever(num);
				}
			}
		}

		private int CalScore(MusicData md, int result)
		{
			bool flag = FeverManager.Instance.IsOnFeverState();
			if (md.isLongPressing)
			{
				return 10;
			}
			if (md.isMul && Singleton<BattleEnemyManager>.instance.GetPlayResult(md.objId) > 1)
			{
				MultHitEnemyController multHitEnemyController = GameGlobal.gGameMusicScene.objCtrls[md.objId] as MultHitEnemyController;
				if ((bool)multHitEnemyController && multHitEnemyController.isOver)
				{
					return 0;
				}
				return 20;
			}
			float num = md.noteData.score;
			int combo = Singleton<StageBattleComponent>.instance.GetCombo();
			float num2 = Mathf.Min((float)Mathf.FloorToInt((float)combo / 100f / 0.1f) * 0.1f, Singleton<BattleProperty>.instance.comboRate) + 1f;
			if (num2 - 1f >= Singleton<BattleProperty>.instance.comboRate && (bool)AttackEffectManager.instance.jokerSkillEffect)
			{
				AttackEffectManager.instance.jokerSkillEffect.SetActive(true);
				AttackEffectManager.instance.jokerEndSkillEffect.SetActive(false);
			}
			float num3 = (result != 3) ? Singleton<BattleProperty>.instance.perfectScoreExtra : 0.5f;
			if (flag)
			{
				num3 += 0.5f;
			}
			float num4 = 1f;
			float num5 = 1f;
			float num6 = 1f;
			float num7 = 1f;
			if (FeverManager.Instance.IsOnFeverState())
			{
				num4 = Singleton<BattleProperty>.instance.feverScoreRate;
			}
			if (md.noteData.boss_action != "0" && !string.IsNullOrEmpty(md.noteData.boss_action))
			{
				num5 = Singleton<BattleProperty>.instance.bossAttackScoreRate;
			}
			if (md.noteData.type == 4)
			{
				num6 = Singleton<BattleProperty>.instance.hideNoteRate;
			}
			num7 = Singleton<BattleProperty>.instance.scoreExtraRate;
			float f = num * num2 * (num3 + num7 + num4 + num5 + num6 - 4f);
			return Mathf.CeilToInt(f);
		}

		public void AttackScore(int idx, int result, TimeNodeOrder tno)
		{
			MusicData md = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
			bool flag = md.doubleIdx > 0;
			bool flag2 = md.isAir;
			bool isPunchBegan = GameGlobal.gGameTouchPlay.isPunchBegan;
			if (md.noteData.pathway == 0 && SingletonMonoBehaviour<GirlManager>.instance.IsAir() && !isPunchBegan && !md.isLongPressType && !md.isMul && md.noteData.type != 5)
			{
				return;
			}
			CalCritical(md, result);
			int value = CalScore(md, result);
			if (md.isLongPressing)
			{
				MusicData musicData = Singleton<StageBattleComponent>.instance.GetMusicData().Find((MusicData m) => m.configData.time == md.longPressPTick && m.isAir == md.isAir && m.configData.length > 0m);
				if (musicData.longPressNum <= musicData.longPressCount - 1)
				{
					musicData.longPressNum++;
					Singleton<StageBattleComponent>.instance.SetMusicData(musicData);
					Singleton<TaskStageTarget>.instance.AddScore(value, musicData.objId, md.noteData.ibms_id, md.isAir);
				}
			}
			else
			{
				Singleton<TaskStageTarget>.instance.AddScore(value, md.objId, md.noteData.ibms_id, md.isAir);
			}
			if (md.isLongPressing || md.isMul || md.noteData.type == 6 || md.noteData.type == 7 || md.noteData.type == 9 || md.noteData.type == 10 || md.noteData.type == 11)
			{
				return;
			}
			if ((md.noteData.type == 5 || md.noteData.type == 8) && md.noteData.boss_action != "0" && !string.IsNullOrEmpty(md.noteData.boss_action) && GameGlobal.gTouch.IsJumpTouch())
			{
				flag2 = true;
			}
			state = 0;
			if (tno != null)
			{
				decimal d = (decimal)GameGlobal.gTouch.tickTime - tno.md.tick;
				if (d > 0.025m)
				{
					if (m_IsShowLateEarly)
					{
						state = 1;
					}
					late++;
				}
				else if (d < -0.025m)
				{
					if (m_IsShowLateEarly)
					{
						state = -1;
					}
					early++;
				}
			}
			if (FeverManager.Instance.IsOnFeverState())
			{
				string uid = (result != 4) ? "Battle/OnDoubleNoteGoldGreatHit" : "Battle/OnDoubleNoteGoldPerfectHit";
				if (!flag)
				{
					uid = ((result == 4) ? ((!flag2) ? "Battle/OnNoteGoldPerfectHit" : "Battle/OnNoteGoldPerfectAirHit") : ((!flag2) ? "Battle/OnNoteGoldGreatHit" : "Battle/OnNoteGoldGreatAirHit"));
				}
				Singleton<EventManager>.instance.Invoke(uid);
				return;
			}
			switch (result)
			{
			case 3:
				Singleton<EventManager>.instance.Invoke(flag ? "Battle/OnDoubleNoteGreatHit" : ((!flag2) ? "Battle/OnNoteGreatHit" : "Battle/OnNoteGreatAirHit"));
				break;
			case 4:
				Singleton<EventManager>.instance.Invoke(flag ? "Battle/OnDoubleNotePerfectHit" : ((!flag2) ? "Battle/OnNotePerfectHit" : "Battle/OnNotePerfectAirHit"));
				break;
			}
		}
	}
}
