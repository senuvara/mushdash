using Assets.Scripts.PeroTools.Nice.Events;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using UnityEngine;

namespace Assets.Scripts.UI.Specials
{
	public class PnlTroveItem : MonoBehaviour
	{
		public OnActivate[] activates = new OnActivate[1];

		public GameObject imgItem;

		public GameObject txtValue;

		public GameObject imgUse;

		public VariableBehaviour type;

		public VariableBehaviour count;

		private bool m_IsDisable;

		public bool isEmpty()
		{
			return count.variable.GetResult<int>() == 0;
		}

		private void OnDisable()
		{
			if (!m_IsDisable)
			{
				activates[0].enabled = false;
				activates[1].enabled = false;
				m_IsDisable = true;
			}
		}

		public void SetActivate(bool enable)
		{
			if (count.variable.GetResult<int>() > 0)
			{
				activates[0].enabled = enable;
				activates[1].enabled = enable;
				imgItem.SetActive(true);
				txtValue.SetActive(true);
				string result = type.variable.GetResult<string>();
				if (result != "loading" && result != "welcome")
				{
					imgUse.SetActive(false);
				}
			}
			GetComponent<CanvasGroup>().alpha = 1f;
		}
	}
}
