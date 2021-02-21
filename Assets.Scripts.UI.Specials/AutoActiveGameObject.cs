using UnityEngine;

namespace Assets.Scripts.UI.Specials
{
	public class AutoActiveGameObject : MonoBehaviour
	{
		public GameObject obj;

		private void OnEnable()
		{
			if (obj != null)
			{
				obj.SetActive(true);
			}
		}

		private void OnDisable()
		{
			if (obj != null)
			{
				obj.SetActive(false);
			}
		}
	}
}
