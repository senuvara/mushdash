using UnityEngine;
using UnityEngine.EventSystems;

public class FSVScrollBar : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler, IEventSystemHandler
{
	public delegate void OnDragBeginDelegate();

	public delegate void OnDragDelegate();

	public delegate void OnDragEndDelegate();

	public delegate void OnPointerDownDelegate();

	public delegate void OnPointerUpDelegate();

	public OnDragBeginDelegate onDragBeginEvent;

	public OnDragDelegate onDragEvent;

	public OnDragEndDelegate onDragEndEvent;

	public OnPointerDownDelegate onPointerDown;

	public OnPointerDownDelegate onPointerUp;

	public void OnBeginDrag(PointerEventData eventData)
	{
		onDragBeginEvent();
	}

	public void OnDrag(PointerEventData eventData)
	{
		onDragEvent();
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		onDragEndEvent();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		onPointerDown();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		onPointerUp();
	}
}
