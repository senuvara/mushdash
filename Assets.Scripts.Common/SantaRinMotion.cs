using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Nice.Actions;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using Assets.Scripts.UI.Controls;
using Assets.Scripts.UI.Panels;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Assets.Scripts.Common
{
	public class SantaRinMotion : SerializedMonoBehaviour
	{
		private float m_StayTime;

		private bool m_HasSet;

		private void Update()
		{
			if (!m_HasSet && !SingletonMonoBehaviour<PnlBulletin>.instance.gameObject.activeSelf && Popup.popups.Count == 0)
			{
				if ((m_StayTime += Time.deltaTime) >= 0.3f)
				{
					DateTime now = DateTime.Now;
					int month = now.Month;
					int day = now.Day;
					IVariable data = Singleton<DataManager>.instance["Account"]["HasGetSantaRin"];
					if (!data.GetResult<bool>() && month == 12 && (day == 24 || day == 25))
					{
						data.SetResult(true);
						m_HasSet = true;
						SingletonMonoBehaviour<CharacterExpression>.instance.Express(1, 1);
						Singleton<DataManager>.instance.Save();
					}
				}
			}
			else
			{
				m_StayTime = 0f;
			}
		}
	}
}
