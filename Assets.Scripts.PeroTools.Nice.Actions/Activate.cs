using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using System;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class Activate : Action
	{
		[SerializeField]
		[Variable(new Type[]
		{
			typeof(GameObject),
			typeof(Component)
		})]
		private IVariable m_Object;

		public override void Execute()
		{
			object result = m_Object.result;
			GameObject gameObject = result as GameObject;
			if ((bool)gameObject)
			{
				if (!gameObject.activeSelf)
				{
					gameObject.SetActive(true);
				}
			}
			else
			{
				result?.GetType().GetProperty("enabled")?.SetValue(result, true, null);
			}
		}
	}
}
