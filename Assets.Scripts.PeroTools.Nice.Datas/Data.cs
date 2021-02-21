using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using Sirenix.Serialization;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Datas
{
	public class Data : IData, IValue
	{
		[SerializeField]
		private string m_Uid;

		[SerializeField]
		private Dictionary<string, IVariable> m_Fields = new Dictionary<string, IVariable>();

		private Dictionary<string, IVariable> m_CurFields;

		public string uid => m_Uid;

		public Dictionary<string, IVariable> fields
		{
			get
			{
				if (m_CurFields == null && !string.IsNullOrEmpty(m_Uid))
				{
					m_CurFields = new Dictionary<string, IVariable>();
					Dictionary<string, IVariable> fields = Singleton<DataManager>.instance.datas[m_Uid].fields;
					foreach (KeyValuePair<string, IVariable> item in fields)
					{
						if (m_Fields.ContainsKey(item.Key))
						{
							m_CurFields[item.Key] = m_Fields[item.Key];
						}
						else if (!m_CurFields.ContainsKey(item.Key))
						{
							IVariable variable = SerializationUtility.CreateCopy(item.Value) as IVariable;
							Formula.SetFormulaData(variable, this);
							m_CurFields.Add(item.Key, variable);
						}
					}
				}
				else if (string.IsNullOrEmpty(m_Uid))
				{
					return m_Fields;
				}
				return m_CurFields;
			}
			set
			{
				m_Fields = Enumerable.Where(value, (KeyValuePair<string, IVariable> d) => d.Value is Constance).ToDictionary((KeyValuePair<string, IVariable> d) => d.Key, (KeyValuePair<string, IVariable> d) => d.Value);
			}
		}

		public IVariable this[string uid]
		{
			get
			{
				return this.Get(uid);
			}
			set
			{
				this.Set(uid, value);
			}
		}

		public object result
		{
			get
			{
				return this;
			}
			set
			{
				Data data = (Data)value;
				Dictionary<string, IVariable> dictionary = this.fields = data.fields;
			}
		}

		public Data(string uid, IData data = null)
		{
			m_Uid = uid;
			data = (data ?? Singleton<DataManager>.instance.datas[uid]);
			fields = Enumerable.Where(data.fields, (KeyValuePair<string, IVariable> kvp) => kvp.Value is Constance).ToDictionary((KeyValuePair<string, IVariable> kvp) => kvp.Key, (KeyValuePair<string, IVariable> kvp) => (IVariable)SerializationUtility.CreateCopy(kvp.Value));
		}

		public Data()
		{
			fields = new Dictionary<string, IVariable>();
		}
	}
}
