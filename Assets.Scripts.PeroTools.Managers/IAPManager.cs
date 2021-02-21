using Assets.Scripts.PeroTools.Commons;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.PeroTools.Managers
{
	public class IAPManager : SingletonMonoBehaviour<IAPManager>
	{
		private const char splitChar = '-';

		public bool isInited
		{
			get;
			private set;
		}

		public Product[] products
		{
			get;
			private set;
		}

		public bool isPurchaseAvaible => false;

		public event Action<Product[]> onInitSucceed = delegate
		{
		};

		public event Action<TransactionResult> onTransactionCompleted = delegate
		{
		};

		public event Action onRestoreCompleted = delegate
		{
		};

		public event Action<string[]> onReceiptCompleted = delegate
		{
		};

		private void Init()
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}

		public void InitIAP(string[] productIds)
		{
		}

		public void Purchase(string productId)
		{
		}

		public void RestoreAll()
		{
		}

		private void OnInitSucceed(string info)
		{
			Debug.Log($"[IAPManager]: Inited succeed with info:\n{info}");
			isInited = true;
			List<Product> list = new List<Product>();
			string[] array = info.Split('\n');
			string[] array2 = array;
			foreach (string text in array2)
			{
				if (string.IsNullOrEmpty(text))
				{
					break;
				}
				string[] array3 = text.Split('-');
				string id = array3[0];
				string title = array3[1];
				string s = array3[2];
				string currencySymbol = array3[3];
				string localizedPrice = array3[4];
				Product product = new Product();
				product.isAvailable = true;
				product.id = id;
				product.title = title;
				product.price = float.Parse(s);
				product.currencySymbol = currencySymbol;
				product.localizedPrice = localizedPrice;
				Product item = product;
				list.Add(item);
			}
			products = list.ToArray();
			this.onInitSucceed(products.ToArray());
		}

		private void OnTransactionCompleted(string info)
		{
			Debug.Log($"[IAPManager]: Transaction completed with info:\n{info}");
			string[] array = info.Split('-');
			string productId = array[0];
			int num;
			switch (array[1])
			{
			case "purchased":
				num = 0;
				break;
			case "restored":
				num = 1;
				break;
			case "deferred":
				num = 2;
				break;
			default:
				num = 3;
				break;
			}
			TransactionState state = (TransactionState)num;
			string receipt = (array.Length <= 2) ? string.Empty : array[2];
			TransactionResult transactionResult = new TransactionResult();
			transactionResult.productId = productId;
			transactionResult.state = state;
			transactionResult.receipt = receipt;
			TransactionResult obj = transactionResult;
			this.onTransactionCompleted(obj);
		}

		private void OnRestoreCompleted()
		{
			Debug.Log("[IAPManager]: Restore completed!");
			this.onRestoreCompleted();
		}

		private void OnReceiptCompleted(string receiptData)
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("receipt-data", receiptData);
			Dictionary<string, object> target = dictionary;
			byte[] binaryData = Encoding.UTF8.GetBytes(JsonUtils.Serialize(target));
			WWW www = new WWW("https://buy.itunes.apple.com/verifyReceipt", binaryData);
			SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
			{
				if (www.error == null)
				{
					JObject jObject = JsonUtils.Deserialize<JObject>(www.text);
					int status = (int)jObject["status"];
					if (status == 0)
					{
						OnReceiptDataVertify(jObject);
					}
					else if (status == 21007)
					{
						www = new WWW("https://sandbox.itunes.apple.com/verifyReceipt", binaryData);
						SingletonMonoBehaviour<CoroutineManager>.instance.StartCoroutine(delegate
						{
							if (www.error == null)
							{
								Debug.Log($"[IAPManager]: receiptData {www.text}");
								jObject = JsonUtils.Deserialize<JObject>(www.text);
								status = (int)jObject["status"];
								if (status == 0)
								{
									OnReceiptDataVertify(jObject);
								}
							}
						}, () => www.isDone);
					}
				}
			}, () => www.isDone);
		}

		private void OnReceiptDataVertify(JObject jObject)
		{
			JToken jToken = jObject["receipt"];
			JToken jToken2 = jToken["in_app"];
			List<string> list = new List<string>();
			foreach (JToken item2 in (IEnumerable<JToken>)jToken2)
			{
				string item = (string)item2["product_id"];
				list.Add(item);
			}
			this.onReceiptCompleted(list.ToArray());
		}
	}
}
