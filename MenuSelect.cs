using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.UI;
using UnityEngine.UI;

public class MenuSelect : UISelectManage
{
	public Button howToPlay;

	private bool m_FromPreparation;

	public override void OnEnablePnl()
	{
		PanelType panelType = PanelManage.panel.Peek();
		if (panelType == (PanelType.Second | PanelType.Popup))
		{
			m_FromPreparation = true;
		}
		PanelManage.panel.Pop();
		PanelType panelType2 = PanelManage.panel.Peek();
		PanelManage.panel.Push(panel);
		if (panelType2 == (PanelType.Second | PanelType.Popup))
		{
			m_FromPreparation = true;
		}
		PanelManage.panel.Push(panel);
	}

	public override void OnDisablePnl()
	{
		if (m_FromPreparation)
		{
			PanelManage.panel.Push(PanelType.Second | PanelType.Popup);
			m_FromPreparation = false;
		}
	}

	public void SaveData()
	{
		Singleton<DataManager>.instance.Save();
	}

	public void SetBtnTutorialEnable(bool enable)
	{
	}
}
