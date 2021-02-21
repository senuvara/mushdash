using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.PeroTools.UI
{
	public class PassthoughDragEvent : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
	{
		[Tooltip("指定将滑动事件传递给的目标。")]
		public GameObject passTarget;

		[Tooltip("是否也同时传递给目标的子结点。")]
		public bool includeChildrens;

		public void OnDrag(PointerEventData eventData)
		{
			if (passTarget == null)
			{
				return;
			}
			IDragHandler[] array = (!includeChildrens) ? passTarget.GetComponents<IDragHandler>() : passTarget.GetComponentsInChildren<IDragHandler>();
			if (array != null && array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].OnDrag(eventData);
				}
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (passTarget == null)
			{
				return;
			}
			IBeginDragHandler[] array = (!includeChildrens) ? passTarget.GetComponents<IBeginDragHandler>() : passTarget.GetComponentsInChildren<IBeginDragHandler>();
			if (array != null && array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].OnBeginDrag(eventData);
				}
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (passTarget == null)
			{
				return;
			}
			IEndDragHandler[] array = (!includeChildrens) ? passTarget.GetComponents<IEndDragHandler>() : passTarget.GetComponentsInChildren<IEndDragHandler>();
			if (array != null && array.Length > 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].OnEndDrag(eventData);
				}
			}
		}
	}
}
