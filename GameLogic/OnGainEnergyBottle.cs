using Assets.Scripts.PeroTools.Commons;
using Spine;
using UnityEngine;

namespace GameLogic
{
	public class OnGainEnergyBottle : DoNothing
	{
		private static GameObject recoveryEffect;

		private void Start()
		{
		}

		private void PlayRecoveryEffect()
		{
			recoveryEffect.SetActive(true);
			SpineActionController.Play("in", recoveryEffect);
		}

		public override void Do(TrackEntry entry)
		{
			PlayRecoveryEffect();
			SpineMountController component = gameObject.GetComponent<SpineMountController>();
			if (component != null)
			{
				component.DestoryDynamicObjects();
			}
			gameObject.SetActive(false);
			string text = gameObject.name.Remove(gameObject.name.Length - 7);
			Debug.Log("name to be destroy is: " + text);
			SingletonMonoBehaviour<SceneObjectController>.instance.SceneObjectPool[text] = null;
			gameObject.SetActive(false);
		}
	}
}
