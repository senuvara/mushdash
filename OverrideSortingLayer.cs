using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Actions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class OverrideSortingLayer : Action
{
	public Transform target;

	[CustomValueDrawer("OnLayerNameDraw")]
	public string layerName;

	public int sortingOrder;

	public override void Execute()
	{
		if (!(target == null))
		{
			List<Renderer> array = GameUtils.FindObjectsOfType<Renderer>(target);
			array.For(delegate(Renderer r)
			{
				r.sortingLayerName = layerName;
				r.sortingOrder = sortingOrder;
			});
		}
	}
}
