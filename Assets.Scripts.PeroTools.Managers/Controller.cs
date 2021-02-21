using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using System.Collections.Generic;

namespace Assets.Scripts.PeroTools.Managers
{
	public abstract class Controller<T> : IControlable where T : ControllerConfig
	{
		public T configs;

		public void Init(string path)
		{
			LoadAssets(path);
			OnInit();
		}

		public virtual void LoadAssets(string fileName)
		{
			configs = Singleton<AssetBundleManager>.instance.LoadFromName<T>(fileName);
		}

		public virtual void OnInit()
		{
		}

		public virtual void OnDisableController()
		{
		}

		public abstract List<int> GetButtonDown(string buttonName);

		public abstract List<int> GetButtonUp(string buttonName);

		public abstract List<int> GetButton(string buttonName);

		public virtual void SwitchProposal(string proposalName)
		{
		}

		public virtual void CustomButton(string buttonName, string key)
		{
		}

		public virtual void SetVibration(InputManager.VibrationVaule vaule, InputManager.DeviceHandles handles, float time)
		{
		}

		public virtual void OnClickChangeButton(string buttonName, string key)
		{
		}

		public virtual void OnClickNoKeyToChange(string buttonName)
		{
		}

		public virtual void UpdateRepeatList(List<string> list)
		{
		}

		public virtual void SaveJsonProposal(string keyboardProposal, string handleProposal, bool isVibration)
		{
		}

		public virtual void ChangeButton(string buttonName, string newKey, string uiName)
		{
		}
	}
}
