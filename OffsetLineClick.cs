using Assets.Scripts.UI.Panels;
using UnityEngine;
using UnityEngine.EventSystems;

public class OffsetLineClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	public GameObject parent;

	public void OnPointerDown(PointerEventData data)
	{
		parent.GetComponent<PnlOffsetOption>().isLineDraging = true;
	}

	public void OnPointerUp(PointerEventData data)
	{
		parent.GetComponent<PnlOffsetOption>().isLineDraging = false;
	}
}
