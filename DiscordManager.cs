using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Discord;
using SA.Common.Pattern;
using UnityEngine;

public class DiscordManager : SA.Common.Pattern.Singleton<DiscordManager>
{
	public Discord.Discord discord;

	public ActivityManager activityManager;

	public ApplicationManager applicationManager;

	private bool m_DiscordRunCallback;

	public void InitDiscord()
	{
		discord = new Discord.Discord(599659394082406493L, 1uL);
		if (discord.IsInit == Result.Ok)
		{
			SingletonMonoBehaviour<UnityGameManager>.instance.RegLoop("discordCallBack", DiscordRunCallbacks, UnityGameManager.LoopType.Update);
			activityManager = discord.GetActivityManager();
			applicationManager = discord.GetApplicationManager();
			SetUpdateActivity(false, string.Empty);
		}
	}

	private void DiscordRunCallbacks(float time)
	{
		if (discord.IsInit == Result.Ok)
		{
			m_DiscordRunCallback = discord.RunCallbacks();
			if (!m_DiscordRunCallback)
			{
				SingletonMonoBehaviour<UnityGameManager>.instance.UnregLoop("discordCallBack");
			}
		}
	}

	public void SetUpdateActivity(bool isPlaying, string levelInfo)
	{
		if (activityManager == null || applicationManager == null)
		{
			return;
		}
		int result2 = Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedDifficulty"].GetResult<int>();
		string str = string.Empty;
		switch (result2)
		{
		case 1:
			str = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "diffcultyEasy");
			break;
		case 2:
			str = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "diffcultyHard");
			break;
		case 3:
			str = Assets.Scripts.PeroTools.Commons.Singleton<ConfigManager>.instance.GetConfigStringValue("tip", 0, "diffcultyMaster");
			break;
		}
		string state = (!isPlaying) ? "In Menu" : levelInfo;
		Activity activity = default(Activity);
		activity.State = state;
		activity.Details = str + " - Lvl." + Assets.Scripts.PeroTools.Commons.Singleton<DataManager>.instance["Account"]["SelectedMusicLevel"].GetResult<string>();
		Activity activity2 = activity;
		if (!isPlaying)
		{
			activity = default(Activity);
			activity.State = state;
			activity2 = activity;
		}
		activityManager.UpdateActivity(activity2, delegate(Result result)
		{
			if (result != 0)
			{
				Debug.Log("Discord Update Activity Failed!");
			}
		});
	}
}
