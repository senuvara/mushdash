using Assets.Scripts.Common;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.GeneralLocalization;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Actions;
using Assets.Scripts.PeroTools.Nice.Events;
using DG.Tweening;
using FormulaBase;
using PeroTools.Commons;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
	public class PnlBattle : MonoBehaviour
	{
		public Slider sldProgress;

		public Toggle tglAutoPlay;

		public Animator feverAnimator;

		public Animator feverAnimatorX;

		public GameObject combo;

		public Slider sldProgress1;

		public Toggle tglAutoPlay1;

		public GameObject combo1;

		public GameObject comboGC;

		public GameObject comboGC1;

		private Tweener m_TwnProgress;

		private DG.Tweening.Sequence m_SeqGirlIn;

		private DG.Tweening.Sequence m_SeqSongPlay;

		private bool m_IsEditorMode;

		private bool m_IsiPhoneX;

		private static PnlBattle m_Instance;

		private List<DOTweenAnimation> m_PauseAnim;

		public static PnlBattle instance
		{
			get
			{
				if (!m_Instance)
				{
					m_Instance = GameUtils.FindObjectOfType<PnlBattle>();
				}
				return m_Instance;
			}
		}

		private void Awake()
		{
			m_Instance = this;
			m_IsiPhoneX = (SingletonScriptableObject<LocalizationSettings>.instance.GetActiveOption("Device") == "iPhoneX");
		}

		private void Start()
		{
			m_IsEditorMode = GameSceneMainController.isEditorMode;
			GameObject gameObject = Singleton<UIManager>.instance["UITest"];
			if (gameObject != null)
			{
				gameObject.SetActive(m_IsEditorMode);
			}
			(m_IsiPhoneX ? tglAutoPlay1 : tglAutoPlay).isOn = Singleton<StageBattleComponent>.instance.IsAutoPlay();
			if (!m_IsEditorMode)
			{
				GameStart();
			}
		}

		private void OnDestroy()
		{
			m_Instance = null;
		}

		public void Pause()
		{
			feverAnimator.enabled = false;
			feverAnimatorX.enabled = false;
			m_TwnProgress.Pause();
			m_SeqGirlIn.Pause();
			m_SeqSongPlay.Pause();
			m_PauseAnim = (from d in GameUtils.FindObjectsOfType<DOTweenAnimation>()
				where d.tween != null && d.tween.IsPlaying()
				select d).ToList();
			m_PauseAnim.For(delegate(DOTweenAnimation d)
			{
				d.tween.Pause();
			});
			if (Singleton<BattleProperty>.instance.isGCScene)
			{
				(m_IsiPhoneX ? comboGC1 : comboGC).Enable(false, false, typeof(Animator));
			}
			else
			{
				(m_IsiPhoneX ? combo1 : combo).Enable(false, false, typeof(Animator));
			}
		}

		public void Resume()
		{
			feverAnimator.enabled = true;
			feverAnimatorX.enabled = true;
			m_TwnProgress.Play();
			m_SeqGirlIn.Play();
			m_SeqSongPlay.Play();
			if (m_PauseAnim != null)
			{
				m_PauseAnim.For(delegate(DOTweenAnimation d)
				{
					d.tween.Play();
				});
			}
			if (Singleton<BattleProperty>.instance.isGCScene)
			{
				(m_IsiPhoneX ? comboGC1 : comboGC).Enable(true, false, typeof(Animator));
			}
			else
			{
				(m_IsiPhoneX ? combo1 : combo).Enable(true, false, typeof(Animator));
			}
		}

		public void OnGameStart()
		{
			m_TwnProgress = DOTween.To(() => m_IsiPhoneX ? sldProgress1.value : sldProgress.value, delegate(float value)
			{
				(m_IsiPhoneX ? sldProgress1 : sldProgress).value = value;
			}, 1f, Singleton<AudioManager>.instance.bgm.clip.length).SetEase(Ease.Linear);
			m_TwnProgress.SetAutoKill(false);
		}

		public void GameStart()
		{
			SingletonMonoBehaviour<SteamGoogleLogin>.instance.OnWebEnd();
			Singleton<StageBattleComponent>.instance.FindAllParticles();
			GcControl.Collect();
			(m_IsiPhoneX ? sldProgress1 : sldProgress).value = 0f;
			float dt = (!Singleton<StageBattleComponent>.instance.isTutorial) ? float.Parse(SingletonScriptableObject<ConstanceManager>.instance["SongDelay"]) : 0f;
			float dt2 = (!Singleton<StageBattleComponent>.instance.isTutorial) ? float.Parse(SingletonScriptableObject<ConstanceManager>.instance["GirlDelay"]) : 0f;
			m_SeqSongPlay = DOTweenUtils.Delay(delegate
			{
				if (!Singleton<StageBattleComponent>.instance.isPause)
				{
					Singleton<StageBattleComponent>.instance.GameStart(null, 0u, null);
				}
			}, dt);
			m_SeqGirlIn = DOTweenUtils.Delay(delegate
			{
				SingletonMonoBehaviour<GirlManager>.instance.ComeOut();
				Singleton<EventManager>.instance.Invoke("Battle/OnGirlIn");
			}, dt2);
		}

		public void UIPause(bool isPause)
		{
			if (!EventSystem.current || !Singleton<StageBattleComponent>.instance.isInGame || Singleton<UIManager>.instance["PnlFailText"].activeInHierarchy || Singleton<UIManager>.instance["PnlFail"].activeInHierarchy)
			{
				return;
			}
			if (!isPause)
			{
				Assets.Scripts.PeroTools.Nice.Events.Event component = Singleton<UIManager>.instance["PnlPause"].gameObject.GetComponent<Assets.Scripts.PeroTools.Nice.Events.Event>();
				if (Singleton<StageBattleComponent>.instance.isTutorial)
				{
					component = Singleton<UIManager>.instance["PnlSkipTutorial"].gameObject.GetComponent<Assets.Scripts.PeroTools.Nice.Events.Event>();
				}
				List<Popup> playables = component.GetPlayables<Popup>();
				playables.First().OnShutButtonClick();
			}
			else
			{
				if (Singleton<StageBattleComponent>.instance.isTutorial)
				{
					Singleton<EventManager>.instance.Invoke("UI/OnShowPnlSkipTutorial");
				}
				else
				{
					Singleton<EventManager>.instance.Invoke("UI/OnShowPnlPause");
				}
				Singleton<EventManager>.instance.Invoke("Battle/OnPause");
				Singleton<StageBattleComponent>.instance.Pause();
			}
		}

		public bool IsIPhoneX()
		{
			return m_IsiPhoneX;
		}

		private void Update()
		{
			if (m_IsEditorMode && Input.GetKeyDown(KeyCode.F1) && (!UITest.instance || !UITest.instance.gameObject.activeInHierarchy))
			{
				if (Singleton<StageBattleComponent>.instance.IsAutoPlay())
				{
					(m_IsiPhoneX ? tglAutoPlay1 : tglAutoPlay).isOn = false;
					Singleton<StageBattleComponent>.instance.SetAutoPlay(false);
				}
				else
				{
					(m_IsiPhoneX ? tglAutoPlay1 : tglAutoPlay).isOn = true;
					Singleton<StageBattleComponent>.instance.SetAutoPlay(true);
				}
			}
		}
	}
}
