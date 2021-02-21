using Assets.Scripts.PeroTools.AssetBundles;
using Assets.Scripts.PeroTools.Commons;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Controls
{
	public class LoadDLCInterludeRuntime : MonoBehaviour
	{
		private void Start()
		{
			Image component = GetComponent<Image>();
			if (component != null)
			{
				component.sprite = Singleton<AssetBundleManager>.instance.LoadFromName<Sprite>("interlude_partner");
			}
		}
	}
}
