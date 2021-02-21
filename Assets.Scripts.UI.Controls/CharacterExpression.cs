using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.GeneralLocalization;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Controls
{
	public class CharacterExpression : SingletonMonoBehaviour<CharacterExpression>
	{
		[Serializable]
		public class Expression
		{
			public string animName;

			public string[] audioNames;

			[NonSerialized]
			public List<string> texts;

			public float weight;
		}

		private static List<Expression> m_Expressions = new List<Expression>();

		private int m_CharacterIdx = -1;

		private string m_Language;

		public Transform characterPivot;

		public Text text;

		public Animator bubbleAnimator;

		public Animator bubbleNekoAnimator;

		private Coroutine m_Coroutine;

		private Coroutine m_EndCoroutine;

		private CoroutineManager m_CoroutineManager;

		private AudioSource m_AudioSource;

		private bool m_IsFinishAnim;

		[InfoBox("用于测试在NS平台上的角色互动提示", InfoMessageType.Info, null)]
		public string tipExpressionAnimName = "touch_confident";

		public float tipExpressionTime = 3f;

		public GameObject tipExpressionContent;

		private bool m_TipExpressionAnimFinish = true;

		private bool m_IsTriggerNekoMainHidden;

		private int m_NekoMainHiddenNum;

		private void Awake()
		{
			GetComponent<Button>().onClick.AddListener(RandomExpress);
			m_CoroutineManager = SingletonMonoBehaviour<CoroutineManager>.instance;
		}

		private void Start()
		{
			bool flag = false;
			string result = Singleton<DataManager>.instance["Account"]["Language"].GetResult<string>();
			if (Singleton<BulletinManager>.instance.bulletins != null && Singleton<BulletinManager>.instance.bulletins.ContainsKey(result))
			{
				List<BulletinManager.Bulletin> list = Singleton<BulletinManager>.instance.bulletins[result];
				if (list.Exists((BulletinManager.Bulletin b) => b.isNew && b.force))
				{
					flag = true;
				}
			}
			if (Singleton<DataManager>.instance["Account"]["Exp"].GetResult<int>() != 0 && !flag)
			{
				TipExpression();
			}
		}

		public void RandomExpress()
		{
			Expression expression = RandomExpression();
			Express(expression);
		}

		public void Express(int index, int soundIndex)
		{
			RefreshExpressions();
			Express(m_Expressions[index], soundIndex);
		}

		public void Express(Expression expression, int soundIndex = -1)
		{
			if (Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>() != 16)
			{
				m_NekoMainHiddenNum = 0;
				m_IsTriggerNekoMainHidden = false;
			}
			if (m_IsTriggerNekoMainHidden && m_IsFinishAnim)
			{
				if (m_NekoMainHiddenNum > 0)
				{
					m_NekoMainHiddenNum--;
					ShowText.ShowInfo(Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "cytusIINekoBan"));
					GetComponent<Button>().interactable = false;
					SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
					{
						GetComponent<Button>().interactable = true;
					}, 1.1f);
					return;
				}
				m_NekoMainHiddenNum = 0;
				m_IsTriggerNekoMainHidden = false;
			}
			if (((bool)m_AudioSource && m_AudioSource.gameObject.activeSelf) || !m_IsFinishAnim)
			{
				return;
			}
			m_IsFinishAnim = false;
			Animator animator = characterPivot.GetComponentInChildren<Animator>();
			if (animator == null)
			{
				return;
			}
			animator.SetLayerWeight(1, 1f);
			animator.Play($"{expression.animName}_Start", 1);
			bubbleAnimator.gameObject.SetActive(true);
			bubbleAnimator.Play("TalkBubbleStart", 0, 0f);
			string txt = (soundIndex != -1) ? expression.texts[soundIndex] : expression.texts.Random();
			text.text = txt;
			if ((expression.animName == "touch_shy" || expression.animName == "touch_angry") && !m_IsTriggerNekoMainHidden)
			{
				if (Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>() == 16)
				{
					if (expression.texts.FindIndex((string p) => p == txt) == 0)
					{
						m_IsTriggerNekoMainHidden = true;
						m_NekoMainHiddenNum = 2;
					}
				}
				else
				{
					m_IsTriggerNekoMainHidden = false;
					m_NekoMainHiddenNum = 0;
				}
			}
			AudioClip audioClip = Singleton<AssetBundleManager>.instance.LoadFromName<AudioClip>(expression.audioNames[expression.texts.IndexOf(txt)]);
			string endClipName = $"{expression.animName}_End";
			AnimationClip endClip = animator.runtimeAnimatorController.animationClips.Find((AnimationClip a) => a.name == endClipName);
			m_AudioSource = Singleton<AudioManager>.instance.PlayOneShot(audioClip, Singleton<DataManager>.instance["GameConfig"]["VoiceVolume"].GetResult<float>());
			m_Coroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				m_AudioSource = null;
				animator.Play(endClipName, 1);
				bubbleAnimator.Play("TalkBubbleEnd", 0, 0f);
				m_EndCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					m_IsFinishAnim = true;
					animator.SetLayerWeight(1, 0f);
				}, endClip.length);
			}, audioClip.length);
		}

		private void OnEnable()
		{
			m_IsFinishAnim = true;
			Animator componentInChildren = characterPivot.GetComponentInChildren<Animator>();
			if ((bool)componentInChildren)
			{
				componentInChildren.Play("BgmStandby", 0, 0f);
			}
			if ((bool)bubbleAnimator)
			{
				bubbleAnimator.gameObject.SetActive(false);
			}
		}

		private void OnDisable()
		{
			if ((bool)m_AudioSource)
			{
				m_AudioSource.Stop();
				m_AudioSource = null;
			}
			if ((bool)m_CoroutineManager && m_Coroutine != null)
			{
				m_CoroutineManager.StopCoroutine(m_Coroutine);
			}
			if ((bool)m_CoroutineManager && m_EndCoroutine != null)
			{
				m_CoroutineManager.StopCoroutine(m_EndCoroutine);
			}
		}

		private Expression RandomExpression()
		{
			RefreshExpressions();
			int num = 1000;
			List<int> list = new List<int>();
			List<float> list2 = m_Expressions.Select((Expression e) => e.weight);
			for (int i = 0; i < list2.Count; i++)
			{
				float num2 = list2[i];
				for (int j = 0; (float)j < (float)num * num2; j++)
				{
					list.Add(i);
				}
			}
			return m_Expressions[list.Random()];
		}

		private void RefreshExpressions()
		{
			int result = Singleton<DataManager>.instance["Account"]["SelectedRoleIndex"].GetResult<int>();
			string activeOption = SingletonScriptableObject<LocalizationSettings>.instance.GetActiveOption("Language");
			if (m_CharacterIdx == result && !(m_Language != activeOption))
			{
				return;
			}
			m_Language = activeOption;
			m_CharacterIdx = result;
			m_Expressions = JsonUtils.Deserialize<List<Expression>>(Singleton<ConfigManager>.instance.GetJson("character", false)[m_CharacterIdx]["expressions"].ToString());
			JToken jToken = Singleton<ConfigManager>.instance["character"][m_CharacterIdx]["expressions"];
			for (int i = 0; i < m_Expressions.Count; i++)
			{
				Expression expression = m_Expressions[i];
				JToken jToken2 = jToken[i];
				List<string> list = new List<string>();
				for (int j = 0; j < jToken2.Count(); j++)
				{
					list.Add(jToken2[j].ToString());
				}
				expression.texts = list;
			}
		}

		[Button]
		public void TipExpression()
		{
			if (Singleton<DataManager>.instance["Account"]["TipExpression"].GetResult<bool>() || ((bool)m_AudioSource && m_AudioSource.gameObject.activeSelf) || !m_IsFinishAnim)
			{
				return;
			}
			Singleton<DataManager>.instance["Account"]["TipExpression"].SetResult(true);
			Singleton<DataManager>.instance.Save();
			m_IsFinishAnim = false;
			tipExpressionContent.GetComponent<Button>().interactable = true;
			Animator animator = characterPivot.GetComponentInChildren<Animator>();
			m_CoroutineManager.StartCoroutine(delegate
			{
				animator.SetLayerWeight(1, 1f);
				animator.Play($"{tipExpressionAnimName}_Start", 1);
				m_TipExpressionAnimFinish = false;
				bubbleAnimator.gameObject.SetActive(true);
				bubbleAnimator.Play("TalkBubbleStart", 0, 0f);
				text.text = string.Empty;
				tipExpressionContent.SetActive(true);
				m_Coroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					string stateName = $"{tipExpressionAnimName}_End";
					if ((bool)animator)
					{
						animator.Play(stateName, 1);
					}
					m_EndCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
					{
						if ((bool)animator)
						{
							animator.SetLayerWeight(1, 0f);
						}
						m_TipExpressionAnimFinish = true;
					}, 0.5f);
				}, tipExpressionTime);
			}, () => animator = characterPivot.GetComponentInChildren<Animator>());
		}

		public void ColseTipExpression()
		{
			bool flag = Input.GetButtonDown("Space") || Singleton<InputManager>.instance.RewiredGetButtonDown("Space");
			Animator animator = characterPivot.GetComponentInChildren<Animator>();
			if (flag)
			{
				bubbleAnimator.gameObject.SetActive(true);
				bubbleAnimator.Play("TalkBubbleStart", 0, 0f);
				tipExpressionContent.SetActive(false);
				Expression expression = RandomExpression();
				if (!m_TipExpressionAnimFinish)
				{
					m_CoroutineManager.StopAllCoroutines();
				}
				animator.SetLayerWeight(1, 1f);
				animator.Play($"{expression.animName}_Start", 1);
				string item = expression.texts.Random();
				text.text = item;
				AudioClip audioClip = Singleton<AssetBundleManager>.instance.LoadFromName<AudioClip>(expression.audioNames[expression.texts.IndexOf(item)]);
				string endClipName = $"{expression.animName}_End";
				AnimationClip endClip = animator.runtimeAnimatorController.animationClips.Find((AnimationClip a) => a.name == endClipName);
				m_AudioSource = Singleton<AudioManager>.instance.PlayOneShot(audioClip, Singleton<DataManager>.instance["GameConfig"]["VoiceVolume"].GetResult<float>());
				m_Coroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					m_AudioSource = null;
					animator.Play(endClipName, 1);
					bubbleAnimator.Play("TalkBubbleEnd", 0, 0f);
					m_EndCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
					{
						m_IsFinishAnim = true;
						animator.SetLayerWeight(1, 0f);
					}, endClip.length);
				}, audioClip.length);
			}
			else
			{
				string stateName = $"{tipExpressionAnimName}_End";
				animator.Play(stateName, 1);
				bubbleAnimator.Play("TalkBubbleEnd", 0, 0f);
				m_EndCoroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
				{
					m_IsFinishAnim = true;
					animator.SetLayerWeight(1, 0f);
					tipExpressionContent.SetActive(false);
				}, 0.5f);
			}
		}
	}
}
