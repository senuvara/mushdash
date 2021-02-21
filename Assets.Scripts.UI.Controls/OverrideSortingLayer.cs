using Assets.Scripts.PeroTools.Commons;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Controls
{
	public class OverrideSortingLayer : SerializedMonoBehaviour
	{
		[CustomValueDrawer("OnLayerNameDraw")]
		public string layerName;

		public int sortingOrder;

		private void Start()
		{
			List<Renderer> array = GameUtils.FindObjectsOfType<Renderer>(base.transform);
			array.For(delegate(Renderer r)
			{
				r.sortingLayerName = layerName;
				r.sortingOrder = sortingOrder;
			});
		}
	}
}
