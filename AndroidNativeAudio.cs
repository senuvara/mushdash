using System;
using UnityEngine;

public static class AndroidNativeAudio
{
	private const string _logPrefix = "AndroidNativeAudio: ";

	public static int load(string audioFile, bool usePersistentDataPath = false, Action<int> callback = null)
	{
		Debug.Log(string.Concat("AndroidNativeAudio: load(\"", audioFile, "\", ", usePersistentDataPath, "\", ", callback, ")"));
		return 1;
	}

	public static void makePool(int maxStreams = 16)
	{
		Debug.Log("AndroidNativeAudio: makePool(" + maxStreams + ")");
	}

	public static void pause(int streamID)
	{
		Debug.Log("AndroidNativeAudio: pause(" + streamID + ")");
	}

	public static void pauseAll()
	{
		Debug.Log("AndroidNativeAudio: pauseAll()");
	}

	public static int play(int fileID, float leftVolume = 1f, float rightVolume = -1f, int priority = 1, int loop = 0, float rate = 1f)
	{
		Debug.Log("AndroidNativeAudio: play(" + fileID + ", " + leftVolume + ", " + rightVolume + ", " + priority + ", " + loop + ", " + rate + ")");
		return 1;
	}

	public static void releasePool()
	{
		Debug.Log("AndroidNativeAudio: releasePool()");
	}

	public static void resume(int streamID)
	{
		Debug.Log("AndroidNativeAudio: resume(" + streamID + ")");
	}

	public static void resumeAll()
	{
		Debug.Log("AndroidNativeAudio: resumeAll()");
	}

	public static void setLoop(int streamID, int loop)
	{
		Debug.Log("AndroidNativeAudio: setLoop(" + streamID + ", " + loop + ")");
	}

	public static void setPriority(int streamID, int priority)
	{
		Debug.Log("AndroidNativeAudio: setPriority(" + streamID + ", " + priority + ")");
	}

	public static void setRate(int streamID, float rate)
	{
		Debug.Log("AndroidNativeAudio: setRate(" + streamID + ", " + rate + ")");
	}

	public static void setVolume(int streamID, float leftVolume, float rightVolume = -1f)
	{
		Debug.Log("AndroidNativeAudio: setVolume(" + streamID + ", " + leftVolume + ", " + rightVolume + ")");
	}

	public static void stop(int streamID)
	{
		Debug.Log("AndroidNativeAudio: stop(" + streamID + ")");
	}

	public static bool unload(int fileID)
	{
		Debug.Log("AndroidNativeAudio: unload(" + fileID + ")");
		return true;
	}
}
