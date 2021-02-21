using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Controls
{
	public class ShowText : MonoBehaviour
	{
		public Text text;

		private static GameObject m_GameObject;

		private static Text m_Text;

		private static Coroutine m_Coroutine;

		private void Awake()
		{
			m_GameObject = base.gameObject;
			m_Text = text;
		}

		public void Show(string info)
		{
			base.gameObject.SetActive(true);
			if (m_Coroutine != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_Coroutine);
			}
			m_Coroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				text.text = info;
			}, 1);
		}

		public static void ShowInfo(string info)
		{
			Singleton<EventManager>.instance.Invoke("UI/OnShowText");
			if ((bool)m_GameObject)
			{
				m_GameObject.SetActive(true);
			}
			if (m_Coroutine != null)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StopCoroutine(m_Coroutine);
			}
			m_Coroutine = SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				if ((bool)m_Text)
				{
					m_Text.text = info;
				}
			}, 1);
		}
	}
}
