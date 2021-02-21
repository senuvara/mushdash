using Assets.Scripts.GameCore.HostComponent;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using CodeStage.AntiCheat.ObscuredTypes;
using FormulaBase;

namespace Assets.Scripts.GameCore.Managers
{
	public class FeverManager
	{
		private static FeverManager m_Instance;

		private ObscuredFloat m_FeverValue;

		private bool m_IsActivateFever;

		public bool isAutoFever;

		public static FeverManager Instance => m_Instance ?? (m_Instance = new FeverManager());

		public int feverCount
		{
			get;
			private set;
		}

		public bool isManualFeverEnable
		{
			get;
			private set;
		}

		public FeverManager()
		{
			InitGameKernel();
		}

		public void InitGameKernel()
		{
			feverCount = 0;
			m_FeverValue = 0f;
			m_IsActivateFever = false;
			isAutoFever = Singleton<DataManager>.instance["Account"]["IsAutoFever"].GetResult<bool>();
		}

		public float GetFeverRate()
		{
			return (float)m_FeverValue / Singleton<BattleProperty>.instance.maxFever;
		}

		public float GetWholeFever()
		{
			return m_FeverValue;
		}

		public void ResetFever()
		{
			AddFever(0f - GetWholeFever());
			feverCount = 0;
			m_FeverValue = 0f;
			m_IsActivateFever = false;
		}

		public void AddFever(float value)
		{
			if ((float)m_FeverValue + value < Singleton<BattleProperty>.instance.maxFever)
			{
				if ((float)m_FeverValue + value < 0f)
				{
					m_IsActivateFever = false;
					m_FeverValue = 0f;
					FeverEffectManager.instance.CancelFeverEffect();
					Singleton<EventManager>.instance.Invoke("Battle/OnFeverEnd");
				}
				else
				{
					m_FeverValue = (float)m_FeverValue + value;
					if (value > 0f)
					{
						Singleton<EventManager>.instance.Invoke("Battle/OnFeverAdd");
					}
				}
				isManualFeverEnable = false;
			}
			else
			{
				if ((float)m_FeverValue != Singleton<BattleProperty>.instance.maxFever && (float)m_FeverValue + value >= Singleton<BattleProperty>.instance.maxFever)
				{
					Singleton<EventManager>.instance.Invoke("Battle/OnFeverMax");
				}
				m_FeverValue = Singleton<BattleProperty>.instance.maxFever;
				if (isAutoFever)
				{
					isManualFeverEnable = false;
					InvokeFever();
				}
				else
				{
					isManualFeverEnable = true;
				}
			}
			Singleton<EventManager>.instance.Invoke("Battle/OnFeverRateChanged");
		}

		public void InvokeFever()
		{
			if (!((float)m_FeverValue < Singleton<BattleProperty>.instance.maxFever) && !Singleton<StageBattleComponent>.instance.isPause)
			{
				isManualFeverEnable = false;
				feverCount++;
				m_FeverValue = Singleton<BattleProperty>.instance.maxFever;
				m_IsActivateFever = true;
				FeverEffectManager.instance.ActivateFever();
				Singleton<EventManager>.instance.Invoke("Battle/OnFever");
				if (IsGod())
				{
					BattleRoleAttributeComponent.instance.MissHardEffect(Singleton<BattleProperty>.instance.feverTime);
				}
			}
		}

		public bool IsOnFeverState()
		{
			return m_IsActivateFever;
		}

		public bool IsGod()
		{
			return m_IsActivateFever && Singleton<BattleProperty>.instance.isFeverGod;
		}

		public bool GetIsAutoFever()
		{
			return isAutoFever;
		}
	}
}
