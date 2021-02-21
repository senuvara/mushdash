using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Rewired.Demos
{
	[AddComponentMenu("")]
	[RequireComponent(typeof(RectTransform))]
	public sealed class UIPointer : UIBehaviour
	{
		[Tooltip("Should the hardware pointer be hidden?")]
		[SerializeField]
		private bool _hideHardwarePointer = true;

		[Tooltip("Sets the pointer to the last sibling in the parent hierarchy. Do not enable this on multiple UIPointers under the same parent transform or they will constantly fight each other for dominance.")]
		[SerializeField]
		private bool _autoSort = true;

		[NonSerialized]
		private RectTransform _canvasRectTransform;

		public bool autoSort
		{
			get
			{
				return _autoSort;
			}
			set
			{
				if (value != _autoSort)
				{
					_autoSort = value;
					if (value)
					{
						base.transform.SetAsLastSibling();
					}
				}
			}
		}

		protected override void Awake()
		{
			base.Awake();
			Graphic[] componentsInChildren = GetComponentsInChildren<Graphic>();
			Graphic[] array = componentsInChildren;
			foreach (Graphic graphic in array)
			{
				graphic.raycastTarget = false;
			}
			if (_hideHardwarePointer)
			{
				Cursor.visible = false;
			}
			if (_autoSort)
			{
				base.transform.SetAsLastSibling();
			}
			GetDependencies();
		}

		private void Update()
		{
			if (_autoSort && base.transform.GetSiblingIndex() < base.transform.parent.childCount - 1)
			{
				base.transform.SetAsLastSibling();
			}
		}

		protected override void OnTransformParentChanged()
		{
			base.OnTransformParentChanged();
			GetDependencies();
		}

		protected override void OnCanvasGroupChanged()
		{
			base.OnCanvasGroupChanged();
			GetDependencies();
		}

		public void OnScreenPositionChanged(Vector2 screenPosition)
		{
			if (!(_canvasRectTransform == null))
			{
				Rect rect = _canvasRectTransform.rect;
				Vector2 anchoredPosition = Camera.main.ScreenToViewportPoint(screenPosition);
				float num = anchoredPosition.x * rect.width;
				Vector2 pivot = _canvasRectTransform.pivot;
				anchoredPosition.x = num - pivot.x * rect.width;
				float num2 = anchoredPosition.y * rect.height;
				Vector2 pivot2 = _canvasRectTransform.pivot;
				anchoredPosition.y = num2 - pivot2.y * rect.height;
				(base.transform as RectTransform).anchoredPosition = anchoredPosition;
			}
		}

		private void GetDependencies()
		{
			Canvas componentInChildren = base.transform.root.GetComponentInChildren<Canvas>();
			_canvasRectTransform = ((!(componentInChildren != null)) ? null : componentInChildren.GetComponent<RectTransform>());
		}
	}
}
