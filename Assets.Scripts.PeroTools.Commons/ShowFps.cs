using UnityEngine;

namespace Assets.Scripts.PeroTools.Commons
{
	public class ShowFps : SingletonMonoBehaviour<ShowFps>
	{
		public float updateInterval = 0.5f;

		private double m_LastInterval;

		private int m_Frames;

		private float m_CurrFps;

		private GUIStyle m_Style;

		private void Start()
		{
			m_LastInterval = Time.realtimeSinceStartup;
			m_Frames = 0;
			m_Style = new GUIStyle
			{
				fontSize = 50,
				normal = 
				{
					textColor = Color.white
				}
			};
		}

		private void Update()
		{
			m_Frames++;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			if ((double)realtimeSinceStartup > m_LastInterval + (double)updateInterval)
			{
				m_CurrFps = (float)((double)m_Frames / ((double)realtimeSinceStartup - m_LastInterval));
				m_Frames = 0;
				m_LastInterval = realtimeSinceStartup;
			}
		}

		private void OnGUI()
		{
			GUI.Label(new Rect(Screen.width - m_Style.fontSize * 6, m_Style.fontSize, m_Style.fontSize, m_Style.fontSize), "FPS:" + m_CurrFps.ToString("f2"), m_Style);
		}
	}
}
