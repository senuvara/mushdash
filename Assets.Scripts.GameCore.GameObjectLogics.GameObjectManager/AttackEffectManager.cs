using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using FormulaBase;
using GameLogic;
using UnityEngine;

namespace Assets.Scripts.GameCore.GameObjectLogics.GameObjectManager
{
	public class AttackEffectManager : MonoBehaviour
	{
		public GameObject zombiaSkill;

		public const int COMBO_CHANGE_LIM = 20;

		private Effect[] m_Effects;

		private float[] playResultOffset;

		private GameObject pressEffect;

		private GameObject m_PlayResult;

		private MusicData m_Playmd;

		private bool m_PlayIsAir;

		private bool m_PlayIsBossNearAtk;

		public GameObject longPressEffectAir;

		public GameObject longPressEffectGround;

		public ParticleSystem longPressEffectAirParticle;

		public ParticleSystem longPressEffectGroundParticle;

		public GameObject longPressEffectEndAir;

		public GameObject longPressEffectEndGround;

		public ParticleSystem longPressEffectEndAirParticle;

		public ParticleSystem longPressEffectEndGroundParticle;

		public static AttackEffectManager instance
		{
			get;
			private set;
		}

		public GameObject elfinRecoveryEffect
		{
			get;
			private set;
		}

		public GameObject roleRecoveryEffect
		{
			get;
			private set;
		}

		public GameObject jokerSkillEffect
		{
			get;
			private set;
		}

		public GameObject jokerEndSkillEffect
		{
			get;
			private set;
		}

		public GameObject zombiaSkillEffect
		{
			get;
			private set;
		}

		public GameObject nekoSkillEffect
		{
			get;
			private set;
		}

		public GameObject switchRockEffect
		{
			get;
			private set;
		}

		public GameObject swirchSleepEffect
		{
			get;
			private set;
		}

		public GameObject sleepSkillEffect
		{
			get;
			private set;
		}

		public Effect carrotRobotSkillEffect
		{
			get;
			private set;
		}

		public Effect rampageSkillEffect
		{
			get;
			private set;
		}

		private void Awake()
		{
			instance = this;
		}

		public static void ReleaseReferences()
		{
			instance.zombiaSkill = null;
			instance.m_Effects = null;
			instance.pressEffect = null;
			instance.elfinRecoveryEffect = null;
			instance.roleRecoveryEffect = null;
			instance.jokerSkillEffect = null;
			instance.jokerEndSkillEffect = null;
			instance.zombiaSkillEffect = null;
			instance.nekoSkillEffect = null;
			instance.switchRockEffect = null;
			instance.swirchSleepEffect = null;
			instance.sleepSkillEffect = null;
			instance.carrotRobotSkillEffect = null;
			instance.rampageSkillEffect = null;
		}

		public void Reset()
		{
			switch (Singleton<DataManager>.instance["Account"]["SelectedElfinIndex"].GetResult<int>())
			{
			case 3:
				carrotRobotSkillEffect = Singleton<EffectManager>.instance.Preload("elfin_carrot_robot_fx_skill", 3);
				break;
			case 4:
				elfinRecoveryEffect = Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>("elfin_fan_robot_fx_skill"));
				elfinRecoveryEffect.SetActive(false);
				roleRecoveryEffect = Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>("elfin_fan_robot_fx_role_skill"), SingletonMonoBehaviour<GirlManager>.instance.girl.transform);
				roleRecoveryEffect.transform.localPosition = Vector3.zero;
				roleRecoveryEffect.SetActive(false);
				break;
			}
			switch (Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>())
			{
			case 1:
				rampageSkillEffect = Singleton<EffectManager>.instance.Preload("rampage_girl_fx_skill", 3);
				break;
			case 6:
				zombiaSkillEffect = Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>("zombie_girl_fx_skill"), SingletonMonoBehaviour<GirlManager>.instance.girl.transform);
				zombiaSkillEffect.transform.localPosition = Vector3.zero;
				zombiaSkillEffect.SetActive(false);
				break;
			case 7:
				jokerEndSkillEffect = Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>("joker_girl_fx_skill_end"), SingletonMonoBehaviour<GirlManager>.instance.girl.transform);
				jokerEndSkillEffect.transform.localPosition = new Vector3(0f, 1.5f, 0f);
				jokerEndSkillEffect.SetActive(false);
				jokerSkillEffect = Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>("joker_girl_fx_skill"), SingletonMonoBehaviour<GirlManager>.instance.girl.transform);
				jokerSkillEffect.transform.localPosition = new Vector3(0f, 1.5f, 0f);
				jokerSkillEffect.SetActive(false);
				break;
			case 16:
				nekoSkillEffect = Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>("neko_girl_fx_skill"), SingletonMonoBehaviour<GirlManager>.instance.girl.transform);
				nekoSkillEffect.transform.localPosition = new Vector3(0f, 1.5f, 0f);
				nekoSkillEffect.SetActive(false);
				break;
			}
			if (Singleton<StageBattleComponent>.instance.isTutorial)
			{
				switchRockEffect = Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>("fx_rock_switch"), SingletonMonoBehaviour<GirlManager>.instance.girl.transform);
				switchRockEffect.transform.localPosition = Vector3.zero;
				switchRockEffect.SetActive(false);
				swirchSleepEffect = Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>("fx_sleep_switch"), SingletonMonoBehaviour<GirlManager>.instance.girl.transform);
				swirchSleepEffect.transform.localPosition = Vector3.zero;
				swirchSleepEffect.SetActive(false);
				sleepSkillEffect = Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>("fx_sleep_skill"), SingletonMonoBehaviour<GirlManager>.instance.girl.transform);
				sleepSkillEffect.transform.localPosition = new Vector3(-0.15f, 1.3f, 0f);
				sleepSkillEffect.SetActive(false);
			}
			Singleton<EventManager>.instance.RegEvent("Battle/OnComboChanged").trigger += OnComboChanged;
			Singleton<EventManager>.instance.RegEvent("Battle/OnFail").trigger += OnFail;
		}

		private void OnFail(object sender, object reciever, params object[] args)
		{
			if ((bool)roleRecoveryEffect)
			{
				roleRecoveryEffect.SetActive(false);
			}
		}

		private void OnComboChanged(object sender, object reciever, params object[] args)
		{
			if ((bool)jokerSkillEffect && Singleton<StageBattleComponent>.instance.GetCombo() == 0 && jokerSkillEffect.activeSelf)
			{
				jokerEndSkillEffect.SetActive(true);
				jokerSkillEffect.SetActive(false);
			}
		}

		private void OnDestroy()
		{
			instance = null;
			Singleton<EventManager>.instance.RegEvent("Battle/OnComboChanged").trigger -= OnComboChanged;
			Singleton<EventManager>.instance.RegEvent("Battle/OnFail").trigger -= OnFail;
		}

		public void SetEffectByCharact(int heroIndex)
		{
			string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("boss", "scene_name", "fever", Singleton<StageBattleComponent>.instance.GetSceneName());
			Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(configStringValue));
			string uid = "girl_fx_atk_g";
			string uid2 = "girl_fx_atk_p";
			string uid3 = "girl_fx_atk_c";
			Effect effect = Singleton<EffectManager>.instance.Preload(uid, 10);
			Effect effect2 = Singleton<EffectManager>.instance.Preload(uid2, 10);
			Effect effect3 = Singleton<EffectManager>.instance.Preload(uid3, 10);
			m_Effects = new Effect[7];
			for (int i = 3; i < 7; i++)
			{
				switch (i)
				{
				case 3:
					m_Effects[i] = effect;
					break;
				case 4:
					m_Effects[i] = effect2;
					break;
				case 6:
					m_Effects[i] = effect3;
					break;
				}
			}
			InitAttackFxOffset();
		}

		private void InitAttackFxOffset()
		{
			Vector3 currentGirlPositon = SingletonMonoBehaviour<GirlManager>.instance.GetCurrentGirlPositon();
			playResultOffset = new float[7];
			for (int i = 0; i < playResultOffset.Length; i++)
			{
				uint num = (uint)i;
				Effect effect = m_Effects[num];
				if (effect != null)
				{
					GameObject sourcePrefab = effect.pool.sourcePrefab;
					if (!(sourcePrefab == null))
					{
						float[] array = playResultOffset;
						int num2 = i;
						float y = currentGirlPositon.y;
						Vector3 position = sourcePrefab.transform.position;
						array[num2] = y - position.y;
					}
				}
			}
		}

		public void ShowPlayResult(uint resultCode, int idx)
		{
			m_Playmd = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
			m_PlayIsAir = m_Playmd.isAir;
			m_PlayIsBossNearAtk = ((m_Playmd.noteData.type == 5 || m_Playmd.noteData.type == 8) && m_Playmd.noteData.boss_action != "0" && !string.IsNullOrEmpty(m_Playmd.noteData.boss_action));
			if (m_PlayIsBossNearAtk && GameGlobal.gTouch.IsJumpTouch())
			{
				m_PlayIsAir = true;
			}
			if (FeverManager.Instance.IsOnFeverState())
			{
				resultCode = 6u;
			}
			m_PlayResult = m_Effects[resultCode].CreateInstance();
			Transform transform = m_PlayResult.transform;
			Vector3 position = m_PlayResult.transform.position;
			float x = position.x;
			float y = (!m_PlayIsAir) ? (-1f) : 1.28f;
			Vector3 position2 = m_PlayResult.transform.position;
			transform.position = new Vector3(x, y, position2.z);
			if (m_Playmd.isMul)
			{
				Transform transform2 = m_PlayResult.transform;
				Vector3 position3 = m_PlayResult.transform.position;
				float x2 = position3.x;
				Vector3 position4 = m_PlayResult.transform.position;
				transform2.position = new Vector3(x2, 0f, position4.z);
			}
		}
	}
}
