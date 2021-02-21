using UnityEngine;

namespace Assets.Scripts.PeroTools.AssetBundles
{
	public class AssetBundleLoadAssetOperationSimulation<T> : AssetBundleLoadAssetOperation<T> where T : Object
	{
		private readonly T m_SimulatedObject;

		public AssetBundleLoadAssetOperationSimulation(T simulatedObject)
		{
			m_SimulatedObject = simulatedObject;
		}

		public override T GetAsset()
		{
			return m_SimulatedObject;
		}

		public override bool Update()
		{
			return false;
		}

		public override bool IsDone()
		{
			return true;
		}

		public override bool IsError()
		{
			return false;
		}
	}
}
