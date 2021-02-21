using Assets.Scripts.PeroTools.Commons;
using UnityEngine;

namespace Assets.Scripts.PeroTools.AssetBundles
{
	public class AssetBundleLoadAssetOperationFull<T> : AssetBundleLoadAssetOperation<T> where T : Object
	{
		protected string m_AssetBundleName;

		protected string m_AssetName;

		protected string m_LoadingError;

		protected AssetBundleRequest m_Request;

		public AssetBundleLoadAssetOperationFull(string bundleName, string assetName)
		{
			m_AssetBundleName = bundleName;
			m_AssetName = assetName;
		}

		public override T GetAsset()
		{
			if (m_Request != null && m_Request.isDone)
			{
				return m_Request.asset as T;
			}
			return (T)null;
		}

		public override bool Update()
		{
			if (m_Request != null)
			{
				return false;
			}
			if (isBundleLoadDone())
			{
				LoadedAssetBundle loadedAssetBundle = Singleton<AssetBundleManager>.instance.GetLoadedAssetBundle(m_AssetBundleName, out m_LoadingError);
				m_Request = loadedAssetBundle.assetBundle.LoadAssetAsync<T>(m_AssetName);
				return false;
			}
			return true;
		}

		public override bool IsDone()
		{
			if (m_Request == null && !string.IsNullOrEmpty(m_LoadingError))
			{
				Debug.LogError(m_LoadingError);
				return true;
			}
			return m_Request != null && m_Request.isDone;
		}

		public override bool IsError()
		{
			return !string.IsNullOrEmpty(m_LoadingError);
		}

		private bool isBundleLoadDone()
		{
			LoadedAssetBundle loadedAssetBundle = Singleton<AssetBundleManager>.instance.GetLoadedAssetBundle(m_AssetBundleName, out m_LoadingError);
			if (loadedAssetBundle != null)
			{
				return true;
			}
			return false;
		}
	}
}
