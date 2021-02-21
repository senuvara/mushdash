using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	[HideReferenceObjectPicker]
	public abstract class Action : IPlayable
	{
		public virtual float duration => 0f;

		public virtual void Enter()
		{
		}

		public virtual void Execute()
		{
		}

		public virtual void Exit()
		{
		}

		public virtual void Pause()
		{
		}

		public virtual void Resume()
		{
		}

		public virtual void OnGUISelected()
		{
		}
	}
}
