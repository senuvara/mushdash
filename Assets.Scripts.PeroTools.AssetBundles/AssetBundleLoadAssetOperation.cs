using UnityEngine;

namespace Assets.Scripts.PeroTools.AssetBundles
{
	public abstract class AssetBundleLoadAssetOperation<T> : AssetBundleLoadOperation where T : Object
	{
		public abstract T GetAsset();
	}
}
