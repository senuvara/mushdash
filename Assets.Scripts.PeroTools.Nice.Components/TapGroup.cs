using Assets.Scripts.PeroTools.Nice.Actions;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Components
{
	public class TapGroup : Action
	{
		[Required]
		public Transform tapGroupRoot;

		[Variable(typeof(int), null, false)]
		public IVariable activeIndex;

		private Transform m_CurrentActive;

		public override void Execute()
		{
			int num = (int)activeIndex.result;
			if (num >= 0 && num < tapGroupRoot.childCount)
			{
				if (m_CurrentActive != null)
				{
					m_CurrentActive.gameObject.SetActive(false);
				}
				m_CurrentActive = tapGroupRoot.GetChild(num);
				m_CurrentActive.gameObject.SetActive(true);
			}
		}

		public override void Enter()
		{
			for (int i = 0; i < tapGroupRoot.childCount; i++)
			{
				tapGroupRoot.GetChild(i).gameObject.SetActive(false);
			}
		}
	}
}
