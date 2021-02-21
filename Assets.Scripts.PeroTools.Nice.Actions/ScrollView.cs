using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.UI;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class ScrollView : Action
	{
		[SerializeField]
		[Variable(typeof(Transform), null, false)]
		private IVariable m_ScrollSpace;

		[SerializeField]
		[EnumToggleButtons]
		private ContentType m_ContentType;

		[SerializeField]
		[HideIf("IsSceneObjects", true)]
		private GameObject m_Prefab;

		[SerializeField]
		[ShowIf("IsSceneObjects", true)]
		private List<GameObject> m_SceneObjects = new List<GameObject>();

		[SerializeField]
		[EnumToggleButtons]
		private ScrollDirection m_Direction;

		[SerializeField]
		private Scrollbar m_Scrollbar;

		[SerializeField]
		private float m_ScrollSensitivity;

		[SerializeField]
		private Vector4 m_Padding;

		[SerializeField]
		private float m_Spacing;

		[SerializeField]
		[EnumToggleButtons]
		private TextAnchor m_ChildAlignment;

		[SerializeField]
		private bool m_ChildControlWidth;

		[SerializeField]
		private bool m_ChildControlHeight;

		[SerializeField]
		private bool m_ChildForceExpandWidth;

		[SerializeField]
		private bool m_ChildForceExpandHeight;

		[SerializeField]
		private bool m_IsAlign;

		[SerializeField]
		[ShowIf("m_IsAlign", true)]
		private float m_MidPosition;

		[SerializeField]
		[ShowIf("m_IsAlign", true)]
		private Vector2 m_ScaleRate;

		[SerializeField]
		[ShowIf("m_IsAlign", true)]
		private float m_ScaleRange;

		[SerializeField]
		private Button m_BtnSwitchLeftUp;

		[SerializeField]
		private Button m_BtnSwitchRightDown;

		[SerializeField]
		private AudioClip m_AudioClip;

		[SerializeField]
		[Variable(typeof(int), null, false)]
		private IVariable m_Count;

		private const float speedThreshold = 200f;

		private const float moveBackTime = 0.4f;

		private const Ease stopCurve = Ease.InExpo;

		private const float moveSpeed = 1200f;

		private const Ease moveCurve = Ease.Linear;

		private const float elasticity = 0.1f;

		private const float decelerationRate = 0.135f;

		[HideInInspector]
		public int index = -1;

		private List<Transform> m_Children;

		private bool m_IsDragging;

		private Tweener m_ScrollTwner;

		private Tweener m_PageTwner;

		private Tweener m_ToTwner;

		private GameObject m_Layout;

		public ScrollRect scrollRect
		{
			get;
			private set;
		}

		public event Action<int> onIndexChanged;

		public event Action<Vector2> onValueChanged;

		public event Action<int, float> onScrollTo;

		public override void Execute()
		{
			m_Children = (m_Children ?? new List<Transform>());
			if ((bool)scrollRect)
			{
				EventTrigger component = scrollRect.gameObject.GetComponent<EventTrigger>();
				index = -1;
				m_IsDragging = false;
				m_Children.For(delegate(Transform c)
				{
					UnityEngine.Object.Destroy(c.gameObject);
				});
				m_Children.Clear();
				m_ScrollTwner.Kill();
				m_PageTwner.Kill();
				m_ToTwner.Kill();
				m_ScrollTwner = null;
				m_PageTwner = null;
				m_ToTwner = null;
				UnityEngine.Object.Destroy(m_Layout);
				UnityEngine.Object.Destroy(component);
				UnityEngine.Object.Destroy(scrollRect);
			}
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				if (m_ContentType != ContentType.SceneObjects || m_SceneObjects.Count > 0)
				{
					Rect rect = (m_ContentType != 0) ? m_SceneObjects[0].GetComponent<RectTransform>().rect : m_Prefab.GetComponent<RectTransform>().rect;
					int num = (m_ContentType != 0) ? m_SceneObjects.Count : m_Count.GetResult<int>();
					float width = rect.width;
					float height = rect.height;
					GameObject gameObject = new GameObject("Content");
					Transform @object = GameUtils.GetObject<Transform>(m_ScrollSpace.result);
					GameObject gameObject2 = @object.transform.parent.gameObject;
					m_Layout = gameObject;
					gameObject.transform.SetParent(@object, false);
					Image image = gameObject.AddComponent<Image>();
					image.color = Vector4.zero;
					List<GameObject> submitGameObjects = (from t in gameObject2.GetComponentsInChildren<Transform>()
						select t.gameObject).ToList();
					submitGameObjects.Remove(@object.gameObject);
					UIEventUtils.OnEvent(@object.gameObject, EventTriggerType.PointerClick, delegate(BaseEventData eventData)
					{
						PointerEventData data = eventData as PointerEventData;
						UIEventUtils.PassEvent(data, ExecuteEvents.pointerClickHandler, submitGameObjects);
					});
					float singleSize = (m_Direction != ScrollDirection.Vertical) ? width : height;
					RectTransform layoutRt = gameObject.GetOrAddComponent<RectTransform>();
					if (m_Direction == ScrollDirection.Horizontal)
					{
						layoutRt.sizeDelta = new Vector2(singleSize * (float)num + m_Padding.x + m_Padding.y + m_Spacing * (float)(num - 1), height);
						RectTransform rectTransform = layoutRt;
						Vector2 sizeDelta = layoutRt.sizeDelta;
						rectTransform.localPosition = new Vector3(sizeDelta.x / 2f, 0f, 0f);
					}
					else
					{
						layoutRt.sizeDelta = new Vector2(width, singleSize * (float)num + m_Padding.z + m_Padding.w + m_Spacing * (float)(num - 1));
						RectTransform rectTransform2 = layoutRt;
						Vector2 sizeDelta2 = layoutRt.sizeDelta;
						rectTransform2.localPosition = new Vector3(0f, sizeDelta2.y / 2f, 0f);
					}
					if ((bool)m_Prefab)
					{
						Toggle componentInChildren = m_Prefab.GetComponentInChildren<Toggle>();
						if (componentInChildren != null)
						{
							ToggleGroup toggleGroup = componentInChildren.group = gameObject.GetOrAddComponent<ToggleGroup>();
						}
					}
					if (m_Direction == ScrollDirection.Vertical)
					{
						VerticalLayoutGroup verticalLayoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
						verticalLayoutGroup.padding = new RectOffset((int)m_Padding.x, (int)m_Padding.y, (int)m_Padding.z, (int)m_Padding.w);
						verticalLayoutGroup.spacing = m_Spacing;
						verticalLayoutGroup.childControlWidth = m_ChildControlWidth;
						verticalLayoutGroup.childControlHeight = m_ChildControlHeight;
						verticalLayoutGroup.childForceExpandWidth = m_ChildForceExpandWidth;
						verticalLayoutGroup.childForceExpandHeight = m_ChildForceExpandHeight;
						verticalLayoutGroup.childAlignment = m_ChildAlignment;
					}
					else
					{
						HorizontalLayoutGroup horizontalLayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
						horizontalLayoutGroup.padding = new RectOffset((int)m_Padding.x, (int)m_Padding.y, (int)m_Padding.z, (int)m_Padding.w);
						horizontalLayoutGroup.spacing = m_Spacing;
						horizontalLayoutGroup.childControlWidth = m_ChildControlWidth;
						horizontalLayoutGroup.childControlHeight = m_ChildControlHeight;
						horizontalLayoutGroup.childForceExpandWidth = m_ChildForceExpandWidth;
						horizontalLayoutGroup.childForceExpandHeight = m_ChildForceExpandHeight;
						horizontalLayoutGroup.childAlignment = m_ChildAlignment;
					}
					for (int j = 0; j < num; j++)
					{
						Transform transform = (m_ContentType != 0) ? m_SceneObjects[j].transform : UnityEngine.Object.Instantiate(m_Prefab, gameObject.transform).transform;
						transform.SetParent(gameObject.transform);
						transform.localScale = Vector3.one;
						transform.localPosition = Vector3.zero;
						m_Children.Add(transform);
					}
					scrollRect = @object.gameObject.AddComponent<ScrollRect>();
					scrollRect.transform.SetAsLastSibling();
					scrollRect.content = layoutRt;
					scrollRect.viewport = scrollRect.gameObject.GetComponent<RectTransform>();
					scrollRect.vertical = (m_Direction == ScrollDirection.Vertical);
					scrollRect.horizontal = (m_Direction == ScrollDirection.Horizontal);
					scrollRect.elasticity = 0.1f;
					scrollRect.decelerationRate = 0.135f;
					scrollRect.scrollSensitivity = m_ScrollSensitivity;
					if (scrollRect.vertical)
					{
						scrollRect.verticalScrollbar = m_Scrollbar;
					}
					else
					{
						scrollRect.horizontalScrollbar = m_Scrollbar;
					}
					if (m_Spacing == 0f && m_Padding.x == 0f && m_Padding.y == 0f && m_Padding.z == 0f && m_Padding.w == 0f)
					{
						UnityEngine.UI.ContentSizeFitter contentSizeFitter = gameObject.AddComponent<UnityEngine.UI.ContentSizeFitter>();
						contentSizeFitter.horizontalFit = ((m_Direction == ScrollDirection.Horizontal) ? UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize : UnityEngine.UI.ContentSizeFitter.FitMode.Unconstrained);
						contentSizeFitter.verticalFit = ((m_Direction == ScrollDirection.Vertical) ? UnityEngine.UI.ContentSizeFitter.FitMode.PreferredSize : UnityEngine.UI.ContentSizeFitter.FitMode.Unconstrained);
						RectTransform component2 = @object.GetComponent<RectTransform>();
						Assets.Scripts.PeroTools.UI.ContentSizeFitter contentSizeFitter2 = gameObject.AddComponent<Assets.Scripts.PeroTools.UI.ContentSizeFitter>();
						contentSizeFitter2.minSize = ((m_Direction != 0) ? ((int)component2.rect.height) : ((int)component2.rect.width));
						contentSizeFitter2.childSize = ((m_Direction != 0) ? ((int)height) : ((int)width));
						contentSizeFitter2.gap = (int)m_Spacing;
					}
					singleSize += m_Spacing;
					float extra = (num % 2 == 1) ? 0f : (singleSize / 2f);
					scrollRect.onValueChanged.AddListener(delegate(Vector2 offset)
					{
						if (this.onValueChanged != null)
						{
							this.onValueChanged(offset);
						}
						if (m_IsAlign)
						{
							float num9;
							if (scrollRect.vertical)
							{
								Vector3 localPosition5 = scrollRect.content.transform.localPosition;
								num9 = localPosition5.y;
							}
							else
							{
								Vector3 localPosition6 = scrollRect.content.transform.localPosition;
								num9 = localPosition6.x;
							}
							float num10 = num9;
							float num11 = Vector3.Magnitude(scrollRect.velocity);
							int num12 = index;
							float num13;
							if (scrollRect.vertical)
							{
								Vector2 sizeDelta5 = layoutRt.sizeDelta;
								num13 = sizeDelta5.y / 2f;
							}
							else
							{
								Vector2 sizeDelta6 = layoutRt.sizeDelta;
								num13 = sizeDelta6.x / 2f;
							}
							float num14 = num13;
							index = Mathf.RoundToInt((num14 - num10 - singleSize / 2f) / singleSize) - 1;
							if (this.onIndexChanged != null && num12 != index && index >= 0)
							{
								this.onIndexChanged(index);
							}
							if (num11 > 0f && num11 < 200f && !m_IsDragging)
							{
								float num15 = Mathf.Abs(num10);
								float f2;
								if (scrollRect.vertical)
								{
									Vector2 sizeDelta7 = layoutRt.sizeDelta;
									f2 = sizeDelta7.y - m_Padding.z - m_Padding.w;
								}
								else
								{
									Vector2 sizeDelta8 = layoutRt.sizeDelta;
									f2 = sizeDelta8.x - m_Padding.x - m_Padding.y;
								}
								if (num15 <= Mathf.Abs(f2) / 2f)
								{
									scrollRect.StopMovement();
									float endValue = (float)Mathf.RoundToInt((num10 + extra) / singleSize) * singleSize - extra + m_MidPosition;
									if (m_ScrollTwner == null)
									{
										TweenCallback action = delegate
										{
											if (m_IsDragging)
											{
												m_ScrollTwner.Kill();
												m_ScrollTwner = null;
											}
											else
											{
												scrollRect.StopMovement();
											}
										};
										m_ScrollTwner = ((!scrollRect.vertical) ? scrollRect.content.transform.DOLocalMoveX(endValue, 0.4f).SetEase(Ease.InExpo).OnUpdate(action) : scrollRect.content.transform.DOLocalMoveY(endValue, 0.4f).SetEase(Ease.InExpo).OnUpdate(action));
									}
								}
							}
							m_Children.ToList().For(delegate(Transform item)
							{
								Transform transform4 = item.transform;
								float x2 = m_ScaleRate.x;
								float y2 = m_ScaleRate.y;
								float num16;
								if (scrollRect.vertical)
								{
									Vector3 position5 = item.transform.position;
									num16 = position5.y;
								}
								else
								{
									Vector3 position6 = item.transform.position;
									num16 = position6.x;
								}
								transform4.localScale = Mathf.Lerp(x2, y2, 1f - Mathf.Abs(num16 / Singleton<UIManager>.instance.scale - m_MidPosition) / m_ScaleRange) * Vector3.one;
							});
						}
					});
					UIEventUtils.OnEvent(@object.gameObject, EventTriggerType.BeginDrag, delegate
					{
						if (m_IsAlign)
						{
							m_IsDragging = true;
							if (m_ScrollTwner != null)
							{
								m_ScrollTwner.Kill();
								m_ScrollTwner = null;
							}
						}
					});
					UIEventUtils.OnEvent(@object.gameObject, EventTriggerType.EndDrag, delegate
					{
						if (m_IsAlign)
						{
							m_IsDragging = false;
							if (m_ScrollTwner != null)
							{
								m_ScrollTwner.Kill();
								m_ScrollTwner = null;
							}
						}
					});
					Action<bool> page = delegate(bool isRightBottom)
					{
						Singleton<AudioManager>.instance.PlayOneShot(m_AudioClip, Singleton<DataManager>.instance["GameConfig"]["SfxVolume"].GetResult<float>());
						float num5;
						if (scrollRect.vertical)
						{
							Vector3 localPosition = scrollRect.content.transform.localPosition;
							num5 = localPosition.y;
						}
						else
						{
							Vector3 localPosition2 = scrollRect.content.transform.localPosition;
							num5 = localPosition2.x;
						}
						float num6 = num5;
						float num7 = (!isRightBottom) ? ((float)(Mathf.RoundToInt((num6 + extra) / singleSize) + 1) * singleSize - extra) : ((float)(Mathf.RoundToInt((num6 + extra) / singleSize) - 1) * singleSize - extra);
						float num8;
						if (scrollRect.vertical)
						{
							Vector3 localPosition3 = scrollRect.content.transform.localPosition;
							num8 = localPosition3.y;
						}
						else
						{
							Vector3 localPosition4 = scrollRect.content.transform.localPosition;
							num8 = localPosition4.x;
						}
						float duration = Mathf.Abs(num7 - num8) / 1200f;
						if (m_PageTwner != null)
						{
							m_PageTwner.Kill();
						}
						m_PageTwner = ((!scrollRect.vertical) ? scrollRect.content.transform.DOLocalMoveX(num7, duration).SetEase(Ease.Linear) : scrollRect.content.transform.DOLocalMoveY(num7, duration).SetEase(Ease.Linear));
					};
					Action<int, float> value = delegate(int i, float f)
					{
						if (m_ToTwner != null)
						{
							m_ToTwner.Kill();
						}
						float num2;
						if (scrollRect.vertical)
						{
							Vector2 sizeDelta3 = layoutRt.sizeDelta;
							num2 = sizeDelta3.y;
						}
						else
						{
							Vector2 sizeDelta4 = layoutRt.sizeDelta;
							num2 = sizeDelta4.x;
						}
						float num3 = num2;
						float num4 = num3 / 2f - extra - singleSize - (float)i * singleSize;
						if (Math.Abs(f) <= 0f)
						{
							if (scrollRect.vertical)
							{
								Transform transform2 = scrollRect.content.transform;
								Vector3 position = scrollRect.content.transform.position;
								float x = position.x;
								Vector3 position2 = scrollRect.content.transform.position;
								transform2.localPosition = new Vector3(x, num4, position2.z);
							}
							else
							{
								Transform transform3 = scrollRect.content.transform;
								Vector3 position3 = scrollRect.content.transform.position;
								float y = position3.y;
								Vector3 position4 = scrollRect.content.transform.position;
								transform3.localPosition = new Vector3(num4, y, position4.z);
							}
						}
						else
						{
							m_ToTwner = ((!scrollRect.vertical) ? scrollRect.content.transform.DOLocalMoveX(num4, f).SetEase(Ease.Linear) : scrollRect.content.transform.DOLocalMoveY(num4, f).SetEase(Ease.Linear));
						}
						DOTweenUtils.Delay(delegate
						{
							scrollRect.onValueChanged.Invoke(scrollRect.normalizedPosition);
						}, Time.deltaTime * 2f);
					};
					onScrollTo += value;
					if ((bool)m_BtnSwitchLeftUp)
					{
						UIEventUtils.OnEvent(m_BtnSwitchLeftUp.gameObject, EventTriggerType.PointerClick, delegate
						{
							page(false);
						});
					}
					if ((bool)m_BtnSwitchRightDown)
					{
						UIEventUtils.OnEvent(m_BtnSwitchRightDown.gameObject, EventTriggerType.PointerClick, delegate
						{
							page(true);
						});
					}
				}
			}, () => !scrollRect);
		}

		public Tweener ScrollToIdx(int i, float time = 0f)
		{
			if (this.onScrollTo != null)
			{
				this.onScrollTo(i, time);
			}
			return m_ToTwner;
		}
	}
}
