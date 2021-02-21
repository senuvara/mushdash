using UnityEngine;

namespace ChristopherCreates.AndroidNativeAudio
{
	public class ANAMusicBackgroundPause : MonoBehaviour
	{
		private void OnApplicationPause(bool isPaused)
		{
			ANAMusic.OnApplicationPause(isPaused);
		}
	}
}
