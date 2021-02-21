using Spine;
using UnityEngine;

namespace GameLogic
{
	public class DoNothing
	{
		public int idx;

		public GameObject gameObject;

		public virtual void Init()
		{
		}

		public void SetIdx(int idx)
		{
			this.idx = idx;
		}

		public void SetGameObject(GameObject obj)
		{
			gameObject = obj;
		}

		public virtual void Do(TrackEntry entry)
		{
		}
	}
}
