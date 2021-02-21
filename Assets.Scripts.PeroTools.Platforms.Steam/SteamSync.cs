using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Microsoft.Win32;
using Steamworks;
using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Platforms.Steam
{
	public class SteamSync : ISync
	{
		private string m_FolderPath;

		private string m_FileName;

		private string m_FilePath;

		public SteamSync()
		{
			m_FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/Steam/MuseDash";
			CSteamID steamID = SteamUser.GetSteamID();
			m_FileName = steamID.m_SteamID + "MuseDashSaves.sav";
			m_FilePath = m_FolderPath + "/" + m_FileName;
		}

		public void LoadLocal()
		{
			Singleton<DataManager>.instance["GameConfig"].Load();
			if (!File.Exists(m_FilePath))
			{
				DeleteRegedit();
			}
			else
			{
				Singleton<DataManager>.instance.LoadFromBytes(File.ReadAllBytes(m_FilePath));
			}
		}

		public void SaveLocal()
		{
			Singleton<DataManager>.instance["GameConfig"].Save();
			byte[] bytes = Singleton<DataManager>.instance.ToBytes();
			if (!Directory.Exists(m_FolderPath))
			{
				Directory.CreateDirectory(m_FolderPath);
			}
			File.WriteAllBytes(m_FilePath, bytes);
			RemoveFile();
		}

		private void RemoveFile()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(m_FolderPath);
			FileInfo[] files = directoryInfo.GetFiles();
			foreach (FileInfo fileInfo in files)
			{
				if (fileInfo.Name != m_FileName)
				{
					File.Delete(m_FolderPath + "/" + fileInfo.Name);
				}
			}
		}

		private void DeleteRegedit()
		{
			Debug.Log("VortexBoy --- Steam DeleteRegedit Y !");
			Registry.CurrentUser.DeleteSubKey("Software\\PeroPeroGames\\Muse Dash", true);
		}
	}
}
