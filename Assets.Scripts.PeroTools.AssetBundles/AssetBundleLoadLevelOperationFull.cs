using Assets.Scripts.PeroTools.Commons;
using System.IO;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.PeroTools.AssetBundles
{
	public class AssetBundleLoadLevelOperationFull : AssetBundleLoadLevelOperation
	{
		protected string m_AssetBundleName;

		protected string m_FullLevelPath;

		protected LoadSceneMode m_Mode;

		protected string m_DownloadingError;

		public AssetBundleLoadLevelOperationFull(string assetbundleName, string fullLevelPath, LoadSceneMode mode)
		{
			m_AssetBundleName = assetbundleName;
			m_FullLevelPath = fullLevelPath;
			m_Mode = mode;
		}

		public override bool Update()
		{
			if (m_Request != null)
			{
				return false;
			}
			LoadedAssetBundle loadedAssetBundle = Singleton<AssetBundleManager>.instance.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError);
			if (loadedAssetBundle != null)
			{
				LoadSceneMode mode = m_Mode;
				m_Request = SceneManager.LoadSceneAsync(Path.GetFileNameWithoutExtension(m_FullLevelPath), mode);
				return false;
			}
			return true;
		}

		public override bool IsDone()
		{
			return Singleton<AssetBundleManager>.instance.GetLoadedAssetBundle(m_AssetBundleName, out m_DownloadingError) != null;
		}

		public override bool IsError()
		{
			return !string.IsNullOrEmpty(m_DownloadingError);
		}
	}
}
