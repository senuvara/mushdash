using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.PeroTools.Managers
{
	public class SceneManager : Singleton<SceneManager>
	{
		public string loadingSceneName = "Loading";

		public float leastLoadingTime = 2f;

		public string loadSceneNameViaLoadingScene;

		public UnityAction completeCallBack;

		public UnityAction preloadCallback;

		public string preSceneName
		{
			get;
			private set;
		}

		public Scene curScene => UnityEngine.SceneManagement.SceneManager.GetActiveScene();

		public string sceneName => UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

		public Scene scene => UnityEngine.SceneManagement.SceneManager.GetActiveScene();

		public event UnityAction onBeforeLoadNewScene;

		public event UnityAction<Scene, LoadSceneMode> onSceneLoad
		{
			add
			{
				UnityEngine.SceneManagement.SceneManager.sceneLoaded += value;
			}
			remove
			{
				UnityEngine.SceneManagement.SceneManager.sceneLoaded -= value;
			}
		}

		public event UnityAction<Scene, Scene> onSceneChanged
		{
			add
			{
				UnityEngine.SceneManagement.SceneManager.activeSceneChanged += value;
			}
			remove
			{
				UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= value;
			}
		}

		private void Init()
		{
			ConstanceManager.ConstanceInfo constanceInfo = SingletonScriptableObject<ConstanceManager>.instance.Get("LoadingTime");
			leastLoadingTime = ((constanceInfo != null) ? float.Parse(constanceInfo.value) : 0f);
		}

		public void InvokeOnBeforeLoadNewScene()
		{
			if (this.onBeforeLoadNewScene != null)
			{
				this.onBeforeLoadNewScene();
			}
		}

		public void LoadSceneSync(string sName)
		{
			preSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
			InvokeOnBeforeLoadNewScene();
			Singleton<AssetBundleManager>.instance.LoadLevelByName(sName);
			AssetBundleConfigManager.ABConfig aBConfig = SingletonScriptableObject<AssetBundleConfigManager>.instance.Get(preSceneName);
			if (aBConfig != null)
			{
				Singleton<AssetBundleManager>.instance.UnloadAssetBundle(aBConfig.abName, false);
			}
		}

		public void LoadSceneViaLoadingScene(string sceneName, UnityAction completeCallBack = null)
		{
			loadSceneNameViaLoadingScene = sceneName;
			LoadSceneSync(loadingSceneName);
			this.completeCallBack = completeCallBack;
		}
	}
}
