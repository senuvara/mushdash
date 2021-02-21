using System.Collections.Generic;
using UnityEngine;

public class ANAMusicExample : MonoBehaviour
{
	private enum GUIRects
	{
		NativeLabel,
		PlayPauseButton,
		IsPlayingLabel,
		NativeStopButton,
		PositionLabel,
		SeekButton,
		LoopUnloopButton,
		IsLoopingLabel,
		VolumeMuteButton,
		VolumeLabel,
		PlayInBackgroundButton,
		PlayInBackgroundLabel,
		ReleaseLoadButton,
		DurationLabel,
		UnityLabel,
		UnityPlayButton,
		UnityStopButton
	}

	private int MusicID;

	public GUISkin GUISkin;

	private GUIStyle StatusStyle;

	private bool IsLoaded;

	private string PlayPauseButton = "Play";

	private string IsPlayingString = "Is Playing: False";

	private string LoopUnloopButton = "Loop";

	private string IsLoopingString = "Is Looping: False";

	private bool IsMute;

	private string VolumeMuteButton = "Mute";

	private string VolumeString = "Volume: 1.0";

	private bool IsPlayInBackground;

	private string PlayInBackgroundString = "Play In Background: False";

	private string DurationString;

	private AudioSource UnityAudio;

	private Dictionary<GUIRects, Rect> GUIRect = new Dictionary<GUIRects, Rect>();

	private void Start()
	{
		Load();
		UnityAudio = GetComponent<AudioSource>();
		SetupGUI();
	}

	private void Load()
	{
		MusicID = ANAMusic.load("Android Native Audio/Music Native.ogg", false, true, Loaded);
	}

	private void Loaded(int musicID)
	{
		DurationString = "Duration: " + ANAMusic.getDuration(musicID);
		IsLoaded = true;
		PlayPauseButton = "Play";
		IsPlayingString = "Is Playing: False";
		LoopUnloopButton = "Loop";
		IsLoopingString = "Is Looping: False";
		IsMute = false;
		VolumeMuteButton = "Mute";
		VolumeString = "Volume: 1.0";
		IsPlayInBackground = false;
		PlayInBackgroundString = "Play In Background: False";
	}

	private void PlayPause()
	{
		if (ANAMusic.isPlaying(MusicID))
		{
			ANAMusic.pause(MusicID);
			StoppedStrings(MusicID);
		}
		else
		{
			ANAMusic.play(MusicID, StoppedStrings);
			PlayPauseButton = "Pause";
			IsPlayingString = "Is Playing: True";
		}
	}

	private void Stop()
	{
		ANAMusic.pause(MusicID);
		ANAMusic.seekTo(MusicID, 0);
		StoppedStrings(MusicID);
	}

	private string GetPositionLabel()
	{
		return "Position: " + ANAMusic.getCurrentPosition(MusicID);
	}

	private void Seek()
	{
		ANAMusic.seekTo(MusicID, 73070);
	}

	private void LoopUnloop()
	{
		if (ANAMusic.isLooping(MusicID))
		{
			ANAMusic.setLooping(MusicID, false);
			LoopUnloopButton = "Loop";
			IsLoopingString = "Is Looping: False";
		}
		else
		{
			ANAMusic.setLooping(MusicID, true);
			LoopUnloopButton = "Unloop";
			IsLoopingString = "Is Looping: True";
		}
	}

	private void VolumeMute()
	{
		if (IsMute)
		{
			ANAMusic.setVolume(MusicID, 1f);
			VolumeMuteButton = "Mute";
			VolumeString = "Volume: 1.0";
			IsMute = false;
		}
		else
		{
			ANAMusic.setVolume(MusicID, 0f);
			VolumeMuteButton = "Volume";
			VolumeString = "Volume: 0.0";
			IsMute = true;
		}
	}

	private void PlayInBackground()
	{
		if (IsPlayInBackground)
		{
			ANAMusic.setPlayInBackground(MusicID, false);
			PlayInBackgroundString = "Play In Background: False";
			IsPlayInBackground = false;
		}
		else
		{
			ANAMusic.setPlayInBackground(MusicID, true);
			PlayInBackgroundString = "Play In Background: True";
			IsPlayInBackground = true;
		}
	}

	private void Release()
	{
		ANAMusic.release(MusicID);
		IsLoaded = false;
	}

	private void OnApplicationQuit()
	{
		ANAMusic.release(MusicID);
	}

	private void StoppedStrings(int musicID)
	{
		PlayPauseButton = "Play";
		IsPlayingString = "Is Playing: False";
	}

	private void OnGUI()
	{
		GUI.skin = GUISkin;
		StatusStyle = GUI.skin.GetStyle("status");
		GUI.Label(GUIRect[GUIRects.NativeLabel], "Native Audio");
		if (IsLoaded)
		{
			if (GUI.Button(GUIRect[GUIRects.PlayPauseButton], PlayPauseButton))
			{
				PlayPause();
			}
			GUI.Label(GUIRect[GUIRects.IsPlayingLabel], IsPlayingString, StatusStyle);
			if (GUI.Button(GUIRect[GUIRects.NativeStopButton], "Stop"))
			{
				Stop();
			}
			GUI.Label(GUIRect[GUIRects.PositionLabel], GetPositionLabel(), StatusStyle);
			if (GUI.Button(GUIRect[GUIRects.SeekButton], "Seek"))
			{
				Seek();
			}
			if (GUI.Button(GUIRect[GUIRects.LoopUnloopButton], LoopUnloopButton))
			{
				LoopUnloop();
			}
			GUI.Label(GUIRect[GUIRects.IsLoopingLabel], IsLoopingString, StatusStyle);
			if (GUI.Button(GUIRect[GUIRects.VolumeMuteButton], VolumeMuteButton))
			{
				VolumeMute();
			}
			GUI.Label(GUIRect[GUIRects.VolumeLabel], VolumeString, StatusStyle);
			if (GUI.Button(GUIRect[GUIRects.PlayInBackgroundButton], "PIBG"))
			{
				PlayInBackground();
			}
			GUI.Label(GUIRect[GUIRects.PlayInBackgroundLabel], PlayInBackgroundString, StatusStyle);
			if (GUI.Button(GUIRect[GUIRects.ReleaseLoadButton], "Release"))
			{
				Release();
			}
			GUI.Label(GUIRect[GUIRects.DurationLabel], DurationString, StatusStyle);
		}
		else if (GUI.Button(GUIRect[GUIRects.ReleaseLoadButton], "Load"))
		{
			Load();
		}
		GUI.Label(GUIRect[GUIRects.UnityLabel], "Unity Audio");
		if (GUI.Button(GUIRect[GUIRects.UnityPlayButton], "Play"))
		{
			UnityAudio.Play();
		}
		if (GUI.Button(GUIRect[GUIRects.UnityStopButton], "Stop"))
		{
			UnityAudio.Stop();
		}
	}

	private void SetupGUI()
	{
		GUIRect.Add(GUIRects.NativeLabel, new Rect((float)Screen.width * 0.2f, (float)Screen.height * 0f, (float)Screen.width * 0.5f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.PlayPauseButton, new Rect((float)Screen.width * 0.2f, (float)Screen.height * 0.1f, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.IsPlayingLabel, new Rect((float)Screen.width * 0.5f, (float)Screen.height * 0.15f, (float)Screen.width * 0.5f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.NativeStopButton, new Rect((float)Screen.width * 0.2f, (float)Screen.height * 0.2f, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.PositionLabel, new Rect((float)Screen.width * 0.5f, (float)Screen.height * 0.25f, (float)Screen.width * 0.5f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.SeekButton, new Rect((float)Screen.width * 0.2f, (float)Screen.height * 0.3f, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.LoopUnloopButton, new Rect((float)Screen.width * 0.2f, (float)Screen.height * 0.4f, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.IsLoopingLabel, new Rect((float)Screen.width * 0.5f, (float)Screen.height * 0.4f, (float)Screen.width * 0.5f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.VolumeMuteButton, new Rect((float)Screen.width * 0.2f, (float)Screen.height * 0.5f, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.VolumeLabel, new Rect((float)Screen.width * 0.5f, (float)Screen.height * 0.5f, (float)Screen.width * 0.5f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.PlayInBackgroundButton, new Rect((float)Screen.width * 0.2f, (float)Screen.height * 0.6f, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.PlayInBackgroundLabel, new Rect((float)Screen.width * 0.5f, (float)Screen.height * 0.6f, (float)Screen.width * 0.5f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.ReleaseLoadButton, new Rect((float)Screen.width * 0.2f, (float)Screen.height * 0.7f, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.DurationLabel, new Rect((float)Screen.width * 0.5f, (float)Screen.height * 0.7f, (float)Screen.width * 0.5f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.UnityLabel, new Rect((float)Screen.width * 0.2f, (float)Screen.height * 0.8f, (float)Screen.width * 0.5f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.UnityPlayButton, new Rect((float)Screen.width * 0.2f, (float)Screen.height * 0.9f, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f));
		GUIRect.Add(GUIRects.UnityStopButton, new Rect((float)Screen.width * 0.4f, (float)Screen.height * 0.9f, (float)Screen.width * 0.2f, (float)Screen.height * 0.1f));
	}
}
