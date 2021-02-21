using System;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class PlayPopuper
	{
		private readonly System.Action m_EndCallback;

		public Popup popup
		{
			get;
			private set;
		}

		public PlayPopuper(Popup p, System.Action callback = null)
		{
			popup = p;
			m_EndCallback = callback;
		}

		public void Exit(System.Action callback = null)
		{
			popup.Leave(delegate
			{
				if (callback != null)
				{
					callback();
				}
				if (m_EndCallback != null)
				{
					m_EndCallback();
				}
			});
		}
	}
}
