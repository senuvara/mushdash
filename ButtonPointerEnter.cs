using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPointerEnter : MonoBehaviour
{
	private bool m_IsPointerHover;

	public void PointerEnter()
	{
		if (!m_IsPointerHover && (bool)EventSystem.current)
		{
			EventSystem.current.SetSelectedGameObject(base.gameObject);
			m_IsPointerHover = true;
		}
	}

	public void PointerExit()
	{
		m_IsPointerHover = false;
	}
}
