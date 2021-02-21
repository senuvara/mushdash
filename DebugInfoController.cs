using Spine.Unity;
using UnityEngine;

public class DebugInfoController : MonoBehaviour
{
	private GameObject parentObject;

	private void Start()
	{
	}

	private void InitParentObject()
	{
		BoneFollower component = base.gameObject.GetComponent<BoneFollower>();
		if (component == null)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		parentObject = component.skeletonRenderer.gameObject;
		if (parentObject == null)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
