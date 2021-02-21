using Assets.Scripts.PeroTools.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Managers
{
	public class AssetBundleConfigManager : SingletonScriptableObject<AssetBundleConfigManager>
	{
		public class DefaultAsset
		{
		}

		[Serializable]
		public class ABConfig
		{
			public const string head = "Assets/Static Resources/";

			public string abName;

			public string directory;

			[SerializeField]
			private string m_Extension;

			[SerializeField]
			private ushort m_TypeIndex;

			public Tag tag;

			private string m_FullAssetPah;

			private string m_Directory;

			public string fileName
			{
				get;
				set;
			}

			public string extension
			{
				get
				{
					return (tag == Tag.Folder) ? string.Empty : $".{m_Extension}";
				}
				set
				{
					m_Extension = ((!string.IsNullOrEmpty(value)) ? value.Substring(1) : string.Empty);
				}
			}

			public Type type
			{
				get
				{
					return Type.GetType(SingletonScriptableObject<AssetBundleConfigManager>.instance.types[m_TypeIndex]);
				}
				set
				{
					List<string> types = SingletonScriptableObject<AssetBundleConfigManager>.instance.types;
					int num = types.IndexOf(value.AssemblyQualifiedName);
					if (num >= 0)
					{
						m_TypeIndex = (ushort)num;
						return;
					}
					m_TypeIndex = (ushort)types.Count;
					types.Add(value.AssemblyQualifiedName);
					if (types.Count > 65535)
					{
						Debug.LogError("Out of ushort range.");
					}
				}
			}

			public string GetDirectory()
			{
				if (m_Directory == null)
				{
					m_Directory = string.Format("{0}{1}", "Assets/Static Resources/", directory);
				}
				return m_Directory;
			}

			public string GetRelAssetPathWithoutExtension()
			{
				return Path.Combine(directory, fileName).Replace("\\", "/");
			}

			public string GetFullAssetPath()
			{
				if (string.IsNullOrEmpty(m_FullAssetPah))
				{
					m_FullAssetPah = Path.Combine(GetDirectory(), $"{fileName}{extension}").Replace("\\", "/");
				}
				return m_FullAssetPah;
			}

			public string GetFullAssetPathWithoutExtension()
			{
				return Path.Combine(GetDirectory(), fileName).Replace("\\", "/");
			}

			public void Init(string key)
			{
				if (string.IsNullOrEmpty(fileName))
				{
					fileName = Path.GetFileNameWithoutExtension(key);
					m_FullAssetPah = null;
				}
			}

			public override string ToString()
			{
				return $"ABName : {abName}\nFullName : {GetFullAssetPath()}\nType : {type.ToString()}\nTag : {tag}";
			}
		}

		public enum Tag : short
		{
			None,
			JsonConfig,
			Folder
		}

		[SerializeField]
		public List<string> types = new List<string>();

		[SerializeField]
		[HideInInspector]
		public Dictionary<string, List<ABConfig>> dict = new Dictionary<string, List<ABConfig>>();

		public List<ABConfig> GetList(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return null;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(key);
			List<ABConfig> value;
			if (!dict.TryGetValue(fileNameWithoutExtension, out value))
			{
				Debug.LogWarningFormat("Can no find asset bundle config of name : {0}", fileNameWithoutExtension);
				return null;
			}
			for (int i = 0; i < value.Count; i++)
			{
				value[i].Init(key);
			}
			return value;
		}

		public bool Contains(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return false;
			}
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(key);
			List<ABConfig> value;
			if (!dict.TryGetValue(fileNameWithoutExtension, out value))
			{
				return false;
			}
			return true;
		}

		public ABConfig Get(string assetPath, Type type)
		{
			if (string.IsNullOrEmpty(assetPath))
			{
				return null;
			}
			string fileName = Path.GetFileNameWithoutExtension(assetPath);
			string extension = Path.GetExtension(assetPath);
			string fullName = (assetPath.Length <= fileName.Length + extension.Length) ? null : assetPath;
			List<ABConfig> value;
			if (!dict.TryGetValue(fileName, out value))
			{
				return null;
			}
			ABConfig result = null;
			if (value.Count <= 0)
			{
				return result;
			}
			if (value.Count <= 1)
			{
				value[0].Init(fileName);
				if (Judge(value[0], type, extension, fullName))
				{
					result = value[0];
				}
			}
			else
			{
				result = value.Find(delegate(ABConfig config)
				{
					config.Init(fileName);
					return Judge(config, type, extension, fullName);
				});
			}
			return result;
		}

		public ABConfig Get<T>(string assetPath)
		{
			return Get(assetPath, typeof(T));
		}

		public ABConfig Get(string assetPath)
		{
			return Get(assetPath, typeof(UnityEngine.Object));
		}

		private bool Judge(ABConfig abconfig, Type type, string extension, string fullName)
		{
			if (abconfig.type.IsSubclassOf(type) || abconfig.type == type)
			{
				if (!string.IsNullOrEmpty(fullName))
				{
					if (string.IsNullOrEmpty(extension))
					{
						if (string.Equals(abconfig.GetFullAssetPathWithoutExtension(), fullName))
						{
							return true;
						}
						return false;
					}
					if (string.Equals(abconfig.GetFullAssetPath(), fullName))
					{
						return true;
					}
					return false;
				}
				if (string.IsNullOrEmpty(extension))
				{
					return true;
				}
				if (string.Equals(abconfig.extension, extension))
				{
					return true;
				}
				return false;
			}
			return false;
		}

		public List<string> GetAssetPathsInFolder(string folderPath)
		{
			ABConfig aBConfig = GetList(folderPath)?.Find((ABConfig config) => config.tag == Tag.Folder && config.GetFullAssetPath() == folderPath);
			List<string> list = new List<string>();
			if (aBConfig != null)
			{
				List<string> array = from config in GetAllABConfig((ABConfig config) => config.GetDirectory() == folderPath)
					select config.GetFullAssetPath();
				array.For(delegate(string r)
				{
					if (!list.Contains(r))
					{
						list.Add(r);
					}
				});
				return list;
			}
			return null;
		}

		public List<ABConfig> GetAllABConfig(Predicate<ABConfig> condition)
		{
			List<ABConfig> list = new List<ABConfig>();
			foreach (KeyValuePair<string, List<ABConfig>> item in dict)
			{
				List<ABConfig> value = item.Value;
				foreach (ABConfig item2 in value)
				{
					item2.Init(item.Key);
					if (condition(item2))
					{
						list.Add(item2);
					}
				}
			}
			return list;
		}
	}
}
