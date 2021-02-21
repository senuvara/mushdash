using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using FormulaBase;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Effects
{
	public class ExpParticles : MonoBehaviour
	{
		[InfoBox("一个粒子对应得到的经验数", InfoMessageType.Info, null)]
		public float expRate = 1f;

		public Transform target;

		private ParticleSystem m_System;

		private ParticleSystem.Particle[] m_Particles;

		private IVariable m_ExpVariable;

		private IVariable m_ExpCount;

		private IVariable m_Level;

		private IVariable m_ExpPercent;

		private List<IData> m_Messages;

		private IData m_Message;

		private int m_Num;

		private float m_Sum;

		private RectTransform m_TargeTransform;

		private float m_X;

		private bool m_IsLevelUp;

		private void Awake()
		{
			m_ExpVariable = Singleton<DataManager>.instance["Account"]["Exp"];
			m_Messages = Singleton<DataManager>.instance["Account"]["Messages"].GetResult<List<IData>>();
			m_Level = Singleton<DataManager>.instance["Account"]["Level"];
			m_ExpPercent = Singleton<DataManager>.instance["Account"]["ExpRate"];
			m_TargeTransform = target.GetComponent<RectTransform>();
			Vector2 anchoredPosition = m_TargeTransform.anchoredPosition;
			m_X = anchoredPosition.x - (float)m_ExpPercent.GetResult<int>() * m_TargeTransform.rect.width;
		}

		private void OnEnable()
		{
			m_IsLevelUp = false;
			if (Singleton<DataManager>.instance["Account"]["IsShowMusynx"].GetResult<bool>() && !Singleton<DataManager>.instance["Account"]["HasShowMusynx"].GetResult<bool>())
			{
				Singleton<DataManager>.instance["Account"]["HasShowMusynx"].SetResult(true);
				Singleton<EventManager>.instance.Invoke("UI/OnMusynxShow");
				Singleton<DataManager>.instance.Save();
				base.gameObject.SetActive(false);
				return;
			}
			InitializeIfNeeded();
			m_Message = m_Messages.Find((IData d) => d["type"].GetResult<string>() == "exp");
			if (m_Message != null)
			{
				m_ExpCount = m_Message["count"];
				if (m_ExpCount.GetResult<int>() <= 0)
				{
					m_Messages.Remove(m_Message);
					base.gameObject.SetActive(false);
					return;
				}
				ParticleSystem.Burst burst = m_System.emission.GetBurst(0);
				ParticleSystem.MinMaxCurve count = burst.count;
				count.constant = Mathf.Ceil((float)m_ExpCount.GetResult<int>() / expRate);
				burst.count = count;
				m_System.emission.SetBurst(0, burst);
				Singleton<EventManager>.instance.Invoke("UI/DisableTouch");
				Singleton<EventManager>.instance.Invoke("UI/DisableInputKey");
			}
			else
			{
				base.gameObject.SetActive(false);
			}
			RectTransform targeTransform = m_TargeTransform;
			float x = m_X + m_TargeTransform.rect.width * m_ExpPercent.GetResult<float>();
			Vector2 anchoredPosition = m_TargeTransform.anchoredPosition;
			targeTransform.anchoredPosition = new Vector2(x, anchoredPosition.y);
		}

		private void LateUpdate()
		{
			InitializeIfNeeded();
			int num = m_Num;
			m_Num = m_System.GetParticles(m_Particles);
			int num2 = num - m_Num;
			if (num2 <= 0)
			{
				return;
			}
			m_Sum += expRate * (float)num2;
			if (!(m_Sum >= 1f))
			{
				return;
			}
			int num3 = Mathf.FloorToInt((float)m_ExpVariable.GetResult<int>() / 100f) + 1;
			int num4 = Mathf.FloorToInt(m_Sum);
			int result = m_ExpCount.GetResult<int>();
			if (result < num4)
			{
				num4 = result;
			}
			m_ExpVariable.SetResult(m_ExpVariable.GetResult<int>() + num4);
			m_ExpCount.SetResult(result - num4);
			if (Mathf.FloorToInt((float)m_ExpVariable.GetResult<int>() / 100f) + 1 != num3)
			{
				SingletonMonoBehaviour<MessageManager>.instance.Send("level", 1);
				SingletonMonoBehaviour<MessageManager>.instance.Receive("level");
				Singleton<EventManager>.instance.Invoke("UI/OnLevelUp");
				m_IsLevelUp = true;
			}
			if (m_ExpCount.GetResult<int>() == 0)
			{
				if (!m_IsLevelUp)
				{
					Singleton<EventManager>.instance.Invoke("UI/EnableTouch");
					Singleton<EventManager>.instance.Invoke("UI/EnableInputKey");
				}
				m_Messages.Remove(m_Message);
				IVariable data = Singleton<DataManager>.instance["Achievement"]["pass_count"];
				bool isRate = false;
				if (m_Messages.Count == 0 && !Singleton<DataManager>.instance["Account"]["HasRate"].GetResult<bool>() && Singleton<StageBattleComponent>.instance.evaluateNum >= 4 && data.GetResult<int>() >= 5)
				{
					Singleton<DataManager>.instance["Account"]["HasRate"].SetResult(true);
					isRate = true;
				}
				if (m_Level.GetResult<int>() == num3)
				{
					SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
					{
						if ((bool)this)
						{
							if ((bool)base.gameObject)
							{
								base.gameObject.SetActive(false);
							}
							if (isRate)
							{
								Singleton<EventManager>.instance.Invoke("UI/RateOurGame");
							}
						}
					}, 1f);
					Singleton<DataManager>.instance.Save();
					if (Singleton<DataManager>.instance["Account"]["IsShowNanoCore"].GetResult<bool>() && !Singleton<DataManager>.instance["Account"]["HasShowNanoCore"].GetResult<bool>())
					{
						Singleton<DataManager>.instance["Account"]["HasShowNanoCore"].SetResult(true);
						Singleton<EventManager>.instance.Invoke("UI/OnNanoCoreShow");
						Singleton<DataManager>.instance.Save();
						base.gameObject.SetActive(false);
					}
					Singleton<ServerManager>.instance.Synchronize();
				}
				Singleton<DataManager>.instance.Save();
			}
			Singleton<EventManager>.instance.Invoke("UI/OnExpAdded");
			Singleton<AudioManager>.instance.PlayOneShot("sfx_crystal", Singleton<DataManager>.instance["GameConfig"]["SfxVolume"].GetResult<float>());
			RectTransform targeTransform = m_TargeTransform;
			float x = m_X + m_TargeTransform.rect.width * m_ExpPercent.GetResult<float>();
			Vector2 anchoredPosition = m_TargeTransform.anchoredPosition;
			targeTransform.anchoredPosition = new Vector2(x, anchoredPosition.y);
			m_Sum -= num4;
			m_Sum = ((!(m_Sum < 0f)) ? m_Sum : 0f);
		}

		private void InitializeIfNeeded()
		{
			if (m_System == null)
			{
				m_System = GetComponent<ParticleSystem>();
			}
			if (m_Particles == null || m_Particles.Length < m_System.main.maxParticles)
			{
				m_Particles = new ParticleSystem.Particle[m_System.main.maxParticles];
			}
		}
	}
}
