using System.Collections.Generic;
using UnityEngine;

public class SpineSynchroObjects : MonoBehaviour
{
	[SerializeField]
	public List<GameObject> synchroObjects;

	private void Start()
	{
		SpineActionController component = base.gameObject.GetComponent<SpineActionController>();
		if (!(component == null))
		{
			component.SetSynchroObjects(synchroObjects);
		}
	}
}
