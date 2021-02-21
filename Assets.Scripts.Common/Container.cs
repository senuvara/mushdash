using Assets.Scripts.PeroTools.Commons;
using UnityEngine;

namespace Assets.Scripts.Common
{
	public class Container : MonoBehaviour
	{
		[SerializeField]
		private Component[] components;

		public Component this[string key] => components.Find((Component c) => c.name == key);
	}
}
