using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace UnityEngine.AssetGraph
{
	public class AssetBundleBuildMap : ScriptableObject
	{
		[Serializable]
		public class AssetBundleEntry
		{
			[Serializable]
			internal struct AssetPathString
			{
				[SerializeField]
				public string original;

				[SerializeField]
				public string lower;

				internal AssetPathString(string s)
				{
					original = s;
					if (!string.IsNullOrEmpty(s))
					{
						lower = s.ToLower();
					}
					else
					{
						lower = s;
					}
				}
			}

			[SerializeField]
			internal string m_assetBundleName;

			[SerializeField]
			internal string m_assetBundleVariantName;

			[SerializeField]
			internal string m_fullName;

			[SerializeField]
			internal List<AssetPathString> m_assets;

			[SerializeField]
			public string m_registererId;

			public string Name => m_assetBundleName;

			public string Variant => m_assetBundleVariantName;

			public string FullName => m_fullName;

			public AssetBundleEntry(string registererId, string assetBundleName, string variantName)
			{
				m_registererId = registererId;
				m_assetBundleName = assetBundleName.ToLower();
				m_assetBundleVariantName = variantName.ToLower();
				m_fullName = MakeFullName(assetBundleName, variantName);
				m_assets = new List<AssetPathString>();
			}

			public void Clear()
			{
				m_assets.Clear();
				SetMapDirty();
			}

			public void AddAssets(string id, IEnumerable<string> assets)
			{
				foreach (string asset in assets)
				{
					m_assets.Add(new AssetPathString(asset));
				}
				SetMapDirty();
			}

			public IEnumerable<string> GetAssetFromAssetName(string assetName)
			{
				assetName = assetName.ToLower();
				return from a in m_assets
					where Path.GetFileNameWithoutExtension(a.lower) == assetName
					select a into s
					select s.original;
			}
		}

		[SerializeField]
		private List<AssetBundleEntry> m_assetBundles;

		private static AssetBundleBuildMap s_map;

		public static AssetBundleBuildMap GetBuildMap()
		{
			if (s_map == null && !Load())
			{
				s_map = ScriptableObject.CreateInstance<AssetBundleBuildMap>();
				s_map.m_assetBundles = new List<AssetBundleEntry>();
			}
			return s_map;
		}

		private static bool Load()
		{
			return false;
		}

		public static void SetMapDirty()
		{
		}

		internal static string MakeFullName(string assetBundleName, string variantName)
		{
			if (string.IsNullOrEmpty(assetBundleName))
			{
				return string.Empty;
			}
			if (string.IsNullOrEmpty(variantName))
			{
				return assetBundleName.ToLower();
			}
			return $"{assetBundleName.ToLower()}.{variantName.ToLower()}";
		}

		internal static string[] FullNameToNameAndVariant(string assetBundleFullName)
		{
			assetBundleFullName = assetBundleFullName.ToLower();
			int num = assetBundleFullName.LastIndexOf('.');
			if (num > 0 && num + 1 < assetBundleFullName.Length)
			{
				string text = assetBundleFullName.Substring(0, num);
				string text2 = assetBundleFullName.Substring(num + 1);
				return new string[2]
				{
					text,
					text2
				};
			}
			return new string[2]
			{
				assetBundleFullName,
				string.Empty
			};
		}

		public AssetBundleEntry GetAssetBundle(string registererId, string assetBundleFullName)
		{
			AssetBundleEntry assetBundleEntry = m_assetBundles.Find((AssetBundleEntry v) => v.m_fullName == assetBundleFullName);
			if (assetBundleEntry == null)
			{
				string[] array = FullNameToNameAndVariant(assetBundleFullName);
				assetBundleEntry = new AssetBundleEntry(registererId, array[0], array[1]);
				m_assetBundles.Add(assetBundleEntry);
				SetMapDirty();
			}
			return assetBundleEntry;
		}

		public void Clear()
		{
			m_assetBundles.Clear();
			SetMapDirty();
		}

		public void ClearFromId(string id)
		{
			m_assetBundles.RemoveAll((AssetBundleEntry e) => e.m_registererId == id);
		}

		public AssetBundleEntry GetAssetBundleWithNameAndVariant(string registererId, string assetBundleName, string variantName)
		{
			return GetAssetBundle(registererId, MakeFullName(assetBundleName, variantName));
		}

		public string[] GetAssetPathsFromAssetBundleAndAssetName(string assetbundleName, string assetName)
		{
			assetName = assetName.ToLower();
			return m_assetBundles.Where((AssetBundleEntry ab) => ab.m_fullName == assetbundleName).SelectMany((AssetBundleEntry ab) => ab.GetAssetFromAssetName(assetName)).ToArray();
		}

		public string[] GetAssetPathsFromAssetBundle(string assetBundleName)
		{
			assetBundleName = assetBundleName.ToLower();
			return (from s in m_assetBundles.Where((AssetBundleEntry e) => e.m_fullName == assetBundleName).SelectMany((AssetBundleEntry e) => e.m_assets)
				select s.original).ToArray();
		}

		public string GetAssetBundleName(string assetPath)
		{
			AssetBundleEntry assetBundleEntry = m_assetBundles.Find((AssetBundleEntry e) => e.m_assets.Contains(new AssetBundleEntry.AssetPathString(assetPath)));
			if (assetBundleEntry != null)
			{
				return assetBundleEntry.m_fullName;
			}
			return string.Empty;
		}

		public string GetImplicitAssetBundleName(string assetPath)
		{
			return GetAssetBundleName(assetPath);
		}

		public string[] GetAllAssetBundleNames()
		{
			return m_assetBundles.Select((AssetBundleEntry e) => e.m_fullName).ToArray();
		}
	}
}
