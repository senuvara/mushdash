using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Values;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Scripts.PeroTools.Nice.Interface
{
	public static class VariableUtils
	{
		public static void SetResult(this IVariable data, object value)
		{
			Type type = value.GetType();
			if (ReflectionUtils.IsArray(type))
			{
				List<object> list = new List<object>();
				IList list2 = value as IList;
				if (list2 != null)
				{
					for (int i = 0; i < list2.Count; i++)
					{
						list.Add(list2[i]);
					}
				}
				data.result = list;
			}
			else
			{
				data.result = value;
			}
		}

		public static object GetResult(this IVariable data, Type type)
		{
			object result = data.result;
			object result2 = null;
			if (result != null)
			{
				if (type.IsInstanceOfType(result))
				{
					result2 = result;
				}
				else if (ReflectionUtils.IsArray(type))
				{
					IList list = result as IList;
					if (list != null)
					{
						if (type.IsArray)
						{
							Array array = Array.CreateInstance(ReflectionUtils.GetElementType(type), list.Count);
							for (int i = 0; i < list.Count; i++)
							{
								object obj = list[i];
								IValue value = obj as IValue;
								array.SetValue((value == null) ? obj : value.result, i);
							}
							result2 = array;
						}
						if (typeof(IList).IsAssignableFrom(type))
						{
							IList list2 = Activator.CreateInstance(type) as IList;
							if (list2 != null)
							{
								for (int j = 0; j < list.Count; j++)
								{
									object obj2 = list[j];
									IValue value2 = obj2 as IValue;
									list2.Add((value2 == null) ? obj2 : value2.result);
								}
								result2 = list2;
							}
						}
					}
				}
				else
				{
					float num = float.Parse(result.ToString());
					result2 = ((type != typeof(int)) ? ((object)num) : ((object)(int)num));
				}
			}
			data.result = result2;
			return result2;
		}

		public static T GetResult<T>(this IVariable data)
		{
			object obj = data.result;
			T val = default(T);
			if (obj == null)
			{
				obj = val;
			}
			Type typeFromHandle = typeof(T);
			if (typeFromHandle.IsInstanceOfType(obj) && obj != null)
			{
				val = (T)obj;
			}
			else if (ReflectionUtils.IsArray(typeFromHandle))
			{
				IList list = (obj != null) ? (obj as IList) : new List<object>();
				if (list != null)
				{
					if (typeFromHandle.IsArray)
					{
						Array array = Array.CreateInstance(ReflectionUtils.GetElementType(typeFromHandle), list.Count);
						for (int i = 0; i < list.Count; i++)
						{
							object obj2 = list[i];
							IValue value = obj2 as IValue;
							array.SetValue((value == null) ? obj2 : value.result, i);
						}
						val = (T)(object)array;
					}
					if (typeof(IList).IsAssignableFrom(typeFromHandle))
					{
						IList list2 = Activator.CreateInstance<T>() as IList;
						if (list2 != null)
						{
							for (int j = 0; j < list.Count; j++)
							{
								object obj3 = list[j];
								IValue value2 = obj3 as IValue;
								list2.Add((value2 == null) ? obj3 : value2.result);
							}
							val = (T)list2;
						}
					}
				}
			}
			else if (obj != null)
			{
				if (typeFromHandle == typeof(string))
				{
					val = (T)(object)obj.ToString();
				}
				else
				{
					float num = float.Parse(obj.ToString());
					val = ((typeFromHandle != typeof(int)) ? ((T)(object)num) : ((T)(object)(int)num));
				}
			}
			data.result = val;
			return val;
		}

		public static IValue CreateIValue(Type resultType)
		{
			IValue result = null;
			Type type = typeof(IValue);
			List<Type> list = (from t in type.Assembly.GetTypes()
				where t.InheritsFrom(type)
				select t).ToList();
			Type type2 = list.Find(delegate(Type t)
			{
				FieldInfo field = t.GetField("m_Result", BindingFlags.Instance | BindingFlags.NonPublic);
				return field != null && field.FieldType == resultType;
			});
			if (type2 != null)
			{
				result = (IValue)Activator.CreateInstance(type2);
			}
			return result;
		}

		public static IValue CreateIValue(object result)
		{
			if (result == null)
			{
				return null;
			}
			IValue value = null;
			IValue value2 = result as IValue;
			if (value2 != null)
			{
				return value2;
			}
			Type type = typeof(IValue);
			Type resultType = result.GetType();
			if (resultType == typeof(long))
			{
				resultType = typeof(int);
				result = int.Parse(result.ToString());
			}
			if (resultType == typeof(double))
			{
				resultType = typeof(float);
				result = float.Parse(result.ToString());
			}
			Type type2;
			if (ReflectionUtils.IsArray(resultType))
			{
				type2 = typeof(List);
			}
			else
			{
				List<Type> list = (from t in type.Assembly.GetTypes()
					where t.InheritsFrom(type)
					select t).ToList();
				type2 = list.Find(delegate(Type t)
				{
					FieldInfo field = t.GetField("m_Result", BindingFlags.Instance | BindingFlags.NonPublic);
					return field != null && field.FieldType == resultType;
				});
			}
			if (type2 != null)
			{
				value = (IValue)Activator.CreateInstance(type2);
				value.result = result;
			}
			return value;
		}

		public static void Set(this IVariable variable, string name, object value)
		{
			Type type = variable.GetType();
			type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.SetValue(variable, value);
		}

		public static object Get(this IVariable variable, string name)
		{
			Type type = variable.GetType();
			return type.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(variable);
		}
	}
}
