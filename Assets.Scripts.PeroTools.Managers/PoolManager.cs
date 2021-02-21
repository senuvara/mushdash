using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Managers
{
	public class PoolManager : Singleton<PoolManager>
	{
		public Dictionary<string, FastPool> fastPools
		{
			get;
			private set;
		}

		public FastPool this[string name] => (!fastPools.ContainsKey(name)) ? null : fastPools[name];

		private void Init()
		{
			fastPools = new Dictionary<string, FastPool>();
		}

		public FastPool MakeFastPool<T>(T source, int preload, int capacity = -1, Transform parent = null) where T : Component
		{
			if (fastPools.ContainsKey(source.name))
			{
				return fastPools[source.name];
			}
			FastPool fastPool = FastPoolManager.CreatePoolC(source, false, preload, capacity, parent);
			fastPools.Add(source.name, fastPool);
			return fastPool;
		}

		public FastPool MakeFastPool(GameObject source, int preload, int capacity = -1, Transform parent = null)
		{
			if (fastPools.ContainsKey(source.name))
			{
				return fastPools[source.name];
			}
			FastPool fastPool = FastPoolManager.CreatePool(source, false, preload, capacity, parent);
			fastPools.Add(source.name, fastPool);
			return fastPool;
		}

		public FastPool MakeFastPool(string name, int preload, int capacity, Transform parent = null)
		{
			if (fastPools.ContainsKey(name))
			{
				return fastPools[name];
			}
			GameObject gameObject = Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(name);
			FastPool fastPool = FastPoolManager.CreatePool(gameObject, false, preload, capacity, parent);
			fastPools.Add(gameObject.name, fastPool);
			return fastPool;
		}

		public void DestroyFastPool(FastPool pool)
		{
			if (pool != null)
			{
				DestroyFastPool(pool.sourcePrefab.name);
			}
		}

		public void DestroyFastPool(string uid)
		{
			if (fastPools.ContainsKey(uid))
			{
				fastPools[uid].ClearCache();
				fastPools.Remove(uid);
			}
		}

		public GameObject FastInstantiate(string name, int preload, int capacity = -1, Transform parent = null)
		{
			FastPool fastPool = MakeFastPool(name, preload, capacity, parent);
			return fastPool.FastInstantiate();
		}

		public GameObject FastInstantiate(GameObject go, int preload, int capacity = -1, Transform parent = null)
		{
			FastPool fastPool = MakeFastPool(go, preload, capacity, parent);
			return fastPool.FastInstantiate();
		}

		public void FastDestroy(GameObject gameObject)
		{
			if ((bool)gameObject)
			{
				FastPool fastPool = this[gameObject.name];
				if (fastPool != null && gameObject.activeSelf)
				{
					fastPool.FastDestroy(gameObject);
				}
				else
				{
					Object.Destroy(gameObject);
				}
			}
		}
	}
}
