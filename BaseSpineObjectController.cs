using UnityEngine;

public abstract class BaseSpineObjectController : MonoBehaviour
{
	public int idx = -1;

	protected Renderer m_Renderer;

	protected SpriteRenderer m_CatchAir;

	protected SpriteRenderer m_CatchGround;

	public bool isIn
	{
		get;
		protected set;
	}

	public abstract void SetIdx(int idx);

	public abstract void Init();

	public abstract bool ControllerMissCheck(int idx, decimal currentTick);

	public abstract void OnControllerStart();

	public abstract bool OnControllerMiss(int idx);

	public abstract void OnControllerAttacked(int result, bool isDeaded);

	public int GetIdx()
	{
		return idx;
	}

	public void SetVisible(bool enable)
	{
		if ((bool)m_Renderer)
		{
			m_Renderer.enabled = enable;
			if ((bool)m_CatchAir)
			{
				m_CatchAir.enabled = enable;
			}
			if ((bool)m_CatchGround)
			{
				m_CatchGround.enabled = enable;
			}
		}
	}
}
