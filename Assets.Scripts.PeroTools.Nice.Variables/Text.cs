using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Variables
{
	public class Text : IVariable, IValue
	{
		[SerializeField]
		[Variable(typeof(string), "OnNameGUI", false)]
		private IVariable m_Name;

		[SerializeField]
		[Variable(typeof(string), "OnKeyGUI", false)]
		private IVariable m_Key;

		[SerializeField]
		[Variable(typeof(int), null, false)]
		[HideIf("m_IsArrayCount", true)]
		[HideIf("m_IsPro", true)]
		private IVariable m_Index;

		[SerializeField]
		private bool m_IsPro;

		[SerializeField]
		[Variable(typeof(string), null, false)]
		[ShowIf("m_IsPro", true)]
		private IVariable m_CompareKey;

		[SerializeField]
		[Variable(typeof(string), null, false)]
		[ShowIf("m_IsPro", true)]
		private IVariable m_CompareValue;

		public string name
		{
			get
			{
				return m_Name.GetResult<string>();
			}
			set
			{
				m_Name.result = value;
			}
		}

		public string key
		{
			get
			{
				return m_Key.GetResult<string>();
			}
			set
			{
				m_Key.result = value;
			}
		}

		public int index
		{
			get
			{
				return m_Index.GetResult<int>();
			}
			set
			{
				m_Index.result = value;
			}
		}

		public object result
		{
			get
			{
				string result = m_Name.GetResult<string>();
				if (string.IsNullOrEmpty(result))
				{
					return null;
				}
				if ((string)m_Key.result == "Array Count")
				{
					return Singleton<ConfigManager>.instance[result].Count;
				}
				string result2 = m_Key.GetResult<string>();
				JToken jToken = (!m_IsPro) ? Singleton<ConfigManager>.instance.GetConfigToken(result, m_Index.GetResult<int>(), result2) : ((!(result2 != "index")) ? ((JToken)Singleton<ConfigManager>.instance.GetConfigIndex(result, m_CompareKey.GetResult<string>(), m_CompareValue.result)) : Singleton<ConfigManager>.instance.GetConfigToken(result, m_CompareKey.GetResult<string>(), result2, m_CompareValue.result));
				object obj = jToken;
				if (jToken != null)
				{
					switch (jToken.Type)
					{
					case JTokenType.Integer:
						obj = (int)jToken;
						break;
					case JTokenType.Float:
						obj = (float)jToken;
						break;
					case JTokenType.String:
						obj = (string)jToken;
						break;
					case JTokenType.Boolean:
						obj = (bool)jToken;
						break;
					}
				}
				if (obj != null)
				{
					float result3 = 0f;
					if (float.TryParse(obj.ToString(), out result3))
					{
						return result3;
					}
				}
				return obj;
			}
			set
			{
			}
		}
	}
}
