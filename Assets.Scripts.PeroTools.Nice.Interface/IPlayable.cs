namespace Assets.Scripts.PeroTools.Nice.Interface
{
	public interface IPlayable
	{
		float duration
		{
			get;
		}

		void Enter();

		void Execute();

		void Exit();

		void Pause();

		void Resume();
	}
}
