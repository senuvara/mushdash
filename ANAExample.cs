using System.Collections.Generic;
using UnityEngine;

public class ANAExample : MonoBehaviour
{
	private enum GUIRects
	{
		NativeLabel,
		NativePlayButton,
		UnityLabel,
		UnityPlayButton
	}

	private int FileID;

	private int SoundID;

	public GUISkin GUISkin;

	private AudioSource UnityAudio;

	private Dictionary<GUIRects, Rect> GUIRect = new Dictionary<GUIRects, Rect>();

	private void Start()
	{
		AndroidNativeAudio.makePool();
		FileID = AndroidNativeAudio.load("Android Native Audio/Tone Native.wav");
		UnityAudio = GetComponent<AudioSource>();
		SetupGUI();
	}

	private void OnGUI()
	{
		GUI.skin = GUISkin;
		GUI.Label(GUIRect[GUIRects.NativeLabel], "Native Audio");
		if (GUI.Button(GUIRect[GUIRects.NativePlayButton], "Play"))
		{
			SoundID = AndroidNativeAudio.play(FileID);
		}
		GUI.Label(GUIRect[GUIRects.UnityLabel], "Unity Audio");
		if (GUI.Button(GUIRect[GUIRects.UnityPlayButton], "Play"))
		{
			UnityAudio.Play();
		}
	}

	private void OnApplicationQuit()
	{
		AndroidNativeAudio.unload(FileID);
		AndroidNativeAudio.releasePool();
	}

	private void ModifySound()
	{
		AndroidNativeAudio.pause(SoundID);
		AndroidNativeAudio.resume(SoundID);
		AndroidNativeAudio.stop(SoundID);
		AndroidNativeAudio.pauseAll();
		AndroidNativeAudio.resumeAll();
		AndroidNativeAudio.setVolume(SoundID, 0.5f);
		AndroidNativeAudio.setLoop(SoundID, 3);
		AndroidNativeAudio.setPriority(SoundID, 5);
		AndroidNativeAudio.setRate(SoundID, 0.75f);
	}

	private void SetupGUI()
	{
		GUIRect.Add(GUIRects.NativeLabel, new Rect((float)Screen.width * 0.3f, (float)Screen.height * 0.2f, (float)Screen.width * 0.5f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.NativePlayButton, new Rect((float)Screen.width * 0.4f, (float)Screen.height * 0.3f, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.UnityLabel, new Rect((float)Screen.width * 0.3f, (float)Screen.height * 0.5f, (float)Screen.width * 0.5f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.UnityPlayButton, new Rect((float)Screen.width * 0.4f, (float)Screen.height * 0.6f, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f));
	}
}
