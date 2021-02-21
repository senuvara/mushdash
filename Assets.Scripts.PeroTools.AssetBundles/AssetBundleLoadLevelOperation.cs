using UnityEngine;

namespace Assets.Scripts.PeroTools.AssetBundles
{
	public abstract class AssetBundleLoadLevelOperation : AssetBundleLoadOperation
	{
		protected AsyncOperation m_Request;

		public AsyncOperation GetLoadRequest()
		{
			return m_Request;
		}
	}
}
