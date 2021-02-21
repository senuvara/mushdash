using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Events
{
	[DisallowMultipleComponent]
	public class OnClick : Event
	{
		[SerializeField]
		[PropertyOrder(-1)]
		[Required]
		private Button m_Button;

		protected override void OnEnter()
		{
			m_Button.onClick.AddListener(base.Execute);
		}

		protected override void OnExit()
		{
			m_Button.onClick.RemoveListener(base.Execute);
		}

		protected override void OnExecute()
		{
			base.OnExecute();
		}
	}
}
