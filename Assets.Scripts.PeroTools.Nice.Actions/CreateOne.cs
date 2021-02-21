using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class CreateOne : Action
	{
		[SerializeField]
		[HideInEditorMode]
		[HideInPlayMode]
		private bool m_IsPro;

		[SerializeField]
		[AssetsOnly]
		[LabelText("Folder/Json/Prefab")]
		[OnValueChanged("OnListChanged", false)]
		private Object m_List;

		[SerializeField]
		[CustomValueDrawer("OnJsonKeyShow")]
		[ShowIf("HasJsonKeys", true)]
		private string m_JsonKey;

		[SerializeField]
		[HideInInspector]
		private List<string> m_ObjectNames;

		[SerializeField]
		[HideInInspector]
		private GameObject m_GameObject;

		[SerializeField]
		[HideInInspector]
		private string m_Path;

		[SerializeField]
		[Variable(0, null, false)]
		[ShowIf("m_IsPro", true)]
		[HideIf("IsObjectsNull", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		private IVariable m_Index;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		[ShowIf("IsObjectsNull", true)]
		[HideIf("IsListNull", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		private int m_Preload = 5;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		[ShowIf("IsObjectsNull", true)]
		[HideIf("IsListNull", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		private int m_Capacity = -1;

		[SerializeField]
		[Variable(typeof(Vector3), null, false)]
		[ShowIf("m_IsPro", true)]
		[ShowIf("IsObjectsNull", true)]
		[HideIf("IsListNull", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		private IVariable m_Position;

		[SerializeField]
		[Variable(typeof(Vector3), null, false)]
		[ShowIf("m_IsPro", true)]
		[ShowIf("IsObjectsNull", true)]
		[HideIf("IsListNull", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		private IVariable m_Rotation;

		[HideInInspector]
		public GameObject rootGameObject;

		private GameObject m_CreatedGameObject;

		private Effect m_Effect;

		private List<string> m_JsonKeys;

		public int count => (m_ObjectNames != null) ? m_ObjectNames.Count : 0;

		public void SetIndex(int index)
		{
			m_IsPro = true;
			m_Index.result = index;
		}

		public override void Enter()
		{
			GameObject gameObject = m_List as GameObject;
			if ((bool)gameObject)
			{
				m_Effect = Singleton<EffectManager>.instance.Preload(gameObject, m_Preload, m_Capacity);
			}
			else
			{
				OnListChanged();
			}
		}

		public override void Execute()
		{
			if (m_Effect != null)
			{
				GameObject gameObject = m_Effect.CreateInstance();
				if (m_IsPro)
				{
					gameObject.transform.position = m_Position.GetResult<Vector3>();
					gameObject.transform.eulerAngles = m_Rotation.GetResult<Vector3>();
				}
				return;
			}
			if ((bool)m_CreatedGameObject)
			{
				Object.Destroy(m_CreatedGameObject);
			}
			int num = (!m_IsPro) ? rootGameObject.transform.GetSiblingIndex() : m_Index.GetResult<int>();
			if (num >= m_ObjectNames.Count)
			{
				return;
			}
			string text = m_ObjectNames[num];
			Object @object = Singleton<AssetBundleManager>.instance.LoadFromName(text);
			if (@object is GameObject)
			{
				m_CreatedGameObject = Object.Instantiate(@object as GameObject, m_GameObject.transform);
				return;
			}
			if (@object is Texture2D)
			{
				Image component = m_GameObject.GetComponent<Image>();
				if ((bool)component)
				{
					Texture2D texture = @object as Texture2D;
					component.sprite = GameUtils.CreateSpriteFromTexture(texture);
				}
				return;
			}
			if (@object is AudioClip)
			{
				AudioClip clip = @object as AudioClip;
				Singleton<AudioManager>.instance.PlayBGM(clip);
				return;
			}
			Text component2 = m_GameObject.GetComponent<Text>();
			if ((bool)component2)
			{
				component2.text = text;
			}
			TextMeshPro component3 = m_GameObject.GetComponent<TextMeshPro>();
			if ((bool)component3)
			{
				component3.text = text;
			}
			TextMeshProUGUI component4 = m_GameObject.GetComponent<TextMeshProUGUI>();
			if ((bool)component4)
			{
				component4.text = text;
			}
			InputField component5 = m_GameObject.GetComponent<InputField>();
			if ((bool)component5)
			{
				component5.text = text;
			}
		}

		public override void Pause()
		{
			if (m_Effect != null)
			{
				m_Effect.Pause();
			}
		}

		public override void Resume()
		{
			if (m_Effect != null)
			{
				m_Effect.Resume();
			}
		}

		public override void Exit()
		{
			if (m_Effect != null)
			{
				Singleton<EffectManager>.instance.Unload(m_Effect);
			}
		}

		public void OnListChanged()
		{
			if (m_List == null || m_List is GameObject)
			{
				m_ObjectNames = null;
				m_JsonKeys = null;
				return;
			}
			List<string> assetPathsInFolder = SingletonScriptableObject<AssetBundleConfigManager>.instance.GetAssetPathsInFolder(m_Path);
			if (assetPathsInFolder != null)
			{
				m_ObjectNames = assetPathsInFolder.Select(Path.GetFileNameWithoutExtension);
			}
			else
			{
				m_ObjectNames = null;
			}
			if (m_ObjectNames != null && m_ObjectNames.Count != 0)
			{
				m_JsonKeys = null;
				return;
			}
			switch (Path.GetExtension(m_Path))
			{
			case ".xls":
			case ".xlsx":
			case ".json":
			{
				TextAsset textAsset = m_List as TextAsset;
				JArray jArray;
				if (textAsset != null)
				{
					jArray = JsonUtils.ToArray(textAsset.text);
				}
				else
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(m_List.name);
					jArray = Singleton<ConfigManager>.instance[fileNameWithoutExtension];
				}
				if (jArray != null)
				{
					m_JsonKeys = JsonUtils.Paths(jArray[0]);
					m_ObjectNames = new List<string>();
					OnJsonKeyChanged(m_JsonKey);
				}
				break;
			}
			default:
				m_JsonKeys = null;
				break;
			}
		}

		private void OnJsonKeyChanged(string value)
		{
			if (!string.IsNullOrEmpty(value) && !(m_List is GameObject))
			{
				m_ObjectNames.Clear();
				TextAsset textAsset = m_List as TextAsset;
				JArray jArray = (!textAsset) ? Singleton<ConfigManager>.instance[Path.GetFileNameWithoutExtension(m_Path)] : JsonUtils.ToArray(textAsset.text);
				m_ObjectNames = new List<string>(new string[jArray.Count]);
				for (int i = 0; i < jArray.Count; i++)
				{
					m_ObjectNames[i] = (string)jArray[i][value];
				}
			}
		}
	}
}
