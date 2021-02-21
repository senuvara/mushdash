using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class SetPrefab : Action
	{
		[SerializeField]
		[HideInEditorMode]
		[HideInPlayMode]
		private bool m_IsPro;

		[SerializeField]
		private IVariable m_Prefab;

		[SerializeField]
		[Variable(1, null, false)]
		private IVariable m_Count;

		[SerializeField]
		[ShowIf("m_IsPro", true)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		private Transform m_Transform;

		private List<GameObject> m_GameObjects;

		public override void Execute()
		{
			if (m_GameObjects != null)
			{
				m_GameObjects.For(Object.Destroy);
			}
			SingletonMonoBehaviour<CoroutineManager>.instance.Delay(delegate
			{
				m_GameObjects = new List<GameObject>();
				GameObject @object = GameUtils.GetObject<GameObject>(m_Prefab.result);
				if ((bool)@object)
				{
					for (int i = 0; i < m_Count.GetResult<int>(); i++)
					{
						m_GameObjects.Add(Object.Instantiate(@object, m_Transform));
					}
				}
			}, 1);
		}
	}
}
