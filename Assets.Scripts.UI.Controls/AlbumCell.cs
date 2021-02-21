using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.Nice.Variables;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Controls
{
	public class AlbumCell : MonoBehaviour
	{
		public Button btn;

		public GameObject imgLock;

		public GameObject imgMy;

		public GameObject imgHeart;

		public GameObject locker;

		private VariableBehaviour m_VariableBehaviour;

		public GameObject imgFavoriteMusic;

		public GameObject weekFreeIcon;

		private bool m_Lock;

		public string uid
		{
			get;
			private set;
		}

		public int GetDataIndex()
		{
			return (int)m_VariableBehaviour.result;
		}

		public void SetUid(string uid)
		{
			this.uid = uid;
			if ((bool)imgFavoriteMusic)
			{
				imgFavoriteMusic.SetActive(uid == "collections");
			}
		}

		public void SetLock(bool isLock)
		{
			if ((bool)imgHeart)
			{
				imgHeart.SetActive(uid == "collections");
			}
			if ((bool)locker)
			{
				if (!uid.StartsWith("tag-new"))
				{
					locker.SetActive(uid != "collections");
				}
				else
				{
					locker.SetActive(isLock);
					imgMy.SetActive(!isLock);
					m_Lock = isLock;
				}
			}
			if ((bool)imgLock)
			{
				if (uid == "collections")
				{
					imgLock.gameObject.SetActive(Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>().Count == 0);
				}
				else if (uid == "hide")
				{
					imgLock.gameObject.SetActive(Singleton<DataManager>.instance["Account"]["Hides"].GetResult<List<string>>().Count == 0);
				}
				else
				{
					imgLock.gameObject.SetActive(isLock);
				}
			}
		}

		public void SetWeekFree(bool isWeekFree)
		{
			if ((bool)weekFreeIcon)
			{
				weekFreeIcon.SetActive(isWeekFree);
			}
			if (!imgMy)
			{
				return;
			}
			if (!uid.StartsWith("tag-new"))
			{
				imgMy.SetActive(!isWeekFree);
			}
			else if ((bool)locker)
			{
				if (!m_Lock)
				{
					imgMy.SetActive(!isWeekFree);
				}
				else
				{
					locker.SetActive(!isWeekFree);
				}
			}
		}

		public bool isLock()
		{
			if (Enumerable.Contains(Singleton<WeekFreeManager>.instance.freeAlbumUids, uid))
			{
				return false;
			}
			if (uid == "collections" && (bool)imgLock)
			{
				imgLock.gameObject.SetActive(Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>().Count == 0);
			}
			if (uid == "hide")
			{
				bool flag = Singleton<DataManager>.instance["Account"]["Hides"].GetResult<List<string>>().Count == 0;
				if ((bool)imgLock)
				{
					imgLock.gameObject.SetActive(flag);
				}
				return flag;
			}
			bool flag2 = imgLock != null;
			if (flag2)
			{
				flag2 = imgLock.gameObject.activeSelf;
			}
			return flag2 || (uid == "collections" && Singleton<DataManager>.instance["Account"]["Collections"].GetResult<List<string>>().Count == 0);
		}

		private void Awake()
		{
			m_VariableBehaviour = GetComponent<VariableBehaviour>();
		}
	}
}
