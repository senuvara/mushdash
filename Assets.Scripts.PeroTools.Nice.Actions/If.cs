using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class If : Group
	{
		[SerializeField]
		[Variable(true, null, false)]
		[PropertyOrder(-1)]
		private IVariable m_IsExecute;

		public override void Execute()
		{
			if (m_IsExecute.GetResult<bool>())
			{
				base.Execute();
			}
		}
	}
}
