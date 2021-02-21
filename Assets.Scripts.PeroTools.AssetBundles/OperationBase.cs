using UnityEngine;

namespace Assets.Scripts.PeroTools.AssetBundles
{
	public abstract class OperationBase : CustomYieldInstruction
	{
		public override bool keepWaiting => !IsDone();

		public abstract bool IsDone();
	}
}
