using Assets.Scripts.PeroTools.Nice.Interface;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Assets.Scripts.UI.Specials
{
	public class DesElfinIfLessThanZero : SerializedMonoBehaviour
	{
		public IVariable elfinIndex;

		private void OnEnable()
		{
			if (elfinIndex.GetResult<int>() < 0 && base.transform.childCount > 0)
			{
				Object.Destroy(base.transform.GetChild(0).gameObject);
			}
		}
	}
}
