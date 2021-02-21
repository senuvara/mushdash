using Assets.Scripts.PeroTools.Commons;
using System.Collections.Generic;
using UnityEngine;

public class FastPoolManager : SingletonMonoBehaviour<FastPoolManager>
{
	[SerializeField]
	private List<FastPool> predefinedPools = new List<FastPool>();

	private Dictionary<int, FastPool> pools;

	private Transform curTransform;

	public Dictionary<int, FastPool> Pools => pools;

	private void Awake()
	{
		curTransform = GetComponent<Transform>();
		pools = new Dictionary<int, FastPool>();
		for (int i = 0; i < predefinedPools.Count; i++)
		{
			if (predefinedPools[i].Init(curTransform))
			{
				if (!pools.ContainsKey(predefinedPools[i].ID))
				{
					pools.Add(predefinedPools[i].ID, predefinedPools[i]);
				}
				else
				{
					Debug.LogError("FastPoolManager has a several pools with the same ID. Please make sure that all your pools have unique IDs");
				}
			}
		}
		predefinedPools.Clear();
	}

	private void Start()
	{
	}

	public static FastPool CreatePoolC<T>(T component, bool warmOnLoad = true, int preloadCount = 0, int capacity = 0, Transform newRootTransform = null) where T : Component
	{
		if ((Object)component != (Object)null)
		{
			return CreatePool(component.gameObject, warmOnLoad, preloadCount, capacity, newRootTransform);
		}
		return null;
	}

	public static FastPool CreatePool(GameObject prefab, bool warmOnLoad = true, int preloadCount = 0, int capacity = 0, Transform newRootTransform = null)
	{
		if (prefab != null)
		{
			if (!SingletonMonoBehaviour<FastPoolManager>.instance.pools.ContainsKey(prefab.GetInstanceID()))
			{
				FastPool fastPool = new FastPool(prefab, newRootTransform ?? SingletonMonoBehaviour<FastPoolManager>.instance.curTransform, warmOnLoad, preloadCount, capacity);
				SingletonMonoBehaviour<FastPoolManager>.instance.pools.Add(prefab.GetInstanceID(), fastPool);
				return fastPool;
			}
			return SingletonMonoBehaviour<FastPoolManager>.instance.pools[prefab.GetInstanceID()];
		}
		Debug.LogError("Creating pool with null object");
		return null;
	}

	public static FastPool CreatePool(int id, GameObject prefab, bool warmOnLoad = true, int preloadCount = 0, int capacity = 0)
	{
		if (prefab != null)
		{
			if (!SingletonMonoBehaviour<FastPoolManager>.instance.pools.ContainsKey(id))
			{
				FastPool fastPool = new FastPool(id, prefab, SingletonMonoBehaviour<FastPoolManager>.instance.curTransform, warmOnLoad, preloadCount, capacity);
				SingletonMonoBehaviour<FastPoolManager>.instance.pools.Add(id, fastPool);
				return fastPool;
			}
			return SingletonMonoBehaviour<FastPoolManager>.instance.pools[id];
		}
		Debug.LogError("Creating pool with null object");
		return null;
	}

	public static FastPool GetPool(GameObject prefab, bool createIfNotExists = true)
	{
		if (prefab != null)
		{
			if (SingletonMonoBehaviour<FastPoolManager>.instance.pools.ContainsKey(prefab.GetInstanceID()))
			{
				return SingletonMonoBehaviour<FastPoolManager>.instance.pools[prefab.GetInstanceID()];
			}
			return CreatePool(prefab);
		}
		Debug.LogError("Trying to get pool for null object");
		return null;
	}

	public static FastPool GetPool(int id, GameObject prefab, bool createIfNotExists = true)
	{
		if (SingletonMonoBehaviour<FastPoolManager>.instance.pools.ContainsKey(id))
		{
			return SingletonMonoBehaviour<FastPoolManager>.instance.pools[id];
		}
		return CreatePool(id, prefab);
	}

	public static FastPool GetPool(Component component, bool createIfNotExists = true)
	{
		if (component != null)
		{
			GameObject gameObject = component.gameObject;
			if (SingletonMonoBehaviour<FastPoolManager>.instance.pools.ContainsKey(gameObject.GetInstanceID()))
			{
				return SingletonMonoBehaviour<FastPoolManager>.instance.pools[gameObject.GetInstanceID()];
			}
			return CreatePool(gameObject);
		}
		Debug.LogError("Trying to get pool for null object");
		return null;
	}

	public static void DestroyPool(FastPool pool)
	{
		pool.ClearCache();
		SingletonMonoBehaviour<FastPoolManager>.instance.pools.Remove(pool.ID);
	}
}
