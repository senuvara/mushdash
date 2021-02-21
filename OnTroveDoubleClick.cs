using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnTroveDoubleClick : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
{
	private bool m_IsPotinterDowned;

	private bool m_BtnInvoked;

	public void OnPointerDown(PointerEventData data)
	{
		Button component = base.transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.transform.parent.Find("BtnCherk").GetComponent<Button>();
		if (m_IsPotinterDowned && component.interactable)
		{
			component.onClick.Invoke();
			m_BtnInvoked = true;
			m_IsPotinterDowned = false;
			CancelInvoke("ClickWaitTimeOver");
		}
		if (!m_BtnInvoked)
		{
			m_IsPotinterDowned = true;
			Invoke("ClickWaitTimeOver", 0.5f);
		}
		m_BtnInvoked = false;
	}

	private void ClickWaitTimeOver()
	{
		m_IsPotinterDowned = false;
		m_BtnInvoked = false;
	}
}
