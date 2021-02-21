public class NexMusicInfo
{
	private static string[] uidSplit = new string[2];

	public static uint GetCategory(int musicIndex, int musicDiff)
	{
		return (uint)(musicIndex * 10 + musicDiff);
	}

	public static uint GetCategory(string musicUid, int musicDiff)
	{
		return GetCategory(GetIndex(musicUid), musicDiff);
	}

	public static int GetIndex(uint category)
	{
		return (int)(category / 10u);
	}

	public static int GetDiff(uint category)
	{
		return (int)(category - (int)(category / 10u * 10));
	}

	public static int GetDiff(string music)
	{
		return int.Parse(music.Split('_')[1]);
	}

	public static int GetIndex(string musicUid)
	{
		uidSplit = musicUid.Split('-');
		return int.Parse(uidSplit[0]) * 100 + int.Parse(uidSplit[1]);
	}

	public static string GetMusic(string musicUid, int diff)
	{
		return $"{musicUid}_{diff}";
	}
}
