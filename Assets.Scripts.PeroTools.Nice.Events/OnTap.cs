using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[DisallowMultipleComponent]
	public class OnTap : Event
	{
		private float m_HardTime = 0.1f;

		private float m_TapTime;

		protected override void OnEnter()
		{
			SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("Tap", delegate
			{
				if (m_TapTime < m_HardTime)
				{
					m_TapTime += Time.deltaTime;
				}
				else if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
				{
					Do();
				}
			});
		}

		protected override void OnExit()
		{
			SingletonMonoBehaviour<UnityGameManager>.instance.UnregLoop("Tap");
		}

		private void Do()
		{
			Execute();
			m_TapTime = 0f;
		}
	}
}
