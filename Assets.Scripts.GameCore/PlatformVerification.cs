using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Platforms.Steam;
using PaperPlaneTools;
using Steamworks;
using System;
using UnityEngine;

namespace Assets.Scripts.GameCore
{
	public class PlatformVerification : MonoBehaviour
	{
		protected void Start()
		{
			try
			{
				if (SteamAPI.RestartAppIfNecessary(new AppId_t(774171u)))
				{
					Application.Quit();
					return;
				}
			}
			catch (DllNotFoundException arg)
			{
				Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + arg, this);
				Application.Quit();
				return;
			}
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				Singleton<SceneManager>.instance.LoadSceneSync("Welcome");
			}, new Func<bool>(null, __ldftn(SteamManager.get_Initialized)));
		}

		private void ShowGenuineCheckFail()
		{
			Alert alert = new Alert(null, "Genuine check failed!");
			alert.SetPositiveButton("Exit", Application.Quit);
			alert.AddOptions(new AlertAndroidOptions
			{
				Cancelable = false
			});
			alert.Show();
		}
	}
}
