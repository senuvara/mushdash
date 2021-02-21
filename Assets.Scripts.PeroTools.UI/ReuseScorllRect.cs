using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.UI
{
	[RequireComponent(typeof(ScrollRect))]
	public class ReuseScorllRect : MonoBehaviour
	{
		public enum ScrollType
		{
			Horizontal,
			Vertical
		}

		[EnumToggleButtons]
		public ScrollType scrollbarType;

		[CustomValueDrawer("OnSetScrollbar")]
		public Scrollbar scrollbar;

		[Tooltip("指定联动的 Sldier")]
		public Slider slider;

		[Required]
		[Tooltip("预设的元素")]
		public GameObject cell;

		[Tooltip("列表最大的元素数量")]
		public int maxCount;

		private int m_ContentCount;

		[Tooltip("可见的元素数量")]
		public int seeCount;

		[Tooltip("起始位置")]
		public int startAt;

		public bool isInitAtStart;

		public string positiveButtonName = "Option1";

		public string negativeButtonName = "Option2";

		[CustomValueDrawer("OnSetContent")]
		public RectTransform content;

		private int m_FirstCellIndex;

		protected bool isHideSlider;

		private bool m_IsHasListener;

		private bool m_AfterAddListener;

		private void Awake()
		{
			m_FirstCellIndex = startAt;
			m_ContentCount = maxCount;
			if (slider != null)
			{
				isHideSlider = (maxCount < seeCount);
				slider.wholeNumbers = true;
				slider.minValue = startAt;
				slider.maxValue = maxCount - seeCount + 1;
				slider.value = startAt;
				slider.gameObject.SetActive(!isHideSlider);
			}
			scrollbar.value = 0.5f;
			scrollbar.handleRect.gameObject.SetActive(false);
			if (isInitAtStart)
			{
				InitList();
			}
		}

		private void Update()
		{
			if (!string.IsNullOrEmpty(positiveButtonName) && (Input.GetButtonDown(positiveButtonName) || Input.GetButton(positiveButtonName)))
			{
				ScrollToPrevious();
			}
			if (!string.IsNullOrEmpty(negativeButtonName) && (Input.GetButtonDown(negativeButtonName) || Input.GetButton(negativeButtonName)))
			{
				ScrollToNext();
			}
			OnUpdate();
		}

		private void RefreshScorllBar(float f)
		{
			if (m_AfterAddListener)
			{
				m_AfterAddListener = false;
				return;
			}
			if (f <= 0.1f)
			{
				if (m_FirstCellIndex + seeCount >= m_ContentCount)
				{
					return;
				}
				RefreshFrom(m_FirstCellIndex + 1);
			}
			else if (f >= 0.9f)
			{
				if (m_FirstCellIndex <= startAt)
				{
					return;
				}
				RefreshFrom(m_FirstCellIndex - 1);
			}
			scrollbar.value = 0.5f;
			if ((bool)slider)
			{
				slider.value = m_FirstCellIndex;
			}
		}

		private void RefreshFrom(int start)
		{
			if (isHideSlider)
			{
				return;
			}
			for (int i = 0; i < seeCount; i++)
			{
				if (start + i >= m_ContentCount)
				{
					return;
				}
				SetCell(content.GetChild(i).gameObject, start + i);
			}
			m_FirstCellIndex = start;
			if ((bool)slider)
			{
				slider.value = m_FirstCellIndex;
			}
		}

		public void InitList()
		{
			SetScorllbarListenerEnable(false);
			if ((bool)slider)
			{
				slider.value = startAt;
			}
			for (int i = startAt; i < seeCount + startAt; i++)
			{
				if (i < m_ContentCount)
				{
					Object.Instantiate(cell, content);
				}
			}
			m_FirstCellIndex = startAt;
			SetScorllbarListenerEnable(true);
		}

		public virtual void SetCell(GameObject obj, int index)
		{
		}

		public void SetContentCount(int i)
		{
			m_ContentCount = ((i < maxCount) ? i : maxCount);
			if (!(slider == null))
			{
				isHideSlider = (m_ContentCount <= seeCount);
				slider.gameObject.SetActive(!isHideSlider);
				if (!isHideSlider)
				{
					slider.maxValue = m_ContentCount - seeCount + 1;
				}
				scrollbar.value = 0.5f;
			}
		}

		public void ScrollToNext()
		{
			if (m_FirstCellIndex + seeCount < m_ContentCount)
			{
				RefreshFrom(m_FirstCellIndex + 1);
			}
		}

		public void ScrollToPrevious()
		{
			if (m_FirstCellIndex > startAt)
			{
				RefreshFrom(m_FirstCellIndex - 1);
			}
		}

		public void SetScorllbarListenerEnable(bool enable)
		{
			if (!enable && m_IsHasListener)
			{
				scrollbar.onValueChanged.RemoveListener(RefreshScorllBar);
				m_IsHasListener = false;
			}
			else if (enable && !m_IsHasListener)
			{
				scrollbar.onValueChanged.AddListener(RefreshScorllBar);
				m_IsHasListener = true;
				m_AfterAddListener = true;
			}
		}

		public virtual void OnUpdate()
		{
		}
	}
}
