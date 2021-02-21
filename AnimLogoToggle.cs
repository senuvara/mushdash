using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using GameFlow;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AnimLogoToggle : MonoBehaviour
{
	public GameObject peroperoAnimLogoObj;

	public GameObject eroeroAnimLogoObj;

	public OnStart start;

	public VariableBehaviour peroVariable;

	private void Start()
	{
		peroVariable = peroperoAnimLogoObj.GetComponent<VariableBehaviour>();
	}

	private void OnEnable()
	{
		int month = DateTime.Now.Month;
		int day = DateTime.Now.Day;
		if (month == 4 && day == 1)
		{
			eroeroAnimLogoObj.SetActive(true);
			return;
		}
		peroperoAnimLogoObj.SetActive(true);
		OnPeroLogo();
	}

	private void OnPeroLogo()
	{
		string empty = string.Empty;
		if (IsLoveDay())
		{
			empty = "VoiceBlowLogo";
		}
		else
		{
			List<string> result = peroVariable.variable.GetResult<List<string>>();
			empty = result.Random();
		}
		Singleton<AudioManager>.instance.PlayOneShot(empty, Singleton<DataManager>.instance["GameConfig"]["VoiceVolume"].GetResult<float>());
	}

	private void OnEroLogo()
	{
	}

	private bool IsLoveDay()
	{
		DateTime now = DateTime.Now;
		int month = now.Month;
		int day = now.Day;
		if (month == 2 && day == 14)
		{
			List<string> result = Singleton<DataManager>.instance["GameConfig"]["FirsetOpenEvent"].GetResult<List<string>>();
			if (!result.Contains("IsLoveDayFirstOpen"))
			{
				result.Add("IsLoveDayFirstOpen");
				Singleton<DataManager>.instance.Save();
				List<GameFlow.Action> actions = start.GetActions();
				for (int i = 0; i < actions.Count; i++)
				{
					if (i == 20)
					{
						GameFlow.Action obj = actions[i];
						FieldInfo field = typeof(TimeAction).GetField("_duration", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.GetField);
						field.SetValue(obj, 5f);
					}
				}
				Debug.Log("=======1");
				return true;
			}
		}
		return false;
	}
}
