using Assets.Scripts.PeroTools.Commons;
using UnityEngine;

namespace Assets.Scripts.Common.XDSDK
{
	public class XDSDKHandler : MonoBehaviour
	{
		private void OnLoginSuccess(string sid)
		{
			Debug.Log($"[XDSDK(OS)]: Login success with sid: {sid}");
			Singleton<XDSDKManager>.instance.OnOSLoginSuccess(sid);
		}
	}
}
