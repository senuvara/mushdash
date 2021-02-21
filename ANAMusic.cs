using System;
using UnityEngine;

public static class ANAMusic
{
	private const string _logPrefix = "ANA Music: ";

	public static int getCurrentPosition(int musicID)
	{
		Debug.Log("ANA Music: getCurrentPosition(" + musicID + ")");
		return 1;
	}

	public static int getDuration(int musicID)
	{
		Debug.Log("ANA Music: getDuration(" + musicID + ")");
		return 1;
	}

	public static bool isLooping(int musicID)
	{
		Debug.Log("ANA Music: isLooping(" + musicID + ")");
		return false;
	}

	public static bool isPlaying(int musicID)
	{
		Debug.Log("ANA Music: isPlaying(" + musicID + ")");
		return false;
	}

	public static int load(string audioFile, bool usePersistentDataPath = false, bool loadAsync = false, Action<int> loadedCallback = null, bool playInBackground = false)
	{
		Debug.Log(string.Concat("ANA Music: load(\"", audioFile, "\", ", usePersistentDataPath, ", ", loadAsync, ", ", loadedCallback, ", ", playInBackground, ")"));
		return 1;
	}

	public static void OnApplicationPause(bool isPaused)
	{
		Debug.Log("ANA Music: OnApplicationPause(" + isPaused + ")");
	}

	public static void pause(int musicID)
	{
		Debug.Log("ANA Music: pause(" + musicID + ")");
	}

	public static void pauseAll()
	{
		Debug.Log("ANA Music: pauseAll()");
	}

	public static void play(int musicID, Action<int> completionCallback = null)
	{
		Debug.Log(string.Concat("ANA Music: play(", musicID, ", ", completionCallback, ")"));
	}

	public static void release(int musicID)
	{
		Debug.Log("ANA Music: release(" + musicID + ")");
	}

	public static void resumeAll()
	{
		Debug.Log("ANA Music: resumeAll()");
	}

	public static void seekTo(int musicID, int msec)
	{
		Debug.Log("ANA Music: seekTo(" + musicID + ", " + msec + ")");
	}

	public static void setLooping(int musicID, bool looping)
	{
		Debug.Log("ANA Music: setLooping(" + musicID + ", " + looping + ")");
	}

	public static void setPlayInBackground(int musicID, bool playInBackground)
	{
		Debug.Log("ANA Music: setPlayInBackground(" + musicID + ", " + playInBackground + ")");
	}

	public static void setVolume(int musicID, float leftVolume, float rightVolume = -1f)
	{
		Debug.Log("ANA Music: setVolume(" + musicID + ", " + leftVolume + ", " + rightVolume + ")");
	}
}
