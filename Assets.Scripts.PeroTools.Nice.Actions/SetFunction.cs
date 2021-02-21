using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class SetFunction : Action
	{
		[SerializeField]
		private MethodType m_Type;

		[SerializeField]
		private Script m_Script = new Script();

		[SerializeField]
		private string m_EnumKey;

		[SerializeField]
		private List<IVariable> m_Params = new List<IVariable>();

		public override void Execute()
		{
			switch (m_Type)
			{
			case MethodType.Enum:
			{
				List<object> list = m_Params.Select((IVariable p) => p.result);
				if (Singleton<EventManager>.instance.events.ContainsKey(m_EnumKey))
				{
					object obj = Singleton<EventManager>.instance.events[m_EnumKey];
					(obj as EventManager.EventCallFunc)?.Invoke(list.ToArray());
				}
				else
				{
					Singleton<EventManager>.instance.Invoke(m_EnumKey, list.ToArray());
				}
				break;
			}
			case MethodType.Script:
				m_Script.Invoke();
				break;
			}
		}
	}
}
