using Assets.Scripts.PeroTools.Nice.Components;
using Assets.Scripts.PeroTools.PreWarm;
using UnityEngine;

public class DisableFSVPreloadOnUnSupportASTCPlatform : MonoBehaviour, IPreWarm
{
	public void PreWarm(int slice)
	{
		if (slice == 0 && !SystemInfo.SupportsTextureFormat(TextureFormat.ASTC_RGBA_4x4))
		{
			FancyScrollView component = GetComponent<FancyScrollView>();
			component.disableAutoCache = false;
			component.initOnPrewarm = false;
			component.dontInit = false;
		}
	}
}
