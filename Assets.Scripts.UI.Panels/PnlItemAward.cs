using Assets.Scripts.GameCore.Managers;
using Assets.Scripts.PeroTools.Commons;
using Assets.Scripts.PeroTools.Managers;
using Assets.Scripts.PeroTools.Nice.Datas;
using Assets.Scripts.PeroTools.Nice.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Panels
{
	public class PnlItemAward : MonoBehaviour
	{
		public Transform chipGrid;

		private void OnEnable()
		{
			List<IData> result = Singleton<DataManager>.instance["Account"]["Items"].GetResult<List<IData>>();
			int result2 = Singleton<DataManager>.instance["Account"]["Level"].GetResult<int>();
			int num = result.Sum((IData i) => (i["free"].GetResult<bool>() && !i["hide"].GetResult<bool>()) ? i["count"].GetResult<int>() : 0);
			int num2 = (result2 - 1) * 2;
			List<IData> rewardItems = new List<IData>();
			if (num2 > num)
			{
				int num3 = Singleton<ItemManager>.instance.totalLoadingCount * 5 + Singleton<ItemManager>.instance.totalCharacterCount * 8 + Singleton<ItemManager>.instance.totalElfinCount * 8;
				int a = num2 - num;
				int b = num3 - num;
				int num4 = Mathf.Min(a, b);
				if (num4 > 0)
				{
					Singleton<ItemManager>.instance.Reward(num4);
					rewardItems = Singleton<DataManager>.instance["Account"]["RewardItems"].GetResult<List<IData>>();
					rewardItems.For(delegate(IData item)
					{
						string type = item["type"].GetResult<string>();
						int index = item["index"].GetResult<int>();
						int num5 = rewardItems.Count((IData t) => t["type"].GetResult<string>() == type && t["index"].GetResult<int>() == index);
						if (num5 > 0)
						{
							item["count"].SetResult(item["count"].GetResult<int>() + num5);
						}
						rewardItems.RemoveAll((IData t) => t["type"].GetResult<string>() == type && t["index"].GetResult<int>() == index && t != item);
					});
				}
			}
			if (Singleton<DataManager>.instance["Account"]["ShowPnlItemAward"].GetResult<bool>() || rewardItems.Count != 0)
			{
				Singleton<DataManager>.instance["Account"]["ShowPnlItemAward"].SetResult(false);
				Singleton<DataManager>.instance.Save();
				chipGrid.gameObject.SetActive(true);
			}
			else
			{
				base.gameObject.SetActive(false);
			}
		}

		private void OnDisable()
		{
			if (!Singleton<UIManager>.instance["PnlAwardFly"].activeSelf)
			{
				Singleton<EventManager>.instance.Invoke("UI/EnableTouch");
			}
			chipGrid.gameObject.SetActive(false);
			if (Singleton<DataManager>.instance["Account"]["CollectionTip"].GetResult<bool>())
			{
				Singleton<EventManager>.instance.Invoke("UI/OnCollectionsTip");
			}
			if (Singleton<DataManager>.instance["Account"]["PurchaseTip"].GetResult<bool>() && Singleton<DataManager>.instance["IAP"]["unlockall_0"].GetResult<bool>())
			{
				Singleton<EventManager>.instance.Invoke("UI/OnPurchasePlanSucceedTips");
			}
		}
	}
}
