using Assets.Scripts.PeroTools.Commons;
using Spine;

namespace GameLogic
{
	public class DestorySelf : DoNothing
	{
		public override void Do(TrackEntry entry)
		{
			SpineMountController component = gameObject.GetComponent<SpineMountController>();
			if (component != null)
			{
				component.DestoryDynamicObjects();
			}
			string key = gameObject.name.Split('(')[0];
			if (SingletonMonoBehaviour<SceneObjectController>.instance != null && SingletonMonoBehaviour<SceneObjectController>.instance.SceneObjectPool != null && SingletonMonoBehaviour<SceneObjectController>.instance.SceneObjectPool.ContainsKey(key))
			{
				SingletonMonoBehaviour<SceneObjectController>.instance.SceneObjectPool[key] = null;
			}
			gameObject.SetActive(false);
		}
	}
}
