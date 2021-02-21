using Assets.Scripts.PeroTools.Commons;
using UnityEngine;

public class LockRotate : Singleton<LockRotate>
{
	private ScreenOrientation m_PlayerRotate;

	public void BattleStartLockRotate()
	{
		m_PlayerRotate = Screen.orientation;
		if (m_PlayerRotate == ScreenOrientation.LandscapeLeft)
		{
			Screen.autorotateToLandscapeRight = false;
		}
		else if (m_PlayerRotate == ScreenOrientation.LandscapeRight)
		{
			Screen.autorotateToLandscapeLeft = false;
		}
	}

	public void BattleStartUnlockRotate()
	{
		Screen.orientation = m_PlayerRotate;
		Screen.orientation = ScreenOrientation.AutoRotation;
		Screen.autorotateToLandscapeLeft = true;
		Screen.autorotateToLandscapeRight = true;
	}
}
