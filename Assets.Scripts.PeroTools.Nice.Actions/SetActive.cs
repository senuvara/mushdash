using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class SetActive : Action
	{
		[SerializeField]
		[Variable(typeof(Object), null, false)]
		[Required]
		private IVariable m_Object;

		[SerializeField]
		[Variable(typeof(bool), null, false)]
		private IVariable m_IsActive;

		public override void Execute()
		{
			object result = m_Object.result;
			bool result2 = m_IsActive.GetResult<bool>();
			GameObject gameObject = result as GameObject;
			if ((bool)gameObject)
			{
				if (gameObject.activeSelf != result2)
				{
					gameObject.SetActive(result2);
				}
			}
			else
			{
				result.GetType().GetProperty("enabled")?.SetValue(result, result2, null);
			}
		}
	}
}
