using System.Collections.Generic;

namespace Assets.Scripts.PeroTools.Managers
{
	public interface IControlable
	{
		void Init(string path);

		List<int> GetButtonDown(string buttonName);

		List<int> GetButtonUp(string buttonName);

		List<int> GetButton(string buttonName);

		void SwitchProposal(string proposalName);

		void CustomButton(string buttonName, string key);

		void SetVibration(InputManager.VibrationVaule vaule, InputManager.DeviceHandles handles, float time);

		void OnClickChangeButton(string buttonName, string key);

		void OnClickNoKeyToChange(string buttonName);

		void UpdateRepeatList(List<string> list);

		void OnDisableController();

		void SaveJsonProposal(string keyboardProposal, string handleProposal, bool isVibration);

		void ChangeButton(string buttonName, string newKey, string uiName);
	}
}
