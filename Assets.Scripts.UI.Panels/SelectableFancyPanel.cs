using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Components;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.PreWarm;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
	public class SelectableFancyPanel : SerializedMonoBehaviour, IPreWarm
	{
		public string animApply;

		public string animNoApply;

		[Required]
		public GameObject apply;

		public GameObject applyShowObject;

		[Required]
		public Button btnApply;

		[Required]
		public Button btnBackPnlMenus;

		public bool canCancleSelected;

		[Required]
		public FancyScrollView fancyScrollView;

		[Variable(0, null, false)]
		public IVariable indexData;

		public bool needExtraIndex;

		[Variable(0, null, false)]
		public IVariable extraIndexData;

		[Required]
		public GameObject inUse;

		public string itemDataName;

		public Button btnImgLocked;

		private Animator m_Anim;

		public void PreWarm(int slice)
		{
			if (slice != 0)
			{
				return;
			}
			btnApply.onClick.AddListener(OnClickApply);
			fancyScrollView.onItemIndexChange += ChangeItem;
			if (btnBackPnlMenus != null)
			{
				btnBackPnlMenus.onClick.AddListener(delegate
				{
					if (applyShowObject != null)
					{
						Image component = applyShowObject.GetComponent<Image>();
						if (component != null)
						{
							component.enabled = false;
						}
					}
				});
			}
			if (applyShowObject != null)
			{
				m_Anim = applyShowObject.GetComponent<Animator>();
			}
		}

		public void Start()
		{
			if (!(itemDataName == "character"))
			{
				return;
			}
			btnImgLocked.onClick.AddListener(delegate
			{
				if (extraIndexData.GetResult<int>() == 14)
				{
					Singleton<EventManager>.instance.Invoke("UI/OnJKCharactorTipsShow");
				}
				else if (extraIndexData.GetResult<int>() == 15)
				{
					Singleton<EventManager>.instance.Invoke("UI/OnYumeCharactorTipsShow");
				}
				else if (extraIndexData.GetResult<int>() == 16)
				{
					Singleton<EventManager>.instance.Invoke("UI/OnNekoCharactorTipsShow");
				}
				else
				{
					Singleton<EventManager>.instance.Invoke("UI/OnCharactorTipsShow");
				}
				Singleton<AudioManager>.instance.PlayOneShot("sfx_common_button", Singleton<DataManager>.instance["GameConfig"]["SfxVolume"].GetResult<float>());
			});
		}

		private void OnEnable()
		{
			if (applyShowObject != null)
			{
				Image component = applyShowObject.GetComponent<Image>();
				if (component != null)
				{
					component.enabled = true;
				}
			}
			if (!string.IsNullOrEmpty(itemDataName))
			{
				StartCoroutine(PlayScrollToAndUnLockAnims());
			}
		}

		private void OnDisable()
		{
			Singleton<EventManager>.instance.Invoke("UI/EnableTouch");
		}

		private IEnumerator PlayScrollToAndUnLockAnims()
		{
			yield return null;
			List<IData> items = Singleton<DataManager>.instance["Account"]["Items"].GetResult<List<IData>>();
			if (items == null || items.Count <= 0)
			{
				yield break;
			}
			List<int> lockIndexs = from i in items
				where i["lockNew"].GetResult<bool>() && i["type"].GetResult<string>() == itemDataName && !Singleton<ConfigManager>.instance.GetConfigBoolValue(itemDataName, i["index"].GetResult<int>(), "hide")
				select Singleton<ConfigManager>.instance.GetConfigIntValue(itemDataName, i["index"].GetResult<int>(), "order") - 1;
			if (lockIndexs != null && lockIndexs.Count > 0)
			{
				lockIndexs.Sort();
				Singleton<EventManager>.instance.Invoke("UI/DisableTouch");
				Singleton<EventManager>.instance.Invoke("UI/DisableInputKey");
				yield return new WaitForEndOfFrame();
				yield return fancyScrollView.PlayScrollTo(lockIndexs[0], 0f, false);
				yield return new WaitForSecondsRealtime(2f);
				for (int j = 1; j < lockIndexs.Count; j++)
				{
					yield return fancyScrollView.PlayScrollTo(lockIndexs[j], fancyScrollView.switchDuration, false);
					yield return new WaitForSecondsRealtime(2f);
				}
				yield return new WaitForSecondsRealtime(0.5f);
				Singleton<EventManager>.instance.Invoke("UI/EnableTouch");
				Singleton<EventManager>.instance.Invoke("UI/EnableInputKey");
				PnlNavigationBtnOption.ClearNewTip(itemDataName);
			}
		}

		private void ChangeItem(int i)
		{
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				i = ((!needExtraIndex) ? i : extraIndexData.GetResult<int>());
				if (i != indexData.GetResult<int>())
				{
					PlayNoApply();
				}
				else
				{
					PlayApply();
				}
			}, 2);
		}

		private void PlayApply()
		{
			inUse.SetActive(true);
			apply.SetActive(false);
			if (applyShowObject != null)
			{
				applyShowObject.SetActive(true);
			}
			PlayFadeIn();
		}

		private void PlayNoApply()
		{
			apply.SetActive(true);
			inUse.SetActive(false);
			if (applyShowObject != null && m_Anim != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
				{
					applyShowObject.SetActive(false);
				}, isAnimiatorFinish);
			}
			PlayFadeOut();
		}

		private bool isAnimiatorFinish()
		{
			if (m_Anim == null || !m_Anim.isActiveAndEnabled)
			{
				return true;
			}
			AnimatorStateInfo currentAnimatorStateInfo = m_Anim.GetCurrentAnimatorStateInfo(0);
			return currentAnimatorStateInfo.IsName(animNoApply) && currentAnimatorStateInfo.normalizedTime > 1f;
		}

		private void OnClickApply()
		{
			int num = (!needExtraIndex) ? fancyScrollView.selectItemIndex : extraIndexData.GetResult<int>();
			if (indexData.GetResult<int>() != num)
			{
				indexData.SetResult(num);
				Singleton<DataManager>.instance.Save();
				PlayApply();
			}
			else if (canCancleSelected)
			{
				indexData.SetResult(-1);
				Singleton<DataManager>.instance.Save();
				PlayNoApply();
			}
		}

		private void PlayFadeIn()
		{
			if (m_Anim != null && m_Anim.isActiveAndEnabled)
			{
				m_Anim.Play(animApply);
			}
		}

		private void PlayFadeOut()
		{
			if (m_Anim != null && m_Anim.isActiveAndEnabled)
			{
				m_Anim.Play(animNoApply);
			}
		}
	}
}
