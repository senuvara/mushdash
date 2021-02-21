using Assets.Scripts.PeroTools.Nice.Attributes;
using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Actions
{
	public class ToggleGameObjectActivation : Action
	{
		[SerializeField]
		[Variable(typeof(GameObject), null, false)]
		[ShowIf("HasToggle", true)]
		private IVariable m_GameObject;

		[SerializeField]
		[HideInInspector]
		private Toggle m_Toggle;

		public override void Enter()
		{
			if ((bool)m_Toggle)
			{
				OnValueChanged(m_Toggle.isOn);
				m_Toggle.onValueChanged.AddListener(OnValueChanged);
			}
		}

		public override void Exit()
		{
			if ((bool)m_Toggle)
			{
				m_Toggle.onValueChanged.RemoveListener(OnValueChanged);
			}
		}

		private void OnValueChanged(bool isOn)
		{
			GameObject result = m_GameObject.GetResult<GameObject>();
			result.SetActive(isOn);
		}
	}
}
