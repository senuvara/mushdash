using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Events;
using Assets.Scripts.PeroTools.Nice.Interface;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class CreateList : Action
	{
		[SerializeField]
		[AssetsOnly]
		[LabelText("Folder/Json/Prefab")]
		[OnValueChanged("OnListChanged", false)]
		private UnityEngine.Object m_List;

		[SerializeField]
		[HideInInspector]
		private string m_Path;

		[SerializeField]
		[ShowIf("IsGameObject", true)]
		[Variable(typeof(int), "OnCountShow", true)]
		private IVariable m_Count;

		[SerializeField]
		[ShowIf("IsAutoCount", true)]
		private bool m_AutoCount;

		[SerializeField]
		[CustomValueDrawer("OnJsonKeyShow")]
		[ShowIf("HasJsonKeys", true)]
		private string m_JsonKey;

		[SerializeField]
		[HideInInspector]
		private List<GameObject> m_GameObjects;

		[SerializeField]
		[HideInInspector]
		private GameObject m_Parent;

		[SerializeField]
		[HideInInspector]
		private int m_MaxCount;

		private List<string> m_JsonKeys;

		public override void Enter()
		{
			OnListChanged();
		}

		public override void Execute()
		{
			int result = m_Count.GetResult<int>();
			if (m_GameObjects == null || m_GameObjects.Count == 0)
			{
				GameObject gameObject = m_List as GameObject;
				if (gameObject != null)
				{
					for (int i = 0; i < result; i++)
					{
						UnityEngine.Object.Instantiate(gameObject, m_Parent.transform);
					}
				}
			}
			else
			{
				for (int j = 0; j < result; j++)
				{
					UnityEngine.Object.Instantiate(m_GameObjects[j], m_Parent.transform);
				}
			}
		}

		private void OnJsonKeyChanged(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			TextAsset textAsset = m_List as TextAsset;
			JArray jArray = (!textAsset) ? Singleton<ConfigManager>.instance[Path.GetFileNameWithoutExtension(m_List.name)] : JsonUtils.ToArray(textAsset.text);
			int count = 0;
			m_GameObjects = new List<GameObject>(new GameObject[jArray.Count]);
			int num = 0;
			while (true)
			{
				if (num >= jArray.Count)
				{
					return;
				}
				JToken jToken = jArray[num];
				string text = (string)jToken[value];
				if (!SingletonScriptableObject<AssetBundleConfigManager>.instance.dict.ContainsKey(text))
				{
					break;
				}
				int index = num;
				Action<GameObject> action = delegate(GameObject gameObject)
				{
					m_GameObjects[index] = gameObject;
					if (++count == jArray.Count)
					{
						m_MaxCount = m_GameObjects.Count;
					}
				};
				if (Application.isPlaying)
				{
					action(Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(text));
				}
				else
				{
					Singleton<AssetBundleManager>.instance.LoadFromNameAsyn(text, action);
				}
				num++;
			}
			m_GameObjects.Clear();
		}

		private void OnListChanged()
		{
			if (m_List == null)
			{
				m_GameObjects = null;
				m_JsonKeys = null;
				return;
			}
			GameObject gameObject = m_List as GameObject;
			if (gameObject != null)
			{
				m_GameObjects = null;
				m_MaxCount = 0;
				m_JsonKeys = null;
				List<Assets.Scripts.PeroTools.Nice.Events.Event> allComponents = gameObject.GetAllComponents<Assets.Scripts.PeroTools.Nice.Events.Event>();
				for (int i = 0; i < allComponents.Count; i++)
				{
					List<CreateOne> playables = allComponents[i].GetPlayables<CreateOne>();
					for (int j = 0; j < playables.Count; j++)
					{
						CreateOne createOne = playables[i];
						if (createOne.rootGameObject == gameObject && createOne.count != 0)
						{
							m_MaxCount = createOne.count;
							i = allComponents.Count;
							break;
						}
					}
				}
				return;
			}
			List<string> assetPathsInFolder = SingletonScriptableObject<AssetBundleConfigManager>.instance.GetAssetPathsInFolder(m_Path);
			if (assetPathsInFolder != null)
			{
				m_GameObjects = assetPathsInFolder.Select((string p) => Singleton<AssetBundleManager>.instance.LoadFromName<GameObject>(p));
			}
			else
			{
				m_GameObjects = null;
			}
			if (m_GameObjects != null && m_GameObjects.Count != 0)
			{
				m_MaxCount = m_GameObjects.Count;
				m_JsonKeys = null;
				return;
			}
			switch (Path.GetExtension(m_Path))
			{
			case ".xls":
			case ".xlsx":
			case ".json":
			{
				TextAsset textAsset = (TextAsset)m_List;
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
					m_GameObjects = new List<GameObject>();
					OnJsonKeyChanged(m_JsonKey);
				}
				break;
			}
			default:
				m_GameObjects = null;
				m_JsonKeys = null;
				break;
			}
		}
	}
}
