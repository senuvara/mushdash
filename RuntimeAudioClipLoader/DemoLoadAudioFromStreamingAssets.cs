using System.Collections;
using System.IO;
using UnityEngine;

namespace RuntimeAudioClipLoader
{
	[RequireComponent(typeof(AudioSource))]
	public class DemoLoadAudioFromStreamingAssets : MonoBehaviour
	{
		public bool loadInBackground = true;

		public bool doStream;

		public string urlToLoad = "http://media.soundcloud.com/stream/dP7wRLjm32Eh.mp3";

		private AudioSource audioSource;

		private string sourceFolder => Path.Combine(Application.streamingAssetsPath, "RuntimeAudioClipLoader demo StreamingAssets");

		private void Start()
		{
			audioSource = GetComponent<AudioSource>();
		}

		private void OnGUI()
		{
			int num = 0;
			string[] files = Directory.GetFiles(sourceFolder, "*", SearchOption.AllDirectories);
			string[] array = files;
			foreach (string text in array)
			{
				if (!text.EndsWith(".meta"))
				{
					if (num > 10)
					{
						break;
					}
					num++;
					if (GUILayout.Button("Load: " + text.Substring(sourceFolder.Length)))
					{
						audioSource.clip = Manager.Load(text, doStream, loadInBackground);
					}
				}
			}
			GUILayout.Space(20f);
			GUILayout.Label("Url to load:");
			urlToLoad = GUILayout.TextArea(urlToLoad);
			if (GUILayout.Button("Load from url"))
			{
				StartCoroutine(DownloadClipFromUrl(urlToLoad));
			}
			GUILayout.Space(30f);
			if ((bool)audioSource.clip)
			{
				GUILayout.Label("AudioClip.name " + audioSource.clip.name);
				GUILayout.Label("AudioDataLoadState: " + Manager.GetAudioClipLoadState(audioSource.clip));
				GUILayout.Label("AudioClipLoadType: " + Manager.GetAudioClipLoadType(audioSource.clip));
				GUILayout.Label("time: " + audioSource.time + "/" + audioSource.clip.length);
				if (audioSource.isPlaying)
				{
					if (GUILayout.Button("Stop"))
					{
						audioSource.Stop();
					}
				}
				else if (GUILayout.Button("Play"))
				{
					audioSource.Play();
				}
			}
			doStream = GUILayout.Toggle(doStream, "doStream (audioClip is loaded on the fly on demand, use for long one time use clips)");
			loadInBackground = GUILayout.Toggle(loadInBackground, "loadInBackground (if !doStream and loadInBackground then loading is done in own thread so it doesnt hang up caller's thread)");
			if (GUILayout.Button("Clear cache (so you can test loading times again)"))
			{
				Manager.ClearCache();
				audioSource.clip = null;
			}
		}

		private IEnumerator DownloadClipFromUrl(string url)
		{
			WWW www = new WWW(url);
			yield return www;
			MemoryStream stream = new MemoryStream(www.bytes);
			AudioFormat format = Manager.GetAudioFormat(url);
			if (format == AudioFormat.unknown)
			{
				format = AudioFormat.mp3;
			}
			audioSource.clip = Manager.Load(stream, format, url, doStream, loadInBackground);
		}
	}
}
