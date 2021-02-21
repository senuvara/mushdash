using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Events;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.PreWarm;
using Assets.Scripts.PeroTools.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Components
{
	public class FancyScrollView : SerializedSimplySelectable, IBeginDragHandler, IDragHandler, IEndDragHandler, IPreWarm, IEventSystemHandler
	{
		public enum ScrollDirection
		{
			Vertical,
			Horizontal
		}

		public enum MovementType
		{
			Loop,
			Elastic,
			Clamped
		}

		public enum SortType
		{
			None = -1,
			LeftToRight = 0,
			MiddleToSide = 1,
			RightToLeft = 2,
			Custom = 999
		}

		public enum SourceType
		{
			Prefabs,
			Children
		}

		public enum State
		{
			Static,
			Dynamic
		}

		[InfoBox("勾选Show Advance Options 来查看更多选项。", InfoMessageType.Info, null)]
		[ToggleLeft]
		public bool showAdvanceOptions;

		[TitleGroup("Datas", null, TitleAlignments.Left, true, true, false, 0)]
		[Required]
		[Tooltip("指定内容的根节点。")]
		public RectTransform content;

		[TitleGroup("Datas", null, TitleAlignments.Left, true, true, false, 0)]
		[EnumToggleButtons]
		[Tooltip("指定条目的类型。")]
		public SourceType sourceType;

		[TitleGroup("Datas", null, TitleAlignments.Left, true, true, false, 0)]
		[Required]
		[ShowIf("sourceType", SourceType.Prefabs, true)]
		[Tooltip("指定条目的预设。")]
		[Variable(typeof(GameObject), null, false)]
		public IVariable cellPrefabs;

		[TitleGroup("Datas", null, TitleAlignments.Left, true, true, false, 0)]
		[ShowIf("sourceType", SourceType.Prefabs, true)]
		[Tooltip("指定条目的数量。")]
		[Variable(5, null, false)]
		public IVariable cellCount;

		[TitleGroup("Datas", null, TitleAlignments.Left, true, true, false, 0)]
		[Tooltip("指定条目之间的间距。取值0-1，表示在动画片段中的长度")]
		[MinValue(0.0)]
		[MaxValue(1.0)]
		public float cellInterval = 0.1f;

		[TitleGroup("Datas", null, TitleAlignments.Left, true, true, false, 0)]
		[ShowIf("sourceType", SourceType.Prefabs, true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[ShowIf("showAdvanceOptions", true)]
		[Tooltip("条目填补预设，指定默认用于填补列表空缺的条目预设。")]
		[Variable(typeof(GameObject), null, false)]
		public IVariable cellPlaceholderPrefabs;

		[TitleGroup("Datas", null, TitleAlignments.Left, true, true, false, 0)]
		[ShowIf("sourceType", SourceType.Prefabs, true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[ShowIf("showAdvanceOptions", true)]
		[Tooltip("指定列表的最小条目数量，当条目数量不足时，将会使用条目填补预设来填充。")]
		[Variable(0, null, false)]
		public IVariable minCellCount;

		[TitleGroup("Datas", null, TitleAlignments.Left, true, true, false, 0)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[ShowIf("showAdvanceOptions", true)]
		[MinValue(0.0)]
		[MaxValue(1.0)]
		[Tooltip("指定列表的中心点偏移值，0在viewport左边，1在viewport右边。")]
		public float cellOffset = 0.5f;

		[TitleGroup("Datas", null, TitleAlignments.Left, true, true, false, 0)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[ShowIf("showAdvanceOptions", true)]
		[Tooltip("指定列表初始化处于哪一个页面，如果值为负数，则初始页面与上一次打开时相同。")]
		[Variable(-1, null, false)]
		public IVariable startIndex;

		[TitleGroup("Datas", null, TitleAlignments.Left, true, true, false, 0)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[ShowIf("showAdvanceOptions", true)]
		[Tooltip("不让列表在Start时或PreWarm时初始化。在一些情况下，你可能希望完全由自己在代码中控制列表的初始化，那么你可以对该项打勾")]
		public bool dontInit;

		[TitleGroup("Datas", null, TitleAlignments.Left, true, true, false, 0)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[ShowIf("showAdvanceOptions", true)]
		[Tooltip("让列表在PreWarm中初始化，否则列表会在Awake中初始化。")]
		public bool initOnPrewarm = true;

		[TitleGroup("Datas", null, TitleAlignments.Left, true, true, false, 0)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[ShowIf("sourceType", SourceType.Prefabs, true)]
		[ShowIf("showAdvanceOptions", true)]
		[Tooltip("是否关闭自动缓存？\nFancyScrollView是一个自动缓存的列表，无论列表的数据有多少项，只有能看得见的部分才会被加载出来，看不见的部分会自动销毁，保证内存使用处于非常低的状态。\n但在某些情况下，这样的特性会造成不好的效果。例如列表的项目包含Spine的UGUI功能，那么很可能在项目被创建时占用大量的时间用于初始化，导致切换项目时卡顿严重。\n建议慎重选择此功能，请务必先明白该选项会为游戏带来哪些影响。")]
		public bool disableAutoCache;

		[TitleGroup("Views", null, TitleAlignments.Left, true, true, false, 0)]
		[Required]
		[Tooltip("指定可视区域")]
		public RectTransform viewPort;

		[TitleGroup("Views", null, TitleAlignments.Left, true, true, false, 0)]
		[Required]
		[Tooltip("指定动画名称，注意：请确保动画长度为1s（60帧）。")]
		public string animName;

		[ShowIf("showAdvanceOptions", true)]
		[TitleGroup("Views", null, TitleAlignments.Left, true, true, false, 0)]
		[OnValueChanged("UpdateHighestPos", false)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[EnumPaging]
		[Tooltip("指定列表页面的排序方式，注意如果不需要排序请关闭此选项，可以提高不小的运行效率。")]
		public SortType sortType = SortType.None;

		[Indent(2)]
		[TitleGroup("Views", null, TitleAlignments.Left, true, true, false, 0)]
		[ShowIf("sortType", SortType.Custom, true)]
		[Range(0f, 1f)]
		[Tooltip("指示列表的最高点，取值0-1，表示在动画时间线中的位置。")]
		public float highestPos = -1f;

		[TitleGroup("Views", null, TitleAlignments.Left, true, true, false, 0)]
		[Tooltip("指示一个Scrollbar与列表联动。")]
		public Scrollbar processBar;

		[TitleGroup("Views", null, TitleAlignments.Left, true, true, false, 0)]
		[ShowIf("showAdvanceOptions", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[Tooltip("切换的音效。")]
		public AudioClip audioClip;

		[SerializeField]
		[Variable(1f, null, false)]
		private IVariable m_Volume;

		[TitleGroup("Views", null, TitleAlignments.Left, true, true, false, 0)]
		[ShowIf("showAdvanceOptions", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[Tooltip("当列表处于边界时，点击左右按钮会触发错误音效。")]
		public AudioClip errorAudioClip;

		[TitleGroup("Control", null, TitleAlignments.Left, true, true, false, 0)]
		[EnumToggleButtons]
		[Tooltip("手指/鼠标拖动方向。")]
		public ScrollDirection dragDirection = ScrollDirection.Horizontal;

		[TitleGroup("Control", null, TitleAlignments.Left, true, true, false, 0)]
		[EnumToggleButtons]
		[Tooltip("指定列表的运动方式：Loop（循环无限制）、Elastic（弹性）、Clamped（固定在区域内）,注意SourceType为Children时，使用LoopUnrestricted无法循环。")]
		public MovementType movementType = MovementType.Elastic;

		[ShowIf("showAdvanceOptions", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[TitleGroup("Control", null, TitleAlignments.Left, true, true, false, 0)]
		[Tooltip("指定列表滚动敏感度。")]
		public float scrollSensitivity = 1f;

		[ShowIf("showAdvanceOptions", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[TitleGroup("Control", null, TitleAlignments.Left, true, true, false, 0)]
		[Tooltip("指定页面是否具备滚动惯性。")]
		public bool inertia = true;

		[TitleGroup("Control", null, TitleAlignments.Left, true, true, false, 0)]
		[Tooltip("指定一个按钮移动列表到上一个项目。")]
		public Button btnPrevious;

		[TitleGroup("Control", null, TitleAlignments.Left, true, true, false, 0)]
		[Tooltip("指定一个按钮移动列表到下一个项目。")]
		public Button btnNext;

		[ShowIf("showAdvanceOptions", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[TitleGroup("Control", null, TitleAlignments.Left, true, true, false, 0)]
		[Tooltip("跳转到上/下一个项目的期望时间,松开鼠标之后对其到某个项目的时间也受其影响。注意该值并不是准确时间，实际使用时间一般会大于该值。")]
		public float switchDuration = 0.1f;

		[ShowIf("showAdvanceOptions", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[TitleGroup("Control", null, TitleAlignments.Left, true, true, false, 0)]
		[Tooltip("按住按钮，到开始长按滚动时，需要的时间")]
		public float longPressTriggerTime = 0.5f;

		[ShowIf("showAdvanceOptions", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[TitleGroup("Control", null, TitleAlignments.Left, true, true, false, 0)]
		[MinValue(0.0)]
		[MaxValue(1.0)]
		[Tooltip("当列表处于边界，按下左右按钮，列表会向边界移动一小段距离。该值表示这个距离的最大值。")]
		public float borderMaxOffset = 0.1f;

		[TitleGroup("Control", null, TitleAlignments.Left, true, true, false, 0)]
		[ShowIf("showAdvanceOptions", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[Tooltip("启用这个选项时。当列表抵达左边界会自动disable左按钮，右边界同理。")]
		public bool disableButtonWhenReachBorder;

		[TitleGroup("Control", null, TitleAlignments.Left, true, true, false, 0)]
		[ShowIf("showAdvanceOptions", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		[Tooltip("启用这个选项时屏蔽拖拽行为，只有按键响应。")]
		public bool disableDrag;

		private float m_PrevScrollPosition;

		private Vector2 m_PointerStartLocalPosition;

		private float m_DragStartScrollPosition;

		private int m_FinalSelectItemIndex = -1;

		private bool m_LongPressing;

		private bool m_AutoScrolling;

		private bool m_KeepAutoScrolling;

		private bool m_ShouldBorderAutoScroll;

		private float m_AutoScrollDuration;

		private float m_AutoScrollStartTime;

		private float m_AutoScrollPosition;

		private bool m_HasInit;

		private bool m_FirstInit;

		[SerializeField]
		[HideInInspector]
		private int m_AnimNameHash;

		[SerializeField]
		[HideInInspector]
		private float m_DragMulFactor;

		private int m_CellCount;

		private readonly Dictionary<int, OnFancyScrollViewCellUpdate> m_Cells = new Dictionary<int, OnFancyScrollViewCellUpdate>();

		private FastPool m_PoolPrefabs;

		private FastPool m_PoolPlaceHoldPrefabs;

		private bool isScrollBarDrag;

		private bool m_PointerDownToScroll;

		public bool loop => movementType == MovementType.Loop;

		public float velocity
		{
			get;
			private set;
		}

		public bool dragging
		{
			get;
			private set;
		}

		public float currentScrollPosition
		{
			get;
			private set;
		}

		public float expectCurrentScollPosition => (state != State.Dynamic) ? currentScrollPosition : m_AutoScrollPosition;

		public int selectItemIndex
		{
			get;
			set;
		}

		public State state
		{
			get;
			set;
		}

		public event UnityAction<int> onFinalItemIndexChange;

		public event UnityAction<int> onItemIndexChange;

		public event UnityAction<float> onUpdatePosition;

		public event Action<State> OnStateChange;

		private void UpdateHighestPos()
		{
			switch (sortType)
			{
			case SortType.None:
				highestPos = -1f;
				break;
			case SortType.LeftToRight:
				highestPos = 0f;
				break;
			case SortType.MiddleToSide:
				highestPos = 0.5f;
				break;
			case SortType.RightToLeft:
				highestPos = 1f;
				break;
			}
		}

		public void Rebuild()
		{
			if (sourceType == SourceType.Children)
			{
				RebuildChildren();
				return;
			}
			if (sourceType == SourceType.Prefabs)
			{
				RebuildPrefabs();
				return;
			}
			throw new ArgumentException();
		}

		public void Rebuild(GameObject prefabs)
		{
			sourceType = SourceType.Prefabs;
			cellPrefabs.SetResult(prefabs);
			ReleaseAllCell();
			RebuildPrefabs();
		}

		public void Rebuild(GameObject[] childrens)
		{
			if (childrens != null)
			{
				sourceType = SourceType.Children;
				DestroyAllChildren();
				for (int i = 0; i < childrens.Length; i++)
				{
					childrens[i].transform.SetParent(content);
					childrens[i].transform.SetSiblingIndex(i);
				}
				RebuildChildren();
			}
		}

		private void RebuildChildren()
		{
			m_CellCount = content.childCount;
			for (int i = 0; i < content.childCount; i++)
			{
				Transform child = content.GetChild(i);
				OnFancyScrollViewCellUpdate onFancyScrollViewCellUpdate = PackageCellGo(child.gameObject);
				m_Cells[i] = onFancyScrollViewCellUpdate;
				UpdateCellData(onFancyScrollViewCellUpdate, GetLoopIndex(i));
				UpdateCellVisibleByIndex(onFancyScrollViewCellUpdate, i);
			}
			InitCommonData();
		}

		private void RebuildPrefabs()
		{
			if (m_PoolPrefabs != null)
			{
				Singleton<PoolManager>.instance.DestroyFastPool(m_PoolPrefabs);
			}
			if (m_PoolPlaceHoldPrefabs != null)
			{
				Singleton<PoolManager>.instance.DestroyFastPool(m_PoolPlaceHoldPrefabs);
			}
			GameObject @object = GameUtils.GetObject<GameObject>(cellPrefabs.result);
			if (@object != null)
			{
				m_PoolPrefabs = Singleton<PoolManager>.instance.MakeFastPool(@object, -1);
			}
			else
			{
				Debug.LogError("Cell prefabs should not be null! " + base.gameObject.name);
			}
			if (cellPlaceholderPrefabs != null)
			{
				GameObject object2 = GameUtils.GetObject<GameObject>(cellPlaceholderPrefabs.result);
				if (object2 != null)
				{
					m_PoolPlaceHoldPrefabs = Singleton<PoolManager>.instance.MakeFastPool(object2, -1);
				}
			}
			m_CellCount = Mathf.Max(minCellCount.GetResult<int>(), cellCount.GetResult<int>());
			if (disableAutoCache)
			{
				for (int i = 0; i < m_CellCount; i++)
				{
					OnFancyScrollViewCellUpdate onFancyScrollViewCellUpdate = CreateNewCell(i);
					m_Cells[i] = onFancyScrollViewCellUpdate;
					UpdateCellData(onFancyScrollViewCellUpdate, GetLoopIndex(i));
					UpdateCellVisibleByIndex(onFancyScrollViewCellUpdate, i);
				}
			}
			InitCommonData();
		}

		public void ScrollToDataIndex(int dataIndex, float duration, bool timeRelToDis = true, int dirTendFlag = 0)
		{
			if (m_CellCount != 0)
			{
				if (dataIndex < 0 || dataIndex >= m_CellCount)
				{
					Debug.LogErrorFormat("Invaild data index! expect within from [{0}] to [{1}],but get [{2}].", 0, m_CellCount, dataIndex);
				}
				else
				{
					ScrollTo(dataIndex, duration, timeRelToDis, dirTendFlag);
				}
			}
		}

		public void ScrollToDataUid(string uid, float duration, bool timeRelToDis = true, int dirTendFlag = 0)
		{
			int configIndex = Singleton<ConfigManager>.instance.GetConfigIndex("ALBUM1", "uid", uid);
			ScrollTo(configIndex, duration, timeRelToDis, dirTendFlag);
		}

		public void ScrollTo(float position, float duration, bool timeRelToDis = true, int dirTendFlag = 0)
		{
			if (!dragging && IsInteractable())
			{
				m_AutoScrolling = true;
				m_AutoScrollStartTime = Time.unscaledTime;
				m_DragStartScrollPosition = currentScrollPosition;
				position = ((!loop) ? position : CalculateClosestPosition(position, dirTendFlag));
				m_AutoScrollDuration = ((!timeRelToDis) ? duration : (duration * Mathf.Min(1f, Mathf.Abs(position - currentScrollPosition))));
				m_AutoScrollPosition = position;
			}
		}

		public IEnumerator PlayScrollTo(int dataIndex, float duration, bool timeRelToDis = true)
		{
			ScrollToDataIndex(dataIndex, duration, timeRelToDis);
			yield return new WaitUntil(() => !m_AutoScrolling || !base.isActiveAndEnabled);
		}

		public void ScrollToNext(float time)
		{
			ScrollTo((!loop) ? Mathf.Clamp(selectItemIndex + 1, 0f, m_CellCount - 1) : ((float)(selectItemIndex + 1)), time, true, 1);
		}

		public void ScrollToPrevious(float time)
		{
			int num = selectItemIndex + m_CellCount * GetLoop(expectCurrentScollPosition);
			ScrollTo((!loop) ? Mathf.Clamp(selectItemIndex - 1, 0f, m_CellCount - 1) : ((float)(num - 1)), time, true, -1);
		}

		public int GetLoop(float pos)
		{
			return Mathf.FloorToInt(pos / (float)m_CellCount);
		}

		public float GetUnloopPosition(float loopPos)
		{
			if (loopPos < 0f || loopPos > (float)(m_CellCount - 1))
			{
				Debug.LogError("Unvaild position.");
				return -1f;
			}
			return (float)(GetLoop(Mathf.RoundToInt(currentScrollPosition)) * m_CellCount) + loopPos;
		}

		public int GetUnloopIndex(int dataIndex)
		{
			if (dataIndex < 0 || dataIndex > m_CellCount - 1)
			{
				Debug.LogErrorFormat("Unvaild index. from [{0}] to [{1}] ,but get value [{2}].", 0, m_CellCount, dataIndex);
				return -1;
			}
			return GetLoop(Mathf.RoundToInt(currentScrollPosition)) * m_CellCount + dataIndex;
		}

		public int GetLoopIndex(int index)
		{
			if (m_CellCount <= 0)
			{
				return -1;
			}
			if (index < 0)
			{
				index = m_CellCount - 1 + (index + 1) % m_CellCount;
			}
			else if (index > m_CellCount - 1)
			{
				index %= m_CellCount;
			}
			return index;
		}

		public float GetLoopPosition(float position)
		{
			if (m_CellCount <= 0)
			{
				return -1f;
			}
			if (position < 0f)
			{
				position = (float)(m_CellCount - 1) + (position + 1f) % (float)m_CellCount;
			}
			else if (position > (float)(m_CellCount - 1))
			{
				position %= (float)m_CellCount;
			}
			return position;
		}

		public OnFancyScrollViewCellUpdate GetCell(int dataIndex)
		{
			if (dataIndex < 0 || dataIndex >= m_CellCount)
			{
				return null;
			}
			int unloopIndex = GetUnloopIndex(dataIndex);
			OnFancyScrollViewCellUpdate value = null;
			m_Cells.TryGetValue(unloopIndex, out value);
			return value;
		}

		private void LongPressToNext(bool isLongPress = true)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (isLongPress)
			{
				if (!dragging && !m_KeepAutoScrolling)
				{
					m_LongPressing = true;
					ScrollToNext(switchDuration);
				}
			}
			else if (!m_LongPressing && !loop && selectItemIndex == m_CellCount - 1)
			{
				if (!m_KeepAutoScrolling && errorAudioClip != null)
				{
					Singleton<AudioManager>.instance.PlayOneShot(errorAudioClip, (m_Volume == null) ? 1f : m_Volume.GetResult<float>());
				}
				m_KeepAutoScrolling = true;
				ScrollTo((float)selectItemIndex + borderMaxOffset, switchDuration, false);
			}
		}

		private void LongPressToPrevious(bool isLongPress = true)
		{
			if (!base.isActiveAndEnabled)
			{
				return;
			}
			if (isLongPress)
			{
				if (!dragging && !m_KeepAutoScrolling)
				{
					m_LongPressing = true;
					ScrollToPrevious(switchDuration);
				}
			}
			else if (!m_LongPressing && !loop && selectItemIndex == 0)
			{
				if (!m_KeepAutoScrolling && errorAudioClip != null)
				{
					Singleton<AudioManager>.instance.PlayOneShot(errorAudioClip, (m_Volume == null) ? 1f : m_Volume.GetResult<float>());
				}
				m_KeepAutoScrolling = true;
				ScrollTo((float)selectItemIndex - borderMaxOffset, switchDuration, false);
			}
		}

		private void LongPressNextUp()
		{
			if (base.isActiveAndEnabled)
			{
				if (m_KeepAutoScrolling)
				{
					m_AutoScrolling = false;
					m_KeepAutoScrolling = false;
					m_ShouldBorderAutoScroll = true;
					velocity = 0f;
				}
				if (!m_LongPressing && (loop || selectItemIndex != m_CellCount - 1))
				{
					ScrollToNext(switchDuration);
				}
				m_LongPressing = false;
			}
		}

		private void LongPressPreviousUp()
		{
			if (base.isActiveAndEnabled)
			{
				if (m_KeepAutoScrolling)
				{
					m_AutoScrolling = false;
					m_KeepAutoScrolling = false;
					m_ShouldBorderAutoScroll = true;
					velocity = 0f;
				}
				if (!m_LongPressing && (loop || selectItemIndex != 0))
				{
					ScrollToPrevious(switchDuration);
				}
				m_LongPressing = false;
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (!disableDrag && IsInteractable() && !m_LongPressing && !(eventData.delta.sqrMagnitude < 8f) && eventData.button == PointerEventData.InputButton.Left)
			{
				m_PointerStartLocalPosition = Vector2.zero;
				RectTransformUtility.ScreenPointToLocalPointInRectangle(viewPort, eventData.position, eventData.pressEventCamera, out m_PointerStartLocalPosition);
				m_DragStartScrollPosition = currentScrollPosition;
				dragging = true;
				m_AutoScrolling = false;
			}
		}

		public void OnDrag(PointerEventData eventData)
		{
			Vector2 localPoint;
			if (dragging && eventData.button == PointerEventData.InputButton.Left && RectTransformUtility.ScreenPointToLocalPointInRectangle(viewPort, eventData.position, eventData.pressEventCamera, out localPoint))
			{
				Vector2 vector = localPoint - m_PointerStartLocalPosition;
				float num = ((dragDirection != ScrollDirection.Horizontal) ? vector.y : (0f - vector.x)) * m_DragMulFactor + m_DragStartScrollPosition;
				float num2 = CalculateOffset(num);
				num += num2;
				if (movementType == MovementType.Elastic && !Mathf.Approximately(num2, 0f))
				{
					num -= RubberDelta(num2, scrollSensitivity + 1f);
				}
				UpdatePosition(num);
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (dragging && eventData.button == PointerEventData.InputButton.Left)
			{
				dragging = false;
			}
		}

		public void PreWarm(int slice)
		{
			if (m_HasInit)
			{
				return;
			}
			switch (slice)
			{
			case 0:
			{
				InitData();
				m_AnimNameHash = Animator.StringToHash(animName);
				m_DragMulFactor = scrollSensitivity * (1f / cellOffset + 1f) / GetViewportSize();
				for (int i = 0; i < content.childCount; i++)
				{
					if (!content.GetChild(i).gameObject.activeSelf)
					{
						UnityEngine.Object.Destroy(content.GetChild(i).gameObject);
					}
				}
				if (btnPrevious != null)
				{
					LongPressButton orAddComponent = btnPrevious.GetOrAddComponent<LongPressButton>();
					orAddComponent.longPressIncludePointerDown = false;
					orAddComponent.delta = longPressTriggerTime;
					orAddComponent.onPress = (Action<bool>)Delegate.Combine(orAddComponent.onPress, new Action<bool>(LongPressToPrevious));
					orAddComponent.onPressUp = (Action)Delegate.Combine(orAddComponent.onPressUp, new Action(LongPressPreviousUp));
				}
				if (btnNext != null)
				{
					LongPressButton orAddComponent2 = btnNext.GetOrAddComponent<LongPressButton>();
					orAddComponent2.longPressIncludePointerDown = false;
					orAddComponent2.delta = longPressTriggerTime;
					orAddComponent2.onPress = (Action<bool>)Delegate.Combine(orAddComponent2.onPress, new Action<bool>(LongPressToNext));
					orAddComponent2.onPressUp = (Action)Delegate.Combine(orAddComponent2.onPressUp, new Action(LongPressNextUp));
				}
				break;
			}
			case 2:
				if (!dontInit && initOnPrewarm)
				{
					Rebuild();
					m_HasInit = true;
				}
				break;
			}
		}

		protected override void Start()
		{
			if (!dontInit && !initOnPrewarm && !m_HasInit)
			{
				PreWarm(0);
				PreWarm(1);
				Rebuild();
				UpdatePosition(currentScrollPosition);
				m_HasInit = true;
			}
		}

		protected override void OnEnable()
		{
			if (m_HasInit)
			{
				UpdatePosition(Mathf.RoundToInt(currentScrollPosition));
			}
		}

		protected override void OnDisable()
		{
			InitData();
		}

		protected override void OnDestroy()
		{
			if (m_PoolPrefabs != null)
			{
				Singleton<PoolManager>.instance.DestroyFastPool(m_PoolPrefabs);
			}
			if (m_PoolPlaceHoldPrefabs != null)
			{
				Singleton<PoolManager>.instance.DestroyFastPool(m_PoolPlaceHoldPrefabs);
			}
		}

		private void Update()
		{
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			float a = CalculateOffset(currentScrollPosition);
			if (m_AutoScrolling || m_KeepAutoScrolling)
			{
				float currentVelocity = velocity;
				float num = Time.unscaledTime - m_AutoScrollStartTime;
				float num2 = Mathf.SmoothDamp(currentScrollPosition, m_AutoScrollPosition, ref currentVelocity, Mathf.Max(0f, m_AutoScrollDuration - 0.5f * num));
				velocity = currentVelocity;
				if (Mathf.Approximately(num2, m_AutoScrollPosition))
				{
					m_AutoScrolling = false;
					velocity = 0f;
				}
				UpdatePosition(num2);
			}
			else if (!dragging)
			{
				bool flag = Mathf.Approximately(a, 0f);
				if (movementType == MovementType.Elastic && !flag)
				{
					int num3 = (!(currentScrollPosition < 0f)) ? (m_CellCount - 1) : 0;
					ScrollTo(num3, switchDuration, !m_ShouldBorderAutoScroll);
					if (m_ShouldBorderAutoScroll)
					{
						m_ShouldBorderAutoScroll = false;
					}
				}
				else if (inertia && !Mathf.Approximately(velocity, 0f) && flag)
				{
					ScrollTo(CalculateTarget(currentScrollPosition, velocity, switchDuration * 0.5f), switchDuration * 1.5f);
					isScrollBarDrag = false;
				}
				else if (m_PointerDownToScroll)
				{
					ScrollTo(CalculateTarget(currentScrollPosition, velocity, switchDuration * 0.5f), switchDuration * 1.5f);
					isScrollBarDrag = false;
					m_PointerDownToScroll = false;
				}
			}
			if (!m_AutoScrolling && !m_KeepAutoScrolling && dragging && inertia)
			{
				float b = (currentScrollPosition - m_PrevScrollPosition) / unscaledDeltaTime;
				velocity = Mathf.Lerp(velocity, b, unscaledDeltaTime * 10f);
			}
			if (!Mathf.Approximately(currentScrollPosition, m_PrevScrollPosition))
			{
				m_PrevScrollPosition = currentScrollPosition;
			}
			State state = (dragging || m_AutoScrolling || m_KeepAutoScrolling) ? State.Dynamic : State.Static;
			if (state != this.state)
			{
				this.state = state;
				if (this.OnStateChange != null)
				{
					this.OnStateChange(state);
				}
			}
		}

		private OnFancyScrollViewCellUpdate CreateNewCell(int dataIndex)
		{
			if (m_PoolPlaceHoldPrefabs != null)
			{
				int result = cellCount.GetResult<int>();
				if (result < minCellCount.GetResult<int>() && dataIndex >= result)
				{
					return PackagePlaceHolderCellGo(m_PoolPlaceHoldPrefabs.FastInstantiate());
				}
			}
			return (m_PoolPrefabs == null) ? null : PackageCellGo(m_PoolPrefabs.FastInstantiate());
		}

		private void UpdatePosition(float position)
		{
			float arg = currentScrollPosition - position;
			currentScrollPosition = position;
			float num = cellOffset / cellInterval;
			float num2 = position - num;
			float num3 = (Mathf.Ceil(num2) - num2) * cellInterval;
			int num4 = Mathf.CeilToInt(num2);
			int num5 = (!loop) ? Mathf.Clamp(Mathf.RoundToInt(currentScrollPosition), 0, m_CellCount - 1) : GetLoopIndex(Mathf.RoundToInt(currentScrollPosition));
			int num6 = 0;
			int[] array = m_Cells.Keys.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				m_Cells[array[i]].isInViewport = false;
			}
			for (float num7 = num3; num7 <= 1f; num7 += cellInterval)
			{
				int num8 = num4 + num6;
				int num9 = num8;
				if (loop)
				{
					num9 = GetLoopIndex(num8);
				}
				else if (num8 < 0 || num8 > m_CellCount - 1)
				{
					num9 = -1;
				}
				if (num9 >= 0)
				{
					OnFancyScrollViewCellUpdate value;
					if (!m_Cells.TryGetValue(num8, out value))
					{
						if (sourceType == SourceType.Prefabs && !disableAutoCache)
						{
							value = CreateNewCell(num9);
							m_Cells[num8] = value;
							UpdateCellData(value, num9);
						}
						else if (sourceType == SourceType.Children && movementType == MovementType.Loop)
						{
							value = m_Cells[GetLoopIndex(num9)];
						}
					}
					if ((bool)value)
					{
						value.isInViewport = true;
						UpdateCellVisibleByIndex(value, num8);
						value.UpdatePosition(m_AnimNameHash, num7);
					}
				}
				num6++;
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (!m_Cells[array[j]].isInViewport)
				{
					ReleaseCell(array[j]);
				}
			}
			if (highestPos >= 0f)
			{
				content.SortChildren(delegate(Transform transform1, Transform transform2)
				{
					float f = transform1.GetComponentInChildren<OnFancyScrollViewCellUpdate>().animPos - highestPos;
					float f2 = transform2.GetComponentInChildren<OnFancyScrollViewCellUpdate>().animPos - highestPos;
					float num10 = Mathf.Abs(f);
					float num11 = Mathf.Abs(f2);
					return Math.Sign(num11 - num10);
				});
			}
			if (processBar != null)
			{
				if (loop && m_CellCount - 1 == 0)
				{
					processBar.value = 0f;
				}
				else
				{
					processBar.value = ((!loop) ? currentScrollPosition : GetLoopPosition(currentScrollPosition)) / (float)(m_CellCount - 1);
				}
			}
			if (this.onUpdatePosition != null)
			{
				this.onUpdatePosition(arg);
			}
			if (selectItemIndex != num5)
			{
				if (!m_FirstInit && audioClip != null)
				{
					Singleton<AudioManager>.instance.PlayOneShot(audioClip, (m_Volume == null) ? 1f : m_Volume.GetResult<float>());
				}
				if (disableButtonWhenReachBorder)
				{
					if (m_CellCount == 1)
					{
						if (btnPrevious != null && btnPrevious.gameObject.activeSelf)
						{
							btnPrevious.gameObject.SetActive(false);
						}
						if (btnNext != null && btnNext.gameObject.activeSelf)
						{
							btnNext.gameObject.SetActive(false);
						}
					}
					else if (num5 == 0)
					{
						if (btnPrevious != null && btnPrevious.gameObject.activeSelf)
						{
							btnPrevious.gameObject.SetActive(false);
						}
						if (btnNext != null && !btnNext.gameObject.activeSelf)
						{
							btnNext.gameObject.SetActive(true);
						}
					}
					else if (num5 == m_CellCount - 1)
					{
						if (btnNext != null && btnNext.gameObject.activeSelf)
						{
							btnNext.gameObject.SetActive(false);
						}
						if (btnPrevious != null && !btnPrevious.gameObject.activeSelf)
						{
							btnPrevious.gameObject.SetActive(true);
						}
					}
					else
					{
						if (btnPrevious != null && !btnPrevious.gameObject.activeSelf)
						{
							btnPrevious.gameObject.SetActive(true);
						}
						if (btnNext != null && !btnNext.gameObject.activeSelf)
						{
							btnNext.gameObject.SetActive(true);
						}
					}
				}
				if (this.onItemIndexChange != null)
				{
					this.onItemIndexChange(num5);
				}
				selectItemIndex = num5;
			}
			if (!dragging && !m_AutoScrolling && !m_KeepAutoScrolling && m_FinalSelectItemIndex != selectItemIndex)
			{
				m_FinalSelectItemIndex = selectItemIndex;
				if (this.onFinalItemIndexChange != null)
				{
					this.onFinalItemIndexChange(m_FinalSelectItemIndex);
				}
			}
		}

		private void OnScrollBarDragBegin()
		{
			m_PointerStartLocalPosition = Vector2.zero;
			m_DragStartScrollPosition = currentScrollPosition;
			dragging = true;
			m_AutoScrolling = false;
		}

		private void OnScrollBarValueChange()
		{
			if (dragging)
			{
				float position = processBar.value * (float)(m_CellCount - 1);
				if (loop)
				{
					position = GetLoopPosition(position);
				}
				UpdatePositionWithScrollBar(position);
			}
		}

		private void OnScrollBarDragEnd()
		{
			if (dragging)
			{
				dragging = false;
				isScrollBarDrag = true;
			}
		}

		private void OnPointerDownScrollBar()
		{
			m_AutoScrolling = false;
			dragging = true;
		}

		private void OnPointerUpScrollBar()
		{
			float position = processBar.value * (float)(m_CellCount - 1);
			if (loop)
			{
				position = GetLoopPosition(position);
			}
			UpdatePositionWithScrollBar(position);
			dragging = false;
			isScrollBarDrag = true;
			m_PointerDownToScroll = true;
		}

		private void UpdatePositionWithScrollBar(float position)
		{
			float arg = currentScrollPosition - position;
			currentScrollPosition = position;
			float num = cellOffset / cellInterval;
			float num2 = position - num;
			float num3 = (Mathf.Ceil(num2) - num2) * cellInterval;
			int num4 = Mathf.CeilToInt(num2);
			int num5 = (!loop) ? Mathf.Clamp(Mathf.RoundToInt(currentScrollPosition), 0, m_CellCount - 1) : GetLoopIndex(Mathf.RoundToInt(currentScrollPosition));
			int num6 = 0;
			int[] array = m_Cells.Keys.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				m_Cells[array[i]].isInViewport = false;
			}
			for (float num7 = num3; num7 <= 1f; num7 += cellInterval)
			{
				int num8 = num4 + num6;
				int num9 = num8;
				if (loop)
				{
					num9 = GetLoopIndex(num8);
				}
				else if (num8 < 0 || num8 > m_CellCount - 1)
				{
					num9 = -1;
				}
				if (num9 >= 0)
				{
					OnFancyScrollViewCellUpdate value;
					if (!m_Cells.TryGetValue(num8, out value))
					{
						if (sourceType == SourceType.Prefabs && !disableAutoCache)
						{
							value = CreateNewCell(num9);
							m_Cells[num8] = value;
							UpdateCellData(value, num9);
						}
						else if (sourceType == SourceType.Children && movementType == MovementType.Loop)
						{
							value = m_Cells[GetLoopIndex(num9)];
						}
					}
					if ((bool)value)
					{
						value.isInViewport = true;
						UpdateCellVisibleByIndex(value, num8);
						value.UpdatePosition(m_AnimNameHash, num7);
					}
				}
				num6++;
			}
			for (int j = 0; j < array.Length; j++)
			{
				if (!m_Cells[array[j]].isInViewport)
				{
					ReleaseCell(array[j]);
				}
			}
			if (highestPos >= 0f)
			{
				content.SortChildren(delegate(Transform transform1, Transform transform2)
				{
					float f = transform1.GetComponentInChildren<OnFancyScrollViewCellUpdate>().animPos - highestPos;
					float f2 = transform2.GetComponentInChildren<OnFancyScrollViewCellUpdate>().animPos - highestPos;
					float num10 = Mathf.Abs(f);
					float num11 = Mathf.Abs(f2);
					return Math.Sign(num11 - num10);
				});
			}
			if (this.onUpdatePosition != null)
			{
				this.onUpdatePosition(arg);
			}
			if (selectItemIndex != num5)
			{
				if (!m_FirstInit && audioClip != null)
				{
					Singleton<AudioManager>.instance.PlayOneShot(audioClip, (m_Volume == null) ? 1f : m_Volume.GetResult<float>());
				}
				if (disableButtonWhenReachBorder)
				{
					if (m_CellCount == 1)
					{
						if (btnPrevious != null && btnPrevious.gameObject.activeSelf)
						{
							btnPrevious.gameObject.SetActive(false);
						}
						if (btnNext != null && btnNext.gameObject.activeSelf)
						{
							btnNext.gameObject.SetActive(false);
						}
					}
					else if (num5 == 0)
					{
						if (btnPrevious != null && btnPrevious.gameObject.activeSelf)
						{
							btnPrevious.gameObject.SetActive(false);
						}
						if (btnNext != null && !btnNext.gameObject.activeSelf)
						{
							btnNext.gameObject.SetActive(true);
						}
					}
					else if (num5 == m_CellCount - 1)
					{
						if (btnNext != null && btnNext.gameObject.activeSelf)
						{
							btnNext.gameObject.SetActive(false);
						}
						if (btnPrevious != null && !btnPrevious.gameObject.activeSelf)
						{
							btnPrevious.gameObject.SetActive(true);
						}
					}
					else
					{
						if (btnPrevious != null && !btnPrevious.gameObject.activeSelf)
						{
							btnPrevious.gameObject.SetActive(true);
						}
						if (btnNext != null && !btnNext.gameObject.activeSelf)
						{
							btnNext.gameObject.SetActive(true);
						}
					}
				}
				if (this.onItemIndexChange != null)
				{
					this.onItemIndexChange(num5);
				}
				selectItemIndex = num5;
			}
			if (!dragging && !m_AutoScrolling && !m_KeepAutoScrolling && m_FinalSelectItemIndex != selectItemIndex)
			{
				m_FinalSelectItemIndex = selectItemIndex;
				if (this.onFinalItemIndexChange != null)
				{
					this.onFinalItemIndexChange(m_FinalSelectItemIndex);
				}
			}
		}

		private void InitCommonData()
		{
			m_HasInit = true;
			InitData();
			int num = (startIndex == null) ? (-1) : Mathf.Clamp(startIndex.GetResult<int>(), 0, m_CellCount - 1);
			if (num >= 0)
			{
				currentScrollPosition = num;
			}
			if (base.isActiveAndEnabled)
			{
				UpdatePosition(currentScrollPosition);
			}
			if (processBar != null && (bool)processBar.GetComponent<FSVScrollBar>())
			{
				FSVScrollBar component = processBar.GetComponent<FSVScrollBar>();
				component.onDragBeginEvent = (FSVScrollBar.OnDragBeginDelegate)Delegate.Combine(component.onDragBeginEvent, new FSVScrollBar.OnDragBeginDelegate(OnScrollBarDragBegin));
				FSVScrollBar component2 = processBar.GetComponent<FSVScrollBar>();
				component2.onDragEvent = (FSVScrollBar.OnDragDelegate)Delegate.Combine(component2.onDragEvent, new FSVScrollBar.OnDragDelegate(OnScrollBarValueChange));
				FSVScrollBar component3 = processBar.GetComponent<FSVScrollBar>();
				component3.onDragEndEvent = (FSVScrollBar.OnDragEndDelegate)Delegate.Combine(component3.onDragEndEvent, new FSVScrollBar.OnDragEndDelegate(OnScrollBarDragEnd));
				FSVScrollBar component4 = processBar.GetComponent<FSVScrollBar>();
				component4.onPointerDown = (FSVScrollBar.OnPointerDownDelegate)Delegate.Combine(component4.onPointerDown, new FSVScrollBar.OnPointerDownDelegate(OnPointerDownScrollBar));
				FSVScrollBar component5 = processBar.GetComponent<FSVScrollBar>();
				component5.onPointerUp = (FSVScrollBar.OnPointerDownDelegate)Delegate.Combine(component5.onPointerUp, new FSVScrollBar.OnPointerDownDelegate(OnPointerUpScrollBar));
			}
		}

		private void InitData()
		{
			selectItemIndex = -1;
			m_FinalSelectItemIndex = -1;
			dragging = false;
			m_LongPressing = false;
			state = State.Static;
			m_AutoScrolling = false;
			m_KeepAutoScrolling = false;
			velocity = 0f;
		}

		private void ReleaseAllCell()
		{
			if (m_Cells != null)
			{
				int[] array = m_Cells.Keys.ToArray();
				for (int i = 0; i < array.Length; i++)
				{
					ReleaseCell(array[i]);
				}
			}
		}

		private void DestroyAllChildren()
		{
			if (m_Cells == null)
			{
				return;
			}
			foreach (KeyValuePair<int, OnFancyScrollViewCellUpdate> cell in m_Cells)
			{
				cell.Value.transform.SetParent(null);
				UnityEngine.Object.Destroy(cell.Value.gameObject);
			}
			m_Cells.Clear();
		}

		private void ReleaseCell(int cellIndex)
		{
			OnFancyScrollViewCellUpdate value;
			if (!m_Cells.TryGetValue(cellIndex, out value))
			{
				return;
			}
			if (sourceType == SourceType.Prefabs && !disableAutoCache)
			{
				m_Cells.Remove(cellIndex);
				if (!value.isPlaceHolder)
				{
					m_PoolPrefabs.FastDestroy(value.gameObject);
				}
				else
				{
					m_PoolPlaceHoldPrefabs.FastDestroy(value.gameObject);
				}
			}
			else
			{
				value.SetVisible(false);
			}
		}

		private OnFancyScrollViewCellUpdate PackageCellGo(GameObject cellGo)
		{
			cellGo.transform.SetParent(content);
			cellGo.transform.localPosition = Vector3.zero;
			cellGo.transform.localScale = Vector3.one;
			OnFancyScrollViewCellUpdate orAddComponent = cellGo.GetOrAddComponent<OnFancyScrollViewCellUpdate>();
			orAddComponent.isPlaceHolder = false;
			orAddComponent.Init();
			return orAddComponent;
		}

		private OnFancyScrollViewCellUpdate PackagePlaceHolderCellGo(GameObject cellGo)
		{
			OnFancyScrollViewCellUpdate onFancyScrollViewCellUpdate = PackageCellGo(cellGo);
			onFancyScrollViewCellUpdate.isPlaceHolder = true;
			return onFancyScrollViewCellUpdate;
		}

		private float GetViewportSize()
		{
			float result;
			if (dragDirection == ScrollDirection.Horizontal)
			{
				Vector2 size = viewPort.rect.size;
				result = size.x;
			}
			else
			{
				Vector2 size2 = viewPort.rect.size;
				result = size2.y;
			}
			return result;
		}

		private int CalculateTarget(float currentPosition, float velocity, float delta)
		{
			float num = Mathf.Sign(currentPosition - m_DragStartScrollPosition);
			float f = currentPosition + num * Mathf.Abs(velocity) * delta;
			int num2 = (!(num >= 0f)) ? Mathf.FloorToInt(f) : Mathf.CeilToInt(f);
			int result = (!loop) ? Mathf.Clamp(num2, 0, m_CellCount - 1) : num2;
			if (isScrollBarDrag)
			{
				result = Mathf.Clamp(num2, 0, m_CellCount - 1);
			}
			return result;
		}

		private float CalculateOffset(float position)
		{
			if (movementType == MovementType.Loop)
			{
				return 0f;
			}
			if (position < 0f)
			{
				return 0f - position;
			}
			if (position > (float)(m_CellCount - 1))
			{
				return (float)(m_CellCount - 1) - position;
			}
			return 0f;
		}

		private void UpdateCellData(OnFancyScrollViewCellUpdate cell, int dataIndex)
		{
			if (loop || (dataIndex >= 0 && dataIndex <= m_CellCount - 1))
			{
				cell.UpdateData(dataIndex);
			}
		}

		private void UpdateCellVisibleByIndex(OnFancyScrollViewCellUpdate cell, int cellIndex)
		{
			if (!loop && (cellIndex < 0 || cellIndex > m_CellCount - 1))
			{
				cell.SetVisible(false);
			}
			else
			{
				cell.SetVisible(true);
			}
		}

		private float RubberDelta(float overStretching, float viewSize)
		{
			return (1f - 1f / (Mathf.Abs(overStretching) * 0.55f / viewSize + 1f)) * viewSize * Mathf.Sign(overStretching) - overStretching * 0.05f;
		}

		private float CalculateClosestPosition(float scrollPosition, int dictTendFlag)
		{
			float num = GetLoopPosition(scrollPosition) - GetLoopPosition(currentScrollPosition);
			if ((m_CellCount == 1 || m_CellCount == 2) && !Mathf.Approximately(dictTendFlag, 0f))
			{
				float num2 = Mathf.Sign(dictTendFlag);
				float num3 = Mathf.Sign(dictTendFlag) * (float)Mathf.Abs(1);
				return (!(num2 > 0f)) ? Mathf.FloorToInt(currentScrollPosition + num3) : Mathf.CeilToInt(currentScrollPosition + num3);
			}
			if (Mathf.Abs(num) > (float)m_CellCount * 0.5f)
			{
				num = Mathf.Sign(0f - num) * ((float)m_CellCount - Mathf.Abs(num));
			}
			return num + currentScrollPosition;
		}

		private float EaseInOutCubic(float start, float end, float value)
		{
			value /= 0.5f;
			end -= start;
			if (value < 1f)
			{
				return end * 0.5f * value * value * value + start;
			}
			value -= 2f;
			return end * 0.5f * (value * value * value + 2f) + start;
		}
	}
}
