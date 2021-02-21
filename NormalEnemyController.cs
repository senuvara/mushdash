using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;

public class NormalEnemyController : BaseEnemyObjectController
{
	private void Start()
	{
		if (m_MusicData.noteData.type == 2 && !Singleton<BattleProperty>.instance.isGcCharacter)
		{
		}
	}

	public override void OnAttackDestory()
	{
		if (IsEmptyNode())
		{
			base.gameObject.SetActive(false);
		}
	}

	public override bool OnControllerMiss(int idx)
	{
		if (FeverManager.Instance.IsGod())
		{
			return false;
		}
		bool flag = base.OnControllerMiss(idx);
		if (flag)
		{
			SingletonMonoBehaviour<GirlManager>.instance.BeAttackEffect();
		}
		return flag;
	}
}
