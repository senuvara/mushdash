using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.PeroTools.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PnlStageIcon : MonoBehaviour
{
	private Image m_Image;

	private Sprite m_Sprite;

	private Text m_Title;

	private Color m_TitleColor;

	private Color m_BgColor;

	private bool m_IsLock;

	public Sprite iconLock;

	public Sprite iconWeekFree;

	private void Awake()
	{
		m_Image = base.transform.Find("ImgIcon").GetComponent<Image>();
		m_Title = base.transform.Find("TxtAlbumTitle").GetComponent<Text>();
		m_TitleColor = m_Title.color;
		m_Sprite = m_Image.sprite;
		m_BgColor = GetComponent<Image>().color;
	}

	private bool IsPurchased()
	{
		if (BtnIAP.IsUnlockAll())
		{
			return true;
		}
		return false;
	}

	private void OnEnable()
	{
		if (base.gameObject.name == "ImgAlbum6")
		{
			if (Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>() >= 30)
			{
				m_Image.sprite = m_Sprite;
				m_Image.color = Color.white;
				m_Title.color = m_TitleColor;
				GetComponent<Image>().color = m_BgColor;
			}
			else
			{
				m_Image.sprite = iconLock;
				m_Image.color = new Color(0.384f, 0.29f, 0.576f, 1f);
				m_Title.color = new Color(0.384f, 0.29f, 0.576f, 1f);
				GetComponent<Image>().color = new Color(0.216f, 0.106f, 0.369f, 1f);
			}
			return;
		}
		if (base.gameObject.name == "ImgAlbum21")
		{
			if (Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>() >= 15)
			{
				m_Image.sprite = m_Sprite;
				m_Image.color = Color.white;
				m_Title.color = m_TitleColor;
				GetComponent<Image>().color = m_BgColor;
			}
			else
			{
				m_Image.sprite = iconLock;
				m_Image.color = new Color(0.384f, 0.29f, 0.576f, 1f);
				m_Title.color = new Color(0.384f, 0.29f, 0.576f, 1f);
				GetComponent<Image>().color = new Color(0.216f, 0.106f, 0.369f, 1f);
			}
			return;
		}
		if (m_IsLock && IsPurchased())
		{
			m_Image.sprite = m_Sprite;
			m_Image.color = Color.white;
			m_Title.color = m_TitleColor;
			GetComponent<Image>().color = m_BgColor;
			m_IsLock = false;
		}
		if (m_IsLock || IsPurchased())
		{
			return;
		}
		if (Singleton<WeekFreeManager>.instance.freeAlbumIndexs != null)
		{
			if (Enumerable.Contains(Singleton<WeekFreeManager>.instance.freeAlbumIndexs, int.Parse(base.gameObject.name.Replace("ImgAlbum", string.Empty))))
			{
				m_Image.sprite = iconWeekFree;
				GetComponent<Image>().color = m_BgColor;
			}
			else
			{
				m_Image.sprite = iconLock;
				m_Image.color = new Color(0.384f, 0.29f, 0.576f, 1f);
				m_Title.color = new Color(0.384f, 0.29f, 0.576f, 1f);
				GetComponent<Image>().color = new Color(0.216f, 0.106f, 0.369f, 1f);
			}
		}
		m_IsLock = true;
	}
}
