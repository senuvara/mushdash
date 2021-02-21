using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.Utilities;
using System;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Variables
{
	public class Reflecter
	{
		[SerializeField]
		private string m_Path;

		[SerializeField]
		private IVariable[] m_Params;

		[SerializeField]
		private Type m_Type;

		public object GetValue(object source)
		{
			object source2 = GetSource(source);
			if (source2 == null)
			{
				return null;
			}
			string name = m_Path.Split(' ')[1].Split('(')[0];
			MemberInfo[] member = m_Type.GetMember(name);
			if (m_Params != null)
			{
				Type[] array = new Type[m_Params.Length];
				for (int i = 0; i < m_Params.Length; i++)
				{
					IVariable variable = m_Params[i];
					if (variable != null)
					{
						object result = variable.result;
						if (result != null)
						{
							array[i] = result.GetType();
						}
					}
				}
				MethodBase methodBase = null;
				PropertyInfo propertyInfo = null;
				foreach (MemberInfo memberInfo in member)
				{
					MethodBase methodBase2 = memberInfo as MethodBase;
					PropertyInfo propertyInfo2 = memberInfo as PropertyInfo;
					if (methodBase2 != null)
					{
						ParameterInfo[] parameters = methodBase2.GetParameters();
						if (parameters.Length != m_Params.Length)
						{
							continue;
						}
						bool flag = true;
						for (int k = 0; k < parameters.Length; k++)
						{
							Type parameterType = parameters[k].ParameterType;
							Type from = array[k];
							if (parameters[k].ParameterType != array[k] && !from.IsCastableTo(parameterType))
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							methodBase = methodBase2;
							break;
						}
					}
					else if (propertyInfo2 != null)
					{
						propertyInfo = propertyInfo2;
					}
				}
				if (methodBase != null)
				{
					object[] array2 = new object[m_Params.Length];
					for (int l = 0; l < m_Params.Length; l++)
					{
						IVariable variable2 = m_Params[l];
						if (variable2 != null)
						{
							array2[l] = m_Params[l].result;
						}
					}
					return methodBase.Invoke(source2, array2);
				}
				if (propertyInfo != null)
				{
					if (m_Params.Length <= 0)
					{
						return propertyInfo.GetValue(source2, null);
					}
					IVariable variable3 = m_Params.First();
					if (variable3 != null)
					{
						propertyInfo.SetValue(source2, variable3.result, null);
					}
				}
			}
			else if (member.Length > 0)
			{
				MemberInfo memberInfo2 = member.Find((MemberInfo m) => m.MemberType != MemberTypes.Method);
				if (memberInfo2.GetReturnType() != null)
				{
					return memberInfo2.GetMemberValue(source2);
				}
				(member.First() as MethodBase)?.Invoke(source2, null);
			}
			return null;
		}

		public void SetValue(object value, object source)
		{
			object source2 = GetSource(source);
			string name = m_Path.Split(' ')[1].Split('(')[0];
			MemberInfo[] member = m_Type.GetMember(name);
			MemberInfo member2 = member.Find((MemberInfo m) => m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property);
			member2.SetMemberValue(source2, value);
		}

		private object GetSource(object source)
		{
			object result = null;
			GameObject gameObject = source as GameObject;
			if ((bool)gameObject)
			{
				result = ((m_Type == typeof(GameObject)) ? ((UnityEngine.Object)gameObject) : ((UnityEngine.Object)gameObject.GetComponent(m_Type)));
			}
			else
			{
				Component component = source as Component;
				if ((bool)component)
				{
					result = component.GetComponent(m_Type);
				}
				else
				{
					MonoBehaviour monoBehaviour = source as MonoBehaviour;
					if ((bool)monoBehaviour)
					{
						result = monoBehaviour;
					}
				}
			}
			return result;
		}
	}
}
