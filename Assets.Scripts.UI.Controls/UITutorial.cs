using Assets.Scripts.Common;
using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI.Controls
{
	public class UITutorial : SingletonMonoBehaviour<UITutorial>
	{
		private Animator m_Animator;

		private Sequence m_AddLifeSeq;

		public Animator animator
		{
			get
			{
				m_Animator = (m_Animator ?? GetComponent<Animator>());
				return m_Animator;
			}
		}

		private void Awake()
		{
			Singleton<EventManager>.instance.RegEvent("Battle/OnHpDeduct").trigger += OnHpReduct;
			Singleton<EventManager>.instance.RegEvent("Battle/OnResume").trigger += OnResume;
			Singleton<EventManager>.instance.RegEvent("Battle/OnPause").trigger += OnPause;
		}

		private void Start()
		{
			m_Animator = GetComponent<Animator>();
			string stateName = "Tutorial";
			if (!Singleton<DataManager>.instance["Account"]["IsNew"].GetResult<bool>() && CustomDefines.GetEntityValue<bool>("Game/IsFool"))
			{
				stateName = "Tutorial_fool";
			}
			m_Animator.Play(stateName);
		}

		private void OnDestroy()
		{
			Singleton<EventManager>.instance.RegEvent("Battle/OnHpDeduct").trigger -= OnHpReduct;
			Singleton<EventManager>.instance.RegEvent("Battle/OnResume").trigger -= OnResume;
			Singleton<EventManager>.instance.RegEvent("Battle/OnPause").trigger -= OnPause;
		}

		private void Update()
		{
			if ((bool)m_Animator && Singleton<AudioManager>.instance.bgm.time > 1f && !m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
			{
				m_Animator.SetTime(Singleton<AudioManager>.instance.bgm.time);
			}
		}

		private void OnResume(object sender, object reciever, params object[] arg)
		{
			if ((bool)m_Animator)
			{
				m_Animator.enabled = true;
			}
		}

		private void OnPause(object sender, object reciever, params object[] args)
		{
			if ((bool)m_Animator)
			{
				m_Animator.enabled = false;
			}
		}

		private void OnHpReduct(object sender, object reciever, params object[] args)
		{
			if (base.gameObject.activeInHierarchy)
			{
				if (m_AddLifeSeq != null)
				{
					m_AddLifeSeq.Kill();
				}
				m_AddLifeSeq = DOTweenUtils.Delay(delegate
				{
					BattleRoleAttributeComponent.instance.AddHp(BattleRoleAttributeComponent.instance.GetHpMax() - (int)BattleRoleAttributeComponent.instance.hp);
				}, 1.5f);
			}
		}
	}
}
