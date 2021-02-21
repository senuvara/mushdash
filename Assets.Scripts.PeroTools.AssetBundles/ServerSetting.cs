using Assets.Scripts.PeroTools.Commons;
using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.PeroTools.AssetBundles
{
	public class ServerSetting : ScriptableObject
	{
		[SerializeField]
		private ServerSettingType m_ServerType;

		[SerializeField]
		private string m_ServerUrl;

		[SerializeField]
		private string m_LocalServerPath;

		[SerializeField]
		private int m_GameID;

		[SerializeField]
		private string m_GameKey;

		[SerializeField]
		private string m_AppVersion;

		[SerializeField]
		private string m_ResourcesVersion;

		[SerializeField]
		private uint m_ChannelId;

		private string m_CacheStreamingAssetAbsPath;

		private string m_CacheLocalSavePath;

		private string m_CacheServerUrl;

		public string Name => base.name;

		public ServerSettingType serverType
		{
			get
			{
				return m_ServerType;
			}
			set
			{
				m_ServerType = value;
			}
		}

		public string localSavePath
		{
			get
			{
				if (string.IsNullOrEmpty(m_CacheLocalSavePath))
				{
					m_CacheLocalSavePath = Path.Combine(Application.persistentDataPath, "AssetBundles").Replace("\\", "/");
					Debug.Log("Save Path : " + m_CacheLocalSavePath);
				}
				return m_CacheLocalSavePath;
			}
		}

		public string serverUrl
		{
			get
			{
				if (string.IsNullOrEmpty(m_CacheServerUrl))
				{
					switch (serverType)
					{
					case ServerSettingType.StreamingAssets:
						m_CacheServerUrl = Path.Combine(AssetsUtils.streamingPathServer, "AssetBundles").Replace("\\", "/");
						break;
					case ServerSettingType.TencentGCloud:
						m_CacheServerUrl = m_ServerUrl;
						break;
					}
				}
				return m_CacheServerUrl;
			}
		}

		public string localServerPath => m_LocalServerPath;

		public string firstLoadAssetPath
		{
			get
			{
				ServerSettingType serverType = this.serverType;
				if (serverType == ServerSettingType.StreamingAssets)
				{
					return streamingAssetAbsPath;
				}
				throw new ArgumentOutOfRangeException();
			}
		}

		public string streamingAssetAbsPath
		{
			get
			{
				if (string.IsNullOrEmpty(m_CacheStreamingAssetAbsPath))
				{
					m_CacheStreamingAssetAbsPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles").Replace("\\", "/");
				}
				return m_CacheStreamingAssetAbsPath;
			}
		}

		public string manifestFileName => "AssetBundles";

		public int gameID => m_GameID;

		public string gameKey => m_GameKey;

		public string appVersion => m_AppVersion;

		public string resourceVersion => m_ResourcesVersion;

		public uint ChannelId => m_ChannelId;

		public static ServerSetting CreateServerSetting(string name, ServerSettingType t)
		{
			ServerSetting serverSetting = ScriptableObject.CreateInstance<ServerSetting>();
			serverSetting.name = name;
			serverSetting.m_ServerType = t;
			serverSetting.m_LocalServerPath = string.Empty;
			serverSetting.m_ServerUrl = string.Empty;
			return serverSetting;
		}
	}
}
