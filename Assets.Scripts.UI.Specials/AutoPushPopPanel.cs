using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Specials
{
	public class AutoPushPopPanel : MonoBehaviour
	{
		[Required]
		public Button btnPreparationBackToStage;

		private GameObject m_PreActivePanel;

		[Required]
		public GameObject pnlHome;

		[Required]
		public GameObject pnlMenu;

		[Required]
		public GameObject pnlNavigation;

		[Required]
		public GameObject pnlPreparation;

		[Required]
		public GameObject pnlStage;

		[Required]
		public Button btnMenusBack;

		private void Awake()
		{
			EnableDisableHooker orAddComponent = pnlMenu.GetOrAddComponent<EnableDisableHooker>();
			orAddComponent.onEnable += OnEnablePnlMenu;
			btnMenusBack.onClick.AddListener(OnDisablePnlMenu);
			orAddComponent = pnlStage.GetOrAddComponent<EnableDisableHooker>();
			orAddComponent.onEnable += OnEnablePnlStage;
			orAddComponent.onDisable += OnDisablePnlStage;
			orAddComponent = pnlPreparation.GetOrAddComponent<EnableDisableHooker>();
			orAddComponent.onEnable += OnEnablePnlPreparation;
			btnPreparationBackToStage.onClick.AddListener(OnClickBtnPreparationBackToStage);
			m_PreActivePanel = pnlHome;
		}

		private void OnClickBtnPreparationBackToStage()
		{
			m_PreActivePanel = pnlStage;
		}

		private void OnEnablePnlPreparation(GameObject arg0)
		{
			m_PreActivePanel = pnlPreparation;
			Singleton<EventManager>.instance.Invoke("UI/DisableTouch");
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				Singleton<EventManager>.instance.Invoke("UI/EnableTouch");
			}, 0.15f);
		}

		private void OnDisablePnlStage(GameObject arg0)
		{
			if (m_PreActivePanel == pnlStage && !pnlMenu.activeSelf)
			{
				pnlHome.SetActive(true);
				m_PreActivePanel = pnlHome;
			}
		}

		private void OnEnablePnlStage(GameObject arg0)
		{
			if (m_PreActivePanel == pnlHome)
			{
				pnlHome.SetActive(false);
			}
			if (!pnlPreparation.activeSelf)
			{
				m_PreActivePanel = pnlStage;
			}
		}

		private void OnEnablePnlMenu(GameObject arg0)
		{
			if (m_PreActivePanel == pnlHome)
			{
				pnlHome.SetActive(false);
			}
			else if (m_PreActivePanel == pnlStage)
			{
				pnlStage.SetActive(false);
			}
			else if (m_PreActivePanel == pnlPreparation)
			{
				pnlPreparation.SetActive(false);
				pnlStage.SetActive(false);
			}
			pnlNavigation.SetActive(false);
		}

		private void OnDisablePnlMenu()
		{
			if (m_PreActivePanel == pnlHome)
			{
				pnlHome.SetActive(true);
			}
			else if (m_PreActivePanel == pnlStage)
			{
				pnlStage.SetActive(true);
			}
			else if (m_PreActivePanel == pnlPreparation)
			{
				pnlPreparation.SetActive(true);
				pnlStage.SetActive(true);
				pnlStage.GetComponent<Animator>().Play("StageToPreparation", 0, 1f);
			}
			pnlNavigation.SetActive(true);
		}
	}
}
