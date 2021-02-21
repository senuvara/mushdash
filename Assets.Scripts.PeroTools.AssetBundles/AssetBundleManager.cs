using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.PeroTools.AssetBundles
{
	[ExecuteInEditMode]
	public class AssetBundleManager : Singleton<AssetBundleManager>
	{
		public const string suffixRemoteManifest = "_remote";

		public const string suffixBackup = "_backup";

		private AssetBundleManifest m_AssetBundleManifest;

		private Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();

		private Dictionary<string, AssetBundleCreateRequest> m_LoadingRequests = new Dictionary<string, AssetBundleCreateRequest>();

		private Dictionary<string, string> m_LoadingErrors = new Dictionary<string, string>();

		private List<AssetBundleLoadOperation> m_InProgressOperations = new List<AssetBundleLoadOperation>();

		private Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();

		private List<string> m_KeysToRemove = new List<string>();

		public T LoadFromName<T>(string name) where T : UnityEngine.Object
		{
			AssetBundleConfigManager.ABConfig aBConfig = SingletonScriptableObject<AssetBundleConfigManager>.instance.Get<T>(name);
			if (aBConfig != null)
			{
				return LoadAssetFromAssetBundle<T>(aBConfig.abName, aBConfig.GetFullAssetPath());
			}
			Debug.LogWarningFormat("Can no find ab config of name : {0}", name);
			return (T)null;
		}

		public UnityEngine.Object LoadFromName(string name)
		{
			return LoadFromName<UnityEngine.Object>(name);
		}

		public void LoadFromNameAsyn<T>(string name, Action<T> callback) where T : UnityEngine.Object
		{
			AssetBundleConfigManager.ABConfig aBConfig = SingletonScriptableObject<AssetBundleConfigManager>.instance.Get(name);
			if (aBConfig != null)
			{
				LoadAssetAsync(aBConfig.abName, aBConfig.GetFullAssetPath(), callback);
			}
			else
			{
				callback((T)null);
			}
		}

		public void LoadLevelByName(string levelName, LoadSceneMode mode = LoadSceneMode.Single)
		{
			levelName = ((!levelName.EndsWith(".unity")) ? (levelName + ".unity") : levelName);
			AssetBundleConfigManager.ABConfig aBConfig = SingletonScriptableObject<AssetBundleConfigManager>.instance.Get<AssetBundleConfigManager.DefaultAsset>(levelName);
			if (aBConfig != null)
			{
				LoadLevel(aBConfig.abName, aBConfig.GetFullAssetPath(), mode);
				return;
			}
			Debug.LogWarningFormat("Can no find ab config of name : {0}", levelName);
		}

		public AssetBundleLoadLevelOperation LoadLevelAsyncByName(string levelName, LoadSceneMode mode = LoadSceneMode.Single)
		{
			levelName = ((!levelName.EndsWith(".unity")) ? (levelName + ".unity") : levelName);
			AssetBundleConfigManager.ABConfig aBConfig = SingletonScriptableObject<AssetBundleConfigManager>.instance.Get<AssetBundleConfigManager.DefaultAsset>(levelName);
			if (aBConfig != null)
			{
				return LoadLevelAsync(aBConfig.abName, aBConfig.GetFullAssetPath(), mode);
			}
			Debug.LogWarningFormat("Can no find ab config of name : {0}", levelName);
			return null;
		}

		public LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
		{
			if (m_LoadingErrors.TryGetValue(assetBundleName, out error))
			{
				return null;
			}
			LoadedAssetBundle value;
			m_LoadedAssetBundles.TryGetValue(assetBundleName, out value);
			if (value == null)
			{
				return null;
			}
			string[] value2;
			if (!m_Dependencies.TryGetValue(assetBundleName, out value2))
			{
				return value;
			}
			string[] array = value2;
			foreach (string key in array)
			{
				if (m_LoadingErrors.TryGetValue(assetBundleName, out error))
				{
					return value;
				}
				LoadedAssetBundle value3;
				m_LoadedAssetBundles.TryGetValue(key, out value3);
				if (value3 == null)
				{
					return null;
				}
			}
			return value;
		}

		public T LoadAssetFromAssetBundle<T>(string assetBundleName, string fullAssetPath) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(assetBundleName))
			{
				Debug.LogErrorFormat("Unable to load assetbundle with an empty name");
				return (T)null;
			}
			LoadAssetBundle(assetBundleName, false);
			return m_LoadedAssetBundles[assetBundleName].assetBundle.LoadAsset<T>(fullAssetPath);
		}

		public T[] LoadAllAssetFromAssetBundle<T>(string assetBundleName) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(assetBundleName))
			{
				Debug.LogErrorFormat("Unable to load assetbundle with an empty name");
				return null;
			}
			LoadAssetBundle(assetBundleName, false);
			return m_LoadedAssetBundles[assetBundleName].assetBundle.LoadAllAssets<T>();
		}

		public void LoadAssetAsync<T>(string assetBundleName, string fullAssetPath, Action<T> callback) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(assetBundleName))
			{
				Debug.LogErrorFormat("Unable to load assetbundle with an empty name");
				return;
			}
			LoadAssetBundle(assetBundleName, true);
			AssetBundleLoadAssetOperation<T> operation = new AssetBundleLoadAssetOperationFull<T>(assetBundleName, fullAssetPath);
			m_InProgressOperations.Add(operation);
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				callback(operation.GetAsset());
			}, () => operation.IsDone());
		}

		private void LoadLevel(string assetBundleName, string fullLevelPath, LoadSceneMode mode = LoadSceneMode.Single)
		{
			if (string.IsNullOrEmpty(assetBundleName))
			{
				Debug.LogErrorFormat("Unable to load assetbundle with an empty name");
				return;
			}
			LoadAssetBundle(assetBundleName, false);
			string sceneName = m_LoadedAssetBundles[assetBundleName].assetBundle.GetAllScenePaths()[0];
			UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, mode);
		}

		private AssetBundleLoadLevelOperation LoadLevelAsync(string assetBundleName, string fullLevelPath, LoadSceneMode mode = LoadSceneMode.Single)
		{
			if (string.IsNullOrEmpty(assetBundleName))
			{
				Debug.LogErrorFormat("Unable to load assetbundle with an empty name");
				return null;
			}
			LoadAssetBundle(assetBundleName, true);
			AssetBundleLoadLevelOperation assetBundleLoadLevelOperation = new AssetBundleLoadLevelOperationFull(assetBundleName, fullLevelPath, mode);
			m_InProgressOperations.Add(assetBundleLoadLevelOperation);
			return assetBundleLoadLevelOperation;
		}

		public void LoadAssetBundle(string assetBundleName, bool async)
		{
			if (!LoadAssetBundleExternal(assetBundleName, async))
			{
				LoadDependencies(assetBundleName, async);
			}
		}

		protected bool LoadAssetBundleExternal(string assetBundleName, bool async)
		{
			LoadedAssetBundle value;
			if (m_LoadedAssetBundles.TryGetValue(assetBundleName, out value))
			{
				value.referencedCount++;
				return true;
			}
			if (m_LoadingRequests.ContainsKey(assetBundleName))
			{
				if (!async)
				{
					Debug.LogFormat("[AssetBundle] You're sync loading a AssetBundle that is in async loading progress,Auto change the async progress to sync.");
					AssetBundle assetBundle = m_LoadingRequests[assetBundleName].assetBundle;
					AddAssetBundleRecord(assetBundleName, assetBundle);
					return true;
				}
				return true;
			}
			string text = Path.Combine(Settings.currentSetting.firstLoadAssetPath, assetBundleName);
			if (!FileUtils.Exists(text))
			{
				text = Path.Combine(Settings.currentSetting.streamingAssetAbsPath, assetBundleName);
				if (!FileUtils.Exists(text))
				{
					Debug.LogErrorFormat("Unable to loaded file [{0}],it doesn't exist. ", text);
				}
			}
			if (async)
			{
				try
				{
					AssetBundleCreateRequest value2 = AssetBundle.LoadFromFileAsync(text, 0u);
					m_LoadingRequests.Add(assetBundleName, value2);
				}
				catch (Exception message)
				{
					Debug.LogError(message);
				}
				return false;
			}
			try
			{
				AssetBundle assetBundle2 = AssetBundle.LoadFromFile(text, 0u);
				if (assetBundle2 == null)
				{
					Debug.LogErrorFormat("Load AssetBundle from [{0}] fail , file doesn't exist.", text);
				}
				AddAssetBundleRecord(assetBundleName, assetBundle2);
			}
			catch (Exception message2)
			{
				Debug.LogError(message2);
			}
			return false;
		}

		protected void LoadDependencies(string assetBundleName, bool async)
		{
			string[] dependencies = GetDependencies(assetBundleName);
			if (dependencies.Length != 0)
			{
				m_Dependencies.Add(assetBundleName, dependencies);
				for (int i = 0; i < dependencies.Length; i++)
				{
					LoadAssetBundleExternal(dependencies[i], async);
				}
			}
		}

		private string[] GetDependencies(string assetBundleName)
		{
			return m_AssetBundleManifest.GetAllDependencies(assetBundleName);
		}

		public void UnloadAssetBundle(string assetBundleName, bool unloadAllObjects)
		{
			UnloadAssetBundleInternal(assetBundleName, unloadAllObjects);
			UnloadDependencies(assetBundleName, unloadAllObjects);
		}

		protected void UnloadDependencies(string assetBundleName, bool unloadAllObjects)
		{
			string[] value;
			if (m_Dependencies.TryGetValue(assetBundleName, out value))
			{
				string[] array = value;
				foreach (string assetBundleName2 in array)
				{
					UnloadAssetBundleInternal(assetBundleName2, unloadAllObjects);
				}
				m_Dependencies.Remove(assetBundleName);
			}
		}

		protected void UnloadAssetBundleInternal(string assetBundleName, bool unloadAllObjects)
		{
			string error;
			LoadedAssetBundle loadedAssetBundle = GetLoadedAssetBundle(assetBundleName, out error);
			if (loadedAssetBundle != null && --loadedAssetBundle.referencedCount <= 0)
			{
				loadedAssetBundle.assetBundle.Unload(unloadAllObjects);
				m_LoadedAssetBundles.Remove(assetBundleName);
				Debug.Log("[Unload] Assetbundle [" + assetBundleName + "] successfully");
			}
		}

		private void Update(float runtime)
		{
			if (m_LoadingRequests != null && m_LoadingRequests.Count > 0)
			{
				m_KeysToRemove.Clear();
				foreach (KeyValuePair<string, AssetBundleCreateRequest> loadingRequest in m_LoadingRequests)
				{
					AssetBundleCreateRequest value = loadingRequest.Value;
					if (!value.isDone)
					{
						continue;
					}
					AssetBundle assetBundle = value.assetBundle;
					if (assetBundle == null)
					{
						m_LoadingErrors.Add(loadingRequest.Key, $"{loadingRequest.Key} is not a valid asset bundle.");
						m_KeysToRemove.Add(loadingRequest.Key);
						continue;
					}
					if (!m_LoadedAssetBundles.ContainsKey(loadingRequest.Key))
					{
						AddAssetBundleRecord(loadingRequest.Key, value.assetBundle);
					}
					m_KeysToRemove.Add(loadingRequest.Key);
				}
				for (int i = 0; i < m_KeysToRemove.Count; i++)
				{
					m_LoadingRequests.Remove(m_KeysToRemove[i]);
				}
			}
			for (int num = m_InProgressOperations.Count - 1; num >= 0; num--)
			{
				if (!m_InProgressOperations[num].Update())
				{
					m_InProgressOperations.RemoveAt(num);
				}
			}
		}

		private void LoadManifestFile()
		{
			LoadAssetBundleExternal(Settings.currentSetting.manifestFileName, false);
			string error;
			AssetBundle assetBundle = GetLoadedAssetBundle(Settings.currentSetting.manifestFileName, out error).assetBundle;
			m_AssetBundleManifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
			UnloadAssetBundle(Settings.currentSetting.manifestFileName, false);
		}

		private void Init()
		{
			if (Application.isPlaying)
			{
				SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop(GetType().Name, Update, UnityGameManager.LoopType.Update);
			}
			LoadManifestFile();
		}

		private void AddAssetBundleRecord(string assetBundleName, AssetBundle ab)
		{
			m_LoadedAssetBundles.Add(assetBundleName, new LoadedAssetBundle(ab));
			ShaderHack(ab);
		}

		public void ShaderHack(AssetBundle bundle)
		{
		}
	}
}
