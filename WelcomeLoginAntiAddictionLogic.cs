using Assets.Scripts.Common.XDSDK;
using Assets.Scripts.PeroTools.Commons;
using System;
using UnityEngine;
using UnityEngine.UI;

public class WelcomeLoginAntiAddictionLogic : MonoBehaviour
{
	public Button[] btns;

	private Action<string> onLoginSucceed;

	private void Start()
	{
		GetComponent<Button>().interactable = false;
		OnDestroy();
		onLoginSucceed = delegate
		{
			OnDestroy();
		};
	}

	public void LoginDetection()
	{
		if (Singleton<XDSDKManager>.instance.IsLoggedIn() && Singleton<XDSDKManager>.instance.IsXDLoggedIn())
		{
			OnDestroy();
			return;
		}
		Singleton<XDSDKManager>.instance.Login();
		XDSDKManager.XDSDKHandler.onLoginSucceed -= onLoginSucceed;
		XDSDKManager.XDSDKHandler.onLoginSucceed += onLoginSucceed;
	}

	private void OnDestroy()
	{
		for (int i = 0; i < btns.Length; i++)
		{
			btns[i].interactable = true;
		}
		XDSDKManager.XDSDKHandler.onLoginSucceed -= onLoginSucceed;
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
