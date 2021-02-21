using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Interface;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Variables
{
	public class Enum : IVariable, IValue
	{
		[SerializeField]
		private string m_Key;

		[SerializeField]
		private List<IVariable> m_Params = new List<IVariable>();

		public object result
		{
			get
			{
				object result = null;
				object obj = Singleton<EntityManager>.instance.entities[m_Key];
				if (m_Params == null)
				{
					Func<object> func = obj as Func<object>;
					if (func != null)
					{
						result = func();
					}
				}
				else
				{
					Func<object[], object> func2 = obj as Func<object[], object>;
					object[] arg = m_Params.Select((IVariable p) => p.result).ToArray();
					if (func2 != null)
					{
						result = func2(arg);
					}
				}
				return result;
			}
			set
			{
			}
		}
	}
}
