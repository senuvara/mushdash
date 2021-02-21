using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Components
{
	public class SerializedSimplySelectable : SerializedUIBehaviour
	{
		[PropertyOrder(100)]
		[LabelText("Interactable")]
		public bool interactable = true;

		private bool m_GroupsAllowInteraction = true;

		private readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();

		protected override void OnCanvasGroupChanged()
		{
			bool flag = true;
			Transform transform = base.transform;
			while (transform != null)
			{
				transform.GetComponents(m_CanvasGroupCache);
				bool flag2 = false;
				for (int i = 0; i < m_CanvasGroupCache.Count; i++)
				{
					if (!m_CanvasGroupCache[i].interactable)
					{
						flag = false;
						flag2 = true;
					}
					if (m_CanvasGroupCache[i].ignoreParentGroups)
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					break;
				}
				transform = transform.parent;
			}
			if (flag != m_GroupsAllowInteraction)
			{
				m_GroupsAllowInteraction = flag;
			}
		}

		public virtual bool IsInteractable()
		{
			return m_GroupsAllowInteraction && interactable;
		}
	}
}
