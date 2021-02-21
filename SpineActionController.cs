using Assets.Scripts.GameCore.GameObjectLogics.GameObjectManager;
using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.Graphics;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using DG.Tweening;
using FormulaBase;
using GameLogic;
using Spine;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SpineActionController : MonoBehaviour
{
	public static Type[] TYPE_POLL;

	public BaseSpineObjectController objController;

	private int protectLevel;

	private string currentActionName;

	private SpineEventFactory eventFactory;

	public SkeletonAnimation skAnimation;

	private Dictionary<string, Bone> bones;

	private string[] skAnimationsName;

	private static Assembly assembly = Assembly.Load("Assembly-CSharp");

	private List<GameObject> synchroObjects;

	private List<Animator> m_SyncAnimators;

	private List<SpineActionController> m_SyncSpineActionControllers;

	[SerializeField]
	public SkeletActionData[] actionData;

	public int actionMode;

	public float startDelay;

	public float duration;

	public float scale = 1f;

	public float lengthRate;

	public GameObject rendererPreb;

	public GameObject destroyEffect;

	public GameObject clipEffect;

	public GameObject catchObj;

	public float rotateRuration = 1f;

	private Material m_Mtrl;

	private Renderer m_Renderer;

	private decimal m_Length;

	private int m_Idx;

	private static bool m_InitFlag;

	public static List<ParticleSystem> groundClipParticles;

	public static List<ParticleSystem> airClipParticles;

	private bool m_HasDestroy;

	private bool m_HasAlpha;

	private MusicData m_MusicData;

	private SpriteRenderer m_StartStar;

	private SpriteRenderer m_EndStar;

	private UnityGameManager m_UnityGameManger;

	private static Dictionary<Transform, GameObject> m_Stars = new Dictionary<Transform, GameObject>();

	public static List<Tweener> starTweeners = new List<Tweener>();

	private static Coroutine m_JumpUpCoroutine;

	private static Coroutine m_JumpDownCoroutine;

	private float m_StarSpeed;

	private string m_StartName;

	private bool m_HasClip;

	public string curActionKey
	{
		get;
		private set;
	}

	public static SpineActionController curPlpSac
	{
		get
		{
			int idx = LongPressController.GetIdx(false);
			if (idx >= 0 && idx < GameMusicScene.instance.spineActionCtrls.Length)
			{
				return GameMusicScene.instance.spineActionCtrls[idx];
			}
			return null;
		}
	}

	public static SpineActionController curJlpSac
	{
		get
		{
			int idx = LongPressController.GetIdx(true);
			if (idx >= 0 && idx < GameMusicScene.instance.spineActionCtrls.Length)
			{
				return GameMusicScene.instance.spineActionCtrls[idx];
			}
			return null;
		}
	}

	public static List<ParticleSystem> curParticles
	{
		get;
		private set;
	}

	public static List<ParticleSystem> curAirParticles
	{
		get;
		private set;
	}

	public static void ReleaseReferences()
	{
		curParticles = null;
		curAirParticles = null;
	}

	public static void InitTypePoll()
	{
		if (TYPE_POLL == null || TYPE_POLL.Length <= 0)
		{
			TYPE_POLL = new Type[EditorData.Instance.SpineModeName.Length];
			for (int i = 0; i < EditorData.Instance.SpineModeName.Length; i++)
			{
				string name = EditorData.Instance.SpineModeName[i];
				TYPE_POLL[i] = assembly.GetType(name);
			}
		}
	}

	private void Awake()
	{
		m_UnityGameManger = SingletonMonoBehaviour<UnityGameManager>.instance;
		InitEffect();
	}

	private void OnDestroy()
	{
		groundClipParticles = null;
		airClipParticles = null;
		m_InitFlag = false;
		if (m_UnityGameManger != null)
		{
			m_UnityGameManger.UnregLoop("LongPress" + m_MusicData.objId);
		}
	}

	private void Start()
	{
	}

	private void InitEffect()
	{
		if (!m_InitFlag && (bool)clipEffect)
		{
			destroyEffect = UnityEngine.Object.Instantiate(destroyEffect);
			destroyEffect.SetActive(false);
			rendererPreb = UnityEngine.Object.Instantiate(rendererPreb);
			rendererPreb.SetActive(false);
			m_InitFlag = true;
			starTweeners = new List<Tweener>();
			m_Stars = new Dictionary<Transform, GameObject>();
			curActionKey = string.Empty;
		}
	}

	public void Init(int idx)
	{
		m_Idx = idx;
		if (eventFactory == null)
		{
			eventFactory = base.gameObject.AddComponent<SpineEventFactory>();
		}
		InitTypePoll();
		eventFactory.SetIdx(idx);
		if (objController == null)
		{
			Type componentType = TYPE_POLL[actionMode];
			objController = (BaseSpineObjectController)base.gameObject.AddComponent(componentType);
		}
		objController.SetIdx(idx);
		objController.Init();
		SkeletonAnimation component = base.gameObject.GetComponent<SkeletonAnimation>();
		if (component != null && component.skeleton != null)
		{
			bones = new Dictionary<string, Bone>();
			int i = 0;
			for (int count = component.skeleton.Bones.Count; i < count; i++)
			{
				string name = component.skeleton.Bones.Items[i].Data.Name;
				bones[name] = component.skeleton.Bones.Items[i];
			}
		}
		SpineSynchroObjects component2 = base.gameObject.GetComponent<SpineSynchroObjects>();
		if (component2 != null)
		{
			SetSynchroObjects(component2.synchroObjects);
		}
		m_MusicData = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(idx);
		if (!m_MusicData.isLongPressStart)
		{
			return;
		}
		SetLength(m_MusicData.configData.length);
		SpriteRenderer[] componentsInChildren = base.gameObject.transform.GetComponentsInChildren<SpriteRenderer>(true);
		if (!m_StartStar)
		{
			int num = componentsInChildren.ToList().FindIndex((SpriteRenderer s) => s.gameObject.name.Contains("_top"));
			if (num != -1)
			{
				m_StartStar = componentsInChildren[num];
				m_StartStar.gameObject.SetActive(false);
				m_Stars.Add(base.transform, m_StartStar.gameObject);
				if (Singleton<BattleProperty>.instance.isCatchAvailable)
				{
					catchObj = UnityEngine.Object.Instantiate(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>("Catch"), m_StartStar.transform.parent);
					Color color = (!m_MusicData.isAir) ? GameGlobal.gGameMusicScene.groundCatchCircleColor : GameGlobal.gGameMusicScene.airCatchCircleColor;
					Color color2 = (!m_MusicData.isAir) ? GameGlobal.gGameMusicScene.groundCatchCircle2Color : GameGlobal.gGameMusicScene.airCatchCircle2Color;
					Sprite sprite = (!m_MusicData.isAir) ? GameGlobal.gGameMusicScene.catchGroundSprite : GameGlobal.gGameMusicScene.catchAirSprite;
					catchObj.transform.Find("CatchCircle").localScale = new Vector3(1.7f, 1.7f, 1f);
					catchObj.transform.Find("CatchCircle2").localScale = new Vector3(2.5f, 2.5f, 1f);
					catchObj.transform.Find("CatchCircle").GetComponent<SpriteRenderer>().color = color;
					catchObj.transform.Find("CatchCircle2").GetComponent<SpriteRenderer>().color = color2;
					catchObj.transform.Find("CatchCircle").GetComponent<SpriteRenderer>().material = GameGlobal.gGameMusicScene.catchMaterial;
					catchObj.transform.Find("CatchCircle2").GetComponent<SpriteRenderer>().material = GameGlobal.gGameMusicScene.catchMaterial;
					catchObj.transform.Find("CatchCircle").GetComponent<SpriteRenderer>().sprite = sprite;
					catchObj.transform.Find("CatchCircle2").GetComponent<SpriteRenderer>().sprite = sprite;
					if (!GameGlobal.gGameMusic.catchNotes.Contains(m_MusicData.objId))
					{
						catchObj.SetActive(false);
					}
					if (!GameGlobal.gGameMusic.catchHeadNotes.Contains(m_MusicData.objId))
					{
						catchObj.transform.Find("CatchCircle2").gameObject.SetActive(false);
					}
				}
			}
		}
		if (!m_EndStar)
		{
			int num2 = componentsInChildren.ToList().FindIndex((SpriteRenderer s) => s.gameObject.name.Contains("_end"));
			if (num2 != -1)
			{
				m_EndStar = componentsInChildren[num2];
				m_EndStar.gameObject.SetActive(false);
			}
		}
	}

	public bool IsLongPressDestroy()
	{
		return m_HasDestroy;
	}

	public bool IsLongPressAlpha()
	{
		return m_HasAlpha;
	}

	public void Pause()
	{
		if ((bool)m_Mtrl)
		{
			m_StarSpeed = m_Mtrl.GetFloat("_Speed");
			m_Mtrl.SetFloat("_Speed", 0f);
		}
	}

	public void Resume()
	{
		if ((bool)m_Mtrl)
		{
			m_Mtrl.SetFloat("_Speed", m_StarSpeed);
		}
	}

	public void Clip(float percent, uint result)
	{
		if (m_HasAlpha)
		{
			return;
		}
		m_HasClip = true;
		MusicData musicDataByIdx = Singleton<StageBattleComponent>.instance.GetMusicDataByIdx(m_Idx);
		if ((bool)m_StartStar)
		{
			if ((bool)destroyEffect && m_StartName != "Fucked")
			{
				if (m_MusicData.isAir)
				{
					if (!AttackEffectManager.instance.longPressEffectAir)
					{
						AttackEffectManager.instance.longPressEffectAir = UnityEngine.Object.Instantiate(destroyEffect, AttackEffectManager.instance.transform);
						GameObject longPressEffectAir = AttackEffectManager.instance.longPressEffectAir;
						Transform transform = longPressEffectAir.transform;
						Vector3 position = longPressEffectAir.transform.position;
						float x = position.x;
						Vector3 position2 = longPressEffectAir.transform.position;
						transform.position = new Vector3(x, 1.15f, position2.z);
						AttackEffectManager.instance.longPressEffectAirParticle = longPressEffectAir.GetComponent<ParticleSystem>();
						if (!longPressEffectAir.activeInHierarchy)
						{
							longPressEffectAir.SetActive(true);
						}
					}
					else
					{
						AttackEffectManager.instance.longPressEffectAirParticle.Play(true);
					}
				}
				else if (!AttackEffectManager.instance.longPressEffectGround)
				{
					AttackEffectManager.instance.longPressEffectGround = UnityEngine.Object.Instantiate(destroyEffect, AttackEffectManager.instance.transform);
					GameObject longPressEffectGround = AttackEffectManager.instance.longPressEffectGround;
					AttackEffectManager.instance.longPressEffectGroundParticle = longPressEffectGround.GetComponent<ParticleSystem>();
					if (!longPressEffectGround.activeInHierarchy)
					{
						longPressEffectGround.SetActive(true);
					}
				}
				else
				{
					AttackEffectManager.instance.longPressEffectGroundParticle.Play(true);
				}
				AttacksController.Instance.PlayOneShot(m_MusicData.noteData.key_audio, AttacksController.KeyAudioType.Others);
			}
			m_StartStar.transform.parent = SingletonMonoBehaviour<SceneObjectController>.instance.transform;
			m_StartName = "Fucked";
			if ((bool)catchObj)
			{
				catchObj.SetActive(false);
			}
			Transform transform2 = m_StartStar.transform;
			float y;
			if (musicDataByIdx.isAir)
			{
				y = 1.15f;
			}
			else
			{
				Vector3 position3 = m_StartStar.transform.position;
				y = position3.y;
			}
			Vector3 position4 = m_StartStar.transform.position;
			transform2.position = new Vector3(-3.88f, y, position4.z);
		}
		if (percent >= 1f && (bool)m_EndStar && (bool)destroyEffect && m_EndStar.gameObject.activeSelf)
		{
			if (Singleton<BattleEnemyManager>.instance.GetPlayResult(m_Idx) == 0)
			{
				Singleton<TaskStageTarget>.instance.AddLongPressFinishCount(1);
			}
			DestroyLongPress();
		}
		m_Mtrl.SetFloat("_ClipValue", percent);
	}

	public void StopStar()
	{
		if ((bool)m_StartStar)
		{
			m_StartStar.transform.parent = base.transform.GetChild(0).GetChild(0);
		}
	}

	public static void StopStars()
	{
		foreach (KeyValuePair<Transform, GameObject> star in m_Stars)
		{
			star.Value.transform.parent = star.Key.GetChild(0).GetChild(0);
		}
	}

	public void SetAlpha(float alpha)
	{
		StopStar();
		if (m_HasAlpha)
		{
			return;
		}
		m_HasAlpha = true;
		if ((bool)m_Mtrl)
		{
			DOTween.To(() => m_Mtrl.GetFloat("_Alpha"), delegate(float a)
			{
				m_Mtrl.SetFloat("_Alpha", a);
			}, alpha, 0.2f);
		}
		if ((bool)m_StartStar)
		{
			DOTween.To(() => m_StartStar.color, delegate(Color a)
			{
				m_StartStar.color = a;
			}, new Color(1f, 1f, 1f, alpha), 0.2f);
		}
		if ((bool)m_EndStar)
		{
			DOTween.To(() => m_EndStar.color, delegate(Color a)
			{
				m_EndStar.color = a;
			}, new Color(1f, 1f, 1f, alpha), 0.2f);
		}
	}

	public void DestroyLongPress()
	{
		if (m_HasDestroy || !m_HasClip)
		{
			return;
		}
		m_HasDestroy = true;
		Singleton<BattleEnemyManager>.instance.SetLongPressEffect(false, m_MusicData.isAir);
		StopStar();
		m_StartStar.gameObject.SetActive(false);
		m_EndStar.gameObject.SetActive(false);
		m_Renderer.gameObject.SetActive(false);
		if (m_MusicData.isAir)
		{
			if (!AttackEffectManager.instance.longPressEffectEndAir)
			{
				AttackEffectManager.instance.longPressEffectEndAir = UnityEngine.Object.Instantiate(destroyEffect, AttackEffectManager.instance.transform);
				GameObject longPressEffectEndAir = AttackEffectManager.instance.longPressEffectEndAir;
				Transform transform = longPressEffectEndAir.transform;
				Vector3 position = longPressEffectEndAir.transform.position;
				float x = position.x;
				Vector3 position2 = longPressEffectEndAir.transform.position;
				transform.position = new Vector3(x, 1.15f, position2.z);
				AttackEffectManager.instance.longPressEffectEndAirParticle = longPressEffectEndAir.GetComponent<ParticleSystem>();
				if (!longPressEffectEndAir.activeInHierarchy)
				{
					longPressEffectEndAir.SetActive(true);
				}
			}
			else
			{
				AttackEffectManager.instance.longPressEffectEndAirParticle.Play(true);
			}
		}
		else if (!AttackEffectManager.instance.longPressEffectEndGround)
		{
			AttackEffectManager.instance.longPressEffectEndGround = UnityEngine.Object.Instantiate(destroyEffect, AttackEffectManager.instance.transform);
			GameObject longPressEffectEndGround = AttackEffectManager.instance.longPressEffectEndGround;
			AttackEffectManager.instance.longPressEffectEndGroundParticle = longPressEffectEndGround.GetComponent<ParticleSystem>();
			if (!longPressEffectEndGround.activeInHierarchy)
			{
				longPressEffectEndGround.SetActive(true);
			}
		}
		else
		{
			AttackEffectManager.instance.longPressEffectEndGroundParticle.Play(true);
		}
		AttacksController.Instance.PlayOneShot(m_MusicData.noteData.key_audio, AttacksController.KeyAudioType.Others);
	}

	public static void PlayLongPressEffect(bool isTo, bool isAir = false)
	{
		SpineActionController x = (!isAir) ? curPlpSac : curJlpSac;
		if (x != null)
		{
			if (isAir)
			{
				curAirParticles = airClipParticles;
				foreach (ParticleSystem curAirParticle in curAirParticles)
				{
					ParticleSystem.EmissionModule emission = curAirParticle.emission;
					emission.enabled = isTo;
				}
			}
			else
			{
				curParticles = groundClipParticles;
				foreach (ParticleSystem curParticle in curParticles)
				{
					ParticleSystem.EmissionModule emission2 = curParticle.emission;
					emission2.enabled = isTo;
				}
			}
		}
		else
		{
			List<ParticleSystem> list = (!isAir) ? curParticles : curAirParticles;
			if (list != null)
			{
				foreach (ParticleSystem item in list)
				{
					ParticleSystem.EmissionModule emission3 = item.emission;
					if (item.emission.enabled)
					{
						emission3.enabled = isTo;
					}
				}
			}
		}
		if (!isTo)
		{
			StopStars();
		}
		if (!Singleton<StageBattleComponent>.instance.isDead)
		{
			bool isAirPressing = Singleton<BattleEnemyManager>.instance.isAirPressing;
			bool isGroundPressing = Singleton<BattleEnemyManager>.instance.isGroundPressing;
			bool flag = SingletonMonoBehaviour<GirlManager>.instance.IsJumpingAction();
			bool isCharacterGroundHitting = Singleton<BattleEnemyManager>.instance.isCharacterGroundHitting;
			bool isCharacterAirHitting = Singleton<BattleEnemyManager>.instance.isCharacterAirHitting;
			bool flag2 = SingletonMonoBehaviour<GirlManager>.instance.IsAir();
			if (isTo)
			{
				if (isAir)
				{
					if (isGroundPressing)
					{
						Play("char_up_press_s2b", -1);
					}
					else if (isCharacterGroundHitting)
					{
						Play("char_up_press_s", -1);
					}
					else if (!flag2)
					{
						Play("char_uppress_start", -1);
						Play("char_uppress", -1, 0f, false);
					}
					else
					{
						Play("char_uppress", -1);
					}
				}
				else if (isAirPressing)
				{
					Play("char_down_press_s2b", -1);
				}
				else if (isCharacterAirHitting)
				{
					Play("char_down_press_s", -1);
				}
				else
				{
					if (flag)
					{
						Play("char_downpress", -1);
					}
					Play("char_press", -1, 0f, false);
				}
			}
			else if (isAir)
			{
				if (isGroundPressing)
				{
					Play("char_down_press_b2s", -1);
				}
				else
				{
					if (Singleton<SkillManager>.instance.characterSkill.uid == "Character_14")
					{
						GirlActionController.isPressHitNote = false;
					}
					if (!GirlActionController.isPressHitNote)
					{
						Play(SingletonMonoBehaviour<GirlManager>.instance.IsJumpingAction() ? "char_uppress_end" : "char_down_press_b2s", -1);
						PlaySkeleton("char_run", -1);
					}
					else
					{
						GirlActionController.isPressHitNote = true;
						Play(GirlActionController.pressHitNoteActKey, -1);
					}
				}
			}
			else if (isAirPressing)
			{
				Play("char_up_press_b2s", -1);
			}
			else
			{
				Play("char_run", -1);
			}
		}
		if (!isTo && GirlActionController.instance.ghostMtrl.GetFloat("_Distortion") != 1f)
		{
			GirlActionController.instance.GhostDisappear(0f);
		}
		if (isAir)
		{
			Singleton<BattleEnemyManager>.instance.isAirPressing = isTo;
		}
		else
		{
			Singleton<BattleEnemyManager>.instance.isGroundPressing = isTo;
		}
		if ((bool)SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs)
		{
			if (isTo)
			{
				SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs.loop = true;
				SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs.Play();
			}
			else if (!Singleton<BattleEnemyManager>.instance.isAirPressing && !Singleton<BattleEnemyManager>.instance.isGroundPressing)
			{
				SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs.loop = false;
				SingletonMonoBehaviour<GirlManager>.instance.girlEffectAs.Stop();
			}
		}
		if (!isTo)
		{
			if (!Singleton<BattleEnemyManager>.instance.isGroundPressing && !Singleton<BattleEnemyManager>.instance.isAirPressing)
			{
				Singleton<BattleProperty>.instance.isHpChangable = true;
			}
		}
		else
		{
			Singleton<BattleProperty>.instance.isHpChangable = false;
		}
	}

	public void SetLength(decimal time)
	{
		m_Length = time * (decimal)lengthRate * 1.0746m;
		GameObject gameObject = UnityEngine.Object.Instantiate(rendererPreb, base.transform);
		gameObject.SetActive(false);
		gameObject.transform.localScale = new Vector3((float)m_Length, 1f, 1f);
		m_Renderer = gameObject.GetComponent<Renderer>();
		m_Mtrl = m_Renderer.material;
		float @float = m_Mtrl.GetFloat("_Length");
		m_Mtrl.SetFloat("_LengthRatio", (float)m_Length);
		GameObject gameObject2 = base.transform.GetChild(0).GetChild(0).gameObject;
		Tweener item = gameObject2.transform.GetChild(0).DOLocalRotate(new Vector3(0f, 0f, 360f), rotateRuration, DG.Tweening.RotateMode.LocalAxisAdd).SetEase(Ease.Linear)
			.SetLoops(-1);
		gameObject2.transform.localScale = new Vector3(1f / (float)m_Length, 1f, 1f);
		gameObject2.transform.GetChild(0).gameObject.SetActive(false);
		GameObject gameObject3 = UnityEngine.Object.Instantiate(gameObject2, gameObject2.transform.parent);
		gameObject3.transform.GetChild(0).gameObject.SetActive(false);
		Tweener item2 = gameObject3.transform.transform.GetChild(0).DOLocalRotate(new Vector3(0f, 0f, 360f), rotateRuration, DG.Tweening.RotateMode.LocalAxisAdd).SetEase(Ease.Linear)
			.SetLoops(-1);
		gameObject3.transform.localPosition = gameObject2.transform.localPosition + Vector3.right * @float;
		gameObject3.transform.GetChild(0).name = gameObject3.transform.GetChild(0).name.Replace("top", "end");
		starTweeners.Add(item);
		starTweeners.Add(item2);
		if (groundClipParticles == null && !m_MusicData.isAir)
		{
			GameObject gameObject4 = UnityEngine.Object.Instantiate(clipEffect);
			gameObject4.transform.SetParent(AttackEffectManager.instance.transform);
			gameObject4.SetActive(true);
			groundClipParticles = gameObject4.GetComponentsInChildren<ParticleSystem>().ToList();
			for (int i = 0; i < groundClipParticles.Count; i++)
			{
				ParticleSystem.EmissionModule emission = groundClipParticles[i].emission;
				emission.enabled = false;
			}
		}
		if (airClipParticles == null && m_MusicData.isAir)
		{
			GameObject gameObject5 = UnityEngine.Object.Instantiate(clipEffect);
			gameObject5.transform.SetParent(AttackEffectManager.instance.transform);
			Transform transform = gameObject5.transform;
			Vector3 position = gameObject5.transform.position;
			float x = position.x;
			Vector3 position2 = gameObject5.transform.position;
			transform.position = new Vector3(x, 1.15f, position2.z);
			gameObject5.SetActive(true);
			airClipParticles = gameObject5.GetComponentsInChildren<ParticleSystem>().ToList();
			for (int j = 0; j < airClipParticles.Count; j++)
			{
				ParticleSystem.EmissionModule emission2 = airClipParticles[j].emission;
				emission2.enabled = false;
			}
		}
	}

	private void Update()
	{
		if ((bool)m_Renderer && (bool)m_Mtrl)
		{
			m_Renderer.material = m_Mtrl;
		}
	}

	public void OnControllerStart()
	{
		if (objController == null)
		{
			return;
		}
		if (actionMode == 12)
		{
			if (!m_Renderer)
			{
				base.gameObject.SetActive(true);
				return;
			}
			base.gameObject.SetActive(false);
			for (int i = 0; i < base.transform.childCount; i++)
			{
				Transform child = base.transform.GetChild(i);
				child.gameObject.SetActive(true);
			}
			float num = (float)GameSceneMainController.curResolutionWidth * 1f / (float)GameSceneMainController.curResolutionHeight / 1.77777779f;
			float num2 = 13.333333f * num;
			float startPos = 10f;
			float num3 = 3.87f;
			float num4 = startPos + num3;
			float num5 = num4 / (float)m_MusicData.dt;
			float endPos = (0f - num2) / 2f - 8.8f * (float)m_Length * 1.075f;
			float totalTime = (startPos - endPos) / num5;
			string id = "LongPress" + m_MusicData.objId;
			decimal showTick = m_MusicData.tick - m_MusicData.dt;
			m_UnityGameManger.RegLoop(id, delegate
			{
				if (!Singleton<StageBattleComponent>.instance.isPause)
				{
					float num6 = (float)(Singleton<StageBattleComponent>.instance.timeFromMusicStartDecimal - showTick);
					float num7 = num6 / totalTime;
					if (num7 >= 0f)
					{
						base.gameObject.SetActive(true);
						float x = Mathf.Lerp(startPos, endPos, num7);
						Transform transform = base.transform;
						float y;
						if (m_MusicData.isAir)
						{
							y = -0.1f;
						}
						else
						{
							Vector3 position = base.transform.position;
							y = position.y;
						}
						Vector3 position2 = base.transform.position;
						transform.position = new Vector3(x, y, position2.z);
						if ((double)num7 > 1.0)
						{
							base.gameObject.SetActive(false);
							m_UnityGameManger.UnregLoop(id);
						}
					}
					else
					{
						base.gameObject.SetActive(false);
					}
				}
			}, (!GraphicSettings.isOverOneHundred) ? UnityGameManager.LoopType.FixedUpdate : UnityGameManager.LoopType.Update);
			if ((bool)m_StartStar)
			{
				m_StartStar.gameObject.SetActive(true);
			}
			if ((bool)m_EndStar)
			{
				m_EndStar.gameObject.SetActive(true);
			}
		}
		else
		{
			objController.OnControllerStart();
		}
	}

	private SkeletonAnimation GetSkeletonAnimation()
	{
		if (skAnimation != null)
		{
			return skAnimation;
		}
		skAnimation = base.gameObject.GetComponent<SkeletonAnimation>();
		if (skAnimation == null)
		{
			Debug.Log("Animation is null");
		}
		else
		{
			skAnimation.Initialize(false);
		}
		InitAnimationsName();
		return skAnimation;
	}

	private void ClearTracks()
	{
		if (!(skAnimation == null) && skAnimation.state != null)
		{
			skAnimation.state.ClearTracks();
		}
	}

	private void InitAnimationsName()
	{
		if (skAnimationsName != null || skAnimation == null || skAnimation.skeletonDataAsset == null)
		{
			return;
		}
		ExposedList<Spine.Animation> animations = skAnimation.skeletonDataAsset.GetAnimationStateData().SkeletonData.Animations;
		skAnimationsName = new string[animations.Count];
		for (int i = 0; i < animations.Count; i++)
		{
			Spine.Animation animation = animations.Items[i];
			if (animation != null)
			{
				skAnimationsName[i] = animation.Name;
			}
		}
	}

	public static void PlayAnimator(string actionKey, int idx, int layer = 0, float tick = 0f, bool isOverride = true)
	{
		object obj;
		switch (idx)
		{
		case -1:
			obj = GirlActionController.instance.animator;
			break;
		case -2:
			obj = Boss.Instance.animator;
			break;
		default:
			obj = ((idx >= GameMusicScene.instance.animators.Length) ? null : GameMusicScene.instance.animators[idx]);
			break;
		}
		Animator animator = (Animator)obj;
		if (!animator)
		{
			return;
		}
		Animator animator2 = animator;
		if (!isOverride && animator2.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			return;
		}
		animator2.enabled = true;
		if (!animator2.HasState(0, Animator.StringToHash(actionKey)))
		{
			return;
		}
		animator2.speed = 1f;
		if (tick > 0f)
		{
			AnimationClip animationClip = animator2.runtimeAnimatorController.animationClips.Find((AnimationClip a) => a.name == actionKey);
			if (animationClip != null)
			{
				float normalizedTime = tick / animationClip.length;
				animator2.Play(actionKey, layer, normalizedTime);
			}
		}
		else
		{
			animator2.Play(actionKey, layer, 0f);
		}
	}

	private static void PlayAnimator(string actionKey, GameObject obj, Animator animator = null)
	{
		if (!animator)
		{
			if (obj == GirlActionController.instance.go)
			{
				animator = GirlActionController.instance.animator;
			}
			else if (obj == Boss.Instance.go)
			{
				animator = Boss.Instance.animator;
			}
		}
		if (obj == null)
		{
			return;
		}
		if (!animator)
		{
			int num = GameMusicScene.instance.preloads.IndexOf(obj);
			animator = ((num == -1) ? obj.GetComponent<Animator>() : GameMusicScene.instance.animators[num]);
			if (animator == null)
			{
				return;
			}
		}
		if (animator.HasState(0, Animator.StringToHash(actionKey)))
		{
			animator.speed = 1f;
			animator.Play(actionKey, 0, 0f);
		}
	}

	public static void Play(string actionKey, GameObject obj, float tick = 0f, SpineActionController sac = null, Animator animator = null, bool isOverride = true)
	{
		PlayAnimator(actionKey, obj, animator);
		if (obj == null)
		{
			return;
		}
		if (!sac)
		{
			if (obj == GirlActionController.instance.go)
			{
				sac = GirlActionController.instance.spineActionCtrl;
			}
			else if (obj == Boss.Instance.go)
			{
				sac = Boss.Instance.spineActionController;
			}
		}
		if (sac == null)
		{
			int num = GameMusicScene.instance.preloads.FindIndex((GameObject g) => g == obj);
			sac = ((num == -1) ? obj.GetComponent<SpineActionController>() : GameMusicScene.instance.spineActionCtrls[num]);
			if (sac == null)
			{
				return;
			}
		}
		if (sac.actionData == null || sac.actionData.Length == 0)
		{
			return;
		}
		sac.PlayByKey(actionKey, isOverride);
		if (tick > 0f)
		{
			sac.SetCurrentAnimationTime(tick);
		}
		if (sac.synchroObjects != null && sac.synchroObjects.Count > 0)
		{
			for (int i = 0; i < sac.synchroObjects.Count; i++)
			{
				Play(actionKey, sac.synchroObjects[i], tick, sac.m_SyncSpineActionControllers[i], sac.m_SyncAnimators[i], isOverride);
			}
		}
	}

	public static void PlaySkeletonAnim(string actionKey, int idx, bool isLoop = false, bool isOverride = true, float delay = 0f, float time = -1f)
	{
		object obj;
		switch (idx)
		{
		case -1:
			obj = GirlActionController.instance.spineActionCtrl;
			break;
		case -2:
			obj = Boss.Instance.spineActionController;
			break;
		default:
			obj = ((idx >= GameMusicScene.instance.spineActionCtrls.Length) ? null : GameMusicScene.instance.spineActionCtrls[idx]);
			break;
		}
		SpineActionController spineActionController = (SpineActionController)obj;
		if ((bool)spineActionController)
		{
			if (!(spineActionController.curActionKey == "multi_atk_48_end") || !(actionKey == "boss_far_atk_2_start"))
			{
				spineActionController.curActionKey = actionKey;
			}
			if (isOverride)
			{
				spineActionController.skAnimation.state.ClearTracks();
			}
			if (isOverride)
			{
				spineActionController.skAnimation.state.SetAnimation(0, actionKey, isLoop);
			}
			else
			{
				spineActionController.skAnimation.state.AddAnimation(0, actionKey, isLoop, delay);
			}
			if (time > 0f)
			{
				spineActionController.SetCurrentAnimationTime(time);
			}
		}
	}

	public static void PlaySkeleton(string actionKey, int idx)
	{
		object obj;
		switch (idx)
		{
		case -1:
			obj = GirlActionController.instance.spineActionCtrl;
			break;
		case -2:
			obj = Boss.Instance.spineActionController;
			break;
		default:
			obj = ((idx >= GameMusicScene.instance.spineActionCtrls.Length) ? null : GameMusicScene.instance.spineActionCtrls[idx]);
			break;
		}
		SpineActionController spineActionController = (SpineActionController)obj;
		if ((bool)spineActionController && spineActionController.actionData != null && spineActionController.actionData.Length != 0)
		{
			spineActionController.PlayByKey(actionKey);
		}
	}

	public static void Play(string actionKey, int idx, float tick = 0f, bool isOverride = true)
	{
		if (actionKey == "char_downhit" && SingletonMonoBehaviour<GirlManager>.instance.animator.GetCurrentAnimatorStateInfo(0).IsName("char_downhit"))
		{
			return;
		}
		object obj;
		switch (idx)
		{
		case -1:
			obj = GirlActionController.instance.spineActionCtrl;
			break;
		case -2:
			obj = Boss.Instance.spineActionController;
			break;
		default:
			obj = ((idx >= GameMusicScene.instance.spineActionCtrls.Length) ? null : GameMusicScene.instance.spineActionCtrls[idx]);
			break;
		}
		SpineActionController spineActionController = (SpineActionController)obj;
		if (!spineActionController || spineActionController.actionData == null || spineActionController.actionData.Length == 0)
		{
			return;
		}
		if (!(spineActionController.curActionKey == "multi_atk_48_end") || !(actionKey == "boss_far_atk_2_start"))
		{
			spineActionController.curActionKey = actionKey;
		}
		PlayAnimator(actionKey, idx, 0, tick, isOverride);
		spineActionController.PlayByKey(actionKey, isOverride);
		if (tick > 0f)
		{
			spineActionController.SetCurrentAnimationTime(tick);
		}
		if (spineActionController.synchroObjects != null && spineActionController.synchroObjects.Count > 0)
		{
			for (int i = 0; i < spineActionController.synchroObjects.Count; i++)
			{
				Play(actionKey, spineActionController.synchroObjects[i], tick, spineActionController.m_SyncSpineActionControllers[i], spineActionController.m_SyncAnimators[i], isOverride);
			}
		}
		if (idx != -1 || string.IsNullOrEmpty(actionKey))
		{
			return;
		}
		GirlActionController.instance.isThisFrameJumpUp = (actionKey == "char_jump");
		GirlActionController.instance.isThisFrameJumpDown = (actionKey == "char_downhit");
		if (GirlActionController.instance.isThisFrameJumpUp)
		{
			if (m_JumpUpCoroutine != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_JumpUpCoroutine);
			}
			m_JumpUpCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				GirlActionController.instance.isThisFrameJumpUp = false;
			}, 0.1f);
		}
		if (GirlActionController.instance.isThisFrameJumpDown)
		{
			if (m_JumpDownCoroutine != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_JumpDownCoroutine);
			}
			m_JumpDownCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				GirlActionController.instance.isThisFrameJumpDown = false;
			}, 0.1f);
		}
	}

	public static string CurrentAnimationName(int idx)
	{
		object obj;
		switch (idx)
		{
		case -1:
			obj = GirlActionController.instance.spineActionCtrl;
			break;
		case -2:
			obj = Boss.Instance.spineActionController;
			break;
		default:
			obj = ((idx >= GameMusicScene.instance.spineActionCtrls.Length) ? null : GameMusicScene.instance.spineActionCtrls[idx]);
			break;
		}
		SpineActionController spineActionController = (SpineActionController)obj;
		if ((bool)spineActionController)
		{
			return spineActionController.GetCurrentAnimationName();
		}
		return null;
	}

	public static Vector3 GetBoneRealPosition(string boneName, GameObject obj)
	{
		if (obj == null)
		{
			return Vector3.zero;
		}
		SpineActionController component = obj.GetComponent<SpineActionController>();
		if (component == null)
		{
			return Vector3.zero;
		}
		if (component.bones == null || !component.bones.ContainsKey(boneName))
		{
			return Vector3.zero;
		}
		Bone bone = component.bones[boneName];
		if (bone == null)
		{
			return Vector3.zero;
		}
		float worldX;
		float worldY;
		bone.LocalToWorld(bone.X, bone.Y, out worldX, out worldY);
		return new Vector3(worldX, worldY, 0f);
	}

	public static void SetSynchroObjectsActive(GameObject obj, bool val)
	{
		if (obj == null)
		{
			return;
		}
		SpineActionController component = obj.GetComponent<SpineActionController>();
		if (component == null || component.synchroObjects == null)
		{
			return;
		}
		foreach (GameObject synchroObject in component.synchroObjects)
		{
			synchroObject.SetActive(val);
		}
	}

	private void SetCurrentAnimationTime(float tick)
	{
		if (!(skAnimation == null) && skAnimation.state != null)
		{
			skAnimation.state.GetCurrent(0).TrackTime = tick;
		}
	}

	public float GetCurrentAnimationTime()
	{
		if (skAnimation == null)
		{
			return 0f;
		}
		if (skAnimation.state != null)
		{
			return skAnimation.state.GetCurrent(0).TrackTime;
		}
		return 0f;
	}

	private TrackEntry AddAnimation(string n, bool isLoop, float delay)
	{
		if (n == null)
		{
			return null;
		}
		if (skAnimation == null || skAnimation.state == null)
		{
			return null;
		}
		return skAnimation.state.AddAnimation(0, n, isLoop, delay);
	}

	public TrackEntry SetAnimation(string n, bool isLoop)
	{
		if (n == null)
		{
			return null;
		}
		if (skAnimation == null || skAnimation.state == null)
		{
			return null;
		}
		return skAnimation.state.SetAnimation(0, n, isLoop);
	}

	public void PlayByKey(string actionKey, bool isOverride = true)
	{
		int num = 0;
		SkeletActionData data;
		while (true)
		{
			if (num < actionData.Length)
			{
				data = GetData(num);
				if (data.name == actionKey)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		PlayByData(data, isOverride);
	}

	public void PlayByData(SkeletActionData data, bool isOverride = true)
	{
		if (GetSkeletonAnimation() == null || skAnimationsName == null || CheckActionProtect(data))
		{
			return;
		}
		if (skAnimation.state.Tracks.Count > 0)
		{
			ClearTracks();
		}
		currentActionName = data.name;
		protectLevel = data.protectLevel;
		if (data.isRandomSequence)
		{
			int num = UnityEngine.Random.Range(0, data.actionIdx.Length);
			int idx = data.actionEventIdx[num];
			string n = data.actionIdx[num];
			Spine.AnimationState.TrackEntryDelegate function = SpineEventFactory.GetFunction(base.gameObject, idx);
			TrackEntry trackEntry = (!isOverride) ? AddAnimation(n, data.isEndLoop, 0f) : SetAnimation(n, data.isEndLoop);
			if (trackEntry != null && function != null)
			{
				trackEntry.Complete += function;
			}
			return;
		}
		int num2 = data.actionIdx.Length - 1;
		for (int i = 0; i < data.actionIdx.Length; i++)
		{
			string n2 = data.actionIdx[i];
			bool isLoop = data.isEndLoop && i >= num2;
			TrackEntry trackEntry2 = (!isOverride) ? AddAnimation(n2, isLoop, 0f) : SetAnimation(n2, isLoop);
			if (trackEntry2 != null)
			{
				int idx2 = data.actionEventIdx[i];
				Spine.AnimationState.TrackEntryDelegate function2 = SpineEventFactory.GetFunction(base.gameObject, idx2);
				if (function2 != null)
				{
					trackEntry2.Complete += function2;
				}
			}
		}
	}

	public int GetProtectLevel()
	{
		return protectLevel;
	}

	public void SetProtectLevel(int level)
	{
		protectLevel = level;
	}

	public void SetCurrentActionName(string name)
	{
		currentActionName = name;
	}

	public string GetCurrentActionName()
	{
		return currentActionName;
	}

	public void SetSynchroObjects(List<GameObject> objs)
	{
		if (synchroObjects != null)
		{
			return;
		}
		synchroObjects = objs;
		if (synchroObjects == null || synchroObjects.Count <= 0)
		{
			return;
		}
		m_SyncSpineActionControllers = new List<SpineActionController>();
		m_SyncAnimators = new List<Animator>();
		for (int i = 0; i < synchroObjects.Count; i++)
		{
			GameObject gameObject = synchroObjects[i];
			SpineActionController component = gameObject.GetComponent<SpineActionController>();
			if (!(component == null))
			{
				gameObject = UnityEngine.Object.Instantiate(gameObject);
				gameObject.transform.parent = base.gameObject.transform.parent;
				component = gameObject.GetComponent<SpineActionController>();
				component.Init(-1);
				synchroObjects[i] = gameObject;
				m_SyncSpineActionControllers.Add(component);
				m_SyncAnimators.Add(gameObject.GetComponent<Animator>());
			}
		}
	}

	public string GetCurrentAnimationName()
	{
		if (GetSkeletonAnimation() == null)
		{
			return null;
		}
		return skAnimation.AnimationName;
	}

	private bool CheckActionProtect(SkeletActionData data)
	{
		if (skAnimation == null)
		{
			return true;
		}
		if (data.isSelfProtect && currentActionName == data.name)
		{
			return true;
		}
		return protectLevel > data.protectLevel;
	}

	public void AddData(SkeletActionData d)
	{
		List<SkeletActionData> list = (actionData == null || actionData.Length <= 0) ? new List<SkeletActionData>() : actionData.ToList();
		list.Add(d);
		actionData = list.ToArray();
	}

	public void DelData(int idx)
	{
		if (actionData != null && actionData.Length > idx)
		{
			List<SkeletActionData> list = actionData.ToList();
			list.RemoveAt(idx);
			actionData = list.ToArray();
		}
	}

	public void SetData(int idx, SkeletActionData d)
	{
		if (actionData != null && actionData.Length > idx)
		{
			d.name = EditorData.Instance.SpineActionKeys[d.spineActionKeyIndex];
			actionData[idx] = d;
		}
	}

	public SkeletActionData GetData(int idx)
	{
		return actionData[idx];
	}

	public int DataCount()
	{
		if (actionData == null)
		{
			return 0;
		}
		return actionData.Length;
	}
}
