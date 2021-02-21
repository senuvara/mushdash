using Rewired;

namespace Assets.Scripts.PeroTools.Commons
{
	public static class ControllerUtils
	{
		private static void CheckJoysticks(out int index)
		{
			int num = 0;
			if (ReInput.controllers.GetJoystick(num) == null)
			{
				for (int i = 0; i < ReInput.controllers.Joysticks.Count && ReInput.controllers.GetJoystick(i) == null; i++)
				{
					num++;
				}
			}
			index = num;
		}

		public static bool IsPS4Controller()
		{
			if (ReInput.controllers.Joysticks.Count == 0)
			{
				return false;
			}
			int index = 0;
			CheckJoysticks(out index);
			if (ReInput.controllers.GetJoystick(index) == null)
			{
				return false;
			}
			return ReInput.controllers.GetJoystick(index).name.StartsWith("Sony DualShock");
		}

		public static bool IsXBoxController()
		{
			if (ReInput.controllers.Joysticks.Count == 0)
			{
				return false;
			}
			int index = 0;
			CheckJoysticks(out index);
			if (ReInput.controllers.GetJoystick(index) == null)
			{
				return false;
			}
			if (!ReInput.controllers.GetJoystick(index).name.StartsWith("Sony DualShock"))
			{
				return true;
			}
			return false;
		}

		public static bool IsNSController()
		{
			if (ReInput.controllers.Joysticks.Count == 0)
			{
				return false;
			}
			int index = 0;
			CheckJoysticks(out index);
			if (ReInput.controllers.GetJoystick(index) == null)
			{
				return false;
			}
			if ((!ReInput.controllers.GetJoystick(index).name.StartsWith("Sony DualShock") && ReInput.controllers.GetJoystick(index).name.StartsWith("Pro Controller")) || (!ReInput.controllers.GetJoystick(index).name.StartsWith("Sony DualShock") && ReInput.controllers.GetJoystick(index).name.StartsWith("Nintendo Switch")))
			{
				return true;
			}
			return false;
		}

		public static bool IsCustomController()
		{
			if (ReInput.controllers.Joysticks.Count == 0)
			{
				return false;
			}
			int index = 0;
			CheckJoysticks(out index);
			if (ReInput.controllers.GetJoystick(index) == null)
			{
				return false;
			}
			if (!ReInput.controllers.GetJoystick(index).name.StartsWith("Sony DualShock") && ReInput.controllers.GetJoystick(index).name.StartsWith("YuanCon"))
			{
				return true;
			}
			return false;
		}

		public static bool IsKeyboardController()
		{
			if (ReInput.controllers.Joysticks.Count == 0)
			{
				return true;
			}
			return false;
		}

		public static string CurrentInputController()
		{
			if (ReInput.controllers.polling.PollAllControllersForFirstButtonDown().controllerName != string.Empty)
			{
				return ReInput.controllers.polling.PollAllControllersForFirstButtonDown().controllerName;
			}
			return string.Empty;
		}
	}
}
