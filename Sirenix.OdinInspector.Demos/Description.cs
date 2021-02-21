using UnityEngine;

namespace Sirenix.OdinInspector.Demos
{
	[HideMonoScript]
	[AddComponentMenu("Nice/Tools/Description")]
	public class Description : SerializedMonoBehaviour
	{
		[Space(10f)]
		[HideLabel]
		[Multiline(2)]
		[GUIColor(0.3f, 0.8f, 0.8f, 1f)]
		public string WideMultilineTextField = "Write something.(ฅ\u00b4ω`ฅ)";
	}
}
