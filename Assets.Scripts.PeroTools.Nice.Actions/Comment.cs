using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class Comment : Action
	{
		[SerializeField]
		[Multiline(3)]
		[HideLabel]
		[GUIColor(0f, 1f, 0f, 1f)]
		private string m_Comment;
	}
}
