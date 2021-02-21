using UnityEngine;

namespace Assets.Scripts.PeroTools.AssetBundles
{
	public class LoadedAssetBundle
	{
		public AssetBundle assetBundle;

		public int referencedCount;

		public LoadedAssetBundle(AssetBundle assetBundle)
		{
			this.assetBundle = assetBundle;
			referencedCount = 1;
		}
	}
}
