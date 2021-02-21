using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Values;
using Assets.Scripts.PeroTools.Nice.Variables;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Assets.Scripts.PeroTools.Nice.Interface
{
	public static class DataUtils
	{
		public static bool Exists(this IData data, string uid)
		{
			Dictionary<string, IVariable> fields = data.fields;
			if (!fields.ContainsKey(uid) || fields[uid] == null)
			{
				return false;
			}
			return true;
		}

		public static T Get<T>(this IData data)
		{
			T val = Activator.CreateInstance<T>();
			Type typeFromHandle = typeof(T);
			foreach (KeyValuePair<string, IVariable> field in data.fields)
			{
				MemberInfo[] member = typeFromHandle.GetMember(field.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (member.Length > 0)
				{
					MemberInfo memberInfo = member.First();
					memberInfo.SetMemberValue(val, field.Value.GetResult(memberInfo.GetReturnType()));
				}
			}
			return val;
		}

		public static void Set(this IData data, string uid, IVariable variable)
		{
			data.fields[uid] = variable;
		}

		public static IVariable Get(this IData data, string uid)
		{
			if (!data.Exists(uid))
			{
				Constance constance = new Constance();
				constance.Set("m_Value", new Ref());
				data.Set(uid, constance);
				return constance;
			}
			return data.fields[uid];
		}
	}
}
