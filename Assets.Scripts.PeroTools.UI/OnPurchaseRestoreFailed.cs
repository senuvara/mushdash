using Assets.Scripts.PeroTools.Managers;
using System;
using UnityEngine.Events;

namespace Assets.Scripts.PeroTools.UI
{
	[Serializable]
	public class OnPurchaseRestoreFailed : UnityEvent<string, TransactionResult>
	{
	}
}
