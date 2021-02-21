using Assets.Scripts.PeroTools.Nice.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.UI
{
	[RequireComponent(typeof(UnityEngine.UI.ContentSizeFitter))]
	public class ContentSizeFitter : MonoBehaviour
	{
		public int minSize;

		public int childSize;

		public int gap;

		private RectTransform m_RectTransform;

		private UnityEngine.UI.ContentSizeFitter m_ContentSizeFitter;

		private void Awake()
		{
			m_RectTransform = GetComponent<RectTransform>();
			m_ContentSizeFitter = GetComponent<UnityEngine.UI.ContentSizeFitter>();
			m_ContentSizeFitter.enabled = false;
			Assets.Scripts.PeroTools.Nice.Events.Event.OnEvent(base.gameObject, typeof(OnActivate)).AddListener(delegate
			{
				int num = 0;
				for (int i = 0; i < base.transform.childCount; i++)
				{
					Transform child = base.transform.GetChild(i);
					if (child.gameObject.activeSelf)
					{
						num++;
					}
				}
				int num2 = (childSize + gap) * num - gap;
				m_ContentSizeFitter.enabled = (num2 > minSize);
				if (num2 <= minSize)
				{
					m_RectTransform.sizeDelta = new Vector2(m_RectTransform.rect.width, minSize);
				}
				else
				{
					RectTransform rectTransform = m_RectTransform;
					Vector3 position = m_RectTransform.transform.position;
					float x = position.x;
					float y = (float)(minSize - num2) / 2f;
					Vector3 position2 = m_RectTransform.transform.position;
					rectTransform.position = new Vector3(x, y, position2.z);
				}
			});
		}

		private void Update()
		{
			m_ContentSizeFitter.enabled = false;
			m_ContentSizeFitter.enabled = true;
		}
	}
}
