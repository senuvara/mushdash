using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using Assets.Scripts.UI.Panels;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicStageCell : MonoBehaviour
{
	[SerializeField]
	private VariableBehaviour m_CellVariable;

	private int m_CellIndex;

	private UnityEngine.UI.Text m_StageTitle;

	[SerializeField]
	private Image m_StageImg;

	public string[] m_MusicList;

	public int[] m_MusicLevelList;

	private bool m_IsBase;

	[SerializeField]
	private GameObject m_LockObj;

	private int m_MusicLevel;

	[SerializeField]
	private UnityEngine.UI.Text m_LockTxt;

	private List<int> m_LockAlbumUids = new List<int>
	{
		1,
		7,
		22
	};

	[SerializeField]
	private GameObject m_WeekFreeImg;

	private void Awake()
	{
		m_CellIndex = m_CellVariable.GetResult<int>();
		PnlStage.m_MusicCellSelected = (PnlStage.OnMusicCellSelected)Delegate.Combine(PnlStage.m_MusicCellSelected, new PnlStage.OnMusicCellSelected(OnChangeCell));
	}

	private void OnDestroy()
	{
		PnlStage.m_MusicCellSelected = (PnlStage.OnMusicCellSelected)Delegate.Remove(PnlStage.m_MusicCellSelected, new PnlStage.OnMusicCellSelected(OnChangeCell));
	}

	internal void OnChangeCell(float i)
	{
		m_CellIndex = m_CellVariable.GetResult<int>();
		string text = m_MusicList[m_CellIndex];
		m_WeekFreeImg.SetActive(Singleton<WeekFreeManager>.instance.freeSongUids.Contains(text));
		if (text != "?")
		{
			string s = text.BeginBefore('-');
			int num = int.Parse(s);
			s = (num + 1).ToString();
			m_MusicLevel = m_MusicLevelList[m_CellIndex];
			if (m_LockAlbumUids.Contains(num + 1))
			{
				IsBaseAlbum();
			}
			else
			{
				m_LockObj.SetActive(false);
			}
			string configStringValue = Singleton<ConfigManager>.instance.GetConfigStringValue("ALBUM" + s, "uid", "cover", m_MusicList[m_CellIndex]);
			m_StageImg.sprite = Singleton<AssetBundleManager>.instance.LoadFromName<Sprite>(configStringValue);
		}
		else
		{
			m_LockObj.SetActive(false);
			m_MusicLevel = m_MusicLevelList[m_CellIndex];
			m_StageImg.sprite = Singleton<AssetBundleManager>.instance.LoadFromName<Sprite>("random_song_cover");
		}
	}

	private void IsBaseAlbum()
	{
		bool flag = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>() < m_MusicLevel;
		m_LockObj.SetActive(flag);
		if (flag)
		{
			switch (Singleton<DataManager>.instance.GetVariable("Account/Language").GetResult<string>())
			{
			case "ChineseS":
				m_LockTxt.text = "     等级 " + m_MusicLevel + " 解锁     ";
				break;
			case "ChineseT":
				m_LockTxt.text = "等級 " + m_MusicLevel + " 解鎖";
				break;
			case "English":
				m_LockTxt.text = "UNLOCKED AT LV." + m_MusicLevel;
				break;
			case "Japanese":
				m_LockTxt.text = "LV." + m_MusicLevel + " で解禁されます";
				break;
			case "Korean":
				m_LockTxt.text = "     레벨 " + m_MusicLevel + " 해제     ";
				break;
			}
		}
	}
}
