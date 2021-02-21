namespace Assets.Scripts.GameCore.Skill
{
	public interface ISkill
	{
		string uid
		{
			get;
		}

		void Apply();
	}
}
