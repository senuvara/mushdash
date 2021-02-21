using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using UnityEngine;
using UnityEngine.UI;

public class PCControllerCell : MonoBehaviour
{
	private GameObject m_Cover;

	private GameObject m_Reverse;

	private Image m_Selected;

	public GameObject controllerTypePS4;

	public GameObject controllerTypeXBox;

	public GameObject controllerTypeNs;

	private GameObject m_CurrentControllerType;

	private void Awake()
	{
		if (ControllerUtils.IsPS4Controller())
		{
			controllerTypePS4.SetActive(true);
			controllerTypeXBox.SetActive(false);
			controllerTypeNs.SetActive(false);
			m_CurrentControllerType = controllerTypePS4;
		}
		else if (ControllerUtils.IsNSController())
		{
			controllerTypePS4.SetActive(false);
			controllerTypeXBox.SetActive(false);
			controllerTypeNs.SetActive(true);
			m_CurrentControllerType = controllerTypeNs;
		}
		else
		{
			controllerTypePS4.SetActive(false);
			controllerTypeXBox.SetActive(true);
			controllerTypeNs.SetActive(false);
			m_CurrentControllerType = controllerTypeXBox;
		}
		m_Cover = m_CurrentControllerType.transform.Find("ImgCover").gameObject;
		m_Reverse = m_CurrentControllerType.transform.Find("ImgCoverReverse").gameObject;
	}

	private void OnEnable()
	{
		if (Singleton<InputManager>.instance.currentControllerName == "PS4")
		{
			controllerTypePS4.SetActive(true);
			controllerTypeXBox.SetActive(false);
			controllerTypeNs.SetActive(false);
			m_CurrentControllerType = controllerTypePS4;
		}
		else if (Singleton<InputManager>.instance.currentControllerName == "Ns")
		{
			controllerTypePS4.SetActive(false);
			controllerTypeXBox.SetActive(false);
			controllerTypeNs.SetActive(true);
			m_CurrentControllerType = controllerTypeNs;
		}
		else
		{
			controllerTypePS4.SetActive(false);
			controllerTypeXBox.SetActive(true);
			controllerTypeNs.SetActive(false);
			m_CurrentControllerType = controllerTypeXBox;
		}
	}

	public void SetSelectColor(Color c)
	{
	}

	public void SetReverse(bool isReverse)
	{
		m_Reverse.SetActive(isReverse);
		m_Cover.SetActive(!isReverse);
	}
}
