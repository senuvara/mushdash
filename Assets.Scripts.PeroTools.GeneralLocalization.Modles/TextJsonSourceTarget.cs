using Assets.Scripts.PeroTools.Nice.Actions;
using Assets.Scripts.PeroTools.Nice.Events;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.GeneralLocalization.Modles
{
	[Serializable]
	public class TextJsonSourceTarget
	{
		[Required]
		public Assets.Scripts.PeroTools.Nice.Events.Event @event;

		[Tooltip("如果Event下有多个SetText，则index用于表示第几个SetText。从0开始\n因为SetTexe这样的Action无法拖动到其他组件中，所以暂时用这样的办法做支持，等以后Action可以拖动到其他组件之后，再修改。")]
		public int index;

		public SetText GetSetText(out int indexInPlayables)
		{
			return GetSetText(@event, index, out indexInPlayables);
		}

		public static SetText GetSetText(Assets.Scripts.PeroTools.Nice.Events.Event @event, int index, out int indexInPlayables)
		{
			indexInPlayables = -1;
			if (@event == null)
			{
				return null;
			}
			List<SetText> playables = @event.GetPlayables<SetText>();
			if (playables != null && playables.Count > 0)
			{
				index = Mathf.Clamp(index, 0, playables.Count - 1);
				SetText setText = playables[index];
				indexInPlayables = @event.playables.IndexOf(setText);
				return setText;
			}
			return null;
		}
	}
}
