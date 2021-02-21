using UnityEngine;

namespace Assets.Scripts.PeroTools.Commons
{
	public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T m_Instance;

		public static T instance
		{
			get
			{
				if ((bool)(Object)m_Instance)
				{
					return m_Instance;
				}
				T obj = GameUtils.FindObjectOfType<T>() ?? new GameObject(typeof(T).Name).AddComponent<T>();
				m_Instance = obj;
				return obj;
			}
		}

		public void DestroyInstance()
		{
			if ((bool)base.gameObject)
			{
				Object.Destroy(base.gameObject);
			}
			m_Instance = (T)null;
		}

		private void Awake()
		{
			if (this != instance)
			{
				if (Application.isPlaying)
				{
					Object.Destroy(base.gameObject);
				}
				else
				{
					Object.DestroyImmediate(base.gameObject);
				}
			}
			else
			{
				Singleton.Init(m_Instance);
			}
		}
	}
}
