using Assets.Scripts.PeroTools.Commons;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.PeroTools.Nice.Components
{
	public class SetToggleGroup : MonoBehaviour
	{
		public Transform parent;

		public ToggleGroup group;

		private void Start()
		{
			Toggle[] componentsInChildren = parent.GetComponentsInChildren<Toggle>(true);
			componentsInChildren.For(delegate(Toggle toggle)
			{
				toggle.isOn = false;
				toggle.group = group;
			});
			componentsInChildren.First().isOn = true;
		}
	}
}
