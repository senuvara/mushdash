using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Variables
{
	public class Script : IVariable, IValue
	{
		[SerializeField]
		private GameObject m_GameObject;

		[SerializeField]
		[HideReferenceObjectPicker]
		private List<Reflecter> m_Reflecters;

		public object result
		{
			get
			{
				return GetValue();
			}
			set
			{
				SetValue(value);
			}
		}

		public void Invoke()
		{
			GetValue();
		}

		public object GetValue()
		{
			object obj = m_GameObject;
			for (int i = 0; i < m_Reflecters.Count; i++)
			{
				Reflecter reflecter = m_Reflecters[i];
				obj = reflecter.GetValue(obj);
			}
			return obj;
		}

		public void SetValue(object value)
		{
		}
	}
}
