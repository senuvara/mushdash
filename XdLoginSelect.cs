using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class XdLoginSelect : UISelectManage
{
	public enum XdWindowType
	{
		Main,
		Web
	}

	public XdWindowType type;

	private InputKeyBinding account;

	[ShowIf("m_IsWeb", true)]
	public Button WebCloseButton;

	private bool m_IsWebClose;

	private bool m_IsWeb()
	{
		return type == XdWindowType.Web;
	}

	public override void OnInit()
	{
		if (type == XdWindowType.Main)
		{
			account = Singleton<UIManager>.instance["PnlOption"].transform.Find("Toggles/Account").GetComponent<InputKeyBinding>();
			account.enabled = false;
		}
		else if (type == XdWindowType.Web)
		{
			WebCloseButton.onClick.AddListener(delegate
			{
				m_IsWebClose = true;
			});
		}
	}

	private void OnDestroy()
	{
		if (type == XdWindowType.Main)
		{
			GameObject gameObject = Singleton<UIManager>.instance["PnlOption"];
			gameObject.GetComponent<OptionSelect>().SetEnable(true);
			GameObject gameObject2 = gameObject.transform.Find("Toggles/Account").gameObject;
			EventSystem.current.SetSelectedGameObject(gameObject2);
			account.enabled = true;
		}
		else if (type == XdWindowType.Web && !m_IsWebClose)
		{
			CloseAllStackPopup();
		}
	}
}
