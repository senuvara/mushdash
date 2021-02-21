using System;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Commons
{
	public class Singleton
	{
		public static void Init(object instance)
		{
			MemberInfo[] member = instance.GetType().GetMember("Init", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (member.Length <= 0)
			{
				return;
			}
			MethodBase methodBase = member.First() as MethodBase;
			if (methodBase != null)
			{
				try
				{
					methodBase.Invoke(instance, null);
				}
				catch (Exception arg)
				{
					Debug.LogWarning(string.Concat(instance.GetType(), "====", arg));
				}
			}
		}

		public static void Destroy(object instance)
		{
			MemberInfo[] member = instance.GetType().GetMember("Destroy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (member.Length <= 0)
			{
				return;
			}
			MethodBase methodBase = member.First() as MethodBase;
			if (methodBase != null)
			{
				try
				{
					methodBase.Invoke(instance, null);
				}
				catch (Exception ex)
				{
					Debug.Log(ex.ToString());
				}
			}
		}
	}
	public class Singleton<T> where T : new()
	{
		private static T m_Instance;

		public static T instance
		{
			get
			{
				if (m_Instance == null)
				{
					m_Instance = new T();
					Singleton.Init(m_Instance);
				}
				return m_Instance;
			}
		}

		public void Reset()
		{
			Singleton.Destroy(m_Instance);
			m_Instance = default(T);
		}
	}
}
