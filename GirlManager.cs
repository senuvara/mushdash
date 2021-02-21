using Assets.Scripts.GameCore.GameObjectLogics.GameObjectManager;
using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using DYUnityLib;
using FormulaBase;
using GameLogic;
using Spine;
using Spine.Unity;
using System.Collections;
using UnityEngine;

public class GirlManager : SingletonMonoBehaviour<GirlManager>
{
	private bool m_IsJumpingAction;

	private GirlActionController m_CurGirlActionController;

	private Animator m_CurAnimator;

	private SpineActionController m_Sac;

	public AudioSource girlEffectAs;

	public Coroutine hurtCoroutine;

	public GameObject girl;

	public GameObject girlGhost;

	public float playWaitForComeOut;

	public bool isCommingOut = true;

	private GameObject m_VictoryGirl;

	private UnityGameManager m_UnityGameManager;

	public GameObject forward;

	private bool m_IsAir;

	public Animator animator => m_CurAnimator;

	public static void ReleaseReferences()
	{
		SingletonMonoBehaviour<GirlManager>.instance.m_CurGirlActionController = null;
		SingletonMonoBehaviour<GirlManager>.instance.m_CurAnimator = null;
		SingletonMonoBehaviour<GirlManager>.instance.m_Sac = null;
		SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs = null;
		SingletonMonoBehaviour<GirlManager>.instance.girl = null;
		SingletonMonoBehaviour<GirlManager>.instance.girlGhost = null;
		SingletonMonoBehaviour<GirlManager>.instance.m_VictoryGirl = null;
	}

	private void Start()
	{
		m_UnityGameManager = SingletonMonoBehaviour<UnityGameManager>.instance;
		m_UnityGameManager.RegLoop("GirlManager", delegate
		{
			if (GameGlobal.stopwatch.IsRunning && !FixUpdateTimer.IsPausing() && FeverManager.Instance.IsOnFeverState())
			{
				ReduceFeverEnergyPerFixedTime();
			}
		}, UnityGameManager.LoopType.Update);
		SetJumpingAction(false);
		SetTone(false);
		Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false);
		Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, true);
		if (bool.Parse(SingletonScriptableObject<ConstanceManager>.instance["IsEditorMode"]))
		{
			forward.SetActive(false);
		}
	}

	private void OnDisable()
	{
		if (m_UnityGameManager != null)
		{
			m_UnityGameManager.UnregLoop("GirlManager");
		}
	}

	private void ReduceFeverEnergyPerFixedTime()
	{
		float value = Time.deltaTime / Singleton<BattleProperty>.instance.feverTime * (0f - Singleton<BattleProperty>.instance.maxFever);
		FeverManager.Instance.AddFever(value);
	}

	public void Reset()
	{
		m_IsJumpingAction = false;
		m_IsAir = false;
		int result = Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>();
		string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("character", result, "battleShow");
		m_VictoryGirl = Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(Singleton<ConfigManager>.instance.GetConfigStringValue("character", result, "victoryShow"));
		girlGhost = Singleton<StageBattleComponent>.instance.AddObj(configStringValue + "_ghost");
		girlGhost.SetActive(false);
		girl = Singleton<StageBattleComponent>.instance.AddObj(configStringValue);
		if (girl != null)
		{
			girl.transform.SetParent(base.gameObject.transform, false);
			SpineActionController component = girl.GetComponent<SpineActionController>();
			component.Init(-1);
			SpineActionController.SetSynchroObjectsActive(girl, false);
			m_CurGirlActionController = girl.GetComponent<GirlActionController>();
			m_CurAnimator = girl.GetComponent<Animator>();
			m_Sac = girl.GetComponent<SpineActionController>();
			AttackEffectManager.instance.SetEffectByCharact(result);
		}
	}

	public void ComeOut()
	{
		isCommingOut = true;
		StartCoroutine(AfterComeOut());
		StartCoroutine(ComeOutFinished());
	}

	private IEnumerator ComeOutFinished()
	{
		yield return new WaitForSeconds(playWaitForComeOut);
		isCommingOut = false;
	}

	private IEnumerator AfterComeOut()
	{
		yield return new WaitForSeconds(0.1f);
		girl.SetActive(true);
		if (bool.Parse(SingletonScriptableObject<ConstanceManager>.instance["IsEditorMode"]))
		{
			forward.SetActive(true);
		}
		m_CurGirlActionController.OnControllerStart();
		Singleton<EventManager>.instance.Invoke("Battle/OnStageStart");
		Singleton<EventManager>.instance.Invoke("UI/EnableTouch");
	}

	public void UnLockActionProtect()
	{
		if (m_Sac != null)
		{
			m_Sac.SetProtectLevel(0);
			m_Sac.SetCurrentActionName(null);
		}
	}

	public void AttacksWithoutExchange(uint result, string actKey = null, int id = -1)
	{
		m_CurGirlActionController.AttackQuick(actKey, result, id);
	}

	public void AttackWithExchange(uint result, string actKey = null, int id = -1)
	{
		AttacksWithoutExchange(result, actKey, id);
	}

	public void BeAttackEffect()
	{
		if (Singleton<StageBattleComponent>.instance.isDead || Singleton<BattleEnemyManager>.instance.isAirPressing || Singleton<BattleEnemyManager>.instance.isGroundPressing)
		{
			return;
		}
		if (!IsAir())
		{
			Hurt();
		}
		else
		{
			float tick = 0f;
			if (m_CurAnimator != null)
			{
				tick = m_CurAnimator.GetTime();
			}
			if (Singleton<BattleEnemyManager>.instance.isAirPressing)
			{
				SpineActionController.Play("char_uppress_end", -1, tick);
				SpineActionController.Play("char_uppress_hurt", -1, tick);
			}
			else
			{
				Hurt();
			}
		}
		if (Singleton<BattleProperty>.instance.isNekoSkillTrigger || Singleton<BattleProperty>.instance.isInGod)
		{
			return;
		}
		GirlActionController.instance.animator.Play("empty", 1, 0f);
		GirlActionController.instance.animator.Play("char_blood_hurt", 1, 0f);
		hurtCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
		{
			if ((bool)GirlActionController.instance.animator)
			{
				GirlActionController.instance.animator.Play("empty", 1, 0f);
			}
		}, Singleton<BattleProperty>.instance.missHardTime);
	}

	public void RecoveryEffect(int hp)
	{
		GameObject gameObject = Singleton<EffectManager>.instance.Play("fx_hp_ground");
		Transform transform = gameObject.transform;
		Vector3 position = gameObject.transform.position;
		float x = position.x;
		float y = (!m_IsAir) ? (-0.94f) : 0.94f;
		Vector3 position2 = gameObject.transform.position;
		transform.position = new Vector3(x, y, position2.z);
		Singleton<EventManager>.instance.Invoke((!m_IsAir) ? "Battle/OnHpGet" : "Battle/OnHpGetAir", hp);
	}

	public void StopBeAttckedEffect()
	{
	}

	public bool IsAir()
	{
		return m_IsAir;
	}

	public void SetTone(bool isAir)
	{
		m_IsAir = isAir;
	}

	public void SetJumpingAction(bool value)
	{
		m_IsJumpingAction = value;
	}

	public bool IsJumpingAction()
	{
		return m_IsJumpingAction;
	}

	public void Hurt()
	{
		if (Singleton<StageBattleComponent>.instance.isDead)
		{
			return;
		}
		SkeletonAnimation skAnimation = m_Sac.skAnimation;
		if (m_IsJumpingAction)
		{
			float trackTime = skAnimation.state.GetCurrent(0).TrackTime;
			Animator animator = GirlActionController.instance.animator;
			bool flag = animator.GetCurrentAnimatorStateInfo(0).IsName("char_jump");
			bool flag2 = Singleton<BattleEnemyManager>.instance.isGroundPressing || Singleton<BattleEnemyManager>.instance.isAirPressing;
			string animationName = flag2 ? "AirPressHurt" : ((!flag) ? "AirHitHurt" : "JumpHurt");
			if (flag2)
			{
				trackTime = 0f;
			}
			skAnimation.state.SetAnimation(0, animationName, false).Complete += delegate
			{
				Singleton<EffectManager>.instance.Play("dust_fx");
				SetJumpingAction(false);
				SetTone(false);
			};
			skAnimation.state.GetCurrent(0).TrackTime = trackTime;
			skAnimation.state.AddAnimation(0, "Run", true, 0f);
		}
		else
		{
			skAnimation.state.SetAnimation(0, "Hurt", false);
			skAnimation.state.AddAnimation(0, "Run", true, 0f);
		}
	}

	public void DestroyGirlSpineAnimation()
	{
		if ((bool)GirlActionController.instance)
		{
			SkeletonAnimation component = GirlActionController.instance.GetComponent<SkeletonAnimation>();
			component.state.ClearTracks();
		}
		Object.Destroy(m_Sac);
	}

	public Vector3 GetCurrentGirlPositon()
	{
		if (!girl)
		{
			return Vector3.zero;
		}
		return girl.transform.position;
	}

	public Vector3 GetCurrentGirlBonePosition(string boneName)
	{
		if (!m_Sac)
		{
			return Vector2.zero;
		}
		Bone bone = m_Sac.skAnimation.skeleton.FindBone(boneName);
		if (bone == null)
		{
			Debug.LogErrorFormat("No bone name {0} found.", boneName);
		}
		Vector3 result = (bone != null) ? m_Sac.skAnimation.transform.TransformPoint(new Vector3(bone.WorldX, bone.WorldY, 0f)) : Vector3.zero;
		result = new Vector3(result.x, (!m_IsAir) ? (-0.2f) : 2.2f, 0f);
		return result;
	}
}
