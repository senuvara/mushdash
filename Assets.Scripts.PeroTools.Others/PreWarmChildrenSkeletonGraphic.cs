using Assets.Scripts.PeroTools.PreWarm;
using Spine.Unity;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Others
{
	public class PreWarmChildrenSkeletonGraphic : MonoBehaviour, IPreWarm
	{
		public void PreWarm(int slice)
		{
			if (slice == 2)
			{
				SkeletonGraphic[] componentsInChildren = GetComponentsInChildren<SkeletonGraphic>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].Initialize(true);
				}
			}
		}
	}
}
