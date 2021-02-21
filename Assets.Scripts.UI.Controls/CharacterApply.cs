using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.UI.Controls
{
	public class CharacterApply : SerializedMonoBehaviour
	{
		[CustomValueDrawer("OnAnimNameGUI")]
		public string characterAnimation;

		public string characterSound;

		[SerializeField]
		[HideInInspector]
		private Animator m_Animator;

		private int m_Index;

		private AudioSource m_AudioSource;

		private Coroutine m_EndCoroutine;

		private bool m_IsNekoPlayFinish;

		private void Awake()
		{
			Singleton<EventManager>.instance.RegEvent("UI/OnCharacterApply").trigger += OnCharacterApply;
			Singleton<EventManager>.instance.RegEvent("UI/OnRoleSelected").trigger += StopAudioSource;
			m_Animator = GetComponent<Animator>();
			string cmpValue = base.name.Replace("(Clone)", string.Empty);
			m_Index = Singleton<ConfigManager>.instance.GetConfigIndex("character", "mainShow", cmpValue);
			if (m_Index == -1)
			{
				m_Index = Singleton<ConfigManager>.instance.GetConfigIndex("character", "mainShow", base.transform.parent.name.Replace("(Clone)", string.Empty));
			}
		}

		private void Start()
		{
			Singleton<EventManager>.instance.RegEvent("UI/OnNekoSkillAvailable").trigger += OnNekoSkillAvailable;
		}

		private void StopAudioSource(object sender, object reciever, params object[] args)
		{
			if ((bool)m_AudioSource)
			{
				m_AudioSource.Stop();
			}
		}

		private void OnDestroy()
		{
			Singleton<EventManager>.instance.RegEvent("UI/OnCharacterApply").trigger -= OnCharacterApply;
			Singleton<EventManager>.instance.RegEvent("UI/OnRoleSelected").trigger -= StopAudioSource;
			if (m_Index == 16)
			{
				Singleton<EventManager>.instance.RegEvent("UI/OnNekoSkillAvailable").trigger -= OnNekoSkillAvailable;
			}
		}

		private void OnEnable()
		{
			m_Animator.SetLayerWeight(1, 0f);
			SingletonMonoBehaviour<CharacterExpression>.instance.bubbleNekoAnimator.gameObject.SetActive(false);
		}

		private void OnDisable()
		{
			if (m_EndCoroutine != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_EndCoroutine);
				m_EndCoroutine = null;
			}
			StopAudioSource(null, null);
			m_Animator.SetLayerWeight(1, 0f);
		}

		private void OnCharacterApply(object sender, object reciever, params object[] args)
		{
			if (m_Index != Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>())
			{
				return;
			}
			if (m_EndCoroutine != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_EndCoroutine);
				m_EndCoroutine = null;
			}
			m_Animator.SetLayerWeight(1, 1f);
			m_Animator.Play(characterAnimation, 1, 0f);
			string clipName = string.Format("{0}_End", characterAnimation.Replace("_Start", string.Empty));
			AudioClip clip = Singleton<AssetBundleManager>.instance.LoadFromName<AudioClip>(characterSound);
			AnimationClip endClip = m_Animator.runtimeAnimatorController.animationClips.Find((AnimationClip a) => a.name == clipName);
			m_AudioSource = Singleton<AudioManager>.instance.PlayOneShot(clip, Singleton<DataManager>.instance["GameConfig"]["VoiceVolume"].GetResult<float>(), delegate
			{
				if (!m_IsNekoPlayFinish)
				{
					m_AudioSource = null;
					if ((bool)m_Animator)
					{
						m_Animator.Play(clipName, 1, 0f);
						m_EndCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
						{
							if ((bool)m_Animator)
							{
								m_Animator.SetLayerWeight(1, 0f);
							}
							m_EndCoroutine = null;
						}, endClip.length);
					}
				}
			});
		}

		public void OnNekoSkillAvailable(object sender, object reciever, params object[] args)
		{
			if (m_Index != 16)
			{
				return;
			}
			m_Animator.SetLayerWeight(1, 1f);
			m_Animator.Play("touch_shy_Start", 1, 0f);
			string clipName = "touch_shy_End";
			AudioClip clip = Singleton<AssetBundleManager>.instance.LoadFromName<AudioClip>("char_neko_easter_eggs_bgm");
			AnimationClip endClip = m_Animator.runtimeAnimatorController.animationClips.Find((AnimationClip a) => a.name == clipName);
			SingletonMonoBehaviour<CharacterExpression>.instance.bubbleNekoAnimator.gameObject.SetActive(true);
			SingletonMonoBehaviour<CharacterExpression>.instance.bubbleNekoAnimator.Play("TalkBubbleStart", 0, 0f);
			m_IsNekoPlayFinish = true;
			m_AudioSource = Singleton<AudioManager>.instance.PlayOneShot(clip, Singleton<DataManager>.instance["GameConfig"]["VoiceVolume"].GetResult<float>(), delegate
			{
				m_AudioSource = null;
				if ((bool)m_Animator)
				{
					m_Animator.Play(clipName, 1, 0f);
					m_EndCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
					{
						m_Animator.SetLayerWeight(1, 0f);
						m_EndCoroutine = null;
					}, endClip.length);
				}
				SingletonMonoBehaviour<CharacterExpression>.instance.bubbleNekoAnimator.Play("TalkBubbleEnd", 0, 0f);
				m_IsNekoPlayFinish = false;
			});
		}
	}
}
