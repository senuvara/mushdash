using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.PreProcessSystem;
using PeroTools.Commons;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.PeroTools.Others
{
	public class LoadingScene : MonoBehaviour
	{
		private float m_LoadingTime;

		private IEnumerator Start()
		{
			Object.DontDestroyOnLoad(base.gameObject);
			Stopwatch watch = new Stopwatch();
			watch.Start();
			UnityEngine.Debug.Log("Load Scene Start at " + (float)watch.ElapsedMilliseconds * 0.001f);
			LoadSceneMode mode = LoadSceneMode.Single;
			Singleton<Assets.Scripts.PeroTools.Managers.SceneManager>.instance.InvokeOnBeforeLoadNewScene();
			AssetBundleLoadLevelOperation loadLevelOpr = Singleton<AssetBundleManager>.instance.LoadLevelAsyncByName(Singleton<Assets.Scripts.PeroTools.Managers.SceneManager>.instance.loadSceneNameViaLoadingScene, mode);
			yield return loadLevelOpr;
			AsyncOperation loadReq = loadLevelOpr.GetLoadRequest();
			loadReq.allowSceneActivation = false;
			yield return new WaitUntil(() => loadReq.progress >= 0.9f);
			yield return new WaitUntil(() => m_LoadingTime > Singleton<Assets.Scripts.PeroTools.Managers.SceneManager>.instance.leastLoadingTime);
			UnityEngine.Debug.Log("Load Scene Complete at " + (float)watch.ElapsedMilliseconds * 0.001f);
			loadReq.allowSceneActivation = true;
			UnityEngine.Debug.Log("Activate Scene at " + (float)watch.ElapsedMilliseconds * 0.001f);
			yield return loadReq.isDone;
			yield return new WaitUntil(() => loadReq.progress >= 1f);
			if (PreWarmSystem.current == null)
			{
				Complete();
				yield break;
			}
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(Complete, () => PreWarmSystem.current.prewarmCompleted);
		}

		private void Complete()
		{
			if (Singleton<Assets.Scripts.PeroTools.Managers.SceneManager>.instance.completeCallBack != null)
			{
				Singleton<Assets.Scripts.PeroTools.Managers.SceneManager>.instance.completeCallBack();
			}
			AssetBundleConfigManager.ABConfig aBConfig = SingletonScriptableObject<AssetBundleConfigManager>.instance.Get(Singleton<Assets.Scripts.PeroTools.Managers.SceneManager>.instance.loadingSceneName);
			if (aBConfig != null)
			{
				Singleton<AssetBundleManager>.instance.UnloadAssetBundle(aBConfig.abName, false);
			}
			Resources.UnloadUnusedAssets();
			GcControl.Collect();
			DelayDestroy();
		}

		private void DelayDestroy()
		{
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				Object.DestroyImmediate(base.gameObject);
			}, 4);
		}

		private void Update()
		{
			m_LoadingTime += Time.unscaledDeltaTime;
		}
	}
}
