using Rewired.Components;
using Rewired.UI;
using Rewired.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Rewired.Integration.UnityUI
{
	[AddComponentMenu("Event/Rewired Standalone Input Module")]
	public sealed class RewiredStandaloneInputModule : RewiredPointerInputModule
	{
		[Serializable]
		public class PlayerSetting
		{
			public int playerId;

			public List<Rewired.Components.PlayerMouse> playerMice = new List<Rewired.Components.PlayerMouse>();

			public PlayerSetting()
			{
			}

			private PlayerSetting(PlayerSetting other)
			{
				if (other == null)
				{
					throw new ArgumentNullException("other");
				}
				playerId = other.playerId;
				playerMice = new List<Rewired.Components.PlayerMouse>();
				if (other.playerMice == null)
				{
					return;
				}
				foreach (Rewired.Components.PlayerMouse playerMouse in other.playerMice)
				{
					playerMice.Add(playerMouse);
				}
			}

			public PlayerSetting Clone()
			{
				return new PlayerSetting(this);
			}
		}

		private const string DEFAULT_ACTION_MOVE_HORIZONTAL = "UIHorizontal";

		private const string DEFAULT_ACTION_MOVE_VERTICAL = "UIVertical";

		private const string DEFAULT_ACTION_SUBMIT = "UISubmit";

		private const string DEFAULT_ACTION_CANCEL = "UICancel";

		[Tooltip("(Optional) Link the Rewired Input Manager here for easier access to Player ids, etc.")]
		[SerializeField]
		private InputManager_Base rewiredInputManager;

		[SerializeField]
		[Tooltip("Use all Rewired game Players to control the UI. This does not include the System Player. If enabled, this setting overrides individual Player Ids set in Rewired Player Ids.")]
		private bool useAllRewiredGamePlayers;

		[SerializeField]
		[Tooltip("Allow the Rewired System Player to control the UI.")]
		private bool useRewiredSystemPlayer;

		[SerializeField]
		[Tooltip("A list of Player Ids that are allowed to control the UI. If Use All Rewired Game Players = True, this list will be ignored.")]
		private int[] rewiredPlayerIds = new int[1];

		[SerializeField]
		[Tooltip("Allow only Players with Player.isPlaying = true to control the UI.")]
		private bool usePlayingPlayersOnly;

		[SerializeField]
		[Tooltip("Player Mice allowed to interact with the UI. Each Player that owns a Player Mouse must also be allowed to control the UI or the Player Mouse will not function.")]
		private List<Rewired.Components.PlayerMouse> playerMice = new List<Rewired.Components.PlayerMouse>();

		[SerializeField]
		[Tooltip("Makes an axis press always move only one UI selection. Enable if you do not want to allow scrolling through UI elements by holding an axis direction.")]
		private bool moveOneElementPerAxisPress;

		[SerializeField]
		[Tooltip("If enabled, Action Ids will be used to set the Actions. If disabled, string names will be used to set the Actions.")]
		private bool setActionsById;

		[SerializeField]
		[Tooltip("Id of the horizontal Action for movement (if axis events are used).")]
		private int horizontalActionId = -1;

		[SerializeField]
		[Tooltip("Id of the vertical Action for movement (if axis events are used).")]
		private int verticalActionId = -1;

		[SerializeField]
		[Tooltip("Id of the Action used to submit.")]
		private int submitActionId = -1;

		[SerializeField]
		[Tooltip("Id of the Action used to cancel.")]
		private int cancelActionId = -1;

		[SerializeField]
		[Tooltip("Name of the horizontal axis for movement (if axis events are used).")]
		private string m_HorizontalAxis = "UIHorizontal";

		[SerializeField]
		[Tooltip("Name of the vertical axis for movement (if axis events are used).")]
		private string m_VerticalAxis = "UIVertical";

		[SerializeField]
		[Tooltip("Name of the action used to submit.")]
		private string m_SubmitButton = "UISubmit";

		[SerializeField]
		[Tooltip("Name of the action used to cancel.")]
		private string m_CancelButton = "UICancel";

		[SerializeField]
		[Tooltip("Number of selection changes allowed per second when a movement button/axis is held in a direction.")]
		private float m_InputActionsPerSecond = 10f;

		[SerializeField]
		[Tooltip("Delay in seconds before vertical/horizontal movement starts repeating continouously when a movement direction is held.")]
		private float m_RepeatDelay;

		[SerializeField]
		[Tooltip("Allows the mouse to be used to select elements.")]
		private bool m_allowMouseInput = true;

		[SerializeField]
		[Tooltip("Allows the mouse to be used to select elements if the device also supports touch control.")]
		private bool m_allowMouseInputIfTouchSupported = true;

		[SerializeField]
		[Tooltip("Allows touch input to be used to select elements.")]
		private bool m_allowTouchInput = true;

		[SerializeField]
		[FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
		[Tooltip("Forces the module to always be active.")]
		private bool m_ForceModuleActive;

		[NonSerialized]
		private int[] playerIds;

		private bool recompiling;

		[NonSerialized]
		private bool isTouchSupported;

		[NonSerialized]
		private float m_PrevActionTime;

		[NonSerialized]
		private Vector2 m_LastMoveVector;

		[NonSerialized]
		private int m_ConsecutiveMoveCount;

		[NonSerialized]
		private bool m_HasFocus = true;

		public InputManager_Base RewiredInputManager
		{
			get
			{
				return rewiredInputManager;
			}
			set
			{
				rewiredInputManager = value;
			}
		}

		public bool UseAllRewiredGamePlayers
		{
			get
			{
				return useAllRewiredGamePlayers;
			}
			set
			{
				bool flag = value != useAllRewiredGamePlayers;
				useAllRewiredGamePlayers = value;
				if (flag)
				{
					SetupRewiredVars();
				}
			}
		}

		public bool UseRewiredSystemPlayer
		{
			get
			{
				return useRewiredSystemPlayer;
			}
			set
			{
				bool flag = value != useRewiredSystemPlayer;
				useRewiredSystemPlayer = value;
				if (flag)
				{
					SetupRewiredVars();
				}
			}
		}

		public int[] RewiredPlayerIds
		{
			get
			{
				return (int[])rewiredPlayerIds.Clone();
			}
			set
			{
				rewiredPlayerIds = ((value == null) ? new int[0] : ((int[])value.Clone()));
				SetupRewiredVars();
			}
		}

		public bool UsePlayingPlayersOnly
		{
			get
			{
				return usePlayingPlayersOnly;
			}
			set
			{
				usePlayingPlayersOnly = value;
			}
		}

		public List<Rewired.Components.PlayerMouse> PlayerMice
		{
			get
			{
				return new List<Rewired.Components.PlayerMouse>(playerMice);
			}
			set
			{
				if (value == null)
				{
					playerMice = new List<Rewired.Components.PlayerMouse>();
					SetupRewiredVars();
				}
				else
				{
					playerMice = new List<Rewired.Components.PlayerMouse>(value);
					SetupRewiredVars();
				}
			}
		}

		public bool MoveOneElementPerAxisPress
		{
			get
			{
				return moveOneElementPerAxisPress;
			}
			set
			{
				moveOneElementPerAxisPress = value;
			}
		}

		public bool allowMouseInput
		{
			get
			{
				return m_allowMouseInput;
			}
			set
			{
				m_allowMouseInput = value;
			}
		}

		public bool allowMouseInputIfTouchSupported
		{
			get
			{
				return m_allowMouseInputIfTouchSupported;
			}
			set
			{
				m_allowMouseInputIfTouchSupported = value;
			}
		}

		public bool allowTouchInput
		{
			get
			{
				return m_allowTouchInput;
			}
			set
			{
				m_allowTouchInput = value;
			}
		}

		public bool SetActionsById
		{
			get
			{
				return setActionsById;
			}
			set
			{
				if (setActionsById != value)
				{
					setActionsById = value;
					SetupRewiredVars();
				}
			}
		}

		public int HorizontalActionId
		{
			get
			{
				return horizontalActionId;
			}
			set
			{
				if (value != horizontalActionId)
				{
					horizontalActionId = value;
					if (ReInput.isReady)
					{
						m_HorizontalAxis = ((ReInput.mapping.GetAction(value) == null) ? string.Empty : ReInput.mapping.GetAction(value).name);
					}
				}
			}
		}

		public int VerticalActionId
		{
			get
			{
				return verticalActionId;
			}
			set
			{
				if (value != verticalActionId)
				{
					verticalActionId = value;
					if (ReInput.isReady)
					{
						m_VerticalAxis = ((ReInput.mapping.GetAction(value) == null) ? string.Empty : ReInput.mapping.GetAction(value).name);
					}
				}
			}
		}

		public int SubmitActionId
		{
			get
			{
				return submitActionId;
			}
			set
			{
				if (value != submitActionId)
				{
					submitActionId = value;
					if (ReInput.isReady)
					{
						m_SubmitButton = ((ReInput.mapping.GetAction(value) == null) ? string.Empty : ReInput.mapping.GetAction(value).name);
					}
				}
			}
		}

		public int CancelActionId
		{
			get
			{
				return cancelActionId;
			}
			set
			{
				if (value != cancelActionId)
				{
					cancelActionId = value;
					if (ReInput.isReady)
					{
						m_CancelButton = ((ReInput.mapping.GetAction(value) == null) ? string.Empty : ReInput.mapping.GetAction(value).name);
					}
				}
			}
		}

		protected override bool isMouseSupported
		{
			get
			{
				if (!base.isMouseSupported)
				{
					return false;
				}
				if (!m_allowMouseInput)
				{
					return false;
				}
				return !isTouchSupported || m_allowMouseInputIfTouchSupported;
			}
		}

		private bool isTouchAllowed => m_allowTouchInput;

		[Obsolete("allowActivationOnMobileDevice has been deprecated. Use forceModuleActive instead")]
		public bool allowActivationOnMobileDevice
		{
			get
			{
				return m_ForceModuleActive;
			}
			set
			{
				m_ForceModuleActive = value;
			}
		}

		public bool forceModuleActive
		{
			get
			{
				return m_ForceModuleActive;
			}
			set
			{
				m_ForceModuleActive = value;
			}
		}

		public float inputActionsPerSecond
		{
			get
			{
				return m_InputActionsPerSecond;
			}
			set
			{
				m_InputActionsPerSecond = value;
			}
		}

		public float repeatDelay
		{
			get
			{
				return m_RepeatDelay;
			}
			set
			{
				m_RepeatDelay = value;
			}
		}

		public string horizontalAxis
		{
			get
			{
				return m_HorizontalAxis;
			}
			set
			{
				if (!(m_HorizontalAxis == value))
				{
					m_HorizontalAxis = value;
					if (ReInput.isReady)
					{
						horizontalActionId = ReInput.mapping.GetActionId(value);
					}
				}
			}
		}

		public string verticalAxis
		{
			get
			{
				return m_VerticalAxis;
			}
			set
			{
				if (!(m_VerticalAxis == value))
				{
					m_VerticalAxis = value;
					if (ReInput.isReady)
					{
						verticalActionId = ReInput.mapping.GetActionId(value);
					}
				}
			}
		}

		public string submitButton
		{
			get
			{
				return m_SubmitButton;
			}
			set
			{
				if (!(m_SubmitButton == value))
				{
					m_SubmitButton = value;
					if (ReInput.isReady)
					{
						submitActionId = ReInput.mapping.GetActionId(value);
					}
				}
			}
		}

		public string cancelButton
		{
			get
			{
				return m_CancelButton;
			}
			set
			{
				if (!(m_CancelButton == value))
				{
					m_CancelButton = value;
					if (ReInput.isReady)
					{
						cancelActionId = ReInput.mapping.GetActionId(value);
					}
				}
			}
		}

		private RewiredStandaloneInputModule()
		{
		}

		protected override void Awake()
		{
			base.Awake();
			isTouchSupported = base.defaultTouchInputSource.touchSupported;
			TouchInputModule component = GetComponent<TouchInputModule>();
			if (component != null)
			{
				component.enabled = false;
			}
			ReInput.InitializedEvent += OnRewiredInitialized;
			InitializeRewired();
		}

		public override void UpdateModule()
		{
			CheckEditorRecompile();
			if (!recompiling && ReInput.isReady && !m_HasFocus && !ShouldIgnoreEventsOnNoFocus())
			{
			}
		}

		public override bool IsModuleSupported()
		{
			return true;
		}

		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule())
			{
				return false;
			}
			if (recompiling)
			{
				return false;
			}
			if (!ReInput.isReady)
			{
				return false;
			}
			bool flag = m_ForceModuleActive;
			for (int i = 0; i < playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(playerIds[i]);
				if (player != null && (!usePlayingPlayersOnly || player.isPlaying))
				{
					flag |= GetButtonDown(player, submitActionId);
					flag |= GetButtonDown(player, cancelActionId);
					if (moveOneElementPerAxisPress)
					{
						flag |= (GetButtonDown(player, horizontalActionId) || GetNegativeButtonDown(player, horizontalActionId));
						flag |= (GetButtonDown(player, verticalActionId) || GetNegativeButtonDown(player, verticalActionId));
					}
					else
					{
						flag |= !Mathf.Approximately(GetAxis(player, horizontalActionId), 0f);
						flag |= !Mathf.Approximately(GetAxis(player, verticalActionId), 0f);
					}
				}
			}
			if (isMouseSupported)
			{
				flag |= DidAnyMouseMove();
				flag |= GetMouseButtonDownOnAnyMouse(0);
			}
			if (isTouchAllowed)
			{
				for (int j = 0; j < base.defaultTouchInputSource.touchCount; j++)
				{
					Touch touch = base.defaultTouchInputSource.GetTouch(j);
					flag |= (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary);
				}
			}
			return flag;
		}

		public override void ActivateModule()
		{
			if (m_HasFocus || !ShouldIgnoreEventsOnNoFocus())
			{
				base.ActivateModule();
				GameObject gameObject = base.eventSystem.currentSelectedGameObject;
				if (gameObject == null)
				{
					gameObject = base.eventSystem.firstSelectedGameObject;
				}
				base.eventSystem.SetSelectedGameObject(gameObject, GetBaseEventData());
			}
		}

		public override void DeactivateModule()
		{
			base.DeactivateModule();
			ClearSelection();
		}

		public override void Process()
		{
			if (!ReInput.isReady || (!m_HasFocus && ShouldIgnoreEventsOnNoFocus()) || !base.enabled || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			bool flag = SendUpdateEventToSelectedObject();
			if (base.eventSystem.sendNavigationEvents)
			{
				if (!flag)
				{
					flag |= SendMoveEventToSelectedObject();
				}
				if (!flag)
				{
					SendSubmitEventToSelectedObject();
				}
			}
			if (!ProcessTouchEvents() && isMouseSupported)
			{
				ProcessMouseEvents();
			}
		}

		private bool ProcessTouchEvents()
		{
			if (!isTouchAllowed)
			{
				return false;
			}
			for (int i = 0; i < base.defaultTouchInputSource.touchCount; i++)
			{
				Touch touch = base.defaultTouchInputSource.GetTouch(i);
				if (touch.type != TouchType.Indirect)
				{
					bool pressed;
					bool released;
					PlayerPointerEventData touchPointerEventData = GetTouchPointerEventData(0, 0, touch, out pressed, out released);
					ProcessTouchPress(touchPointerEventData, pressed, released);
					if (!released)
					{
						ProcessMove(touchPointerEventData);
						ProcessDrag(touchPointerEventData);
					}
					else
					{
						RemovePointerData(touchPointerEventData);
					}
				}
			}
			return base.defaultTouchInputSource.touchCount > 0;
		}

		private void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
		{
			GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
			if (pressed)
			{
				pointerEvent.eligibleForClick = true;
				pointerEvent.delta = Vector2.zero;
				pointerEvent.dragging = false;
				pointerEvent.useDragThreshold = true;
				pointerEvent.pressPosition = pointerEvent.position;
				pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
				DeselectIfSelectionChanged(gameObject, pointerEvent);
				if (pointerEvent.pointerEnter != gameObject)
				{
					HandlePointerExitAndEnter(pointerEvent, gameObject);
					pointerEvent.pointerEnter = gameObject;
				}
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy(gameObject, pointerEvent, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == pointerEvent.lastPress)
				{
					float num = unscaledTime - pointerEvent.clickTime;
					if (num < 0.3f)
					{
						pointerEvent.clickCount++;
					}
					else
					{
						pointerEvent.clickCount = 1;
					}
					pointerEvent.clickTime = unscaledTime;
				}
				else
				{
					pointerEvent.clickCount = 1;
				}
				pointerEvent.pointerPress = gameObject2;
				pointerEvent.rawPointerPress = gameObject;
				pointerEvent.clickTime = unscaledTime;
				pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (released)
			{
				ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (pointerEvent.pointerPress == eventHandler && pointerEvent.eligibleForClick)
				{
					ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
				}
				else if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.ExecuteHierarchy(gameObject, pointerEvent, ExecuteEvents.dropHandler);
				}
				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = null;
				pointerEvent.rawPointerPress = null;
				if (pointerEvent.pointerDrag != null && pointerEvent.dragging)
				{
					ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.dragging = false;
				pointerEvent.pointerDrag = null;
				if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				}
				pointerEvent.pointerDrag = null;
				ExecuteEvents.ExecuteHierarchy(pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
				pointerEvent.pointerEnter = null;
			}
		}

		private bool SendSubmitEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			if (recompiling)
			{
				return false;
			}
			BaseEventData baseEventData = GetBaseEventData();
			for (int i = 0; i < playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(playerIds[i]);
				if (player != null && (!usePlayingPlayersOnly || player.isPlaying))
				{
					if (GetButtonDown(player, submitActionId))
					{
						ExecuteEvents.Execute(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.submitHandler);
						break;
					}
					if (GetButtonDown(player, cancelActionId))
					{
						ExecuteEvents.Execute(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.cancelHandler);
						break;
					}
				}
			}
			return baseEventData.used;
		}

		private Vector2 GetRawMoveVector()
		{
			if (recompiling)
			{
				return Vector2.zero;
			}
			Vector2 zero = Vector2.zero;
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(playerIds[i]);
				if (player == null || (usePlayingPlayersOnly && !player.isPlaying))
				{
					continue;
				}
				if (moveOneElementPerAxisPress)
				{
					float num = 0f;
					if (GetButtonDown(player, horizontalActionId))
					{
						num = 1f;
					}
					else if (GetNegativeButtonDown(player, horizontalActionId))
					{
						num = -1f;
					}
					float num2 = 0f;
					if (GetButtonDown(player, verticalActionId))
					{
						num2 = 1f;
					}
					else if (GetNegativeButtonDown(player, verticalActionId))
					{
						num2 = -1f;
					}
					zero.x += num;
					zero.y += num2;
				}
				else
				{
					zero.x += GetAxis(player, horizontalActionId);
					zero.y += GetAxis(player, verticalActionId);
				}
				flag |= (GetButtonDown(player, horizontalActionId) || GetNegativeButtonDown(player, horizontalActionId));
				flag2 |= (GetButtonDown(player, verticalActionId) || GetNegativeButtonDown(player, verticalActionId));
			}
			if (flag)
			{
				if (zero.x < 0f)
				{
					zero.x = -1f;
				}
				if (zero.x > 0f)
				{
					zero.x = 1f;
				}
			}
			if (flag2)
			{
				if (zero.y < 0f)
				{
					zero.y = -1f;
				}
				if (zero.y > 0f)
				{
					zero.y = 1f;
				}
			}
			return zero;
		}

		private bool SendMoveEventToSelectedObject()
		{
			if (recompiling)
			{
				return false;
			}
			float unscaledTime = Time.unscaledTime;
			Vector2 rawMoveVector = GetRawMoveVector();
			if (Mathf.Approximately(rawMoveVector.x, 0f) && Mathf.Approximately(rawMoveVector.y, 0f))
			{
				m_ConsecutiveMoveCount = 0;
				return false;
			}
			bool flag = Vector2.Dot(rawMoveVector, m_LastMoveVector) > 0f;
			bool downHorizontal;
			bool downVertical;
			CheckButtonOrKeyMovement(unscaledTime, out downHorizontal, out downVertical);
			AxisEventData axisEventData = null;
			bool flag2 = downHorizontal || downVertical;
			if (flag2)
			{
				axisEventData = GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
				MoveDirection moveDir = axisEventData.moveDir;
				flag2 = (((moveDir == MoveDirection.Up || moveDir == MoveDirection.Down) && downVertical) || ((moveDir == MoveDirection.Left || moveDir == MoveDirection.Right) && downHorizontal));
			}
			if (!flag2)
			{
				flag2 = ((!(m_RepeatDelay > 0f)) ? (unscaledTime > m_PrevActionTime + 1f / m_InputActionsPerSecond) : ((!flag || m_ConsecutiveMoveCount != 1) ? (unscaledTime > m_PrevActionTime + 1f / m_InputActionsPerSecond) : (unscaledTime > m_PrevActionTime + m_RepeatDelay)));
			}
			if (!flag2)
			{
				return false;
			}
			if (axisEventData == null)
			{
				axisEventData = GetAxisEventData(rawMoveVector.x, rawMoveVector.y, 0.6f);
			}
			if (axisEventData.moveDir != MoveDirection.None)
			{
				ExecuteEvents.Execute(base.eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
				if (!flag)
				{
					m_ConsecutiveMoveCount = 0;
				}
				if (m_ConsecutiveMoveCount == 0 || (!downHorizontal && !downVertical))
				{
					m_ConsecutiveMoveCount++;
				}
				m_PrevActionTime = unscaledTime;
				m_LastMoveVector = rawMoveVector;
			}
			else
			{
				m_ConsecutiveMoveCount = 0;
			}
			return axisEventData.used;
		}

		private void CheckButtonOrKeyMovement(float time, out bool downHorizontal, out bool downVertical)
		{
			downHorizontal = false;
			downVertical = false;
			for (int i = 0; i < playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(playerIds[i]);
				if (player != null && (!usePlayingPlayersOnly || player.isPlaying))
				{
					downHorizontal |= (GetButtonDown(player, horizontalActionId) || GetNegativeButtonDown(player, horizontalActionId));
					downVertical |= (GetButtonDown(player, verticalActionId) || GetNegativeButtonDown(player, verticalActionId));
				}
			}
		}

		private void ProcessMouseEvents()
		{
			for (int i = 0; i < playerIds.Length; i++)
			{
				Player player = ReInput.players.GetPlayer(playerIds[i]);
				if (player != null && (!usePlayingPlayersOnly || player.isPlaying))
				{
					int mouseInputSourceCount = GetMouseInputSourceCount(playerIds[i]);
					for (int j = 0; j < mouseInputSourceCount; j++)
					{
						ProcessMouseEvent(playerIds[i], j);
					}
				}
			}
		}

		private void ProcessMouseEvent(int playerId, int pointerIndex)
		{
			MouseState mousePointerEventData = GetMousePointerEventData(playerId, pointerIndex);
			if (mousePointerEventData != null)
			{
				MouseButtonEventData eventData = mousePointerEventData.GetButtonState(0).eventData;
				ProcessMousePress(eventData);
				ProcessMove(eventData.buttonData);
				ProcessDrag(eventData.buttonData);
				ProcessMousePress(mousePointerEventData.GetButtonState(1).eventData);
				ProcessDrag(mousePointerEventData.GetButtonState(1).eventData.buttonData);
				ProcessMousePress(mousePointerEventData.GetButtonState(2).eventData);
				ProcessDrag(mousePointerEventData.GetButtonState(2).eventData.buttonData);
				IMouseInputSource mouseInputSource = GetMouseInputSource(playerId, pointerIndex);
				for (int i = 3; i < mouseInputSource.buttonCount; i++)
				{
					ProcessMousePress(mousePointerEventData.GetButtonState(i).eventData);
					ProcessDrag(mousePointerEventData.GetButtonState(i).eventData.buttonData);
				}
				if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0f))
				{
					GameObject eventHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject);
					ExecuteEvents.ExecuteHierarchy(eventHandler, eventData.buttonData, ExecuteEvents.scrollHandler);
				}
			}
		}

		private bool SendUpdateEventToSelectedObject()
		{
			if (base.eventSystem.currentSelectedGameObject == null)
			{
				return false;
			}
			BaseEventData baseEventData = GetBaseEventData();
			ExecuteEvents.Execute(base.eventSystem.currentSelectedGameObject, baseEventData, ExecuteEvents.updateSelectedHandler);
			return baseEventData.used;
		}

		private void ProcessMousePress(MouseButtonEventData data)
		{
			PlayerPointerEventData buttonData = data.buttonData;
			GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;
			if (data.PressedThisFrame())
			{
				buttonData.eligibleForClick = true;
				buttonData.delta = Vector2.zero;
				buttonData.dragging = false;
				buttonData.useDragThreshold = true;
				buttonData.pressPosition = buttonData.position;
				buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;
				DeselectIfSelectionChanged(gameObject, buttonData);
				GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
				if (gameObject2 == null)
				{
					gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				}
				float unscaledTime = Time.unscaledTime;
				if (gameObject2 == buttonData.lastPress)
				{
					float num = unscaledTime - buttonData.clickTime;
					if (num < 0.3f)
					{
						buttonData.clickCount++;
					}
					else
					{
						buttonData.clickCount = 1;
					}
					buttonData.clickTime = unscaledTime;
				}
				else
				{
					buttonData.clickCount = 1;
				}
				buttonData.pointerPress = gameObject2;
				buttonData.rawPointerPress = gameObject;
				buttonData.clickTime = unscaledTime;
				buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
				if (buttonData.pointerDrag != null)
				{
					ExecuteEvents.Execute(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
				}
			}
			if (data.ReleasedThisFrame())
			{
				ExecuteEvents.Execute(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
				GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);
				if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
				{
					ExecuteEvents.Execute(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
				}
				else if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.ExecuteHierarchy(gameObject, buttonData, ExecuteEvents.dropHandler);
				}
				buttonData.eligibleForClick = false;
				buttonData.pointerPress = null;
				buttonData.rawPointerPress = null;
				if (buttonData.pointerDrag != null && buttonData.dragging)
				{
					ExecuteEvents.Execute(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
				}
				buttonData.dragging = false;
				buttonData.pointerDrag = null;
				if (gameObject != buttonData.pointerEnter)
				{
					HandlePointerExitAndEnter(buttonData, null);
					HandlePointerExitAndEnter(buttonData, gameObject);
				}
			}
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			m_HasFocus = hasFocus;
		}

		private bool ShouldIgnoreEventsOnNoFocus()
		{
			if (!ReInput.isReady)
			{
				return true;
			}
			return ReInput.configuration.ignoreInputWhenAppNotInFocus;
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			ReInput.InitializedEvent -= OnRewiredInitialized;
		}

		protected override bool IsDefaultPlayer(int playerId)
		{
			if (playerIds == null)
			{
				return false;
			}
			if (!ReInput.isReady)
			{
				return false;
			}
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < playerIds.Length; j++)
				{
					Player player = ReInput.players.GetPlayer(playerIds[j]);
					if (player != null && (i >= 1 || !usePlayingPlayersOnly || player.isPlaying) && (i >= 2 || player.controllers.hasMouse))
					{
						return playerIds[j] == playerId;
					}
				}
			}
			return false;
		}

		private void InitializeRewired()
		{
			if (!ReInput.isReady)
			{
				Debug.LogError("Rewired is not initialized! Are you missing a Rewired Input Manager in your scene?");
				return;
			}
			ReInput.ShutDownEvent -= OnRewiredShutDown;
			ReInput.ShutDownEvent += OnRewiredShutDown;
			ReInput.EditorRecompileEvent -= OnEditorRecompile;
			ReInput.EditorRecompileEvent += OnEditorRecompile;
			SetupRewiredVars();
		}

		private void SetupRewiredVars()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			SetUpRewiredActions();
			if (useAllRewiredGamePlayers)
			{
				IList<Player> list = (!useRewiredSystemPlayer) ? ReInput.players.Players : ReInput.players.AllPlayers;
				playerIds = new int[list.Count];
				for (int i = 0; i < list.Count; i++)
				{
					playerIds[i] = list[i].id;
				}
			}
			else
			{
				bool flag = false;
				List<int> list2 = new List<int>(rewiredPlayerIds.Length + 1);
				for (int j = 0; j < rewiredPlayerIds.Length; j++)
				{
					Player player = ReInput.players.GetPlayer(rewiredPlayerIds[j]);
					if (player != null && !list2.Contains(player.id))
					{
						list2.Add(player.id);
						if (player.id == 9999999)
						{
							flag = true;
						}
					}
				}
				if (useRewiredSystemPlayer && !flag)
				{
					list2.Insert(0, ReInput.players.GetSystemPlayer().id);
				}
				playerIds = list2.ToArray();
			}
			SetUpRewiredPlayerMice();
		}

		private void SetUpRewiredPlayerMice()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			ClearMouseInputSources();
			for (int i = 0; i < playerMice.Count; i++)
			{
				Rewired.Components.PlayerMouse playerMouse = playerMice[i];
				if (!UnityTools.IsNullOrDestroyed(playerMouse))
				{
					AddMouseInputSource(playerMouse);
				}
			}
		}

		private void SetUpRewiredActions()
		{
			if (!ReInput.isReady)
			{
				return;
			}
			if (!setActionsById)
			{
				horizontalActionId = ReInput.mapping.GetActionId(m_HorizontalAxis);
				verticalActionId = ReInput.mapping.GetActionId(m_VerticalAxis);
				submitActionId = ReInput.mapping.GetActionId(m_SubmitButton);
				cancelActionId = ReInput.mapping.GetActionId(m_CancelButton);
				return;
			}
			InputAction action = ReInput.mapping.GetAction(horizontalActionId);
			m_HorizontalAxis = ((action == null) ? string.Empty : action.name);
			if (action == null)
			{
				horizontalActionId = -1;
			}
			action = ReInput.mapping.GetAction(verticalActionId);
			m_VerticalAxis = ((action == null) ? string.Empty : action.name);
			if (action == null)
			{
				verticalActionId = -1;
			}
			action = ReInput.mapping.GetAction(submitActionId);
			m_SubmitButton = ((action == null) ? string.Empty : action.name);
			if (action == null)
			{
				submitActionId = -1;
			}
			action = ReInput.mapping.GetAction(cancelActionId);
			m_CancelButton = ((action == null) ? string.Empty : action.name);
			if (action == null)
			{
				cancelActionId = -1;
			}
		}

		private bool GetButtonDown(Player player, int actionId)
		{
			if (actionId < 0)
			{
				return false;
			}
			return player.GetButtonDown(actionId);
		}

		private bool GetNegativeButtonDown(Player player, int actionId)
		{
			if (actionId < 0)
			{
				return false;
			}
			return player.GetNegativeButtonDown(actionId);
		}

		private float GetAxis(Player player, int actionId)
		{
			if (actionId < 0)
			{
				return 0f;
			}
			return player.GetAxis(actionId);
		}

		private void CheckEditorRecompile()
		{
			if (recompiling && ReInput.isReady)
			{
				recompiling = false;
				InitializeRewired();
			}
		}

		private void OnEditorRecompile()
		{
			recompiling = true;
			ClearRewiredVars();
		}

		private void ClearRewiredVars()
		{
			Array.Clear(playerIds, 0, playerIds.Length);
			ClearMouseInputSources();
		}

		private bool DidAnyMouseMove()
		{
			for (int i = 0; i < playerIds.Length; i++)
			{
				int playerId = playerIds[i];
				Player player = ReInput.players.GetPlayer(playerId);
				if (player == null || (usePlayingPlayersOnly && !player.isPlaying))
				{
					continue;
				}
				int mouseInputSourceCount = GetMouseInputSourceCount(playerId);
				for (int j = 0; j < mouseInputSourceCount; j++)
				{
					IMouseInputSource mouseInputSource = GetMouseInputSource(playerId, j);
					if (mouseInputSource != null && mouseInputSource.screenPositionDelta.sqrMagnitude > 0f)
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool GetMouseButtonDownOnAnyMouse(int buttonIndex)
		{
			for (int i = 0; i < playerIds.Length; i++)
			{
				int playerId = playerIds[i];
				Player player = ReInput.players.GetPlayer(playerId);
				if (player == null || (usePlayingPlayersOnly && !player.isPlaying))
				{
					continue;
				}
				int mouseInputSourceCount = GetMouseInputSourceCount(playerId);
				for (int j = 0; j < mouseInputSourceCount; j++)
				{
					IMouseInputSource mouseInputSource = GetMouseInputSource(playerId, j);
					if (mouseInputSource != null && mouseInputSource.GetButtonDown(buttonIndex))
					{
						return true;
					}
				}
			}
			return false;
		}

		private void OnRewiredInitialized()
		{
			InitializeRewired();
		}

		private void OnRewiredShutDown()
		{
			ClearRewiredVars();
		}
	}
}
