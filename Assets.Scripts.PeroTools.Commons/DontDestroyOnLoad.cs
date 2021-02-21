using UnityEngine;

namespace Assets.Scripts.PeroTools.Commons
{
	public class DontDestroyOnLoad : MonoBehaviour
	{
		private void Awake()
		{
			Object.DontDestroyOnLoad(base.gameObject);
		}
	}
}
