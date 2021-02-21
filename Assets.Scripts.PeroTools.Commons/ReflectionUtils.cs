using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Commons
{
	public static class ReflectionUtils
	{
		[Serializable]
		public class ReflectObject
		{
			public UnityEngine.Object sourceObject;

			public string reflectName;
		}

		public static Dictionary<string, MemberInfo> GetTypeMemberNames(Type type, bool get = true, bool set = true, BindingFlags bf = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
		{
			Dictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();
			BindingFlags bindingAttr = bf;
			if (type.IsSubclassOf(typeof(MonoBehaviour)))
			{
				bindingAttr = (BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			}
			MemberInfo[] members2 = type.GetMembers(bindingAttr);
			members2.For(delegate(MemberInfo m)
			{
				Type returnType = m.GetReturnType();
				if (returnType != null)
				{
					string key = $"{type.Name}/{m.GetReturnType().GetNiceName()} {m.GetNiceName()}";
					if (!members.ContainsKey(key))
					{
						members.Add(key, m);
					}
				}
			});
			return LimitMemeberInfo(members, get, set);
		}

		public static Dictionary<string, MemberInfo> GetMemberNames(this GameObject gameObject, bool get = true, bool set = true, Func<MemberInfo, bool> predicate = null)
		{
			Dictionary<string, MemberInfo> members = new Dictionary<string, MemberInfo>();
			if (!gameObject)
			{
				return members;
			}
			BindingFlags bf = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
			List<UnityEngine.Object> list = new List<UnityEngine.Object>();
			list.AddRange(gameObject.GetComponents<Component>());
			list.Add(gameObject);
			list.For(delegate(UnityEngine.Object obj)
			{
				Type type = obj.GetType();
				Dictionary<string, MemberInfo> typeMemberNames = GetTypeMemberNames(type, get, set, bf);
				foreach (KeyValuePair<string, MemberInfo> item in typeMemberNames)
				{
					if (!members.ContainsKey(item.Key))
					{
						MemberInfo value = item.Value;
						if (predicate != null)
						{
							if (predicate(value))
							{
								members.Add(item.Key, value);
							}
						}
						else
						{
							members.Add(item.Key, value);
						}
					}
				}
			});
			return LimitMemeberInfo(members, get, set);
		}

		public static Dictionary<string, MemberInfo> LimitMemeberInfo(Dictionary<string, MemberInfo> members, bool get, bool set)
		{
			List<string> list = (from m in members
				where m.Key.Contains("INTERNAL") || m.Key.Contains("get") || m.Key.Contains("set") || m.Key.Contains("<>f_am")
				where m.Value.MemberType == MemberTypes.Method
				select m.Key).ToList();
			if (!get)
			{
				foreach (KeyValuePair<string, MemberInfo> member in members)
				{
					if (!member.Key.Contains("Void ") && member.Value.MemberType == MemberTypes.Method && !list.Contains(member.Key))
					{
						list.Add(member.Key);
					}
				}
			}
			if (!set)
			{
				foreach (KeyValuePair<string, MemberInfo> member2 in members)
				{
					if (member2.Key.Contains("Void ") && member2.Value.MemberType == MemberTypes.Method && !list.Contains(member2.Key))
					{
						list.Add(member2.Key);
					}
				}
			}
			list.For(delegate(string l)
			{
				members.Remove(l);
			});
			return members;
		}

		public static string NickNameToReflectName(string str)
		{
			return str.Split(' ')[1].Split('(')[0];
		}

		public static string Reflect(ReflectObject reflectObj, object[] idxs)
		{
			return Reflect(reflectObj.sourceObject, reflectObj.reflectName, idxs);
		}

		public static string Reflect(UnityEngine.Object sourceObj, string fieldName, object[] idxs)
		{
			string result = "1";
			if (sourceObj != null)
			{
				string[] array = fieldName.Split('/');
				string text = array[0];
				string name = array[1];
				if (text != "GameObject")
				{
					GameObject gameObject = sourceObj as GameObject;
					if (gameObject != null)
					{
						sourceObj = gameObject.GetComponent(text);
					}
				}
				object value = GetValue(sourceObj, name, idxs);
				result = ((value == null) ? string.Empty : value.ToString());
			}
			return result;
		}

		public static object Reflect(object obj, string path, string[] idxs)
		{
			List<string> list = path.Split('/').ToList();
			MemberInfo memberInfo = obj.GetType().GetMember(list.First()).First();
			list.RemoveAt(0);
			int idxsNum = 0;
			list.For(delegate(string m)
			{
				Type memberType = GetMemberType(memberInfo);
				if (IsArray(memberType))
				{
					Type elementType = GetElementType(memberType);
					if (elementType != null)
					{
						obj = JsonUtils.Deserialize(JsonUtils.Serialize(GetValue(obj, memberInfo.Name)), elementType.MakeArrayType());
						Array array = obj as Array;
						if (array != null && idxsNum < idxs.Length)
						{
							int num = int.Parse(idxs[idxsNum++].ToString());
							if (num >= 0 && num < array.Length)
							{
								obj = array.GetValue(num);
							}
						}
						MemberInfo[] member = elementType.GetMember(m);
						if (member.Length == 0)
						{
							if (m == "Count" && array != null)
							{
								obj = array.Length;
							}
							memberInfo = null;
						}
						else
						{
							memberInfo = member.First();
						}
					}
				}
				else if (IsClass(memberType))
				{
					obj = GetValue(obj, memberInfo.Name);
					memberInfo = memberType.GetMember(m).First();
				}
			});
			if (memberInfo != null)
			{
				obj = GetValue(obj, memberInfo.Name);
			}
			return obj;
		}

		public static object GetValue(object sourceObj, string name, object[] objs = null, BindingFlags bf = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
		{
			object result = string.Empty;
			if (sourceObj == null)
			{
				return result;
			}
			Type type = sourceObj.GetType();
			MemberInfo[] member = type.GetMember(name, bf);
			if (member.Length != 0)
			{
				MemberInfo memberInfo = member.First();
				switch (memberInfo.MemberType)
				{
				case MemberTypes.Field:
				{
					FieldInfo field = type.GetField(name, bf);
					if (field != null)
					{
						result = field.GetValue(sourceObj);
					}
					break;
				}
				case MemberTypes.Method:
				{
					MethodInfo method = type.GetMethod(name, bf);
					List<object> list = new List<object>();
					if (method == null)
					{
						break;
					}
					for (int j = 0; j < method.GetParameters().Length; j++)
					{
						if (objs != null && j < objs.Length)
						{
							Type parameterType = method.GetParameters()[j].ParameterType;
							object item = Convert.ChangeType(objs[j], parameterType);
							list.Add(item);
						}
					}
					result = method.Invoke(sourceObj, list.ToArray());
					break;
				}
				case MemberTypes.Property:
				{
					PropertyInfo property = type.GetProperty(name, bf);
					if (property == null)
					{
						break;
					}
					MethodInfo setMethod = property.GetSetMethod();
					if (setMethod != null && objs != null && objs.Length > 0)
					{
						ParameterInfo[] parameters = setMethod.GetParameters();
						List<object> list = new List<object>();
						for (int i = 0; i < parameters.Length; i++)
						{
							if (i < objs.Length)
							{
								ParameterInfo parameterInfo = parameters[i];
								list.Add(Convert.ChangeType(objs[i], parameterInfo.ParameterType));
							}
						}
						setMethod.Invoke(sourceObj, list.ToArray());
					}
					else
					{
						result = property.GetValue(sourceObj, null);
					}
					break;
				}
				}
			}
			return result;
		}

		public static Type GetElementType(Type type)
		{
			return (!type.IsArray) ? type.GetGenericArguments().First() : type.GetElementType();
		}

		public static Type GetMemberType(MemberInfo memberInfo)
		{
			MemberTypes memberType = memberInfo.MemberType;
			Type result = memberType.GetType();
			switch (memberType)
			{
			case MemberTypes.Field:
			{
				FieldInfo fieldInfo = memberInfo as FieldInfo;
				if (fieldInfo != null)
				{
					result = fieldInfo.FieldType;
				}
				break;
			}
			case MemberTypes.Method:
			{
				MethodInfo methodInfo = memberInfo as MethodInfo;
				if (methodInfo != null)
				{
					result = methodInfo.ReturnType;
				}
				break;
			}
			case MemberTypes.Property:
			{
				PropertyInfo propertyInfo = memberInfo as PropertyInfo;
				if (propertyInfo != null)
				{
					result = propertyInfo.PropertyType;
				}
				break;
			}
			}
			return result;
		}

		public static int ArrayDepth(Type type, string path)
		{
			int num = 0;
			List<string> array = path.Split('/').ToList();
			array.For(delegate(string mi)
			{
				MemberInfo[] member = type.GetMember(mi);
				if (member.Length != 0)
				{
					type = GetMemberType(member.First());
					if (IsArray(type))
					{
						type = GetElementType(type);
						num++;
					}
				}
				else if (mi == "Count")
				{
					num--;
				}
			});
			return num;
		}

		public static string GetFuncName(object obj, string method)
		{
			if (obj == null)
			{
				return "<null>";
			}
			string text = obj.GetType().ToString();
			int num = text.LastIndexOf('/');
			if (num > 0)
			{
				text = text.Substring(num + 1);
			}
			return (!string.IsNullOrEmpty(method)) ? (text + "/" + method) : text;
		}

		public static bool IsArray(Type type)
		{
			return typeof(ICollection).IsAssignableFrom(type) || type.IsArray;
		}

		public static bool IsClass(Type type)
		{
			return type.IsClass && !typeof(string).IsAssignableFrom(type) && type != typeof(Type) && type != typeof(object);
		}

		public static Type[] GetSubClassOf(Type type)
		{
			List<Type> list = from t in Assembly.GetExecutingAssembly().GetTypes()
				where IsSubClassOf(t, type)
				select t;
			return list.ToArray();
		}

		public static string[] GetSubClassNameOf(Type type)
		{
			return (from t in GetSubClassOf(type)
				select t.Name).ToArray();
		}

		public static bool IsSubClassOf(Type type, Type baseType)
		{
			for (Type baseType2 = type.BaseType; baseType2 != null; baseType2 = baseType2.BaseType)
			{
				if (baseType2 == baseType)
				{
					return true;
				}
			}
			return false;
		}
	}
}
