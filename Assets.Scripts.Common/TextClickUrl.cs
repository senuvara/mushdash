using System;
using UnityEngine;

namespace Assets.Scripts.Common
{
	[RequireComponent(typeof(YlyRichText))]
	public class TextClickUrl : MonoBehaviour
	{
		private YlyRichText m_Text;

		private void Awake()
		{
			m_Text = GetComponent<YlyRichText>();
			YlyRichText text = m_Text;
			text.onLinkClick = (YlyDelegateUtil.StringDelegate)Delegate.Combine(text.onLinkClick, new YlyDelegateUtil.StringDelegate(OnClickUrl));
		}

		public void OnClickUrl(string url)
		{
			url = url.Substring(1, url.Length - 2);
			Debug.Log("Click " + url);
			string arg = "taptap";
			Application.OpenURL($"{url}?channel={arg}");
		}
	}
}
