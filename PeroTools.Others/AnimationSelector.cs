using System;
using System.Linq;
using UnityEngine;

namespace PeroTools.Others
{
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class AnimationSelector : Attribute
	{
		public string animatorFieldName
		{
			get;
			private set;
		}

		public AnimationSelector()
		{
			animatorFieldName = null;
		}

		public AnimationSelector(string animFieldName)
		{
			animatorFieldName = animFieldName;
		}

		private static GUIContent[] StringsToGUIContents(string[] strs)
		{
			return strs.Select((string s) => new GUIContent(s)).ToArray();
		}
	}
}
