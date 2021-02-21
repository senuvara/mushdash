using Rewired;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Commons
{
	public class HideCursor : MonoBehaviour
	{
		[InfoBox("If [Hard Time] <= 0, the cursor will only hide if joystick input detected and will show if the mouse moved.\nIf [Hard Time] > 0, the cursor will also hide if the mouse didn't move in the time.", InfoMessageType.Info, null)]
		public float hideTime;

		private float m_Time;

		private void Update()
		{
			if (Cursor.visible)
			{
				if (ReInput.controllers.GetLastActiveController().type == ControllerType.Joystick)
				{
					Cursor.visible = false;
				}
				if (!(hideTime > 0f))
				{
					return;
				}
				if (ReInput.controllers.Mouse.screenPositionDelta == Vector2.zero)
				{
					if (m_Time < hideTime)
					{
						m_Time += Time.deltaTime;
					}
					else
					{
						Cursor.visible = false;
					}
				}
				else
				{
					Cursor.visible = true;
					m_Time = 0f;
				}
			}
			else if (ReInput.controllers.Mouse.screenPositionDelta != Vector2.zero)
			{
				Cursor.visible = true;
				m_Time = 0f;
			}
		}
	}
}
