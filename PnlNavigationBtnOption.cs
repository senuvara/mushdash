using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PnlNavigationBtnOption : MonoBehaviour
{
	[Flags]
	public enum MenuType
	{
		Option = 0x0,
		Elfin = 0x1,
		Role = 0x2,
		Trove = 0x4,
		Achv = 0x8
	}

	[Required]
	public Transform toggleGroupPc;

	[Required]
	public Transform toggleGroup;

	private Toggle m_Option;

	private static Toggle m_Role;

	private static Toggle m_Eflin;

	private static Toggle m_OptionNs;

	private static Toggle m_Trove;

	private static Toggle m_Achv;

	private static GameObject m_Shine;

	private static MenuType m_LastType;

	private Toggle[] m_Panels;

	private static MenuType m_Type;

	private void Start()
	{
		m_Role = toggleGroupPc.Find("TglRole").GetComponent<Toggle>();
		m_Eflin = toggleGroupPc.Find("TglElfin").GetComponent<Toggle>();
		m_OptionNs = toggleGroupPc.Find("TglOption_Pc").GetComponent<Toggle>();
		m_Trove = toggleGroupPc.Find("TglTrove").GetComponent<Toggle>();
		m_Achv = toggleGroupPc.Find("TglAchv").GetComponent<Toggle>();
		try
		{
			m_Shine = base.transform.Find("ImgShine").gameObject;
		}
		catch (Exception)
		{
		}
		m_Panels = new Toggle[9]
		{
			m_OptionNs,
			m_Eflin,
			m_Role,
			null,
			m_Trove,
			null,
			null,
			null,
			m_Achv
		};
		int result = Singleton<DataManager>.instance["Account"]["NsUnlockNewTip"].GetResult<int>();
		if (result != 0)
		{
			UnlockNew((MenuType)result);
		}
	}

	public static void UnlockNew(MenuType t)
	{
		if ((m_Type & t) == 0)
		{
			m_Type += (int)t;
		}
		Singleton<DataManager>.instance["Account"]["NsUnlockNewTip"].SetResult((int)m_Type);
		if (m_Shine != null)
		{
			m_Shine.gameObject.SetActive(true);
		}
	}

	public void SetMenuType(MenuType type)
	{
		Toggle toggle = m_Panels[(int)type];
		toggle.isOn = true;
	}

	public void OnClick()
	{
		if ((m_Type & MenuType.Role) != 0)
		{
			m_Role.isOn = true;
			return;
		}
		if ((m_Type & MenuType.Elfin) != 0)
		{
			m_Eflin.isOn = true;
			return;
		}
		int lastType = (int)m_LastType;
		Toggle toggle = m_Panels[lastType];
		toggle.isOn = true;
	}

	public static void ClearNewTip(string name)
	{
		MenuType menuType = (!(name == "character")) ? MenuType.Elfin : MenuType.Role;
		Debug.Log(menuType);
		if ((m_Type & menuType) > MenuType.Option)
		{
			m_Type -= (int)menuType;
		}
		else
		{
			Debug.LogError("Not include " + name + " Tip");
		}
		if (m_Type == MenuType.Option && m_Shine != null)
		{
			m_Shine.gameObject.SetActive(false);
		}
		Singleton<DataManager>.instance["Account"]["NsUnlockNewTip"].SetResult((int)m_Type);
	}

	public static void RecordClosePanel(int t)
	{
		m_LastType = (MenuType)t;
	}
}
