using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.UI.Specials
{
	public class EnableDisableHooker : MonoBehaviour
	{
		public event UnityAction<GameObject> onEnable;

		public event UnityAction<GameObject> onDisable;

		private void OnEnable()
		{
			if (this.onEnable != null)
			{
				this.onEnable(base.gameObject);
			}
		}

		private void OnDisable()
		{
			if (this.onDisable != null)
			{
				this.onDisable(base.gameObject);
			}
		}
	}
}
