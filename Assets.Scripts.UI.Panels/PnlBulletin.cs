using Assets.Scripts.Common;
using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.PreWarm;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
	public class PnlBulletin : SingletonMonoBehaviour<PnlBulletin>, IPreWarm
	{
		public YlyRichText txtContent;

		public Image image;

		public GameObject tglBulletin;

		public Transform content;

		public ToggleGroup toggleGroup;

		private readonly List<Toggle> m_Tgls = new List<Toggle>();

		private string m_Language;

		private static bool m_ShowFlag;

		private void OnEnable()
		{
			RefreshUI();
			if (!m_Tgls.Exists((Toggle t) => t.isOn) && m_Tgls.Count > 0)
			{
				m_Tgls.First().isOn = true;
			}
		}

		private void OnDisable()
		{
			Singleton<DataManager>.instance.Save();
		}

		public void Popup()
		{
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				string result = Singleton<DataManager>.instance["Account"]["Language"].GetResult<string>();
				if (Singleton<BulletinManager>.instance.bulletins.ContainsKey(result))
				{
					List<BulletinManager.Bulletin> list = Singleton<BulletinManager>.instance.bulletins[result];
					if (list.Exists((BulletinManager.Bulletin b) => b.isNew && b.force) && !m_ShowFlag)
					{
						m_ShowFlag = true;
						Singleton<EventManager>.instance.Invoke("UI/ShowPnlBulletin");
					}
				}
			}, () => Singleton<BulletinManager>.instance.bulletins != null, 20f);
		}

		public void PreWarm(int slice)
		{
			if (slice == 0)
			{
				SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
				{
					Singleton<BulletinManager>.instance.RefreshBulletin();
				}, () => Application.internetReachability != 0 || !this);
				SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(RefreshUI, () => Singleton<BulletinManager>.instance.bulletins != null || !this);
			}
		}

		private void RefreshUI()
		{
			string result = Singleton<DataManager>.instance["Account"]["Language"].GetResult<string>();
			if (m_Language == result || Singleton<BulletinManager>.instance.bulletins == null)
			{
				return;
			}
			m_Language = result;
			if (!Singleton<BulletinManager>.instance.bulletins.ContainsKey(result))
			{
				return;
			}
			for (int i = 0; i < content.childCount; i++)
			{
				Object.Destroy(content.GetChild(i).gameObject);
			}
			m_Tgls.Clear();
			List<BulletinManager.Bulletin> list = Singleton<BulletinManager>.instance.bulletins[result];
			for (int j = 0; j < list.Count; j++)
			{
				BulletinManager.Bulletin bulletin = list[j];
				Container component = Object.Instantiate(tglBulletin, content).GetComponent<Container>();
				Component goImg = component["ImgNew"];
				bulletin.GetTexture(delegate(Texture2D t)
				{
					GameUtils.CreateSpriteFromTexture(t);
					GetComponent<PnlBulletinSelect>().RefreshUISelectObj();
				});
				goImg.gameObject.SetActive(bulletin.isNew);
				YlyRichText ylyRichText = component["TxtTitle"] as YlyRichText;
				YlyRichText ylyRichText2 = component["TxtTitleSelected"] as YlyRichText;
				if ((bool)ylyRichText)
				{
					ylyRichText.text = bulletin.title;
				}
				if ((bool)ylyRichText2)
				{
					ylyRichText2.text = bulletin.title;
				}
				Toggle tgl = component["TglBulletinTitle(Clone)"] as Toggle;
				if (!tgl)
				{
					continue;
				}
				m_Tgls.Add(tgl);
				tgl.group = toggleGroup;
				tgl.onValueChanged.AddListener(delegate(bool isOn)
				{
					if (isOn)
					{
						bulletin.isNew = false;
						goImg.gameObject.SetActive(false);
						txtContent.text = bulletin.content;
						bulletin.GetTexture(delegate(Texture2D t)
						{
							if (tgl.isOn)
							{
								image.sprite = GameUtils.CreateSpriteFromTexture(t);
							}
						});
					}
				});
				tgl.isOn = false;
			}
		}
	}
}
