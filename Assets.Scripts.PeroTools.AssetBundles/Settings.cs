using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.AssetBundles
{
	public class Settings : ScriptableObject
	{
		public enum AssetBundleManagerMode
		{
			Simulation,
			Server
		}

		[SerializeField]
		private List<ServerSetting> m_ServerSettings;

		[SerializeField]
		private ServerSetting m_CurrentSetting;

		[SerializeField]
		private ServerSetting m_DevBuildSetting;

		[SerializeField]
		private ServerSetting m_ReleaseBuildSetting;

		[SerializeField]
		private AssetBundleManagerMode m_Mode;

		private static Settings m_Settings;

		public static ServerSetting currentSetting => ReleaseBuildSetting;

		public static ServerSetting DevelopmentBuildSetting => GetSettings().m_DevBuildSetting;

		public static ServerSetting ReleaseBuildSetting => GetSettings().m_ReleaseBuildSetting;

		public static AssetBundleManagerMode Mode => GetSettings().m_Mode;

		private static Settings GetSettings()
		{
			if (m_Settings == null && !Load())
			{
				m_Settings = ScriptableObject.CreateInstance<Settings>();
				m_Settings.m_ServerSettings = new List<ServerSetting>();
			}
			return m_Settings;
		}

		private static bool Load()
		{
			bool result = false;
			m_Settings = Resources.Load<Settings>("AssetBundleManagerSettings");
			if (m_Settings != null)
			{
				result = true;
			}
			return result;
		}
	}
}
