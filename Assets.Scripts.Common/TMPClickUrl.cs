using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Common
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TMPClickUrl : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		private TextMeshProUGUI m_TextMeshPro;

		private void Awake()
		{
			m_TextMeshPro = GetComponent<TextMeshProUGUI>();
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			int num = TMP_TextUtilities.FindIntersectingLink(m_TextMeshPro, eventData.position, Camera.main);
			if (num != -1)
			{
				TMP_LinkInfo tMP_LinkInfo = m_TextMeshPro.textInfo.linkInfo[num];
				string arg = "taptap";
				Application.OpenURL($"{tMP_LinkInfo.GetLinkID()}?channel={arg}");
			}
		}
	}
}
