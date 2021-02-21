using Assets.Scripts.GameCore.Controller;
using Assets.Scripts.PeroTools.Managers;
using UnityEngine;

public static class ControllerFactory
{
	public static IControlable GetController(out string config, RuntimePlatform platform)
	{
		switch (platform)
		{
		case RuntimePlatform.OSXEditor:
		case RuntimePlatform.OSXPlayer:
		case RuntimePlatform.WindowsPlayer:
			config = "InputStandlone";
			return new StandloneController();
		case RuntimePlatform.WindowsEditor:
			config = "InputStandlone";
			return new StandloneController();
		case RuntimePlatform.LinuxPlayer:
		case RuntimePlatform.LinuxEditor:
			config = "InputStandlone";
			return new StandloneController();
		default:
			Debug.LogWarningFormat("UnSupport Controller for platform [{0}].", platform);
			config = string.Empty;
			return null;
		}
	}
}
