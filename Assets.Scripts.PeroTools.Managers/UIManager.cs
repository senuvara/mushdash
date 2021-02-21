using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Managers
{
	public class UIManager : Singleton<UIManager>
	{
		public class UIPanle
		{
		}

		private EventSystem m_Es;

		private GameObject m_PreSelected;

		private List<GameObject> m_PnlGameObjects;

		private RectTransform m_RectTransform;

		private readonly Stack<GameObject> m_InactiveStack = new Stack<GameObject>();

		public Camera camera
		{
			get;
			private set;
		}

		public RectTransform rectTransform
		{
			get
			{
				if (!m_RectTransform)
				{
					CanvasScaler canvasScaler = Object.FindObjectsOfType<CanvasScaler>().Find((CanvasScaler c) => c.GetComponent<Canvas>().worldCamera == Camera.main);
					if ((bool)canvasScaler)
					{
						m_RectTransform = canvasScaler.gameObject.GetComponent<RectTransform>();
					}
				}
				return m_RectTransform;
			}
		}

		public GameObject top
		{
			get
			{
				if (m_PnlGameObjects.Count == 0)
				{
					return null;
				}
				List<GameObject> list = new List<GameObject>(m_PnlGameObjects);
				list.Sort(delegate(GameObject l, GameObject r)
				{
					if (l.activeInHierarchy && !r.activeInHierarchy)
					{
						return -1;
					}
					if (!l.activeInHierarchy && r.activeInHierarchy)
					{
						return 1;
					}
					if (!l.activeInHierarchy && !r.activeInHierarchy)
					{
						return 0;
					}
					if (l.transform.parent != r.transform.parent)
					{
						if (l.transform.parent == r.transform)
						{
							return 1;
						}
						if (r.transform.parent == r.transform)
						{
							return -1;
						}
						return r.transform.parent.GetSiblingIndex() - l.transform.parent.GetSiblingIndex();
					}
					return r.transform.GetSiblingIndex() - l.transform.GetSiblingIndex();
				});
				return list[0];
			}
		}

		public float scale
		{
			get
			{
				Vector3 lossyScale = rectTransform.lossyScale;
				return lossyScale.x;
			}
		}

		public GameObject peek
		{
			get
			{
				if (m_InactiveStack.Count > 0)
				{
					return m_InactiveStack.Peek();
				}
				return null;
			}
		}

		public string[] pnlNames
		{
			get
			{
				m_PnlGameObjects = GameUtils.FindGameObjectsWithTag("Panel").ToList();
				return m_PnlGameObjects.Select((GameObject p) => (!p) ? string.Empty : p.name).ToArray();
			}
		}

		public GameObject this[string n] => m_PnlGameObjects.Find((GameObject p) => (bool)p && p.name == n);

		public UIManager()
		{
			SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("AutoResetSelectedObj", AutoResetSelectedObj);
			UnityEngine.SceneManagement.SceneManager.activeSceneChanged += delegate
			{
				InitEachScene();
			};
		}

		public void Push(Object pnlGO)
		{
			GameObject t = pnlGO as GameObject;
			m_InactiveStack.Push(t);
		}

		public void Pop(Object pnlGO)
		{
			GameObject go = pnlGO as GameObject;
			if (m_InactiveStack.Count != 0)
			{
				m_InactiveStack.Remove((GameObject g) => g == go);
			}
		}

		public void Init()
		{
		}

		private void InitEachScene()
		{
			m_Es = EventSystem.current;
			if ((bool)m_Es)
			{
				m_PreSelected = m_Es.firstSelectedGameObject;
			}
			m_PnlGameObjects = GameUtils.FindGameObjectsWithTag("Panel").ToList();
			if (Application.isPlaying)
			{
				foreach (GameObject pnlGameObject in m_PnlGameObjects)
				{
					GameObject go = pnlGameObject;
					EventInvoker eventInvoker = Assets.Scripts.PeroTools.Nice.Events.Event.OnEvent(pnlGameObject, typeof(OnDeactivate));
					eventInvoker.AddListener(delegate
					{
						Pop(go);
					});
				}
			}
			m_RectTransform = null;
		}

		private void AutoResetSelectedObj(float arg0)
		{
			if ((bool)m_Es && m_Es.currentSelectedGameObject != m_PreSelected)
			{
				if (m_Es.currentSelectedGameObject == null)
				{
					m_Es.SetSelectedGameObject(m_PreSelected);
				}
				else
				{
					m_PreSelected = m_Es.currentSelectedGameObject;
				}
			}
		}
	}
}
