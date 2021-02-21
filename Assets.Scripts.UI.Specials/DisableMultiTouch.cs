using UnityEngine;

namespace Assets.Scripts.UI.Specials
{
	public class DisableMultiTouch : MonoBehaviour
	{
		private void Awake()
		{
			Input.multiTouchEnabled = false;
		}

		private void OnDestroy()
		{
			Input.multiTouchEnabled = true;
		}
	}
}
