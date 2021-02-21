using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Managers;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Reflection;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Commons
{
	public class SingletonScriptableObject<T> : SerializedScriptableObject where T : SerializedScriptableObject
	{
		protected static T m_Instance;

		public static T instance
		{
			get
			{
				if (!(Object)m_Instance)
				{
					if (typeof(T) == typeof(AssetBundleConfigManager))
					{
						m_Instance = Singleton<AssetBundleManager>.instance.LoadAssetFromAssetBundle<T>("globalconfigs", "Assets/Static Resources/_Programs/GlobalConfigs/AssetBundleConfigManager.asset");
					}
					else
					{
						m_Instance = Singleton<AssetBundleManager>.instance.LoadFromName<T>(typeof(T).GetNiceName());
					}
					if ((Object)m_Instance == (Object)null)
					{
						Debug.LogErrorFormat("Unable to load ScriptableObject : [{0]}, no asset exist.", typeof(T).GetNiceName());
						return (T)null;
					}
					Singleton.Init(m_Instance);
				}
				return m_Instance;
			}
		}

		public static string GetSingleScriptableObjectDirectory()
		{
			MethodInfo method = typeof(T).GetMethod("OverrideDirectory");
			if (method != null)
			{
				return (string)method.Invoke(null, null);
			}
			return "Assets/Static Resources/_Programs/GlobalConfigs/";
		}

		public void DestroyInstance()
		{
			Object.Destroy(m_Instance);
			m_Instance = (T)null;
		}
	}
}
