using Rewired.Platforms;
using Rewired.Utils;
using Rewired.Utils.Interfaces;
using System.ComponentModel;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rewired
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public sealed class InputManager : InputManager_Base
	{
		protected override void OnInitialized()
		{
			SubscribeEvents();
		}

		protected override void OnDeinitialized()
		{
			UnsubscribeEvents();
		}

		protected override void DetectPlatform()
		{
			scriptingBackend = ScriptingBackend.Mono;
			scriptingAPILevel = ScriptingAPILevel.Net20;
			editorPlatform = EditorPlatform.None;
			platform = Platform.Unknown;
			webplayerPlatform = WebplayerPlatform.None;
			isEditor = false;
			string text = SystemInfo.deviceName ?? string.Empty;
			string text2 = SystemInfo.deviceModel ?? string.Empty;
			platform = Platform.Windows;
			scriptingBackend = ScriptingBackend.Mono;
			scriptingAPILevel = ScriptingAPILevel.Net20Subset;
		}

		protected override void CheckRecompile()
		{
		}

		protected override IExternalTools GetExternalTools()
		{
			return new ExternalTools();
		}

		private bool CheckDeviceName(string searchPattern, string deviceName, string deviceModel)
		{
			return Regex.IsMatch(deviceName, searchPattern, RegexOptions.IgnoreCase) || Regex.IsMatch(deviceModel, searchPattern, RegexOptions.IgnoreCase);
		}

		private void SubscribeEvents()
		{
			UnsubscribeEvents();
			SceneManager.sceneLoaded += OnSceneLoaded;
		}

		private void UnsubscribeEvents()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			OnSceneLoaded();
		}
	}
}
