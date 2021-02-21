using Assets.Scripts.PeroTools.Commons;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Panels
{
	public class PnlOverseaAsk : UISelectManage
	{
		private List<Button> m_Buttons = new List<Button>();

		public override void OnInit()
		{
			m_Buttons = GameUtils.FindObjectsOfType<Button>(base.transform);
		}

		public override List<GameObject> SetSelectableObj()
		{
			List<GameObject> list = new List<GameObject>();
			m_Buttons.For(delegate(Button b)
			{
				list.Add(b.gameObject);
			});
			return list;
		}
	}
}
