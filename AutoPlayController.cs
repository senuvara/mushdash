using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Spine.Unity;

public class AutoPlayController : BaseEnemyObjectController
{
	private static SkeletonDataAsset m_Rock;

	private static SkeletonDataAsset m_Sleepy;

	private static bool m_IsInstance;

	private void Awake()
	{
	}

	public override void OnControllerStart()
	{
		base.OnControllerStart();
		if (m_MusicData.noteData.type == 10)
		{
			Singleton<BattleProperty>.instance.isAutoPlay = true;
		}
		if (m_MusicData.noteData.type == 11)
		{
			Singleton<BattleProperty>.instance.isAutoPlay = false;
		}
	}
}
