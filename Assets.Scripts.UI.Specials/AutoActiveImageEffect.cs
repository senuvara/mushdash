using Assets.Scripts.Graphics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.UI.Specials
{
	public class AutoActiveImageEffect : MonoBehaviour
	{
		[InfoBox("特例化脚本，用于使PnlStage打开/关闭时 同时激活/关闭ImageEffect。", InfoMessageType.Info, null)]
		public PostEffectsBase imageEffect;

		private void OnEnable()
		{
			if (imageEffect != null)
			{
				imageEffect.enabled = true;
			}
		}

		private void OnDisable()
		{
			if (imageEffect != null)
			{
				imageEffect.enabled = false;
			}
		}
	}
}
