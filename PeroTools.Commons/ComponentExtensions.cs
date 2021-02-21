using System.Collections.Generic;
using UnityEngine;

namespace PeroTools.Commons
{
	public static class ComponentExtensions
	{
		public static T[] GetComponentsInDirectChildren<T>(this Component gameObject, bool includeInactive = false) where T : Component
		{
			List<T> list = new List<T>();
			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				T component = gameObject.transform.GetChild(i).GetComponent<T>();
				if ((Object)component != (Object)null && (component.gameObject.activeSelf || includeInactive))
				{
					list.Add(component);
				}
			}
			return list.ToArray();
		}
	}
}
