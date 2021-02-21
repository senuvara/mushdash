using Assets.Scripts.PeroTools.PreWarm;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.PeroTools.PreProcessSystem
{
	public class PreWarmSystem : SerializedMonoBehaviour
	{
		public const int sliceCount = 3;

		public static PreWarmSystem current;

		public IPreWarm[] iPreWarms;

		[NonSerialized]
		[HideInInspector]
		public bool prewarmCompleted;

		public void Awake()
		{
			if (current != null && current != this)
			{
				Debug.LogError("There is more than one PreWarmSystem in the scene.");
			}
			current = this;
			StartCoroutine(Execute());
		}

		private IEnumerator Execute()
		{
			prewarmCompleted = false;
			DisableGameObjects();
			yield return CallPreWarms();
			EnableAllDisabledGameObjects();
			prewarmCompleted = true;
		}

		public void DisableGameObjects()
		{
			for (int i = 0; i < iPreWarms.Length; i++)
			{
				Behaviour behaviour = (Behaviour)iPreWarms[i];
				if (behaviour != null)
				{
					behaviour.enabled = false;
				}
			}
		}

		public void EnableAllDisabledGameObjects()
		{
			for (int i = 0; i < iPreWarms.Length; i++)
			{
				Behaviour behaviour = (Behaviour)iPreWarms[i];
				if (behaviour != null)
				{
					behaviour.enabled = true;
				}
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}

		public IEnumerator CallPreWarms()
		{
			for (int slice = 0; slice < 3; slice++)
			{
				Debug.LogFormat("[PreWarmSystem] Start Execute slice [{0}]", slice);
				for (int i = 0; i < iPreWarms.Length; i++)
				{
					iPreWarms[i].PreWarm(slice);
				}
				yield return new WaitForSecondsRealtime(0.2f);
			}
		}
	}
}
