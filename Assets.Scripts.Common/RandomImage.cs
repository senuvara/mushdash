using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Common
{
	[RequireComponent(typeof(Image))]
	public class RandomImage : SerializedMonoBehaviour
	{
		public List<string> texNames;

		private void Start()
		{
			GetComponent<Image>().sprite = Singleton<AssetBundleManager>.instance.LoadFromName<Sprite>(texNames.Random());
		}
	}
}
