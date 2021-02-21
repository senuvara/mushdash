using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using GameLogic;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameCore.Managers
{
	public class NoteDataMananger : SingletonScriptableObject<NoteDataMananger>
	{
		public List<NoteConfigData> noteDatas = new List<NoteConfigData>();

		public List<GameObject> notePrefabs;

		public void PreloadNotePrefabs()
		{
			foreach (NoteConfigData noteData in noteDatas)
			{
				Singleton<AssetBundleManager>.instance.LoadFromNameAsyn(noteData.prefab_name, delegate(GameObject g)
				{
					notePrefabs.Add(g);
				});
			}
		}
	}
}
