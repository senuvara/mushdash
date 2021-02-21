using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
	[HideMonoScript]
	[AddComponentMenu("Nice/Test/FPSCounter")]
	public class FPSCounter : MonoBehaviour
	{
		public float updateInterval = 0.5f;

		private Text m_Text;

		private TextMeshProUGUI m_TextMeshProUGUI;

		private int m_PassFrame;

		private float m_PassTime;

		private IVariable m_ShowFpsVariable;

		private bool m_ShouldShow;

		private void Awake()
		{
			Object.Destroy(base.gameObject);
		}

		private void OnDestroy()
		{
			if (m_ShouldShow)
			{
			}
			SingletonMonoBehaviour<UnityGameManager>.instance.UnregLoop("FPSCounter");
		}
	}
}
