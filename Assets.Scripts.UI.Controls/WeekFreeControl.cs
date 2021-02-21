using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.Controls
{
	public class WeekFreeControl : MonoBehaviour
	{
		private void OnEnable()
		{
			int num = Singleton<ConfigManager>.instance["albums"].Count - 2;
			int[] freeAlbumIndexs = Singleton<WeekFreeManager>.instance.freeAlbumIndexs;
			Dictionary<int, Transform> dictionary = new Dictionary<int, Transform>();
			for (int i = 1; i < num + 1; i++)
			{
				Transform value = base.transform.Find($"ImgAlbum{i}");
				int key = num - i + 2;
				dictionary.Add(key, value);
			}
			dictionary = dictionary.OrderByDescending((KeyValuePair<int, Transform> d) => d.Key).ToDictionary((KeyValuePair<int, Transform> d) => d.Key, (KeyValuePair<int, Transform> d) => d.Value);
			foreach (KeyValuePair<int, Transform> item in dictionary)
			{
				item.Value.SetSiblingIndex(item.Key);
			}
			if (freeAlbumIndexs != null && freeAlbumIndexs.Length > 0)
			{
				for (int j = 0; j < freeAlbumIndexs.Length; j++)
				{
					int num2 = freeAlbumIndexs[j];
					Transform transform = base.transform.Find($"ImgAlbum{num2}");
					transform.SetSiblingIndex(j + 2);
				}
			}
		}
	}
}
