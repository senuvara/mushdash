using GameLogic;
using UnityEngine;

public class NodeInitController : MonoBehaviour
{
	private int idx = -1;

	private decimal countdown = -1m;

	private SpineActionController m_Sac;

	private MeshRenderer m_MeshRenderer;

	private Animator m_Animator;

	public void InitWithoutRun(int index)
	{
		if (index >= 0 && idx < 0)
		{
			idx = index;
			base.enabled = true;
			m_Sac = GetComponent<SpineActionController>();
			m_MeshRenderer = GetComponent<MeshRenderer>();
			if ((bool)m_MeshRenderer)
			{
				m_MeshRenderer.enabled = false;
			}
			m_Animator = GetComponent<Animator>();
			if ((bool)m_Animator)
			{
				m_Animator.enabled = false;
			}
		}
	}

	public void Run()
	{
		if ((bool)m_Animator)
		{
			m_Animator.enabled = true;
		}
		if ((bool)m_Sac)
		{
			m_Sac.OnControllerStart();
		}
		GameGlobal.gGameMusicScene.OnObjRun(idx);
		countdown = -1m;
	}

	public void AddCoundDown(decimal value)
	{
		countdown += value;
	}
}
