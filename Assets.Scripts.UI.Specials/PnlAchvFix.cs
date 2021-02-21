using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI.Specials
{
	public class PnlAchvFix : MonoBehaviour
	{
		[InfoBox("特例化脚本，误删。PnlAchv拥有独立的Canvas，并且带嵌套的ContentSizeFilter时，第一次显示会有误，此脚本用于修复这个显示错误。", InfoMessageType.Info, null)]
		public GameObject target;

		private IEnumerator Start()
		{
			if (!(target == null))
			{
				target.SetActive(false);
				yield return null;
				target.SetActive(true);
			}
		}
	}
}
