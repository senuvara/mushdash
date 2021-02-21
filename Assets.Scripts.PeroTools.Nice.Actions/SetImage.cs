using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class SetImage : Action
	{
		[SerializeField]
		[Variable(typeof(Image), null, false)]
		private IVariable m_Object;

		[SerializeField]
		[Variable(typeof(Object), null, false)]
		[OnConstanceChanged("OnImageSourceChanged")]
		private IVariable m_ImageSource;

		[SerializeField]
		[HideInInspector]
		private string m_Path;

		[SerializeField]
		[HideInInspector]
		private List<string> m_TextureNames;

		public override void Enter()
		{
			OnImageSourceChanged();
		}

		public override void Execute()
		{
			Image @object = GameUtils.GetObject<Image>(m_Object.result);
			if (!@object)
			{
				return;
			}
			object obj = m_ImageSource.result;
			if (m_TextureNames != null && m_TextureNames.Count > 0)
			{
				obj = m_TextureNames.Random();
			}
			if (obj == null)
			{
				return;
			}
			Sprite object2 = GameUtils.GetObject<Sprite>(obj);
			if ((bool)object2)
			{
				@object.sprite = object2;
				return;
			}
			Texture2D object3 = GameUtils.GetObject<Texture2D>(obj);
			if ((bool)object3)
			{
				@object.sprite = GameUtils.CreateSpriteFromTexture(object3);
			}
		}

		private void OnImageSourceChanged()
		{
			if (!Application.isPlaying && m_TextureNames != null)
			{
				m_TextureNames.Clear();
			}
			if (m_ImageSource != null && m_ImageSource.result != null)
			{
				object result = m_ImageSource.result;
				List<string> assetPathsInFolder = SingletonScriptableObject<AssetBundleConfigManager>.instance.GetAssetPathsInFolder(m_Path);
				if (assetPathsInFolder != null)
				{
					m_TextureNames = assetPathsInFolder.Select(Path.GetFileNameWithoutExtension);
				}
			}
		}
	}
}
